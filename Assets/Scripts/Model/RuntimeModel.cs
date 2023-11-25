using System.Collections.Generic;
using System.Linq;
using Model.Runtime;

namespace Model
{
    public class RuntimeModel : IReadOnlyRuntimeModel
    {
        public const int PlayerId = 0;
        public const int BotPlayerId = 1;
        
        public IReadOnlyMap RoMap => Map;
        public IReadOnlyDictionary<int, int> RoMoney => Money;

        public IEnumerable<IReadOnlyUnit> RoUnits =>
            _playersUnits[PlayerId].Concat(_playersUnits[BotPlayerId]);
        
        public IEnumerable<IReadOnlyUnit> RoPlayerUnits => _playersUnits[PlayerId];
        public IEnumerable<IReadOnlyUnit> RoBotUnits => _playersUnits[BotPlayerId];
        public IReadOnlyList<IReadOnlyBase> RoBases => Bases;
        
        public Map Map { get; set; }
        public GameStage Stage { get; set; } = GameStage.None;
        public int Level { get; set; }
        public IReadOnlyList<List<Unit>> PlayersUnits => _playersUnits;
        public List<Projectile> Projectiles { get; } = new();
        public List<MainBase> Bases { get; } = new()
        {
            null, null
        };

        public Dictionary<int, int> Money { get; } = new()
        {
            { PlayerId, 0 },
            { BotPlayerId, 0 }
        };
        
        private readonly List<List<Unit>> _playersUnits = new()
        {
            new (), new ()
        };

        public void SetMoneyForAll(int startMoney)
        {
            Money[PlayerId] = startMoney;
            Money[BotPlayerId] = startMoney;
        }
        
        public enum GameStage
        {
            None,
            ChooseUnit,
            Simulation,
            Finished
        }
    }
}