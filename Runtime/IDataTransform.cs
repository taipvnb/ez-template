using Cysharp.Threading.Tasks;

namespace com.ez.engine.save.core
{
	public interface IDataTransform
	{
		byte[] Apply(byte[] data);

		UniTask<byte[]> ApplyAsync(byte[] data);

		byte[] Reverse(byte[] data);

		UniTask<byte[]> ReverseAsync(byte[] data);
	}
}
