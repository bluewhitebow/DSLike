using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Retake
{
    public class ResourcesManager: MonoBehaviour
    {
        Dictionary<string, int> Spell_ids = new Dictionary<string, int>();
        Dictionary<string, int> Weapon_ids = new Dictionary<string, int>();
        Dictionary<string, int> Weaponstats_ids = new Dictionary<string, int>();
        Dictionary<string, int> consum_ids = new Dictionary<string, int>();

        public static ResourcesManager singleton;

        //Init
        void Awake()
        {
            singleton = this;
            LoadWeapomIds();
            LoadSpellIds();
            LoadConsumables();
        }

        void LoadSpellIds()
        {
            SpellItemScriptableObject obj = Resources.Load("Retake.SpellItemScriptableObject") as SpellItemScriptableObject;

            if (obj == null)
            {
                Debug.Log("Retake.SpellItemScriptableObject could not be loaded!");
                return;
            }

            for(int i = 0;i < obj.spell_Items.Count;i++)
            {
                if(Spell_ids.ContainsKey(obj.spell_Items[i].itemName))
                {
                    Debug.Log(obj.spell_Items[i].itemName + "item is a duplicate");
                }
                else
                {
                    Spell_ids.Add(obj.spell_Items[i].itemName, i);
                }
            }
        }

        void LoadWeapomIds()
        {
            WeaponScriptableObject obj = Resources.Load("Retake.WeaponScriptableObject") as WeaponScriptableObject;

            if (obj == null)
            {
                Debug.Log("Retake.WeaponScriptableObject could not be loaded!");
                return;
            }

            for (int i = 0; i < obj.weapons_all.Count; i++)
            {
                if(Weapon_ids.ContainsKey(obj.weapons_all[i].itemName))
                {
                    Debug.Log(obj.weapons_all[i].itemName + "item is a dupilcate");
                }
                else
                {
                    Weapon_ids.Add(obj.weapons_all[i].itemName, i);
                }
            }

            for(int i = 0; i < obj.weaponStats.Count; i++)
            {
                if (Weaponstats_ids.ContainsKey(obj.weaponStats[i].weaponId))
                {
                    Debug.Log(obj.weaponStats[i].weaponId + "is a dupliacte");
                }
                else
                {
                    Weaponstats_ids.Add(obj.weaponStats[i].weaponId, i);
                }
            }
        }

        void LoadConsumables()
        {
            ConsumableScriptableObject obj = Resources.Load("Retake.ConsumableScriptableObject") as ConsumableScriptableObject;

            if (obj == null)
            {
                Debug.Log("Retake.ConsumableScriptableObject could not be loaded!");
                return;
            }

            for (int i = 0; i < obj.consumabs.Count; i++)
            {
                if (consum_ids.ContainsKey(obj.consumabs[i].itemName))
                {
                    Debug.Log(obj.consumabs[i].itemName + "item is a dupilcate");
                }
                else
                {
                    consum_ids.Add(obj.consumabs[i].itemName, i);
                }
            }
        }

        //weapon
        int GetWeaponIdFromString(string id)
        {
            int index = -1;
            if(Weapon_ids.TryGetValue(id,out index))
            {
                return index;
            }

            return -1;
        }

        public Weapon GetWeapon(string id)
        {
            WeaponScriptableObject obj = Resources.Load("Retake.WeaponScriptableObject") as WeaponScriptableObject;

            if (obj == null)
            {
                Debug.Log("Retake.WeaponScriptableObject could not be loaded!");
                return null;
            }

            int index = GetWeaponIdFromString(id);

            if (index == -1)
                return null;

             return obj.weapons_all[index];
        }

        int GetWeaponStatsIdFromString(string id)
        {
            int index = -1;
            if (Weaponstats_ids.TryGetValue(id, out index))
            {
                return index;
            }

            return -1;
        }

        public WeaponStats GetWeaponStats(string id)
        {
            WeaponScriptableObject obj = Resources.Load("Retake.WeaponScriptableObject") as WeaponScriptableObject;

            if (obj == null)
            {
                Debug.Log("Retake.WeaponScriptableObject could not be loaded!");
                return null;
            }

            int index = GetWeaponStatsIdFromString(id);

            if (index == -1)
                return null;

            return obj.weaponStats[index];
        }

        //spells
        int GetSpellIdFromString(string id)
        {
            int index = -1;
            if (Spell_ids.TryGetValue(id, out index))
            {
                return index;
            }

            return -1;
        }

        public Spell GetSpell(string id)
        {
            SpellItemScriptableObject obj = Resources.Load("Retake.SpellItemScriptableObject") as SpellItemScriptableObject;

            if(obj == null)
            {
                Debug.Log("Retake.SpellItemScriptableObject could not be loaded!");
                return null;
            }

            int index = GetSpellIdFromString(id);

            if (index == -1)
                return null;

            return obj.spell_Items[index];
        }

        //Consumables
        int GetConsumablesIdFromString(string id)
        {
            int index = -1;
            if (consum_ids.TryGetValue(id, out index))
            {
                return index;
            }

            return -1;
        }

        public Consumable GetConsumable(string id)
        {
            ConsumableScriptableObject obj = Resources.Load("Retake.ConsumableScriptableObject") as ConsumableScriptableObject;

            if (obj == null)
            {
                Debug.Log("Retake.ConsumableScriptableObject could not be loaded!");
                return null;
            }

            int index = GetConsumablesIdFromString(id);

            if (index == -1)
                return null;

            return obj.consumabs[index];
        }
    }
}