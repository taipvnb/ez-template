using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using com.ez.engine.save.core;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace com.ez.engine.save.file
{
	public class FileStorage : IFileStorage
	{
		private readonly string _path;
		private readonly ISerializationProvider _serializationProvider;
		private readonly Dictionary<string, object> _fileLocks = new();

		public FileStorage(ISerializationProvider serializationProvider, string path)
		{
			_serializationProvider = serializationProvider;

			_path = string.IsNullOrEmpty(path)
				? Application.persistentDataPath
				: Path.Combine(Application.persistentDataPath, path);

			if (Directory.Exists(_path) == false)
			{
				Directory.CreateDirectory(_path);
			}
		}

		private object GetLock(string key)
		{
			lock (_fileLocks)
			{
				if (!_fileLocks.ContainsKey(key))
				{
					_fileLocks[key] = new object();
				}

				return _fileLocks[key];
			}
		}

		public string GetFilePath(string fileName)
		{
			return Path.Combine(_path, fileName);
		}

		public bool Exists(string key)
		{
			lock (GetLock(key))
			{
				return File.Exists(GetFilePath(key));
			}
		}

		public void Save<TData>(string key, TData data)
		{
			lock (GetLock(key))
			{
				Write(_serializationProvider.Serialize(data), key);
			}
		}

		public async UniTask SaveAsync<TData>(string key, TData data)
		{
			await UniTask.RunOnThreadPool(() =>
			{
				lock (GetLock(key))
				{
					var bytes = _serializationProvider.Serialize(data);
					using var fileStream = CreateFileStream(key, FileMode.Create, FileAccess.Write, FileShare.None);
					fileStream.Write(bytes, 0, bytes.Length);
				}
			});
		}

		public TData Load<TData>(string key)
		{
			lock (GetLock(key))
			{
				return _serializationProvider.Deserialize<TData>(Read(key));
			}
		}

		public object Load(string key, Type dataType)
		{
			lock (GetLock(key))
			{
				return _serializationProvider.Deserialize(Read(key), dataType);
			}
		}

		public async UniTask<TData> LoadAsync<TData>(string key)
		{
			return await UniTask.RunOnThreadPool(() =>
			{
				lock (GetLock(key))
				{
					using var fileStream = CreateFileStream(key, FileMode.Open, FileAccess.Read, FileShare.Read);
					var buffer = new byte[fileStream.Length];
					var read = fileStream.Read(buffer, 0, buffer.Length);
					return _serializationProvider.Deserialize<TData>(buffer);
				}
			});
		}

		public async UniTask<object> LoadAsync(string key, Type dataType)
		{
			return await UniTask.RunOnThreadPool(() =>
			{
				lock (GetLock(key))
				{
					using var fileStream = CreateFileStream(key, FileMode.Open, FileAccess.Read, FileShare.Read);
					var buffer = new byte[fileStream.Length];
					var read = fileStream.Read(buffer, 0, buffer.Length);
					return _serializationProvider.Deserialize(buffer, dataType);
				}
			});
		}

		public void Copy(string fromKey, string toKey)
		{
			lock (GetLock(fromKey))
			lock (GetLock(toKey))
			{
				var fromPath = GetFilePath(fromKey);
				var toPath = GetFilePath(toKey);
				if (File.Exists(fromPath))
				{
					File.Copy(fromPath, toPath);
				}
			}
		}

		public bool Delete(string key)
		{
			lock (GetLock(key))
			{
				if (Exists(key))
				{
					File.Delete(GetFilePath(key));
					return true;
				}

				return false;
			}
		}

		public void DeleteAll()
		{
			var info = new DirectoryInfo(_path);
			var files = info.GetFiles();
			for (var i = 0; i < files.Length; i++)
			{
				lock (GetLock(files[i].Name))
				{
					files[i].Delete();
				}
			}
		}

		private void Write(byte[] output, string fileName)
		{
			const int maxRetries = 3;
			var retries = 0;
			while (retries < maxRetries)
			{
				try
				{
					using var fileStream = CreateFileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
					fileStream.Write(output, 0, output.Length);
					return;
				}
				catch (IOException e)
				{
					retries++;
					if (retries == maxRetries)
					{
						Debug.LogError($"Failed to write {fileName} after {maxRetries} retries: {e.Message}");
						throw;
					}

					Thread.Sleep(100);
				}
			}
		}

		private byte[] Read(string fileName)
		{
			using var fileStream = CreateFileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			var buffer = new byte[fileStream.Length];
			var read = fileStream.Read(buffer, 0, buffer.Length);
			return buffer;
		}

		private FileStream CreateFileStream(string fileName, FileMode fileMode, FileAccess fileAccess, FileShare fileShare)
		{
			return new FileStream(GetFilePath(fileName), fileMode, fileAccess, fileShare, 4096, true);
		}
	}
}
