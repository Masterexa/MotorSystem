using UnityEngine;
using NowhereUnity.Movement;
using System;

namespace NowhereUnity.Movement {

    [System.Serializable]
    public class AirborneCapacityModel : MotorCapacityModel {
        
        #region Instance
            #region Fields
                [Header("Air Control")]
                public float        airDragValue;
                public float        airControlSpeed;
                public ForceMode    airControlForceMode;

                [Header("Jump")]
                public float        jumpPower;
                public ForceMode    jumpForceMode;
                public float        jumpStaminaCost = 10f;
            #endregion

            #region Overrides
                public AirborneCapacityModel() : base(){}

                public override void Reset() {
                    airDragValue    = 1;
                    airControlSpeed = 1;
                    airControlForceMode = ForceMode.Acceleration;

                    jumpPower       = 4f;
                    jumpForceMode   = ForceMode.VelocityChange;
                    jumpStaminaCost = 10f;
                }
            #endregion
        #endregion
    }
}
