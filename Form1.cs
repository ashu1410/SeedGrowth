using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Windows.Forms;
using CellularAutomata;
using System.Runtime.InteropServices;
namespace SeedGrowth
{
    enum BoundaryConditions
    {
        Normal,
        Periodic
    }
    enum Neighbourhood
    {
        Moore,
        VonNoyman,
        RandomPentagonal,
        RandomHexagonal
    }
    enum SeedDraw
    {
        Random,
        Evenly,
       RandomWithRadius,
        ByClick
    }
    public partial class Form1 : Form
    {
         SeedGrowth seedgtowth = null;
        MonteCarlo monte = null;
         Form2 simulationDisplayer;
        BackgroundWorker worker = new BackgroundWorker();
        int width;
        int height;
        List<String> bc = new List<String>();
        List<String> nh = new List<String>();
        List<String> sd = new List<String>();
        neigbhourhoodType neigbhourhoodType;
        internal SeedGrowth Automata
        {
            get { return seedgtowth; }     
            set { seedgtowth = value; }
        }

        internal neigbhourhoodType NeigbhourhoodType { get {return neigbhourhoodType; } set { neigbhourhoodType = value; } }

        public Form1()
        {
            
            InitializeComponent();

            foreach (var condition in Enum.GetNames(typeof(BoundaryConditions)))
                bc.Add(condition);
            foreach (var condition in Enum.GetNames(typeof(Neighbourhood)))
                nh.Add(condition);
            foreach (var condition in Enum.GetNames(typeof(SeedDraw)))
                sd.Add(condition);

            comboBox1.DataSource = new BindingSource(bc, null);
            comboBox2.DataSource = new BindingSource(nh, null);
            comboBox3.DataSource = new BindingSource(sd, null);
        }

        private void PictureBox1_MouseClick1(object sender, MouseEventArgs e)
        {
            int x = Convert.ToInt32(e.X );
            int y = Convert.ToInt32(e.Y );
            ((SeedGrowth)(seedgtowth)).setSeed(x, y);
            refreshView2();
        }

        private  void button1_Click(object sender, EventArgs e)
        {
           
            {
                if (worker.IsBusy)
                    seedgtowth?.stop();


                simulationDisplayer?.Dispose();
                width = Convert.ToInt32(textBox1.Text == "" ? "300" : textBox1.Text);
                height = Convert.ToInt32(textBox2.Text == "" ? "300" : textBox2.Text);
                int seeds = Convert.ToInt32(textBox3.Text == "" ? "3" : textBox3.Text);

                seedgtowth = new SeedGrowth(width, height);
                

                simulationDisplayer = new Form2();
                simulationDisplayer.Size = new Size(width + 20, height + 45);
                simulationDisplayer.pictureBox.Size = new Size(width, height);
                simulationDisplayer.Disposed += Form2_Disposed;
                simulationDisplayer.pictureBox.MouseClick += PictureBox1_MouseClick1;
                switch ((String)comboBox3.SelectedItem)
                {
                    case "Random":

                        seedgtowth.setSeedsRandomly(seeds);
                        break;
                    case "Evenly":
                        int xasisseeds = Convert.ToInt32(textBox4.Text == "" ? "3" : textBox4.Text);
                        int yaxisseeds = Convert.ToInt32(textBox5.Text == "" ? "3" : textBox5.Text);
                        seedgtowth.setSeedsEvenly(xasisseeds, yaxisseeds);
                        break;
                    case "RandomWithRadius":
                        int radius = Convert.ToInt32(textBox6.Text == "" ? "3" : textBox6.Text);
                        seedgtowth.setSeedswithRadius(seeds, radius);
                        break;
                    case "ByClick":
                        simulationDisplayer.pictureBox.MouseClick += PictureBox1_MouseClick1;
                        break;
                }


                switch ((string)comboBox1.SelectedValue)
                {
                    case "Normal":
                        switch ((string)comboBox2.SelectedValue)
                        {
                            case "Moore":
                                neigbhourhoodType = neigbhourhoodType.Moore;
                                break;
                            case "VonNoyman":
                                neigbhourhoodType = neigbhourhoodType.VonNoyman;
                                break;
                            case "RandomPentagonal":
                                neigbhourhoodType = neigbhourhoodType.PentagonalRandom;
                                break;
                            case "RandomHexagonal":
                                neigbhourhoodType = neigbhourhoodType.HexagonalRandom;
                                break;
                        }
                        break;
                    case "Periodic":
                        switch ((string)comboBox2.SelectedValue)
                        {
                            case "Moore":
                                neigbhourhoodType = neigbhourhoodType.MoorePeriodic;
                                break;
                            case "VonNoyman":
                                neigbhourhoodType = neigbhourhoodType.VonNoymanPeriodic;
                                break;
                            case "RandomPentagonal":
                                neigbhourhoodType = neigbhourhoodType.PentagonalRandomPeriodic;
                                break;
                            case "RandomHexagonal":
                                neigbhourhoodType = neigbhourhoodType.HexagonalRandomPeriodic;
                                break;
                        }
                        break;
                }

                button6.Text = "Stop";
                simulationDisplayer.Show();



                worker = new BackgroundWorker();
                worker.DoWork += Worker_DoWork;
                worker.WorkerSupportsCancellation = true;
                worker.RunWorkerAsync();
            }
            

        }

