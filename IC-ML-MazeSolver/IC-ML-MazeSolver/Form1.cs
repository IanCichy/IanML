using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
        private List<Tuple<int,int>> previousLocations = new List<Tuple<int,int>>();
        private Dictionary<char, int> tileTypes;
        //GUI Components
        private bool gui = false;

        public frmMazeSolver()
        {
            InitializeComponent();
            //Initialization
            tileTypes = new Dictionary<char, int>();
            tileTypes.Add('O', 0);
            tileTypes.Add('S', 1);
            tileTypes.Add('G', 2);
            tileTypes.Add('H', 3);
            tileTypes.Add('I', 4);

            map = new Map();
        }

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
                map = readMapData(openFileDialog1.FileName);
                //SET WIDTH AND HEIGHT
                txtWidth.Text = map.width.ToString();
                txtHeight.Text = map.height.ToString();

                //BUILD OTHER DATA STRUCTURES
                maxSteps = map.height * map.width * 3;
                stateAction = initDictonary();
                elgStateAction = initDictonary();

                //SET UP THE INITAL FRAME
                buildFrame();
            }
        }

        /*
         * Main input processing method to read in from the given file and create the map object
         * PRE:A Filename is given to be parsed
         * POST:A two dimensional array is returned to represent the map the agent will be learning in
         */
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
                            case 0:
                                t.btn.Text = "O";
                                t.btn.BackColor = System.Drawing.Color.White;
                                break;
                            case 1:
                                t.btn.Text = "S";
                                t.btn.BackColor = System.Drawing.Color.Violet;
                                break;
                            case 2:
                                t.btn.Text = "G";
                                t.btn.BackColor = System.Drawing.Color.Orange;
                                break;
                            case 3:
                                t.btn.Text = "H";
                                t.btn.BackColor = System.Drawing.Color.SlateGray;
                                break;
                            case 4:
                                t.btn.Text = "I";
                                t.btn.BackColor = System.Drawing.Color.Cyan;
                                break;
                            default:
                                break;
                        }

                        map[x, y] = t;

                        if (t.type == 1)
                        {
                            STARTW = y;
                            STARTH = x;
                        }
                        else if (t.type == 2)
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
            newMap.initActions();
            return newMap;
        }

        /*
         * BuildFrame 
         *  - Builds the GUI interface to show the map that was read in.
         * Pre: A JFrame
         * Post: A visible window with the map displayed
         */
        private void buildFrame()
        {
            tlpMaze.Controls.Clear();
            tlpMaze.ColumnStyles.Clear();
            tlpMaze.RowStyles.Clear();

            tlpMaze.RowCount = map.height;
            tlpMaze.ColumnCount = map.width;

            int width = (int)(tlpMaze.Width / tlpMaze.ColumnCount);
            int height = (int)(tlpMaze.Height / tlpMaze.RowCount);
            //int btnsize = Math.Min(width, height);

            for (int x = 0; x < map.height; x++)
            {
                for (int y = 0; y < map.width; y++)
                {
                     map.tiles[x, y].btn.Width = width;
                     map.tiles[x, y].btn.Height = height;
                     tlpMaze.Controls.Add(map.tiles[x, y].btn, y, x);
                }
            }
            this.Refresh();
        }

        private void frmMazeSolver_ResizeEnd(object sender, EventArgs e)
        {
            if(txtFile.Text != null && !txtFile.Text.Equals(""))
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
                    //DO SOMETHING THREAD LIKE
                Q_Learning(map);
            }
            else if (rbtnSarsa.Checked)
            {

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
            foreach (Tuple<int,int> t in previousLocations)
            {
                Color c = Color.FromArgb((map.tiles[t.Item1, t.Item2].btn.BackColor.R) != 255 ? 255 : 255,
                    (map.tiles[t.Item1, t.Item2].btn.BackColor.G) < 220 ? (map.tiles[t.Item1, t.Item2].btn.BackColor.G) + 1 : 220,
                    (map.tiles[t.Item1, t.Item2].btn.BackColor.B) < 220 ? (map.tiles[t.Item1, t.Item2].btn.BackColor.B) + 1 : 220);
                map.tiles[t.Item1, t.Item2].btn.BackColor = c;
            }
            map.tiles[currentY, currentX].btn.BackColor = Color.FromArgb(50,50,50);

            this.Refresh();
        }

        /*
         * ResetFrame 
         *  - RE-Builds the GUI interface to show the map that was read in.
         *  - Used only if GUI was set to 1 in input phase
         * Pre: JFrame
         * Post: A visible window with the map displayed
         */
        private void resetFrame()
        {
            for (int x = 0; x < map.height; x++)
            {
                for (int y = 0; y < map.width; y++)
                {
                    if (map.tiles[x, y].btn.Text.Equals("O"))
                    {
                        map.tiles[x, y].btn.BackColor = System.Drawing.Color.White;
                    }
                    else if (map.tiles[x, y].btn.Text.Equals("I"))
                    {
                        map.tiles[x, y].btn.BackColor = System.Drawing.Color.Cyan;
                    }
                    else if (map.tiles[x, y].btn.Text.Equals("S"))
                    {
                        map.tiles[x, y].btn.BackColor = System.Drawing.Color.Violet;
                    }
                    else if (map.tiles[x, y].btn.Text.Equals("G"))
                    {
                        map.tiles[x, y].btn.BackColor = System.Drawing.Color.Orange;
                    }
                }
            }
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
                    String s = map.actions[x, y].ToString();
                    map.tiles[x, y].btn.Text = s;

                    if (s.Equals("R"))
                    {
                        map.tiles[x, y].btn.BackColor = System.Drawing.Color.FromArgb(156, 255, 56);
                    }
                    else if (s.Equals("U"))
                    {
                        map.tiles[x, y].btn.BackColor = System.Drawing.Color.FromArgb(156, 56, 255);
                    }
                    else if (s.Equals("D"))
                    {
                        map.tiles[x, y].btn.BackColor = System.Drawing.Color.FromArgb(56, 255, 255);
                    }
                    else if (s.Equals("L"))
                    {
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
        //private  void SARSA_Learning(Map M)
        //{
        //    boolean icy = false;
        //    for (int x = 0; x < episodeNum; x++)
        //    {
        //        int w = STARTW, h = STARTH;
        //        count = 0;
        //        char action = 'R';

        //        //M.getActions()[h,w] = getBestActionByState(h,w);

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

        //        //OPTINAL GUI INTERFACE
        //        if (gui)
        //            resetFrame(frame);

        //        while ((!(w == GOALW && h == GOALH)))
        //        {
        //            count++;

        //            //OPTINAL GUI INTERFACE
        //            if (gui)
        //                updateFrame(frame, h, w);

        //            //Optional Step Limit
        //            //if(eps<0.1 & count >= maxSteps){
        //            //	break;
        //            //}

        //            double stepReward = -1.0;
        //            char currA = M.getActions()[h, w];
        //            State s = new State(h, w, currA);
        //            double currR = stateAction.get(s);

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

        //            double e = currR + alpha * (stepReward + gamma * (newR - currR));
        //            stateAction.replace(s, e);
        //            M.getActions()[h, w] = action;
        //        }
        //    }
        //}

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
                 lblEpisodeProgress.Text = (currentEpisodeNumber + 1) + "/" + totalEpisodesToRun;
                 pgbEpisodeNum.Value = currentEpisodeNumber + 1;

                int w = STARTW, h = STARTH;
                int prew = STARTW, preh = STARTH;
                stepCount = 0;

                char stepAction;
                //M.getActions()[h, w] = getBestActionByState(h, w);

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

                    //Set next action a', chosen E-greedily based on Q(s',a')
                    Random rnd = new Random();
                    double r = rnd.NextDouble();
                    if (r <= epsilon)
                        stepAction = getRandomActionByState(h, w);//choose random action
                    else
                        stepAction = getBestActionByState(h, w);//choose best action
                    M.actions[h, w] = stepAction;

                    State s = new State(h, w, stepAction);
                    double currR = stateAction[s];

                    //Take Action A
                    //check to see if we are on ice-
                    if (M.tiles[h, w].type == 4)
                        icy = true;
                    else
                        icy = false;

                    //If the current action is UP
                    if (M.actions[h, w] == 'U')
                    {
                        //if are moving into an open space and were not on ice
                        if (isValidMove(h, w, 'U') > 0 && !icy)
                        {
                            h--;
                        }
                        //if are moving into a hole and were not on ice
                        else if (isValidMove(h, w, 'U') == -1 && !icy)
                        {
                            stepReward = -100;
                        }
                        //we are moving on an icy tile
                        else if (icy)
                        {
                            int[] v = getIcyMove(h, w, 'U');
                            h = v[0];
                            w = v[1];
                            stepReward = (double)v[2];
                        }
                        else
                        {
                            //This means we tried to move off the grid, stay in place and take -1.0 penalty
                        }
                    }
                    else if (M.actions[h, w] == 'D')
                    {
                        //if are moving into an open space and were not on ice
                        if (isValidMove(h, w, 'D') > 0 && !icy)
                        {
                            h++;
                        }
                        //if are moving into a hole and were not on ice
                        else if (isValidMove(h, w, 'D') == -1 && !icy)
                        {
                            stepReward = -100;
                        }
                        //we are moving on an icy tile
                        else if (icy)
                        {
                            int[] v = getIcyMove(h, w, 'D');
                            h = v[0];
                            w = v[1];
                            stepReward = (double)v[2];
                        }
                        else
                        {
                            //This means we tried to move off the grid, stay in place and take -1.0 penalty
                        }
                    }
                    else if (M.actions[h, w] == 'L')
                    {
                        //if are moving into an open space and were not on ice
                        if (isValidMove(h, w, 'L') > 0 && !icy)
                        {
                            w--;
                        }
                        //if are moving into a hole and were not on ice
                        else if (isValidMove(h, w, 'L') == -1 && !icy)
                        {
                            stepReward = -100;
                        }
                        //we are moving on an icy tile
                        else if (icy)
                        {
                            int[] v = getIcyMove(h, w, 'L');
                            h = v[0];
                            w = v[1];
                            stepReward = (double)v[2];
                        }
                        else
                        {
                            //This means we tried to move off the grid, stay in place and take -1.0 penalty
                        }
                    }
                    else if (M.actions[h, w] == 'R')
                    {
                        //if are moving into an open space and were not on ice
                        if (isValidMove(h, w, 'R') > 0 && !icy)
                        {
                            w++;
                        }
                        //if are moving into a hole and were not on ice
                        else if (isValidMove(h, w, 'R') == -1 && !icy)
                        {
                            stepReward = -100;
                        }
                        //we are moving on an icy tile
                        else if (icy)
                        {
                            int[] v = getIcyMove(h, w, 'R');
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

                    //find Max Q(s',a')
                    char t = getBestActionByState(h, w);
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





        public void takeAction()
        {

        }











        /*
         * Reduction
         * Pace: reductionConstant episodes
         * Reduce By: (0.9/Math.floor(x/10.0))
         */
        private double calcuateReductionConstant(int x, Double D)
        {
            if (x >= (totalEpisodesToRun/2))
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
        private int[] getIcyMove(int h, int w, char c)
        {
            Random rnd = new Random();
            double R = rnd.NextDouble();
            int[] vars = { 0, 0, 0 };

            if (R <= 0.8)
            {
                if (isValidMove(h, w, c) > 0)
                {//normal move
                    if (c == 'U')
                    {
                        h--;
                    }
                    else if (c == 'D')
                    {
                        h++;
                    }
                    else if (c == 'L')
                    {
                        w--;
                    }
                    else if (c == 'R')
                    {
                        w++;
                    }
                    vars[0] = h;
                    vars[1] = w;
                    vars[2] = -1;
                }
                else if (isValidMove(h, w, c) == -1)
                {//normal move hole
                    vars[0] = h;
                    vars[1] = w;
                    vars[2] = -100;
                }
                else if (isValidMove(h, w, c) == -1)
                {//normal move off map, stay put
                    vars[0] = h;
                    vars[1] = w;
                    vars[2] = -1;
                }
                return vars;
            }
            else if (R > 0.8 && R <= 0.9)
            {//Slip to left/above intended move

                int orig_h = h;
                int orig_w = w;

                //slip height and width
                if (c == 'U')
                {
                    h--;
                    w--;
                }
                else if (c == 'D')
                {
                    h++;
                    w++;
                }
                else if (c == 'L')
                {
                    h++;
                    w--;
                }
                else if (c == 'R')
                {
                    h--;
                    w++;
                }

                try
                {
                    if (map.tiles[h, w].type == 3)
                    {
                        vars[0] = orig_h;
                        vars[1] = orig_w;
                        vars[2] = -100;
                    }
                    else
                    {
                        vars[0] = h;
                        vars[1] = w;
                        vars[2] = -1;
                    }
                }
                catch (Exception e)
                {
                    vars[0] = orig_h;
                    vars[1] = orig_w;
                    vars[2] = -1;
                }
                return vars;
            }
            else
            {//Slip to right/below intended move

                int orig_h = h;
                int orig_w = w;

                //slip height and width
                if (c == 'U')
                {
                    h--;
                    w++;
                }
                else if (c == 'D')
                {
                    h++;
                    w--;
                }
                else if (c == 'L')
                {
                    h--;
                    w--;
                }
                else if (c == 'R')
                {
                    h++;
                    w++;
                }

                try
                {
                    if (map.tiles[h, w].type == 3)
                    {
                        vars[0] = orig_h;
                        vars[1] = orig_w;
                        vars[2] = -100;
                    }
                    else
                    {
                        vars[0] = h;
                        vars[1] = w;
                        vars[2] = -1;
                    }
                }
                catch (Exception e)
                {
                    vars[0] = orig_h;
                    vars[1] = orig_w;
                    vars[2] = -1;
                }
                return vars;
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
        private int isValidMove(int h, int w, char c)
        {
            if (c == 'U')
            {
                h--;
            }
            else if (c == 'D')
            {
                h++;
            }
            else if (c == 'L')
            {
                w--;
            }
            else if (c == 'R')
            {
                w++;
            }

            try
            {
                if (map.actions[h, w] == 'H')
                    return -1;
            }
            catch (Exception e)
            {
                return -2;
            }
            return 1;
        }

        /*
         * Returns best action
         * pre: Given a h,w pair in the map
         * post: Returns the best action {U,D,L,R} based on the scores of the 
         * moves at this location
         */
        private char getBestActionByState(int h, int w)
        {
            Double s1 = stateAction[new State(h, w, 'U')];
            Double s2 = stateAction[new State(h, w, 'D')];
            Double s3 = stateAction[new State(h, w, 'L')];
            Double s4 = stateAction[new State(h, w, 'R')];

            if (s1 > s2 && s1 > s3 && s1 > s4)
            {
                return 'U';
            }
            else if (s2 > s1 && s2 > s3 && s2 > s4)
            {
                return 'D';
            }
            else if (s3 > s2 && s3 > s1 && s3 > s4)
            {
                return 'L';
            }
            else
            {
                return 'R';
            }
        }

        /*
         * Returns a random action {U,D,L,R}
         */
        private char getRandomActionByState(int h, int w)
        {
            Random rnd = new Random();
            int R = (int)(rnd.NextDouble() * 4);
            if (R == 0)
                return 'U';
            else if (R == 1)
                return 'D';
            else if (R == 2)
                return 'L';
            else
                return 'R';
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
                    State S = new State(x, y, 'U');
                    dict.Add(S, 0.0);

                    State S1 = new State(x, y, 'D');
                    dict.Add(S1, 0.0);

                    State S2 = new State(x, y, 'L');
                    dict.Add(S2, 0.0);

                    State S3 = new State(x, y, 'R');
                    dict.Add(S3, 0.0);
                }
            }
            return dict;
        }
    }





























}

