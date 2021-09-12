using Camera;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
    public struct CharacterInputs {
        public Vector2 moveAxis;
        public bool fire;
        public bool dodge;
        public bool interact;
        public bool switchWeapon;
        public Vector2 aimVector;
    }

    public class InputValues {
        public Vector2 moveAxis = Vector2.zero;
        public bool fire = false;
        public bool dodge = false;
        public bool interact = false;
        public bool switchWeapon = false;
        public Vector2 aimVector = Vector2.zero;
    }
    
    public class InputHandler : MonoBehaviour {
        public PlayerCamera playerCamera;
        public PlayerController playerController;

        private InputValues m_InputValues = new InputValues();
        private bool m_IsPlayerControllerNull;

        private void Start() {
            m_IsPlayerControllerNull = playerController == null;
        }

        private void Update() {
            if (m_IsPlayerControllerNull) {
                return;
            }
            
            HandleCharacterInput();
        }
        
        private void HandleCharacterInput() {
            CharacterInputs inputs = new CharacterInputs {
                moveAxis = m_InputValues.moveAxis,
                fire = m_InputValues.fire,
                dodge = m_InputValues.dodge,
                interact = m_InputValues.interact,
                switchWeapon = m_InputValues.switchWeapon,
                aimVector = m_InputValues.aimVector,
            };
            
            playerController.SetInputs(ref inputs);

            m_InputValues.dodge = false;
            m_InputValues.interact = false;
            m_InputValues.switchWeapon = false;
        }
        
        #region inputs
        public void OnMove(InputValue input) {
            Vector2 move = input.Get<Vector2>();
            m_InputValues.moveAxis = input.Get<Vector2>();
        }

        public void OnLook(InputValue input) {
            m_InputValues.aimVector = input.Get<Vector2>();
        }

        public void OnInteract(InputValue input) {
            m_InputValues.interact = true;
        }

        public void OnSwitchWeapon(InputValue input) {
            m_InputValues.switchWeapon = true;
        }

        public void OnFire(InputValue input) {
            m_InputValues.fire = input.Get<float>() > 0.5f;
        }

        public void OnDodge(InputValue input) {
            m_InputValues.dodge = true;
        }
        #endregion
    }
}
