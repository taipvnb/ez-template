using com.ez.engine.core;

namespace com.ez.engine.services.device
{
    public interface IDeviceService : IService, IInitializable
    {
        string ID { get; }
        
        DeviceClass Class { get; }
        
        bool IsBlacklist { get; }
        
        bool IsEmulator { get; }
        
        bool IsRooted { get; }

        bool ContainsBlacklistDevice(string deviceId);
        
        void AddBlacklistDevice(string deviceId);

        void RemoveBlacklistDevice(string deviceId);
        
        void ClearBlacklistDevice();
    }
}