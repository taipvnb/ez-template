using System;
using System.Collections.Generic;
using com.ez.engine.json.mini;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace com.ez.engine.save.core
{
	public abstract class BaseGameLoader<TGameSave, TGameRuntime> : IGameLoader where TGameSave : IGameSave where TGameRuntime : IGameRuntime
	{
		private readonly TGameSave _gameSave;
		private readonly TGameRuntime _gameRuntime;
		private readonly IStorageProvider _storageProvider;
		private readonly Dictionary<Type, ILoadStrategy> _loadStrategies;
		private readonly Dictionary<Type, Type> _runtimeModelTypes;

		public IReadOnlyCollection<Type> Keys => _loadStrategies.Keys;

		public IReadOnlyCollection<ILoadStrategy> Strategies => _loadStrategies.Values;

		protected BaseGameLoader(TGameSave gameSave, TGameRuntime gameRuntime, IStorageProvider storageProvider)
		{
			_gameSave = gameSave;
			_gameRuntime = gameRuntime;
			_storageProvider = storageProvider;
			_loadStrategies = new Dictionary<Type, ILoadStrategy>();
			_runtimeModelTypes = new Dictionary<Type, Type>();
		}

		public void AddLoadStrategy<TSaveModel, TRuntimeModel>(ILoadStrategy<TSaveModel, TRuntimeModel> loadStrategy)
			where TSaveModel : ISaveModel
			where TRuntimeModel : IRuntimeModel
		{
			if (_loadStrategies.ContainsKey(typeof(TSaveModel)))
			{
				Debug.LogError($"Load strategy has already been added {loadStrategy.GetType().Name}");
				return;
			}

			_loadStrategies.Add(typeof(TSaveModel), loadStrategy);
			_runtimeModelTypes.Add(typeof(TSaveModel), typeof(TRuntimeModel));
		}

		public void AddLoadStrategy(ILoadStrategy loadStrategy, Type saveModelType, Type runtimeModelType)
		{
			if (_loadStrategies.ContainsKey(saveModelType))
			{
				Debug.LogError($"Load strategy for {saveModelType.Name} already exists");
				return;
			}

			_loadStrategies.Add(saveModelType, loadStrategy);
			_runtimeModelTypes.Add(saveModelType, runtimeModelType ?? typeof(IRuntimeModel));
		}

		public bool Load(Type saveModelType)
		{
			var fileName = saveModelType.Name;
			try
			{
				if (_loadStrategies.TryGetValue(saveModelType, out var loadStrategy))
				{
					bool firstLoad;
					ISaveModel saveModel;

					if (_storageProvider.Exists(fileName))
					{
						firstLoad = false;
						saveModel = (ISaveModel)_storageProvider.Load(fileName, saveModelType);
					}
					else
					{
						firstLoad = true;
						saveModel = (ISaveModel)Activator.CreateInstance(saveModelType);
					}

					var runtimeModel = _gameRuntime.GetModel(_runtimeModelTypes[saveModelType]);
					loadStrategy.Load(saveModel, runtimeModel, firstLoad);
					_gameSave.TrySetModel(saveModel);
					return true;
				}
				else
				{
					Debug.LogError($"Save strategy for {saveModelType.Name} not found");
					return false;
				}
			}
			catch (Exception e)
			{
				Debug.LogError($"Load save file {fileName} failed\n{e.Message}\n{e.StackTrace}");
				return false;
			}
		}

		public bool Load<TSaveModel>() where TSaveModel : ISaveModel, new()
		{
			return Load(typeof(TSaveModel));
		}

		public async UniTask<bool> LoadAsync(Type saveModelType)
		{
			var fileName = saveModelType.Name;
			try
			{
				if (_loadStrategies.TryGetValue(saveModelType, out var loadStrategy))
				{
					bool firstLoad;
					ISaveModel saveModel;

					if (_storageProvider.Exists(fileName))
					{
						firstLoad = false;
						saveModel = (ISaveModel)await _storageProvider.LoadAsync(fileName, saveModelType);
					}
					else
					{
						firstLoad = true;
						saveModel = (ISaveModel)Activator.CreateInstance(saveModelType);
					}

					var runtimeModel = _gameRuntime.GetModel(_runtimeModelTypes[saveModelType]);
					loadStrategy.Load(saveModel, runtimeModel, firstLoad);
					_gameSave.TrySetModel(saveModel);
					return true;
				}
				else
				{
					Debug.LogError($"Load strategy for {saveModelType.Name} not found");
					return false;
				}
			}
			catch (Exception e)
			{
				Debug.LogError($"Load save file {fileName} failed: {e.Message}");
				return false;
			}
		}

		public async UniTask<bool> LoadAsync<TSaveModel>() where TSaveModel : ISaveModel, new()
		{
			return await LoadAsync(typeof(TSaveModel));
		}

		public void LoadAll()
		{
			foreach (var key in _loadStrategies.Keys)
			{
				Load(key);
			}
		}

		public async UniTask LoadAllAsync()
		{
			foreach (var key in _loadStrategies.Keys)
			{
				await LoadAsync(key);
			}
		}

		public bool LoadFromRawData(string rawData)
		{
			if (string.IsNullOrEmpty(rawData))
			{
				Debug.LogError("Raw data is null");
				return false;
			}

			if (MiniJson.JsonDecode(rawData) is Dictionary<string, object> data)
			{
				foreach ((var key, var value) in data)
				{
					foreach ((var type, var strategy) in _loadStrategies)
					{
						if (type.Name.Equals(key))
						{
							var runtimeModel = _gameRuntime.GetModel(_runtimeModelTypes[type]);
							var saveModel = (ISaveModel)JsonConvert.DeserializeObject((string)value, type);
							strategy.Load(saveModel, runtimeModel, false);
						}
					}
				}

				return true;
			}

			return false;
		}

		public void Clear()
		{
			_loadStrategies.Clear();
			_runtimeModelTypes.Clear();
		}
	}
}
