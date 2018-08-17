/*
 * Author: Evan Brooks
 * Date: 7/24/2018
 * Synopsis: prototype for color guessing game
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ColorRiotPrototype
{
    class Player
    {
        /*
         * _____________________________________________________________________________________________________________________________________________________
         * Attributes
         */

        public string name { get; set; }
        public int score { get; set; }
		public bool current { get; set; }
        private bool winner { get; set; }

        /*
         * _____________________________________________________________________________________________________________________________________________________
         * Constructor
         */

        public Player (string name, int score, bool current, bool winner)
        {
            this.name = name;
            this.score = score;
			this.current = current;
			this.winner = winner;
        }

        /*
         * _____________________________________________________________________________________________________________________________________________________
         * Behaviors
         */

        //Primarily for debug purposes - create string containing player information
        public override string ToString()
        {
            string print = " Score: " + score + " Winner: " + winner + " Current: " + current;
            return print;
        }

        //Check if the player has the score neccessary to win the game
        public bool ScoreCheck()
        {
            if (score >= 1000)
            {
                winner = true;
                return winner;
            }
            else
            {
                winner = false;
                return winner;
            }
        }

        //Add 100 points to the player's score
        public int ScoreAdd()
        {
            score += 50;
			Debug.Print("Score: " + score.ToString());

            return score;
        }
    }
}
