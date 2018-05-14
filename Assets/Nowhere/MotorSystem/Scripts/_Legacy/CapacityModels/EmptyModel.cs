using UnityEngine;
using NowhereUnity.Movement;
using System;

namespace NowhereUnity.Movement {

    [System.Serializable]
    public class EmptyCapacityModel : MotorCapacityModel {


        #region Instance
            #region Fields
            #endregion

            #region Overrides
                public EmptyCapacityModel() : base() {}

                public override void Reset() {
                    // write default values
                }
            #endregion
        #endregion
    }
}
