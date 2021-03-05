using System;
using System.Collections.Generic;
using RPG.Skills.Behaviors;
using UnityEngine;

namespace RPG.Skills
{
	public struct SkillData
	{
		public GameObject User;
		public GameObject Target;
		public Vector3? Point;

		public SkillData(GameObject user, GameObject target, Vector3? point)
		{
			User = user;
			Target = target;
			Point = point;
		}
	}

	[CreateAssetMenu(fileName = "Skill", menuName = "RPG/Skills/New Skill", order = 0)]
	public class Skill : ScriptableObject, ISerializationCallbackReceiver
	{
		[Tooltip("Auto-generated UUID for saving/loading. Clear this field if you want to generate a new one.")] [SerializeField]
		private string skillID;

		[Tooltip("Item name to be displayed in UI.")] [SerializeField]
		private string displayName = null;

		[Tooltip("Item description to be displayed in UI.")] [SerializeField] [TextArea]
		private string description = null;

		[Tooltip("The UI icon to represent this item in the inventory.")] [SerializeField]
		private Sprite icon = null;

		[SerializeField] private float duration;
		[SerializeField] private TargetBehavior targetBehavior;
		[SerializeField] private SkillBehavior skillBehavior;

		[Header("On Start")] [Space(15)] [SerializeField]
		private GameObject[] vfxOnUserStart;

		[SerializeField] private GameObject[] vfxOnTargetStart;
		[SerializeField] private AudioClip[] sfxOnUserStart, sfxOnTargetStart;

		[Header("On End")] [Space(15)]
		[HideInInspector] public GameObject[] vfxOnUserEnd;

		[HideInInspector] public GameObject[] vfxOnTargetEnd;
		[HideInInspector] public AudioClip[] sfxOnUserEnd, sfxOnTargetEnd;

		public string SkillID => skillID;
		public Sprite Icon => icon;
		public string DisplayName => displayName;
		public string Description => description;
		public float Duration => duration;
		public bool RequiresTarget => targetBehavior.RequireTarget();
		public TargetBehavior TargetBehavior => targetBehavior;

		private static Dictionary<string, Skill> ItemLookupCache;

		public SkillData? OnStart(GameObject user, GameObject target = null, Vector3? point = null)
		{
			var targets = targetBehavior.GetTargets(user, target, point);
			if(targets == null) return null;
			skillBehavior.OnStart += PlayStartVfx;
			skillBehavior.OnEnd += PlayEndVfx;
			skillBehavior.BehaviorStart(user, targets);
			return new SkillData(user, target, point);
		}

		public void OnUpdate(SkillData data) => skillBehavior.BehaviorUpdate(data.User, targetBehavior.GetTargets(data.User, data.Target, data.Point));

		public void OnEnd(SkillData data) => skillBehavior.BehaviorEnd(data.User, targetBehavior.GetTargets(data.User, data.Target, data.Point));

		private void PlayStartVfx(GameObject user, GameObject[] targets)
		{
			foreach(var vfx in vfxOnUserStart)
			{
				Instantiate(vfx, user.transform);
			}

			foreach(var vfx in vfxOnTargetStart)
			{
				foreach(var target in targets)
				{
					Instantiate(vfx, target.transform);
				}
			}
		}

		private void PlayEndVfx(GameObject user, GameObject[] targets)
		{
			foreach(var vfx in vfxOnUserEnd)
			{
				Instantiate(vfx, user.transform);
			}

			foreach(var vfx in vfxOnTargetEnd)
			{
				foreach(var target in targets)
				{
					Instantiate(vfx, target.transform);
				}
			}
		}

		public static Skill GetFromID(string skillID)
		{
			if(ItemLookupCache == null)
			{
				ItemLookupCache = new Dictionary<string, Skill>();
				var itemList = Resources.LoadAll<Skill>("");
				foreach(var item in itemList)
				{
					if(ItemLookupCache.ContainsKey(item.skillID))
					{
						Debug.LogError($"Looks like there's a duplicate RPG.UI.InventorySystem ID for objects: {ItemLookupCache[item.skillID]} and {item}");
						continue;
					}

					ItemLookupCache[item.skillID] = item;
				}
			}

			if(skillID == null || !ItemLookupCache.ContainsKey(skillID)) return null;
			return ItemLookupCache[skillID];
		}

		public void OnBeforeSerialize()
		{
			if(string.IsNullOrWhiteSpace(skillID))
			{
				skillID = Guid.NewGuid().ToString();
			}

			if(string.IsNullOrWhiteSpace(displayName))
			{
				displayName = name;
			}
		}

		public void OnAfterDeserialize()
		{
		}
	}
}