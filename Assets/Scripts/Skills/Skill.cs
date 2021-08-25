using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Skills.Behaviors;
using RPG.Skills.TargetFiltering;
using UnityEngine;

namespace RPG.Skills
{
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

		[SerializeField] private float cooldown;
		[SerializeField] private bool canBeCancelled;
		[SerializeField] private TargetBehavior targetBehavior;
		[SerializeField] private SkillBehavior skillBehavior;
		[SerializeField] private FilterStrategy[] filterStrategy;
		[SerializeField] private EffectsStrategy[] startFx;
		[SerializeField] private EffectsStrategy[] endFx;

		public Sprite Icon => icon;
		public string SkillID => skillID;
		public string DisplayName => displayName;
		public string Description => description;
		public float Duration => skillBehavior.Duration;
		public float Cooldown => cooldown;
		public float Radius => targetBehavior.GetRadius();
		public float CastingRange => skillBehavior.GetCastingRange();
		public float MinClickDistance => targetBehavior.GetMinRange();
		public bool? RequiresTarget => targetBehavior.RequireTarget();
		public bool CanTargetSelf => skillBehavior.CanTargetSelf;
		public bool MoveInRangeBeforeCasting => skillBehavior.MoveInRangeBefore;
		public bool HasExtraAnimation => skillBehavior.UseExtraAnimation();
		public bool CanBeCancelled => canBeCancelled;
		public bool HasCastTime => skillBehavior.HasCastTime();
		public bool AdjustAnimationSpeed => skillBehavior.AdjustAnimationSpeed;
		public int AnimationHash => skillBehavior.SkillAnimationNumber();
		public IndicatorType IndicatorType => skillBehavior.IndicatorType;
		public float[] SpecialFloats => skillBehavior.SpecialFloats();

		private static Dictionary<string, Skill> SkillLookupCache;

		public (SkillData, IEnumerator)? OnStart(GameObject user, GameObject initialTarget = null, Vector3? point = null)
		{
			//if interested in targets, but targets are null, return
			if (targetBehavior.GetTargets(out var targets, user, initialTarget, point))
				if (targets == null)
					return null;

			targets = FilterTargets(targets);
			var skillData = new SkillData(user, initialTarget, point, targets);
			skillBehavior.OnStart += StartFX;
			skillBehavior.OnEnd += EndFX;
			skillBehavior.BehaviorStart(skillData);

			return (skillData, skillBehavior.BehaviorUpdate(skillData));
		}

		private List<GameObject> FilterTargets(List<GameObject> targets)
		{
			foreach (var filter in filterStrategy)
			{
				targets = filter.Filter(targets);
			}

			return targets;
		}

		public void OnEnd(SkillData data) => skillBehavior.BehaviorEnd(data);

		private void StartFX(SkillData data)
		{
			foreach (var effect in startFx)
			{
				effect.ExecuteStrategy(data);
			}
		}

		private void EndFX(SkillData data)
		{
			foreach (var effect in endFx)
			{
				effect.ExecuteStrategy(data);
			}
		}

		public static Skill GetFromID(string skillID)
		{
			if (SkillLookupCache == null)
			{
				SkillLookupCache = new Dictionary<string, Skill>();
				var itemList = Resources.LoadAll<Skill>("");
				foreach (var item in itemList)
				{
					if (SkillLookupCache.ContainsKey(item.skillID))
					{
						Debug.LogError($"Looks like there's a duplicate RPG.UI.InventorySystem ID for objects: {SkillLookupCache[item.skillID]} and {item}");
						continue;
					}

					SkillLookupCache[item.skillID] = item;
				}
			}

			if (skillID == null || !SkillLookupCache.ContainsKey(skillID)) return null;
			return SkillLookupCache[skillID];
		}

		public void OnBeforeSerialize()
		{
			if (string.IsNullOrWhiteSpace(skillID))
			{
				skillID = Guid.NewGuid().ToString();
			}

			if (string.IsNullOrWhiteSpace(displayName))
			{
				displayName = name;
			}
		}

		public void OnAfterDeserialize()
		{
		}

		public void OnAnimationEvent() => skillBehavior.OnAnimationEvent();
	}
}