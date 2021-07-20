using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace DynGO
{
    public partial class Form1 : Form
    {
        double lengthTriangle = 1;
        int colorTriangle = 0;
        int widthTriangle = 0;
        int dushStyleTriangle = 0;
        int cycle = 0;
        int step = 1;
        int speed = 1;
        int widthTrajectory = 0;
        int colorTrajectory = 0;
        int dushStyleTrajectory = 0;
        int j = 0;
        int R = 3; // радиус окружности, которуб огибает траектория
        private void startPotok() // запуск параллельного потока
        {
            Thread thread = new System.Threading.Thread(Paint);
            thread.Start();
        }
        private void Paint() // рисовка окна настройки треуголника
        {
            {
                Graphics graphics = pictureBox2.CreateGraphics();
                while (button1.Enabled) // пока не старт
                {
                    graphics.Clear(System.Drawing.SystemColors.HighlightText); // очитска picturbox

                    string temp2 = lengthTriangle.ToString();
                    Invoke((Action)delegate { label3.Text = temp2; });

                    Paint_Triangle(135, 150, lengthTriangle, -1.57, colorTriangle, pictureBox2); // рисовка треугольника
                    Thread.Sleep(100);
                }
                PaintMain();
            }
        }
        private void PaintMain() // рисовка траектории и треуголника
        {
            double InitT = -10, LastT = 10; // оборот в 360 градусов (6,28 радиан)
            double Step = 0.1, angle = 0; // шаг и угол поворота треугольника
            double x = InitT, y;
            int cX = 310, cY = 335; // центр координтной оси
            int i = 0; // количество точек прорисовки
            int size_pT = (int)((LastT - InitT) / Step) + 1;
            Point[] pT = new Point[size_pT]; // точки для прорисовки (LastT - InitT/Step)

            while (x <= LastT)
            {
                if ((Math.Pow(R, 3) - Math.Pow(x, 3)) < 0)
                    y = -Math.Pow((Math.Pow(x, 3) - Math.Pow(R, 3)), 1.0 / 3.0);
                else
                    y = Math.Pow((Math.Pow(R, 3) - Math.Pow(x, 3)), 1.0 / 3.0);
                pT[i] = new System.Drawing.Point(cX + (int)(30 * x), cY - (int)(30 * y)); // расчет очередной точки траектории
                Paint_Traektory(pT);  // рисовка траектории
                x += Step;
                i++;
            }
            for (j = 0; button2.Enabled; j += step) // пока не нажата кнопка стоп
            {
                if (j >= size_pT - 11)
                {
                    j = 0;
                    cycle++;
                    string temp2 = cycle.ToString();
                    Invoke((Action)delegate { label14.Text = temp2; });
                }
                Paint_Traektory(pT); // рисовка траектории
                Paint_Circle(cX - 30 * R, cY - 30 * R, R * 30);
                angle = Get_Angle(pT[j], pT[j + 5]); // расчет угла
                Paint_axix(cX, cY); // рисовка осей ХУ
                Paint_Triangle(pT[j].X, pT[j].Y, lengthTriangle, angle, colorTriangle, pictureBox1); // рисовка треугольника
                int temp = (int)(j / 10);
                Invoke((Action)delegate { trackBar2.Value = temp; });
                Thread.Sleep(300 - 29 * speed);
            }
        }
        public double Get_Angle(Point a, Point b) // расчет угла поворота треугольника
        {
            Point a2 = new Point(0, 0);
            Point b2 = new Point(10, 0);
            Point A = new Point(b2.X - a2.X, b2.Y - a2.Y);
            Point B = new Point(b.X - a.X, b.Y - a.Y);
            double absA = Math.Sqrt(A.X * A.X + A.Y * A.Y);
            double absB = Math.Sqrt(B.X * B.X + B.Y * B.Y);
            double AB = A.X * B.X + A.Y * B.Y;
            double cos = AB / (absA * absB);
            return Math.Acos(cos);
        }
        public Form1()
        {
            InitializeComponent();
        }
        private void Paint_Traektory(Point[] p) // рисовка траектории
        {
            Graphics graphics = pictureBox1.CreateGraphics();
            graphics.Clear(System.Drawing.SystemColors.HighlightText);
            Pen pen = PenSetting(colorTrajectory, widthTrajectory, dushStyleTrajectory);
            graphics.DrawLines(pen, p);
        }
        private Pen PenSetting(int color, int width, int dash_style) // настройка линии (цвет, ширина)
        {
            Pen pen = new Pen(Brushes.Black);
            switch (color)
            {
                case 0: pen = new Pen(Brushes.Red); break;
                case 1: pen = new Pen(Brushes.Blue); break;
                case 2: pen = new Pen(Brushes.Green); break;
                case 3: pen = new Pen(Brushes.Pink); break;
                case 4: pen = new Pen(Brushes.Yellow); break;
                case 5: pen = new Pen(Brushes.Black); break;
            }
            switch (width)
            {
                case 0: pen.Width = 1.0F; break;
                case 1: pen.Width = 2.0F; break;
                case 2: pen.Width = 4.0F; break;
                case 3: pen.Width = 6.0F; break;
                case 4: pen.Width = 8.0F; break;
                case 5: pen.Width = 10.0F; break;
                case 6: pen.Width = 12.0F; break;
                case 7: pen.Width = 14.0F; break;
                case 8: pen.Width = 16.0F; break;
                case 9: pen.Width = 18.0F; break;
                case 10: pen.Width = 20.0F; break;
            }
            switch (dash_style)
            {
                case 0: pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid; break;
                case 1: pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash; break;
                case 2: pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot; break;
                case 3: pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot; break;
            }
            return pen;
        }
        private void Paint_Triangle(int x0, int y0, double a, double ang, int color, PictureBox pictureBox) // рисовка треугольника
        {
            Point A, B, C; // углы треугольника
            Graphics graphics = pictureBox.CreateGraphics();
            int R = (int)(30 * a * Math.Sqrt(3) / 3);
            A = new System.Drawing.Point(x0 + (int)(R * Math.Cos(2.0 * Math.PI + ang)), y0 + (int)(R * Math.Sin(2.0 * Math.PI + ang)));
            B = new System.Drawing.Point(x0 + (int)(R * Math.Cos(2.0 * Math.PI / 3.0 + ang)), y0 + (int)(R * Math.Sin(2.0 * Math.PI / 3.0 + ang)));
            C = new System.Drawing.Point(x0 + (int)(R * Math.Cos(4.0 * Math.PI / 3.0 + ang)), y0 + (int)(R * Math.Sin(4.0 * Math.PI / 3.0 + ang)));
            Pen pen = PenSetting(color, widthTriangle, dushStyleTriangle);
            graphics.DrawLine(pen, A, B);
            graphics.DrawLine(pen, B, C);
            graphics.DrawLine(pen, C, A);
        }
        private void Paint_Circle(int cX, int cY, int R)
        {
            Graphics graphics = pictureBox1.CreateGraphics();
            graphics.DrawEllipse(Pens.Black, cX, cY, 2 * R, 2 * R);
        }
        private void Paint_axix(int x0, int y0) // создание координтной оси
        {
            Graphics graphics = pictureBox1.CreateGraphics();
            graphics.DrawLine(Pens.Black, x0, 0, x0, 1000);
            graphics.DrawLine(Pens.Black, 0, y0, 1000, y0);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            startPotok();
        }
        private void button1_Click(object sender, EventArgs e) // кнопка старта
        {
            button1.Enabled = false;
            button2.Enabled = true;
            comboBox7.Enabled = false;
        }
        private void trackBar1_Scroll(object sender, EventArgs e) // длинна стороны треугольника
        {
            lengthTriangle = trackBar1.Value;
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) // выбор цвета линии треугольника
        {
            colorTriangle = comboBox1.SelectedIndex;
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) // ширина линии треугольника
        {
            widthTriangle = comboBox2.SelectedIndex;
        }
        private void trackBar2_Scroll(object sender, EventArgs e) // перемотка движения треугольника
        {
            j = trackBar2.Value * 10;
        }
        private void button2_Click(object sender, EventArgs e) // кнопка стоп
        {
            button2.Enabled = false;
            comboBox1.Enabled = false;
            comboBox2.Enabled = false;
            trackBar1.Enabled = false;
            trackBar2.Enabled = false;
            comboBox3.Enabled = false;
            comboBox4.Enabled = false;
            comboBox5.Enabled = false;
            comboBox6.Enabled = false;
            trackBar3.Enabled = false;
            trackBar4.Enabled = false;
        }
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            widthTrajectory = comboBox3.SelectedIndex;
        }
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            colorTrajectory = comboBox4.SelectedIndex;
        }
        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            dushStyleTrajectory = comboBox5.SelectedIndex;
        }
        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            dushStyleTriangle = comboBox6.SelectedIndex;
        }
        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            speed = trackBar3.Value;
        }
        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            step = trackBar4.Value;
        }
        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            R = comboBox7.SelectedIndex + 1;
        }
    }
}
