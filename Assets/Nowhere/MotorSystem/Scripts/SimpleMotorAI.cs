using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NowhereUnity.Movement{

    ///<summary>SimpleMotorAI</summary>
    ///<remarks>
    ///Use this for control objects in a scene.
    ///</remarks>
    
    public class SimpleMotorAI : MonoBehaviour{
		#region Instance
			#region Fields
                IMotorMoving    m_move;
                IMotorJumping   m_jump;
                [SerializeField]float   m_speed  = 5f;
                [SerializeField]float   m_radius = 10f;
                [SerializeField]bool    m_doJump = false;
                [SerializeField]float   m_jumpFrequencyMax = 5f;

                float   m_time  =0f;
                float   m_sign  =1f;

                float   m_timeOfJump = -1f;
			#endregion

			#region Properties
			#endregion

			#region Events
				///<summary>
				///Use this for initialization.
				///</summary>
				void Start () {
                    m_move  = GetComponent<IMotorMoving>();
					m_jump  = GetComponent<IMotorJumping>();
                    m_time  = Time.time;
                    m_sign  = (Random.Range(0,2)%2==0) ? 1f : -1f;
				}
	
				///<summary>
				/// Update is called once per frame
				///</summary>
				void Update () {

                    m_time          += Time.deltaTime*m_speed*m_sign / m_radius;
                    m_timeOfJump    -= Time.deltaTime;

					if( m_move!=null )
                    {
                        m_move.Move(new Vector3(Mathf.Cos(m_time), 0f, Mathf.Sin(m_time)), Space.World, MotorAmountSpace.Absolute);
                    }
                    if( m_doJump && m_jump!=null && (m_timeOfJump<0f) )
                    {
                        m_jump.Jump(5f, MotorAmountSpace.Absolute);
                        m_timeOfJump = Random.Range(0f, m_jumpFrequencyMax);
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