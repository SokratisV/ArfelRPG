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
		private const string Resolution = "resolution";
		private const string MusicVolume = "musicVolume";
		private const string CameraRotationSpeed = "cameraRotationSpeed";
		private const string CameraZoomSpeed = "cameraZoomSpeed";

		[SerializeField] private Button revertChanges, saveChanges;
		[SerializeField] private TMP_Dropdown resolutionDropdown;
		[SerializeField] private Slider musicVolume, cameraRotationSpeed, cameraZoomSpeed;

		[Header("References")] [SerializeField]
		private AudioMixer musicMixer;

		[SerializeField] private CameraZoomControl zoomController;
		[SerializeField] private CameraController cameraRotator;

		private Resolution[] _resolutions;
		private float _currentVolume;

		private void Awake() => SetupResolutions();

		private void OnEnable() => LoadSettings();

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

		public void SetZoomSpeed(float value) => zoomController.ZoomSpeed = value;

		public void SetRotationSpeed(float value) => cameraRotator.RotationAdjustedSpeed = value;

		public void SetResolution(int resolutionIndex)
		{
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
			PlayerPrefs.SetInt(Resolution, resolutionDropdown.value);
			PlayerPrefs.SetFloat(MusicVolume, _currentVolume);
			PlayerPrefs.SetFloat(CameraRotationSpeed, cameraRotationSpeed.value);
			PlayerPrefs.SetFloat(CameraZoomSpeed, cameraZoomSpeed.value);
			OnChangesMade(false);
			LogPlayerPrefs();
		}

		public void LoadSettings()
		{
			OnChangesMade(false);
			resolutionDropdown.value = PlayerPrefs.HasKey(Resolution) ? PlayerPrefs.GetInt(Resolution) : 0;
			musicVolume.value = PlayerPrefs.HasKey(MusicVolume) ? PlayerPrefs.GetFloat(MusicVolume) : 1;
			cameraRotationSpeed.value = PlayerPrefs.HasKey(CameraRotationSpeed) ? PlayerPrefs.GetFloat(CameraRotationSpeed) : 15f;
			cameraZoomSpeed.value = PlayerPrefs.HasKey(CameraZoomSpeed) ? PlayerPrefs.GetFloat(CameraZoomSpeed) : 5;
			LogPlayerPrefs();
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
			Debug.Log($"Resolution {PlayerPrefs.GetInt(Resolution)}" );
			Debug.Log($"Music Volume {PlayerPrefs.GetFloat(MusicVolume)}" );
			Debug.Log($"Camera Rotation Speed {PlayerPrefs.GetFloat(CameraRotationSpeed)}" );
			Debug.Log($"Camera Zoom Speed {PlayerPrefs.GetFloat(CameraZoomSpeed)}" );
		}

		#endregion
	}
}