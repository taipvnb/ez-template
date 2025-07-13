using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace com.ez.engine.save.core
{
	public class SaveBuffer
	{
		private readonly IStorageProvider _storageProvider;
		private readonly Dictionary<string, (Type saveModelType, ISaveModel saveModel)> _buffer = new();
		private readonly object _lock = new();
		private int _saveCallCount;
		private bool _isDirty;

		public SaveBuffer(IStorageProvider storageProvider)
		{
			_storageProvider = storageProvider;
		}

		public bool TrySave<TSaveModel>(string key, TSaveModel saveModel, int flushThreshold = 1) where TSaveModel : ISaveModel
		{
			lock (_lock)
			{
				_buffer[key] = (typeof(TSaveModel), saveModel);
				_saveCallCount++;
				_isDirty = true;
				return _saveCallCount >= flushThreshold;
			}
		}

		public void Flush()
		{
			lock (_lock)
			{
				if (!_isDirty)
				{
					return;
				}

				FlushInternal();
			}
		}

		public async UniTask FlushAsync()
		{
			Dictionary<string, (Type saveModelType, ISaveModel saveModel)> snapshot;
			lock (_lock)
			{
				if (!_isDirty)
				{
					return;
				}

				snapshot = new Dictionary<string, (Type saveModelType, ISaveModel saveModel)>(_buffer);
				_buffer.Clear();
				_saveCallCount = 0;
				_isDirty = false;
			}

			foreach (var item in snapshot)
			{
				try
				{
					await _storageProvider.SaveAsync(item.Key, item.Value.saveModel);
					// Debug.Log($"Flushed async save for {item.Key} of type {item.Value.saveModelType.Name}");
				}
				catch (Exception e)
				{
					Debug.LogError($"Failed to flush async {item.Key}: {e.Message}\n{e.StackTrace}");
				}
			}
		}

		private void FlushInternal()
		{
			var snapshot = new Dictionary<string, (Type saveModelType, ISaveModel saveModel)>(_buffer);
			_buffer.Clear();
			_saveCallCount = 0;
			_isDirty = false;

			foreach (var item in snapshot)
			{
				try
				{
					_storageProvider.Save(item.Key, item.Value.saveModel);
					// Debug.Log($"Flushed save for {item.Key} of type {item.Value.saveModelType.Name}");
				}
				catch (Exception e)
				{
					Debug.LogError($"Failed to flush {item.Key}: {e.Message}\n{e.StackTrace}");
				}
			}
		}
	}
}
