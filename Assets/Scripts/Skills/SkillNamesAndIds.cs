using System;
using RotaryHeart.Lib.SerializableDictionary;
using RPG.Skills;
using Sirenix.OdinInspector;
using UnityEngine;

public class SkillNamesAndIds : ScriptableObject
{
	[SerializeField] private SkillNameToId nameToId = new SkillNameToId();

	public string GetSkillId(string skillName) => nameToId.TryGetValue(skillName, out var id) ? id : null;

	[Serializable]
	private class SkillNameToId : SerializableDictionaryBase<string, string>
	{
	}

	[Button(nameof(UpdateDictionary), ButtonSizes.Large)]
	public void UpdateDictionary()
	{
		var skills = Resources.LoadAll<Skill>("");
		nameToId.Clear();
		foreach (var skill in skills)
		{
			nameToId.Add(skill.DisplayName, skill.SkillID);
		}
	}
}