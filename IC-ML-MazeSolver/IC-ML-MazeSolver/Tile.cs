using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IC_ML_MazeSolver
{
    public class Tile
    {
        public int xLoc { get; set; }
        public int yLoc { get; set; }
        public int type { get; set; }
        public Button btn { get; set; }
        public bool isHole { get; set; }

        public Tile(int x, int y, int t)
        {
            btn = new Button();
            xLoc = x;
            yLoc = y;
            type = t;
            if(type == 3)
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
