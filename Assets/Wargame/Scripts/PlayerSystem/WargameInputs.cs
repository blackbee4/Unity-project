using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace WargameSystem.PlayerSystem
{
    [RequireComponent(typeof(PlayerInput))]
    public class WargameInputs : MonoBehaviour
    {

        [Header("Character Input Values")]
        public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;
        public bool fire;
        public bool reload;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;

        public void OnMove(InputValue value)
        {
            move = value.Get<Vector2>();
        }

        public void OnLook(InputValue value)
        {
            look = value.Get<Vector2>();
        }

        public void OnJump(InputValue value)
        {
            jump = value.isPressed;
        }

        public void OnSprint(InputValue value)
        {
            sprint = value.isPressed;
        }

        public void OnFire(InputValue value)
        {
            fire = value.isPressed;
        }

        public void OnReload(InputValue value)
        {
            reload = value.isPressed;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}