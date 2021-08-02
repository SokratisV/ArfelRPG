using UnityEngine;

namespace Core.Interfaces
{
	public interface IOutlineable
	{
		public bool Enable { get; set; }
		public Color32 OutlineColor { get; set; }
	}

	//for QuickOutline asset
	public class QuickOutlineable : IOutlineable
	{
		private Outline _outline;

		public QuickOutlineable(GameObject gameObject)
		{
			var existingOutline = gameObject.GetComponentInChildren<Outline>();
			if (!existingOutline) existingOutline = gameObject.AddComponent<Outline>();
			_outline = existingOutline;
		}

		public bool Enable
		{
			get => _outline.enabled;
			set => _outline.enabled = value;
		}

		public Color32 OutlineColor
		{
			get => _outline.OutlineColor;
			set => _outline.OutlineColor = value;
		}
	}
}