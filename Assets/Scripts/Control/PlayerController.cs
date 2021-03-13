using UnityEngine;
using RPG.Movement;
using System;
using RotaryHeart.Lib.SerializableDictionary;
using RPG.Attributes;
using RPG.Core;
using RPG.Skills;
using UnityEngine.EventSystems;
using UnityEngine.AI;

namespace RPG.Control
{
	public class PlayerController : MonoBehaviour
	{
		[SerializeField] private float maxNavMeshProjectionDistance = 1f, raycastRadius;
		
		private RaycastHit[] _hits;
		private RaycastHit _movementRaycast;
		private Health _health;
		private SpawnFeedback _movementFeedbackPrefab;
		private Mover _mover;
		private SkillUser _skillUser;
		private Camera _mainCamera;
		private bool _isDraggingUI = false;
		private bool _hasInputBeenReset = true;

		[Serializable]
		private struct CursorMapping
		{
			public Texture2D texture;
			public Vector2 hotspot;
		}

		[SerializeField] private CursorMappings cursorMappings = new CursorMappings();

		private void Awake()
		{
			_health = GetComponent<Health>();
			_movementFeedbackPrefab = GetComponent<SpawnFeedback>();
			_mover = GetComponent<Mover>();
			_skillUser = GetComponent<SkillUser>();
			_mainCamera = Camera.main;
		}

		private void Update()
		{
			if(InteractWithUI()) return;
			if(_health.IsDead)
			{
				SetCursor(CursorType.None);
				return;
			}

			ResetInput();
			if(HandleSkillUsage()) return;
			if(InteractWithComponent()) return;
			if(InteractWithMovement()) return;

			SetCursor(CursorType.None);
		}

		private bool HandleSkillUsage()
		{
			if(!_skillUser.IsPreparingSkill && !_skillUser.CanCurrentSkillBeUsed) return false;
			if(_skillUser.SkillRequiresTarget == null)
			{
				_skillUser.Execute(gameObject);
				return true;
			}

			if(_skillUser.SkillRequiresTarget.Value)
			{
				if(RaycastForSkillTarget()) return true;
				SetCursor(CursorType.None);
				return true;
			}

			if(HasHitNavMesh(CursorType.Skill, _skillUser.CanExecute)) return true;
			SetCursor(CursorType.None);
			return true;
		}

		private bool RaycastForSkillTarget()
		{
			_hits = RaycastAllSorted();
			foreach(var hit in _hits)
			{
				var skillcastables = hit.transform.GetComponents<ISkillcastable>();
				foreach(var raycastable in skillcastables)
				{
					if(raycastable.HandleSkillcast(gameObject))
					{
						raycastable.ShowInteractivity();
						SetCursor(raycastable.GetSkillCursorType());
						return true;
					}
				}
			}

			return false;
		}

		private bool InteractWithComponent()
		{
			_hits = RaycastAllSorted();
			foreach(var hit in _hits)
			{
				var raycastables = hit.transform.GetComponents<IRaycastable>();
				foreach(var raycastable in raycastables)
				{
					if(raycastable.HandleRaycast(gameObject))
					{
						raycastable.ShowInteractivity();
						SetCursor(raycastable.GetCursorType());
						return true;
					}
				}
			}

			return false;
		}

		private RaycastHit[] RaycastAllSorted()
		{
			var hits = Physics.SphereCastAll(GetMouseRay(), raycastRadius);
			var distances = new float[hits.Length];
			for(var i = 0;i < hits.Length;i++)
			{
				distances[i] = hits[i].distance;
			}

			Array.Sort(distances, hits);
			return hits;
		}

		private bool InteractWithUI()
		{
			if(Input.GetMouseButtonUp(0)) _isDraggingUI = false;
			if(EventSystem.current.IsPointerOverGameObject())
			{
				if(Input.GetMouseButtonDown(0)) _isDraggingUI = true;
				SetCursor(CursorType.UI);
				return true;
			}

			return _isDraggingUI;
		}

		private bool InteractWithMovement() => HasHitNavMesh(CursorType.Movement, _mover.CanMoveTo);

		private bool HasHitNavMesh(CursorType cursor, Func<Vector3, bool> extraCheck)
		{
			var hasHit = RaycastNavMesh(out var target, extraCheck);
			if(!hasHit) return false;
			CheckPressedButtons(target);
			SetCursor(cursor);
			return true;
		}

		private void CheckPressedButtons(Vector3 target)
		{
			if(Input.GetKey(KeyCode.LeftControl))
			{
				if(!Input.GetMouseButtonDown(0)) return;
				if(_skillUser.IsPreparingSkill) _skillUser.Execute(target);
				else
				{
					_mover.QueueMoveAction(target);
				}

				MovementFeedback(target);
			}
			else
			{
				if(!Input.GetMouseButton(0)) return;
				if(!_hasInputBeenReset) return;

				if(_skillUser.IsPreparingSkill)
				{
					_hasInputBeenReset = false;
					_skillUser.Execute(target);
				}
				else
				{
					_skillUser.CancelAction();
					_mover.Move(target);
				}

				MovementFeedback(target);
			}
		}

		private void ResetInput()
		{
			if(Input.GetMouseButtonUp(0)) _hasInputBeenReset = true;
			if(Input.GetMouseButtonDown(1) && _skillUser.CanCurrentSkillBeCancelled) _skillUser.CancelAction();
		}

		private bool RaycastNavMesh(out Vector3 target, Func<Vector3, bool> extraCheck)
		{
			target = new Vector3();
			var hasHit = Physics.Raycast(GetMouseRay(), out _movementRaycast);
			if(!hasHit) return false;

			var hasCastToNavMesh = NavMesh.SamplePosition(_movementRaycast.point, out var navMeshHit, maxNavMeshProjectionDistance, NavMesh.AllAreas);
			if(!hasCastToNavMesh) return false;

			target = navMeshHit.position;
			return extraCheck.Invoke(target);
		}

		private void MovementFeedback(Vector3 target)
		{
			if(Input.GetMouseButtonDown(0)) _movementFeedbackPrefab.Spawn(target, _movementRaycast.normal);
		}

		private void SetCursor(CursorType type)
		{
			var mapping = GetCursorMapping(type);
			Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
		}

		private CursorMapping GetCursorMapping(CursorType type) => cursorMappings.TryGetValue(type, out var mapping)? mapping:cursorMappings[0];

		private Ray GetMouseRay() => _mainCamera.ScreenPointToRay(Input.mousePosition);

		[Serializable]
		private class CursorMappings : SerializableDictionaryBase<CursorType, CursorMapping>
		{
		}
	}
}