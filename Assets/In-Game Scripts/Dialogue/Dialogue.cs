using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
	[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/New Dialogue", order = 0)]
	public class Dialogue : ScriptableObject
	{
		[SerializeField] private List<DialogueNode> nodes = new List<DialogueNode>();

		private Dictionary<string, DialogueNode> _dialogueNodes = new Dictionary<string, DialogueNode>();

#if UNITY_EDITOR
		private void Awake()
		{
			if(nodes.Count == 0) nodes.Add(new DialogueNode());
			OnValidate();
		}
#endif

		private void OnValidate()
		{
			_dialogueNodes.Clear();
			foreach(var node in nodes)
			{
				_dialogueNodes[node.uniqueID] = node;
			}
		}

		public IEnumerable<DialogueNode> GetAllNodes() => nodes;

		public DialogueNode GetRootNode() => nodes[0];

		public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode)
		{
			foreach(var child in parentNode.children)
			{
				if(_dialogueNodes.TryGetValue(child, out var value))
				{
					yield return value;
				}
			}
		}

		public void CreateNode(DialogueNode parent)
		{
			var node = new DialogueNode();
			parent.children.Add(node.uniqueID);
			nodes.Add(node);
			OnValidate();
		}

		public void DeleteNode(DialogueNode nodeToDelete)
		{
			nodes.Remove(nodeToDelete);
			OnValidate();
			CleanConnectedChildren(nodeToDelete);
		}

		private void CleanConnectedChildren(DialogueNode nodeToDelete)
		{
			foreach(var dialogueNode in GetAllNodes())
			{
				dialogueNode.children.Remove(nodeToDelete.uniqueID);
			}
		}
	}
}