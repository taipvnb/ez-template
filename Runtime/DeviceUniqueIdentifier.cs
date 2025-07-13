using UnityEngine;

#if UNITY_IOS || UNITY_IPHONE
using System.Runtime.InteropServices;
#endif

namespace com.ez.engine.device_uid
{
	public class DeviceUniqueIdentifier : IDeviceUniqueIdentifier
	{
		public string Uuid
		{
			get
			{
				if (string.IsNullOrEmpty(_uuid) || _uuid == "null" || _uuid.Length < 4)
				{
					_uuid = GenerateUuid();
				}

				return _uuid;
			}
		}

#if UNITY_IOS || UNITY_IPHONE
		[DllImport("__Internal")]
		private static extern string DeviceUniqueId();

#endif

		private string _uuid;

		public DeviceUniqueIdentifier()
		{
			_uuid = PlayerPrefs.GetString("DeviceUniqueIdentifierID");
			Debug.Log($"[{GetType().Name}] [Uuid]: {Uuid}");
		}

		private static string GenerateUuid()
		{
#if UNITY_EDITOR || UNITY_STANDALONE
			var sid = SystemInfo.deviceUniqueIdentifier;
#else
#if UNITY_IOS || UNITY_IPHONE
			var sid = DeviceUniqueId();
#elif UNITY_ANDROID
			var context = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			var activity = context.GetStatic<AndroidJavaObject>("currentActivity");
			var androidJavaObject = new AndroidJavaObject("com.unimob.deviceuniqueidentifier.DeviceUniqueIdentifier");
			var sid = androidJavaObject.CallStatic<string>("DeviceUniqueId", activity);
#else
			var sid = SystemInfo.deviceUniqueIdentifier;
#endif
#endif
			sid = sid.Replace("-", "");
			sid = sid.Length >= 32 ? sid.Substring(0, 32).ToLower() : sid.ToLower();
			PlayerPrefs.SetString("DeviceUniqueIdentifierID", sid);
			return sid;
		}
	}
}
