using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RunningButton
{
    public partial class Form1 : Form
    {
        private Size lastFieldSize;
        private Point btnCntrLoc;
        private Point prevMouseLoc;
        private Point prevTopBordLoc;
        private Point prevBottomBordLoc;
        private Point prevLeftBordLoc;
        private Point prevRightBordLoc;
        private const float minDistPx = 100f;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Place button at center of window
            Size content_size = pnlContent.Size;
           /* button1.Location = new Point
            {
                X = 305,
                Y = 200
            };*/
            //    (
            //    content_size.Width - button1.Size.Width >> 2,
            //    content_size.Height - button1.Size.Height >> 2);
            this.lastFieldSize = content_size;
            // textBox1.Text = textBox1.Text+"Form1_Load!";
            btnCntrLoc = new Point
            {
                X = (int)(button1.Location.X + button1.Width / 2f),
                Y = (int)(button1.Location.Y + button1.Height / 2f)
            };
            prevTopBordLoc = new Point(btnCntrLoc.X, 0);
            prevTopBordLoc = new Point(btnCntrLoc.X, 0);
            prevBottomBordLoc = new Point(btnCntrLoc.X, pnlContent.Size.Height);
            prevLeftBordLoc = new Point(0, btnCntrLoc.Y);
            prevRightBordLoc = new Point(pnlContent.Size.Width, btnCntrLoc.Y);

        }

        private void OnFormResize(object sender, EventArgs e)
        {
            PlaceWithoutClipping(button1);
        }

        private void PlaceWithoutClipping(Control control)
        {
            if (WindowState != FormWindowState.Minimized)
            {

                control.Location = new Point
                {
                    X = Math.Min(Math.Max(0, control.Location.X), control.Parent.Size.Width - control.Size.Width),
                    Y = Math.Min(Math.Max(0, control.Location.Y), control.Parent.Size.Height - control.Size.Height),
                };
            }
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            MessageBox.Show(this, "Поздравляем! Вы смогли нажать кнопку!", "You are winner", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void OnMouseEnter(object sender, EventArgs e)
        {
            prevMouseLoc = MousePosition;
        }

        private void OnMouseOnForm(object sender, MouseEventArgs e)
        {
            Point mouseLoc = MousePosition;
            if (mouseLoc == prevMouseLoc) return;
            btnCntrLoc = new Point
            {
                X = (int)(button1.Location.X + button1.Width / 2f),
                Y = (int)(button1.Location.Y + button1.Height / 2f)
            };
            Point topBorderLoc = new Point(btnCntrLoc.X, 0);
            Point bottomBorderLoc = new Point(btnCntrLoc.X, pnlContent.Size.Height);
            Point leftBorderLoc = new Point(0, btnCntrLoc.Y);
            Point rightBorderLoc = new Point(pnlContent.Size.Width, btnCntrLoc.Y);
            

            Point btnLoc = button1.Location;
            
            btnLoc.Offset(ShiftCalculate(ConvertToFormCoordinates(mouseLoc), ConvertToFormCoordinates(prevMouseLoc), true));
            btnLoc.Offset(ShiftCalculate(topBorderLoc, prevTopBordLoc, false));
            btnLoc.Offset(ShiftCalculate(bottomBorderLoc, prevBottomBordLoc, false));
            btnLoc.Offset(ShiftCalculate(leftBorderLoc, prevLeftBordLoc, false));
            btnLoc.Offset(ShiftCalculate(rightBorderLoc, prevRightBordLoc, false));


            button1.Location = btnLoc;
            PlaceWithoutClipping(button1);

            prevMouseLoc = MousePosition;
            prevTopBordLoc = topBorderLoc;
            prevBottomBordLoc = bottomBorderLoc;
            prevLeftBordLoc = leftBorderLoc;
            prevRightBordLoc = rightBorderLoc;
        }

        private Point ConvertToFormCoordinates(Point p)
        {
            p.Offset(
                -this.Location.X - pnlContent.Location.X,
                -this.Location.Y - pnlContent.Location.Y
            );
            return p;
        }
        private  Point Minus(Point left, Point right)
        {
            return new Point(left.X - right.X, left.Y - right.Y);
        }
        private Point ShiftCalculate(Point chaser, Point prev_chaser, bool is_mouse)
        {
            Point chMovingDelt = Minus(chaser, prev_chaser);
            Point butDelta = Minus(btnCntrLoc, chaser);
            float distance = minDistPx / (float)Length(butDelta);

            Point newLoc = Point.Empty;
            if (distance > 1)
            {
                if (is_mouse)
                {
                    newLoc.Offset(
                       (Math.Sign(butDelta.X) == Math.Sign(chMovingDelt.X)) ?
                        (int)(chMovingDelt.X * distance) : 0,
                       (Math.Sign(butDelta.Y) == Math.Sign(chMovingDelt.Y)) ?
                        (int)(chMovingDelt.Y * distance) : 0
                        );
                }
                else
                {
                    int offset = (Math.Abs(chMovingDelt.X) + Math.Abs(chMovingDelt.Y));
                    if (chMovingDelt.X == 0)
                    {
                        newLoc.Offset(
                            (int)(Math.Sign(butDelta.X) * offset * distance),
                            (int)(Math.Sign(chMovingDelt.Y) * distance)
                            );
                    }
                    else
                    {
                        newLoc.Offset(
                            (int)(Math.Sign(chMovingDelt.X) * distance),
                            (int)(Math.Sign(butDelta.Y) * offset * distance)
                            );
                    }
                }
            }

            return newLoc;
        }

        private double Length(Point p)
        {
            return Math.Sqrt(p.X*p.X + p.Y * p.Y);
        }


    }
}
