using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    public partial class Settings : Form
    {

        public int speed = 100;
        public bool increasingSpeed = false;
        public string username;


        public Settings()
        {
            InitializeComponent();
            radioButton1.Checked = true;
            listBox1.SelectedItem = null;
            textBox1.Enabled = false;
        }

        private Point previousLocation;
        private bool sliding;
        private int sliderValue = 100;
        private void slider_MouseDown(object sender, MouseEventArgs e)
        {
            sliding = true;
            previousLocation = e.Location;
        }

        private void slider_MouseMove(object sender, MouseEventArgs e)
        {
            if (sliding)
            {
                Point newLocation = slider.Location;
                // e.location -> BUTTONS LOCATION so where the mouse is relative to the button
                // x and y if its in the miidle is like x = 20 so it adds 20 to the new location
                newLocation.Offset(0, e.Location.Y - previousLocation.Y);
                // newLocation = new Point(slider.Location.X + e.Location.X - previousLocation.X, 0); // the same as above
                if (!(newLocation.Y + slider.Height >= slidingPanel.Height))
                {
                    if (!(newLocation.Y <= -2))
                    {
                        slider.Location = newLocation;
                        sliderValue = slider.Location.Y + 10;
                    }
                }
            }
            else
            {
                return;
            }
        }

        private void slider_MouseUp(object sender, MouseEventArgs e)
        {
            sliding = false;
            speed = sliderValue;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Game game;
            username = textBox1.Text;
            if (increasingSpeed)
            {
                game = new Game(username); // increasing speed game
            }
            else
            {
                game = new Game(username, speed); // static speed game
            }
            this.Hide();
            game.Show();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                radioButton1.Checked = true;
                staticButton.Checked = false;

                slidingPanel.Enabled = false;
                label7.Enabled = false;
                label8.Enabled = false;
                label9.Enabled = false;

                increasingSpeed = true;
            }
        }

        private void staticButton_CheckedChanged(object sender, EventArgs e)
        {
            if (staticButton.Checked)
            {
                staticButton.Checked = true;
                radioButton1.Checked = false;

                slidingPanel.Enabled = true;
                label7.Enabled = true;
                label8.Enabled = true;
                label9.Enabled = true;

                increasingSpeed = false;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                if (!(listBox1.SelectedItem == "<Add New User>"))
                {
                    label4.Visible = true;
                    textBox1.Text = listBox1.SelectedItem.ToString();
                    label4.Text = "Best Score:" + scores[listBox1.SelectedIndex];
                    textBox1.Enabled = false;
                }
                else // if selected user is new user ask for name
                {
                    label4.Visible = false;
                    textBox1.Clear();
                    button1.Enabled = false;
                    textBox1.Enabled = true;
                }
            }
        }


        List<string> scores = new List<string>();
        private void Settings_Load(object sender, EventArgs e)
        {
            try
            {
                StreamReader sr = new StreamReader("Score.txt");
                string s = sr.ReadLine();

                while (s != null)
                {
                    string[] sl = s.Split('.');
                    scores.Add(sl[1]);
                    listBox1.Items.Add(sl[0]);
                    s = sr.ReadLine();
                }

                sr.Close();

                listBox1.Items.Add("<Add New User>");

            }
            catch (FileNotFoundException)
            {
                StreamWriter sw = new StreamWriter("Score.txt");
                sw.Close();
                listBox1.Items.Add("<Add New User>");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                button1.Enabled = false;
            }
            else
            {
                button1.Enabled = true;
            }
        }

        private void Settings_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private bool flag = true;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (flag)
            {
                slider.BackgroundImage = Properties.Resources.closeeye;
                flag = false;
            }
            else
            {
                slider.BackgroundImage = Properties.Resources.SnakeDown25px;
                flag = true;
            }

        }
    }
}
