using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IC_ML_MazeSolver
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

    class DataStructures
    {
    }
}
