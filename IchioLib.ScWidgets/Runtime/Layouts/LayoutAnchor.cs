namespace ILib.ScWidgets
{
	[System.Flags]
	public enum LayoutAnchor
	{
		None = 0,
		Left = 1,
		Top = 1 << 2,
		Bottom = 1 << 3,
		Right = 1 << 4,
		LeftTop = Left | Top,
		RightTop = Right | Top,
		RightBottom = Right | Bottom,
		LeftBottom = Left | Bottom,
	}
}