using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using NowhereUnity.Movement;
using UnityEngine.AI;
using NowhereUnity.Utility;
using NowhereUnity.Projects.OW.Myst;
using NowhereUnity.Audio;

namespace NowhereUnity.Movement{


    /*
     * interface IStrafeHandler{
     *  void Strafe(Vector3 dir)
     * }
     * 
     * interface I
     * 
     */


    /// <summary>
    /// 人型キャラクターモーター
    /// </summary>
    [
        RequireComponent(typeof(Rigidbody)),
        RequireComponent(typeof(CapsuleCollider))
    ]
    public class HumanoidMotorLegacy : RigidBodyMotor,IStrafeHandler,ILockDirectionHandler,IMotorMoving{

        public enum LookAtRotation {
            HeadOnly,
            Spine,
            Body
        }

        enum StepSoundType {
            Step,
            Land,
            Jump
        }


        #region Instance
            #region Fields
                [SerializeField]MotorCapacity   m_capacity;

                [SerializeField]AudioSource             m_stepSource;
                [SerializeField]HeadBobCinemachine      m_headBob;
                [SerializeField]CollisionCapacityModel  m_collisionCap = new CollisionCapacityModel();

                protected Animator          m_animator;
                protected CapsuleCollider   m_capsuleCollider;
                protected NavMeshAgent      m_navAgent;

                protected int       m_lastGroundCheck   = 0;
                protected bool      m_isJumping;
                protected bool      m_isCrouching;
                protected bool      m_isGrounded    = true;
                protected Vector3   m_groundNormal  = Vector3.up;
                private Vector3     m_lookDir       = Vector3.forward;
                private float       m_turnAmount    = 0f;
                
                Vector3 m_moveVelocity;


                protected PhysicsMaterialEx m_currentPhysMat;

                bool    m_jump;
                bool    m_crouch;
                float   m_stepTime;
                int     m_steps;
            #endregion

            #region Prop
                public MotorCapacity capacity {
                    get {return m_capacity;}
                    set {m_capacity = value;}
                }

                public bool isCrouching {
                    get {return m_isCrouching;}
                }

                public bool     isSprint    {get; set;}
                public Vector3  moveAmount  {get; set;}

                public bool isGrounded {
                    get {
                        return m_isGrounded;
                    }
                }

                public MatchTargetWeightMask offMeshMTWeightMask {get;set;}

                public Vector3 motorLookDirection {
                    get {
                        return m_lookDir;
                    }
                    set {
                        m_lookDir = value;
                    }
                }

                public bool motorAutomaticLook { get; set; }
        #endregion

            #region Events
                private void Reset() {
                    var rd = GetComponent<Rigidbody>();
                    if( rd )
                    {
                        rd.constraints  = RigidbodyConstraints.FreezeRotation;
                        rd.mass         = 10f;
                        rd.drag         = 1f;
                    }
                }

                protected override void OnAwake() {
                    base.OnAwake();
                    m_capsuleCollider   = GetComponent<CapsuleCollider>();
                    m_animator          = GetComponent<Animator>();
                    m_navAgent          = GetComponent<NavMeshAgent>();
                    m_currentPhysMat    = capacity.general.defaultPhysicsMaterial;
                }

                private void Start() {
                    if( m_navAgent )
                    {
                        m_navAgent.updateRotation   = false;
                        m_navAgent.updatePosition   = false;
                        m_navAgent.speed            = 0f;
                    }
                    m_collisionCap.standHeight = m_capsuleCollider.height;
                }

                private void FixedUpdate() {

                    CheckGround(false);

                    // 移動プロセス
                    MoveProcess();
                    CrouchProcess();
                    JumpProcess();
                    SetRotation(motorLookDirection);
                    ApplyExtraTurnRotation();
                    
                    UpdateAnimator();
                }

                private void OnAnimatorMove() {

                    MoveProcessRoot();
                }

                private void OnDrawGizmosSelected() {
            
                    if( Application.isPlaying && m_capsuleCollider )
                    {
                        float   dist    = m_capsuleCollider.height*.5f - m_capsuleCollider.radius + capacity.walking.groundCheckDistance;
                        var     src     = transform.position + m_capsuleCollider.center;
                        var     dst     = src + Vector3.down*dist;
                        var     r       = m_capsuleCollider.radius*2f;
                        var     rw      = r*0.8f;

                        Gizmos.color = Color.yellow;
                        Gizmos.DrawWireSphere(src, 0.01f);

                        Gizmos.color = Color.green;
                        Gizmos.DrawLine(src, dst);
                        //Gizmos.DrawWireSphere(dst, m_capsuleCollider.radius);
                        Gizmos.DrawWireCube(dst, new Vector3(rw,r,rw));
                    }
                }
            #endregion

