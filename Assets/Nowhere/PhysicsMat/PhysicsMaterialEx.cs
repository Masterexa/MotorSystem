using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NowhereUnity.Audio;

namespace NowhereUnity.Movement {

    [CreateAssetMenu(menuName = "Scriptable/PhysicsMaterialEx")]
    public class PhysicsMaterialEx : ScriptableObject {

        #region Instance
            #region Fields
                [Header("Sounds")]
                public SoundPreset  hitSound;
                public SoundPreset  footstepSound;
                public SoundPreset  jumpSound;
                public SoundPreset  landSound;
                [Header("Particles")]
                public GameObject   landParticle;
                public GameObject   bulletHitDecal;
            #endregion

            #region Events
                // Use this for initialization
                void OnEnable() {

                }
            #endregion
        #endregion
    }
}