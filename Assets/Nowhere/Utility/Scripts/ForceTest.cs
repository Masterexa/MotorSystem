using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NowhereUnity.Utility{

    public enum ForceFieldMode {
        Wind,
        Explosion
    }

	///<summary>ForceTest</summary>
	///<remarks>
	///Use this for control objects in a scene.
	///</remarks>
	public class ForceTest : MonoBehaviour {
		#region Instance
			#region Fields
                [SerializeField]ForceFieldMode  m_mode;
                [SerializeField]Vector3     m_velocity  = Vector3.forward*5f;
                [SerializeField]ForceMode   m_forceMode = ForceMode.Impulse;
                [SerializeField]float       m_explosionRadius = 5f;
                [SerializeField]float       m_delayTime = 5f;

                float   m_delay;
            #endregion

            #region Properties
            #endregion

            #region Events
                private void FixedUpdate() {
                    if( m_delay < 0f )
                    {
                        m_delay = m_delayTime;
                    }
                    m_delay -= Time.deltaTime;
                }

                private void OnTriggerStay(Collider other){

                    if( other.attachedRigidbody )
                    {
                        switch(m_mode)
                        {
                            case ForceFieldMode.Wind:
                                other.attachedRigidbody.AddForce(m_velocity,m_forceMode);
                                break;
                            case ForceFieldMode.Explosion:
                                if( m_delay<0f )
                                {
                                    other.attachedRigidbody.AddExplosionForce(m_velocity.magnitude, transform.position, m_explosionRadius,0f, m_forceMode);
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }

                private void OnDrawGizmosSelected() {

                    //Gizmos.matrix   = transform.localToWorldMatrix;
                    Vector3 to  = transform.position + m_velocity;

                    Gizmos.color    = Color.green;
                    Gizmos.DrawLine(transform.position, to);

                    Gizmos.matrix = Matrix4x4.TRS(
                        to, Quaternion.LookRotation(-m_velocity.normalized), Vector3.one
                    );
                    Gizmos.DrawFrustum(Vector3.zero, 60f, m_velocity.magnitude*0.1f, 0f, 1f);
                }
            #endregion

            #region Pipeline
            #endregion

            #region Methods
            #endregion
        #endregion
    }
}