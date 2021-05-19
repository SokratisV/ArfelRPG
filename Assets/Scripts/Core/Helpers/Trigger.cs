namespace RPG.Core
{
	public struct Trigger
	{
		private bool _trigger;

		public bool Value
		{
			get
			{
				if (_trigger)
				{
					_trigger = false;
					return true;
				}
				return _trigger;
			}
			set => _trigger = value;
		}
	}
}

