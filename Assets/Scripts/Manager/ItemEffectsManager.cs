using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Retake
{
    public class ItemEffectsManager : MonoBehaviour
    {
        Dictionary<string, int> effects = new Dictionary<string, int>();

        void InitEffectsId()
        {
            effects.Add("bestus", 0);
            effects.Add("bestus_focus", 1);
            effects.Add("souls", 2);
        }

        public void CastEffect(string effectId,StateManager  states)
        {
            int i = GetIntFromId(effectId);
            if (i < 0)
            {
                Debug.Log(effectId + "effect doesnst exist");
                return;
            }
            switch(i)
            {
                case 0: //bestus
                    AddHealth(states);
                    break;
                case 1: //focus
                    AddFocus(states);
                    break;
                case 2: //focus
                    AddSouls(states);
                    break;
            }
        }

        #region Effects Actual
        void AddHealth(StateManager states)
        {
            states.characterStats._health += states.characterStats._healthRecoverValue;
        }

        void AddFocus(StateManager states)
        {
            states.characterStats._focus += states.characterStats._focusRecoverValue;
        }

        void AddSouls(StateManager states)
        {
            states.characterStats._souls += 100;
        }
        #endregion

        int GetIntFromId(string id)
        {
            int index = -1;
            if(effects.TryGetValue(id,out index))
            {
                return index;
            }

            return index;
        }

        public static ItemEffectsManager singleton;
        void Awake()
        {
            singleton = this;
            InitEffectsId();
        }
    }
}
