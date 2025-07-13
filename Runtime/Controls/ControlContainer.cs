using System;
using UnityEngine;

namespace com.ez.engine.manager.ui
{
	public class ControlContainer : ViewContainer
	{
		[SerializeField] private string _name;
		[SerializeField] private UISettings _settings;

		public string ContainerName => _name;

		public override UISettings Settings
		{
			get
			{
				if (_settings == false)
				{
					_settings = UISettings.DefaultSettings;
				}

				return _settings;
			}

			set
			{
				if (value == false)
				{
					throw new ArgumentNullException(nameof(value));
				}

				_settings = value;
			}
		}

		protected override void Awake()
		{
			var _ = CanvasGroup;
			InitializePool();
		}
	}
}
