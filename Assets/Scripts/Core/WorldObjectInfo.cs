using UnityEngine;

namespace RPG.Core
{
	public class WorldObjectInfo : MonoBehaviour
	{
		[SerializeField] private string objectName;
		[SerializeField] [TextArea] private string info;

		public string ObjectName => objectName;
		public string Info => info;
	}
}