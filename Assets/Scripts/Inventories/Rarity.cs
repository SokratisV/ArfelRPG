using System;
using UnityEngine;

namespace RPG.Inventories
{
	[CreateAssetMenu(fileName = "Rarity", menuName = "RPG/Inventory/New Rarity")]
	public class Rarity : ScriptableObject, ISerializationCallbackReceiver
	{
		[SerializeField] private string rarityName;
		[SerializeField] private Color32 color;

		public Color32 Color => color;

		public string RarityName => rarityName;

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			if(string.IsNullOrWhiteSpace(RarityName))
			{
				rarityName = name;
			}
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			// Require by the ISerializationCallbackReceiver but we don't need
			// to do anything with it.
		}
	}
}