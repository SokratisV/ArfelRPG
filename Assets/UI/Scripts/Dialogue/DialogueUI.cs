using RPG.Core;
using RPG.Dialogue;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
	public class DialogueUI : MonoBehaviour
	{
		private PlayerConversant _playerConversant;
		[SerializeField] private ShowHideUIOnButtonPress showHideUIOnButtonPress;
		[SerializeField] private TextMeshProUGUI aiText;
		[SerializeField] private Button nextButton;
		[SerializeField] private Button quitButton;
		[SerializeField] private GameObject nextButtonParent;
		[SerializeField] private Transform choicesButtons;
		[SerializeField] private GameObject choicePrefab;
		[SerializeField] private TextMeshProUGUI currentSpeaker;

		private void Start()
		{
			_playerConversant = PlayerFinder.Player.GetComponent<PlayerConversant>();
			_playerConversant.OnUpdated += UpdateUI;
			nextButton.onClick.AddListener(Next);
			quitButton.onClick.AddListener(_playerConversant.Quit);
			UpdateUI(null);
		}

		private void Next()
		{
			_playerConversant.Next();
			UpdateUI(null);
		}

		private void UpdateUI(bool? status)
		{
			if(status == true || status == false) showHideUIOnButtonPress.Toggle(status.Value);
			if(!_playerConversant.IsActive) return;
			currentSpeaker.SetText(_playerConversant.Name);
			nextButtonParent.SetActive(!_playerConversant.IsChoosing);
			choicesButtons.gameObject.SetActive(_playerConversant.IsChoosing);
			if(_playerConversant.IsChoosing)
			{
				CreatePlayerChoiceButtons();
			}
			else
			{
				aiText.text = _playerConversant.GetText();
				nextButton.gameObject.SetActive(_playerConversant.HasNext);
			}
		}

		private void CreatePlayerChoiceButtons()
		{
			foreach(Transform item in choicesButtons)
			{
				Destroy(item.gameObject);
			}

			foreach(var choice in _playerConversant.GetChoices())
			{
				var instance = Instantiate(choicePrefab, choicesButtons);
				instance.GetComponentInChildren<TextMeshProUGUI>().SetText(choice.Text);
				var button = instance.GetComponentInChildren<Button>();
				button.onClick.AddListener(() => {_playerConversant.SelectChoice(choice);});
			}
		}
	}
}