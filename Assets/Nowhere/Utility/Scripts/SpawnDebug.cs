using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NowhereUnity.Utility{

	///<summary>SpawnDebug</summary>
	///<remarks>
	///Use this for control objects in a scene.
	///</remarks>
	public class SpawnDebug : MonoBehaviour {
		#region Instance
			#region Fields
                [SerializeField]GameObject  m_prefab;
                [SerializeField]int         m_count = 10;
                [SerializeField]int         m_arrayX = 10;
                [SerializeField]float       m_arrayMargin = 5f;
			#endregion

			#region Properties
			#endregion

			#region Events
				///<summary>
				///Use this for initialization.
				///</summary>
				void Start () {
					for(int i=0; i<m_count; i++)
                    {
                        var offset = new Vector3((float)(i%m_arrayX), 0f, (float)(i/m_arrayX));
                        offset *= m_arrayMargin;

                        Instantiate(m_prefab, transform.position + offset, Quaternion.identity);
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