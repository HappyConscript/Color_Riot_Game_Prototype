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

namespace ColorRiotPrototype
{
    class Item
    {
        /*
         * _____________________________________________________________________________________________________________________________________________________
         * Attributes
         */

        public string name { get; set; }
        public string color { get; set; }

        /*
         * _____________________________________________________________________________________________________________________________________________________
         * Constructor
         */

        public Item(string name, string color)
        {
            this.name = name;
            this.color = color;
        }

        /*
         * _____________________________________________________________________________________________________________________________________________________
         * Behaviors
         */

        //Primarily for debug purposes - create string containing player information
        public override string ToString()
        {
            string print = "Name: " + name + " Color: " + color;
            return print;
        }
    }
}
