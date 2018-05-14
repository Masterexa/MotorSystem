using UnityEngine;
using System.Collections;

namespace NowhereUnity.Movement {

    public class HumanoidPlayerPresenter : MonoBehaviour {

        #region Instance
            #region Fields
                HumanoidMotor       m_motor;
                MotorPlayerInput    m_input;
            #endregion

            #region Events
                private void Awake() {
                    m_motor = GetComponent<HumanoidMotor>();

                    m_input = GetComponent<MotorPlayerInput>();
                    m_input.onMove          += OnPlayerMove;
                    m_input.onJumpStart     += OnJumpStart;
                    m_input.onJumpHovering  += OnJumpHovering;
                }

                private void OnDestroy() {
                    m_input.onMove          -= OnPlayerMove;
                    m_input.onJumpStart     -= OnJumpStart;
                    m_input.onJumpHovering  -= OnJumpHovering;
                }

                void OnPlayerMove(Vector3 move) {
                    var velocity    = new Vector3();
                    var cam         = Camera.main.transform;
                    
                    velocity = cam.right*move.x + cam.forward*move.y;
                    
                    // Clamp the length between 0 to 1.
                    float length = velocity.magnitude;
                    if( length>0f )
                    {
                        float sprint = m_input.isSprinting ? 1f : 0f;
                        velocity *= (Mathf.Clamp01(length)/length) * (1f + sprint);
                    }

                    m_motor.Move(velocity, Space.World, MotorAmountSpace.Relative);
                }

                void OnJumpStart() {
                    m_motor.Jump(1f, MotorAmountSpace.Absolute);
                }

                void OnJumpHovering() {
                    m_motor.Jump(1f, MotorAmountSpace.Absolute);
                }
            #endregion
        #endregion
    }
}