        private void Form2_Disposed(object sender, EventArgs e)
        {
            seedgtowth?.stop();
            monte?.stop();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {                         
            seedgtowth.Iterate(refreshView2, neigbhourhoodType );            
        }

        private void Worker_DoWork2(object sender, DoWorkEventArgs e)
        {
            monte.Iterate(refreshView3, neigbhourhoodType.Moore);
        }


        private void refreshView3()
        {
            Bitmap bitmap = new Bitmap(width, height);

            // wersja rownoległa
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
            bitmap.UnlockBits(bitmapData);
            IntPtr ptr = bitmapData.Scan0;
            Parallel.For(0, monte.Ibound, index =>
            {

                Color color;

                for (int j = 0; j <= monte.Jbound; j++)
                {

                    int offset = index * 4 + j * bitmapData.Stride;
                    color = Color.FromArgb(monte.Cells[index, j]);
                    Marshal.WriteByte(ptr, offset, color.R);
                    Marshal.WriteByte(ptr, offset + 1, color.G);
                    Marshal.WriteByte(ptr, offset + 2, color.B);
                    Marshal.WriteByte(ptr, offset + 3, color.A);


                }
            });
            simulationDisplayer.pictureBox.Image = bitmap;
        }
        private void refreshView2()
        {
            Bitmap bitmap = new Bitmap(width, height);

            // wersja rownoległa
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
            bitmap.UnlockBits(bitmapData);
            IntPtr ptr = bitmapData.Scan0;
            Parallel.For(0, seedgtowth.Ibound, index =>
            {

                Color color;

                for (int j = 0; j <= seedgtowth.Jbound; j++)
                {

                    int offset = index * 4 + j * bitmapData.Stride;
                    color = Color.FromArgb(seedgtowth.Cells[index, j]);
                    Marshal.WriteByte(ptr, offset, color.R);
                    Marshal.WriteByte(ptr, offset + 1, color.G);
                    Marshal.WriteByte(ptr, offset + 2, color.B);
                    Marshal.WriteByte(ptr, offset + 3, color.A);


                }
            });
            simulationDisplayer.pictureBox.Image = bitmap;

            if (!areFreeCells())
                seedgtowth.stop();
        }

        private bool areFreeCells()
        {
            foreach (int c in seedgtowth.Cells)
                if (c == Color.Black.ToArgb())
                    return true;
            return false;
        }
          
        private void button6_Click(object sender, EventArgs e)
        {
            switch(button6.Text)
            {
                case "Stop":
                    Automata?.stop();
                    button3.Enabled = true;
                    button6.Text = Automata==null ? "Stop":"Run";
                    break;
                case "Run":
                    worker.RunWorkerAsync();
                    button3.Enabled = false;
                    button6.Text = Automata == null ? "Run" : "Stop";
                    break;
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(!worker.IsBusy)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.AddExtension = true;
                saveFileDialog.DefaultExt = ".bmp";
                
                if (saveFileDialog.ShowDialog()== DialogResult.OK)
                {
                    
                    simulationDisplayer?.pictureBox.Image.Save(saveFileDialog.FileName, ImageFormat.Bmp);
                    
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int seeds = Convert.ToInt32(textBox3.Text == "" ? "3" : textBox3.Text);
            seedgtowth.setSeedsRandomly(seeds);
            refreshView2();
        }

   

        private void textBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            
            width = Convert.ToInt32(textBox1.Text == "" ? "300" : textBox1.Text);
            height = Convert.ToInt32(textBox2.Text == "" ? "300" : textBox2.Text);
            int seeds = Convert.ToInt32(textBox3.Text == "" ? "3" : textBox3.Text);

            monte = new MonteCarlo(width, height, seeds);


            simulationDisplayer = new Form2();
            simulationDisplayer.Size = new Size(width + 20, height + 45);
            simulationDisplayer.pictureBox.Size = new Size(width, height);
            simulationDisplayer.Disposed += Form2_Disposed;
            //simulationDisplayer.pictureBox.MouseClick += PictureBox1_MouseClick1;
            simulationDisplayer.Show();



            worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork2;
            worker.WorkerSupportsCancellation = true;
            worker.RunWorkerAsync();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
    
}
