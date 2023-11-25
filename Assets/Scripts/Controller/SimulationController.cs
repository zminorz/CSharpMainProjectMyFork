
using Model;
using Utilities;

namespace Controller
{
    public class SimulationController
    {
        private readonly RuntimeModel _runtimeModel;

        public SimulationController(RuntimeModel runtimeModel)
        {
            _runtimeModel = runtimeModel;
            var timeUtil = ServiceLocator.Get<TimeUtil>();
            
            timeUtil.AddFixedUpdateAction(Update);
        }
        
        private void Update(float deltaTime)
        {
            if (_runtimeModel.Stage != RuntimeModel.GameStage.Simulation)
                return;

            foreach (var unitList in _runtimeModel.PlayersUnits)
                foreach (var unit in unitList)
                {
                    unit.Update(deltaTime);
                    _runtimeModel.Projectiles.AddRange(unit.PendingProjectiles);
                    unit.ClearPendingProjectiles();
                }

            foreach (var projectile in _runtimeModel.Projectiles)
            {
                projectile.Update(deltaTime);
            }
        }
    }
}