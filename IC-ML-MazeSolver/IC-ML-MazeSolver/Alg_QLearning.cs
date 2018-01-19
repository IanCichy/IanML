using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IC_ML_MazeSolver
{
    class Alg_QLearning
    {
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


}
