using UnityEngine;

namespace RPG.Core
{
	public interface ISkillcastable
	{
		CursorType GetSkillCursorType();
		bool HandleSkillcast(GameObject player);
		void ShowInteractivity();
	}
}