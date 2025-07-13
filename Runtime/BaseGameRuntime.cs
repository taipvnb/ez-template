using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace com.ez.engine.save.core
{
	public abstract class BaseGameRuntime : IGameRuntime
	{
		[ShowInInspector, InlineProperty] private readonly Dictionary<string, IRuntimeModel> _models = new();

		public IReadOnlyCollection<string> Keys => _models.Keys;

		public IReadOnlyCollection<IRuntimeModel> Models => _models.Values;

		public void AddModel<TRuntimeModel>() where TRuntimeModel : IRuntimeModel, new()
		{
			var type = typeof(TRuntimeModel);
			if (_models.ContainsKey(type.Name))
			{
				Debug.LogError($"Obscured model has already been added {type.Name}");
				return;
			}

			_models.Add(type.Name, new TRuntimeModel());
		}

		public void AddModel(IRuntimeModel runtimeModel)
		{
			var type = runtimeModel.GetType();
			if (_models.ContainsKey(type.Name))
			{
				Debug.LogError($"Obscured model has already been added {type.Name}");
				return;
			}

			_models.Add(type.Name, runtimeModel);
		}

		public void RemoveModel<TRuntimeModel>() where TRuntimeModel : IRuntimeModel
		{
			var type = typeof(TRuntimeModel);
			if (_models.ContainsKey(type.Name))
			{
				_models.Remove(type.Name);
			}
		}

		public void RemoveModel(Type runtimeModelType)
		{
			if (_models.ContainsKey(runtimeModelType.Name))
			{
				_models.Remove(runtimeModelType.Name);
			}
		}

		public void RemoveModel(string runtimeModelName)
		{
			if (_models.ContainsKey(runtimeModelName))
			{
				_models.Remove(runtimeModelName);
			}
		}

		public IRuntimeModel GetModel(Type runtimeModelType)
		{
			var result = _models.TryGetValue(runtimeModelType.Name, out var value);
			return result ? value : null;
		}

		public TRuntimeModel GetModel<TRuntimeModel>() where TRuntimeModel : IRuntimeModel
		{
			var type = typeof(TRuntimeModel);
			return _models.TryGetValue(type.Name, out var model) ? (TRuntimeModel)model : default;
		}

		public bool TryGetModel(Type runtimeModelType, out IRuntimeModel runtimeModel)
		{
			var result = _models.TryGetValue(runtimeModelType.Name, out var value);
			runtimeModel = result ? value : null;
			return result;
		}

		public bool TryGetModel<TRuntimeModel>(out TRuntimeModel runtimeModel) where TRuntimeModel : IRuntimeModel
		{
			var type = typeof(TRuntimeModel);
			var result = _models.TryGetValue(type.Name, out var value);
			runtimeModel = result ? (TRuntimeModel)value : default;
			return result;
		}

		public void Clear()
		{
			_models.Clear();
		}
	}
}
