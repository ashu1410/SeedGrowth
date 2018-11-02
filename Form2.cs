using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeedGrowth
{
    public partial class Form2 : Form
    {
        
        public Form2()
        { 
            
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            
        }


        public PictureBox pictureBox
        {
            get { return pictureBox1; }
            
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
