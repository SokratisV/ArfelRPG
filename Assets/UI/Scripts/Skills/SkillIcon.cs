using RPG.Skills;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Skills
{
	[RequireComponent(typeof(Image))]
	public class SkillIcon : MonoBehaviour
	{
		// [SerializeField] private GameObject textContainer = null;
		// [SerializeField] private TextMeshProUGUI cooldown = null;

		private Image _image;

		private void Awake() => _image = GetComponent<Image>();

		public void UpdateIcon(Skill skill)
		{
			if(skill == null)
			{
				_image.enabled = false;
			}
			else
			{
				_image.enabled = true;
				_image.sprite = skill.Icon;
			}
			//add cooldown stuff?
		}
	}
}