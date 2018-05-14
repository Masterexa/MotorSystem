using UnityEngine;
using NowhereUnity.Movement;
using System;

namespace NowhereUnity.Movement {

    [System.Serializable]
    public class OffMeshAnimationCapacityModel : MotorCapacityModel {
        

        #region Instance
            #region Fields
                public AnimationClip    superJump       = null;
                public AnimationClip    jumpDownClip    = null;
            #endregion

            #region Overrides
                public OffMeshAnimationCapacityModel() : base() {}

                public override void Reset() {
                }
            #endregion
        #endregion
    }
}
