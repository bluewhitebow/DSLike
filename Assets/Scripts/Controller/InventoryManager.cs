using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Retake
{
    public class InventoryManager : MonoBehaviour
    {
        public string unarmedId = "unarmed";
        public Runtimeweapon unarmedRuntime;
        public List<string> rh_weapons;
        public List<string> lh_weapons;
        public List<string> spell_items;
        public List<string> consumable_items;

        public int r_index;
        public int l_index;
        public int s_index;
        public int c_index;
        List<Runtimeweapon> r_r_weapons = new List<Runtimeweapon>();
        List<Runtimeweapon> r_l_weapons = new List<Runtimeweapon>();
        List<RuntimeSpellItems> r_spells = new List<RuntimeSpellItems>();
        List<RuntimeConsumable> r_consum = new List<RuntimeConsumable>();

        public RuntimeConsumable curConsumable;
        public RuntimeSpellItems currentSpell;
        public Runtimeweapon rightHandWeapon;
        public bool hasLeftHandWeapon = true;
        public Runtimeweapon leftHandWeapon;

        public GameObject parryCollider;
        public GameObject breathCollider;
        public GameObject blockCollider;

        StateManager states;

        public void Init(StateManager st)
        {
            states = st;
            LoadInventory();

            ParryCollider pr = parryCollider.GetComponent<ParryCollider>();
            pr.InitPlayer(st);
            CloseParryCollider();
            CloseBreathCollider();
            CloseBlockCollider();
        }

        public void LoadInventory()
        {
            unarmedRuntime = WeaponToRuntimeWeapon(ResourcesManager.singleton.GetWeapon(unarmedId),false);

            for (int i = 0; i < rh_weapons.Count; i++)
            {
                Runtimeweapon rw = 
                    WeaponToRuntimeWeapon(ResourcesManager.singleton.GetWeapon(rh_weapons[i]));

                r_r_weapons.Add(rw);
            }

            for (int i = 0; i < lh_weapons.Count; i++)
            {
                Runtimeweapon rw =
                    WeaponToRuntimeWeapon(ResourcesManager.singleton.GetWeapon(lh_weapons[i]), true);

                r_l_weapons.Add(rw);
            }

            if(r_r_weapons.Count > 0)
            {
                if (r_index > r_r_weapons.Count - 1)
                    r_index = 0;

                rightHandWeapon = r_r_weapons[r_index];
            }

            if (r_l_weapons.Count > 0)
            {
                if (l_index > r_l_weapons.Count - 1)
                    l_index = 0;

                leftHandWeapon = r_l_weapons[l_index];
            }

            if (rightHandWeapon != null)
                EquipWeapon(rightHandWeapon, false);

            if (leftHandWeapon != null)
            {
                EquipWeapon(leftHandWeapon, true);
                hasLeftHandWeapon = true;
            }

            for(int i = 0;i < spell_items.Count;i++)
            {
                SpellToRuntimeSpell(ResourcesManager.singleton.GetSpell(spell_items[i]));
            }
            //hasLeftHandWeapon = (leftHandWeapon != null);

            if (r_spells.Count > 0)
            {
                if (s_index > r_spells.Count)
                    s_index = 0;

                EquipSpell(r_spells[s_index]);
            }

            for (int i = 0; i < consumable_items.Count; i++)
            {
                RuntimeConsumable c = ConsumableToRuntime(ResourcesManager.singleton.GetConsumable(consumable_items[i]));
                r_consum.Add(c);
            }

            if(r_consum.Count > 0)
            {
                if (c_index > r_consum.Count - 1)
                    c_index = 0;

                EquipConsuamble(r_consum[c_index]);
            }

            InitALLDamageColliders(states);
            CloseAllDamageColliders();

        }

        public void EquipWeapon(Runtimeweapon w,bool isLeft = false)
        {
            if(isLeft)
            {
                if (leftHandWeapon != null)
                    leftHandWeapon.weaponModel.SetActive(false);

                leftHandWeapon = w;

            }
            else
            {
                if (rightHandWeapon != null)
                    rightHandWeapon.weaponModel.SetActive(false);

                rightHandWeapon = w;
            }
            

            string targetIlde = w.instance.oh_idle;
            targetIlde += (isLeft) ? "_l" : "_r";
            states.anim.SetBool(StaticStrings.mirror, isLeft);
            states.anim.Play(StaticStrings.changeWeapon);
            states.anim.Play(targetIlde);

            UI.QuickSlot uiSlot = UI.QuickSlot.singleton;
            uiSlot.UpdateSlot((isLeft) ? UI.QSlotType.lh : UI.QSlotType.rh, w.instance.icon);

            w.weaponModel.SetActive(true);
        }

        public void EquipSpell(RuntimeSpellItems spell)
        {
            currentSpell = spell;

            UI.QuickSlot uiSlot = UI.QuickSlot.singleton;
            uiSlot.UpdateSlot(UI.QSlotType.spell,spell.instance.icon);
        }

        public void EquipConsuamble(RuntimeConsumable consum)
        {
            curConsumable = consum;

            UI.QuickSlot uiSlot = UI.QuickSlot.singleton;
            uiSlot.UpdateSlot(UI.QSlotType.item, consum.instance.icon);
        }

        public Weapon GetCurrrentWeapon(bool isLeft = false)
        {
            if (isLeft)
                return leftHandWeapon.instance;
            else
                return rightHandWeapon.instance;
        }

        public void OpenAllDmamageColliders()
        {
            if (rightHandWeapon != null)
            {
                if(rightHandWeapon.w_hook != null)
                    rightHandWeapon.w_hook.OpenDamageColliders();
            }

            if (leftHandWeapon != null)
            {
                if(leftHandWeapon.w_hook != null)
                    leftHandWeapon.w_hook.OpenDamageColliders();
            }
        }

        public void CloseAllDamageColliders()
        {
            if (rightHandWeapon != null)
            {
                if(rightHandWeapon.w_hook != null)
                    rightHandWeapon.w_hook.CloseDamageColliders();
            }

            if (leftHandWeapon != null)
            {
                if(leftHandWeapon.w_hook != null)
                    leftHandWeapon.w_hook.CloseDamageColliders();
            }
        }

        public void InitALLDamageColliders(StateManager states)
        {
            if (rightHandWeapon != null)
            { 
                if(rightHandWeapon.w_hook != null)
                    rightHandWeapon.w_hook.InitDamageColliders(states);
            }

            if (leftHandWeapon != null)
            {
                if (leftHandWeapon.w_hook != null)
                    leftHandWeapon.w_hook.InitDamageColliders(states);
            }
        }

        public void CloseParryCollider()
        {
            parryCollider.SetActive(false);

        }

        public void OpenParryCollider()
        {
            parryCollider.SetActive(true);

        }

        public RuntimeSpellItems SpellToRuntimeSpell(Spell s,bool isLeft = false)
        {
            GameObject go = new GameObject();
            RuntimeSpellItems inst = go.AddComponent<RuntimeSpellItems>();
            inst.instance = new Spell();
            StaticFunctions.DeepCopySpell(s, inst.instance);
            go.name = s.itemName;

            r_spells.Add(inst);
            return inst;
        }

        public void CreateSpellParticle(RuntimeSpellItems inst,bool isLeft = false,bool parentUnderRoot = false)
        {
            if (inst.currentParticle == null)
            {
                inst.currentParticle = Instantiate(inst.instance.particle_prefab) as GameObject;
                inst.p_hook = inst.currentParticle.GetComponentInChildren<ParticleHook>();
                inst.p_hook.Init();
            }

            if (!parentUnderRoot)
            {
                Transform p = states.anim.GetBoneTransform((isLeft) ? HumanBodyBones.LeftHand : HumanBodyBones.RightHand);
                inst.currentParticle.transform.parent = p;
                inst.currentParticle.transform.localRotation = Quaternion.identity;
                inst.currentParticle.transform.localPosition = Vector3.zero;
            }
            else
            {
                inst.currentParticle.transform.parent = transform;
                inst.currentParticle.transform.localRotation = Quaternion.identity;
                inst.currentParticle.transform.localPosition = new Vector3(0,1.5f,.9f);
            }
            //inst.currentParticle.SetActive(false);
        }

        public Runtimeweapon WeaponToRuntimeWeapon(Weapon w, bool isLeft = false)
        {
            GameObject go = new GameObject();
            Runtimeweapon inst = go.AddComponent<Runtimeweapon>();
            go.name = w.itemName;
            inst.instance = new Weapon();
            StaticFunctions.DeepCopyWeapon(w, inst.instance);

            inst.WeaponStats = new WeaponStats();
            WeaponStats w_stats = ResourcesManager.singleton.GetWeaponStats(w.itemName);
            StaticFunctions.DeepCopyWeaponStats(w_stats,inst.WeaponStats);

            inst.weaponModel = Instantiate(inst.instance.modelPrefeb) as GameObject;
            Transform p = states.anim.GetBoneTransform((isLeft) ? HumanBodyBones.LeftHand : HumanBodyBones.RightHand);
            inst.weaponModel.transform.parent = p;

            inst.weaponModel.transform.localPosition = (isLeft) ? inst.instance.l_model_pos : inst.instance.r_model_pos;
            inst.weaponModel.transform.localEulerAngles = (isLeft) ? inst.instance.l_model_eulers : inst.instance.r_model_eulers;
            inst.weaponModel.transform.localScale = inst.instance.model_scale;

            inst.w_hook = inst.weaponModel.GetComponentInChildren<WeaponHook>();

            inst.w_hook.InitDamageColliders(states);

   
            inst.weaponModel.SetActive(false);
            return inst;
        }

        public RuntimeConsumable ConsumableToRuntime(Consumable c)
        {
            GameObject go = new GameObject();
            RuntimeConsumable inst = go.AddComponent<RuntimeConsumable>();
            go.name = c.itemName;

            inst.instance = new Consumable();
            StaticFunctions.DeepCopyConsumables(inst.instance, c);

            if(inst.instance.itemPrefab != null)
            {
                GameObject model = Instantiate(inst.instance.itemPrefab) as GameObject;
                Transform p = states.anim.GetBoneTransform(HumanBodyBones.RightHand);
                model.transform.parent = p;
                model.transform.localPosition = inst.instance.r_model_pos;
                model.transform.localEulerAngles = inst.instance.r_model_eulers;

                Vector3 targetScale = inst.instance.model_scale;
                if (targetScale == Vector3.zero)
                    targetScale = Vector3.one;
                model.transform.localScale = targetScale;
                inst.itemModel = model;
                inst.itemModel.SetActive(false);
                //create here
            }
            return inst;

        }

        public void ChangeToNextWeapon(bool isLeft)
        {
            states.isTwoHanded = false;
            states.HandleTwoHanded();

            if (isLeft)
            {
                if (r_l_weapons.Count == 0)
                    return;

                if (l_index < r_l_weapons.Count - 1)
                    l_index++;
                else
                    l_index = 0;

                EquipWeapon(r_l_weapons[l_index], true);
            }
            else
            {
                if (r_r_weapons.Count == 0)
                    return;

                if (r_index < r_r_weapons.Count - 1)
                    r_index++;
                else
                    r_index = 0;

                EquipWeapon(r_r_weapons[r_index]);
            }

            states.actionManager.UpdataActionsOneHanded();
        }

        public void ChangeToNextSpell()
        {
            if (s_index < r_spells.Count - 1)
                s_index++;
            else
                s_index = 0;

            EquipSpell(r_spells[s_index]);
        }

        public void ChangeToNextConsumables()
        {
            if (c_index < r_consum.Count - 1)
                c_index++;
            else
                c_index = 0;

            EquipConsuamble(r_consum[c_index]);
        }

        #region Delegate Calls
        public void OpenBreathCollider()
        {
            breathCollider.SetActive(true);
        }

        public void CloseBreathCollider()
        {
            breathCollider.SetActive(false);
        }

        public void OpenBlockCollider()
        {
            blockCollider.SetActive(true);
        }

        public void CloseBlockCollider()
        {
            blockCollider.SetActive(false);
        }

        public void EmitSpellParticle()
        {
            currentSpell.p_hook.Emit(1);
        }
        #endregion
    }

    [System.Serializable]
    public class Item
    {
        public string itemName;
        public string itemDescription;
        public Sprite icon;
 
    }

    [System.Serializable]
    public class Weapon:Item
    {
        public string oh_idle;
        public string th_idle;
        
        public List<Action> actions;
        public List<Action> two_handedActions;

        public float parryMultiplier;
        public float backstabMultiplier;
        public bool LeftHandMirror;

        public GameObject modelPrefeb;
        public Vector3 r_model_pos;
        public Vector3 l_model_pos;
        public Vector3 r_model_eulers;
        public Vector3 l_model_eulers;
        public Vector3 model_scale;

       
        public Action GetAction(List<Action> l, ActionInput inp)
        {
            if (l == null)
                return null;

            for (int i = 0; i < l.Count; i++)
            {
                if (l[i].GetFirstInput() == inp)
                    return l[i];
            }

            return null;
        }
    }

    [System.Serializable]
    public class Spell:Item
    {
        public SpellType spellType;
        public SpellClass spellClass;
        public List<SpellAction> actions = new List<SpellAction>();
        public GameObject projectile;
        public GameObject particle_prefab;
        public string spell_effect;

        public SpellAction GetAction(List<SpellAction> l, ActionInput inp)
        {
            if (l == null)
                return null;

            for (int i = 0; i < l.Count; i++)
            {
                if (l[i].input == inp)
                    return l[i];
            }

            return null;
        }
    }

    [System.Serializable]
    public class Consumable : Item
    {
        public string consumableEffect;
        public string targetAnim;

        public GameObject itemPrefab;
        public Vector3 r_model_pos;
        public Vector3 r_model_eulers;
        public Vector3 model_scale;
    }

}
