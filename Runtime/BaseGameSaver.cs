using System;
using System.Collections.Generic;
using com.ez.engine.json.mini;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace com.ez.engine.save.core
{
	public abstract class BaseGameSaver<TGameSave, TGameRuntime> : IGameSaver where TGameSave : IGameSave where TGameRuntime : IGameRuntime
	{
		private readonly TGameSave _gameSave;
		private readonly TGameRuntime _gameRuntime;
		private readonly IStorageProvider _storageProvider;
		private readonly Dictionary<Type, ISaveStrategy> _saveStrategies;
		private readonly Dictionary<Type, Type> _runtimeModelTypes;
		private readonly SaveBuffer _saveBuffer;

		public IReadOnlyCollection<Type> Keys => _saveStrategies.Keys;

		public IReadOnlyCollection<ISaveStrategy> Strategies => _saveStrategies.Values;

		protected BaseGameSaver(TGameSave gameSave, TGameRuntime gameRuntime, IStorageProvider storageProvider)
		{
			_gameSave = gameSave;
			_gameRuntime = gameRuntime;
			_storageProvider = storageProvider;
			_saveStrategies = new Dictionary<Type, ISaveStrategy>();
			_runtimeModelTypes = new Dictionary<Type, Type>();
			_saveBuffer = new SaveBuffer(_storageProvider);
		}

		public void AddSaveStrategy<TSaveModel, TRuntimeModel>(ISaveStrategy<TSaveModel, TRuntimeModel> saveStrategy)
			where TSaveModel : ISaveModel
			where TRuntimeModel : IRuntimeModel
		{
			if (_saveStrategies.ContainsKey(typeof(TSaveModel)))
			{
				Debug.LogError($"Save strategy has already been added {saveStrategy.GetType().Name}");
				return;
			}

			_runtimeModelTypes.Add(typeof(TSaveModel), typeof(TRuntimeModel));
			_saveStrategies.Add(typeof(TSaveModel), saveStrategy);
		}

		public void AddSaveStrategy(ISaveStrategy saveStrategy, Type saveModelType, Type runtimeModelType)
		{
			if (_saveStrategies.ContainsKey(saveModelType))
			{
				Debug.LogError($"Save strategy for {saveModelType.Name} already exists");
				return;
			}

			_saveStrategies.Add(saveModelType, saveStrategy);
			_runtimeModelTypes.Add(saveModelType, runtimeModelType ?? typeof(IRuntimeModel));
		}

		public bool TryGetSaveStrategy(Type saveModelType, out ISaveStrategy saveStrategy)
		{
			return _saveStrategies.TryGetValue(saveModelType, out saveStrategy);
		}

		public bool Save(Type saveModelType)
		{
			var fileName = saveModelType.Name;
			var backupFilename = fileName + "-backup";
			var hasPrevSave = _storageProvider.Exists(fileName);

			try
			{
				if (_saveStrategies.TryGetValue(saveModelType, out var saveStrategy))
				{
					if (hasPrevSave)
					{
						if (_storageProvider.Exists(backupFilename))
						{
							_storageProvider.Delete(backupFilename);
						}

						_storageProvider.Copy(fileName, backupFilename);
					}

					var runtimeModel = _gameRuntime.GetModel(_runtimeModelTypes[saveModelType]);
					var saveModel = saveStrategy.Save(runtimeModel);
					saveModel.Version = runtimeModel.Version;

					if (_saveBuffer.TrySave(fileName, saveModel, runtimeModel.SaveThreshold))
					{
						_saveBuffer.Flush();
						_gameSave.TrySetModel(saveModel);
					}

					if (_storageProvider.Exists(backupFilename))
					{
						_storageProvider.Delete(backupFilename);
					}

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
				Debug.LogError($"Save file {fileName} failed\n{e.Message}\n{e.StackTrace}");

				_storageProvider.Delete(fileName);

				if (_storageProvider.Exists(backupFilename))
				{
					_storageProvider.Copy(backupFilename, fileName);
					_storageProvider.Delete(backupFilename);
				}

				return false;
			}
		}

		public bool Save<TSaveModel>() where TSaveModel : class, ISaveModel
		{
			return Save(typeof(TSaveModel));
		}

		public async UniTask<bool> SaveAsync(Type saveModelType)
		{
			var fileName = saveModelType.Name;
			var backupFilename = fileName + "-backup";
			var hasPrevSave = _storageProvider.Exists(fileName);

			try
			{
				if (_saveStrategies.TryGetValue(saveModelType, out var saveStrategy))
				{
					if (hasPrevSave)
					{
						if (_storageProvider.Exists(backupFilename))
						{
							_storageProvider.Delete(backupFilename);
						}

						_storageProvider.Copy(fileName, backupFilename);
					}

					var runtimeModel = _gameRuntime.GetModel(_runtimeModelTypes[saveModelType]);
					var saveModel = saveStrategy.Save(runtimeModel);
					saveModel.Version = runtimeModel.Version;

					if (_saveBuffer.TrySave(fileName, saveModel, runtimeModel.SaveThreshold))
					{
						await _saveBuffer.FlushAsync();
						_gameSave.TrySetModel(saveModel);
					}

					if (_storageProvider.Exists(backupFilename))
					{
						_storageProvider.Delete(backupFilename);
					}

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
				Debug.LogError($"Save file {fileName} failed\n{e.Message}\n{e.StackTrace}");

				_storageProvider.Delete(fileName);

				if (_storageProvider.Exists(backupFilename))
				{
					_storageProvider.Copy(backupFilename, fileName);
					_storageProvider.Delete(backupFilename);
				}

				return false;
			}
		}

		public async UniTask<bool> SaveAsync<TSaveModel>() where TSaveModel : class, ISaveModel
		{
			return await SaveAsync(typeof(TSaveModel));
		}

		public void SaveAll()
		{
			foreach (var saveStrategy in _saveStrategies)
			{
				Save(saveStrategy.Key);
			}

			_saveBuffer.Flush();
		}

		public async UniTask SaveAllAsync()
		{
			foreach (var saveStrategy in _saveStrategies)
			{
				await SaveAsync(saveStrategy.Key);
			}

			await _saveBuffer.FlushAsync();
		}

		public string GetRawData()
		{
			var data = new Dictionary<string, object>();
			foreach ((var saveModelType, var runtimeModelType) in _runtimeModelTypes)
			{
				if (_saveStrategies.TryGetValue(saveModelType, out var saveStrategy))
				{
					var runtimeModel = _gameRuntime.GetModel(runtimeModelType);
					var saveModel = saveStrategy.Save(runtimeModel);
					data.Add(saveModelType.Name, JsonUtility.ToJson(saveModel));
				}
			}

			return MiniJson.JsonEncode(data);
		}

		public void Clear()
		{
			_saveStrategies.Clear();
			_runtimeModelTypes.Clear();
		}
	}
}
