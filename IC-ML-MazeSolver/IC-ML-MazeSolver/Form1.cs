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

namespace IC_ML_MazeSolver
{
    public partial class frmMazeSolver : Form
    {
        private bool gui = false;
        DataStructures data;

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
                //SET FILE NAME
                txtFile.Text = openFileDialog1.FileName;
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
            ////RANDOMIZES MAP ACTIONS
            //map.randomizeActions();

            ////CLEAR OUT LEARNED ACTIONS
            //if (stateAction != null || elgStateAction != null)
            //{
            //    stateAction.Clear();
            //    elgStateAction.Clear();
            //}
            //stateAction = initDictonary();
            //elgStateAction = initDictonary();

            ////RE-DRAW THE ORIGINAL MAP
            //resetFrame();
        }

        /// <summary>
        /// Resets the frame and re-draws the map back to the original state
        /// </summary>
        private void resetFrame()
        {
            //for (int x = 0; x < map.height; x++)
            //{
            //    for (int y = 0; y < map.width; y++)
            //    {
            //        map.tiles[x, y].btn.Text = "";
            //        if (map.tiles[x, y].type == Tiles.Open)
            //        {
            //            map.tiles[x, y].btn.BackColor = System.Drawing.Color.White;
            //        }
            //        else if (map.tiles[x, y].type == Tiles.Icy)
            //        {
            //            map.tiles[x, y].btn.BackColor = System.Drawing.Color.Cyan;
            //        }
            //        else if (map.tiles[x, y].type == Tiles.Start)
            //        {
            //            map.tiles[x, y].btn.Text = "S";
            //            map.tiles[x, y].btn.BackColor = System.Drawing.Color.Violet;
            //        }
            //        else if (map.tiles[x, y].type == Tiles.Goal)
            //        {
            //            map.tiles[x, y].btn.Text = "G";
            //            map.tiles[x, y].btn.BackColor = System.Drawing.Color.Orange;
            //        }
            //    }
            //}
            //this.Refresh();
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

        private void btnStart_Click(object sender, EventArgs e)
        {
            gui = chkAnimateGUI.Checked;
            int totalEpisodesToRun = int.Parse(txtEpisodesToRun.Text.ToString());
            double reductionConstant = double.Parse(txtReductionConstant.Text.ToString());

            if (rbtnQLearning.Checked)
            {
                AlgQLearning learner = new AlgQLearning();
                var t = Task.Run(() => learner.Learn(data, totalEpisodesToRun, reductionConstant));
                t.Wait();
            }
            else if (rbtnSarsa.Checked)
            {
                //var t = Task.Run(() => SARSA_Learning(map));
                //t.Wait();
                //SARSA_Learning(map);
            }
            else if (rbtnSarsaElg.Checked)
            {

            }

            finalFrame();
        }

        /*
         * UpdateFrame 
         *  - Updates the window with the current position of the agent and its past moves
         *  - Used only if GUI was set to 1 in input phase
         * Pre: JFrame, position of agent in map
         * Post: A visible window with the map displayed and the agents position highlighted
         */
        private void updateFrame(int currentY, int currentX, int previousY, int previousX)
        {
            //foreach (Tuple<int, int> t in previousLocations)
            //{
            //    Color c = Color.FromArgb((map.tiles[t.Item1, t.Item2].btn.BackColor.R) != 255 ? 255 : 255,
            //        (map.tiles[t.Item1, t.Item2].btn.BackColor.G) < 220 ? (map.tiles[t.Item1, t.Item2].btn.BackColor.G) + 1 : 220,
            //        (map.tiles[t.Item1, t.Item2].btn.BackColor.B) < 220 ? (map.tiles[t.Item1, t.Item2].btn.BackColor.B) + 1 : 220);
            //    map.tiles[t.Item1, t.Item2].btn.BackColor = c;
            //}
            //map.tiles[currentY, currentX].btn.BackColor = Color.FromArgb(50, 50, 50);

            //this.Refresh();
        }

        /*
         * FinalFrame 
         *  - RE-Builds the GUI interface to show the map with the final move set
         * Pre: JFrame
         * Post: A visible window with the final map displayed
         */
        private void finalFrame()
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

        ///*
        // * SARSA Learning method - learns by On-Policy updates to 
        // * find a goal in a  maze
        // */
        private void SARSA_Learning(Map M)
        {
            //bool icy = false;
            //for (int currentEpisodeNumber = 0; currentEpisodeNumber < totalEpisodesToRun; currentEpisodeNumber++)
            //{
            //    if (gui)
            //    {
            //        previousLocations.Clear();

            //    }
            //    //lblEpisodeProgress.Text = (currentEpisodeNumber + 1) + "/" + totalEpisodesToRun;
            //    //pgbEpisodeNum.Value = currentEpisodeNumber + 1;

            //    int w = STARTW, h = STARTH;
            //    int prew = STARTW, preh = STARTH;
            //    stepCount = 0;
            //    Actions stepAction;

            //    epsilon = calcuateReductionConstant(currentEpisodeNumber, epsilon);

            //    //OPTINAL GUI INTERFACE
            //    if (gui)
            //        resetFrame();

            //    while ((!(w == GOALW && h == GOALH)))
            //    {
            //        stepCount++;

            //        if (gui)
            //        {
            //            previousLocations.Add(new Tuple<int, int>(h, w));
            //        }

            //        prew = w;
            //        preh = h;
            //        //OPTINAL GUI INTERFACE
            //        if (gui)
            //            updateFrame(h, w, preh, prew);

            //        //addded step limit
            //        if (stepCount > maxSteps)
            //        {
            //            break;
            //        }

            //        double stepReward = -1.0;
            //        Actions currA = M.tiles[h, w].Action;
            //        State s = new State(h, w, currA);
            //        double currR = stateAction[s];

            //        //Take Action A
            //        //check to see if we are on ice-
            //        if (M.tiles[h, w].Type == Tiles.Icy)
            //            icy = true;
            //        else
            //            icy = false;

            //        //If the current action is UP
            //        if (M.tiles[h, w].Action == Actions.UP)
            //        {
            //            //if are moving into an open space and were not on ice
            //            if (isValidMove(h, w, Actions.UP) > 0 && !icy)
            //            {
            //                h--;
            //            }
            //            //if are moving into a hole and were not on ice
            //            else if (isValidMove(h, w, Actions.UP) == -1 && !icy)
            //            {
            //                stepReward = -100;
            //            }
            //            //we are moving on an icy tile
            //            else if (icy)
            //            {
            //                int[] v = getIcyMove(h, w, Actions.UP);
            //                h = v[0];
            //                w = v[1];
            //                stepReward = (double)v[2];
            //            }
            //            else
            //            {
            //                //This means we tried to move off the grid, stay in place and take -1.0 penalty
            //            }
            //        }
            //        else if (M.tiles[h, w].Action == Actions.DOWN)
            //        {
            //            //if are moving into an open space and were not on ice
            //            if (isValidMove(h, w, Actions.DOWN) > 0 && !icy)
            //            {
            //                h++;
            //            }
            //            //if are moving into a hole and were not on ice
            //            else if (isValidMove(h, w, Actions.DOWN) == -1 && !icy)
            //            {
            //                stepReward = -100;
            //            }
            //            //we are moving on an icy tile
            //            else if (icy)
            //            {
            //                int[] v = getIcyMove(h, w, Actions.DOWN);
            //                h = v[0];
            //                w = v[1];
            //                stepReward = (double)v[2];
            //            }
            //            else
            //            {
            //                //This means we tried to move off the grid, stay in place and take -1.0 penalty
            //            }
            //        }
            //        else if (M.tiles[h, w].Action == Actions.LEFT)
            //        {
            //            //if are moving into an open space and were not on ice
            //            if (isValidMove(h, w, Actions.LEFT) > 0 && !icy)
            //            {
            //                w--;
            //            }
            //            //if are moving into a hole and were not on ice
            //            else if (isValidMove(h, w, Actions.LEFT) == -1 && !icy)
            //            {
            //                stepReward = -100;
            //            }
            //            //we are moving on an icy tile
            //            else if (icy)
            //            {
            //                int[] v = getIcyMove(h, w, Actions.LEFT);
            //                h = v[0];
            //                w = v[1];
            //                stepReward = (double)v[2];
            //            }
            //            else
            //            {
            //                //This means we tried to move off the grid, stay in place and take -1.0 penalty
            //            }
            //        }
            //        else if (M.tiles[h, w].Action == Actions.RIGHT)
            //        {
            //            //if are moving into an open space and were not on ice
            //            if (isValidMove(h, w, Actions.RIGHT) > 0 && !icy)
            //            {
            //                w++;
            //            }
            //            //if are moving into a hole and were not on ice
            //            else if (isValidMove(h, w, Actions.RIGHT) == -1 && !icy)
            //            {
            //                stepReward = -100;
            //            }
            //            //we are moving on an icy tile
            //            else if (icy)
            //            {
            //                int[] v = getIcyMove(h, w, Actions.RIGHT);
            //                h = v[0];
            //                w = v[1];
            //                stepReward = (double)v[2];
            //            }
            //            else
            //            {
            //                //This means we tried to move off the grid, stay in place and take -1.0 penalty
            //            }
            //        }
            //        //end take action

            //        //Observe next state s' and one step reward
            //        Actions newA = M.tiles[h, w].Action;
            //        double newR = stateAction[new State(h, w, newA)];

            //        //Set next action a', chosen E-greedily based on Q(s',a')
            //        double r = rndNumGen.NextDouble();
            //        if (r <= epsilon)
            //            stepAction = getRandomAction();//choose random action
            //        else
            //            stepAction = getBestActionByState(h, w);//choose best action

            //        double e = currR + alpha * (stepReward + gamma * (newR - currR));
            //        stateAction[s] = e;
            //        M.tiles[h, w].Action = stepAction;
            //    }
            //}
        }

        ///*
        // * SARSA Learning method - learns by On-Policy updates to 
        // * find a goal in a  maze
        // */
        //private  void SARSA_Eligibility(Map M)
        //{
        //    ArrayList<State> stateQueue = new ArrayList<State>();
        //    boolean icy = false;

        //    for (int x = 0; x < episodeNum; x++)
        //    {
        //        stateQueue.clear();
        //        int w = STARTW, h = STARTH;
        //        count = 0;
        //        char action = 'R';

        //        M.getActions()[h, w] = getBestActionByState(h, w);

        //        if (reductionType == 1)
        //            eps = doSlowReduction(x, eps);
        //        else if (reductionType == 2)
        //            eps = doRapidReduction(x, eps);
        //        else
        //            eps = doNormalReduction(x, eps);

        //        if (x % 100 == 0)
        //        {
        //            Console.WriteLine("TRIALS : " + x);
        //            printAll();
        //        }

        //        while ((!(w == GOALW && h == GOALH)))
        //        {
        //            count++;

        //            //addded step limit
        //            if (eps < 0.1 & count >= maxSteps)
        //            {
        //                break;
        //            }

        //            double stepReward = -1.0;

        //            char currA = M.getActions()[h, w];
        //            State s = new State(h, w, currA);
        //            double currR = stateAction.get(s);

        //            //Add the current state to the state queue
        //            if (stateQueue.contains(s))
        //                stateQueue.remove(s);

        //            if (stateQueue.size() >= 10)
        //                stateQueue.remove(0);

        //            stateQueue.add(s);

        //            //Take Action A
        //            //check to see if we are on ice-
        //            if (M.getMap()[h, w] == 'I')
        //                icy = true;
        //            else
        //                icy = false;

        //            //If the current action is UP
        //            if (M.getActions()[h, w] == 'U')
        //            {
        //                //if are moving into an open space and were not on ice
        //                if (isValidMove(h, w, 'U') > 0 && !icy)
        //                {
        //                    h--;
        //                }
        //                //if are moving into a hole and were not on ice
        //                else if (isValidMove(h, w, 'U') == -1 && !icy)
        //                {
        //                    stepReward = -100;
        //                }
        //                //we are moving on an icy tile
        //                else if (icy)
        //                {
        //                    int v[] = getIcyMove(h, w, 'U');
        //                    h = v[0];
        //                    w = v[1];
        //                    stepReward = (double)v[2];
        //                }
        //                else
        //                {
        //                    //This means we tried to move off the grid, stay in place and take -1.0 penalty
        //                }
        //            }
        //            else if (M.getActions()[h, w] == 'D')
        //            {
        //                //if are moving into an open space and were not on ice
        //                if (isValidMove(h, w, 'D') > 0 && !icy)
        //                {
        //                    h++;
        //                }
        //                //if are moving into a hole and were not on ice
        //                else if (isValidMove(h, w, 'D') == -1 && !icy)
        //                {
        //                    stepReward = -100;
        //                }
        //                //we are moving on an icy tile
        //                else if (icy)
        //                {
        //                    int v[] = getIcyMove(h, w, 'D');
        //                    h = v[0];
        //                    w = v[1];
        //                    stepReward = (double)v[2];
        //                }
        //                else
        //                {
        //                    //This means we tried to move off the grid, stay in place and take -1.0 penalty
        //                }
        //            }
        //            else if (M.getActions()[h, w] == 'L')
        //            {
        //                //if are moving into an open space and were not on ice
        //                if (isValidMove(h, w, 'L') > 0 && !icy)
        //                {
        //                    w--;
        //                }
        //                //if are moving into a hole and were not on ice
        //                else if (isValidMove(h, w, 'L') == -1 && !icy)
        //                {
        //                    stepReward = -100;
        //                }
        //                //we are moving on an icy tile
        //                else if (icy)
        //                {
        //                    int v[] = getIcyMove(h, w, 'L');
        //                    h = v[0];
        //                    w = v[1];
        //                    stepReward = (double)v[2];
        //                }
        //                else
        //                {
        //                    //This means we tried to move off the grid, stay in place and take -1.0 penalty
        //                }
        //            }
        //            else if (M.getActions()[h, w] == 'R')
        //            {
        //                //if are moving into an open space and were not on ice
        //                if (isValidMove(h, w, 'R') > 0 && !icy)
        //                {
        //                    w++;
        //                }
        //                //if are moving into a hole and were not on ice
        //                else if (isValidMove(h, w, 'R') == -1 && !icy)
        //                {
        //                    stepReward = -100;
        //                }
        //                //we are moving on an icy tile
        //                else if (icy)
        //                {
        //                    int v[] = getIcyMove(h, w, 'R');
        //                    h = v[0];
        //                    w = v[1];
        //                    stepReward = (double)v[2];
        //                }
        //                else
        //                {
        //                    //This means we tried to move off the grid, stay in place and take -1.0 penalty
        //                }
        //            }
        //            //end take action

        //            //Observe next state s' and one step reward
        //            char newA = M.getActions()[h, w];
        //            double newR = stateAction.get(new State(h, w, newA));

        //            //Set next action a', chosen E-greedily based on Q(s',a')
        //            double r = Math.random();
        //            if (r <= eps)
        //                action = getRandomActionByState(h, w);//choose random action
        //            else
        //                action = getBestActionByState(h, w);//choose best action




        //            //Update Values
        //            double E = stepReward + gamma * newR - currR;

        //            //e(s,a) += 1;				
        //            elgStateAction.replace(s, elgStateAction.get(s) + 1);

        //            for (State t : stateQueue)
        //            {
        //                double val = stateAction.get(t);
        //                elgStateAction.replace(t, (gamma * lambda * elgStateAction.get(t)));
        //                stateAction.replace(t, val + (alpha * E * elgStateAction.get(t)));//*e(s,a)

        //                //e(s,a) = gamma*lambda*e(s,a);
        //            }

        //            //double e = currR+alpha*(stepReward +gamma*(newR-currR));
        //            //stateAction.replace(s, e);
        //            M.getActions()[h, w] = action;
        //        }
        //    }
        //}

    }
}

