using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using OpenTK.Graphics.OpenGL;
using OpenTK.Platform.Windows;
using OpenTK.Math;
using OpenTK.Input;

using Tao.OpenGl;
using Tao.FreeGlut;
using Tao.Platform.Windows;

namespace Galactic1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        Bitmap bp;
        double m_X, m_Y;
        Graphics g;
        private void Form2_Load(object sender, EventArgs e)
        {
            g = panel2.CreateGraphics();
            timer1.Interval = 1;
            timer1.Enabled = true;
            m_X = 0;
            m_Y = 0;

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            panel1.Enabled = !checkBox1.Checked;
        }

        bool set_data=false;
        double []data;
        double angle;
        private void button6_Click(object sender, EventArgs e)
        {//створити
            //передання данних між формами
            timer1.Enabled = false;
            set_data = true;
            data = new double[4];
            Random rnd = new Random();
            if (!checkBox1.Checked)
            {
                data[0] = trackBar1.Value;//0.Маса 
                data[1] = trackBar2.Value;//1.Заряд
                data[2] = trackBar3.Value;//2.Поч. швидкість
                data[3] = Convert.ToDouble(angle);//3.Кут нахилу об'єкту
            }
            else
            {
                data[0] = rnd.Next(0,11);//0.Маса 
                data[1] = rnd.Next(-10, 11);//1.Заряд
                data[2] = rnd.Next(0, 11);//2.Поч. швидкість
                data[3] = Convert.ToDouble(rnd.Next(0, 6) * rnd.NextDouble());//3.кут нахилу 
            }
            this.Close();
        }
        public bool accept
        {//передавати, якщо true
            get
            {
                return set_data;
            }
        }
        public double[] Data
        {
            get
            {
                return data;
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            this.Close();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (mdown)
            {
                m_X = Cursor.Position.X - panel2.Location.X - this.Location.X - 10 - panel2.Width / 2;
                m_Y = -(Cursor.Position.Y - panel1.Location.Y - 32 - panel2.Location.Y - this.Location.Y - panel2.Width / 2);
            }
            if (panel1.Enabled == false)
            {
                panel2.BackColor = Color.Gray;
            }
            else
            {
                //координатні вісі
                panel2.BackColor = Color.White;
                if (mdown)
                    g.Clear(Color.White);
                g.DrawLine(Pens.LightBlue, panel2.Width / 2, 0, panel2.Width / 2, panel2.Height);//y
                g.DrawLine(Pens.LightPink, 0, panel2.Height / 2, panel2.Width, panel2.Height / 2);//x
                g.DrawEllipse(Pens.Black,0,0,panel2.Width,panel2.Height);
                angle = Math.Asin(m_Y / (0.0001+Math.Sqrt(m_X * m_X + m_Y * m_Y)));
                if(m_X<0)
                {//2 чверть
                    angle = Math.PI-angle;
                }
                else if(m_Y<0)
                {//4 чверть
                    angle = 2 * Math.PI + angle;
                }
                //головна лінія:
                g.DrawLine(Pens.Black, panel2.Width / 2, panel2.Height / 2, Convert.ToInt32(Math.Cos(angle) * (panel2.Width / 2))+ panel2.Width / 2, Convert.ToInt32(Math.Sin(angle)  * (-panel2.Width / 2))+ panel2.Width / 2);
                //прямі-проекції з кінця головної лінії до вісей координат:
                g.DrawLine(Pens.Blue, Convert.ToInt32(Math.Cos(angle) * (panel2.Width / 2)) + panel2.Width / 2, Convert.ToInt32(Math.Sin(angle) * (-panel2.Width / 2)) + panel2.Width / 2, Convert.ToInt32(Math.Cos(angle) * (panel2.Width / 2)) + panel2.Width / 2, panel2.Width / 2);
                g.DrawLine(Pens.Red, Convert.ToInt32(Math.Cos(angle) * (panel2.Width / 2)) + panel2.Width / 2, Convert.ToInt32(Math.Sin(angle) * (-panel2.Width / 2)) + panel2.Width / 2, panel2.Width / 2, Convert.ToInt32(Math.Sin(angle) * (-panel2.Width / 2)) + panel2.Width / 2);
                //яскраві позначення проекцій на вісях:
                g.DrawLine(Pens.Red,panel2.Width/2,panel2.Width/2, Convert.ToInt32(Math.Cos(angle) * (panel2.Width / 2)) + panel2.Width / 2, panel2.Width / 2);
                g.DrawLine(Pens.Blue, panel2.Width / 2, panel2.Width / 2, panel2.Width / 2, Convert.ToInt32(Math.Sin(angle) * (-panel2.Width / 2)) + panel2.Width / 2);
                label14.Text = " = "+Math.Round(angle*180/Math.PI,3)+ "º (" + Math.Round(angle/Math.PI,2)+ "π)";
            }
        }
        
        bool mdown=false;
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label2.Text = trackBar1.Value.ToString() + " (од. маси)";
        }
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            label3.Text = trackBar2.Value.ToString() + " (од. заряду)";
        }
        private void panel2_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            mdown = false;
        }
        private void panel2_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            mdown = true;
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            label5.Text = trackBar3.Value.ToString() + " (од. швидкості)";
        }
    }
}
