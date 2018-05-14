using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NowhereUnity.Movement {

    public class PhysicsMatExObject : PhysicsMatBase{

        #region Instance
            #region Fields
                public bool     materialSetter;
                public bool     setOnTrigger;
                public bool     unsetOnExit=true;
            #endregion

            #region Events
                private void OnCollisionEnter(Collision collision) {
            
                    if( materialSetter )
                        SetMaterialToActor(collision.collider, true);
                }

                private void OnCollisionExit(Collision collision) {

                    if(materialSetter && unsetOnExit)
                        SetMaterialToActor(collision.collider, false);
                }

                private void OnTriggerEnter(Collider other) {

                    Terrain tr;

                    if(materialSetter && setOnTrigger)
                        SetMaterialToActor(other, true);
                }

                private void OnTriggerExit(Collider other) {

                    if(materialSetter && setOnTrigger && unsetOnExit)
                        SetMaterialToActor(other, false);
                }

                void SetMaterialToActor(Collider obj, bool active) {

                    var act = obj.GetComponent<PhysicsMatExActor>();
                    if( act )
                    {
                        act.customMaterial = active ? material : null;
                    }
                }
            #endregion

            #region Methods
            #endregion
        #endregion
    }

}