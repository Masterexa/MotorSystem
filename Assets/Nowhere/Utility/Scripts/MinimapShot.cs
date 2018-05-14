using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NowhereUnity.Utility{

    [RequireComponent(typeof(Camera)), ExecuteInEditMode]
	public class MinimapShot : MonoBehaviour {
        

		#region Instance
			#region Fields
                public bool             saveWithRectData=true;
                [HideInInspector]
                public Camera           myCamera;
                public RenderTexture    texture;
			#endregion

			#region Properties
			#endregion

			#region Events
				// Use this for initialization
				void Start () {
				    myCamera    = GetComponent<Camera>();
                    texture     = myCamera.targetTexture;
				}
			#endregion

			#region Pipelines
			#endregion

			#region Methods
			#endregion
		#endregion
	}

#if UNITY_EDITOR
    [CustomEditor(typeof(MinimapShot))]
    public class MinimapShotEdior : Editor {

        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            if( GUILayout.Button("Shot Map Texture") )
            {
                ShotMapTexture();
            }
        }



        void ShotMapTexture() {
            var     ms          = target as MinimapShot;
            var     cam         = ms.myCamera;
            var     rt          = cam.targetTexture;
            var     shotTex     = new Texture2D(rt.width,rt.height,TextureFormat.RGB24,false);
            var     prevActive  = RenderTexture.active;
            bool    isSaveRect  = ms.saveWithRectData;
            var     path        = string.Empty;


            path = EditorUtility.SaveFilePanel("Save texture as PNG", "", "MapTexture.png", "png");
            if( path.Length!=0 )
            {
                cam.Render();
                RenderTexture.active = rt;
                shotTex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
                RenderTexture.active = prevActive;

                var bytes = shotTex.EncodeToPNG();

                System.IO.File.WriteAllBytes(path,bytes);
            }
        }
    }

#endif
}