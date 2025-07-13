using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.ez.engine.resources.core
{
	public class ResourceCollection<T> : IResourceCollection<T>
	{
		public int Count => _resources.Count;

		public IEnumerable<string> Keys => _resources.Keys;

		public IEnumerable<IResource> Resources => _resources.Values;

		private readonly Dictionary<string, IResource<T>> _resources = new();

		public void AddResource(IResource resource)
		{
			if (resource is not IResource<T> typedResource)
			{
				Debug.LogError(resource.GetType());
				Debug.LogError($"[{nameof(ResourceCollection<T>)}] Invalid resource type.");
				return;
			}

			if (_resources.ContainsKey(resource.Id))
			{
				Debug.LogWarning($"[{nameof(ResourceCollection<T>)}] Resource with id {resource.Id} already exists.");
				return;
			}

			_resources.Add(resource.Id, typedResource);
		}

		public void RemoveResource(string id)
		{
			if (!_resources.Remove(id))
			{
				Debug.LogWarning($"[{nameof(ResourceCollection<T>)}] Resource with id {id} does not exist.");
			}
		}

		public IResource<T> GetResource(string id)
		{
			if (_resources.TryGetValue(id, out var resource))
			{
				return resource;
			}

			Debug.LogError($"[{nameof(ResourceCollection<T>)}] Resource with id {id} not found.");
			return null;
		}

		public bool TryGetResource(string id, out IResource<T> resource)
		{
			return _resources.TryGetValue(id, out resource);
		}

		public void SetAmount(string id, T amount)
		{
			ModifyResource(id, amount, (res, amt) => res.SetAmount(amt));
		}

		public void AddAmount(string id, T amount, ResourceAddress address = null)
		{
			ModifyResource(id, amount, (res, amt) => res.Add(amt), address);
		}

		public void SubtractAmount(string id, T amount, ResourceAddress address = null)
		{
			ModifyResource(id, amount, (res, amt) => res.Subtract(amt), address);
		}

		public void MultiplyAmount(string id, T amount, ResourceAddress address = null)
		{
			ModifyResource(id, amount, (res, amt) => res.Multiply(amt), address);
		}

		public void DivideAmount(string id, T amount, ResourceAddress address = null)
		{
			ModifyResource(id, amount, (res, amt) => res.Divide(amt), address);
		}

		private void ModifyResource(string id, T amount, Action<IResource<T>, T> operation, ResourceAddress address = null)
		{
			if (!_resources.TryGetValue(id, out var resource))
			{
				Debug.LogError($"[{nameof(ResourceCollection<T>)}] Resource {id} not found {(address != null ? $"at {address}" : "")}.");
				return;
			}

			operation(resource, amount);
		}
	}
}
