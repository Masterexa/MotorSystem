using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NowhereUnity.Movement{



	///<summary>HumanoidMotor</summary>
	///<remarks>
	///Use this for control objects in a scene.
	///</remarks>
	public class HumanoidMotor : MonoBehaviour, IMotorMoving, IMotorLooking, IMotorJumping {


        struct MoveData {
            public float    jump;
            public Vector3  moveLocal;
            public Vector3  lookDirection;
        }


        static int  GroundMultiplyId;
        static int  ForwardRunSpeedId;

		#region Instance
			#region Fields
                [SerializeField]float   m_speed = 1f;
                [SerializeField]float   m_sprintSpeed = 3f;
                [SerializeField]float   m_jumpAddLimit = 3f;

                [SerializeField]bool    m_useRootMotion;
                bool        m_rootMotionEnabled = true;
                Animator    m_animator;

                // Collision
                [SerializeField]float   m_groundOffset = 0.1f;
                CapsuleCollider         m_capsuleCollider;

                // State
                int     m_lastCheckFrame    = 0;
                bool    m_isGrounded        = true;
                Vector3 m_groundHitNormal   = Vector3.up;  

                // Move query
                Vector3             m_velocity;

                // Jump query
                float               m_jumpPower;
                float               m_jumpAccePower;
                float               m_jumpAddTimer;

                // Look query
                float   m_lookFactor = 0f;
                Vector3 m_lookDirection;

                // Rigid body
                Rigidbody   m_rigidBody3;
			#endregion

			#region Properties
			#endregion

			#region Events
				///<summary>
				///Use this for initialization.
				///</summary>
				void Awake() {
					m_rigidBody3        = GetComponent<Rigidbody>();
                    m_capsuleCollider   = GetComponent<CapsuleCollider>();
                    m_animator          = GetComponent<Animator>();

                    GroundMultiplyId    = Animator.StringToHash("GroundMultiply");
                    ForwardRunSpeedId   = Animator.StringToHash("ForwardRunSpeed");
				}
	
				///<summary>
				/// Update is called once per frame
				///</summary>
				void FixedUpdate () {
                    if( Time.fixedDeltaTime>0f )
                    {
                        MoveData    data = new MoveData();

                        //GroundCheckProcess();
                        VectorTransformProcess(ref data);
                        LookProcess(ref data);
                        MoveProcess(ref data);
                        JumpProcess(ref data);        
                    }
				}

                private void OnAnimatorMove() {
                    if( !(m_useRootMotion && m_rootMotionEnabled) )
                    {
                        return;
                    }
                    m_rigidBody3.MovePosition( m_rigidBody3.position + m_animator.deltaPosition );

                    //m_rigidBody3.AddForce(m_animator.deltaPosition/Time.deltaTime,ForceMode.VelocityChange);
                }
            #endregion

            #region Pipeline
                void GroundCheckProcess() {
                    if( m_lastCheckFrame==Time.frameCount )
                    {
                        return;
                    }

                    RaycastHit  hit;
                    Vector3     lossyScale  = transform.lossyScale;

                    Vector3 origin      = transform.TransformPoint(m_capsuleCollider.center);
                    Vector3 direction   = -transform.up;
                    Vector3 halfExtend  = 0.8f*m_capsuleCollider.radius * Vector3.one;

                    // Raycast
                    m_isGrounded = Physics.BoxCast(
                        origin, halfExtend, Vector3.down, out hit, transform.rotation,
                        m_capsuleCollider.height*.5f - halfExtend.y + m_groundOffset,
                        Physics.AllLayers, QueryTriggerInteraction.UseGlobal
                    );

                    //m_isGrounded = Physics.Raycast(origin, direction, out hit, m_capsuleCollider.height*.5f, Physics.AllLayers, QueryTriggerInteraction.UseGlobal);


                    if( m_isGrounded )
                    {
                        m_groundHitNormal = hit.normal;
                    }
                    else
                    {
                        m_groundHitNormal = transform.up;
                    }

                    // Record frame
                    m_lastCheckFrame = Time.frameCount;

                    /*
                    // Debug draw
                    var gizmoColor      = Color.green;
                    var gizmoHitColor   = m_isGrounded ? gizmoColor : Color.red;
                    Debug.DrawLine(origin, hit.point, gizmoColor);
                    Debug.DrawRay(hit.point, transform.right, gizmoHitColor);*/
                }

                void VectorTransformProcess(ref MoveData data) {
                    
                    data.moveLocal = m_velocity;

                    // Direction transform
                    if( !(m_lookFactor>0f) )
                    {
                        data.lookDirection = data.moveLocal;
                    }
                    else
                    {
                        data.lookDirection  = m_lookDirection;
                    }
                    data.lookDirection.y = 0f;
                    data.lookDirection.Normalize();
                }

                void MoveProcess(ref MoveData data) {
                    float   speed   = data.moveLocal.magnitude;
                    
                    // Clamp speed of Y and correct scale
                    if( (speed>0f) )
                    {
                        data.moveLocal.y    = 0f;
                        data.moveLocal      *= speed / m_velocity.magnitude;
                    }

                    // Animator update
                    if( m_animator )
                    {
                        m_animator.SetFloat("Forward", data.moveLocal.z, 0.1f, Time.deltaTime);
                        m_animator.SetFloat("Right", data.moveLocal.x, 0.1f, Time.deltaTime);
                    
                        float gsp = Mathf.Max(1f, speed/m_animator.GetFloat(ForwardRunSpeedId));
                        m_animator.SetFloat(GroundMultiplyId, gsp );
                    }

                    // Add force
                    if( !(m_useRootMotion) )
                    {
                        m_rigidBody3.MovePosition(m_rigidBody3.position + transform.TransformDirection(data.moveLocal)*Time.deltaTime);
                    }

                    // Airborne
                    if( !m_isGrounded )
                    {
                        m_rigidBody3.AddRelativeForce(data.moveLocal*Time.deltaTime, ForceMode.VelocityChange);
                    }
                }

                void JumpProcess(ref MoveData data) {

                    GroundCheckProcess();
                    if( m_animator )
                    {
                        m_animator.SetBool("isGrounded", m_isGrounded);
                        m_rootMotionEnabled = m_isGrounded;

                        if( m_isGrounded )
                        {
                            m_animator.SetFloat("Fall", m_rigidBody3.velocity.y);
                        }
                    }


                    // drag
                    m_rigidBody3.drag = m_isGrounded ? 5f : 0f;

                    // Ground Check
                    if( m_isGrounded )
                    {
                        if( (m_jumpPower>0f) )
                        {
                            m_rigidBody3.AddRelativeForce(0f,m_jumpPower,0f,ForceMode.VelocityChange);
                            m_rigidBody3.AddRelativeForce(data.moveLocal, ForceMode.VelocityChange);

                            m_jumpAddTimer = m_jumpAddLimit;
                        }
                        m_jumpPower = 0f;
                    }
                    else if( m_jumpAddTimer>0f )
                    {
                        m_jumpPower     += m_jumpAccePower*Time.deltaTime;
                        m_rigidBody3.AddRelativeForce(0f,m_jumpPower,0f,ForceMode.VelocityChange);

                        m_jumpAddTimer = (m_jumpAccePower>0f) ? (m_jumpAddTimer-Time.deltaTime) : 0f;
                        m_jumpAccePower = 0f;
                    }
                }

                void LookProcess(ref MoveData data) {
                    if( !(data.lookDirection.sqrMagnitude>0f) )
                    {
                        return;
                    }
                    float turnSpeed     = 360f;
                    float turnAmount    = Mathf.Atan2(data.lookDirection.x, data.lookDirection.z) * turnSpeed * Time.deltaTime;

                    transform.Rotate(0f,turnAmount,0f,Space.Self);
                    data.moveLocal = Quaternion.AngleAxis(-turnAmount, Vector3.up)*data.moveLocal;

                    if( m_animator )
                    {
                        m_animator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
                    }
                }
			#endregion

			#region Methods
                public void Move(Vector3 velocity, Space directionSpace, MotorAmountSpace velocitySpace) {

                    if( velocitySpace==MotorAmountSpace.Relative )
                    {
                        float sp        = velocity.magnitude;
                        float deltaSp   = m_speed * sp;

                        if( sp>1f )
                        {
                            deltaSp = sp * 0.5f * m_sprintSpeed;
                        }
                        if( sp>0f )
                        {
                            velocity *= deltaSp/sp;
                        }
                    }
                    if( directionSpace==Space.World )
                    {
                        velocity = transform.InverseTransformDirection(velocity);
                    }

                    m_velocity  = velocity;
                }

                public void Jump(float power, MotorAmountSpace powerSpace) {
            
                    if( m_isGrounded )
                        m_jumpPower     = power;
                    m_jumpAccePower     = power;
                }

                public void SetLookDirection(Vector3 direction, float factor, Space directionSpace) {

                    if( directionSpace==Space.World )
                    {
                        direction = transform.InverseTransformDirection(direction);
                    }
                    m_lookDirection = direction;
                    m_lookFactor    = factor;
                }
            #endregion
        #endregion
    }
}