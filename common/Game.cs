using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IslandHeightGame.common
{
	public class Game
	{
		#region Fields
		private Player _player;
		public List<Island> _islands;
		#endregion
		#region Properties
		public TwoKeyDictionary<int, int, Cell> Cells { get; set; } = new TwoKeyDictionary<int, int, Cell>();
		#endregion
		#region Constructors
		public Game(Player player)
		{
			_player = player;
			_islands = new List<Island>();
		}
		#endregion
		#region Methods
		public void IdentifyIslands()
		{
			for(int i = 0; i < 30; i++)
			{
				for(int j = 0; j < 30; j++)
				{
					if (!Cells[j, i].IsVisited && !Cells[j, i].IsWater)
					{
						Island island = new Island();
						FloodFill(j, i, island);
						if (island.Cells.Count > 0)
						{
							_islands.Add(island);
						}
					}
				}
			}
		}
		private void FloodFill(int x, int y, Island island)
		{
			if (!Cells.ContainsKey(x, y) || Cells[x, y].IsVisited || Cells[x, y].IsWater)
			{
				Cells[x, y].IsVisited = true;
				return;
			}
			Cell cell = Cells[x, y];
			cell.IsVisited = true;
			island.Cells.Add(cell);
			island.HeightSum += cell.Height;

			if (x != 0)
				FloodFill(x - 1, y, island);
			if (x < 29)
				FloodFill(x + 1, y, island);
			if (y != 0)
				FloodFill(x, y - 1, island);
			if (y < 29)
				FloodFill(x, y + 1, island);
		}
		private float HighestIslandHeight()
		{
			float highestAverageHeight = 0;
			foreach (Island island in _islands)
			{
				float averageHeight = island.GetAverageHeight();
				if (averageHeight > highestAverageHeight)
				{
					highestAverageHeight = averageHeight;
				}
			}
			return highestAverageHeight;
		}
		public int LowerAndGetLives(bool lower = false)
		{
			if(_player.Lives>0 && lower)
				_player.Lives--;
			return _player.Lives;
		}
		public GameResult IsRightChoice(Cell cell)
		{
			Island cellParent = _islands.Find(i => i.Cells.Any(c => c.Id == cell.Id));

			if (cellParent.Tried)
			{
				return GameResult.TriedAlready;
			}
			if(cellParent.GetAverageHeight() != HighestIslandHeight())
			{
				cellParent.Tried = true;
				return GameResult.Wrong;
			}
			return GameResult.Right;
		}
		#endregion
	}
}
