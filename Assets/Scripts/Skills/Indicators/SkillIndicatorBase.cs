using RPG.Skills.Behaviors;
using UnityEngine;

namespace RPG.Skills
{
	public abstract class SkillIndicatorBase : MonoBehaviour
	{
		[SerializeField] private Material indicatorMaterial;

		[SerializeField, ColorUsage(true, true)]
		private Color32 disabledColor;

		protected IndicatorType Type;
		private Color32 _initialColor;
		private static Material _instancedMaterial = null;
		private bool _indicatorState;

		protected static Material InstancedMaterial
		{
			get => _instancedMaterial;
			private set
			{
				if (_instancedMaterial) return;
				_instancedMaterial = value;
			}
		}

		public IndicatorType IndicatorType() => Type;

		public void Awake()
		{
			_initialColor = indicatorMaterial.color;
			InstancedMaterial = new Material(indicatorMaterial);
			Init();
		}

		protected abstract void Init();

		public void ToggleColorState(bool toggle)
		{
			// if (_indicatorState == toggle) return;
			// _instancedMaterial.color = new Color32(0, 0, 0, 255);
			if (!toggle) _instancedMaterial.color = new Color32(disabledColor.r, disabledColor.g, disabledColor.b, disabledColor.a);
			else _instancedMaterial.color = new Color32(_initialColor.r, _initialColor.g, _initialColor.b, _initialColor.a);
			// else _instancedMaterial.color = new Color32(disabledColor.r, disabledColor.g, disabledColor.b, disabledColor.a);
			// _indicatorState = toggle;
		}	
	}
}