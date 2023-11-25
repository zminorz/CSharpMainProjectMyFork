using System.Collections.Generic;
using Model.Config;
using UnityEngine;

namespace Model.Runtime
{
    public class Unit : IReadOnlyUnit
    {
        public UnitConfig Config { get; }
        public Vector2Int Pos { get; private set; }
        public int Health { get; private set; }
        public bool IsDead => Health <= 0;
        public IReadOnlyList<Projectile> PendingProjectiles => _pendingProjectiles;

        private readonly List<Projectile> _pendingProjectiles = new();
        
        public Unit(UnitConfig config, Vector2Int startPos)
        {
            Config = config;
            Pos = startPos;
            Health = config.MaxHealth;
        }

        public void Update(float deltaTime)
        {
            throw new System.NotImplementedException();
        }
        
        public void ClearPendingProjectiles()
        {
            _pendingProjectiles.Clear();
        }
        
        protected void AddPendingProjectile(Projectile projectile)
        {
            _pendingProjectiles.Add(projectile);
        }
    }
}