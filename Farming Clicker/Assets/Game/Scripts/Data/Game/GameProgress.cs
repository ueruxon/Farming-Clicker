using System;

namespace Game.Scripts.Data.Game
{
    [Serializable]
    public class GameProgress
    {
        public ResourceRepository ResourceRepository;
        public UpgradeRepository UpgradeRepository;

        public GameProgress()
        {
            ResourceRepository = new ResourceRepository();
            UpgradeRepository = new UpgradeRepository();
        }
    }
}