namespace FFPR_Fix;

public class BattleState
{
    public bool Escaping;
    public int PlayerUnitsReadyCount;
    private bool _wait;

    public bool IsWaiting => !Escaping && _wait;

    public BattleState()
    {
        Reset();
    }

    public void Reset()
    {
        _wait = true;
        Escaping = false;
        PlayerUnitsReadyCount = 0;
    }

    public void TurnPassed()
    {
        _wait = true;
    }

    public void PassTurn()
    {
        _wait = false;
    }
}
