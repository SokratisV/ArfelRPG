using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
	public class RenderUiImageInFront : MonoBehaviour
	{
		[SerializeField] private UnityEngine.Rendering.CompareFunction comparison = UnityEngine.Rendering.CompareFunction.Always;

		private static readonly int UnityGuizTestMode = Shader.PropertyToID("unity_GUIZTestMode");

		private void Start()
		{
			var imageToRender = GetComponent<Image>();
			var updatedMaterial = new Material(imageToRender.materialForRendering);
			updatedMaterial.SetInt(UnityGuizTestMode, (int) comparison);
			imageToRender.material = updatedMaterial;
		}
	}
}
