﻿using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue
{
	[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/New Dialogue", order = 0)]
	public class Dialogue : ScriptableObject, ISerializationCallbackReceiver
	{
		[SerializeField] private List<DialogueNode> nodes = new List<DialogueNode>();

		private Dictionary<string, DialogueNode> _dialogueNodes = new Dictionary<string, DialogueNode>();

		private void OnValidate()
		{
			if(nodes.Count == 0)
			{
				CreateNode(null);
			}

			_dialogueNodes.Clear();
			foreach(var node in nodes)
			{
				_dialogueNodes[node.name] = node;
			}
		}

		public IEnumerable<DialogueNode> GetAllNodes() => nodes;

		public DialogueNode GetRootNode() => nodes[0];

		public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode)
		{
			foreach(var child in parentNode.Children)
			{
				if(_dialogueNodes.TryGetValue(child, out var value))
				{
					yield return value;
				}
			}
		}

#if UNITY_EDITOR
		public void CreateNode(DialogueNode parent)
		{
			var node = MakeNode(parent);
			Undo.RegisterCreatedObjectUndo(node, "Created Dialogue Node");
			Undo.RecordObject(this, "Added Dialogue Node");
			AddNode(node);
		}

		public void DeleteNode(DialogueNode nodeToDelete)
		{
			Undo.RecordObject(this, "Deleted Dialogue Node");
			nodes.Remove(nodeToDelete);
			OnValidate();
			CleanConnectedChildren(nodeToDelete);
			Undo.DestroyObjectImmediate(nodeToDelete);
		}

		private void CleanConnectedChildren(DialogueNode nodeToDelete)
		{
			foreach(var dialogueNode in GetAllNodes())
			{
				dialogueNode.RemoveChild(nodeToDelete.name);
			}
		}

		private void AddNode(DialogueNode node)
		{
			nodes.Add(node);
			OnValidate();
		}

		private static DialogueNode MakeNode(DialogueNode parent)
		{
			var node = CreateInstance<DialogueNode>();
			node.name = System.Guid.NewGuid().ToString();
			if(parent != null) parent.AddChild(node.name);
			return node;
		}
#endif

		public void OnBeforeSerialize()
		{
			if(nodes.Count == 0)
			{
				var newNode = MakeNode(null);
				AddNode(newNode);
			}

			if(string.IsNullOrEmpty(AssetDatabase.GetAssetPath(this))) return;
			foreach(var node in GetAllNodes())
			{
				if(string.IsNullOrEmpty(AssetDatabase.GetAssetPath(node)))
				{
					AssetDatabase.AddObjectToAsset(node, this);
				}
			}
		}

		public void OnAfterDeserialize()
		{
		}
	}
}