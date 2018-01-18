using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IC_ML_MazeSolver.frmMazeSolver;

namespace IC_ML_MazeSolver
{
    public class State : IComparable
    {
        private int xPos { get; set; }
        private int yPos { get; set; }
        private Actions action { get; set; }

        public State(int y, int x, Actions a)
        {
            xPos = x;
            yPos = y;
            action = a;
        }

        public String toString()
        {
            return "Height: " + yPos + ", Width : " + xPos + ", Action : " + action;
        }

        public int CompareTo(Object other)
        {
            State S = (State)other;
            if (this.xPos == S.xPos && this.yPos == S.yPos && this.action == S.action)
                return 0;
            else
                return -1;
        }

        public override int GetHashCode()
        {
            int hashcode = 0;
            hashcode = this.xPos * 20;
            hashcode += this.yPos;
            return hashcode;
        }

        public override bool Equals(Object other)
        {
            State S = (State)other;
            if (this.xPos == S.xPos && this.yPos == S.yPos && this.action == S.action)
                return true;
            else
                return false;
        }
    }
}
