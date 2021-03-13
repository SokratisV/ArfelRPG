using System.Collections;
using RPG.Core;
using RPG.Skills;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
	public class Castbar : MonoBehaviour
	{
		[SerializeField] private SkillUser skillUser;
		[SerializeField] private Image castBarFill;
		[SerializeField] private GameObject castBarParent;

		private Coroutine _castBarFillRoutine;

		private void OnEnable()
		{
			castBarParent.SetActive(false);
			skillUser.OnSkillCast += ShowCastBar;
			skillUser.OnActionComplete += DisableCastBar;
			skillUser.OnActionCancelled += DisableCastBar;
		}

		private void OnDisable()
		{
			skillUser.OnSkillCast -= ShowCastBar;
			skillUser.OnActionComplete -= DisableCastBar;
			skillUser.OnActionCancelled -= DisableCastBar;
		}

		private void DisableCastBar()
		{
			castBarParent.SetActive(false);
			_castBarFillRoutine.StopCoroutine(this);
		}

		private void ShowCastBar(Skill skill)
		{
			if(skill.HasCastTime)
			{
				castBarParent.SetActive(true);
				castBarFill.fillAmount = 0;
				_castBarFillRoutine = _castBarFillRoutine.StartCoroutine(this, FillOverTime(skill.Duration));
			}
		}

		private IEnumerator FillOverTime(float overTime)
		{
			var progress = 0f;
			while(progress < 1)
			{
				castBarFill.fillAmount = progress;
				progress += Time.deltaTime / overTime;
				yield return null;
			}

			castBarParent.SetActive(false);
		}
	}
}