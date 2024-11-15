using Godot;
using System;

public partial class MainMenu : Control
{
	#region Fields
	private Button _play;
	private TextureRect _background;
	private Button _howTo;
	private Button _quit;
	#endregion
	#region Methods
	private void Play()
	{
		GetTree().ChangeSceneToFile("res://scenes/MainScene.tscn");
	}
	private void Quit()
	{
		GetTree().Quit();
	}
	private void ShowTutorial()
	{
		Panel popup = new Panel();
		popup.Visible = false;

		Button button = new Button();
		button.SetAnchorsPreset(LayoutPreset.Center);
		button.Text = "Close";
		button.Pressed += () => popup.Hide();

		Label label = new Label();
		label.HorizontalAlignment = HorizontalAlignment.Center;
		label.SetAnchorsPreset(LayoutPreset.CenterTop);
		label.Text = "Users are shown a grid map of 30x30 cells\nwith each cell having a height value assigned to it.\nA cell can either be water(height = 0) or land(height > 0).\nConnected land cells represent an island.\nThe goal of the game is to find which\nisland has the greatest average height.";


		CenterContainer centerContainer = new CenterContainer();
		VBoxContainer vBoxContainer = new VBoxContainer();

		vBoxContainer.AddChild(label);
		vBoxContainer.AddChild(button);
		centerContainer.AddChild(vBoxContainer);
		centerContainer.SetAnchorsPreset(LayoutPreset.Center);
		centerContainer.UseTopLeft = true;
		popup.AddChild(centerContainer);
		popup.SetAnchorsPreset(LayoutPreset.TopLeft);
		popup.CustomMinimumSize = new Vector2((int)GetViewportRect().Size.X, (int)GetViewportRect().Size.Y);

		var styleBox = new StyleBoxFlat
		{
			BgColor = new Color(0, 0, 0, 0.85f),
			ContentMarginLeft = 10,
			ContentMarginRight = 10,
			ContentMarginTop = 5,
			ContentMarginBottom = 5
		};
		label.AddThemeStyleboxOverride("normal", styleBox);

		AddChild(popup);
		popup.Show();
	}
	#endregion
	#region Overrides
	public override void _Ready()
	{

		SetAnchorsPreset(LayoutPreset.FullRect);
		CenterContainer centerContainer = GetNode<CenterContainer>("CenterContainer");
		centerContainer.SetAnchorsPreset(LayoutPreset.Center);
		centerContainer.UseTopLeft = true;
		centerContainer.CustomMinimumSize = new Vector2((int)GetViewportRect().Size.X, (int)GetViewportRect().Size.Y);
		ColorRect backgroundColor = new ColorRect();
		backgroundColor.Color = new Color("#505050", 0.7f);
		backgroundColor.SetAnchorsPreset(LayoutPreset.Center);
		backgroundColor.CustomMinimumSize = new Vector2((int)centerContainer.GetRect().Size.X + 30, (int)centerContainer.GetRect().Size.Y + 30);
		centerContainer.AddChild(backgroundColor);
		
		VBoxContainer vBox = centerContainer.GetNode<VBoxContainer>("VBoxContainer");

		_play = vBox.GetNode<Button>("PlayButton");
		_play.Pressed += () => Play();
		_quit = vBox.GetNode<Button>("QuitButton");
		_quit.Pressed += () => Quit();
		_howTo = vBox.GetNode<Button>("TutorialButton");
		_howTo.Pressed += () => ShowTutorial();
		vBox.MoveToFront();

		CustomMinimumSize = new Vector2((int)GetViewportRect().Size.X, (int)GetViewportRect().Size.Y);

		_background = GetNode<TextureRect>("TextureRect");
		_background.Texture = GD.Load<Texture2D>("res://ui/mainMenu.png");
		_background.StretchMode = TextureRect.StretchModeEnum.Scale;
		_background.ExpandMode = TextureRect.ExpandModeEnum.KeepSize;
		_background.SizeFlagsHorizontal = Control.SizeFlags.Expand | Control.SizeFlags.Fill;
		_background.SizeFlagsVertical = Control.SizeFlags.Expand | Control.SizeFlags.Fill;
		_background.CustomMinimumSize = new Vector2((int)GetViewportRect().Size.X, (int)GetViewportRect().Size.Y);
		_background.SetAnchorsPreset(LayoutPreset.Center);

	}
	public override void _Process(double delta)
	{
	}
	#endregion
}
