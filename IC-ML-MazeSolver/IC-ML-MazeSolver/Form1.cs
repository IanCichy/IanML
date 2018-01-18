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

namespace IC_ML_MazeSolver
{
    public partial class frmMazeSolver : Form
    {
        public int GOALW = 0, GOALH = 0, STARTW = 0, STARTH = 0;
        //The map controller
        private Map map;
        //Constant Variables
        private double gamma = 0.9, epsilon = 0.9, alpha = 0.9, lambda = 0.9, reductionConstant = 10;
        private int stepCount = 0, maxSteps = 0, totalEpisodesToRun = 2000;
        //Dictionaries	
        private Dictionary<State, Double> stateAction;
        private Dictionary<State, Double> elgStateAction;
        private List<Tuple<int, int>> previousLocations = new List<Tuple<int, int>>();
        //Tile Data
        public enum Tiles { Start, Goal, Open, Hole, Icy };
        private Dictionary<char, Tiles> tileTypes = new Dictionary<char, Tiles>() {
            {'O', Tiles.Open},
            {'S', Tiles.Start},
            {'G', Tiles.Goal},
            {'H', Tiles.Hole},
            {'I', Tiles.Icy},
        };
        //Action Data
        public enum Actions { UP, DOWN, LEFT, RIGHT, NONE };
        //GUI Components
        private bool gui = false;
        //RandomNumberGen
        public Random rndNumGen = new Random(DateTime.Now.Millisecond);

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

                //READ FILE IN
                map = new Map();
                map = readMapData(openFileDialog1.FileName);

                //BUILD OTHER DATA STRUCTURES
                maxSteps = map.height * map.width * 3;
                if (stateAction != null || elgStateAction != null)
                {
                    stateAction.Clear();
                    elgStateAction.Clear();
                }
                stateAction = initDictonary();
                elgStateAction = initDictonary();

