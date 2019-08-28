using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Retake
{
    public class UIManager : MonoBehaviour
    {
        public float lerpSpeed = 2;
        public Slider health;
        public Slider h_vis;
        public Slider focus;
        public Slider f_vis;
        public Slider stamina;
        public Slider s_vis;
        public Text souls;
        public float sizeMultiplier = 2;
        int curSouls;

        public GestureManager gestures;

        void Start()
        {
            gestures = GestureManager.singleton;
        }

        public void InitSouls(int v)
        {
            curSouls = v;
        }

        public void InitSlider(StatSlider t, int value)
        {

            Slider s = null;
            Slider v = null;

            switch(t)
            {
                case StatSlider.health:
                    s = health;
                    v = h_vis;
                    break;
                case StatSlider.focus:
                    s = focus;
                    v = f_vis;
                    break;
                case StatSlider.stamina:
                    s = stamina;
                    v = s_vis;
                    break;
                default:
                    break;
            }
            s.maxValue = value;
            v.maxValue = value;
            RectTransform r = s.GetComponent<RectTransform>();
            RectTransform r_v = v.GetComponent<RectTransform>();
            float value_actual = value * sizeMultiplier;
            value_actual = Mathf.Clamp(value_actual, 0, 1000);
            r.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, value_actual);
            r_v.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, value_actual);
        }

        public void Tick(CharacterStats stats, float delta)
        {
            health.value = Mathf.Lerp(health.value, stats._health, delta * lerpSpeed * 2);
            focus.value = Mathf.Lerp(focus.value, stats._focus, delta * lerpSpeed * 2);
            stamina.value = stats._stamina;

            curSouls = Mathf.RoundToInt(Mathf.Lerp(curSouls, stats._souls, delta * lerpSpeed));
            souls.text = curSouls.ToString();

            h_vis.value = Mathf.Lerp(h_vis.value, stats._health, delta * lerpSpeed);
            f_vis.value = Mathf.Lerp(f_vis.value, stats._focus, delta * lerpSpeed);
            s_vis.value = Mathf.Lerp(s_vis.value, stats._stamina, delta * lerpSpeed);
        }

        public void AffectALL(int h,int f,int s)
        {
            InitSlider(StatSlider.health, h);
            InitSlider(StatSlider.focus, f);
            InitSlider(StatSlider.stamina, s);
        }

        public enum StatSlider
        {
            health,focus,stamina

        }

        public static UIManager singleton;
        void Awake()
        {
            singleton = this;
        }

    }
}
