using UnityEngine;
using System.Collections;
using Masterexa;

namespace NowhereUnity.Audio{

	///<summary>
	/// SoundPreset
	///</summary>
	///
	///@date	#CREATIONDATE#
	public class SoundPreset : ScriptableObject {

		#if UNITY_EDITOR
		/* Editor */
			/* Methods */
				/// <summary>
				/// Create a Scriptable instance.
				/// </summary>
				[UnityEditor.MenuItem("Assets/Create/Scriptable/SoundPreset")]
				public static void CreateAsset()
				{
					UnityEditor.ProjectWindowUtil.CreateAsset (ScriptableObject.CreateInstance<SoundPreset> (), typeof(SoundPreset) + ".asset");
				}
		#endif


		/* Inner class */
			[System.Serializable]
			public class ClipPreset
			{
				public AudioClip	clip;
				public bool 		loop;
			}

            public static void PlayOneshot(AudioSource src, SoundPreset sp) {

                if( src&&sp )
                {
                    sp.PlayThisOneshot(src,true);
                }
                else{
                    Debug.LogWarning("Audio source or SoundPreset is null.");
                }
            }


		/* Instance */
			/* Parameters */
				public AudioClip[]	clips;
				public bool			loop;
				public bool			usePitch = false;
				public FloatBound	pitchRange = new FloatBound(1.0f,1.0f);


			/* Methods */
			#region Methods
				/// <summary>
				/// Select sound randomry.
				/// </summary>
				/// <returns>The select.</returns>
				public AudioClip RandomSelect()
				{
					if (clips == null)
					{
						return null;
					}

					return clips[Random.Range (0, clips.Length)];
				}


				public void PlayThisOneshot(AudioSource dest, bool overridePitch, float volumeScale=1f)
				{
					if (dest==null) {
						return;
					}

                    if(overridePitch)
                    {
                        if(usePitch)
                        {
                            dest.pitch = Random.Range(pitchRange.min, pitchRange.max);
                        }
                        else
                        {
                            dest.pitch = 1f;
                        }
                    }

                    dest.PlayOneShot(RandomSelect());
				}
			#endregion
	}


    public static class SoundPresetExtend {


        public static void PlayOneShot(this AudioSource src, SoundPreset preset, float volumeScale=1f, bool overridePitch=true) {

            if( preset )
            {
                preset.PlayThisOneshot(src, overridePitch, volumeScale);
            }
        }
    }
}