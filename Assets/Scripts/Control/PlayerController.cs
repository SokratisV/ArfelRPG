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
		private RaycastHit[] _hits;
		private RaycastHit _movementRaycast;
		private Health _health;
		private SpawnFeedback _movementFeedbackPrefab;
		private Mover _mover;
		private SkillUser _skillUser;
		private Camera _mainCamera;
		private bool _isDraggingUI = false;
		[SerializeField] private float maxNavMeshProjectionDistance = 1f, raycastRadius;

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

			if(HandleSkillUsage()) return;
			if(InteractWithComponent()) return;
			if(InteractWithMovement()) return;

			SetCursor(CursorType.None);
		}

		private bool HandleSkillUsage()
		{
			if(!_skillUser.IsPreparingSkill) return false;
			if(_skillUser.SkillRequiresTarget == null)
			{
				_skillUser.Execute(gameObject);
			}
			else
			{
				if(_skillUser.SkillRequiresTarget.Value)
				{
					if(RaycastForSkillTarget()) return true;
				}
				else
				{
					if(HasHitNavMesh(CursorType.Skill)) return true;
				}
			}

			return false;
		}

		private bool RaycastForSkillTarget()
		{
			_hits = RaycastAllSorted();
			foreach(var hit in _hits)
			{
				var raycastables = hit.transform.GetComponents<ISkillcastable>();
				foreach(var raycastable in raycastables)
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

		private bool InteractWithMovement() => HasHitNavMesh(CursorType.Movement);

		private bool HasHitNavMesh(CursorType cursor)
		{
			var hasHit = RaycastNavMesh(out var target);
			if(!hasHit) return false;
			CheckPressedButtons(target);
			SetCursor(cursor);
			return true;
		}

		private void CheckPressedButtons(Vector3 target)
		{
			if(Input.GetMouseButton(1) && _skillUser.IsPreparingSkill) _skillUser.CancelAction();
			if(Input.GetKey(KeyCode.LeftControl))
			{
				if(!Input.GetMouseButtonDown(0)) return;
				if(_skillUser.IsPreparingSkill) _skillUser.Execute(target);
				else
				{
					_mover.QueueMoveAction(target);
					MovementFeedback(target);
				}
			}
			else
			{
				if(!Input.GetMouseButton(0)) return;
				if(_skillUser.IsPreparingSkill) _skillUser.Execute(target);
				else
				{
					_mover.Move(target);
					MovementFeedback(target);
				}
			}
		}

		private bool RaycastNavMesh(out Vector3 target)
		{
			target = new Vector3();
			var hasHit = Physics.Raycast(GetMouseRay(), out _movementRaycast);
			if(!hasHit) return false;

			var hasCastToNavMesh = NavMesh.SamplePosition(_movementRaycast.point, out var navMeshHit, maxNavMeshProjectionDistance, NavMesh.AllAreas);
			if(!hasCastToNavMesh) return false;

			target = navMeshHit.position;
			return _mover.CanMoveTo(target);
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