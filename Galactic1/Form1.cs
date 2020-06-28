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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //const double k = 8.9E9;//899180469.45           - електростатическая постоянная
        //const double G = 6.67e-11;//0.000000000066743   - гравитационная постоянная
        Planet []pl;
        bool ms_down,ms_up;
        int index = 0;
        private void Form1_Load(object sender, EventArgs e)
        {
            panel2.Location = new Point(this.Width - 255, 0);
            panel2.Height = 37;
            panel2.Width = 200;
            button6.Select();
            //малюємо нульову точку та запускаємо таймер
            GL.Clear(ClearBufferMask.None);
            GL.ClearColor(Color.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Flush();
            glControl1.SwapBuffers();
            pl = new Planet[1000];
            for(int i=0;i<1000;i++)
            {
                pl[i] = new Planet();
            }
            //GL.Viewport(0, 0, glControl1.Width, glControl1.Width);
            timer1.Interval = 100;
            timer1.Enabled = false;
            index = 0;
            label1.Visible = true;
            //timer2.Interval = 1;
            //timer2.Enabled = true;
        }
        
        private void button6_Click(object sender, EventArgs e)
        {
            if(panel2.Height<50)
            {//відкриваємо список
                button6.Text = "Список (приховати)";
                panel2.Location = new Point(this.Width - 330, 0);
                panel2.Height = 200;
                panel2.Width = 326;
            }
            else
            {//приховуємо список
                button6.Text = "Список (показати)";
                panel2.Location = new Point(this.Width - 255, 0);
                panel2.Height = 37;
                panel2.Width = 200;
            }
        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {
            //toolTip1.SetToolTip(button2, "Створити об'єкт");
        }
        //double R=0.05;
        private void timer1_Tick(object sender, EventArgs e)
        {
            //действие над объектом
            label1.Text = "";
            for(int i=0;i<Planet.num;i++)
            {
                pl[i].Next_action(dist(pl[i], pl[i + 1]), pl, k,label1,i);
            }
            //отрисовка объектов
            GL.ClearColor(Color.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Viewport(130, 0, glControl1.Height, glControl1.Height);
            for (int i = 0; i < Planet.num; i++)
            {
                GL.Begin(PrimitiveType.Polygon);
                draw_object(Color.White, 0.01, pl[i].x, pl[i].y);
                GL.End();
            }
            GL.Flush();
            glControl1.SwapBuffers();
            
        }

        /*void f()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadIdentity();
            GL.Color3(1.0f, 0, 0);
            GL.PushMatrix();
            GL.Translate(0, 0, -6);
            GL.Rotate(45, 1, 1, 0);
            //Glut.glutWireSphere(2, 32, 32);
            GL.PopMatrix();
            GL.Flush();
            glControl1.Invalidate();
        }

        void f()
        {
            GL.ClearColor(Color.SkyBlue);
            GL.Enable(EnableCap.DepthTest);

            Matrix4 p = Matrix4.CreatePerspectiveFieldOfView((float)(80 * Math.PI / 180), 1, 20, 500);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref (double)p);

            Matrix4 modelview = Matrix4.LookAt(70, 70, 70, 0, 0, 0, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);
        }*/

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            //призупиняємо процес

        }
        private void contextMenuStrip1_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            //продовжуємо процес (якщо був ввімкнутий)

        }
        private void розпочатиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Розпочати/Призупинити
            GL.Clear(ClearBufferMask.None);
            GL.ClearColor(Color.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.PopMatrix();
            GL.Flush();
            glControl1.SwapBuffers();
            timer1.Interval = 100;
            if (розпочатиToolStripMenuItem.Text == "Розпочати")
            {
                timer1.Enabled = true;
                розпочатиToolStripMenuItem.Text = "Призупинити";
            }
            else
            {
                timer1.Enabled = false;
                розпочатиToolStripMenuItem.Text = "Розпочати";
            }
        }
        private void налаштуванняToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Налаштування

        }

        private void скинутиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Скинути (увесь процес зберігається та безвозворотньо припиняється)
            index = 0;
            Planet.num = 0;
            listView1.Items.Clear();
        }

        class Planet
        {
            public static int num;
            public double x,y/*,z,s*/;//координати та спін
            public double m,q,v_x,v_y,v/*,B,V*/;//маса, заряд, швидкість, магнітна індукція, об'єм
            //public bool l;//чи світиться
            public double F, F_x, F_y/*, F_z,R, F_Grav,F_magn,F_electric,F_Amper,F_Lorens*/;
            public double alpha;
            public int id;
            /*public Vector3 a;*/
            //конструктори:
            public Planet()
            {

            }
            public Planet(int name,double x_coord, double y_coord,double z_coord, double weight, double electric_charge, double speed,double angle)
            {
                id = name;
                x = x_coord;
                y = y_coord;
                m = weight;
                q = electric_charge;
                v=speed;
                alpha = angle;
                num++;
                v_x = v * Math.Cos(alpha);
                v_y = v * Math.Sin(alpha);
                /*
                a.X = 0;
                a.Y = 0;
                a.Z = 0;
                */
            }
            /*
            public Planet(double x_coord, double y_coord, double z_coord, int weight, int electric_charge, int speed,int magnetic_induction,int volume)
            {
                x = x_coord;
                y = y_coord;
                z = z_coord;
                m = weight;
                q = electric_charge;
                v = speed;
                s = 0;
                B = magnetic_induction;
                V = volume;
                l = false;
                //a.X = 0;
                //a.Y = 0;
                //a.Z = 0;
            }
            public Planet(double x_coord, double y_coord, double z_coord, int weight, int electric_charge, int speed, int magnetic_induction, int volume,double spin)
            {
                x = x_coord;
                y = y_coord;
                z = z_coord;
                m = weight;
                q = electric_charge;
                v = speed;
                s = spin;
                B = magnetic_induction;
                V = volume;
                l = false;
                //a.X = 0;
                //a.Y = 0;
                //a.Z = 0;
            }
            public Planet(double x_coord, double y_coord, double z_coord, int weight, int electric_charge, int speed, int magnetic_induction, int volume, double spin,bool is_shining)
            {
                x = x_coord;
                y = y_coord;
                z = z_coord;
                m = weight;
                q = electric_charge;
                v = speed;
                s = spin;
                B = magnetic_induction;
                V = volume;
                l = is_shining;
                //a.X = 0;
                //a.Y = 0;
                //a.Z = 0;
            }
            */
            //підпрограми:
            public void Next_action(double distanse, Planet []pl,double koef,Label lb,int id)
            {//підраховується векторна сума всіх сил, що діють на тіло
                double F_x=0,F_y=0,delta=0;

                for (int i = 0; i < num; i++)//гравітація
                {
                    if (i != id)
                    {
                        F_x += Math.Cos(alpha) * pl[i].m / (distanse * distanse);//прискорення по х
                        F_y += Math.Sin(alpha) * pl[i].m / (distanse * distanse);//прискорення по у
                        delta += Math.Acos((F_x * distanse * distanse) / pl[i].m);
                    }
                }
                alpha += delta;
                while (alpha > 2 * Math.PI)
                    alpha -= 2 * Math.PI;
                /*for (int i = 0; i < num; i++)//ел. притяжіння
                {
                    F_x += pl[i].m / distanse * distanse;//прискорення по х
                    F_y += pl[i].m / distanse * distanse;//прискорення по у
                }*/

                //v = Math.Sqrt(F_x * F_x + F_y * F_y);
                x += v * Math.Cos(alpha) * koef;
                y += v * Math.Sin(alpha) * koef;
                //v_x += F_x;
                //v_y += F_y;
                //x += v_x*koef*00.1;
                //y += v_y*koef*00.1;
                lb.Text += "\npl" + id + ":\nx = "+x+"\ny = "+y+"\nv_x = " + Math.Round(v * Math.Cos(alpha) * koef, 2) + "; \nv_y = " + Math.Round(v * Math.Sin(alpha) * koef, 2) + "; \na =" + Math.Round(180 * alpha / Math.PI)+"; \nv = "+Math.Round(v,2);
            }
        }

        double k = 0.00194;
        private void timer2_Tick(object sender, EventArgs e)
        {
            //постановка объекта
            //label1.Text = "x= " + (Cursor.Position.X - glControl1.Width / 2) * k + "; y= " + -(Cursor.Position.Y - glControl1.Height / 2) * k;
            GL.ClearColor(Color.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Viewport(130, 0, glControl1.Height, glControl1.Height);
            for (int i = 0; i < Planet.num; i++)
            {
                GL.Begin(PrimitiveType.Polygon);
                draw_object(Color.FromArgb(50, 50, 50), 0.01, pl[i].x, pl[i].y);
                GL.End();
            }
            GL.Begin(PrimitiveType.Polygon);
            draw_object(Color.White, 0.01, (Cursor.Position.X - glControl1.Width / 2) * k, -(Cursor.Position.Y - glControl1.Height / 2) * k);
            GL.End();
            GL.Flush();
            glControl1.SwapBuffers();
            if (ms_down)
            {
                pl[index] = new Planet(index,(Cursor.Position.X - glControl1.Width / 2) * k, -(Cursor.Position.Y - glControl1.Height / 2) * k, 0, x[0], x[1], x[2], x[3]);
                timer2.Enabled = false;

                //timer1.Enabled = true;
                index++;
            }
        }

        bool CancelEdit = false;
        ListViewItem.ListViewSubItem CurrentSubItem = default(ListViewItem.ListViewSubItem);
        ListViewItem CurrentItem = default(ListViewItem);

        private void listView1_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // Get current item of ListView
            CurrentItem = listView1.GetItemAt(e.X, e.Y);
            if (CurrentItem == null)
                return;
            // Get sub item of current item
            CurrentSubItem = CurrentItem.GetSubItemAt(e.X, e.Y);
            int SubItembIndex = CurrentItem.SubItems.IndexOf(CurrentSubItem);

            // Check that we try edit column "Value"
            switch (SubItembIndex)
            {
                case 1:
                    break;
                case 2:
                    break;
                default:
                    return;
            }
            // Set params for TextBox, show it and set focus
            int lLeft = CurrentSubItem.Bounds.Left + 2;
            int lWidth = CurrentSubItem.Bounds.Width - 2;
            textBox1.SetBounds(lLeft + listView1.Left, CurrentSubItem.Bounds.Top + listView1.Top, lWidth, CurrentSubItem.Bounds.Height);
            textBox1.Text = CurrentSubItem.Text;
            textBox1.Show();
            textBox1.Focus();
            textBox1.Location = new Point(panel2.Location.X + e.X - textBox1.Width/2, panel2.Location.Y + e.Y  + textBox1.Height*3/2);
        }
        private void textBox1_Leave(object sender, EventArgs e)
        {
            textBox1.Hide();
            if (CancelEdit == false)
                CurrentSubItem.Text = textBox1.Text;
            else
                CancelEdit = false;
            listView1.Focus();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {//при натисканні Enter данні копіюються до ListView
                case (char)Keys.Return:
                    CancelEdit = false;
                    e.Handled = true;
                    textBox1.Hide();
                    break;
                //при натисканні Escape данні нікуди не передаються
                case (char)Keys.Escape:
                    CancelEdit = true;
                    e.Handled = true;
                    textBox1.Hide();
                    break;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {//обмеження на літери
            //string s = textBox1.Text;
            //string str = "";
            //foreach (char c in s)
            //    if (c >= '0' && c <= '9')
            //        str += c;
            //textBox1.Text = str;
        }

        public void Add_planet(int Name, double Weight, double El_charge,double Speed)
        {//Додавання нового об'єкта до гри
            ListViewItem lv = new ListViewItem(Name.ToString());
            lv.SubItems.Add(Weight.ToString());//Маса об'єкта
            lv.SubItems.Add(El_charge.ToString());//Заряд
            lv.SubItems.Add(Speed.ToString());//швидкість
            listView1.Items.Add(lv);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {//виділення поточного об'єкта
                //button1.Enabled = true;
                //label2.Text = listView1.SelectedItems[0].SubItems[0].Text;//индекс
                //textBox1.Text = listView1.SelectedItems[0].SubItems[1].Text;//длинна
                //numericUpDown1.Value = Convert.ToInt32(listView1.SelectedItems[0].SubItems[2].Text);//диапазон
                //textBox2.Text = listView1.SelectedItems[0].SubItems[3].Text;//скорость
            }
        }

        double[] x;
        private void створитиОбєктToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            Form2 f = new Form2();
            f.ShowDialog();
            x = f.Data;
            if (f.accept)
            {
                Add_planet(index, x[0], x[1], x[2]);
                //добавляем объект на общее поле
                timer2.Interval = 1;
                timer2.Enabled = true;
            }
        }

        private void GlControl1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            ms_down = true;
            //ms_up = false;
            bool to_be_or_not_to_be = false;
            for (int i = 0; i < Planet.num; i++)
                if ((Cursor.Position.X - glControl1.Width / 2) * k < pl[i].x + 0.01 && (Cursor.Position.X - glControl1.Width / 2) * k > pl[i].x - 0.01 && -(Cursor.Position.Y - glControl1.Height / 2) * k < pl[i].y + 0.01 && -(Cursor.Position.Y - glControl1.Height / 2) * k > pl[i].y - 0.01)
                    to_be_or_not_to_be = true;
            label1.Text = "pl[0].X =" + pl[0].x + "pl[0].Y =" + pl[0].y + "\nX = " + (Cursor.Position.X - glControl1.Width / 2) * k + "\nY = " + -(Cursor.Position.Y - glControl1.Height / 2) * k+"\nbl = "+to_be_or_not_to_be.ToString();
            if (to_be_or_not_to_be)
            {
                //вызов меню для редактирования планеты
                прибратиToolStripMenuItem.Visible = true;
                змінитиToolStripMenuItem.Visible = true;
            }
            else
            {
                прибратиToolStripMenuItem.Visible = false;
                змінитиToolStripMenuItem.Visible = false;
            }
        }

        private void GlControl1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            ms_down = false;
            //ms_up = true;
            
        }
        void draw_object(Color col,double r,double X,Double Y)
        {
            GL.Color3(col);
            for (int j = 0; j < 360; j++)
            {
                GL.Vertex2(X + r * Math.Cos(Math.PI * j / 180), Y + r * Math.Sin(Math.PI * j / 180));
            }
        }

        private void GlControl1_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if(e.Y>200/*&&e.Y-menuStrip1.Location.Y+menuStrip1.Width>100*/)
            {
                menuStrip1.Visible = false;
                panel2.Visible = false;
                GL.ClearColor(Color.Black);
                glControl1.SwapBuffers();
            }
            else
            {
                menuStrip1.Visible = true;
                panel2.Visible = true;
            }
        }

        private void ContextMenuStrip1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            
        }

        private void СтворитиToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void ЗупинитиToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void ПрибратиToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void ЗмінитиToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        double dist(Planet p1,Planet p2)
        {
            return Math.Sqrt((p1.x + p2.x) * (p1.x + p2.x) + (p1.y + p2.y) * (p1.y + p2.y));
        }
    }
}
