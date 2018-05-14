using UnityEngine;
using NowhereUnity.Movement;
using System;

namespace NowhereUnity.Movement {

    [System.Serializable]
    public class GeneralCapacityModel : MotorCapacityModel {

        #region Instance
            #region Fields
                public PhysicsMaterialEx    defaultPhysicsMaterial = null;
                public bool useAnimator  = true;
                public bool spendStamina = false;
            #endregion

            #region Overrides
                public GeneralCapacityModel() : base() {}

                public override void Reset() {
                    useAnimator = true;
                    defaultPhysicsMaterial = null;
                }
            #endregion
        #endregion
    }
}
