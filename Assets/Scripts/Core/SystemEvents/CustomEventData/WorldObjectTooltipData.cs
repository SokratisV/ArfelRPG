namespace RPG.Core.SystemEvents
{
	[System.Serializable]
	public struct WorldObjectTooltipData
	{
		public string objectName;
		public string objectInfo;
		public bool mouseEnter; //True on mouse over, false on mouse exit
	}
}