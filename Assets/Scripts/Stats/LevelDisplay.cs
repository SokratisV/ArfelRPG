using System;
using TMPro;
using UnityEngine;

namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        BaseStats stats;

        private void Awake()
        {
            stats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        private void Update()
        {
            GetComponent<TextMeshProUGUI>().text = String.Format("{0:0}", stats.CalculateLevel());
        }
    }
}