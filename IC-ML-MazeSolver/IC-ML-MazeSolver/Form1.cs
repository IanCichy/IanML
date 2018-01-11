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

        public int GOALW = 0, GOALH = 0,
                STARTW = 0, STARTH = 0;
        //The map controller
        private Map map;
        //Constant Variables
        private double gamma = 0.9, eps = 0.9, alpha = 0.9, lambda = 0.9, reductionType = 0;
        private int count = 0, normalUpdate = 10, rapidUpdate = 4, slowUpdate = 25, maxSteps = 0, episodeNum = 2000;
        //HashTables	
        private Dictionary<State, Double> stateAction;
        private Dictionary<State, Double> elgStateAction;
        //GUI Components
        private bool gui = false;

        public frmMazeSolver()
        {
            InitializeComponent();
            //Initialization
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
                map.setMap(readInput(openFileDialog1.FileName));
                //SET WIDTH AND HEIGHT
                txtWidth.Text = map.width.ToString();
                txtHeight.Text = map.height.ToString();

                //BUILD OTHER DATA STRUCTURES
                maxSteps = map.height * map.width * 3;
                stateAction = initDictonary(map.getMap());
                elgStateAction = initDictonary(map.getMap());

                //SET UP THE INITAL FRAME
                buildFrame();
            }
        }

        /*
         * Main input processing method to read in from the given file and create the map object
         * PRE:A Filename is given to be parsed
         * POST:A two dimensional array is returned to represent the map the agent will be learning in
         */
        public char[,] readInput(String FileName)
        {
            //Find length and width of the maze we are making
            int h = 0, w = 0;
            try
            {
                System.IO.StreamReader reader = new System.IO.StreamReader(FileName);
                String line = reader.ReadLine();
                w = line.Length;
                int count = 1;
                while (reader.ReadLine() != null)
                {
                    count++;
                }
                h = count;
                reader.Close();
            }
            catch (Exception e)
            {
            }

            char[,] map = null;
            try
            {
                System.IO.StreamReader reader = new System.IO.StreamReader(FileName);
                String line = reader.ReadLine();
                //create the new map for processing and filling
                map = new char[h, w];

                for (int x = 0; x < map.GetLength(0); x++)
                {
                    char[] letters = line.ToCharArray();
                    for (int y = 0; y < map.GetLength(1); y++)
                    {
                        map[x, y] = letters[y];

                        if (letters[y] == 'S')
                        {
                            STARTW = y;
                            STARTH = x;
                        }
                        else if (letters[y] == 'G')
                        {
                            GOALW = y;
                            GOALH = x;
                        }
                    }
                    line = reader.ReadLine();
                }
                //Close reader
                reader.Close();
            }
            catch (Exception e)
            {
            }
            return map;
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

            tlpMaze.RowCount = map.btnList.GetLength(0);
            tlpMaze.ColumnCount = map.btnList.GetLength(1);

            int width = (int)(tlpMaze.Width / tlpMaze.ColumnCount);
            int height = (int)(tlpMaze.Height / tlpMaze.RowCount);
            //int btnsize = Math.Min(width, height);

            for (int x = 0; x < map.btnList.GetLength(0); x++)
            {
                for (int y = 0; y < map.btnList.GetLength(1); y++)
                {

                     map.btnList[x, y].Width = width;
                     map.btnList[x, y].Height = height;
                    tlpMaze.Controls.Add(map.btnList[x, y], y, x);
                }
            }
            this.Refresh();
        }

        private void frmMazeSolver_ResizeEnd(object sender, EventArgs e)
        {
            buildFrame();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {

            runMap();
        }


        public void runMap()
        {
            gui = rbtnGUI.Checked;



            if (rbtnQLearning.Checked)
            {
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
        private void updateFrame(int h, int w)
        {
            for (int x = 0; x < map.btnList.GetLength(0); x++)
            {
                for (int y = 0; y < map.btnList.GetLength(1); y++)
                {
                    if (map.btnList[x, y].BackColor == System.Drawing.Color.Red)
                    {
                        map.btnList[x, y].BackColor = System.Drawing.Color.Orange;
                    }
                }
            }

            map.btnList[h, w].BackColor = System.Drawing.Color.Red;
            this.Refresh();
            try
            {
                System.Threading.Thread.Sleep(5);
            }
            catch (Exception e)
            {

            }
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
            for (int x = 0; x < map.btnList.GetLength(0); x++)
            {
                for (int y = 0; y < map.btnList.GetLength(1); y++)
                {
                    if (map.btnList[x, y].Text.Equals("O"))
                    {
                        map.btnList[x, y].BackColor = System.Drawing.Color.White;
                    }
                    else if (map.btnList[x, y].Text.Equals("I"))
                    {
                        map.btnList[x, y].BackColor = System.Drawing.Color.Cyan;
                    }
                    else if (map.btnList[x, y].Text.Equals("S"))
                    {
                        map.btnList[x, y].BackColor = System.Drawing.Color.Gray;
                    }
                    else if (map.btnList[x, y].Text.Equals("G"))
                    {
                        map.btnList[x, y].BackColor = System.Drawing.Color.Gray;
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
            for (int x = 0; x < map.btnList.GetLength(0); x++)
            {
                for (int y = 0; y < map.btnList.GetLength(1); y++)
                {

                    String s = map.getActions()[x, y].ToString();
                    map.btnList[x, y].Text = s;

                    if (s.Equals("R"))
                    {
                        map.btnList[x, y].BackColor = System.Drawing.Color.FromArgb(156, 255, 56);
                    }
                    else if (s.Equals("U"))
                    {
                        map.btnList[x, y].BackColor = System.Drawing.Color.FromArgb(156, 56, 255);
                    }
                    else if (s.Equals("D"))
                    {
                        map.btnList[x, y].BackColor = System.Drawing.Color.FromArgb(56, 255, 255);
                    }
                    else if (s.Equals("L"))
                    {
                        map.btnList[x, y].BackColor = System.Drawing.Color.FromArgb(255, 56, 56);
                    }
                }
            }
            map.btnList[GOALH, GOALW].BackColor = System.Drawing.Color.Gray;
            map.btnList[STARTH, STARTW].BackColor = System.Drawing.Color.Gray;
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
            for (int x = 0; x < episodeNum; x++)
            {
                int w = STARTW, h = STARTH;
                count = 0;
                char action = 'R';
                M.getActions()[h, w] = getBestActionByState(h, w);

                if (reductionType == 1)
                    eps = doSlowReduction(x, eps);
                else if (reductionType == 2)
                    eps = doRapidReduction(x, eps);
                else
                    eps = doNormalReduction(x, eps);

                //if (x % 100 == 0)
                //{
                //    Console.WriteLine("TRIALS : " + x);
                //    printAll();
                //}

                //OPTINAL GUI INTERFACE
                if (gui)
                    resetFrame();

                while ((!(w == GOALW && h == GOALH)))
                {
                    count++;
                    //OPTINAL GUI INTERFACE
                    if (gui)
                        updateFrame(h, w);

                    //addded step limit
                    if (eps < 0.1 & count >= maxSteps)
                    {
                        break;
                    }

                    double stepReward = -1.0;

                    //Set next action a', chosen E-greedily based on Q(s',a')
                    Random rnd = new Random();
                    double r = rnd.NextDouble();
                    if (r <= eps)
                        action = getRandomActionByState(h, w);//choose random action
                    else
                        action = getBestActionByState(h, w);//choose best action
                    M.getActions()[h, w] = action;

                    State s = new State(h, w, action);
                    double currR = stateAction[s];

                    //Take Action A
                    //check to see if we are on ice-
                    if (M.getMap()[h, w] == 'I')
                        icy = true;
                    else
                        icy = false;

                    //If the current action is UP
                    if (M.getActions()[h, w] == 'U')
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
                    else if (M.getActions()[h, w] == 'D')
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
                    else if (M.getActions()[h, w] == 'L')
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
                    else if (M.getActions()[h, w] == 'R')
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

                    double e = currR + alpha * (stepReward + gamma * (maxR - currR));
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

        /*
         * Normal Reduction
         * Pace: 10 episodes
         * Total: 2000 episodes
         * Reduce By: (0.9/Math.floor(x/10.0))
         */
        private double doNormalReduction(int x, Double D)
        {
            if (x >= 1000)
                return 0;
            else if (x % normalUpdate == 0 && x >= normalUpdate)
                return (0.9 / Math.Floor(x / 10.0));
            else
                return D;
        }

        /*
         * Rapid Reduction
         * Pace: 4 episodes
         * Total: 800 episodes
         * Reduce By: (0.9/Math.floor(x/4.0))
         */
        private double doRapidReduction(int x, Double D)
        {
            if (x >= 400)
                return 0;
            else if (x % rapidUpdate == 0 && x >= rapidUpdate)
                return (0.9 / Math.Floor(x / 4.0));
            else
                return D;
        }

        /*
         * Slow Reduction
         * Pace: 25 episodes
         * Total: 5000 episodes
         * Reduce By: (0.9/Math.floor(x/25.0))
         */
        private double doSlowReduction(int x, Double D)
        {
            if (x >= 2500)
                return 0;
            else if (x % slowUpdate == 0 && x >= slowUpdate)
                return (0.9 / Math.Floor(x / 25.0));
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
                    if (map.getMap()[h, w] == 'H')
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
                    if (map.getMap()[h, w] == 'H')
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
                if (map.getActions()[h, w] == 'H')
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
         * Utility method to print various items during debugging and for pratical use
         */
        private void printAll()
        {
            Console.WriteLine("---------------- Actions ----------------");
            for (int x = 0; x < map.getMap().GetLength(0); x++)
            {
                for (int y = 0; y < map.getMap().GetLength(1); y++)
                {
                    Console.Write(map.getActions()[x, y]);
                }
                Console.WriteLine();
            }
        }


        /*
         * initDictonary
         * pre: A given map of data
         * post: A dictionary with all possible state action combinations
         */
        private Dictionary<State, Double> initDictonary(char[,] chars)
        {
            Dictionary<State, Double> dict = new Dictionary<State, Double>();
            for (int x = 0; x < chars.GetLength(0); x++)
            {
                for (int y = 0; y < chars.GetLength(1); y++)
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

