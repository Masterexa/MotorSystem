using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NowhereUnity.Movement{

	///<summary>MotorTest</summary>
	///<remarks>
	///Use this for control objects in a scene.
	///</remarks>
	public class MotorTest : Movement3DMotor{
        #region Instance
            #region Fields
                [SerializeField]float   m_speed=1f;

                Rigidbody   m_rd3;
            #endregion

            #region Properties
            #endregion

            #region Events
                private void Start()
                {
                    m_rd3 = GetComponent<Rigidbody>();
                }

                private void FixedUpdate(){
                    var input   = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                    var eyeR    = Camera.main.transform.right;
                    var eyeF    = Vector3.Scale(Camera.main.transform.forward, new Vector3(1f,0f,1f));
                    moveQueryVelociy = (eyeR*input.x + eyeF*input.y).normalized * m_speed;


                    m_rd3.AddForce(moveQueryVelociy);
                }
            #endregion

            #region Pipeline
            #endregion

            #region Methods
            #endregion
        #endregion
    }
}