/*
 * Author: Evan Brooks
 * Date: 7/24/2018
 * Synopsis: prototype for color guessing game
 * 
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace ColorRiotPrototype
{
	public partial class frmMain : Form
	{
		public frmMain()
		{
			InitializeComponent();
		}

		/*
         * _____________________________________________________________________________________________________________________________________________________
         * Variables and Objects
         */

		//Initialize variables to store player and item object lists
		List<Player> playerList = new List<Player>();
		List<Item> itemList = new List<Item>();

        //Initialize variables to store random number and timer objects
		Random random = new Random();
        Timer turnTimer;

		//Initialize variables to store number of players
		private int playerCount = 0;

        /*
         * _____________________________________________________________________________________________________________________________________________________
         * Interface Functions
         */

        //Event handler completes several actions on form load
        //Primarily, several overlapping panels are hidden - a system of hiding and revealing overlapping panels replicates a mobile interface experience
		private void frmMain_Load(object sender, EventArgs e)
		{
			plHelp.Enabled = false;
			plHelp.Visible = false;
			plPlayers.Enabled = false;
			plPlayers.Visible = false;
			plReadyUp.Enabled = false;
			plReadyUp.Visible = false;
			plPlayGame.Enabled = false;
			plPlayGame.Visible = false;
			plEndGame.Enabled = false;
			plEndGame.Visible = false;

			lblSubTitle.Text = "";
		}

		//Event handler hides the main menu panel and opens the player select panel
		private void btnPlay_Click(object sender, EventArgs e)
		{
			plMenu.Enabled = false;
			plMenu.Visible = false;

			lblSubTitle.Text = "Player Game";

			plPlayers.Enabled = true;
			plPlayers.Visible = true;
		}

		//Event handler hides the main menu panel and opens the help panel
		private void btnHelp_Click(object sender, EventArgs e)
		{
			plMenu.Enabled = false;
			plMenu.Visible = false;

			lblSubTitle.Text = "Help";

			plHelp.Enabled = true;
			plHelp.Visible = true;
		}

		//Event handler hides the help panel and opens the menu panel
		private void btnHelpBack_Click(object sender, EventArgs e)
		{
			plHelp.Enabled = false;
			plHelp.Visible = false;

			lblSubTitle.Text = "";

			plMenu.Enabled = true;
			plMenu.Visible = true;
		}

		//Event handler prompts user to confirm application exit
		private void btnExit_Click(object sender, EventArgs e)
		{
			DialogResult exit = MessageBox.Show("Ready to go?", "Attention!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

			if (exit == DialogResult.Yes)
			{
				this.Close();
			}
		}

		//The following player button Event handler sets the number of particpating players and passes that number
		//to the playersetup function to instantiate the corresponding number of player objects

		private void btn2Players_Click(object sender, EventArgs e)
		{
			playerCount = 2;
			PlayerSetup(playerCount);
		}

		private void btn3Players_Click(object sender, EventArgs e)
		{
			playerCount = 3;
			PlayerSetup(playerCount);
		}

		private void btn4Players_Click(object sender, EventArgs e)
		{
			playerCount = 4;

			PlayerSetup(playerCount);
		}

		//Event handler hides the ready up panel, opens the play game panel, and prepares the game for the next player
		private void btnReady_Click(object sender, EventArgs e)
		{
			plReadyUp.Enabled = false;
			plReadyUp.Visible = false;

			lblSubTitle.Text = "Play Game";

			plPlayGame.Enabled = true;
			plPlayGame.Visible = true;

            //Run functions to setup next player's turn
			ItemSetup();
			RandSetup();
            TimerSetup();
		}

		//Event handler executes the refresh game function
		private void btnCorrect_Click(object sender, EventArgs e)
		{
			RefreshGame();
		}

		//Event handler hides the end game panel and opens the main menu panel
		private void btnFinish_Click(object sender, EventArgs e)
		{
			plEndGame.Enabled = false;
			plEndGame.Visible = false;

			lblSubTitle.Text = "";

			plMenu.Enabled = true;
			plMenu.Visible = true;
		}

		/*
         * _____________________________________________________________________________________________________________________________________________________
         * Indepedent Functions
         */

		//Function instantiates a number of player objects according to the playercount
		private void PlayerSetup(int playerCount)
		{
			int x = 0;
			int playerCountTemp = playerCount;

			while (x <= playerCountTemp - 1)
			{
				//Instantiate player object and add to playerlist
				Player player = new Player("Name", 0, false, false);
				playerList.Add(player);

				playerCountTemp--;
			}

			//Switch bool for current player status
			playerList[0].current = true;

			//Call the playGame function to progress to gameplay interface
			PlayGame();
		}

        //Function hides the players panel and opens the ready up panel
        private void PlayGame()
		{
			plPlayers.Enabled = false;
			plPlayers.Visible = false;

			lblSubTitle.Text = "Ready Up";

			plReadyUp.Enabled = true;
			plReadyUp.Visible = true;
		}

		//Function increments the current player's score and changes current item / color
		private void RefreshGame()
		{
			bool check;

			foreach (Player player in playerList)
			{
				if (player.current == true)
				{
					player.ScoreAdd();
				}

				check = player.ScoreCheck();

				if (check == true)
				{
					EndGame();
				}
				else
				{
					RandSetup();
				}
			}
		}

		//Function rotate 'current' player to the 'next' player according to player count
		private void NextPlayer()
		{
			foreach (Player player in playerList)
			{
				int index = playerList.IndexOf(player);

				if (player.current == true)
				{
					player.current = false;

					//Divide index of next player by player count and reset index as the remainder
					// % = Division W/ remainder as result // Example: 1 + 1 % 3 -> 3 goes into 2 zero times with a remainder of 2
					index = (playerList.IndexOf(player) + 1) % playerCount;
					playerList[index].current = true;

					break;
				}
			}

			plPlayGame.Enabled = false;
			plPlayGame.Visible = false;

			lblSubTitle.Text = "Ready up!";

			plReadyUp.Enabled = true;
			plReadyUp.Visible = true;
		}

        //Function hides play game panel and opens the end game panel with specific winner declaration
        private void EndGame()
        {
            //Stop the turn timer
            turnTimer.Stop();

            plPlayGame.Enabled = false;
            plPlayGame.Visible = false;

            lblSubTitle.Text = "Game over!";

            plEndGame.Enabled = true;
            plEndGame.Visible = true;

            //Initialize variables to construct winning player name
            string winnerStr;
            int winnerInd = 0;
            
            //Check for player number
            foreach (Player player in playerList)
            {
                if (player.current == true)
                    winnerInd = playerList.IndexOf(player) + 1; //Index 0 = Player 1, Index 1 = Player 2, etc
                break;
            }

            //Construct winning player name with title and player number
			winnerStr = "Player " + winnerInd;
			lblWinner.Text = winnerStr;

			//Reset game settings and player objects
			playerCount = 0;

			foreach (Player player in playerList)
			{
				player.score = 0;
				player.current = false;
			}
		}

		//Function populates the item list with item object
		private void ItemSetup()
		{
            //Hardcode item objects
			Item i1 = new Item("Apple", "Red");
			Item i2 = new Item("Lemon", "Yellow");
			Item i3 = new Item("Grape", "Purple");
			Item i4 = new Item("Grass", "Green");
			Item i5 = new Item("Sky", "Blue");
            Item i6 = new Item("Tree", "Brown");
            Item i7 = new Item("Carrot", "Orange");
            Item i8 = new Item("Sun", "Yellow");
            Item i9 = new Item("Moon", "White");
            Item i10 = new Item("Water", "Blue");

            //Add items objects to list
			itemList.Add(i1);
			itemList.Add(i2);
			itemList.Add(i3);
			itemList.Add(i4);
			itemList.Add(i5);
            itemList.Add(i6);
            itemList.Add(i7);
            itemList.Add(i8);
            itemList.Add(i9);
            itemList.Add(i10);
        }

		//Function initializes rand integer and generates a random number within scope of the item list
		private void RandSetup()
		{
            int rand = 0;

            //Set random number equal to an object in the item list between its maximum and minimum indices
			rand = random.Next(0, itemList.Count() - 1);

			//Use the random number to pull item object information from item list
			lblItemDisp.Text = itemList[rand].name;
			lblColorDisp.Text = itemList[rand].color;
		}

		//Function instantiates and initializes a turn timer with a 30 second interval
		private void TimerSetup()
		{
            //Instantiate new timer object into reference variable to 'reset' the timer
            turnTimer = new Timer();

			turnTimer.Interval = (30000); //time in milliseconds
            turnTimer.Tick += new EventHandler(turnTimer_Tick);
            turnTimer.Start();
		}

		//Event handler executes when timer reaches 30 seconds
		private void turnTimer_Tick(object sender, EventArgs e)
		{
            turnTimer.Stop();
			NextPlayer();
		}
	}
}
