using UnityEngine;
using NowhereUnity.Movement;
using System;

namespace NowhereUnity.Movement {

    [System.Serializable]
    public class CollisionCapacityModel : MotorCapacityModel {
        
        #region Instance
            #region Fields
                [HideInInspector]
                public float    standHeight;
                public float    crouchHeight;
            #endregion

            #region Overrides
                public CollisionCapacityModel() : base() {}

                public override void Reset() {
                    standHeight  = 1.5f;
                    crouchHeight = 1.2f;
                }
            #endregion
        #endregion
    }
}