            #region Pipeline Methods
                public void CheckGroundStatus() {
                    RaycastHit  hit;
                    bool        prevGnd = m_isGrounded;
                    float       r = m_capsuleCollider.radius;
                    float       w = 0.8f;

                    /*
                    if( Physics.SphereCast(
                        transform.position + m_capsuleCollider.center, m_capsuleCollider.radius, Vector3.down, out hit,
                        (m_capsuleCollider.height*0.5f - m_capsuleCollider.radius) + m_groundCheckDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore
                    ))*/
                    if( Physics.BoxCast(transform.position+m_capsuleCollider.center, new Vector3(r*w,r,r*w), -transform.up, out hit, transform.rotation,
                        m_capsuleCollider.height*.5f - r + capacity.walking.groundCheckDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore
                    )){
                        m_groundNormal              = hit.normal;
                        m_isGrounded                = true;
                        if( m_animator )
                        {
                            m_animator.applyRootMotion  = true;
                        }
                    }
                    else
                    {
                        m_groundNormal              = Vector3.up;
                        m_isGrounded                = false;
                        if( m_animator )
                        {
                            m_animator.applyRootMotion  = false;
                        }
                    }
                }

                void ApplyExtraTurnRotation() {
                    float ts = Mathf.Lerp(capacity.walking.stationaryTurnSpeed, capacity.walking.movingTurnSpeed, moveAmount.z);
                    transform.Rotate(0f, m_turnAmount*ts*Time.deltaTime, 0f);
                }
        
                void UpdateAnimator()   {
                    if( m_animator )
                    {
                        var sprintAmount = 2f;

                        m_animator.SetFloat("Forward", moveAmount.z * sprintAmount, 0.1f, Time.deltaTime);
                        m_animator.SetFloat("Right", moveAmount.x * sprintAmount, 0.1f, Time.deltaTime);
                        m_animator.SetFloat("Turn", m_turnAmount, 0.1f, Time.deltaTime);
                        m_animator.SetBool("Grounded", m_isGrounded);
                        m_animator.SetBool("Crouching", m_isCrouching);
                        m_animator.SetFloat("Jump", m_rd3.velocity.y);
                    }
                }

                void MoveProcessRoot() {
                    if( (Time.deltaTime==0f) )
                    {
                        return;
                    }
                    var sprintThisFrame = isSprint;
                    var forceMode       = (m_isGrounded) ? ForceMode.VelocityChange : capacity.airborne.airControlForceMode;


                    if( capacity.walking.useRootMotion )
                    {
                        var vd = (m_animator.deltaPosition);

                        //m_rd3.AddForce(v*10f, forceMode);
                        m_rd3.velocity = vd / Time.deltaTime;
                    }
                }

                void MoveProcess() {
                    if( (Time.deltaTime==0f) )
                    {
                        return;
                    }
                    var sprintThisFrame = isSprint;
                    var forceMode       = (m_isGrounded) ? ForceMode.VelocityChange : capacity.airborne.airControlForceMode;


                    if( !capacity.walking.useRootMotion ){
                        float cts = capacity.walking.CalclateCTS(moveAmount,m_isCrouching,sprintThisFrame);// current target speed
                

                        if( moveAmount==Vector3.zero )
                        {
                            return;
                        }
                        
                        // add force
                        if( m_rd3.velocity.sqrMagnitude < cts*cts )
                        {
                            m_rd3.AddRelativeForce(moveAmount*cts*SlopeMultiplier(), forceMode);
                        }
                    }
                }

                void CrouchProcess() {

                    var offset = m_capsuleCollider.center;

                    if( m_crouch && m_isGrounded )
                    {
                        m_capsuleCollider.height    = m_collisionCap.crouchHeight;
                        m_isCrouching               = true;
                    }
                    else if( m_isCrouching )
                    {
                        RaycastHit  hit;
                        if( !Physics.SphereCast(transform.position+m_capsuleCollider.center, m_capsuleCollider.radius, transform.up, out hit,
                            (m_capsuleCollider.height*0.5f - m_capsuleCollider.radius), Physics.AllLayers, QueryTriggerInteraction.Ignore))
                        {
                            m_capsuleCollider.height = m_collisionCap.standHeight;
                            m_isCrouching = false;
                        }
                    }
                    offset.y                    = m_capsuleCollider.height*0.5f;
                    m_capsuleCollider.center    = offset;
                }

                void JumpProcess() {
                    // Jump process
                    if(m_isGrounded)
                    {
                        m_rd3.drag  = capacity.walking.standDragValue;

                        // if you want do jump
                        if(m_jump && !(m_isCrouching && !m_crouch))
                        {
                            m_rd3.AddForce(0f, capacity.airborne.jumpPower * SlopeMultiplier(), 0f, capacity.airborne.jumpForceMode);

                            if( !m_isJumping )
                            {
                                m_isJumping = true;
                            }
                        }

                        // stationary
                        if(!m_jump && (moveAmount.magnitude<float.Epsilon) && (m_rd3.velocity.magnitude<1f))
                        {
                            m_rd3.Sleep();
                            m_isJumping = false;
                        }
                    }
                    else
                    {
                        m_rd3.drag = capacity.airborne.airDragValue;
                    }
                    m_jump = false;
                }
            #endregion

            #region Movement Methods
                public float SlopeMultiplier() {
                    float angle = Vector3.Angle(m_groundNormal, transform.up);
                    return capacity.walking.slopeCurveModifier.Evaluate(angle);
                }

