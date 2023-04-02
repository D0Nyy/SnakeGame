using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Snake
{
    // This class is responsible for the apple spawn / deletion and its score value
    public class Apple
    {
        private Random random;
        private Canvas canvas;
        private Snake snake;
        private PictureBox apple;
        public bool golden;

        public int scoreGain;
        public Apple(Canvas canvas, Snake snake)
        {
            this.snake = snake;
            this.canvas = canvas;
            golden = false;
        }

        public void CreateApple()
        {
            random = new Random((int)DateTime.Now.Ticks);

            apple = new PictureBox();
            apple.Size = new Size(30, 30);
            apple.Location = new Point(random.Next(0,canvas.Width - apple.Width), random.Next(0, canvas.Height - apple.Height));

            // We check if the apple is touching a part of the snake on spawn( we don't want that)
            while (true)
            {
                bool touching = false;
                LinkedListNode<PictureBox> currentNode = snake.head;

                while (currentNode != null)
                {
                    if (currentNode.Value.Bounds.IntersectsWith(apple.Bounds))
                    {
                        touching = true;
                        break;
                    }
                    currentNode = currentNode.Next;
                }

                if (touching)
                {
                    apple.Location = new Point(random.Next(0, canvas.Width - apple.Width), random.Next(0, canvas.Height - apple.Height));
                }
                else
                {
                    break;
                }
            }

            if (random.Next(0, 5) == 1) // 1 in 5 chance to get golden apple
            {
                apple.Image = Properties.Resources.green;
                scoreGain = 2;
            }
            else
            {
                apple.Image = Properties.Resources.apple;
                scoreGain = 1;
            }
            apple.BackColor = Color.Transparent;
            apple.SizeMode = PictureBoxSizeMode.StretchImage;

            // Add apple to canvas
            canvas.Controls.Add(apple);
            apple.Parent = canvas;
            canvas.Controls.Add(apple);
        }

        public void Remove()
        {
            canvas.Controls.Remove(apple);
        }

        public PictureBox GetApple()
        {
            return apple;
        }

    }
}
