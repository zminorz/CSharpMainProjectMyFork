using Model.Config;
using UnityEngine;

namespace Model.Runtime
{
    public interface IReadOnlyUnit
    {
        public UnitConfig Config { get; }
        public Vector2Int Pos { get; }
    }
}