using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslandHeightGame.common
{
    public class Island
    {
        #region Properties
        public List<Cell> Cells = new List<Cell>();
        public bool Tried { get; set; } = false;
        public int HeightSum { get; set; } = 0;
        #endregion
        #region Methods
        public float GetAverageHeight()
        {
            return Cells.Count > 0 ? HeightSum / Cells.Count : 0;
        }
        #endregion
    }
}
