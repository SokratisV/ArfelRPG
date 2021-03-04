using RPG.Core;
using RPG.Skills;
using RPG.UI.Dragging;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.UI.Skills
{
	public class SkillUi : MonoBehaviour, ISkillHolder, IDragContainer<Skill>, IPointerClickHandler
	{
		[SerializeField] private SkillIcon icon = null;
		[SerializeField] private TextMeshProUGUI keyBindText;
		[SerializeField] private int index = 0;
		[SerializeField] private KeyCode keyBind = KeyCode.None;

		private SkillUser _skillUser;

		private void Awake()
		{
			_skillUser = PlayerFinder.Player.GetComponent<SkillUser>();
			keyBindText.SetText(Helper.KeyCodeName(keyBind));
			_skillUser.SkillsUpdated += UpdateIcon;
		}

		private void Update()
		{
			if(Input.GetKeyDown(keyBind)) _skillUser.SelectSkill(index);
		}

		public Skill GetItem() => _skillUser.GetSkill(index);

		public int GetNumber() => 1;

		public void RemoveItems(int number) => _skillUser.RemoveSkill(index);

		private void UpdateIcon() => icon.UpdateIcon(GetItem());

		void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
		{
			var shouldAct = eventData.button switch
			{
				PointerEventData.InputButton.Right => true,
				PointerEventData.InputButton.Left => true,
				_ => false
			};

			if(shouldAct) _skillUser.SelectSkill(index);
		}

		Skill ISkillHolder.GetSkill() => _skillUser.GetSkill(index);

		public int MaxAcceptable(Skill skill) => 1;

		public void AddItems(Skill skill, int _) => _skillUser.AddSkill(skill, index);
	}
}