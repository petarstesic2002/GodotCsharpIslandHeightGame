using Godot;
using Godot.Collections;
using IslandHeightGame.api;
using IslandHeightGame.common;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

public partial class MainScene : CenterContainer
{
	#region Fields
	private GridContainer _mapContainer;
	private CenterContainer _centerContainer;
	private HBoxContainer _hBoxContainer;
	private TextureRect _colorBar;
	private VBoxContainer _labelContainer;
	private VBoxContainer _mainUIVBox;
	private HBoxContainer _playerHBox;
	private const int _gridSize = 30;
	private const int _maxCellHeight = 1000;
	private const int _colorBarHeight = 300;
	private Game _game;
	private Player _player;
	private Cell _selectedCell = new Cell();
	#endregion
	#region Methods
	private void SetMainVBox()
	{
		_mainUIVBox = GetNode<VBoxContainer>("MainUIVBox");
	}
	private void SetHBoxContainer()
	{
		_hBoxContainer = _mainUIVBox.GetNode<HBoxContainer>("HBoxContainer");
	}
	private void SetCenterContainer()
	{
		_centerContainer = _hBoxContainer.GetNode<CenterContainer>("CenterContainer");
		_centerContainer.SetAnchorsPreset(Control.LayoutPreset.CenterLeft);
	}
	private void SetGridContainer(string data)
	{
		_mapContainer = _centerContainer.GetNode<GridContainer>("MapGridContainer");
		_mapContainer.Columns = _gridSize;
		_mapContainer.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
		_mapContainer.SizeFlagsVertical = Control.SizeFlags.ExpandFill;

		SetGridContainerData(data);
	}
	private void SetGridContainerData(string data)
	{
		string[] rows = data.Split('\n');
		int id = 0;
		for (int i = 0; i < _gridSize; i++)
		{
			if (i >= rows.Length) break;
			string[] rowValues = rows[i].Split(' ');
			for (int j = 0; j < _gridSize; j++)
			{
				if (j >= rowValues.Length) break;
				if (int.TryParse(rowValues[j], out int height))
				{
					var cell = new Cell();
					cell.SetHeight(height);
					cell.Pressed += () => OnCellClicked(cell);
					cell.Id = id;
					id++;
					_mapContainer.AddChild(cell);
					_game.Cells.Add(j, i, cell);
				}
			}
		}
	}
	private void UpdateCellSizes()
	{
		if (_mapContainer == null || _mapContainer.Columns <= 0)
			return;
		float viewportWidth = GetViewportRect().Size.X;
		float viewportHeight = GetViewportRect().Size.Y;
		float cellSize = Math.Min(viewportWidth / _gridSize, viewportHeight / _gridSize) / 1.5f;

		foreach (Node child in _mapContainer.GetChildren())
		{
			if(child is Control cell)
			{
				cell.CustomMinimumSize = new Vector2(cellSize, cellSize);
			}
		}
		_mapContainer.CustomMinimumSize = new Vector2(cellSize * _gridSize, cellSize * _gridSize);
		_mapContainer.QueueSort();
	}
	private void SetColorBar()
	{
		_colorBar = _hBoxContainer.GetNode<TextureRect>("LegendColorBar");
		_colorBar.SizeFlagsVertical = Control.SizeFlags.Fill;
		_colorBar.SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter;
		_colorBar.StretchMode = TextureRect.StretchModeEnum.Scale;
		_colorBar.Texture = GenerateColorBarTexture();
		_colorBar.SetAnchorsPreset(LayoutPreset.FullRect);
		_colorBar.CustomMinimumSize = new Vector2(50, 100);
	}
	private ImageTexture GenerateColorBarTexture()
	{
		var image = Image.CreateEmpty(1, _colorBarHeight, false, Image.Format.Rgb8);
		
		for (int y = 0; y < _colorBarHeight; y++)
		{
			int heightValue = (int)((float)y / _colorBarHeight * _maxCellHeight);
			Color color = MapColorHelper.GetColorFromHeight(heightValue);
			image.SetPixel(0, _colorBarHeight - 1 - y, color);
		}

		ImageTexture texture = ImageTexture.CreateFromImage(image);
		return texture;
	}
	private void SetColorBarLabelContainer()
	{
		_labelContainer = _hBoxContainer.GetNode<VBoxContainer>("LegendColorBarLabels");
		_labelContainer.SizeFlagsVertical = Control.SizeFlags.Fill;
		_labelContainer.SizeFlagsHorizontal = Control.SizeFlags.ShrinkEnd;
		_labelContainer.SetAnchorsPreset(LayoutPreset.FullRect);
		_labelContainer.CustomMinimumSize = new Vector2(50, _colorBarHeight);
		_labelContainer.AddThemeConstantOverride("separation", (int)_colorBar.Size.Y / 3);

		SetColorBarLabels();
	}
	private void SetColorBarLabels()
	{
		for(int i = _maxCellHeight; i >= 0; i -= 200)
		{
			Label label = new Label();
			label.Text = i.ToString();
			label.SizeFlagsVertical = Control.SizeFlags.ShrinkEnd;
			label.HorizontalAlignment = HorizontalAlignment.Right;
			_labelContainer.AddChild(label);
		}
	}
	
