using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Snake
{
    // This class is responsible for the snake creation, its movement, size growth and the ability of the snake to move past the bounds of the canvas
    public class Snake : PictureBox
    {
        public LinkedList<PictureBox> snakeParts = new LinkedList<PictureBox>();
        private Canvas canvas;
        public Point headPoint;

        public int countSize;
        public Size size;

        public LinkedListNode<PictureBox> head;

        public Snake(Canvas canvas)
        {
            DoubleBuffered = true;
            this.canvas = canvas;
        }

        public void CreateSnake(int tailLength, Size size, Point spawnPoint) // tailLength + head
        {
            countSize = tailLength + 1;
            this.size = size;

            // Create snake's Head
            PictureBox head = new PictureBox();
            head.Location = spawnPoint;
            head.Image = Properties.Resources.SnakeUp25px;
            head.SizeMode = PictureBoxSizeMode.StretchImage;
            head.BackColor = Color.DarkGreen;
            head.Size = size;

            // Add head to linked list and canvas
            snakeParts.AddFirst(head);
            canvas.Controls.Add(head); 
            this.head = snakeParts.First;

            bool firstBody = true;
            for (int i = 0; i < tailLength; i++)
            {
                PictureBox body = new PictureBox();
                if (firstBody)
                {
                    body.Location = new Point(snakeParts.Last.Value.Location.X, snakeParts.Last.Value.Location.Y + snakeParts.Last.Value.Height);
                    firstBody = false;
                }
                else
                {
                    body.Location = new Point(snakeParts.Last.Value.Location.X, snakeParts.Last.Value.Location.Y + snakeParts.Last.Value.Height);

                }
                body.Size = size;
                body.BorderStyle = BorderStyle.FixedSingle;
                body.BackColor = Color.Green;

                // Add body part to linked list and canvas
                canvas.Controls.Add(body);
                snakeParts.AddLast(body);
            }
        }

        public void eat()
        {
            // TODO: Make it so the new part appears more smoothly and not always on the +y axis

            // Create body part
            PictureBox body = new PictureBox();
            body.Location = new Point(snakeParts.Last.Value.Location.X, snakeParts.Last.Value.Location.Y + snakeParts.Last.Value.Height);
            body.BorderStyle = BorderStyle.FixedSingle;
            body.BackColor = Color.Green;
            body.Size = size;

            // Add body part to linked list and canvas
            canvas.Controls.Add(body);
            snakeParts.AddLast(body);
            countSize += 1;
        }

        public void MoveSnake(Keys key)
        {
            // Use switch because why not (try something new)
            switch (key)
            {
                // Check direction and set the head's goal position accordingly
                case Keys.W:
                    head.Value.Image = Properties.Resources.SnakeUp25px;
                    headPoint = new Point(head.Value.Location.X, head.Value.Location.Y - size.Height);
                    //head.Value.Size = size;
                    break;
                case Keys.S:
                    head.Value.Image = Properties.Resources.SnakeDown25px;
                    headPoint = new Point(head.Value.Location.X, head.Value.Location.Y + size.Height);
                    //head.Value.Size = size;
                    break;
                case Keys.A:
                    head.Value.Image = Properties.Resources.SnakeLeft25px;
                    headPoint = new Point(head.Value.Location.X - size.Width, head.Value.Location.Y);
                    //head.Value.Size = size;
                    break;
                case Keys.D:
                    head.Value.Image = Properties.Resources.SnakeRight25px;
                    headPoint = new Point(head.Value.Location.X + size.Width, head.Value.Location.Y);
                    //head.Value.Size = size;
                    break;
            }

            // Move snake
            // We go threw the snake backwards
            LinkedListNode<PictureBox> currentNode = snakeParts.Last;
            while (currentNode != null)
            {
                // if current = head move it to the goal set earlier
                if (currentNode == snakeParts.First)
                {
                    currentNode.Value.Location = headPoint;
                }
                else
                {
                    // move parts
                    currentNode.Value.Location = new Point(currentNode.Previous.Value.Location.X, currentNode.Previous.Value.Location.Y);

                }
                // Move current node
                currentNode = currentNode.Previous;
            }

            // Check if the head of the snake goes out of canvas bounds and place it accordingly at the opposite position
            if (head.Value.Location.X <= 0 - head.Value.Width) // From left to the right
            {
                head.Value.Location = new Point(canvas.Width - head.Value.Width, head.Value.Location.Y);
            }
            else if (head.Value.Location.X >= canvas.Width) // From right to the left
            {
                head.Value.Location = new Point(0, head.Value.Location.Y);
            }
            else if (head.Value.Location.Y <= 0 - head.Value.Width) // from the top to bottom
            {
                head.Value.Location = new Point(head.Value.Location.X, canvas.Height - head.Value.Height);
            }
            else if (head.Value.Location.Y >= canvas.Height) // from bottom to top
            {
                head.Value.Location = new Point(head.Value.Location.X, 0);
            }

        }

    }
}
