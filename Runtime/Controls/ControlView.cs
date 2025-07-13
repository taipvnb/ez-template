using System;
using System.Collections.Generic;
using com.ez.engine.foundation;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace com.ez.engine.manager.ui
{
	[DisallowMultipleComponent]
	public class ControlView : Window
	{
		[SerializeField] private ControlTransitionAnimationContainer _animationContainer = new();

		private Progress<float> _transitionProgressReporter;
		private readonly UniquePriorityList<IControlLifecycleEvent> _lifecycleEvents = new();

		private Progress<float> TransitionProgressReporter
		{
			get { return _transitionProgressReporter ??= new Progress<float>(SetTransitionProgress); }
		}

		public ControlTransitionAnimationContainer AnimationContainer => _animationContainer;

		public bool IsTransitioning { get; private set; }

		public ControlTransitionAnimationType? TransitionAnimationType { get; private set; }

		public float TransitionAnimationProgress { get; private set; }

		public event Action<float> TransitionAnimationProgressChanged;

		public void AddLifecycleEvent(IControlLifecycleEvent lifecycleEvent, int priority = 0)
		{
			_lifecycleEvents.Add(lifecycleEvent, priority);
		}

		public void RemoveLifecycleEvent(IControlLifecycleEvent lifecycleEvent)
		{
			_lifecycleEvents.Remove(lifecycleEvent);
		}

		internal async UniTask AfterLoadAsync(RectTransform parentTransform)
		{
			SetIdentifier();

			Parent = parentTransform;
			RectTransform.FillParent(Parent);
			gameObject.SetActive(false);

			OnAfterLoad();

			GetViews();

			var tasks = _lifecycleEvents.Select(x => x.Initialize());
			await WaitForAsync(tasks);
		}

		protected virtual void OnAfterLoad()
		{
			RectTransform.SetParentRect(Parent);
		}

		internal async UniTask BeforeEnterAsync()
		{
			IsTransitioning = true;
			TransitionAnimationType = ControlTransitionAnimationType.Enter;
			gameObject.SetActive(true);

			OnBeforeEnter();

			var tasks = _lifecycleEvents.Select(x => x.WillEnter());
			await WaitForAsync(tasks);
		}

		protected virtual void OnBeforeEnter()
		{
			RectTransform.SetParentRect(Parent);
		}

		internal async UniTask EnterAsync(bool playAnimation, ControlView partnerControl)
		{
			OnEnter();

			if (playAnimation == false)
			{
				return;
			}

			var anim = GetAnimation(true, partnerControl);

			if (anim == null)
			{
				return;
			}

			if (partnerControl)
			{
				anim.SetPartner(partnerControl.RectTransform);
			}

			anim.Setup(RectTransform);

			await anim.PlayAsync(TransitionProgressReporter);
		}

		protected virtual void OnEnter() { }

		internal void AfterEnter()
		{
			foreach (var lifecycleEvent in _lifecycleEvents)
			{
				lifecycleEvent.DidEnter();
			}

			IsTransitioning = false;
			TransitionAnimationType = null;
		}

		internal async UniTask BeforeExitAsync()
		{
			IsTransitioning = true;
			TransitionAnimationType = ControlTransitionAnimationType.Exit;
			gameObject.SetActive(true);

			OnBeforeExit();

			var tasks = _lifecycleEvents.Select(x => x.WillExit());
			await WaitForAsync(tasks);
		}

		protected virtual void OnBeforeExit()
		{
			RectTransform.SetParentRect(Parent);
		}

		internal async UniTask ExitAsync(bool playAnimation, ControlView partnerControl)
		{
			OnExit();

			if (playAnimation == false)
			{
				return;
			}

			var anim = GetAnimation(false, partnerControl);

			if (anim == null)
			{
				return;
			}

			if (partnerControl)
			{
				anim.SetPartner(partnerControl.RectTransform);
			}

			anim.Setup(RectTransform);

			await anim.PlayAsync(TransitionProgressReporter);
		}

		protected virtual void OnExit() { }

		internal void AfterExit()
		{
			foreach (var lifecycleEvent in _lifecycleEvents)
			{
				lifecycleEvent.DidExit();
			}

			gameObject.SetActive(false);
		}

		internal async UniTask BeforeReleaseAsync()
		{
			var tasks = _lifecycleEvents.Select(x => x.Cleanup());
			await WaitForAsync(tasks);
		}

		protected void SetTransitionProgress(float progress)
		{
			TransitionAnimationProgress = progress;
			TransitionAnimationProgressChanged?.Invoke(progress);
		}

		private IReadOnlyList<TransitionAnimation> GetTransitions(bool enter)
		{
			return _animationContainer.GetTransitions(enter);
		}

		protected virtual ITransitionAnimation GetAnimation(bool enter, ControlView partner)
		{
			var partnerIdentifier = partner == true ? partner.Identifier : string.Empty;
			return _animationContainer.GetAnimation(enter, partnerIdentifier);
		}
	}
}
