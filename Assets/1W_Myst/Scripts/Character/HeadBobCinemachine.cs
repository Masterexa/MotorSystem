using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cinemachine;
using System;

namespace NowhereUnity.Projects.OW.Myst{

    ///<summary>HeadBobCinemachine</summary>
    ///<remarks>
    ///Use this for control objects in a scene.
    ///</remarks>
    public class HeadBobCinemachine : CinemachineExtension{

        /// <summary>
        /// 仮想カメラごとの状態
        /// </summary>
        class VcamExtraState {
        }


        #region Instance
            #region Fields
                public float           blurMultiply            = 1f;
                public AnimationCurve  positionYNoise          = new AnimationCurve(new Keyframe(0f,0f), new Keyframe(0.5f,0.05f), new Keyframe(1f,0f), new Keyframe(1.5f,0.05f), new Keyframe(2f,0f));
                public AnimationCurve  rotationYNoise          = new AnimationCurve(new Keyframe(0f,0f), new Keyframe(0.5f,0.15f), new Keyframe(1.5f,-0.15f), new Keyframe(2f,0f));
                public AnimationCurve  rotationZNoise          = new AnimationCurve(new Keyframe(0f,0f), new Keyframe(0.5f,0.1f), new Keyframe(1.5f,-0.1f), new Keyframe(2f,0f));
            #endregion

            #region Properties
                public float stepTime { get; set; }
            #endregion

            #region Events
                private void LateUpdate() {
                }

                protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime) {
                    
                    //var extra = GetExtraState<VcamExtraState>(vcam);
                    if( !enabled || deltaTime<0 )
                    {
                        return;
                    }

                    if( stage==CinemachineCore.Stage.Noise )
                    {
                        state.PositionCorrection =
                            new Vector3(0f, positionYNoise.Evaluate(stepTime) * blurMultiply, 0f)
                        ;

                        state.OrientationCorrection =
                            Quaternion.Euler(0f, rotationYNoise.Evaluate(stepTime) * blurMultiply, rotationZNoise.Evaluate(stepTime) * blurMultiply)
                        ;
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