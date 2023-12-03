using System.Collections.Generic;
using Model;
using Model.Runtime.Projectiles;
using UnityEngine;

namespace UnitBrains.Player
{
    public class DefaultPlayerUnitBrain : BaseUnitBrain
    {
        public override Vector2Int GetNextStep()
        {
            var target = runtimeModel.RoMap.Bases[RuntimeModel.BotPlayerId];
            return CalcNextStepTowards(target);
        }

        public override List<BaseProjectile> GetProjectiles()
        {
            return null;
        }
    }
}