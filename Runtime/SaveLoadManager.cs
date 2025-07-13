using System;
using System.Linq;
using com.ez.engine.core.di;
using com.ez.engine.foundation;
using com.ez.engine.save.core;
using com.ez.engine.save.file;
using com.ez.engine.save.playerprefs;
using com.ez.engine.utils.type_references;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;

namespace com.ez.engine.manager.save_load
{
	public sealed class SaveLoadManager : MonoBehaviour, ISaveLoadManager
	{
		[SerializeField] [TabGroup("General", TextColor = "green")] private GameSave _gameSave;
		[SerializeField] [TabGroup("General", TextColor = "green")] private GameRuntime _gameRuntime;

		// Encryption key should be at least 32 characters long in base64 format, including uppercase, lowercase and numbers (e.g., "YOT3z7WiQygt5jD4cUTiLIpA6SskTU6D").
		[SerializeField] [TabGroup("Settings", TextColor = "cyan")] private string _encryptionKey = "YOT3z7WiQygt5jD4cUTiLIpA6SskTU6D";
		[SerializeField] [TabGroup("Settings", TextColor = "cyan")] private string _savePath = "Data";
		[SerializeField] [TabGroup("Settings", TextColor = "cyan")] private bool _useEncryption = true;
		[SerializeField] [TabGroup("Settings", TextColor = "cyan")] private bool _saveToFile = true;
		[SerializeField] [TabGroup("Settings", TextColor = "cyan")] private bool _autoLoadOnInitialize = true;

		[SerializeField] [TabGroup("Settings", TextColor = "cyan")] [Inherits(typeof(BaseSaveModel), ShortName = true)]
		private TypeReference[] _saveModels;

		[SerializeField] [TabGroup("Settings", TextColor = "cyan")] [Inherits(typeof(BaseRuntimeModel), ShortName = true)]
		private TypeReference[] _runtimeModels;

		[SerializeField] [TabGroup("Settings", TextColor = "cyan")] [Inherits(typeof(BaseSaveStrategy<,>), ShortName = true)]
		private TypeReference[] _saverStrategies;

		[SerializeField] [TabGroup("Settings", TextColor = "cyan")] [Inherits(typeof(BaseLoadStrategy<,>), ShortName = true)]
		private TypeReference[] _loaderStrategies;

		[Inject] private readonly IInjector _injector;
		[Inject] private readonly ILogger _logger;

		public int Priority => 0;
		public bool IsInitialized { get; private set; }

		public GameSaver GameSaver { get; private set; }

		public GameLoader GameLoader { get; private set; }

		private ISerializationProvider _serializationProvider;
		private AesEncryptionDataTransform _encryptionDataTransform;
		private DataTransformSerializationProvider _transformSerializationProvider;
		private IStorageProvider _storageProvider;

		public UniTask Initialize()
		{
			_serializationProvider = new NewtonsoftJsonSerializationProvider(new JsonSerializerSettings());
			_encryptionDataTransform = new AesEncryptionDataTransform(_encryptionKey);
			_transformSerializationProvider = new DataTransformSerializationProvider(_serializationProvider, _encryptionDataTransform);

			if (_saveToFile)
			{
				_storageProvider = new FileStorage(_useEncryption ? _transformSerializationProvider : _serializationProvider, _savePath);
			}
			else
			{
				_storageProvider = new PlayerPrefsStorage(_useEncryption ? _transformSerializationProvider : _serializationProvider);
			}

			_gameSave = new GameSave();
			foreach (var saveModel in _saveModels)
			{
				if (saveModel.Type == null)
				{
					continue;
				}

				var saveInstance = (BaseSaveModel)Activator.CreateInstance(saveModel.Type);
				if (saveInstance == null)
				{
					Debug.LogError($"Failed to create instance of {saveModel.Type.Name}");
					continue;
				}

				_injector.Resolve(saveInstance);
				_gameSave.AddModel(saveInstance);
			}

			_gameRuntime = new GameRuntime();
			foreach (var runtimeModel in _runtimeModels)
			{
				if (runtimeModel.Type == null)
				{
					continue;
				}

				var runtimeInstance = (BaseRuntimeModel)Activator.CreateInstance(runtimeModel.Type);
				if (runtimeInstance == null)
				{
					Debug.LogError($"Failed to create instance of {runtimeModel.Type.Name}");
					continue;
				}

				_injector.Resolve(runtimeInstance);
				_gameRuntime.AddModel(runtimeInstance);
			}

			GameSaver = new GameSaver(_gameSave, _gameRuntime, _storageProvider);
			foreach (var saverStrategy in _saverStrategies)
			{
				if (saverStrategy.Type == null)
				{
					continue;
				}

				try
				{
					var saveStrategyInstance = Activator.CreateInstance(saverStrategy.Type);
					switch (saveStrategyInstance)
					{
						case null:
							Debug.LogWarning($"Failed to create instance of {saverStrategy.Type.FullName}");
							continue;
						case ISaveStrategy saveStrategy:
							var arguments = saveStrategy.GetType().GetInterfaces()
								.Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISaveStrategy<,>))
								.Select(i => i.GetGenericArguments())
								.FirstOrDefault();

							if (arguments == null || arguments.Length != 2)
							{
								Debug.LogError($"Cannot determine TSaveModel and TRuntimeModel for {saveStrategy.GetType().Name}");
								continue;
							}

							_injector.Resolve(saveStrategy);
							var saveModelType = arguments[0];
							var runtimeModelType = arguments[1];
							GameSaver.AddSaveStrategy(saveStrategy, saveModelType, runtimeModelType);
							break;
						default:
							Debug.LogError($"Cannot cast {saveStrategyInstance.GetType().FullName} to ISaveStrategy");
							break;
					}
				}
				catch (Exception e)
				{
					Debug.LogError($"Error creating save strategy {saverStrategy.Type.Name}\n{e.Message}\n{e.StackTrace}");
				}
			}

