using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NowhereUnity.Movement;

namespace NowhereUnity.Movement {

    public class MotorBase : MonoBehaviour {

        #region Instance
            #region Fields

            #endregion

            #region Propeties
        #endregion

            #region Events
                private void Awake() {
                    OnAwake();
                }

                protected virtual void OnAwake() {}
            #endregion

            #region Overrides
                public virtual void MoveTo(Vector3 worldPos, float speedLevel) {}

                public virtual void Move(Vector3 move, float speedLevel) {}

                public virtual void StrafeRelative(Vector3 move) {}

                public virtual void Jump() {

                }
            #endregion

            #region Methods
            #endregion
        #endregion

    }
}
