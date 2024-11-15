using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslandHeightGame.common
{
	public class MapColorHelper
	{
		#region Methods
		public static Color GetColorFromHeight(int height)
		{
			if (height <= 0)
			{
				return new Color("1E90FF"); // DodgerBlue
			}
			else if (height <= 200)
			{
				Color baseColor = new Color("1E90FF"); // DodgerBlue
				return baseColor.Lerp(new Color("32CD32"), height / 200.0f); // LimeGreen
			}
			else if (height <= 400)
			{
				Color baseColor = new Color("32CD32"); // LimeGreen
				return baseColor.Lerp(new Color("FFD700"), (height - 200) / 200.0f); // Gold
			}
			else if (height <= 600)
			{
				Color baseColor = new Color("FFD700"); // Gold
				return baseColor.Lerp(new Color("FF8C00"), (height - 400) / 200.0f); // DarkOrange
			}
			else if (height <= 800)
			{
				Color baseColor = new Color("FF8C00"); // DarkOrange
				return baseColor.Lerp(new Color("A9A9A9"), (height - 600) / 200.0f); // DarkGray
			}
			else
			{
				Color baseColor = new Color("A9A9A9"); // DarkGray
				return baseColor.Lerp(new Color("DCDCDC"), (height - 800) / 200.0f); // Gainsboro
			}
		}
		public static Color GetLighterColorFromHeight(int height)
		{
			if (height <= 0)
			{
				return new Color("4682B4"); // SteelBlue
			}
			else if (height <= 200)
			{
				Color baseColor = new Color("4682B4"); // SteelBlue
				return baseColor.Lerp(new Color("228B22"), height / 200.0f); // ForestGreen
			}
			else if (height <= 400)
			{
				Color baseColor = new Color("228B22"); // ForestGreen
				return baseColor.Lerp(new Color("EEE8AA"), (height - 200) / 200.0f); // PaleGoldenrod
			}
			else if (height <= 600)
			{
				Color baseColor = new Color("EEE8AA"); // PaleGoldenrod
				return baseColor.Lerp(new Color("FFA500"), (height - 400) / 200.0f); // Orange
			}
			else if (height <= 800)
			{
				Color baseColor = new Color("FFA500"); // Orange
				return baseColor.Lerp(new Color("BEBEBE"), (height - 600) / 200.0f); // Gray
			}
			else
			{
				Color baseColor = new Color("BEBEBE"); // Gray
				return baseColor.Lerp(new Color("E5E5E5"), (height - 800) / 200.0f); // WhiteSmoke
			}
		}
		#endregion
	}
}
