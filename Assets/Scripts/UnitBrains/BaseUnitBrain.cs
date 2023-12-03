using System.Collections.Generic;
using Model;
using Model.Runtime;
using Model.Runtime.Projectiles;
using Model.Runtime.ReadOnly;
using UnityEngine;
using Utilities;
using Unit = Model.Runtime.Unit;

namespace UnitBrains
{
    public abstract class BaseUnitBrain
    {
        public virtual string TargetUnitName => string.Empty;
        public virtual bool IsPlayerUnitBrain => true;
        
        protected Unit unit { get; private set; }
        protected IReadOnlyRuntimeModel runtimeModel => ServiceLocator.Get<IReadOnlyRuntimeModel>();

        public abstract Vector2Int GetNextStep();
        public abstract List<BaseProjectile> GetProjectiles();

        public void SetUnit(Unit unit)
        {
            this.unit = unit;
        }

        public virtual void Update(float deltaTime, float time)
        {
        }

        protected Vector2Int CalcNextStepTowards(Vector2Int target)
        {
            var diff = target - unit.Pos;
            var stepDiff = diff.SignOrZero();
            var nextStep = unit.Pos + stepDiff;

            if (!runtimeModel.RoMap[nextStep])
                return nextStep;

            var sideStep0 = unit.Pos + new Vector2Int(stepDiff.y, -stepDiff.x);
            if (!runtimeModel.RoMap[sideStep0])
                return sideStep0;
            
            var sideStep1 = unit.Pos + new Vector2Int(-stepDiff.y, stepDiff.x);
            if (!runtimeModel.RoMap[sideStep1])
                return sideStep1;
            
            return unit.Pos;
        }
        
        protected List<IReadOnlyUnit> GetUnitsInRadius(float radius, bool enemies)
        {
            var units = new List<IReadOnlyUnit>();
            var pos = unit.Pos;
            var distanceSqr = radius * radius;
            
            foreach (var otherUnit in runtimeModel.RoUnits)
            {
                if (otherUnit == unit)
                    continue;

                if (enemies != (otherUnit.Config.IsPlayerUnit == unit.Config.IsPlayerUnit))
                    continue;

                var otherPos = otherUnit.Pos;
                var diff = otherPos - pos;
                var distance = diff.sqrMagnitude;
                if (distance <= distanceSqr)
                    units.Add(otherUnit);
            }

            return units;
        }
    }
}