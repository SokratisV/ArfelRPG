using RPG.Dialogue;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
	public class DialogueUI : MonoBehaviour
	{
		private PlayerConversant _playerConversant;
		[SerializeField] private TextMeshProUGUI aiText;
		[SerializeField] private Button nextButton;
		[SerializeField] private Button quitButton;
		[SerializeField] private GameObject nextButtonParent;
		[SerializeField] private Transform choicesButtons;
		[SerializeField] private GameObject choicePrefab;
		[SerializeField] private TextMeshProUGUI currentSpeaker;
		

		private void Start()
		{
			_playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
			_playerConversant.OnUpdated += UpdateUI;
			nextButton.onClick.AddListener(Next);
			quitButton.onClick.AddListener(_playerConversant.Quit);
			UpdateUI();
		}

		private void Next()
		{
			_playerConversant.Next();
			UpdateUI();
		}

		private void UpdateUI()
		{
			gameObject.SetActive(_playerConversant.IsActive);
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