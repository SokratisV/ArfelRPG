using UnityEngine;
using RPG.Movement;
using RPG.Attributes;
using System;
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
		private Camera _mainCamera;
		[SerializeField] private float maxNavMeshProjectionDistance = 1f, raycastRadius;

		[Serializable]
		private struct CursorMapping
		{
			public CursorType type;
			public Texture2D texture;
			public Vector2 hotspot;
		}

		[SerializeField] private CursorMapping[] cursorMappings = null;

		private void Awake()
		{
			_health = GetComponent<Health>();
			_movementFeedbackPrefab = GetComponent<SpawnFeedback>();
			_mover = GetComponent<Mover>();
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

			if(InteractWithComponent()) return;
			if(InteractWithMovement()) return;

			SetCursor(CursorType.None);
		}

		private bool InteractWithComponent()
		{
			_hits = RaycastAllSorted();
			foreach(var hit in _hits)
			{
				var raycastables = hit.transform.GetComponents<IRaycastable>();
				foreach(var raycastable in raycastables)
				{
					if(raycastable.HandleRaycast(this))
					{
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
			if(EventSystem.current.IsPointerOverGameObject())
			{
				SetCursor(CursorType.UI);
				return true;
			}

			return false;
		}

		private bool InteractWithMovement()
		{
			var hasHit = RaycastNavMesh(out var target);
			if(hasHit)
			{
				if(!_mover.CanMoveTo(target)) return false;
				CheckPressedButtons(target);
				SetCursor(CursorType.Movement);
				return true;
			}

			return false;
		}

		private void CheckPressedButtons(Vector3 target)
		{
			if(Input.GetKey(KeyCode.LeftControl))
			{
				if(Input.GetMouseButtonDown(0))
				{
					_mover.QueueMoveAction(target);
				}
			}
			else
			{
				if(Input.GetMouseButton(0))
				{
					_mover.Move(target);
				}
			}

			if(Input.GetMouseButtonDown(0))
			{
				MovementFeedback(target);
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
			if(_movementFeedbackPrefab != null)
			{
				_movementFeedbackPrefab.Spawn(target, _movementRaycast.normal);
			}
		}

		private void SetCursor(CursorType type)
		{
			var mapping = GetCursorMapping(type);
			Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
		}

		private CursorMapping GetCursorMapping(CursorType type)
		{
			foreach(var mapping in cursorMappings)
			{
				if(mapping.type == type)
				{
					return mapping;
				}
			}

			return cursorMappings[0];
		}

		private Ray GetMouseRay() => _mainCamera.ScreenPointToRay(Input.mousePosition);
	}
}