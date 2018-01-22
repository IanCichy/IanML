using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IC_ML_MazeSolver
{
    public class DataStructures
    {
        public int GOALW = 0, GOALH = 0, STARTW = 0, STARTH = 0;
        //The map controller
        public Map map { set; get; }
        //Constant Variables
        public double gamma = 0.9, epsilon = 0.9, alpha = 0.9, lambda = 0.9, reductionConstant = 10;
        public int stepCount = 0, maxSteps = 0, totalEpisodesToRun = 2000;
        //Dictionaries	
        public Dictionary<State, Double> stateAction;
        public Dictionary<State, Double> elgStateAction;
        public List<Tuple<int, int>> previousLocations = new List<Tuple<int, int>>();
        //Tile Data
        public enum Tiles { Start, Goal, Open, Hole, Icy };
        public Dictionary<char, Tiles> tileTypes = new Dictionary<char, Tiles>() {
            {'O', Tiles.Open},
            {'S', Tiles.Start},
            {'G', Tiles.Goal},
            {'H', Tiles.Hole},
            {'I', Tiles.Icy},
        };
        //Action Data
        public enum Actions { UP, DOWN, LEFT, RIGHT, NONE };
        //RandomNumberGen
        public Random rndNumGen = new Random(DateTime.Now.Millisecond);


        public DataStructures(string filename)
        {
            readMapData(filename);
        }

        public void initState(int totalEpisodesToRun, double reductionConstant)
        {
            //BUILD OTHER DATA STRUCTURES
            maxSteps = map.height * map.width * 3;
            stateAction = initDictonary();
            elgStateAction = initDictonary();
        }


        /// <summary>
        /// Reads in the map data and initalizes the environment
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public void readMapData(String FileName)
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

                        switch (t.Type)
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

                        if (t.Type == Tiles.Start)
                        {
                            STARTW = y;
                            STARTH = x;
                        }
                        else if (t.Type == Tiles.Goal)
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
            this.map = newMap;
        }

        /*
         * initDictonary
         * pre: A given map of data
         * post: A dictionary with all possible state action combinations
         */
        public Dictionary<State, Double> initDictonary()
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

        public int[] takeAction(Map M, int h, int w, Actions act, bool icy)
        {
            int stepReward = -1;
            if (M.tiles[h, w].Action == Actions.UP)
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
            else if (M.tiles[h, w].Action == Actions.DOWN)
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
            else if (M.tiles[h, w].Action == Actions.LEFT)
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
            else if (M.tiles[h, w].Action == Actions.RIGHT)
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
        /// Decides if you slide or not when you are on an icy tile
        /// pre: Given the current h, w pair and the move { U,D,L,R }
        /// post: Rolls a random number and returns a triple, {new h, new w, reward]
        /// for a move depending on if we slipped or not, moved into a hole, tried to move off the map, etc. 
        /// </summary>
        /// <param name="h"></param>
        /// <param name="w"></param>
        /// <param name="act"></param>
        /// <returns></returns>
        public int[] getIcyMove(int h, int w, Actions act)
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

        /// <summary>
        /// * Move validity checker
        /// pre: Given the current h, w pair and the move { U,D,L,R }
        /// post: Checks if the move would take the agent off the map, to a free space
        /// or into a hole and returns accordingly
        /// {Hole = -1
        /// normal move = 1
        /// off map = -2}
        /// </summary>
        /// <param name="h"></param>
        /// <param name="w"></param>
        /// <param name="act"></param>
        /// <returns></returns>
        public int isValidMove(int h, int w, Actions act)
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

        /// <summary>
        /// Returns best action
        /// pre: Given a h,w pair in the map
        /// post: Returns the best action { U,D,L,R }
        /// based on the scores of the moves at this location
        /// </summary>
        /// <param name="h"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        public Actions getBestActionByState(int h, int w)
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

        /// <summary>
        /// Calcuates the reduction constatnt based on
        /// (0.9/Math.floor(x/reductionConst))
        /// </summary>
        /// <param name="x"></param>
        /// <param name="D"></param>
        /// <returns></returns>
        public double calcuateReductionConstant(int x, Double D)
        {
            if (x >= (totalEpisodesToRun / 2))
                return 0;
            else if (x % reductionConstant == 0 && x >= reductionConstant)
                return (0.9 / Math.Floor(x / reductionConstant));
            else
                return D;
        }
    }
}
