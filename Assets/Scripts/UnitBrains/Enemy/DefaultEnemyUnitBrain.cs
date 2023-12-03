using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Runtime.Projectiles;
using UnityEngine;

namespace UnitBrains.Enemy
{
    public class DefaultEnemyUnitBrain : BaseUnitBrain
    {
        public override bool IsPlayerUnitBrain => false;

        public override Vector2Int GetNextStep()
        {
            if (HasTargetsInRange())
                return unit.Pos;
            
            var target = runtimeModel.RoMap.Bases[RuntimeModel.PlayerId];
            return CalcNextStepTowards(target);
        }
        
        public override List<BaseProjectile> GetProjectiles()
        {
            var shotTargets = 0;
            var attackRangeSqr = unit.Config.AttackRange * unit.Config.AttackRange;
            List<BaseProjectile> result = new ();
            foreach (var possibleTarget in GetPossibleTargets())
            {
                var diff = possibleTarget - unit.Pos;
                if (diff.sqrMagnitude < attackRangeSqr)
                {
                    for (int i = 0; i < unit.Config.ShotsPerTarget; i++)
                    {
                        var projectile = BaseProjectile.Create(unit.Config.ProjectileType, unit, unit.Pos,
                            possibleTarget, unit.Config.Damage);
                        result.Add(projectile);
                    }
                    shotTargets++;
                }
                
                if (shotTargets >= unit.Config.TargetsInVolley)
                    break;
            }

            return result;
        }

        protected bool HasTargetsInRange()
        {
            var attackRangeSqr = unit.Config.AttackRange * unit.Config.AttackRange;
            foreach (var possibleTarget in GetPossibleTargets())
            {
                var diff = possibleTarget - unit.Pos;
                if (diff.sqrMagnitude < attackRangeSqr)
                    return true;
            }

            return false;
        }

        protected virtual IEnumerable<Vector2Int> GetPossibleTargets()
        {
            return runtimeModel.RoUnits
                .Where(u => u.Config.IsPlayerUnit)
                .Select(u => u.Pos)
                .Append(runtimeModel.RoMap.Bases[RuntimeModel.PlayerId]);
        }
    }
}