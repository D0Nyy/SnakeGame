using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    public class Canvas : Panel
    {
        public Canvas()
        {
            DoubleBuffered = true;
        }
    }
}
