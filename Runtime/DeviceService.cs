using Cysharp.Threading.Tasks;
using com.ez.engine.core;
using com.ez.engine.core.di;
using com.ez.engine.device_uid;
using UnityEngine;

namespace com.ez.engine.services.device
{
	[Service(typeof(IDeviceService))]
	public class DeviceService : MonoBehaviour, IDeviceService
	{
		public int Priority => -1;
		public bool Initialized { get; set; }
		public string ID { get; private set; }
		public DeviceClass Class { get; private set; }
		public bool IsBlacklist { get; private set; }
		public bool IsEmulator { get; private set; }
		public bool IsRooted { get; private set; }
		

		private DeviceServiceSettings _settings;

		public UniTask OnInitialize(IArchitecture architecture)
		{
			_settings = DeviceServiceSettings.Instance;
			var deviceUniqueIdentifier = new DeviceUniqueIdentifier();
			ID = deviceUniqueIdentifier.Uuid;
			IsBlacklist = _settings.Contains(ID);
			Class = GetDeviceClass();
			Initialized = true;
#if UNITY_ANDROID && !UNITY_EDITOR
            IsEmulator = IsEmulatorDevice();
            IsRooted = IsRootedDevice();
			_logger.Info(ToString());
#endif
			return UniTask.CompletedTask;
		}

		public bool ContainsBlacklistDevice(string deviceId)
		{
			return _settings.Contains(deviceId);
		}

		public void AddBlacklistDevice(string deviceId)
		{
			if (_settings.TryAddBlacklistDevice(deviceId))
			{
				IsBlacklist = _settings.Contains(ID);
			}
		}

		public void RemoveBlacklistDevice(string deviceId)
		{
			if (_settings.RemoveBlacklistDevice(deviceId))
			{
				IsBlacklist = _settings.Contains(ID);
			}
		}

		public void ClearBlacklistDevice()
		{
			_settings.ClearBlacklistDevice();
		}

		public void SetQualitySettings(DeviceClass level)
		{
			if (level == DeviceClass.High)
			{
				QualitySettings.vSyncCount = 1;
				Application.targetFrameRate = 30;
			}
			else
			{
				QualitySettings.vSyncCount = 0;
				Application.targetFrameRate = 30;
			}
		}
		
		private bool IsEmulatorDevice()
		{
			var osBuild = new AndroidJavaClass("android.os.Build");
			var fingerPrint = osBuild.GetStatic<string>("FINGERPRINT");
			var model = osBuild.GetStatic<string>("MODEL");
			var manufacturer = osBuild.GetStatic<string>("MANUFACTURER");
			var brand = osBuild.GetStatic<string>("BRAND");
			var device = osBuild.GetStatic<string>("DEVICE");
			var product = osBuild.GetStatic<string>("PRODUCT");

			return fingerPrint.Contains("generic")
				   || fingerPrint.Contains("unknown")
				   || model.Contains("google_sdk")
				   || model.Contains("Emulator")
				   || model.Contains("Android SDK built for x86")
				   || manufacturer.Contains("Genymotion")
				   || (brand.Contains("generic") && device.Contains("generic"))
				   || product.Equals("google_sdk")
				   || product.Equals("unknown");
		}

		private bool IsRootedDevice()
		{
			if (IsRootedPrivate("/system/bin/su"))
			{
				return true;
			}

			if (IsRootedPrivate("/system/xbin/su"))
			{
				return true;
			}

			if (IsRootedPrivate("/system/app/SuperUser.apk"))
			{
				return true;
			}

			if (IsRootedPrivate("/data/data/com.noshufou.android.su"))
			{
				return true;
			}

			if (IsRootedPrivate("/sbin/su"))
			{
				return true;
			}

			return false;
		}

		private static bool IsRootedPrivate(string path)
		{
			return System.IO.File.Exists(path);
		}

		private static DeviceClass GetDeviceClass()
		{
			var processorCount = SystemInfo.processorCount;
			var graphicsMemorySize = SystemInfo.graphicsMemorySize;
			var systemMemorySize = SystemInfo.systemMemorySize;

			if (SystemInfo.graphicsDeviceVendorID == 32902)
			{
				return DeviceClass.Low;
			}
			else
			{
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_WSA_10_0
				if (processorCount <= 2)
				{
					return DeviceClass.Low;
				}
#elif UNITY_STANDALONE_OSX || UNITY_IPHONE
				if (processorCount <= 2)
				{
					return DeviceClass.Low;
				}
#elif UNITY_ANDROID
				if (processorCount <= 4)
				{
					return DeviceClass.Low;
				}
#endif

#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_WSA_10_0
				if (graphicsMemorySize >= 4000 && systemMemorySize >= 8000)
				{
					return DeviceClass.High;
				}
				else if (graphicsMemorySize >= 2000 && systemMemorySize >= 4000)
				{
					return DeviceClass.Mid;
				}
				else
				{
					return DeviceClass.Low;
				}
#elif UNITY_STANDALONE_OSX || UNITY_IPHONE
				if (graphicsMemorySize >= 4000 && systemMemorySize >= 8000)
				{
					return DeviceClass.High;
				}
				else if (graphicsMemorySize >= 2000 && systemMemorySize >= 4000)
				{
					return DeviceClass.Mid;
				}
				else
				{
					return DeviceClass.Low;
				}
#elif UNITY_ANDROID
				if (graphicsMemorySize >= 6000 && systemMemorySize >= 8000)
				{
					return DeviceClass.High;
				}
				else if (graphicsMemorySize >= 2000 && systemMemorySize >= 4000)
				{
					return DeviceClass.Mid;
				}
				else
				{
					return DeviceClass.Low;
				}
#endif
				return DeviceClass.Mid;
			}
		}

		public override string ToString()
		{
			var result = "";
			var osBuild = new AndroidJavaClass("android.os.Build");
			var brand = osBuild.GetStatic<string>("BRAND");
			var fingerPrint = osBuild.GetStatic<string>("FINGERPRINT");
			var model = osBuild.GetStatic<string>("MODEL");
			var manufacturer = osBuild.GetStatic<string>("MANUFACTURER");
			var device = osBuild.GetStatic<string>("DEVICE");
			var product = osBuild.GetStatic<string>("PRODUCT");

			result += Application.installerName;
			result += "/";
			result += Application.installMode.ToString();
			result += "/";
			result += Application.buildGUID;
			result += "/";
			result += "Genuine :" + Application.genuine;
			result += "/";
			result += "Unique identifier :" + ID;
			result += "/";
			result += "Blacklist :" + IsBlacklist;
			result += "/";
			result += "Rooted : " + IsRooted;
			result += "/";
			result += "Emulator : " + IsEmulator;
			result += "/";
			result += "Brand : " + brand;
			result += "/";
			result += "Model : " + model;
			result += "/";
			result += "Manufacturer : " + manufacturer;
			result += "/";
			result += "Device : " + device;
			result += "/";
			result += "Fingerprint : " + fingerPrint;
			result += "/";
			result += "Product : " + product;
			return result;
		}
	}
}
