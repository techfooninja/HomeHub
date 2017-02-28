namespace HomeHub.Shared
{
    using System;

    public interface ISensorReading
    {
        string DeviceId { get; }
        DateTime ReadingTime { get; }
    }
}
