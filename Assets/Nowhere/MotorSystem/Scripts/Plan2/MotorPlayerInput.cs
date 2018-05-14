using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace NowhereUnity.Movement {

    ///<summary>MotorPlayerInput</summary>
    ///<remarks>
    ///Use this for control objects in a scene.
    ///</remarks>
    public class MotorPlayerInput : MonoBehaviour {

		#region Instance
			#region Fields
            #endregion

            #region Properties
                public PlayerInputDelegates.OnMove          onMove { get; set; }
                public PlayerInputDelegates.OnSprinting     onSprinting { get; set; }
                public PlayerInputDelegates.OnJumpStart     onJumpStart { get; set; }
                public PlayerInputDelegates.OnJumpHovering onJumpHovering { get; set; }

                // States
                public bool isSprinting { get; set; }
            #endregion

            #region Events
                private void Awake() {
                }

                ///<summary>
                /// Update is called once per frame
                ///</summary>
                void Update () {

                    var move = new Vector3(
                        CrossPlatformInputManager.GetAxis("Horizontal"),
                        CrossPlatformInputManager.GetAxis("Vertical"),
                        0f
                    );
                    var jumpStart       = CrossPlatformInputManager.GetButtonDown("Jump");
                    var jumpHovering    = CrossPlatformInputManager.GetButton("Jump");
                    this.isSprinting    = CrossPlatformInputManager.GetButton("Sprint");
                    
                    // Moving
                    if( onMove!=null )
                    {
                        onMove.Invoke(move);
                    }
                    if( onSprinting!=null && this.isSprinting)
                    {
                        onSprinting.Invoke();
                    }

                    // Jumping
                    if( onJumpStart!=null && jumpStart)
                    {
                        onJumpStart.Invoke();
                    }
                    if( onJumpHovering!=null && jumpHovering)
                    {
                        onJumpHovering.Invoke();
                    }
				}
			#endregion

			#region Pipeline
			#endregion

			#region Methods
			#endregion
		#endregion
	}
}