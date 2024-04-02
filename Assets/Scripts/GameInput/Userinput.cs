using System;
using Unity.VisualScripting;
using UnityEngine;

namespace SpaceInvaders.GameInput
{
    public class UserInput : MonoBehaviour
    {
        public event Action OnFirePressed;
        public event Action OnLeftPressed;
        public event Action OnRightPressed;

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                OnFirePressed?.Invoke();
            }

            if(Input.GetKey(KeyCode.A))
            {
                OnLeftPressed?.Invoke();
            }

            if(Input.GetKey(KeyCode.D))
            {
                OnRightPressed?.Invoke();
            }
            
        }

    }
}