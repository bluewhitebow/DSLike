using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Retake
{
    public static class StaticCalculations
    {
        public static int CalculateBaseDamage(WeaponStats w, CharacterStats st,float mutiplier = 1)
        {
            float physical = (w.physical * mutiplier) - st.physical;
            float strike = (w.strike * mutiplier) - st.vs_strike;
            float slash = (w.slash * mutiplier) - st.vs_slash;
            float thrust = (w.thrust * mutiplier) - st.vs_thrust;

            float sum = physical + strike + slash + thrust;

            float magic = (w.magic * mutiplier) - st.magic;
            float fire = (w.fire * mutiplier )- st.fire;
            float lighting = (w.lighting * mutiplier) - st.lighting;
            float dark = (w.dark * mutiplier) - st.dark;

            sum += magic + fire + lighting + dark;

            if (sum <= 0)
                sum = 1;

            return Mathf.RoundToInt(sum);
        }

    }
}
