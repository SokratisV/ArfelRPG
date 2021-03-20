﻿using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Skills.Behaviors;
using UnityEngine;

namespace RPG.Skills
{
	public class SkillData
	{
		public readonly GameObject User;
		public readonly GameObject InitialTarget;
		public readonly List<GameObject> Targets;
		public IEnumerator UpdateBehavior;
		public Vector3? Point;

		public SkillData(GameObject user, GameObject initialTarget, Vector3? point, List<GameObject> targets, IEnumerator updateBehavior)
		{
			User = user;
			InitialTarget = initialTarget;
			Point = point;
			Targets = targets;
			UpdateBehavior = updateBehavior;
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

		[SerializeField] private float cooldown;
		[SerializeField] private bool canBeCancelled;
		[SerializeField] private TargetBehavior targetBehavior;
		[SerializeField] private SkillBehavior skillBehavior;

		[Header("On Start")] [Space(15)] [SerializeField]
		private GameObject[] vfxOnUserStart;

		[SerializeField] private GameObject[] vfxOnTargetStart;
		[SerializeField] private AudioClip[] sfxOnUserStart, sfxOnTargetStart;

		[Header("On End")] [Space(15)] [SerializeField]
		private GameObject[] vfxOnUserEnd;

		[SerializeField] private GameObject[] vfxOnTargetEnd;
		[SerializeField] private AudioClip[] sfxOnUserEnd, sfxOnTargetEnd;

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
		public int AnimationHash => skillBehavior.SkillAnimationNumber();

		private static Dictionary<string, Skill> ItemLookupCache;

		public SkillData OnStart(GameObject user, GameObject initialTarget = null, Vector3? point = null)
		{
			//if interested in targets, but targets are null, return
			if(targetBehavior.GetTargets(out var targets, user, initialTarget, point))
				if(targets == null)
					return null;

			skillBehavior.OnStart += StartVfxSfx;
			skillBehavior.OnEnd += EndVfxSfx;
			skillBehavior.BehaviorStart(user, targets, point);
			return new SkillData(user, initialTarget, point, targets, skillBehavior.BehaviorUpdate(user, targets, point));
		}
		
		public void OnEnd(SkillData data) => skillBehavior.BehaviorEnd(data.User, data.Targets, data.Point);

		private void StartVfxSfx(GameObject user, List<GameObject> targets)
		{
			foreach(var vfx in vfxOnUserStart)
			{
				Destroy(Instantiate(vfx, user.transform), 2f);
			}

			if(user.TryGetComponent(out IAudioPlayer audioPlayer))
			{
				audioPlayer.PlaySound(sfxOnUserStart);
			}

			if(targets == null) return;
			foreach(var target in targets)
			{
				if(target == null) continue;
				foreach(var vfx in vfxOnTargetStart)
				{
					Destroy(Instantiate(vfx, target.transform), 2f);
				}

				if(target.TryGetComponent(out audioPlayer))
				{
					audioPlayer.PlaySound(sfxOnTargetStart);
				}
			}
		}

		private void EndVfxSfx(GameObject user, List<GameObject> targets)
		{
			foreach(var vfx in vfxOnUserEnd)
			{
				Destroy(Instantiate(vfx, user.transform), 2f);
			}

			if(user.TryGetComponent(out IAudioPlayer audioPlayer))
			{
				audioPlayer.PlaySound(sfxOnUserEnd);
			}

			if(targets == null) return;
			foreach(var target in targets)
			{
				if(target == null) continue;
				foreach(var vfx in vfxOnTargetEnd)
				{
					Destroy(Instantiate(vfx, target.transform), 2f);
				}

				if(target.TryGetComponent(out audioPlayer))
				{
					audioPlayer.PlaySound(sfxOnTargetEnd);
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