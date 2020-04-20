using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace LD_46
{
    public class StorageController : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_WoodCountText;
        [SerializeField]
        private TextMeshProUGUI m_FoodCountText;
        [SerializeField]
        private int m_WoodStored = 1;
        [SerializeField]
        private int m_FoodStored = 1;

        public int WoodStored { get => m_WoodStored; set => m_WoodStored = value; }
        public int FoodStored { get => m_FoodStored; set => m_FoodStored = value; }
        public TextMeshProUGUI WoodCountText => m_WoodCountText;
        public TextMeshProUGUI FoodCountText => m_FoodCountText;

        private void Update()
        {
            WoodCountText.text = WoodStored.ToString() + "\nWood";
            FoodCountText.text = FoodStored.ToString() + "\nFood";
        }
    }
}
