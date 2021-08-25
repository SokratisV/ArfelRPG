using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
	public class RenderMeshInFront : MonoBehaviour
	{
		[SerializeField] private UnityEngine.Rendering.CompareFunction comparison = UnityEngine.Rendering.CompareFunction.Always;

		private static readonly int UnityGuizTestMode = Shader.PropertyToID("_ZTest");

		private void Start()
		{
			var renderer = GetComponent<MeshRenderer>();
			var updatedMaterial = new Material(renderer.material);
			updatedMaterial.SetInt(UnityGuizTestMode, (int) comparison);
			renderer.material = updatedMaterial;
		}
	}
}