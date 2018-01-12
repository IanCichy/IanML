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

        //public Tile[,] getMap()
        //{
        //    return tiles;
        //}

        //public void setMap(Tile[,] map)
        //{
        //    this.tiles = map;

        //    height = map.GetLength(0);
        //    width = map.GetLength(1);

        //    Button[,] btnCollection = new Button[height, width];
        //    btnCollection = initButtons(btnCollection);
        //    this.btnList = btnCollection;

        //    char[,] actionCollection = new char[height, width];
        //    actionCollection = initActions(actionCollection);
        //    this.actions = actionCollection;
        //}

        //private Button[,] initButtons(Button[,] btnCollection)
        //{
        //    for (int x = 0; x < height; x++)
        //    {
        //        for (int y = 0; y < width; y++)
        //        {
        //            Button btn = new Button();
        //            btn.Margin = new Padding(0, 0, 0, 0);
        //            btn.FlatStyle = FlatStyle.Flat;
        //            if (tiles[x, y] == 'H')
        //            {
        //                btn.Text = "H";
        //                btn.BackColor = System.Drawing.Color.SlateGray;
        //            }
        //            else if (tiles[x, y] == 'O')
        //            {
        //                btn.Text = "O";
        //                btn.BackColor = System.Drawing.Color.White;
        //            }
        //            else if (tiles[x, y] == 'I')
        //            {
        //                btn.Text = "I";
        //                btn.BackColor = System.Drawing.Color.Cyan;
        //            }
        //            else if (tiles[x, y] == 'S')
        //            {
        //                btn.Text = "S";
        //                btn.BackColor = System.Drawing.Color.Violet;
        //            }
        //            else if (tiles[x, y] == 'G')
        //            {
        //                btn.Text = "G";
        //                btn.BackColor = System.Drawing.Color.Orange;
        //            }

        //            btnCollection[x, y] = btn;

        //        }
        //    }
        //    return btnCollection;
        //}

        //public void setActions(char[,] actions)
        //{
        //    this.actions = actions;
        //}

        //private char[,] initActions(char[,] A)
        //{
        //    Random rnd = new Random();
        //    for (int x = 0; x < height; x++)
        //    {
        //        for (int y = 0; y < width; y++)
        //        {
        //            if (tiles[x, y] == 'H')
        //                A[x, y] = 'H';
        //            else
        //            {
        //                int R = (int)(rnd.NextDouble() * 4);
        //                if (R == 0)
        //                    A[x, y] = 'U';
        //                else if (R == 1)
        //                    A[x, y] = 'D';
        //                else if (R == 2)
        //                    A[x, y] = 'L';
        //                else if (R == 3)
        //                    A[x, y] = 'R';
        //            }
        //        }
        //    }
        //    return A;
        //}

        //public char[,] getActions()
        //{
        //    return actions;
        //}

        //public void String(char[,] actions)
        //{
        //    this.actions = actions;
        //}
    }

}
