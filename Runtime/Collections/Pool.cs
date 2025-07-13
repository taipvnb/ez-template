using System.Collections.Generic;
using UnityEngine;

namespace com.ez.engine.foundation.collections
{
	public class Pool<T> where T : new()
	{
		private static Pool<T> _shared = new();

		public static Pool<T> Shared => _shared;

		private readonly Queue<T> _queue = new();

#if UNITY_EDITOR
		/// <seealso href="https://docs.unity3d.com/Manual/DomainReloading.html"/>
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void InitializeOnLoad()
		{
			_shared = new Pool<T>();
		}
#endif

		public T Rent()
		{
			return _queue.Count == 0 ? new T() : _queue.Dequeue();
		}

		public void Return(T instance)
		{
			_queue.Enqueue(instance);
		}
	}
}
