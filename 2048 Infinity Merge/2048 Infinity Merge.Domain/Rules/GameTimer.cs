using System.Threading;
using _2048_Infinity_Merge.Domain.Interfaces;

namespace _2048_Infinity_Merge.Domain.Rules;

public sealed class GameTimer : IGameTimer
{
    private readonly object _gate = new();
    private readonly TimeSpan _tickPeriod = TimeSpan.FromSeconds(1);

    private Timer? _timer;
    private DateTimeOffset _deadline;
    private bool _paused;
    private TimeSpan _pausedRemaining;

    public event Action<TimeSpan>? Tick;
    public event Action<TimeSpan>? Elapsed;

    /// <inheritdoc />
    public void Start(TimeSpan duration)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(duration, TimeSpan.Zero);

        lock (_gate)
        {
            DisposeTimerLocked();
            _paused = false;
            _deadline = DateTimeOffset.UtcNow + duration;
            ScheduleTimerLocked();
        }
    }

    public void Pause()
    {
        lock (_gate)
        {
            if (_timer is null || _paused)
                return;

            _pausedRemaining = RemainingOrZero();
            _paused = true;
            DisposeTimerLocked();
        }
    }

    public void Resume()
    {
        lock (_gate)
        {
            if (!_paused)
                return;

            _paused = false;

            if (_pausedRemaining <= TimeSpan.Zero)
            {
                Tick?.Invoke(TimeSpan.Zero);
                Elapsed?.Invoke(TimeSpan.Zero);
                return;
            }

            _deadline = DateTimeOffset.UtcNow + _pausedRemaining;
            ScheduleTimerLocked();
        }
    }

    public void Stop()
    {
        lock (_gate)
        {
            DisposeTimerLocked();
            _paused = false;
        }
    }

    private void ScheduleTimerLocked()
    {
        _timer?.Dispose();
        _timer = new Timer(OnTick, null, TimeSpan.Zero, _tickPeriod);
    }

    private void OnTick(object? _)
    {
        lock (_gate)
        {
            if (_paused || _timer is null)
                return;

            var remaining = RemainingOrZero();

            if (remaining <= TimeSpan.Zero)
            {
                Tick?.Invoke(TimeSpan.Zero);
                Elapsed?.Invoke(TimeSpan.Zero);
                DisposeTimerLocked();
                return;
            }

            Tick?.Invoke(remaining);
        }
    }

    private TimeSpan RemainingOrZero()
    {
        var r = _deadline - DateTimeOffset.UtcNow;
        return r < TimeSpan.Zero ? TimeSpan.Zero : r;
    }

    private void DisposeTimerLocked()
    {
        _timer?.Dispose();
        _timer = null;
    }
}
