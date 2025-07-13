using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace com.ez.engine.services.message_bus.editor
{
	public class MessageBusMenuBuilder : IDashboardMenuBuilder
	{
		public int Order => -1;

		private const string MenuName = "Message Bus";

		public void Build(OdinMenuTree tree)
		{
			tree.Add(MenuName, new MessageBusDashboard(), SdfIconType.Messenger);
		}
	}

	public class MessageBusDashboard
	{
		[ShowInInspector]
		[ListDrawerSettings(ShowItemCount = false, ShowIndexLabels = false, HideAddButton = true, HideRemoveButton = true,
			ShowFoldout = false, DraggableItems = false, AlwaysAddDefaultValue = false)]
		private List<MessageBusEntry> _messages = new();

		public MessageBusDashboard()
		{
			Refresh();
		}

		private void Refresh()
		{
			_messages.Clear();

			var assetPaths = AssetDatabase.GetAllAssetPaths();
			var messages = MessageBusGenerator.GetAllTypesImpInterface<IMessage>();

			foreach (var assetPath in assetPaths)
			{
				if (assetPath.Contains(".cs"))
				{
					foreach (var message in messages)
					{
						if (message.Name.Equals(Path.GetFileNameWithoutExtension(assetPath)))
						{
							var path = assetPath.Replace("\\", "/");
							if (File.Exists(path))
							{
								var description =
									Attribute.GetCustomAttribute(message, typeof(System.ComponentModel.DescriptionAttribute)) is
										System.ComponentModel.DescriptionAttribute descriptionAttribute
										? descriptionAttribute.Description
										: "No description available";

								_messages.Add(new MessageBusEntry(message.Name, description, message));
							}
						}
					}
				}
			}
		}
	}

	[InlineProperty]
	[HideReferenceObjectPicker]
	public class MessageBusEntry
	{
		[HideLabel] public string Name { get; private set; }

		[HideLabel] public string Description { get; private set; }

		private Type _messageType;

		public MessageBusEntry(string name, string description, Type messageType)
		{
			_messageType = messageType;
			Name = name;
			Description = description;
		}
	}

#if UNITY_EDITOR
	[CustomPropertyDrawer(typeof(MessageBusEntry))]
	public class MessageBusEntryOdinDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var drawer = new MessageBusEntryDrawer();
			drawer.DrawProperty(label);
		}
	}

	public class MessageBusEntryDrawer : OdinValueDrawer<MessageBusEntry>
	{
		private bool _isExpanded = false;

		protected override void DrawPropertyLayout(GUIContent label)
		{
			var entry = ValueEntry.SmartValue;
			if (entry != null)
			{
				var defaultColor = GUI.contentColor;
				GUI.contentColor = Color.white;

				_isExpanded = SirenixEditorGUI.Foldout(_isExpanded, entry.Name);
				if (_isExpanded)
				{
					GUI.contentColor = Color.cyan;
					SirenixEditorGUI.IconMessageBox(entry.Description, SdfIconType.Messenger);
					GUI.contentColor = defaultColor;
				}
			}
			else
			{
				base.DrawPropertyLayout(label);
			}
		}
	}
#endif
}
