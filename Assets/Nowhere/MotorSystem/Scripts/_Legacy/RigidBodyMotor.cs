using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace  NowhereUnity.Movement{

	///<summary>RigidBodyMotor</summary>
	///<remarks>
	///Use this for control objects in a scene.
	///</remarks>
	public class RigidBodyMotor : MotorBase{
		#region Instance
			#region Fields
                // Component
                protected Rigidbody     m_rd3;
			#endregion

			#region Properties
            #endregion

            #region Events
                protected override void OnAwake()
                {
                    base.OnAwake();
                    m_rd3 = GetComponent<Rigidbody>();
                }
            #endregion

            #region Pipeline
            #endregion

            #region Methods
            #endregion
        #endregion
    }
}