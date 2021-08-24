using System;
using System.Collections.Generic;
using RPG.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace RPG.UI
{
	public class SettingsUi : MonoBehaviour
	{
		[SerializeField] private Button revertChanges, saveChanges;
		[SerializeField] private TMP_Dropdown resolutionDropdown;
		[SerializeField] private Slider musicVolume, cameraRotationSpeed, cameraZoomSpeed;

		[Header("References")] [SerializeField]
		private AudioMixer musicMixer;

		[SerializeField] private CameraZoomController zoomController;
		[SerializeField] private CameraController cameraRotator;

		private Resolution[] _resolutions;
		private float _currentVolume;
		private bool _shouldApplyChanges;

		private void Awake()
		{
			_shouldApplyChanges = GlobalGameState.GameState == GameState.InGame;
			SetupResolutions();
		}

		private void Start() => LoadSettings();

		private void SetupResolutions()
		{
			_resolutions = Screen.resolutions;
			resolutionDropdown.ClearOptions();
			var currentResolutionIndex = 0;
			var options = new List<string>();
			for (var i = 0; i < _resolutions.Length; i++)
			{
				var resolution = _resolutions[i];
				options.Add($"{resolution.width} x {resolution.height}");
				if (_resolutions[i].width == Screen.currentResolution.width && _resolutions[i].height == Screen.currentResolution.height)
				{
					currentResolutionIndex = i;
				}
			}

			resolutionDropdown.AddOptions(options);
			resolutionDropdown.SetValueWithoutNotify(currentResolutionIndex);
			resolutionDropdown.RefreshShownValue();
		}

		#region Public

		public void OnChangesMade(bool toggle)
		{
			revertChanges.interactable = toggle;
			saveChanges.interactable = toggle;
		}

		public void SetZoomSpeed(float value)
		{
			if (_shouldApplyChanges) zoomController.ZoomSpeed = value;
		}

		public void SetRotationSpeed(float value)
		{
			if (_shouldApplyChanges) cameraRotator.RotationAdjustedSpeed = value;
		}

		public void SetResolution(int resolutionIndex)
		{
			if (!_shouldApplyChanges) return; 
			var resolution = _resolutions[resolutionIndex];
			Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
		}

		public void SetVolume(float value)
		{
			musicMixer.SetFloat("Volume", Mathf.Log10(value) * 20);
			_currentVolume = value;
		}

		public void SaveSettings()
		{
			PlayerPrefs.SetInt(GlobalValues.Resolution, resolutionDropdown.value);
			PlayerPrefs.SetFloat(GlobalValues.MusicVolume, _currentVolume);
			PlayerPrefs.SetFloat(GlobalValues.CameraRotationSpeed, cameraRotationSpeed.value);
			PlayerPrefs.SetFloat(GlobalValues.CameraZoomSpeed, cameraZoomSpeed.value);
			OnChangesMade(false);
			// LogPlayerPrefs();
		}

		public void LoadSettings()
		{
			OnChangesMade(false);
			resolutionDropdown.value = PlayerPrefs.HasKey(GlobalValues.Resolution) ? PlayerPrefs.GetInt(GlobalValues.Resolution) : 0;
			musicVolume.value = PlayerPrefs.HasKey(GlobalValues.MusicVolume) ? PlayerPrefs.GetFloat(GlobalValues.MusicVolume) : 1;
			cameraRotationSpeed.value = PlayerPrefs.HasKey(GlobalValues.CameraRotationSpeed) ? PlayerPrefs.GetFloat(GlobalValues.CameraRotationSpeed) : 15f;
			cameraZoomSpeed.value = PlayerPrefs.HasKey(GlobalValues.CameraZoomSpeed) ? PlayerPrefs.GetFloat(GlobalValues.CameraZoomSpeed) : 5;
			// LogPlayerPrefs();
		}

		public void LoadDefaultSettings()
		{
			musicVolume.value = 1;
			cameraRotationSpeed.value = 15f;
			cameraZoomSpeed.value = 5;
			SaveSettings();
		}

		private void LogPlayerPrefs()
		{
			Debug.Log($"Resolution {PlayerPrefs.GetInt(GlobalValues.Resolution)}" );
			Debug.Log($"Music Volume {PlayerPrefs.GetFloat(GlobalValues.MusicVolume)}" );
			Debug.Log($"Camera Rotation Speed {PlayerPrefs.GetFloat(GlobalValues.CameraRotationSpeed)}" );
			Debug.Log($"Camera Zoom Speed {PlayerPrefs.GetFloat(GlobalValues.CameraZoomSpeed)}" );
		}

		#endregion
	}
}