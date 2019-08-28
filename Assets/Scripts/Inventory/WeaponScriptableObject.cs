using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Retake
{
    public class WeaponScriptableObject : ScriptableObject
    {
        public List<Weapon> weapons_all = new List<Weapon>();
        public List<WeaponStats> weaponStats = new List<WeaponStats>();
    }
}
