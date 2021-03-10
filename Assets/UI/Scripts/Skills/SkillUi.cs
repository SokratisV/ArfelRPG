using System.Collections;
using RPG.Core;
using RPG.Skills;
using RPG.UI.Dragging;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RPG.UI.Skills
{
	public class SkillUi : MonoBehaviour, ISkillHolder, IDragContainer<Skill>, IPointerClickHandler
	{
		[SerializeField] private SkillIcon icon = null;
		[SerializeField] private TextMeshProUGUI keyBindText;
		[SerializeField] private int index = 0;
		[SerializeField] private KeyCode keyBind = KeyCode.None;
		[SerializeField] private Image globalCooldownFill, cooldownFill;

		private Coroutine _globalCooldownRoutine = null, _cooldownRoutine = null;
		private SkillUser _skillUser;

		#region Unity

		private void Awake()
		{
			_skillUser = SkillUser.GetPlayerSkills();
			keyBindText.SetText(keyBind.ToString());
		}

		private void Update()
		{
			if(Input.GetKeyDown(keyBind)) _skillUser.SelectSkill(index);
		}

		private void OnEnable()
		{
			_skillUser.SkillsUpdated += UpdateIcon;
			_skillUser.OnSkillCast += ShowCooldown;
			_skillUser.OnSkillCast += ShowGlobalCooldown;
		}

		private void OnDestroy()
		{
			_skillUser.SkillsUpdated -= UpdateIcon;
			_skillUser.OnSkillCast -= ShowCooldown;
			_skillUser.OnSkillCast -= ShowGlobalCooldown;
		}

		#endregion

		#region Public

		public Skill GetItem() => _skillUser.GetSkill(index);

		public int GetNumber() => 1;

		public void RemoveItems(int number) => _skillUser.RemoveSkill(index);

		public int MaxAcceptable(Skill skill) => 1;

		public void AddItems(Skill skill, int _) => _skillUser.AddSkill(skill, index);

		#endregion

		#region Private

		private void ShowGlobalCooldown(Skill skill) => _globalCooldownRoutine = _globalCooldownRoutine.StartCoroutine(this, GlobalCooldown(GlobalValues.GlobalCooldown));

		private void ShowCooldown(Skill skill)
		{
			if(_skillUser.GetSkill(index) == skill) _cooldownRoutine = _cooldownRoutine.StartCoroutine(this, Cooldown(skill.Cooldown));
		}

		private IEnumerator Cooldown(float cooldown)
		{
			var progress = 1f;
			while(progress > 0)
			{
				cooldownFill.fillAmount = progress;
				progress -= Time.deltaTime / cooldown;
				yield return null;
			}

			cooldownFill.fillAmount = 0f;
		}

		private IEnumerator GlobalCooldown(float globalCooldown)
		{
			var progress = 1f;
			while(progress > 0)
			{
				globalCooldownFill.fillAmount = progress;
				progress -= Time.deltaTime / globalCooldown;
				yield return null;
			}

			globalCooldownFill.fillAmount = 0f;
		}

		private void UpdateIcon() => icon.UpdateIcon(GetItem());

		#endregion
		
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
	}
}