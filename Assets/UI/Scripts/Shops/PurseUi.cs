using System;
using RPG.Core;
using RPG.Inventories;
using TMPro;
using UnityEngine;

namespace RPG.UI
{
	public class PurseUi : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI balanceField;

		private Purse _playerPurse;

		private void Start()
		{
			_playerPurse = PlayerFinder.Player.GetComponent<Purse>();
			_playerPurse.OnChange += RefreshUi;
			RefreshUi(0);
		}

		private void OnDestroy() => _playerPurse.OnChange -= RefreshUi;

		private void RefreshUi(float changedBy) => balanceField.SetText($"{_playerPurse.Balance:N1}");
	}
}