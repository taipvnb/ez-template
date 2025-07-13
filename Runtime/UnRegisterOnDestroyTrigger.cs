using System.Collections.Generic;
using UnityEngine;

namespace com.ez.engine.unregister
{
    public class UnRegisterOnDestroyTrigger : MonoBehaviour
    {
        private readonly List<IUnRegister> _unRegisters = new List<IUnRegister>();

        public void AddUnRegister(IUnRegister unRegister)
        {
            _unRegisters.Add(unRegister);
        }

        public void RemoveUnRegister(IUnRegister unRegister)
        {
            _unRegisters.Remove(unRegister);
        }

        private void OnDestroy()
        {
            foreach (var unRegister in _unRegisters)
            {
                unRegister.UnRegister();
            }

            _unRegisters.Clear();
        }
    }
}