                public override void MoveTo(Vector3 worldPos, float speedLevel) {

                    bool moved = false;

                    if( m_navAgent )
                    {
                        m_navAgent.SetDestination(worldPos);

                        if( (speedLevel>0f) && (m_navAgent.remainingDistance>m_navAgent.stoppingDistance) )
                        {
                            m_navAgent.nextPosition = transform.position;
                            Move(m_navAgent.desiredVelocity.normalized, speedLevel);

                            moved = true;
                        }
                        else
                        {
                            m_navAgent.speed            = 0f;
                        }
                    }

                    if( !moved )
                    {
                        Move(Vector3.zero,0f);
                    }
                }

                public void Move(Vector3 velocity, Space directionSpace, MotorAmountSpace speedSpace) {

                    float length = velocity.magnitude; // normalization : V*length

                    m_moveVelocity = velocity;
                    if( directionSpace==Space.World )
                    {
                        m_moveVelocity = transform.InverseTransformDirection(m_moveVelocity);
                    }
                }

                public override void Move(Vector3 move, float speedLevel) {

                    bool    is_sprint   = (speedLevel>1f);
                    float   amount      = is_sprint ? (speedLevel/2f) : speedLevel;
            
                    CheckGround(false);

                    if(move.sqrMagnitude > 1f)
                    {
                        move.Normalize();
                    }
                    isSprint        = is_sprint;
                    var moveRel     = transform.InverseTransformDirection(move);
                    moveRel         = Vector3.ProjectOnPlane(moveRel, m_groundNormal);
                    m_turnAmount    = Mathf.Atan2(moveRel.x, moveRel.z);
                    moveAmount      = new Vector3(0f,0f,moveRel.z*amount);
                
                    if( m_navAgent )
                    {
                        m_navAgent.speed = Mathf.Max(capacity.walking.CalclateCTS(moveAmount,false,is_sprint),0.01f);
                    }
                    
                }
        
                public void Strafe(Vector3 move) {

                    StrafeRelative(transform.InverseTransformDirection(move));
                }

                public override void StrafeRelative(Vector3 dir) {

                    CheckGround(false);
                    dir.Normalize();
                    dir         = Vector3.ProjectOnPlane(dir, m_groundNormal);
                    moveAmount  = new Vector3(dir.x,0f,dir.z);
                }
                
                public void SetRotation(Vector3 dir) {
                    var moveRel     = transform.InverseTransformDirection(dir).normalized;
                    moveRel         = Vector3.ProjectOnPlane(moveRel, m_groundNormal);
                    m_turnAmount    = Mathf.Atan2(moveRel.x, moveRel.z);
                }

                public override void Jump() {
                    m_jump = true;
                }
            #endregion

            #region Methods
                public void CheckGround(bool forceCheck) {
                    if(m_lastGroundCheck==Time.frameCount)
                    {
                        return;
                    }
                    m_lastGroundCheck = Time.frameCount;
                    CheckGroundStatus();
                }
            #endregion
        #endregion

        #region Editor
#if UNITY_EDITOR
        [ContextMenu("Add", false)]
        void Add()
        {
            var cap = ScriptableObject.CreateInstance<MotorCapacity>();
            cap.name    = "MotorCapacity";
            m_capacity  = cap;
            UnityEditor.AssetDatabase.AddObjectToAsset(cap, UnityEditor.AssetDatabase.GetAssetPath(gameObject));

            UnityEditor.AssetDatabase.SaveAssets();
        }

        [ContextMenu("Add", true)]
        bool AddValidate()
        {
            var path = UnityEditor.AssetDatabase.GetAssetPath(gameObject);
            bool isPrefab = UnityEditor.PrefabUtility.GetPrefabType(gameObject) == UnityEditor.PrefabType.Prefab;
            var hasCap = System.Array.Exists(UnityEditor.AssetDatabase.LoadAllAssetsAtPath(path), (it) => (it is MotorCapacity));

            return isPrefab && !hasCap;
        }

        [ContextMenu("Remove", false)]
        void Remove()
        {
            var path = UnityEditor.AssetDatabase.GetAssetPath(gameObject);
            var cap = System.Array.Find(UnityEditor.AssetDatabase.LoadAllAssetsAtPath(path), (it) => (it is MotorCapacity));

            UnityEngine.Object.DestroyImmediate(cap, true);
            m_capacity  = null;

            UnityEditor.AssetDatabase.SaveAssets();
        }

        [ContextMenu("Remove", true)]
        bool RemoveValidate()
        {
            var path = UnityEditor.AssetDatabase.GetAssetPath(gameObject);
            bool isPrefab = UnityEditor.PrefabUtility.GetPrefabType(gameObject) == UnityEditor.PrefabType.Prefab;
            var hasCap = System.Array.Exists(UnityEditor.AssetDatabase.LoadAllAssetsAtPath(path), (it) => (it is MotorCapacity));

            return isPrefab && hasCap;
        }
#endif
        #endregion
    }
}

