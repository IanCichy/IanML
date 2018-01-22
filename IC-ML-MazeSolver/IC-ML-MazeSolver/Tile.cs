using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static IC_ML_MazeSolver.DataStructures;

namespace IC_ML_MazeSolver
{
    public class Tile
    {
        public int xLoc { get; set; }
        public int yLoc { get; set; }
        public Tiles Type { get; set; }
        public Button btn { get; set; }
        public bool isHole { get; set; }
        public Actions Action { get; set; }

        public Tile(int x, int y, Tiles t)
        {
            btn = new Button();
            xLoc = x;
            yLoc = y;
            Type = t;
            if(Type == Tiles.Hole)
            {
                isHole = true;
            }
            else
            {
                isHole = false;
            }
        }
    }
}
