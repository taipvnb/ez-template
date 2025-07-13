using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Networking;
using com.ez.engine.core;

namespace com.ez.engine.services.device
{
    [Serializable]
    public class DeviceBlacklist
    {
        [SerializeField] [FoldoutGroup("$name")]
        public string name;

        [SerializeField] [FoldoutGroup("$name")]
        public string id;
    }

    public class DeviceServiceSettings : ServiceSettingsSingleton<DeviceServiceSettings>
    {
        public override string PackageName => GetType().Name;

        [SerializeField] private List<DeviceBlacklist> deviceBlacklists;

        public bool Contains(string id)
        {
            return deviceBlacklists.Exists(x => x.id.Equals(id));
        }

        public bool TryAddBlacklistDevice(string id)
        {
            if (!deviceBlacklists.Exists(x => x.id.Equals(id)))
            {
                deviceBlacklists.Add(new DeviceBlacklist() { id = id });
                return true;
            }

            return false;
        }

        public bool RemoveBlacklistDevice(string id)
        {
            for (var i = deviceBlacklists.Count - 1; i >= 0; i--)
            {
                var device = deviceBlacklists[i];
                if (device.id.Equals(id))
                {
                    deviceBlacklists.Remove(device);
                    return true;
                }
            }

            return false;
        }

        public void ClearBlacklistDevice()
        {
            deviceBlacklists?.Clear();
        }
        
#if UNITY_EDITOR
        [Button("Fetch All Blacklist Device")]
        private async void FetchAllBlacklistDevice()
        {
            try
            {
                // var request = await Http.Get("").SendWebRequest();
                // var response = request.downloadHandler.text;
                // if (!string.IsNullOrEmpty(response))
                // {
                //     var json = "{\"Items\":" + response + "}";
                //     var devices = JsonHelper.FromJson<DeviceBlacklist>(json);
                //     deviceBlacklists.Clear();
                //     foreach (var device in devices)
                //     {
                //         if (!deviceBlacklists.Exists(x => x.id.Equals(device.id)))
                //         {
                //             deviceBlacklists.Add(new DeviceBlacklist() { name = device.name, id = device.id });
                //         }
                //     }
                //
                //     deviceBlacklists.Sort((a, b) => string.Compare(a.name, b.name, StringComparison.Ordinal));
                // }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        [Button("Add New Blacklist Device")]
        private void AddBlacklistDevice()
        {
            Application.OpenURL("");
        }
#endif
    }
}