using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using static IC_ML_MazeSolver.DataStructures;
using System.Threading;

namespace IC_ML_MazeSolver
{
    public partial class frmMazeSolver : Form
    {
        private bool gui = false;
        DataStructures data;

        public bool doGUI = false;

        /// <summary>
        /// Init Method called by Frame launching
        /// </summary>
        public frmMazeSolver()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Opens the file dialog and allows the user to select a map file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = "\\bin";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtFile.Text = openFileDialog1.FileName;
                //Create the DataStructures object that holds all the map and
                //training data
                data = new DataStructures(txtFile.Text);
                buildFrame(data.map);
            }
        }

        /// <summary>
        /// Adds the controls to the frame to display the map
        /// </summary>
        private void buildFrame(Map map)
        {
            //CLEAR ANY EXISTING CONTROLS/STYLES
            tlpMaze.Controls.Clear();
            tlpMaze.ColumnStyles.Clear();
            tlpMaze.RowStyles.Clear();

            //SET THE WIDTH/HEIGHT
            tlpMaze.RowCount = map.height;
            tlpMaze.ColumnCount = map.width;
            int width = (int)(tlpMaze.Width / tlpMaze.ColumnCount);
            int height = (int)(tlpMaze.Height / tlpMaze.RowCount);

            //ADD CONTROLS TO THE TABLE LAYOUT PANEL
            for (int x = 0; x < map.height; x++)
            {
                for (int y = 0; y < map.width; y++)
                {
                    map.tiles[x, y].btn.Width = width;
                    map.tiles[x, y].btn.Height = height;
                    tlpMaze.Controls.Add(map.tiles[x, y].btn, y, x);
                }
            }
            //REFRESH THE FRAME
            this.Refresh();
        }

        /// <summary>
        /// Resets all paramets except the map so additional trials can be run
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            data.resetMapData();
            data.resetFrame(data.map);
        }

        /// <summary>
        /// Redraws the frame to fit the new size
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMazeSolver_ResizeEnd(object sender, EventArgs e)
        {
            if (txtFile.Text != null && !txtFile.Text.Equals(""))
            {
                buildFrame(data.map);
            }
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            gui = chkAnimateGUI.Checked;
            int totalEpisodesToRun = int.Parse(txtEpisodesToRun.Text.ToString());
            double reductionConstant = double.Parse(txtReductionConstant.Text.ToString());

            if (rbtnQLearning.Checked)
            {
                AlgQLearning learner = new AlgQLearning();
                if (gui)
                {
                    learner.Learn(data, totalEpisodesToRun, reductionConstant, gui, this);
                }
                else
                {
                    //AlgQLearning learner = new AlgQLearning();
                    await Task.Run(() => learner.Learn(data, totalEpisodesToRun, reductionConstant, gui, this));
                }
            }
            else if (rbtnSarsa.Checked)
            {
                AlgSARSALearning learner = new AlgSARSALearning();
                await Task.Run(() => learner.Learn(data, totalEpisodesToRun, reductionConstant, gui));
            }
            DisplayResults();
        }

        /*
         * FinalFrame 
         *  - RE-Builds the GUI interface to show the map with the final move set
         * Pre: JFrame
         * Post: A visible window with the final map displayed
         */
        private void DisplayResults()
        {
            Map map = data.map;

            for (int x = 0; x < map.height; x++)
            {
                for (int y = 0; y < map.width; y++)
                {
                    Actions act = map.tiles[x, y].Action;
                    if (act == Actions.RIGHT)
                    {
                        map.tiles[x, y].btn.Text = "R";
                        map.tiles[x, y].btn.BackColor = System.Drawing.Color.FromArgb(156, 255, 56);
                    }
                    else if (act == Actions.UP)
                    {
                        map.tiles[x, y].btn.Text = "U";
                        map.tiles[x, y].btn.BackColor = System.Drawing.Color.FromArgb(156, 56, 255);
                    }
                    else if (act == Actions.DOWN)
                    {
                        map.tiles[x, y].btn.Text = "D";
                        map.tiles[x, y].btn.BackColor = System.Drawing.Color.FromArgb(56, 255, 255);
                    }
                    else if (act == Actions.LEFT)
                    {
                        map.tiles[x, y].btn.Text = "L";
                        map.tiles[x, y].btn.BackColor = System.Drawing.Color.FromArgb(255, 56, 56);
                    }
                }
            }
            map.tiles[data.GOALH, data.GOALW].btn.BackColor = System.Drawing.Color.Orange;
            map.tiles[data.STARTH, data.STARTW].btn.BackColor = System.Drawing.Color.Violet;
            this.Refresh();
        }

    }
}

