using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LD_46
{
    public class Utilities
    {
        public static float NextGaussian()
        {
            float v1, v2, s;
            do
            {
                v1 = 2.0f * UnityEngine.Random.Range(0f, 1f) - 1.0f;
                v2 = 2.0f * UnityEngine.Random.Range(0f, 1f) - 1.0f;
                s = v1 * v1 + v2 * v2;
            } while (s >= 1.0f || s == 0f);

            s = Mathf.Sqrt((-2.0f * Mathf.Log(s)) / s);

            return v1 * s;
        }


    }
}
