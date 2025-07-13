using System;
using com.ez.engine.core.di;
using com.ez.engine.core.manager;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace com.ez.engine.services.scene
{
	public abstract class Scene : MonoBehaviour, IScene
	{
		public event Action<IScene> OnSceneEntered;

		public event Action<IScene> OnSceneExited;

		public virtual ISceneTransition EnterTransition { get; private set; }

		public virtual ISceneTransition ExitTransition { get; private set; }

		public virtual ISceneLoading Loading { get; private set; }
		
		[Inject] private IManagerInstaller _managerInstaller;

		protected virtual void Awake()
		{
			EnterTransition?.Initialize();
			ExitTransition?.Initialize();
			Loading?.Initialize();
		}

		public void SetLoading(ISceneLoading loading)
		{
			Loading = loading;
			Loading.Initialize();
		}

		public void SetEnterTransition(ISceneTransition enterTransition)
		{
			EnterTransition = enterTransition;
			EnterTransition.Initialize();
		}

		public void SetExitTransition(ISceneTransition exitTransition)
		{
			ExitTransition = exitTransition;
			ExitTransition.Initialize();
		}

		public TManager GetManager<TManager>() where TManager : IManager
		{
			return _managerInstaller.GetManager<TManager>();
		}

		private async UniTask InstallManagers()
		{
			_managerInstaller.Clear();

			await OnInstallManagers(_managerInstaller);

			foreach (var manager in _managerInstaller.Managers)
			{
				try
				{
					await manager.Initialize();
				}
				catch (Exception e)
				{
					Debug.LogError($"{manager.GetType()}: {e.Message}\n{e.StackTrace}");
				}
			}
		}

		public async UniTask Enter()
		{
			await InstallManagers();
			await OnEnter();
			OnSceneEntered?.Invoke(this);
		}

		public async UniTask Exit()
		{
			await OnExit();
			OnSceneExited?.Invoke(this);
		}

		protected abstract UniTask OnInstallManagers(IManagerInstaller installer);
		protected abstract UniTask OnEnter();
		protected abstract UniTask OnExit();
	}
}
