using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslandHeightGame.common
{
	public class Player
	{
		#region Properties
		public int Lives { get; set; } = 3;
		public Label LivesLabel = new Label();
		public Button ConfirmButton = new Button();
		public Button QuitToMenu = new Button();
		public Player()
		{
			LivesLabel.HorizontalAlignment = HorizontalAlignment.Center;
			ConfirmButton.Text = "Confirm Choice";
			QuitToMenu.Text = "Quit";
		}
		#endregion
	}
}
