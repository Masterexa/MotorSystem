using NowhereUnity.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NowhereUnity.Movement {
    public class PhysicsMatExSetter : MonoBehaviour {

        #region Instance
            #region Fields
                public PhysicsMaterialEx    material;
                public bool freeOnExit=true;
            #endregion

            #region Events
                private void OnTriggerEnter(Collider other) {
            
                    var ply = other.GetComponent<PhysicsMatExActor>();
                    if( ply )
                    {
                        ply.customMaterial = material;
                    }
                }

                private void OnTriggerExit(Collider other) {
            
                    if( !freeOnExit )
                        return;

                    var ply = other.GetComponent<PhysicsMatExActor>();
                    if( ply )
                    {
                        ply.customMaterial = null;
                    }
                }
            #endregion
        #endregion
    }

}