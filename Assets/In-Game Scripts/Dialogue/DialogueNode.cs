using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
	[Serializable]
	public class DialogueNode
	{
		public string uniqueID;
		public string text;
		public List<string> children = new List<string>();
		public Rect rect = new Rect(0, 0, 200, 100);

		public DialogueNode()
		{
			uniqueID = Guid.NewGuid().ToString();
		}
	}
}