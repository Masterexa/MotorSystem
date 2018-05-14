using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace NowhereUnity.Movement {

    public enum PlayerMoveType {
        Strafe,
        Camera
    }

    [System.Serializable]
    public class PlayerMoveKeySettings {

        #region Fields
            [SerializeField]PlayerMoveType  m_moveType      = PlayerMoveType.Camera;
            [SerializeField]string          m_verticalKey   = "Vertical";
            [SerializeField]string          m_horizontalKey = "Horizontal";
            [SerializeField]string          m_sprintKey     = "Sprint";
        #endregion

        #region Properties
            public string verticalKey {
                get { return m_verticalKey; }
            }

            public string horizontalKey {
                get { return m_horizontalKey; }
            }

            public string sprintKey {
                get { return m_sprintKey; }
            }
        #endregion


        #region Methods
            public Vector3 GetVelocity(Camera cam) {
                var move        = new Vector3();
                var sprint      = CrossPlatformInputManager.GetAxisRaw(this.sprintKey);
                var velocity    = new Vector3();

                move.x  = CrossPlatformInputManager.GetAxis(this.horizontalKey);
                move.y  = CrossPlatformInputManager.GetAxis(this.verticalKey);

                if( cam )
                {
                    velocity = cam.transform.right*move.x + cam.transform.forward*move.y;
                }
                else
                {
                    velocity = move;
                }
                velocity.Normalize();
                velocity *= (1f + sprint);

                return velocity;
            }
        #endregion
    }



    ///<summary>PlayerMotorController</summary>
    ///<remarks>
    ///Use this for control objects in a scene.
    ///</remarks>
    public class PlayerMotorController : MonoBehaviour {

		#region Instance
			#region Fields
                [SerializeField]PlayerMoveKeySettings   m_key = new PlayerMoveKeySettings();

                IMotorMoving    m_moving;
                IMotorJumping   m_jumping;
			#endregion

			#region Properties
			#endregion

			#region Events
				///<summary>
				///Use this for initialization.
				///</summary>
				void Awake() {
					m_moving    = GetComponent<IMotorMoving>();
                    m_jumping   = GetComponent<IMotorJumping>();
				}
	
				///<summary>
				/// Update is called once per frame
				///</summary>
				void Update () {
					OnKeyTranslation();
				}
			#endregion

			#region Pipeline
                void OnKeyTranslation() {
                    if( m_moving!=null )
                    {
                        var velocity = m_key.GetVelocity(Camera.main);

                        m_moving.Move(velocity, Space.World, MotorAmountSpace.Relative);
                    }
                    if( m_jumping!=null )
                    {
                        m_jumping.Jump(CrossPlatformInputManager.GetAxisRaw("Jump"), MotorAmountSpace.Absolute);
                    }
                }
			#endregion

			#region Methods
			#endregion
		#endregion
	}
}