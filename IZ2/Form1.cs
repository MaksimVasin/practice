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

namespace IZ2
{
    public enum position // позиция окружности
    {
        left, // слева
        right, // справа
        up, // свреху
        down, // снизу 
        middle // центральная
    }
    public partial class Form1 : Form
    {
        static int maxLevel = 5; // максимальный уровень фрактала
        static int diameter = 128; // диаметр основной окружности
        Tree tree; // дерево
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e) // старт
        {
            int Num;
            if (int.TryParse(textBox1.Text, out Num)) // проверка ввода диаметра
                diameter = Num;
            if (comboBox1.SelectedIndex != -1) // проверка выбран ли уровень
                maxLevel = Convert.ToInt32(comboBox1.SelectedItem);
            int x = (int)(pictureBox1.Width - diameter) / 2; // находим центр по Х
            int y = (int)(pictureBox1.Height - diameter) / 2; // находим центр по У
            tree = new Tree(maxLevel, diameter, x, y);  // создание дерева
            Paint(); // рисовка
        }
        private void Paint()
        {
            Graphics graphics1 = pictureBox1.CreateGraphics(); // рисунок фрактала
            Graphics graphics2 = pictureBox2.CreateGraphics(); // рисунок дерева
            tree.Paint(graphics1); // рисовка фрактала
            tree.PaintTree(graphics2); // рисовка дерева
        }
    }
    public class Node // Узел дерева
    {
        public Node(int diameter, int x, int y, int lvl)
        {
            this.diameter = diameter; // диаметр
            this.x = x; // координата х
            this.y = y; // координата у
            this.lvl = lvl; // текущий уровень
        }
        public int diameter;
        public int x, y;
        public int lvl;
        public Node childLeft; // потомок 1
        public Node childRight; // потомок 2
        public Node childUp; // потомок 3
        public Node childDown; // потомок 4
    }
    public class Tree
    {
        int maxLevel; // максимальный уровень дерева
        public Tree(int maxLevel, int diameter, int x, int y)
        {
            this.head = new Node(diameter, x, y, 1); // первый уровень
            this.maxLevel = maxLevel;
            OneNode(diameter, 1, head, position.middle); // вызов рекурсии
        }
        Node head; // корень дерева
        public void OneNode(int curdiameter, int curLevel, Node curNode, position pos) // созданик потомков
        {
            int childdiameter = (int)curdiameter / 2; // диаметр потомка
            if (curLevel == maxLevel) // если уровень максимальный потомки нулевые
            {
                curNode.childLeft = null;
                curNode.childRight = null;
                curNode.childUp = null;
                curNode.childDown = null;
            }
            else // иначе создаем
            {
                if (pos == position.left) curNode.childRight = null;
                else
                {
                    curNode.childRight = new Node(childdiameter, curNode.x + curdiameter, curNode.y + (int)(childdiameter / 2), curLevel + 1);
                    OneNode(childdiameter, curLevel+1, curNode.childRight, position.right); // вызов рекурсии
                }
                if (pos == position.right) curNode.childLeft = null;
                else
                {
                    curNode.childLeft = new Node(childdiameter, curNode.x - childdiameter, curNode.y + (int)(childdiameter / 2), curLevel + 1);
                    OneNode(childdiameter, curLevel+1, curNode.childLeft, position.left); // вызов рекурсии
                }
                if (pos == position.up) curNode.childDown = null;
                else
                {
                    curNode.childDown = new Node(childdiameter, curNode.x + (int)(childdiameter / 2), curNode.y + curdiameter, curLevel + 1);
                    OneNode(childdiameter, curLevel+1, curNode.childDown, position.down); // вызов рекурсии
                }
                if (pos == position.down) curNode.childUp = null;
                else
                {
                    curNode.childUp = new Node(childdiameter, curNode.x + (int)(childdiameter / 2), curNode.y - childdiameter, curLevel + 1);
                    OneNode(childdiameter, curLevel+1, curNode.childUp, position.up); // вызов рекурсии
                }
            }
        }
        public void FillEllipseColor(Graphics graphics, int x, int y, int diameter) // рисовка разноцветной окружности
        {
            int green = 0;
            int size = (int)(diameter / 2);
            if (size == 0) size = 1;
            int changeColor = (int)(255 / size);
            int changediameter = (int)diameter / size;
            int newdiameter;
            Pen pen = new Pen(Color.FromArgb(255, 255, green, 0));
            SolidBrush solidBrush = new SolidBrush(Color.FromArgb(255, 255, green, 0));
            graphics.FillEllipse(solidBrush, x, y, diameter, diameter);
            graphics.DrawEllipse(pen, x, y, diameter, diameter);
            for (int i = 0; i < size; i++)
            {
                green += changeColor;
                solidBrush.Color = Color.FromArgb(255, 255, green, 0);
                newdiameter = (int)(diameter - changediameter);
                x += (int)(diameter - newdiameter) / 2;
                y += (int)(diameter - newdiameter) / 2;
                graphics.FillEllipse(solidBrush, x, y, newdiameter, newdiameter);
                diameter = newdiameter;
            }
        }
        public void Paint(Graphics graphics) // рисовка фрактала
        {
            graphics.Clear(Color.White);
            FillEllipseColor(graphics, head.x, head.y, head.diameter);
            OneNodePaint(graphics, head);
        }
        public void OneNodePaint(Graphics graphics, Node cur) // рисовка одного узла
        {
            Pen pen = new Pen(Color.Red);
            if (cur.childUp != null)
            {
                FillEllipseColor(graphics, cur.childUp.x, cur.childUp.y, cur.childUp.diameter);
                OneNodePaint(graphics, cur.childUp);
            }
            if (cur.childLeft != null)
            {
                FillEllipseColor(graphics, cur.childLeft.x, cur.childLeft.y, cur.childLeft.diameter);
                OneNodePaint(graphics, cur.childLeft);
            }
            if (cur.childDown != null)
            {
                FillEllipseColor(graphics, cur.childDown.x, cur.childDown.y, cur.childDown.diameter);
                OneNodePaint(graphics, cur.childDown);
            }
            if (cur.childRight != null)
            {
                FillEllipseColor(graphics, cur.childRight.x, cur.childRight.y, cur.childRight.diameter);
                OneNodePaint(graphics, cur.childRight);
            }
        }
        public void PaintTree(Graphics graphics) // рисовка дерева
        {
            graphics.Clear(Color.White);
            int sizeLine = (int)(400 / maxLevel);
            PaintOneLine(graphics, head, 220, 30, 150, sizeLine);
        }
        public void PaintOneLine(Graphics graphics, Node cur, int x, int y, int sizeShift, int sizeLine) // рисовка линий дерева
        {
            Pen pen = new Pen(color_line(cur.lvl), 2);
            Brush brush = new SolidBrush(color_line(cur.lvl));
            if (cur.childLeft != null)
            {
                graphics.DrawLine(pen, new Point(x, y), new Point(x - (int)(sizeShift / 3), y + sizeLine));
                PaintOneLine(graphics, cur.childLeft, x - (int)(sizeShift / 3), y + sizeLine, (int)(sizeShift / 4), sizeLine);
            }
            if (cur.childUp != null)
            {
                graphics.DrawLine(pen, new Point(x, y), new Point(x - sizeShift, y + sizeLine));
                PaintOneLine(graphics, cur.childUp, x - sizeShift, y + sizeLine, (int)(sizeShift / 4), sizeLine);
            }
            if (cur.childRight != null)
            {
                graphics.DrawLine(pen, new Point(x, y), new Point(x + sizeShift, y + sizeLine));
                PaintOneLine(graphics, cur.childRight, x + sizeShift, y + sizeLine, (int)(sizeShift / 4), sizeLine);
            }
            if (cur.childDown != null)
            {
                graphics.DrawLine(pen, new Point(x, y), new Point(x + (int)(sizeShift / 3), y + sizeLine));
                PaintOneLine(graphics, cur.childDown, x + (int)(sizeShift / 3), y + sizeLine, (int)(sizeShift / 4), sizeLine);
            }
            graphics.FillEllipse(brush, x - 3, y - 3, 6, 6);
            graphics.FillEllipse(brush, x - 3, y - 3, 6, 6);
        }
        public Color color_line(int lvl) // выбор цвета в зависимости от уровня
        {
            lvl %= 5;
            switch (lvl)
            {
                case 2:
                    return Color.DeepSkyBlue;
                case 3:
                    return Color.DodgerBlue;
                case 4:
                    return Color.MediumBlue;
                case 0:
                    return Color.DarkBlue;
                case 1:
                    return Color.SkyBlue;
            }
            return Color.Black;
        }
    }
}