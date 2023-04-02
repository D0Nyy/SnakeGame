using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Snake
{
    //ANIMATE EYES
    public partial class Game : Form
    {
        public int speed;
        public string username;
        public bool increasing;
        private string savedWOuser;

        public Canvas canvas;
        private Snake snake;
        private Apple apple;

        public int defSpeed;
        private bool gameOver = false;
        private bool started = false;
        private bool canMove = true;

        public int score;
        public int bestScore;

        // Default direction of the snake is forward
        private Keys lastMovement = Keys.W;

        // Default constructor
        private Game()
        {
            DoubleBuffered = true;
            InitializeComponent();
        }

        // Increasing constructor
        public Game(string username) : this()
        {
            speed = 150;
            this.username = username;
            increasing = true;

            tick.Interval = speed;
        }

        // Fixed Speed constructor
        public Game(String username, int speed) : this()
        {
            this.username = username;
            increasing = false;
            defSpeed = speed;
            this.speed = speed;

            tick.Interval = speed;
        }

        private void Game_Load(object sender, EventArgs e)
        {
            // Create Canvas
            canvas = new Canvas();
            canvas.Location = new Point(0, 100);
            canvas.Size = new Size(800, 700);
            canvas.BorderStyle = BorderStyle.FixedSingle;
            canvas.BackColor = Color.LightGreen; // grass?!?! xD
            this.Controls.Add(canvas);
            canvas.Parent = this;

            // Create snake
            snake = new Snake(canvas);
            snake.CreateSnake(4, new Size(50, 50), spawnPoint.Location); //spawnPoint = the pictureBox with the snake head in the designer

            // Spawn the first apple
            apple = new Apple(canvas, snake);
            apple.CreateApple();

            // Initialize info
            scoreLabel.Parent = this;
            labelStart.Parent = canvas;
            this.BackColor = Color.Gray;

            // Read score from file or Create file if missing
            try
            {
                StreamReader sr = new StreamReader("Score.txt");
                string s = sr.ReadLine();
                bestLabel.Text = "Best:0";
                bestScore = 0;
                string[] sl;

                while (s != null)
                {
                    sl = s.Split('.');
                    if (sl[0].Equals(username)) // compare username with the current file username so we don't create duplicates
                    {
                        bestScore = int.Parse(sl[1]);
                        bestLabel.Text = "Best:" + bestScore.ToString();
                        s = sr.ReadLine();
                    }
                    else
                    {
                        savedWOuser += s + Environment.NewLine;
                        s = sr.ReadLine();
                    }
                }

                sr.Close();
            }
            catch (FileNotFoundException)
            {
                StreamWriter sw = new StreamWriter("Score.txt");
                sw.Close();
                bestLabel.Text = "Best:0";
                this.bestScore = 0;
            }
        }

        private void tick_Tick(object sender, EventArgs e)
        {
            // Check if the game isn't over and has started
            if (!gameOver && started)
            {
                // We move the snake
                // We put this here so the snake continues to move each tick on its own
                snake.MoveSnake(lastMovement);
                canMove = true; // Bug: This fixed a bug that was created when pressing many movement keys together(it resulted in instant death)

                // We detect if the snake ate an apple TODO make it so it looks like it eats it from its mouth
                if (snake.head.Value.Bounds.IntersectsWith(apple.GetApple().Bounds))
                {
                    score += apple.scoreGain;
                    scoreLabel.Text = "Score:" + score.ToString();

                    // check gameMode if its increasing.
                    if (increasing)
                    {
                        speed = speed - apple.scoreGain;
                        tick.Interval = speed;
                    }

                    apple.Remove();
                    apple.CreateApple();
                    snake.eat();

                    // Check if the current score is higher than the current best score
                    // And display it 
                    if (score > bestScore)
                    {
                        bestLabel.Text = "Best:" + bestScore.ToString() + "  +" + (score - bestScore).ToString();
                    }

                }


                // Check if the snake hit its tail
                // TODO make it so it looks like it ate it ( i will need to redo the end game state)
                LinkedListNode<PictureBox> currentNode = snake.head.Next; // We start checking from the body part after the head
                while (currentNode != null)
                {
                    if (snake.head.Value.Bounds.IntersectsWith(currentNode.Value.Bounds))
                    {
                        gameOver = true;
                        // Write score to file only if the current score is higher than the best score
                        if (score > bestScore)
                        {
                            try
                            {
                                StreamWriter sw = new StreamWriter("Score.txt");
                                sw.Write(savedWOuser + username + "." + score.ToString());
                                sw.Close();
                            }
                            catch (Exception)
                            {

                            }
                        }
                        // We create gameOver screen.
                        // We are giving it this instance of the game with all its attributes
                        // This way we can delete the instance in the End form so we don't have multiple game instances running.
                        End end = new End(this);
                        end.Show();
                        this.Hide();
                        break;
                    }
                    currentNode = currentNode.Next;
                }
            }


        }

        // Movement check & Game start
        private void Game_KeyDown(object sender, KeyEventArgs e)
        {
            // started = false until the user presses "Space" key
            if (!started)
            {
                if (e.KeyData == Keys.Space)
                {
                    started = true;
                    canvas.Controls.Remove(labelStart);
                }
            }

            // We check if we can move ( again this variable fixed an issue that was created when we pressed multiple movement keys in one tick )
            if (canMove)
            {
                // We don't allow the user to go backwards 
                if (e.KeyData == Keys.W)
                {
                    if (lastMovement != Keys.S)
                    {
                        lastMovement = Keys.W;
                        canMove = false;
                    }

                }
                else if (e.KeyData == Keys.S)
                {
                    if (lastMovement != Keys.W)
                    {
                        lastMovement = Keys.S;
                        canMove = false;
                    }
                }
                // We don't brake the else if here because we don't want the user to be able to go diagonally easily
                else if (e.KeyData == Keys.A)
                {
                    if (lastMovement != Keys.D)
                    {
                        lastMovement = Keys.A;
                        canMove = false;
                    }
                }
                else if (e.KeyData == Keys.D)
                {
                    if (lastMovement != Keys.A)
                    {
                        lastMovement = Keys.D;
                        canMove = false;
                    }
                }
            }
        }

        private void Game_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

    }
}
