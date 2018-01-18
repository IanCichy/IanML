using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static IC_ML_MazeSolver.frmMazeSolver;

namespace IC_ML_MazeSolver
{
    public class Map
    {
        public int width { get; set; }
        public int height { get; set; }
        public Tile[,] tiles { get; set; }

        Random rndNumGen = new Random(DateTime.Now.Millisecond);

        public Map()
        {

        }

        public void randomizeActions()
        {
            for (int x = 0; x < height; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    if (tiles[x, y].type == Tiles.Hole)
                        tiles[x, y].action = Actions.NONE;
                    else
                    {
                        tiles[x, y].action = getRandomAction();
                    }
                }
            }
        }

        /// <summary>
        /// Returns a random action form the allowed actions
        /// </summary>
        /// <param name="h"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        public Actions getRandomAction()
        {
            int R = (int)(rndNumGen.NextDouble() * 4);
            switch (R)
            {
                case 0:
                    return Actions.UP;
                case 1:
                    return Actions.DOWN;
                case 2:
                    return Actions.LEFT;
                case 3:
                    return Actions.RIGHT;
            }
            //Default - should never happen
            return Actions.NONE;
        }

    }
}
