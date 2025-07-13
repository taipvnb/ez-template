using Cysharp.Threading.Tasks;

namespace com.ez.engine.core
{
    public interface IInitializable : IService
    {
        bool Initialized { get; set; }

        UniTask OnInitialize(IArchitecture architecture);
    }
}