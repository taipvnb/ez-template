using System.Collections.Generic;

namespace com.ez.engine.resources.core
{
	public interface IResourceCollection
	{
		int Count { get; }

		IEnumerable<string> Keys { get; }

		IEnumerable<IResource> Resources { get; }

		void AddResource(IResource resource);

		void RemoveResource(string id);
	}

	public interface IResourceCollection<T> : IResourceCollection
	{
		IResource<T> GetResource(string id);

		bool TryGetResource(string id, out IResource<T> resource);

		void SetAmount(string id, T amount);

		void AddAmount(string id, T amount, ResourceAddress address = null);

		void SubtractAmount(string id, T amount, ResourceAddress address = null);

		void MultiplyAmount(string id, T amount, ResourceAddress address = null);

		void DivideAmount(string id, T amount, ResourceAddress address = null);
	}
}
