using System;
using UnityEngine;

namespace Utilities
{
    public class TimeUtil : MonoBehaviour
    {
        public static TimeUtil Create()
        {
            var go = new GameObject("TimeUtil");
            DontDestroyOnLoad(go);
            return go.AddComponent<TimeUtil>();
        }
        
        private Action<float> _updateAction;
        private Action<float> _fixedUpdateAction;
        
        public void AddUpdateAction(Action<float> action)
        {
            _updateAction += action;
        }
        
        public void RemoveUpdateAction(Action<float> action)
        {
            _updateAction -= action;
        }
        
        public void AddFixedUpdateAction(Action<float> action)
        {
            _fixedUpdateAction += action;
        }
        
        public void RemoveFixedUpdateAction(Action<float> action)
        {
            _fixedUpdateAction -= action;
        }
        
        private void Update()
        {
            _updateAction?.Invoke(Time.deltaTime);
        }
        
        private void FixedUpdate()
        {
            _fixedUpdateAction?.Invoke(Time.fixedDeltaTime);
        }
        
        private void OnDestroy()
        {
            _updateAction = null;
            _fixedUpdateAction = null;
        }
    }
}