                //SET UP THE INITAL FRAME
                buildFrame();
            }
        }

        /// <summary>
        /// Reads in the map data and initalizes the environment
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public Map readMapData(String FileName)
        {
            Map newMap = new Map();
            Tile[,] map = null;
            try
            {
                //Create a reader to read in the map data
                System.IO.StreamReader reader = new System.IO.StreamReader(FileName);
                //Find the height and width of the map
                String line = reader.ReadLine();
                newMap.height = int.Parse(line);
                line = reader.ReadLine();
                newMap.width = int.Parse(line);
                line = reader.ReadLine();

                //create the new map for processing and filling
                map = new Tile[newMap.height, newMap.width];

                for (int x = 0; x < map.GetLength(0); x++)
                {
                    char[] letters = line.ToCharArray();
                    for (int y = 0; y < map.GetLength(1); y++)
                    {
                        //Create a new tile for that location
                        Tile t = new Tile(x, y, tileTypes[letters[y]]);
                        t.btn = new Button();
                        t.btn.Margin = new Padding(0, 0, 0, 0);
                        t.btn.FlatStyle = FlatStyle.Flat;

                        switch (t.type)
                        {
                            case Tiles.Open:
                                t.btn.BackColor = System.Drawing.Color.White;
                                break;
                            case Tiles.Start:
                                t.btn.Text = "S";
                                t.btn.BackColor = System.Drawing.Color.Violet;
                                break;
                            case Tiles.Goal:
                                t.btn.Text = "G";
                                t.btn.BackColor = System.Drawing.Color.Orange;
                                break;
                            case Tiles.Hole:
                                t.btn.BackColor = System.Drawing.Color.SlateGray;
                                break;
                            case Tiles.Icy:
                                t.btn.BackColor = System.Drawing.Color.Cyan;
                                break;
                            default:
                                break;
                        }

                        map[x, y] = t;

                        if (t.type == Tiles.Start)
                        {
                            STARTW = y;
                            STARTH = x;
                        }
                        else if (t.type == Tiles.Goal)
                        {
                            GOALW = y;
                            GOALH = x;
                        }
                    }
                    line = reader.ReadLine();
                }
                reader.Close();
            }
            catch (Exception e)
            {
                Console.Error.Write(e);
            }
            newMap.tiles = map;
            newMap.randomizeActions();
            return newMap;
        }

        /// <summary>
        /// Adds the controls to the frame to display the map
        /// </summary>
        private void buildFrame()
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
            //RANDOMIZES MAP ACTIONS
            map.randomizeActions();

            //CLEAR OUT LEARNED ACTIONS
            if (stateAction != null || elgStateAction != null)
            {
                stateAction.Clear();
                elgStateAction.Clear();
            }
            stateAction = initDictonary();
            elgStateAction = initDictonary();

            //RE-DRAW THE ORIGINAL MAP
            resetFrame();
        }

        /// <summary>
        /// Resets the frame and re-draws the map back to the original state
        /// </summary>
        private void resetFrame()
        {
            for (int x = 0; x < map.height; x++)
            {
                for (int y = 0; y < map.width; y++)
                {
                    map.tiles[x, y].btn.Text = "";
                    if (map.tiles[x, y].type == Tiles.Open)
                    {
                        map.tiles[x, y].btn.BackColor = System.Drawing.Color.White;
                    }
                    else if (map.tiles[x, y].type == Tiles.Icy)
                    {
                        map.tiles[x, y].btn.BackColor = System.Drawing.Color.Cyan;
                    }
                    else if (map.tiles[x, y].type == Tiles.Start)
                    {
                        map.tiles[x, y].btn.Text = "S";
                        map.tiles[x, y].btn.BackColor = System.Drawing.Color.Violet;
                    }
                    else if (map.tiles[x, y].type == Tiles.Goal)
                    {
                        map.tiles[x, y].btn.Text = "G";
                        map.tiles[x, y].btn.BackColor = System.Drawing.Color.Orange;
                    }
                }
            }
            this.Refresh();
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
                buildFrame();
            }
        }





        private void btnStart_Click(object sender, EventArgs e)
        {
            runMap();
        }

        public void runMap()
        {
            gui = chkAnimateGUI.Checked;
            totalEpisodesToRun = int.Parse(txtEpisodesToRun.Text.ToString());
            reductionConstant = double.Parse(txtReductionConstant.Text.ToString());
            pgbEpisodeNum.Maximum = totalEpisodesToRun;

            if (rbtnQLearning.Checked)
            {
                var t = Task.Run(() => Q_Learning(map));
                t.Wait();
                //Q_Learning(map);
            }
            else if (rbtnSarsa.Checked)
            {
                var t = Task.Run(() => SARSA_Learning(map));
                t.Wait();
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
            foreach (Tuple<int, int> t in previousLocations)
            {
                Color c = Color.FromArgb((map.tiles[t.Item1, t.Item2].btn.BackColor.R) != 255 ? 255 : 255,
                    (map.tiles[t.Item1, t.Item2].btn.BackColor.G) < 220 ? (map.tiles[t.Item1, t.Item2].btn.BackColor.G) + 1 : 220,
                    (map.tiles[t.Item1, t.Item2].btn.BackColor.B) < 220 ? (map.tiles[t.Item1, t.Item2].btn.BackColor.B) + 1 : 220);
                map.tiles[t.Item1, t.Item2].btn.BackColor = c;
            }
            map.tiles[currentY, currentX].btn.BackColor = Color.FromArgb(50, 50, 50);

            this.Refresh();
        }

        /*
         * FinalFrame 
         *  - RE-Builds the GUI interface to show the map with the final move set
         * Pre: JFrame
         * Post: A visible window with the final map displayed
         */
        private void finalFrame()
        {
            for (int x = 0; x < map.height; x++)
            {
                for (int y = 0; y < map.width; y++)
                {
                    Actions act = map.tiles[x, y].action;

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
            map.tiles[GOALH, GOALW].btn.BackColor = System.Drawing.Color.Orange;
            map.tiles[STARTH, STARTW].btn.BackColor = System.Drawing.Color.Violet;
            this.Refresh();
        }

        ///*
        // * SARSA Learning method - learns by On-Policy updates to 
        // * find a goal in a  maze
        // */
        private void SARSA_Learning(Map M)
        {
            bool icy = false;
            for (int currentEpisodeNumber = 0; currentEpisodeNumber < totalEpisodesToRun; currentEpisodeNumber++)
            {
                if (gui)
                {
                    previousLocations.Clear();

                }
                //lblEpisodeProgress.Text = (currentEpisodeNumber + 1) + "/" + totalEpisodesToRun;
                //pgbEpisodeNum.Value = currentEpisodeNumber + 1;

                int w = STARTW, h = STARTH;
                int prew = STARTW, preh = STARTH;
                stepCount = 0;
                Actions stepAction;

                epsilon = calcuateReductionConstant(currentEpisodeNumber, epsilon);

                //OPTINAL GUI INTERFACE
                if (gui)
                    resetFrame();

                while ((!(w == GOALW && h == GOALH)))
                {
                    stepCount++;

                    if (gui)
                    {
                        previousLocations.Add(new Tuple<int, int>(h, w));
                    }

                    prew = w;
                    preh = h;
                    //OPTINAL GUI INTERFACE
                    if (gui)
                        updateFrame(h, w, preh, prew);

                    //addded step limit
                    if (stepCount > maxSteps)
                    {
                        break;
                    }

                    double stepReward = -1.0;
                    Actions currA = M.tiles[h, w].action;
                    State s = new State(h, w, currA);
                    double currR = stateAction[s];

                    //Take Action A
                    //check to see if we are on ice-
                    if (M.tiles[h, w].type == Tiles.Icy)
                        icy = true;
                    else
                        icy = false;

                    //If the current action is UP
                    if (M.tiles[h, w].action == Actions.UP)
                    {
                        //if are moving into an open space and were not on ice
                        if (isValidMove(h, w, Actions.UP) > 0 && !icy)
                        {
                            h--;
                        }
                        //if are moving into a hole and were not on ice
                        else if (isValidMove(h, w, Actions.UP) == -1 && !icy)
                        {
                            stepReward = -100;
                        }
                        //we are moving on an icy tile
                        else if (icy)
                        {
                            int[] v = getIcyMove(h, w, Actions.UP);
                            h = v[0];
                            w = v[1];
                            stepReward = (double)v[2];
                        }
                        else
                        {
                            //This means we tried to move off the grid, stay in place and take -1.0 penalty
                        }
                    }
                    else if (M.tiles[h, w].action == Actions.DOWN)
                    {
                        //if are moving into an open space and were not on ice
                        if (isValidMove(h, w, Actions.DOWN) > 0 && !icy)
                        {
                            h++;
                        }
                        //if are moving into a hole and were not on ice
                        else if (isValidMove(h, w, Actions.DOWN) == -1 && !icy)
                        {
                            stepReward = -100;
                        }
                        //we are moving on an icy tile
                        else if (icy)
                        {
                            int[] v = getIcyMove(h, w, Actions.DOWN);
                            h = v[0];
                            w = v[1];
                            stepReward = (double)v[2];
                        }
                        else
                        {
                            //This means we tried to move off the grid, stay in place and take -1.0 penalty
                        }
                    }
                    else if (M.tiles[h, w].action == Actions.LEFT)
                    {
                        //if are moving into an open space and were not on ice
                        if (isValidMove(h, w, Actions.LEFT) > 0 && !icy)
                        {
                            w--;
                        }
                        //if are moving into a hole and were not on ice
                        else if (isValidMove(h, w, Actions.LEFT) == -1 && !icy)
                        {
                            stepReward = -100;
                        }
                        //we are moving on an icy tile
                        else if (icy)
                        {
                            int[] v = getIcyMove(h, w, Actions.LEFT);
                            h = v[0];
                            w = v[1];
                            stepReward = (double)v[2];
                        }
                        else
                        {
                            //This means we tried to move off the grid, stay in place and take -1.0 penalty
                        }
                    }
                    else if (M.tiles[h, w].action == Actions.RIGHT)
                    {
                        //if are moving into an open space and were not on ice
                        if (isValidMove(h, w, Actions.RIGHT) > 0 && !icy)
                        {
                            w++;
                        }
                        //if are moving into a hole and were not on ice
                        else if (isValidMove(h, w, Actions.RIGHT) == -1 && !icy)
                        {
                            stepReward = -100;
                        }
                        //we are moving on an icy tile
                        else if (icy)
                        {
                            int[] v = getIcyMove(h, w, Actions.RIGHT);
                            h = v[0];
                            w = v[1];
                            stepReward = (double)v[2];
                        }
                        else
                        {
                            //This means we tried to move off the grid, stay in place and take -1.0 penalty
                        }
                    }
                    //end take action

                    //Observe next state s' and one step reward
                    Actions newA = M.tiles[h, w].action;
                    double newR = stateAction[new State(h, w, newA)];

                    //Set next action a', chosen E-greedily based on Q(s',a')
                    double r = rndNumGen.NextDouble();
                    if (r <= epsilon)
                        stepAction = getRandomAction();//choose random action
                    else
                        stepAction = getBestActionByState(h, w);//choose best action

                    double e = currR + alpha * (stepReward + gamma * (newR - currR));
                    stateAction[s] = e;
                    M.tiles[h, w].action = stepAction;
                }
            }
        }

        ///*
         /* Q Learning method - learns by Off-Policy updates to
         /* find a goal in a maze
         */
        private void Q_Learning(Map M)
        {
            bool icy = false;
            for (int currentEpisodeNumber = 0; currentEpisodeNumber < totalEpisodesToRun; currentEpisodeNumber++)
            {
                if (gui)
                {
                    previousLocations.Clear();

                }
                //lblEpisodeProgress.Text = (currentEpisodeNumber + 1) + "/" + totalEpisodesToRun;
                //pgbEpisodeNum.Value = currentEpisodeNumber + 1;

                int w = STARTW, h = STARTH;
                int prew = STARTW, preh = STARTH;
                stepCount = 0;

                Actions stepAction;
                epsilon = calcuateReductionConstant(currentEpisodeNumber, epsilon);

                //OPTINAL GUI INTERFACE
                if (gui)
                    resetFrame();

                while ((!(w == GOALW && h == GOALH)))
                {
                    stepCount++;
                    icy = false;

                    if (gui)
                    {
                        previousLocations.Add(new Tuple<int, int>(h, w));
                    }

                    prew = w;
                    preh = h;
                    //OPTINAL GUI INTERFACE
                    if (gui)
                        updateFrame(h, w, preh, prew);

                    //addded step limit
                    if (stepCount > maxSteps)
                    {
                        break;
                    }

                    double stepReward = -1.0;

                    //Set next action a', chosen E-greedily based on Q(s',a')
                    double r = rndNumGen.NextDouble();
                    if (r <= epsilon)
                        stepAction = getRandomAction();//choose random action
                    else
                        stepAction = getBestActionByState(h, w);//choose best action
                    M.tiles[h, w].action = stepAction;

                    State s = new State(h, w, stepAction);
                    double currR = stateAction[s];

                    //Take Action A

                    if (M.tiles[h, w].type == Tiles.Icy)
                        icy = true;

                    var returnVector = takeAction(M, h, w, M.tiles[h, w].action, icy);
                    h = returnVector[0];
                    w = returnVector[1];
                    stepReward = returnVector[2];

                    //find Max Q(s',a')
                    Actions t = getBestActionByState(h, w);
                    double maxR = stateAction[(new State(h, w, t))];

                    double e = currR + alpha * (stepReward + lambda * (maxR - currR));
                    stateAction[s] = e;
                }
            }
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


        public int[] takeAction(Map M, int h, int w, Actions act, bool icy)
        {
            int stepReward = -1;
            if (M.tiles[h, w].action == Actions.UP)
            {
                if (icy)
                {
                    int[] v = getIcyMove(h, w, Actions.UP);
                    h = v[0];
                    w = v[1];
                    stepReward = v[2];
                }
                else
                {
                    int validityScore = isValidMove(h, w, Actions.UP);
                    if (validityScore == 1)
                    {
                        h--;
                    }
                    else if (validityScore == -1)
                    {
                        stepReward = -100;
                    }
                }
            }
            else if (M.tiles[h, w].action == Actions.DOWN)
            {
                if (icy)
                {
                    int[] v = getIcyMove(h, w, Actions.DOWN);
                    h = v[0];
                    w = v[1];
                    stepReward = v[2];
                }
                else
                {
                    int validityScore = isValidMove(h, w, Actions.DOWN);
                    if (validityScore == 1)
                    {
                        h++;
                    }
                    else if (validityScore == -1)
                    {
                        stepReward = -100;
                    }
                }
            }
            else if (M.tiles[h, w].action == Actions.LEFT)
            {
                if (icy)
                {
                    int[] v = getIcyMove(h, w, Actions.LEFT);
                    h = v[0];
                    w = v[1];
                    stepReward = v[2];
                }
                else
                {
                    int validityScore = isValidMove(h, w, Actions.LEFT);
                    if (validityScore == 1)
                    {
                        w--;
                    }
                    else if (validityScore == -1)
                    {
                        stepReward = -100;
                    }
                }
            }
            else if (M.tiles[h, w].action == Actions.RIGHT)
            {
                if (icy)
                {
                    int[] v = getIcyMove(h, w, Actions.RIGHT);
                    h = v[0];
                    w = v[1];
                    stepReward = v[2];
                }
                else
                {
                    int validityScore = isValidMove(h, w, Actions.RIGHT);
                    if (validityScore == 1)
                    {
                        w++;
                    }
                    else if (validityScore == -1)
                    {
                        stepReward = -100;
                    }
                }
            }
            int[] vector = new int[3];
            vector[0] = h;
            vector[1] = w;
            vector[2] = stepReward;
            return vector;
        }


        /// <summary>
        /// Calcuates the reduction constatnt based on
        /// (0.9/Math.floor(x/reductionConst))
        /// </summary>
        /// <param name="x"></param>
        /// <param name="D"></param>
        /// <returns></returns>
        private double calcuateReductionConstant(int x, Double D)
        {
            if (x >= (totalEpisodesToRun / 2))
                return 0;
            else if (x % reductionConstant == 0 && x >= reductionConstant)
                return (0.9 / Math.Floor(x / reductionConstant));
            else
                return D;
        }

        /*
         * Decides if you slide or not when you are on an icy tile
         * pre: Given the current h,w pair and the move {U,D,L,R}
         * 	post: Rolls a random number and returns a triple, {new h, new w, reward]
         * for a move depending on if we slipped or not, moved into a hole, tried to move off the map, etc. 
         */
        private int[] getIcyMove(int h, int w, Actions act)
        {
            double R = rndNumGen.NextDouble();
            int[] returnVector = { 0, 0, 0 };

            if (R <= 0.8)
            {
                int validityScore = isValidMove(h, w, act);
                //Normal Move in the map
                if (validityScore == 1)
                {
                    if (act == Actions.UP)
                    {
                        h--;
                    }
                    else if (act == Actions.DOWN)
                    {
                        h++;
                    }
                    else if (act == Actions.LEFT)
                    {
                        w--;
                    }
                    else if (act == Actions.RIGHT)
                    {
                        w++;
                    }
                    returnVector[0] = h;
                    returnVector[1] = w;
                    returnVector[2] = -1;
                }
                //Move into a hole, dont actually move
                else if (validityScore == -1)
                {
                    returnVector[0] = h;
                    returnVector[1] = w;
                    returnVector[2] = -100;
                }
                //Move off the map, dont actually move
                else if (validityScore == -2)
                {
                    returnVector[0] = h;
                    returnVector[1] = w;
                    returnVector[2] = -1;
                }
                return returnVector;
            }
            //Slip to left/above intended move
            else if (R > 0.8 && R <= 0.9)
            {
                int orig_h = h;
                int orig_w = w;

                //slip height and width
                if (act == Actions.UP)
                {
                    h--;
                    w--;
                }
                else if (act == Actions.DOWN)
                {
                    h++;
                    w++;
                }
                else if (act == Actions.LEFT)
                {
                    h++;
                    w--;
                }
                else if (act == Actions.RIGHT)
                {
                    h--;
                    w++;
                }

                if ((h >= 0 && h < map.height) && (w >= 0 && w < map.width))
                {
                    if (map.tiles[h, w].isHole)
                    {
                        returnVector[0] = orig_h;
                        returnVector[1] = orig_w;
                        returnVector[2] = -100;
                    }
                    else
                    {
                        returnVector[0] = h;
                        returnVector[1] = w;
                        returnVector[2] = -1;
                    }
                }
                else
                {
                    returnVector[0] = orig_h;
                    returnVector[1] = orig_w;
                    returnVector[2] = -1;
                }
                return returnVector;
            }
            else
            {//Slip to right/below intended move

                int orig_h = h;
                int orig_w = w;

                //slip height and width
                if (act == Actions.UP)
                {
                    h--;
                    w++;
                }
                else if (act == Actions.DOWN)
                {
                    h++;
                    w--;
                }
                else if (act == Actions.LEFT)
                {
                    h--;
                    w--;
                }
                else if (act == Actions.RIGHT)
                {
                    h++;
                    w++;
                }

                if ((h >= 0 && h < map.height) && (w >= 0 && w < map.width))
                {
                    if (map.tiles[h, w].isHole)
                    {
                        returnVector[0] = orig_h;
                        returnVector[1] = orig_w;
                        returnVector[2] = -100;
                    }
                    else
                    {
                        returnVector[0] = h;
                        returnVector[1] = w;
                        returnVector[2] = -1;
                    }
                }
                else
                {
                    returnVector[0] = orig_h;
                    returnVector[1] = orig_w;
                    returnVector[2] = -1;
                }
                return returnVector;
            }
        }

        /*
         * Move validity checker
         * pre: Given the current h,w pair and the move {U,D,L,R}
         * post: Checks if the move would take the agent off the map, to a free space
         * or into a hole and returns accordingly
         * {Hole = -1
         * normal move = 1
         * off map = -2}
         */
        private int isValidMove(int h, int w, Actions act)
        {
            if (act == Actions.UP)
            {
                h--;
            }
            else if (act == Actions.DOWN)
            {
                h++;
            }
            else if (act == Actions.LEFT)
            {
                w--;
            }
            else if (act == Actions.RIGHT)
            {
                w++;
            }

            if ((h >= 0 && h < map.height) && (w >= 0 && w < map.width))
            {
                if (map.tiles[h, w].isHole)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return -2;
            }
        }

        /*
         * Returns best action
         * pre: Given a h,w pair in the map
         * post: Returns the best action {U,D,L,R} based on the scores of the 
         * moves at this location
         */
        private Actions getBestActionByState(int h, int w)
        {
            Double s1 = stateAction[new State(h, w, Actions.UP)];
            Double s2 = stateAction[new State(h, w, Actions.DOWN)];
            Double s3 = stateAction[new State(h, w, Actions.LEFT)];
            Double s4 = stateAction[new State(h, w, Actions.RIGHT)];

            if (s1 > s2 && s1 > s3 && s1 > s4)
            {
                return Actions.UP;
            }
            else if (s2 > s1 && s2 > s3 && s2 > s4)
            {
                return Actions.DOWN;
            }
            else if (s3 > s2 && s3 > s1 && s3 > s4)
            {
                return Actions.LEFT;
            }
            else
            {
                return Actions.RIGHT;
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

        /*
         * initDictonary
         * pre: A given map of data
         * post: A dictionary with all possible state action combinations
         */
        private Dictionary<State, Double> initDictonary()
        {
            Dictionary<State, Double> dict = new Dictionary<State, Double>();
            for (int x = 0; x < map.height; x++)
            {
                for (int y = 0; y < map.width; y++)
                {
                    State S = new State(x, y, Actions.UP);
                    dict.Add(S, 0.0);

                    State S1 = new State(x, y, Actions.DOWN);
                    dict.Add(S1, 0.0);

                    State S2 = new State(x, y, Actions.LEFT);
                    dict.Add(S2, 0.0);

                    State S3 = new State(x, y, Actions.RIGHT);
                    dict.Add(S3, 0.0);
                }
            }
            return dict;
        }
    }
}

