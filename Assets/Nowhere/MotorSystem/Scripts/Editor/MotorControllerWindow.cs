using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NowhereUnity.Movement;
using NowhereUnity.Movement;
using UnityEditor;

namespace NowhereUnityEditor.Movement{

	///<summary>NewBehaviourScript</summary>
	///<remarks>
	///Use this for control objects in a scene.
	///</remarks>
	public class MotorControllerWindow : EditorWindow{

        [MenuItem("Nowhere/Motor tool")]
        static void CreateWindow(){
            EditorWindow.GetWindow<MotorControllerWindow>();
        }

        #region Instance
            #region Fields
                [SerializeField]MotorBase   m_motor;
        #endregion

            #region Properties
            #endregion

            #region Events
                private void OnEnable() {
                    this.titleContent = new GUIContent("Motor Control");
                }

                private void OnGUI() {
                    EditorGUI.BeginDisabledGroup(!m_motor);
                    {/*
                        if( GUILayout.Button("↑") )
                            m_motor.StrafeRelative(Vector3.forward,false,false);
                        EditorGUILayout.BeginHorizontal();
                        {
                            if( GUILayout.Button("←") )
                                m_motor.StrafeRelative(Vector3.left,false,false);
                            if( GUILayout.Button("→") )
                                m_motor.StrafeRelative(Vector3.right,false,false);
                        }
                        EditorGUILayout.EndHorizontal();
                        if( GUILayout.Button("↓") )
                            m_motor.StrafeRelative(Vector3.back,false,false);

                        if( GUILayout.Button("Stop") )
                            m_motor.StrafeRelative(Vector3.zero,false,false);*/
                    }
                    EditorGUI.EndDisabledGroup();
                }

                private void OnSelectionChange() {
                    if( Selection.activeGameObject )
                    {
                        m_motor = Selection.activeGameObject.GetComponent<MotorBase>();
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