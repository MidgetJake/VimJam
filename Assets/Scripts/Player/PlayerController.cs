using Assets.Scripts.Controller;
using Camera;
using Items;
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
        public static PlayerController player;
        public CharacterState state = CharacterState.Default;
        public BaseStats playerStats;
        public Transform weaponFollowPoint;
        public Crosshair crosshair;
        public BaseWeapon currentWeapon;
        public BaseWeapon defaultWeapon;
        public BaseWeapon extraWeapon;
        public Animator animator;
        public bool holdingDefault = true;
        
        private Vector2 m_MovementVector;
        private Rigidbody2D m_Rigidbody2D;
        private float m_DodgingDuration = 0;
        private Vector2 m_DodgeVector;
        private bool m_WeaponLeft = true;
        private BaseInteractable m_CurrentInteractable;
        private BaseStats m_StartingStats;
        private static readonly int m_MoveDir = Animator.StringToHash("WalkDir");
        private static readonly int m_Roll = Animator.StringToHash("Rolling");

        #region Unity Events
        private void Start() {
            player = this;
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            m_StartingStats = playerStats;
        }

        private void FixedUpdate() {
            // Do movement stuff in here
            UpdateMovement(m_MovementVector);
            DetectDistanceFromEnemies();
        }
        #endregion

        private void ChangeState(CharacterState newState) {
            OnLeaveState(state, newState);
            switch (newState) {
                case CharacterState.Dead:
                case CharacterState.Default:
                    state = newState;
                    break;
                case CharacterState.Dodging:
                    playerStats.staminaRegenTime = 0;
                    animator.SetBool(m_Roll, true);
                	Audio.controller.Dodge(transform.position);
                    state = newState;
                    break;
            }
        }

        private void OnLeaveState(CharacterState oldState, CharacterState newState) {
            // In case we want to do something when a state is left
            // Like movement speed on ending a dodge, etc
            switch (oldState) {
                case CharacterState.Dodging:
                    animator.SetBool(m_Roll, false);
                    break;
                case CharacterState.Default:
                    break;
                case CharacterState.Dead:
                    break;
                default:
                    break;
            }
        }
        
        public void SetInputs(ref CharacterInputs inputs) {
            if (state == CharacterState.Dead) {
                return;
            }

            m_MovementVector = inputs.moveAxis;

            if (inputs.moveAxis.x > 0) {
                animator.SetInteger(m_MoveDir, 1);
            } else if (inputs.moveAxis.x < 0) {
                animator.SetInteger(m_MoveDir, 3);
            } else if (inputs.moveAxis.y > 0) {
                animator.SetInteger(m_MoveDir, 2);
            } else if (inputs.moveAxis.y < 0) {
                animator.SetInteger(m_MoveDir, 0);
            }

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
                if (playerStats.currStamina > 0) {
                    playerStats.currStamina--;
                    ChangeState(CharacterState.Dodging);
                    m_DodgeVector = inputs.moveAxis;
                }
            }
            
            crosshair.AimCrosshair(inputs.aimVector);
            if (inputs.fire && currentWeapon != null) {
                currentWeapon.Fire(crosshair.transform.position);
            }

            if (inputs.interact && m_CurrentInteractable != null) {
                m_CurrentInteractable.Interact(this);
            }

            if (inputs.switchWeapon) {
                ChangeWeapon();
            }
        }

        public void PickupWeapon(BaseWeapon weapon, bool isDefault = false) {
            weapon.firePoint = transform;
            weapon.followTransform = weaponFollowPoint;

            if (isDefault) {
                defaultWeapon = weapon;
                currentWeapon = weapon;
                AmmoCount.main.PickupWeapon(ref defaultWeapon, true);
                return;
            }
            
            if (holdingDefault) {
                extraWeapon = weapon;
            } else {
                currentWeapon = weapon;
                extraWeapon = weapon;
            }
            
            AmmoCount.main.PickupWeapon(ref extraWeapon, false);
        }

        public void ResetPlayer() {
            playerStats = m_StartingStats;
            state = CharacterState.Default;
        }
        
        private void ChangeWeapon() {
            if (extraWeapon == null) {
                return;
            }
            
            if (holdingDefault) {
                holdingDefault = false;
                defaultWeapon = currentWeapon;
                currentWeapon = extraWeapon;
            } else {
                holdingDefault = true;
                extraWeapon = currentWeapon;
                currentWeapon = defaultWeapon;
            }
            AmmoCount.main.SwapWeapon();
        }
        
        public void OnEnterInteractable(BaseInteractable interactable) {
            m_CurrentInteractable = interactable;
        }

        public void OnExitInteractable(BaseInteractable interactable) {
            m_CurrentInteractable = null;
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
        
        private void DetectDistanceFromEnemies() {
            Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, 10, 1 << LayerMask.NameToLayer("Ignore Raycast"));
            if (collisions.Length == 0) { BackgroundAudio.controller.IntensityAudioVolume(null); return; }

            float? closestDistance = null;
            foreach (var enemy in collisions) {
                if (!enemy.CompareTag("Enemy")) { continue; }
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (!closestDistance.HasValue || distance < closestDistance) { closestDistance = distance; }
            }

            BackgroundAudio.controller.IntensityAudioVolume(closestDistance);
        }
    }
}
