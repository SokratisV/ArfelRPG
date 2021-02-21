using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.UI.Tooltips
{
	/// <summary>
	/// Abstract base class that handles the spawning of a tooltip prefab at the
	/// correct position on screen relative to a cursor.
	/// 
	/// Override the abstract functions to create a tooltip spawner for your own
	/// data.
	/// </summary>
	public abstract class TooltipSpawner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		[Tooltip("The prefab of the tooltip to spawn.")] [SerializeField]
		private GameObject tooltipPrefab = null;
		private GameObject _tooltip = null;

		/// <summary>
		/// Called when it is time to update the information on the tooltip
		/// prefab.
		/// </summary>
		/// <param name="tooltip">
		/// The spawned tooltip prefab for updating.
		/// </param>
		public abstract void UpdateTooltip(GameObject tooltip);

		/// <summary>
		/// Return true when the tooltip spawner should be allowed to create a tooltip.
		/// </summary>
		public abstract bool CanCreateTooltip();

		private void OnDestroy() => ClearTooltip();

		private void OnDisable() => ClearTooltip();

		void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
		{
			var parentCanvas = GetComponentInParent<Canvas>();

			if(_tooltip && !CanCreateTooltip())
			{
				ClearTooltip();
			}

			if(!_tooltip && CanCreateTooltip())
			{
				_tooltip = Instantiate(tooltipPrefab, parentCanvas.transform);
			}

			if(_tooltip)
			{
				UpdateTooltip(_tooltip);
				PositionTooltip();
			}
		}

		private void PositionTooltip()
		{
			// Required to ensure corners are updated by positioning elements.
			Canvas.ForceUpdateCanvases();

			var tooltipCorners = new Vector3[4];
			_tooltip.GetComponent<RectTransform>().GetWorldCorners(tooltipCorners);
			var slotCorners = new Vector3[4];
			GetComponent<RectTransform>().GetWorldCorners(slotCorners);

			var below = transform.position.y > Screen.height / 2;
			var right = transform.position.x < Screen.width / 2;

			var slotCorner = GetCornerIndex(below, right);
			var tooltipCorner = GetCornerIndex(!below, !right);

			_tooltip.transform.position = slotCorners[slotCorner] - tooltipCorners[tooltipCorner] + _tooltip.transform.position;
		}

		private int GetCornerIndex(bool below, bool right)
		{
			return below switch
			{
				true when!right => 0,
				false when!right => 1,
				false when true => 2,
				_ => 3
			};
		}

		void IPointerExitHandler.OnPointerExit(PointerEventData eventData) => ClearTooltip();

		private void ClearTooltip()
		{
			if(_tooltip)
			{
				Destroy(_tooltip.gameObject);
			}
		}
	}
}