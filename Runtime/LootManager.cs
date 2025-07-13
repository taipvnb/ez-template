using System.Collections.Generic;
using com.ez.engine.core.di;
using com.ez.engine.loots.core;
using com.ez.engine.resources.core;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace com.ez.engine.manager.loot
{
	public class LootManager : MonoBehaviour, ILootManager
	{
		[Inject] private readonly IInjector _injector;

		[ShowInInspector, ReadOnly] private readonly Dictionary<string, ILootCreator> _creators = new();
		[ShowInInspector, ReadOnly] private readonly Dictionary<string, ILootHandler> _handlers = new();

		public int Priority => 0;
		public bool IsInitialized { get; private set; }

		public UniTask Initialize()
		{
			if (IsInitialized)
			{
				return UniTask.CompletedTask;
			}

			RegisterCreators();
			RegisterHandlers();
			IsInitialized = true;
			return UniTask.CompletedTask;
		}

		public void Loot(LootParams lootParams, ResourceAddress address)
		{
			if (_handlers.TryGetValue(lootParams.LootType, out var handler))
			{
				var loot = CreateLoot(lootParams);
				handler.Cache(loot);
				handler.Handle(loot, address);
			}
		}

		public void Loot(List<LootParams> lootParams, ResourceAddress address)
		{
			foreach (var lootParam in lootParams)
			{
				if (_handlers.TryGetValue(lootParam.LootType, out var handler))
				{
					var loot = CreateLoot(lootParam);
					handler.Cache(loot);
					handler.Handle(loot, address);
				}
			}
		}

		public void Loot(ILoot loot, ResourceAddress address)
		{
			if (_handlers.TryGetValue(loot.LootType, out var handler))
			{
				handler.Cache(loot);
				handler.Handle(loot, address);
			}
		}

		public void Loot(List<ILoot> loots, ResourceAddress address)
		{
			foreach (var loot in loots)
			{
				if (_handlers.TryGetValue(loot.LootType, out var handler))
				{
					handler.Cache(loot);
					handler.Handle(loot, address);
				}
			}
		}

		public ILoot CreateLoot(LootParams lootParams)
		{
			if (lootParams.Params.Length == 0)
			{
				return NullLoot.Instance;
			}

			if (!_creators.ContainsKey(lootParams.LootType))
			{
#if UNITY_EDITOR
				Debug.LogError("There is no creator for loot type " + lootParams.LootType);
#endif
				return NullLoot.Instance;
			}

			var creator = _creators[lootParams.LootType];
			return creator.Create(lootParams);
		}

		public List<ILoot> CreateLoots(List<LootParams> lootParams)
		{
			var loots = new List<ILoot>();
			foreach (var lootParam in lootParams)
			{
				loots.Add(CreateLoot(lootParam));
			}

			return loots;
		}

		public THandler GetHandler<THandler>() where THandler : ILootHandler
		{
			foreach (var handler in _handlers.Values)
			{
				if (handler is THandler lootHandler)
				{
					return lootHandler;
				}
			}

			return default;
		}

		public bool Handle(ILoot loot, ResourceAddress address)
		{
			if (!_handlers.ContainsKey(loot.LootType))
			{
#if UNITY_EDITOR
				Debug.LogError("Missing loot handler of type " + loot.LootType);
#endif
				return false;
			}

			_handlers[loot.LootType].Handle(loot, address);
			return true;
		}

		public bool Handle(List<ILoot> loots, ResourceAddress address)
		{
			foreach (var loot in loots)
			{
				Handle(loot, address);
			}

			return true;
		}

		public void Dispose()
		{
			_creators.Clear();
		}

		private void RegisterCreators()
		{
			var creators = GetComponentsInChildren<ILootCreator>();
			foreach (var creator in creators)
			{
				if (_creators.TryAdd(creator.LootType, creator))
				{
					_injector.Resolve(creator);
					Debug.Log($"Registered LootCreator for {creator.LootType}");
				}
				else
				{
					Debug.LogWarning($"Duplicate LootType {creator.LootType} found for {creator.GetType().Name}");
				}
			}
		}

		private void RegisterHandlers()
		{
			var handlers = GetComponentsInChildren<ILootHandler>();
			foreach (var handler in handlers)
			{
				if (_handlers.TryAdd(handler.LootType, handler))
				{
					_injector.Resolve(handler);
					Debug.Log($"Registered LootHandler for {handler.LootType}");
				}
				else
				{
					Debug.LogWarning($"Duplicate LootType {handler.LootType} found for {handler.GetType().Name}");
				}
			}
		}
	}
}
