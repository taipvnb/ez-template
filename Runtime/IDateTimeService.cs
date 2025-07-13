using System;
using com.ez.engine.core;

namespace com.ez.engine.services.datetime
{
	public interface IDateTimeService : IInitializable, IPausable, IQuitable
	{
		DateTime Now { get; }

		DateTime NowUtc { get; }
	}
}
