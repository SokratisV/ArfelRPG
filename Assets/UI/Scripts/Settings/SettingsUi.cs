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

		[SerializeField] private TMP_Dropdown resolutionDropdown;
		[SerializeField] private Slider musicVolume, cameraRotationSpeed, cameraZoomSpeed;

		[Header("References")] [SerializeField]
		private AudioMixer musicMixer;

		[SerializeField] private CameraZoomControl zoomController;
		[SerializeField] private CameraController cameraRotator;


		private Resolution[] _resolutions;
		private float _currentVolume;

		private void Start() => SetupResolutions();

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
			resolutionDropdown.value = currentResolutionIndex;
			resolutionDropdown.RefreshShownValue();
		}

		public void SetZoomSpeed(float value) => zoomController.ZoomSpeed = value;

		public void SetRotationSpeed(float value) => cameraRotator.RotationSpeed = value;

		public void SetResolution(int resolutionIndex)
		{
			var resolution = _resolutions[resolutionIndex];
			Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
		}

		public void SetVolume(float volume)
		{
			musicMixer.SetFloat("Volume", volume);
			_currentVolume = volume;
		}

		public void ToggleLoadSaveSettings(bool toggle)
		{
			if (toggle) LoadSettings();
			else SaveSettings();
		}

		private void SaveSettings()
		{
			PlayerPrefs.SetInt(Resolution, resolutionDropdown.value);
			PlayerPrefs.SetFloat(MusicVolume, musicVolume.value);
			PlayerPrefs.SetFloat(CameraRotationSpeed, cameraRotationSpeed.value);
			PlayerPrefs.SetFloat(CameraZoomSpeed, cameraZoomSpeed.value);
		}

		private void LoadSettings()
		{
			if (PlayerPrefs.HasKey(Resolution))
			{
				resolutionDropdown.value = PlayerPrefs.GetInt(Resolution);
			}

			if (PlayerPrefs.HasKey(MusicVolume))
			{
				musicVolume.value = PlayerPrefs.GetFloat(MusicVolume);
			}

			if (PlayerPrefs.HasKey(CameraRotationSpeed))
			{
				cameraRotationSpeed.value = PlayerPrefs.GetFloat(CameraRotationSpeed);
			}

			if (PlayerPrefs.HasKey(CameraZoomSpeed))
			{
				cameraZoomSpeed.value = PlayerPrefs.GetFloat(CameraRotationSpeed);
			}
		}
	}
}