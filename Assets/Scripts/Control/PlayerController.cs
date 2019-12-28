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
        RaycastHit[] hits;
        RaycastHit movementRaycast;
        Health health;
        SpawnFeedback movementFeedbackPrefab;
        [SerializeField] float maxNavMeshProjectionDistance = 1f, raycastRadius = 1f;

        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] CursorMapping[] cursorMappings = null;

        private void Awake()
        {
            health = GetComponent<Health>();
            movementFeedbackPrefab = GetComponent<SpawnFeedback>();
        }

        private void Update()
        {
            // Unnecessary for now
            // if (InteractWithUI()) { return; }
            if (health.IsDead())
            {
                SetCursor(CursorType.None);
                return;
            }
            if (InteractWithComponent()) return;
            if (InteractWithMovement()) return;

            SetCursor(CursorType.None);
        }

        private bool InteractWithComponent()
        {
            hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
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
            RaycastHit[] raycastHit = Physics.SphereCastAll(GetMouseRay(), raycastRadius);
            float[] distances = new float[hits.Length];
            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            Array.Sort(distances, hits);
            return hits;
        }

        private bool InteractWithUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                return true;
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            Vector3 target;
            bool hasHit = RaycastNavMesh(out target);
            if (hasHit)
            {
                if (!GetComponent<Mover>().CanMoveTo(target)) return false;
                CheckControlsPressed(target);
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }

        private void CheckControlsPressed(Vector3 target)
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    GetComponent<Mover>().QueueMoveAction(target, 1f);
                }
            }
            else
            {
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(target, 1f);
                }
            }
            if (Input.GetMouseButtonDown(0)) { MovementFeedback(target); }
        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();

            bool hasHit = Physics.Raycast(GetMouseRay(), out movementRaycast);
            if (!hasHit) return false;

            NavMeshHit navMeshHit;
            bool hasCastToNavMesh = NavMesh.SamplePosition(
                movementRaycast.point, out navMeshHit, maxNavMeshProjectionDistance, NavMesh.AllAreas);
            if (!hasCastToNavMesh) return false;

            target = navMeshHit.position;

            if (!GetComponent<Mover>().CanMoveTo(target)) return false;
            return true;
        }

        private void MovementFeedback(Vector3 target)
        {
            if (movementFeedbackPrefab != null)
            {
                movementFeedbackPrefab.Spawn(target, movementRaycast.normal);
            }
        }

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping mapping in cursorMappings)
            {
                if (mapping.type == type) { return mapping; }
            }
            return cursorMappings[0];
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}