using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Config;
using Model.Runtime;
using UnityEngine;
using Utilities;

namespace View
{
    public class Gameplay3dView : MonoBehaviour
    {
        private IReadOnlyRuntimeModel _runtimeModel;
        
        private readonly List<TileView> _tiles = new();
        private readonly Dictionary<IReadOnlyUnit, UnitView> _units = new();
        
        private readonly List<TileView> _tilePrefabs = new();
        private readonly Dictionary<string, UnitView> _unitPrefabsPerName = new();
        private readonly HashSet<IReadOnlyUnit> _existingUnits = new();
        
        public void Reinitialize()
        {
            _runtimeModel = ServiceLocator.Get<IReadOnlyRuntimeModel>();
            LoadPrefabsIfNeeded();
            
            Clear();

            CreateTiles();
        }

        private void Update()
        {
            if (_runtimeModel == null)
                return;
            
            _existingUnits.Clear();
            
            foreach (var unitModel in _runtimeModel.RoUnits)
            {
                _existingUnits.Add(unitModel);
                
                if (!_units.TryGetValue(unitModel, out var unitView))
                {
                    unitView = Instantiate(_unitPrefabsPerName[unitModel.Config.name], transform);
                    _units.Add(unitModel, unitView);
                }

                UpdateUnit(unitModel, unitView);
            }
            
            foreach (var unit in _units.Keys.Where(u => !_existingUnits.Contains(u)))
            {
                var unitView = _units[unit];
                _units.Remove(unit);
                Destroy(unitView.gameObject);
            }
        }

        private void UpdateUnit(IReadOnlyUnit unitModel, UnitView unitView)
        {
            unitView.transform.position = ToWorldPosition(unitModel.Pos);
        }

        private void CreateTiles()
        {
            for (int w = 0; w < _runtimeModel.RoMap.Width; w++)
            {
                for (int h = 0; h < _runtimeModel.RoMap.Height; h++)
                {
                    var isBlocked = _runtimeModel.RoMap[w, h];
                    var isPlayerBase = _runtimeModel.RoMap.Bases[RuntimeModel.PlayerId] == new Vector2Int(w, h);
                    var isEnemyBase = _runtimeModel.RoMap.Bases[RuntimeModel.BotPlayerId] == new Vector2Int(w, h);
                    var prefabs = _tilePrefabs.Where( p =>
                            p.IsBlocked == isBlocked && p.IsBaseEnemy == isEnemyBase && p.IsBasePlayer == isPlayerBase)
                        .ToList();
                    
                    if (prefabs.Count == 0)
                    {
                        Debug.LogError($"Could not find prefab for cell ({w}, {h}), isBlocked: {isBlocked}, isPlayerBase: {isPlayerBase}, isEnemyBase: {isEnemyBase}");
                        continue;
                    }
                    var tileView = Instantiate(prefabs[Random.Range(0, prefabs.Count)], transform);
                    tileView.transform.position = ToWorldPosition(new Vector2Int(w, h));
                    _tiles.Add(tileView);
                }
            }
        }
        
        private Vector3 ToWorldPosition(Vector2Int pos)
        {
            return new Vector3(2f * pos.x, 0, 2f * pos.y);
        }
        
        private void Clear()
        {
            foreach (var unit in _units)
            {
                Destroy(unit.Value.gameObject);
            }
            
            _units.Clear();
            
            foreach (var tile in _tiles)
            {
                Destroy(tile.gameObject);
            }
            
            _tiles.Clear();
        }

        private void LoadPrefabsIfNeeded()
        {
            if (_tilePrefabs.Count > 0 && _unitPrefabsPerName.Count > 0)
                return;

            _tilePrefabs.AddRange(Resources.LoadAll<TileView>("Tiles"));
            var settings = ServiceLocator.Get<Settings>();
            foreach (var unitPrefab in settings.EnemyUnits.Values)
            {
                _unitPrefabsPerName.Add(unitPrefab.name, unitPrefab);
            }
            foreach (var unitPrefab in settings.PlayerUnits.Values)
            {
                _unitPrefabsPerName.Add(unitPrefab.name, unitPrefab);
            }
        }
    }
}