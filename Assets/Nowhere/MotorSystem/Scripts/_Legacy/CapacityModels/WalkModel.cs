using UnityEngine;
using NowhereUnity.Movement;
using System;

namespace NowhereUnity.Movement {

    [System.Serializable]
    public class WalkingCapacityModel : MotorCapacityModel {
        
        #region Instance
            #region Fields
                [Header("Root Motion")]
                public bool useRootMotion;
                public bool modifySpeedOnRootMotion;

                [Header("Speed")]
                public float    baseSpeed;
                public float    forwardSpeed;
                public float    backSpeed;
                public float    strafeSpeed;
                public float    sprintSpeed;
                public float    crouchSpeed;
                public float    walkStepLength;
                public float    sprintStepLength;
                [Range(0,360)]
                public float    stationaryTurnSpeed;
                [Range(0,360)]
                public float    movingTurnSpeed;

                [Header("Ground")]
                public float            standDragValue;
                public float            groundCheckDistance;
                public AnimationCurve   slopeCurveModifier;

                [Header("Misc")]
                public float    sprintStaminaCost = 10f;
            #endregion

            #region Overrides
                public WalkingCapacityModel() : base() {}

                public override void Reset() {
                    useRootMotion = false;
                    modifySpeedOnRootMotion = false;

                    baseSpeed           = 1;
                    forwardSpeed        = 1;
                    strafeSpeed         = 1;
                    backSpeed           = 1;
                    sprintSpeed         = 2;
                    crouchSpeed         = 0.5f;
                    stationaryTurnSpeed = 360f;
                    movingTurnSpeed     = 180f;
                    walkStepLength      = 1f;
                    sprintStepLength    = 2f;

                    standDragValue      = 5f;
                    groundCheckDistance = 0.1f;
                    slopeCurveModifier  = new AnimationCurve(new Keyframe(-90f, 1f, 0f, 0f), new Keyframe(0f, 1f, 0f, 0f), new Keyframe(90f, 0f, 0f, 0f));

                    sprintStaminaCost = 10f;
                }

                public float CalclateCTS(Vector3 moveAmount, bool crouch, bool sprint) {
                
                    float cts = 0f;
                    float mul = 1f;

                    if( Mathf.Abs(moveAmount.x)>0f )
                    {
                        cts = this.strafeSpeed;
                    }
                    if( moveAmount.z<0f )
                    {
                        cts = this.backSpeed;
                    }
                    if( moveAmount.z>0f )
                    {
                        cts = this.forwardSpeed;
                    }
                    mul = sprint ? this.sprintSpeed : 1f;
                    mul = crouch ? this.crouchSpeed : mul;
                    mul *= this.baseSpeed;

                    return cts*mul*moveAmount.magnitude;
                }
            #endregion
        #endregion
    }
}
