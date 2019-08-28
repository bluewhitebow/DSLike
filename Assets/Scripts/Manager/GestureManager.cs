using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Retake
{
    public class GestureManager : MonoBehaviour
    {
        public List<GestureContainer> gestures = new List<GestureContainer>();
        Dictionary<string, int> gestures_dict = new Dictionary<string, int>();

        public GameObject gesturesGrid;
        public GameObject gestureIconTemplate;
        public RectTransform gestureSelector;

        public int index;

        void Start()
        {
            CreateGesturesUI();
        }

        public void SelectGesture(bool pos)
        {

            if (pos)
                index++;
            else
                index--;

            if (index < 0)
                index = gestures.Count - 1;

            if (index > gestures.Count - 1)
                index = 0;

            IconBase i = gestures[index].i_base;
            gestureSelector.transform.SetParent(i.transform);
            gestureSelector.anchoredPosition = new Vector2(46, -50);
        }

        public void HandleGestures(bool isOpen)
        {
            if(isOpen)
            {
                if (gesturesGrid.activeInHierarchy == false)
                {
                    gesturesGrid.SetActive(true);
                    gestureSelector.gameObject.SetActive(true);
                }
            }
            else
            {
                if (gesturesGrid.activeInHierarchy)
                {
                    gesturesGrid.SetActive(false);
                    gestureSelector.gameObject.SetActive(false);
                }
            }

        }

        void CreateGesturesUI()
        {
            for(int i = 0; i < gestures.Count;i++)
            {
                GameObject go = Instantiate(gestureIconTemplate) as GameObject;
                go.transform.SetParent(gesturesGrid.transform);
                go.transform.localScale = Vector3.one;
                go.SetActive(true);
                IconBase icon = go.GetComponentInChildren<IconBase>();
                icon.icon.sprite = gestures[i].icon;
                icon.id = gestures[i].targetAnim;
                gestures[i].i_base = icon;
            }

            gesturesGrid.SetActive(false);
            gestureSelector.gameObject.SetActive(false);
            index = 1;
            SelectGesture(false);
        }

        public GestureContainer GetGesture(string id)
        {
            int index = -1;
            if(gestures_dict.TryGetValue(id,out index))
            {
                return gestures[index];
            }

            return null;
        }

        public static GestureManager singleton;
        
        void Awake()
        {
            singleton = this;

            for(int i = 0;i < gestures.Count;i++)
            {
                if(gestures_dict.ContainsKey(gestures[i].targetAnim))
                {
                    Debug.Log(gestures[i].targetAnim + "is a duplicate");
                }
                else
                {
                    gestures_dict.Add(gestures[i].targetAnim, i);
                }
            }
        }
    }

    [System.Serializable]
    public class GestureContainer
    {
        public Sprite icon;
        public string targetAnim;
        public IconBase i_base;
    }
}
