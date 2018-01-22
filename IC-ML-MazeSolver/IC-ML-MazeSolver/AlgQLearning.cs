﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IC_ML_MazeSolver.DataStructures;

namespace IC_ML_MazeSolver
{
    public class AlgQLearning
    {
        DataStructures data;

        public AlgQLearning()
        {

        }

        public void Learn(DataStructures dat, int totalEpisodesToRun, double reductionConstant)
        {
            data = dat;
            dat.initState(totalEpisodesToRun, reductionConstant);
            Q_Learning(data.map);
        }

        public void Q_Learning(Map M)
        {
            bool icy = false;
            for (int currentEpisodeNumber = 0; currentEpisodeNumber < data.totalEpisodesToRun; currentEpisodeNumber++)
            {
                //if (gui)
                //{
                //    previousLocations.Clear();
                //}
                //lblEpisodeProgress.Text = (currentEpisodeNumber + 1) + "/" + totalEpisodesToRun;
                //pgbEpisodeNum.Value = currentEpisodeNumber + 1;

                int w = data.STARTW, h = data.STARTH;
                int prew = data.STARTW, preh = data.STARTH;
                data.stepCount = 0;

                Actions stepAction;
                data.epsilon = data.calcuateReductionConstant(currentEpisodeNumber, data.epsilon);

                //OPTINAL GUI INTERFACE
                //if (gui)
                //    resetFrame();

                while ((!(w == data.GOALW && h == data.GOALH)))
                {
                    data.stepCount++;
                    icy = false;

                    //if (gui)
                    //{
                    //    previousLocations.Add(new Tuple<int, int>(h, w));
                    //}

                    prew = w;
                    preh = h;
                    //OPTINAL GUI INTERFACE
                    //if (gui)
                    //    updateFrame(h, w, preh, prew);

                    //addded step limit
                    if (data.stepCount > data.maxSteps)
                    {
                        break;
                    }

                    double stepReward = -1.0;

                    //Set next action a', chosen E-greedily based on Q(s',a')
                    double r = data.rndNumGen.NextDouble();
                    if (r <= data.epsilon)
                        stepAction = data.getRandomAction();//choose random action
                    else
                        stepAction = data.getBestActionByState(h, w);//choose best action
                    M.tiles[h, w].Action = stepAction;

                    State s = new State(h, w, stepAction);
                    double currR = data.stateAction[s];

                    //Take Action A

                    if (M.tiles[h, w].Type == Tiles.Icy)
                        icy = true;

                    var returnVector = data.takeAction(M, h, w, M.tiles[h, w].Action, icy);
                    h = returnVector[0];
                    w = returnVector[1];
                    stepReward = returnVector[2];

                    //find Max Q(s',a')
                    Actions t = data.getBestActionByState(h, w);
                    double maxR = data.stateAction[(new State(h, w, t))];

                    double e = currR + data.alpha * (stepReward + data.lambda * (maxR - currR));
                    data.stateAction[s] = e;
                }
            }
        }
    }
}
