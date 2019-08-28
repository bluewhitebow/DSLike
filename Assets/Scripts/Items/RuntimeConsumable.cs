using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Retake
{
    public class RuntimeConsumable : MonoBehaviour
    {
        public int itemCount = 2;
        public bool unlimitedCount;
        public Consumable instance;
        public GameObject itemModel;
    }
}
