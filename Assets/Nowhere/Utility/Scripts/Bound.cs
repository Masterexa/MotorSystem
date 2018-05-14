using UnityEngine;
using System.Collections;

namespace Masterexa
{
    [System.Serializable]
    public struct FloatBound
    {
        public float min;
        public float max;

        public FloatBound(float mi, float ma) { min = mi; max = ma; }
    }

    [System.Serializable]
    public struct IntBound
    {
        public int min;
        public int max;

        public IntBound(int mi, int ma) { min = mi; max = ma; }
    }
}
