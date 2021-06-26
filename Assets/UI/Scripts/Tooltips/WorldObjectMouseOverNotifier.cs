using RPG.Core;
using RPG.Core.SystemEvents;
using UnityEngine;

namespace RPG.UI.Tooltips
{
	[RequireComponent(typeof(WorldObjectInfo))]
	public class WorldObjectMouseOverNotifier : MonoBehaviour
	{
		[SerializeField] private WorldObjectMouseOverEvent objectEvent;

		private WorldObjectInfo _objectInfo;
		private WorldObjectTooltipData _data;

		private void Awake() => _objectInfo = GetComponent<WorldObjectInfo>();

		private void OnMouseEnter()
		{
			_data.mouseEnter = true;
			_data.objectInfo = _objectInfo.Info;
			_data.objectName = _objectInfo.ObjectName;
			objectEvent.Raise(_data);
		}

		private void OnMouseExit()
		{
			_data.mouseEnter = false;
			objectEvent.Raise(_data);
		}
	}
}