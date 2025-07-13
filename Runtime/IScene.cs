using System;
using com.ez.engine.core.manager;
using Cysharp.Threading.Tasks;

namespace com.ez.engine.services.scene
{
    public interface IScene
    {
        event Action<IScene> OnSceneEntered;

        event Action<IScene> OnSceneExited;

        ISceneTransition EnterTransition { get; }

        ISceneTransition ExitTransition { get; }

        ISceneLoading Loading { get; }

        TManager GetManager<TManager>() where TManager : IManager;
        
        UniTask Enter();

        UniTask Exit();

        void SetLoading(ISceneLoading loading);

        void SetEnterTransition(ISceneTransition enterTransition);

        void SetExitTransition(ISceneTransition exitTransition);
    }
}