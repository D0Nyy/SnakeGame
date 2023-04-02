using System;
using System.Drawing;
using System.Windows.Forms;

namespace Snake
{
    public partial class End : Form
    {
        private Game game;
        private Game newGame;
        public End(Game game)
        {
            this.game = game;
            InitializeComponent();
            this.BackColor = Color.LightGreen;

            if (game.bestScore < game.score)
            {
                label4.Text = game.score.ToString();
                label3.Text = game.score.ToString();
                label6.Visible = true;
            }
            else
            {
                label4.Text = game.score.ToString();
                label3.Text = game.bestScore.ToString();
            }
            label5.Text = game.username;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void End_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        // Try Again TODO MAKE IT REAL RESTART
        private void button2_Click(object sender, EventArgs e)
        {

            if (game.increasing)
            {
                this.newGame = new Game(game.username); 
            }
            else
            {
                this.newGame = new Game(game.username, game.defSpeed);
            }
            newGame.Show();

            // Free memory of old game form
            game.Dispose();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Create new settings so the user update
            Settings settings = new Settings();
            settings.Show();
            this.Hide();
        }

        //TODO EYES CLOSE FOR LESS TIME
        private bool flag = true;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (flag)
            {
                pictureBox1.Image = Properties.Resources.closeeye;
                pictureBox2.Image = Properties.Resources.closeeye;
                flag = false;
            }
            else
            {
                pictureBox1.Image = Properties.Resources.SnakeDown25px;
                pictureBox2.Image = Properties.Resources.SnakeDown25px;
                flag = true;
            }

        }
    }
}
