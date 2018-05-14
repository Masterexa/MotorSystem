using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NowhereUnity.Utility {

    public class WaitForAnimationState : CustomYieldInstruction {

        Animator    m_animator;
        int         m_layer;
        int         m_lastState;

        public WaitForAnimationState(Animator animator, int layer) {
            m_animator  = animator;
            m_layer     = layer;
        }

        public void Ready() {
            m_lastState = m_animator.GetCurrentAnimatorStateInfo(m_layer).fullPathHash;
        }

        public override bool keepWaiting {
            get {
                var current = m_animator.GetCurrentAnimatorStateInfo(m_layer);
                return (current.fullPathHash == m_lastState) && (current.normalizedTime < 1f);
            }
        }
    }
}