			GameLoader = new GameLoader(_gameSave, _gameRuntime, _storageProvider);
			foreach (var loaderStrategy in _loaderStrategies)
			{
				if (loaderStrategy.Type == null)
				{
					continue;
				}

				try
				{
					var loadStrategyInstance = Activator.CreateInstance(loaderStrategy.Type);
					switch (loadStrategyInstance)
					{
						case null:
							Debug.LogError($"Failed to create instance of {loaderStrategy.Type.FullName}");
							continue;
						case ILoadStrategy loadStrategy:
							var arguments = loadStrategy.GetType().GetInterfaces()
								.Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ILoadStrategy<,>))
								.Select(i => i.GetGenericArguments())
								.FirstOrDefault();
							if (arguments == null || arguments.Length != 2)
							{
								Debug.LogError($"Cannot determine TSaveModel and TRuntimeModel for {loadStrategy.GetType().Name}");
								continue;
							}

							_injector.Resolve(loadStrategy);
							foreach (var converter in loadStrategy.Converters)
							{
								if (converter == null)
								{
									continue;
								}

								_injector.Resolve(converter);
							}

							var saveModelType = arguments[0];
							var runtimeModelType = arguments[1];
							GameLoader.AddLoadStrategy(loadStrategy, saveModelType, runtimeModelType);
							break;
						default:
							Debug.LogError($"Cannot cast {loadStrategyInstance.GetType().FullName} to ILoadStrategy");
							break;
					}
				}
				catch (Exception e)
				{
					Debug.LogError($"Error creating load strategy {loaderStrategy.Type.Name}\n{e.Message}\n{e.StackTrace}");
				}
			}

			if (_autoLoadOnInitialize)
			{
				GameLoader.LoadAll();
			}

			IsInitialized = true;
			return UniTask.CompletedTask;
		}

		public TRuntimeModel GetRuntimeModel<TRuntimeModel>() where TRuntimeModel : IRuntimeModel
		{
			return _gameRuntime.GetModel<TRuntimeModel>();
		}

		public bool TryGetRuntimeModel<TRuntimeModel>(out TRuntimeModel runtimeModel) where TRuntimeModel : IRuntimeModel
		{
			return _gameRuntime.TryGetModel(out runtimeModel);
		}

		public TSaveModel GetSaveModel<TSaveModel>() where TSaveModel : class, ISaveModel
		{
			return _gameSave.GetModel<TSaveModel>();
		}

		public bool TryGetSaveModel<TSaveModel>(out TSaveModel saveModel) where TSaveModel : class, ISaveModel
		{
			return _gameSave.TryGetModel(out saveModel);
		}

		public bool Save<TSaveModel>() where TSaveModel : class, ISaveModel
		{
			return GameSaver.Save<TSaveModel>();
		}

		public UniTask<bool> SaveAsync<TSaveModel>() where TSaveModel : class, ISaveModel
		{
			return GameSaver.SaveAsync<TSaveModel>();
		}

		public bool Load<TDataModel>() where TDataModel : ISaveModel, new()
		{
			return GameLoader.Load<TDataModel>();
		}

		public UniTask<bool> LoadAsync<TDataModel>() where TDataModel : ISaveModel, new()
		{
			return GameLoader.LoadAsync<TDataModel>();
		}

		[Button, GUIColor("cyan")]
		public void SaveAll()
		{
			GameSaver.SaveAll();
		}

		public void Dispose()
		{
			_saveModels?.Clear();
			_runtimeModels?.Clear();
			GameSaver?.Clear();
			GameLoader?.Clear();
		}
	}
}
