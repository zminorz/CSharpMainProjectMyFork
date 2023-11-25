using UnityEngine;

namespace Model.Config
{
    public class UnitConfig : MonoBehaviour
    {
        [SerializeField] private Sprite _icon;
        [SerializeField] private int _cost;
        [SerializeField] private int _maxHealth;
        
        public Sprite Icon => _icon;
        public int Cost => _cost;
        public int MaxHealth => _maxHealth;
    }
}