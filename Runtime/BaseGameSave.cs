using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace com.ez.engine.save.core
{
	public abstract class BaseGameSave : IGameSave
	{
		[ShowInInspector, InlineProperty] private readonly Dictionary<string, ISaveModel> _models = new();

		public IReadOnlyCollection<string> Keys => _models.Keys;

		public IReadOnlyCollection<ISaveModel> Models => _models.Values;

		public void AddModel<TSaveModel>() where TSaveModel : ISaveModel, new()
		{
			var type = typeof(TSaveModel);
			if (_models.ContainsKey(type.Name))
			{
				Debug.LogError($"Save model has already been added {type.Name}");
				return;
			}

			_models.Add(type.Name, new TSaveModel());
		}

		public void AddModel(ISaveModel saveModel)
		{
			var type = saveModel.GetType();
			if (_models.ContainsKey(type.Name))
			{
				Debug.LogError($"Save model has already been added {type.Name}");
				return;
			}

			_models.Add(type.Name, saveModel);
		}

		public void RemoveModel<TSaveModel>() where TSaveModel : ISaveModel
		{
			var type = typeof(TSaveModel);
			if (_models.ContainsKey(type.Name))
			{
				_models.Remove(type.Name);
			}
		}

		public void RemoveModel(Type saveModelType)
		{
			if (_models.ContainsKey(saveModelType.Name))
			{
				_models.Remove(saveModelType.Name);
			}
		}

		public void RemoveModel(string saveModelName)
		{
			if (_models.ContainsKey(saveModelName))
			{
				_models.Remove(saveModelName);
			}
		}

		public ISaveModel GetModel(Type saveModelType)
		{
			return _models.GetValueOrDefault(saveModelType.Name);
		}

		public TSaveModel GetModel<TSaveModel>() where TSaveModel : ISaveModel
		{
			var type = typeof(TSaveModel);
			return _models.TryGetValue(type.Name, out var model) ? (TSaveModel)model : default;
		}

		public bool TryGetModel(Type saveModelType, out ISaveModel saveModel)
		{
			var result = _models.TryGetValue(saveModelType.Name, out var value);
			saveModel = result ? value : null;
			return result;
		}

		public bool TryGetModel<TSaveModel>(out TSaveModel saveModel) where TSaveModel : ISaveModel
		{
			var type = typeof(TSaveModel);
			var result = _models.TryGetValue(type.Name, out var value);
			saveModel = result ? (TSaveModel)value : default;
			return result;
		}

		public bool TrySetModel<TSaveModel>(TSaveModel saveModel) where TSaveModel : ISaveModel
		{
			var type = saveModel.GetType();
			if (_models.ContainsKey(type.Name))
			{
				_models[type.Name] = saveModel;
				return true;
			}
			else
			{
				Debug.LogError($"Save model {type.Name} not found");
				return false;
			}
		}

		public void Clear()
		{
			_models.Clear();
		}
	}
}
