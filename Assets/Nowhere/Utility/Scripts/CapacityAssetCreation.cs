using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NowhereUnity.Utility{


    public static class CapacityAssetCreation {
        
        public static void AddToPrefab<T>(GameObject obj,ref T dst, string name) where T : ScriptableObject{
        #if UNITY_EDITOR
            var cap     = ScriptableObject.CreateInstance<T>();
            cap.name    = name;
            dst         = cap;

            AssetDatabase.AddObjectToAsset(cap,obj);
            AssetDatabase.SaveAssets();
        #endif
        }

        public static void RemoveFromPrefab<T>(GameObject obj, ref T dst) where T:ScriptableObject{
        #if UNITY_EDITOR
            var path    = AssetDatabase.GetAssetPath(obj);
            var cap     = AssetDatabase.LoadAllAssetsAtPath(path).ToList()
                          .Find((it)=>it is T);

            UnityEngine.Object.DestroyImmediate(cap,true);
            dst = null;
            AssetDatabase.SaveAssets();
        #endif
        }

        public static bool HasCapacity<T>(GameObject obj) where T : ScriptableObject {
        #if UNITY_EDITOR
            var     path        = AssetDatabase.GetAssetPath(obj);
            bool    isPrefab    = PrefabUtility.GetPrefabType(obj)==PrefabType.Prefab;
            bool    hasCap      = AssetDatabase.LoadAllAssetsAtPath(path).ToList()
                                  .Exists((it)=>it is T);

            return isPrefab && hasCap;
        #else
            return false;
        #endif
        }
    }
}
