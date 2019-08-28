using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Retake
{
    [ExecuteInEditMode]
    public class WeaponPlacer : MonoBehaviour
    {
        public string iteamName;

        public GameObject targetModel;

        public bool saveItem;
        public bool leftHand;
        public SaveType saveType;

        public enum SaveType
        {
            weapon,item
        }

        void Update()
        {
            if (!saveItem)
                return;

            saveItem = false;


            switch (saveType)
            {
                case SaveType.weapon:
                    SaveWeapon();
                    break;
                case SaveType.item:
                    SaveConsumable();
                    break;
                default:
                    break;
            }

        }

        void SaveWeapon()
        {
            
            if (targetModel == null)
                return;
            if (string.IsNullOrEmpty(iteamName))
                return;

            ConsumableScriptableObject obj = Resources.Load("Retake.ConsumableScriptableObject") as ConsumableScriptableObject;

            if (obj == null)
                return;

            for (int i = 0; i < obj.consumabs.Count; i++)
            {
                if(obj.consumabs[i].itemName == iteamName)
                {
                    Consumable w = obj.consumabs[i];
                    w.r_model_eulers = targetModel.transform.localEulerAngles;
                    w.r_model_pos = targetModel.transform.localPosition;

                    w.model_scale = targetModel.transform.localScale;

                    return;
                }
            }

            Debug.Log(iteamName + "was not found in the inventory");
        }

        void SaveConsumable()
        {
            if (targetModel == null)
                return;
            if (string.IsNullOrEmpty(iteamName))
                return;

            WeaponScriptableObject obj = Resources.Load("Retake.WeaponScriptableObject") as WeaponScriptableObject;

            if (obj == null)
                return;

            for (int i = 0; i < obj.weapons_all.Count; i++)
            {
                if (obj.weapons_all[i].itemName == iteamName)
                {
                    Weapon w = obj.weapons_all[i];

                    if (leftHand)
                    {
                        w.l_model_eulers = targetModel.transform.localEulerAngles;
                        w.l_model_pos = targetModel.transform.localPosition;
                    }
                    else
                    {
                        w.r_model_eulers = targetModel.transform.localEulerAngles;
                        w.r_model_pos = targetModel.transform.localPosition;
                    }

                    w.model_scale = targetModel.transform.localScale;

                    return;
                }
            }

            Debug.Log(iteamName + "was not found in the inventory");
        }
    }

}