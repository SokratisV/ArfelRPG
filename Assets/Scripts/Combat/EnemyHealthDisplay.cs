using TMPro;
using UnityEngine;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        private Fighter _fighter;
        private TextMeshProUGUI _text;

        private void Awake()
        {
            _fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
            _text = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            if(_fighter.GetTarget() == null)
            {
                _text.SetText("N/A");
            }
            else
            {
                var health = _fighter.GetTarget();
                _text.SetText($"{health.GetHealthPoints():0}/{health.GetMaxHealthPoints():0}");
            }
        }
    }
}