using Game.Scripts.Data.Game;

namespace Game.Scripts.Infrastructure.Services.Progress
{
    public class GameProgressService : IGameProgressService
    {
        public GameProgress Progress { get; set; }
    }

    public interface IGameProgressService
    {
        public GameProgress Progress { get; set; }
    }
}