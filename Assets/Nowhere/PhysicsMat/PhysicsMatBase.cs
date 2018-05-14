using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NowhereUnity.Movement {



    public class PhysicsMatBase : MonoBehaviour {

        
        public class PoolElement {
            public GameObject   gameObject;
            public float        timer;

            public PoolElement(PhysicsMaterialEx material, Transform parent) {
                gameObject = Instantiate(material.bulletHitDecal, Vector3.zero, Quaternion.identity, parent);
            }

            public void Spawn(Vector3 worldPos, Vector3 normal) {
                gameObject.SetActive(true);
                gameObject.transform.position = worldPos;
                gameObject.transform.rotation = Quaternion.LookRotation(normal);
                timer = 2f;
            }

            public void Update() {
                if( gameObject.activeSelf )
                {
                    timer -= Time.deltaTime;
                    if(timer<0f)
                    {
                        gameObject.SetActive(false);
                    }
                }
            }
        }


		#region Instance
			#region Fields
                public PhysicsMaterialEx   material;

                List<PoolElement>   m_decalPool = new List<PoolElement>();
                int                 m_index=0;
            #endregion

            #region Properties
            #endregion

            #region Events
                private void Awake() {
                    if(!material || !material.bulletHitDecal)
                    {
                        return;
                    }
                    for(int i=0,end=128; i<end; i++)
                    {
                        m_decalPool.Add(new PoolElement(material,transform));
                    }
                }

                private void Update() {
                    foreach(var it in m_decalPool)
                    {
                        it.Update();
                    }
                }
            #endregion

            #region Pipelines
            #endregion

            #region Methods
                public void SpawnDecal(Vector3 worldPos, Vector3 normal) {

                    if(!material || !material.bulletHitDecal)
                        return;

                    m_decalPool[m_index].Spawn(worldPos,normal);
                    m_index++;
                    if( m_index==m_decalPool.Count )
                    {
                        m_index=0;
                    }
                }
			#endregion
		#endregion
	}

}