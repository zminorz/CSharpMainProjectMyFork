using System.Collections.Generic;
using Model.Runtime;

namespace Model
{
    public interface IReadOnlyRuntimeModel
    {
        IReadOnlyMap RoMap { get; }
        RuntimeModel.GameStage Stage { get; }
        public int Level { get; }
        public IReadOnlyDictionary<int, int> RoMoney { get; }
        public IEnumerable<IReadOnlyUnit> RoUnits { get; }
        
        public IEnumerable<IReadOnlyUnit> RoPlayerUnits { get; }
        public IEnumerable<IReadOnlyUnit> RoBotUnits { get; }
        public IReadOnlyList<IReadOnlyBase> RoBases { get; }
    }
}