using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NowhereUnity.Utility{

	///<summary>VisualMemo</summary>
	///<remarks>
	///Use this for control objects in a scene.
	///</remarks>
	public class VisualMemo : MonoBehaviour {

        [System.Serializable]
        public class ModelMemo {
            public bool     enabled;
            public bool     wireframe;
            public Color32  color = Color.blue;
            public Mesh     mesh;
        }

		#region Instance
			#region Fields
                [SerializeField]ModelMemo   m_model = new ModelMemo();
            #endregion

            #region Properties
            #endregion

            #region Events
                private void OnDrawGizmos() {
                    DrawMemo();
                }

                void DrawMemo() {
                    if( m_model.enabled && m_model.mesh )
                    {
                        Gizmos.color = m_model.color;
                        if( m_model.wireframe )
                        {
                            Gizmos.DrawWireMesh(m_model.mesh, transform.position, transform.rotation, transform.lossyScale);
                        }
                        else{
                            Gizmos.DrawMesh(m_model.mesh, transform.position, transform.rotation, transform.lossyScale);
                        }
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