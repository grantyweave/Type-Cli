namespace TypeCLI.Core;

public class TimerService
{
    private DateTime? _startTime;
    private TimeSpan? _duration;

    public bool IsRunning => _startTime.HasValue;
    public bool HasDuration => _duration.HasValue;

    public void Start(TimeSpan? duration = null)
    {
        _startTime = DateTime.Now;
        _duration = duration;
    }

    public void Reset()
    {
        _startTime = null;
        _duration = null;
    }

    public TimeSpan GetElapsed()
    {
        if (!_startTime.HasValue)
        {
            return TimeSpan.Zero;
        }

        return DateTime.Now - _startTime.Value;
    }

    public TimeSpan GetRemaining()
    {
        if (!_startTime.HasValue || !_duration.HasValue)
        {
            return _duration ?? TimeSpan.Zero;
        }

        TimeSpan remaining = _duration.Value - GetElapsed();
        return remaining > TimeSpan.Zero ? remaining : TimeSpan.Zero;
    }

    public bool IsExpired()
    {
        if (!_startTime.HasValue || !_duration.HasValue)
        {
            return false;
        }

        return GetElapsed() >= _duration.Value;
    }

    public string FormatElapsed()
    {
        TimeSpan elapsed = GetElapsed();
        return FormatTime(elapsed);
    }

    public string FormatRemaining()
    {
        TimeSpan remaining = GetRemaining();
        return FormatTime(remaining);
    }

    private static string FormatTime(TimeSpan time)
    {
        if (time.TotalHours >= 1)
        {
            return time.ToString(@"hh\:mm\:ss");
        }

        return time.ToString(@"mm\:ss");
    }
}
