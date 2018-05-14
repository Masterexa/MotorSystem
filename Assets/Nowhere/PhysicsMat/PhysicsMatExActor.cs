using NowhereUnity.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NowhereUnity.Movement {

    [RequireComponent(typeof(Rigidbody))]
    public class PhysicsMatExActor : PhysicsMatBase{

        public enum ActorType {
            RigidBody,
            Character
        }

        enum PlayType {
            Footstep,
            Jump,
            Land
        }

        #region Instance
            #region Fields
                [SerializeField]ActorType           m_type;
                [SerializeField]float               m_minSpeed=5f;
                [SerializeField]float               m_stepInterval=3f;
                [SerializeField]AudioSource         m_source;

                bool                m_isPlaying=false;
                Rigidbody           m_rd3;

                float       m_stepCycle, m_nextStep;
                PlayType    m_playType;
            #endregion

            #region Propeties
                public PhysicsMaterialEx customMaterial{
                    get; set;
                }

                public PhysicsMaterialEx current {
                    get {
                        return customMaterial ? customMaterial : material;
                    }
                }

                public float stepInterval {
                    get {return m_stepInterval;}
                }
            #endregion

            #region Events
                private void Awake() {
                    m_stepCycle = 0f;
                    m_nextStep  = m_stepInterval;

                    m_rd3           = GetComponent<Rigidbody>();
                    //m_source        = (!m_source) ? gameObject.AddComponent<AudioSource>() : m_source;
                }

                private void OnCollisionEnter(Collision collision) {
                    if( m_rd3 && m_type!=ActorType.RigidBody || !material)
                        return;
                    if( (collision.relativeVelocity.sqrMagnitude > m_minSpeed*m_minSpeed) && m_source )
                        SoundPreset.PlayOneshot(m_source, material.hitSound );
                }

                private void LateUpdate() {
            
                    if( !(m_type == ActorType.Character) || !current )
                    {
                        return;
                    }

                    switch(m_playType)
                    {
                        case PlayType.Footstep:
                        {
                            if( m_stepCycle < m_nextStep )
                            {
                                break;
                            }
                            m_nextStep = m_stepCycle + m_stepInterval;
                            SoundPreset.PlayOneshot(m_source, current.footstepSound);
                        break;
                        }
                        case PlayType.Jump:
                            SoundPreset.PlayOneshot(m_source, current.jumpSound);
                            break;
                        case PlayType.Land:
                            SoundPreset.PlayOneshot(m_source, current.landSound);
                            break;
                        default:
                            break;
                    }
                    m_playType = PlayType.Footstep;
                }
            #endregion

            #region Methods
                public void AddStepCycle(float value) {

                    m_stepCycle += value*Time.deltaTime;
                }

                public void PlayJumpSound() {
                    m_playType = PlayType.Jump;
                }

                public void PlayLandSound() {
                    m_playType = PlayType.Land;
                }
            #endregion
        #endregion
    }
}
