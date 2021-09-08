namespace Zenta.Core.Runtime.Interfaces
{
    public interface IGameStateListener
    {
        void OnStateChanged(GameState from, GameState to);
    }
}