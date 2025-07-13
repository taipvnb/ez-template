namespace com.ez.engine.datetime
{
    public interface IDateTime
    {
        System.DateTime Now { get; }
        System.DateTime NowUtc { get; }
        UnbiasedTime UnbiasedTime { get; }
    }
}