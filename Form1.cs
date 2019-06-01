using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace honeyNoSee
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public struct Peka
        {
            public string type;
            public int number;
            public float xPos;
            public float yPos;
            public float Angle;
            public int Velosity;
            public Peka(
                string typeN,
                    int numberN,
                    float xPosN,
                    float yPosN,
                    float AngleN,
                int Velo)
            {
                type = typeN;
                number = numberN;
                xPos = xPosN;
                yPos = yPosN;
                Angle = AngleN;
                Velosity = Velo;
            }
        };
      public List<Peka> pekas = new List<Peka>();
        Graphics graph;
        Color bg = Color.Yellow;
        public bool ContainsPicture(int x1,int y1,Bitmap img)
        {
          //  this.BackColor = img.GetPixel(x1, y1);
            return (img.GetPixel(x1,y1).A!=0);
        }
        Bitmap im;
        private void Form1_Load(object sender, EventArgs e)
        {
            im = new Bitmap(500, 800);
            graph = this.CreateGraphics();
        }

        private void RemoveInterface()
        {
            foreach(Control c in this.Controls)
            {
                c.Visible = false;
            }
        }

        private void btn_play_Click(object sender, EventArgs e)
        {
            timer.Interval = 10000/ int.Parse(txt_speed.Text);
            timerCreator.Interval =  timer.Interval*10;
            RemoveInterface();
            playing = true;
         //   timer.Start();
            timerCreator.Start();
            //this.Invalidate();
        }

        private void Form1_Click(object sender, EventArgs e)
        {
        }
        void PekasClearExept(int e)
        {
            for (int i = 0; i < pekas.Count; i++)
            {
                if (i != e)
                    pekas.RemoveAt(i);
            }
        }
        bool playing = false;

        private void timer_Tick(object sender, EventArgs e)
        {
            if (playing)
            {
                for (int i = 0; i < pekas.Count; i++)
                {
                    pekas[i] = new Peka(pekas[i].type, pekas[i].number, pekas[i].xPos, pekas[i].yPos + pekas[i].Velosity, pekas[i].Angle,pekas[i].Velosity);
                    if(pekas[i].yPos>=688)
                    {
                        if(pekas[i].type!="gnumme")
                        {
                            // timer.Stop();
                            RectangleF rect = new RectangleF(pekas[i].xPos, pekas[i].yPos, 112.0f, 112.0f);
                              graph.DrawRectangles(new Pen(Color.Red,10),new RectangleF[]{rect});
                              PekasClearExept(i);
                            timerCreator.Stop();
                            playing = false;
                            MessageBox.Show("САСАМБА");
                            pekas.Clear();
                            graph.Clear(Color.Yellow);
                            label1.Visible = true;
                            txt_speed.Visible = true;
                            btn_play.Visible = true;
                        }
                        else if(pekas[i].yPos>=800)
                        {
                            pekas.RemoveAt(i);
                        }
                        if(pekas[i].yPos<=-120)
                        {
                            pekas.RemoveAt(i);
                        }
                    }
                }
                this.Invalidate();
            }
            else
                graph.Clear(Color.Yellow);
        }
        Random Rnd = new Random();
        private void timerCreator_Tick(object sender, EventArgs e)
        {
            switch(Rnd.Next(2))
            { 
                case 0:
            pekas.Add(new Peka("gnumme",Rnd.Next(1,11),Rnd.Next(0,400),0,Rnd.Next(15),5));
            break;
                case 1:
            pekas.Add(new Peka("happa",Rnd.Next(1,11),Rnd.Next(0,400),0,Rnd.Next(15),5));
            break;
        }}

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (playing)
            {
                //  graph.Clear(Color.Yellow);
                foreach (Peka p in pekas)
                {
                    graph.DrawImage((Image)Properties.Resources.ResourceManager.GetObject(p.type + p.number.ToString()), new RectangleF(p.xPos, p.yPos, 112.0f, 112.0f));
                }
            }
            else
            {
                graph.Clear(Color.Yellow);
            }
        }
        string aliveType = "gnumme";
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            
            int xx=e.X;
            int yy = e.Y;
            for (int i = 0; i < pekas.Count; i++)
            {

                RectangleF rect = new RectangleF(pekas[i].xPos, pekas[i].yPos, 112.0f, 112.0f);
             //   graph.DrawRectangles(new Pen(Color.Red),new RectangleF[]{rect});
             if(rect.IntersectsWith(new RectangleF(xx, yy, 1, 1)))
             {
                 if (ContainsPicture(xx - (int)(pekas[i].xPos), yy - (int)(pekas[i].yPos), (Bitmap)Properties.Resources.ResourceManager.GetObject(pekas[i].type + pekas[i].number.ToString())))
                 {
                     if (aliveType != pekas[i].type)
                     {
                         pekas[i] = new Peka(pekas[i].type, pekas[i].number, pekas[i].xPos, pekas[i].yPos, pekas[i].Angle, -20);
                     }
                     else
                     {
                         pekas.Clear();
                        // timer.Stop();
                         timerCreator.Stop();
                         playing = false;
                         graph.Clear(Color.Yellow);
                         label1.Visible = true;
                         btn_play.Visible = true;
                         txt_speed.Visible = true;
                         MessageBox.Show("САСАМБА");
                     }
                 }
             }
         }
        }

    }
}
