using Godot;
using IslandHeightGame.common;
using System;

public partial class Cell : Button
{
	#region Properties
	public int Id { get; set; }
	public int Height {  get; set; }
	public bool IsWater => Height == 0;
	public bool IsVisited { get; set; } = false;
	#endregion
	#region Methods
	public void SetHeight(int height)
	{
		Height = height;
		UpdateColor();
	}
	private void UpdateColor()
	{
		Color initial = MapColorHelper.GetColorFromHeight(Height);
		Color hover = MapColorHelper.GetLighterColorFromHeight(Height);

		StyleBoxFlat initialStyle = new StyleBoxFlat();
		initialStyle.BgColor = initial;

		StyleBoxFlat hoverStyle = new StyleBoxFlat();
		hoverStyle.BgColor = hover;

		this.AddThemeStyleboxOverride("normal", initialStyle);
		this.AddThemeStyleboxOverride("hover", hoverStyle);
	}
	#endregion
	#region Overrides
	public override void _Ready()
	{
	}
	public override void _Process(double delta)
	{
	}
	#endregion
}