	private void SetPlayerHbox()
	{
		_playerHBox = _mainUIVBox.GetNode<HBoxContainer>("PlayerHBox");
		SetPlayerHBoxChildren();
		BindConfirmButton();
		BindQuitButton();
	}
	private void SetPlayerHBoxChildren(bool disabled = true, int lives = 3)
	{
		SetLivesLabelText(lives);
		DisableConfirmButton(disabled);
		_playerHBox.AddChild(_player.LivesLabel);
		_playerHBox.AddChild(_player.ConfirmButton);
		_playerHBox.AddChild(_player.QuitToMenu);
	}
	private void UpdatePlayerHBoxChildren(bool disabled = true, int lives = 3)
	{
		SetLivesLabelText(lives);
		DisableConfirmButton(disabled);
	}
	private void SetLivesLabelText(int lives)
	{
		_player.LivesLabel.Text = $"Lives: {lives}";
	}
	private void DisableConfirmButton(bool disabled)
	{
		_player.ConfirmButton.Disabled = disabled;
	}
	private async Task<bool> SetUpUI()
	{
		MapDataFetcher fetcher = new MapDataFetcher();
		RestResponse mapData = await fetcher.FetchMapDataAsync();
		SetMainVBox();
		SetHBoxContainer();
		SetCenterContainer();
		SetGridContainer(mapData.Content);
		UpdateCellSizes();
		SetColorBar();
		SetColorBarLabelContainer();
		SetPlayerHbox();
		return true;
	}
	public void BindConfirmButton()
	{
		_player.ConfirmButton.Pressed += () => ConfirmChoice();
	}
	private void BindQuitButton()
	{
		_player.QuitToMenu.Pressed += () => QuitToMenu();
	}
	private void QuitToMenu()
	{
		GetTree().ChangeSceneToFile("res://scenes/MainMenu.tscn");
	}
	private async void StartGame()
	{
		_player = new Player();
		_game = new Game(_player);
		bool result = await SetUpUI();
		_game.IdentifyIslands();
	}
	private void OnCellClicked(Cell sender)
	{
		if (sender.IsWater)
		{
			_selectedCell = null;
			UpdatePlayerHBoxChildren(true, _game.LowerAndGetLives());
		}
		else
		{
			_selectedCell = sender;
			UpdatePlayerHBoxChildren(false, _game.LowerAndGetLives());
		}
	}
	private void ConfirmChoice()
	{
		if (_selectedCell == null)
			return;
		GameResult result = _game.IsRightChoice(_selectedCell);
		ShowPopup(result);
	}
	private void ShowPopup(GameResult result)
	{
		Panel window = new Panel();
		window.Visible = false;

		Button button = new Button();
		button.SetAnchorsPreset(LayoutPreset.Center);

		Label label = new Label();
		label.SetAnchorsPreset(LayoutPreset.CenterTop);

		if (result == GameResult.Wrong)
		{
			int lives = _game.LowerAndGetLives(true);
			label.Text = $"Wrong choice, lives remaining: {lives}";
			UpdatePlayerHBoxChildren(lives: lives);
			if (lives == 0)
				result = GameResult.Loss;
		}
		if (result == GameResult.Right)
		{
			label.Text = "Good job, you found the right island.";
		}
		if (result == GameResult.Loss)
		{
			label.Text = "You failed to find the right island.";
		}
		if (result == GameResult.Loss || result == GameResult.Right)
		{
			button.Text = "Quit To Main Menu";
			button.Pressed += () => QuitToMenu();
		}
		if (result == GameResult.TriedAlready)
		{
			label.Text = "You tried this one already, try another one.";
		}
		if (result == GameResult.Wrong || result == GameResult.TriedAlready)
		{
			button.Text = "OK";
			button.Pressed += () => window.Hide();
		}

		CenterContainer centerContainer = new CenterContainer();
		VBoxContainer vBoxContainer = new VBoxContainer();

		vBoxContainer.AddChild(label);
		vBoxContainer.AddChild(button);
		centerContainer.AddChild(vBoxContainer);
		centerContainer.SetAnchorsPreset(LayoutPreset.FullRect);

		window.AddChild(centerContainer);
		window.SetAnchorsPreset(LayoutPreset.Center);
		window.CustomMinimumSize = new Vector2((int)GetViewportRect().Size.X, (int)GetViewportRect().Size.Y);

		AddChild(window);
		window.Show();
	}
	#endregion
	#region Overrides
	public override void _Ready()
	{
		StartGame();
	}
	public override void _Process(double delta)
	{
	}
	public override void _Notification(int what)
	{
		if (what == NotificationWMSizeChanged)
		{
			UpdateCellSizes();
		}
	}
	#endregion
}
