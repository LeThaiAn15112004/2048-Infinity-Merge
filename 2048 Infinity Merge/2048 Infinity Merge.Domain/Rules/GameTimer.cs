using _2048_Infinity_Merge.Domain.Interfaces;



namespace _2048_Infinity_Merge.Domain.Rules;



public class GameTimer : IGameTimer

{

    public event Action<TimeSpan> Tick;

    public event Action<TimeSpan> Elapsed;



    public void Pause()

    {

        throw new NotImplementedException();

    }



    public void Resume()

    {

        throw new NotImplementedException();

    }



    public void Start(TimeSpan duration)

    {

        throw new NotImplementedException();

    }



    public void Stop()

    {

        throw new NotImplementedException();

    }

}

