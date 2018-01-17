using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IC_ML_MazeSolver
{
    public class Map
    {
        public int width { get; set; }
        public int height { get; set; }
        public Tile[,] tiles { get; set; }
        public char[,] actions { get; set; }

        public Map()
        {
            tiles = null;
            actions = null;
        }

        public void initActions()
        {
            actions = new char[height, width];
            Random rnd = new Random(DateTime.Now.Millisecond);
            for (int x = 0; x < height; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    if (tiles[x, y].type == 3)
                        actions[x, y] = 'H';
                    else
                    {
                        int R = (int)(rnd.NextDouble() * 4);
                        if (R == 0)
                            actions[x, y] = 'U';
                        else if (R == 1)
                            actions[x, y] = 'D';
                        else if (R == 2)
                            actions[x, y] = 'L';
                        else if (R == 3)
                            actions[x, y] = 'R';
                    }
                }
            }
        }
    }
}
