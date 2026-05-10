namespace _2048_Infinity_Merge.Domain.Interfaces;



public interface IGameTimer{

    void Start(TimeSpan duration);

    void Pause();

    void Resume();

    void Stop();

    event Action<TimeSpan>? Tick;

    event Action<TimeSpan>? Elapsed;

}

