using UI;
using UnityEngine;
using Weapons;

namespace Player {
    public enum CharacterState {
        Default,
        Dodging,
        Dead
    }
    
    public class PlayerController : MonoBehaviour {
        public CharacterState state = CharacterState.Default;
        public BaseStats playerStats;
        public Transform weaponFollowPoint;
        public Crosshair crosshair;
        public BaseWeapon currentWeapon; 

        private Vector2 m_MovementVector;
        private Rigidbody2D m_Rigidbody2D;
        private float m_DodgingDuration = 0;
        private Vector2 m_DodgeVector;
        private bool m_WeaponLeft = true;

        #region Unity Events
        private void Start() {
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate() {
            // Do movement stuff in here
            UpdateMovement(m_MovementVector);
        }
        #endregion

        private void ChangeState(CharacterState newState) {
            OnLeaveState(state, newState);
            switch (newState) {
                case CharacterState.Dead:
                case CharacterState.Default:
                case CharacterState.Dodging:
                    state = newState;
                    break;
            }
        }

        private void OnLeaveState(CharacterState oldState, CharacterState newState) {
            // In case we want to do something when a state is left
            // Like movement speed on ending a dodge, etc
        }
        
        public void SetInputs(ref CharacterInputs inputs) {
            if (state == CharacterState.Dead) {
                return;
            }

            m_MovementVector = inputs.moveAxis;
            if (inputs.moveAxis.x > 0 && !m_WeaponLeft) {
                m_WeaponLeft = true;
                Vector3 followPoint = weaponFollowPoint.localPosition;
                followPoint.x = -followPoint.x;
                weaponFollowPoint.localPosition = followPoint;
            } else if (inputs.moveAxis.x < 0 && m_WeaponLeft) {
                m_WeaponLeft = false;
                Vector3 followPoint = weaponFollowPoint.localPosition;
                followPoint.x = -followPoint.x;
                weaponFollowPoint.localPosition = followPoint;
            }

            if (inputs.dodge && state == CharacterState.Default) {
                ChangeState(CharacterState.Dodging);
                m_DodgeVector = inputs.moveAxis;
            }
            
            crosshair.AimCrosshair(inputs.aimVector);
            if (inputs.fire) {
                currentWeapon.Fire(crosshair.transform.position);
            }
        }

        private void UpdateMovement(Vector2 moveVector) {
            Vector2 movement = (moveVector * (playerStats.movementSpeed * Time.deltaTime));
            if (m_DodgingDuration < playerStats.dodgeTime && state == CharacterState.Dodging) {
                m_DodgingDuration += Time.deltaTime;
                movement = (m_DodgeVector * (playerStats.dodgeSpeed * Time.deltaTime));
            } else if (state == CharacterState.Dodging){
                ChangeState(CharacterState.Default);
                m_DodgingDuration = 0;
            }
            
            m_Rigidbody2D.MovePosition(m_Rigidbody2D.position + movement);
        }
    }
}
