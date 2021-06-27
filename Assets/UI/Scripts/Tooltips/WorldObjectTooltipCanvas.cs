using System;
using System.Collections;
using RPG.Core;
using RPG.Core.SystemEvents;
using RPG.UI.Tooltips;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Scripts.Tooltips
{
	[RequireComponent(typeof(Canvas))]
	public class WorldObjectTooltipCanvas : MonoBehaviour
	{
		[Tooltip("The prefab of the tooltip to spawn.")] [SerializeField]
		private GameObject tooltipPrefab = null;
		[SerializeField] private Vector3 tooltipOffset;

		private GameObject _tooltip = null;
		private Canvas _canvas;
		private Coroutine _followCursorRoutine;

		private void Awake() => _canvas = GetComponent<Canvas>();

		public void OnWorldObjectNotification(WorldObjectTooltipData data)
		{
			if (data.mouseEnter == false)
			{
				ClearTooltip();
			}
			else
			{
				ClearTooltip();
				_tooltip = Instantiate(tooltipPrefab, _canvas.transform);

				if (_tooltip)
				{
					UpdateTooltip(_tooltip, data);
					PositionTooltip();
				}
			}
		}

		private void UpdateTooltip(GameObject tooltip, WorldObjectTooltipData data) => tooltip.GetComponent<WorldObjectTooltip>().Setup(data);
		private void PositionTooltip() => _followCursorRoutine = _followCursorRoutine.StartCoroutine(this, FollowCursor());

		private void ClearTooltip()
		{
			if (_tooltip)
			{
				_followCursorRoutine.StopCoroutine(this);
				Destroy(_tooltip.gameObject);
				_tooltip = null;
			}
		}

		private IEnumerator FollowCursor()
		{
			while (_tooltip)
			{
				_tooltip.SetActive(!EventSystem.current.IsPointerOverGameObject());
				_tooltip.transform.position = Input.mousePosition + tooltipOffset;
				yield return null;
			}
		}
	}
}