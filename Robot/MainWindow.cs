using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;

namespace Robot
{
    public partial class MainWindow : Form
    {
        private const int joystickR = 70; //poloměr kružnice joysticku
        private Point stickLocation = new Point(joystickR, joystickR); //pozice páčky joysticku
        private Action inicializeObserver; //callback pro dokončení view
        private Action<int, int> joystickObserver; //callback pro pohyb joysticku (int x, int y)

        public MainWindow()
        {
            InitializeComponent();
            createSoftwareJoystick();
        }
        
        /// <summary>
        /// Přiřazení posluchače pro dokončení a zobrazení view
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu</param>
        public void subscribeWindowShownObserver(Action observer)
        {
            inicializeObserver = observer;
        }

        /// <summary>
        /// Přiřazení posluchače pro pohyb joysticku
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu (int x, int y)</param>
        public void subscribeJoystickObserver(Action<int, int> observer)
        {
            joystickObserver = observer;
            panelForJoystick.MouseMove += new MouseEventHandler(panelForJoystick_MouseMove);
            panelForJoystick.MouseDown += new MouseEventHandler(panelForJoystick_MouseDown);
            panelForJoystick.MouseUp += new MouseEventHandler(panelForJoystick_MouseUp);
        }

        /// <summary>
        /// Zobrazí chybovou hlášku týkající se motorů
        /// </summary>
        /// <param name="message">text chyby</param>
        public void showMotorsError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            errorLabelMotors.Text = message;
        }

        /// <summary>
        /// Zobrazí chybovou hlášku týkající se ovládání
        /// </summary>
        /// <param name="message">text chyby</param>
        public void showControlError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            errorLabelControl.Text = message;
        }

        /// <summary>
        /// Zruší zobrazení chybové hlášky
        /// </summary>
        public void clearErrorText()
        {
            errorLabelMotors.Text = "";
        }

        /// <summary>
        /// Pohne se softwarovým joystickem na dané souřadnice
        /// </summary>
        /// <param name="x">x souřadnice od -100 do 100</param>
        /// <param name="y">y souřadnice od -100 do 100</param>
        public void moveJoystick(int x, int y)
        {
            stickLocation = new Point((int)Math.Floor((x + 100) * ((double)joystickR / 100)), (int)Math.Floor((y + 100) * ((double)joystickR / 100)));
            panelForJoystick.Invalidate();
        }

        /// <summary>
        /// Nastaví vzhled tlačítka pro pohyb vzhůru jako stiknuté/nestiknuté podle daného parametru
        /// </summary>
        /// <param name="pressed">stiknuté/nestiknuté</param>
        public void buttonMoveUpPressed(bool pressed)
        {
            buttonPressed(buttonMoveUp, pressed);
        }

        /// <summary>
        /// Nastaví vzhled tlačítka pro pohyb dolů jako stiknuté/nestiknuté podle daného parametru
        /// </summary>
        /// <param name="pressed">stiknuté/nestiknuté</param>
        public void buttonMoveDownPressed(bool pressed)
        {
            buttonPressed(buttonMoveDown, pressed);
        }

        /// <summary>
        /// Nastaví vzhled tlačítka pro rozšíření jako stiknuté/nestiknuté podle daného parametru
        /// </summary>
        /// <param name="pressed">stiknuté/nestiknuté</param>
        public void buttonWidenPressed(bool pressed)
        {
            buttonPressed(buttonWiden, pressed);
        }

        /// <summary>
        /// Nastaví vzhled tlačítka pro zůžení jako stiknuté/nestiknuté podle daného parametru
        /// </summary>
        /// <param name="pressed">stiknuté/nestiknuté</param>
        public void buttonNarrowPressed(bool pressed)
        {
            buttonPressed(buttonNarrow, pressed);
        }

        /// <summary>
        /// Nastaví vzhled tlačítka pro defaultní pozici jako stiknuté/nestiknuté podle daného parametru
        /// </summary>
        /// <param name="pressed">stiknuté/nestiknuté</param>
        public void buttonDefaultPositionPressed(bool pressed)
        {
            buttonPressed(buttonDefaultPosition, pressed);
        }

        /// <summary>
        /// Udělá vzhled daného tlačítko jako stisknuté nebo nestisknuté podle daného parametru
        /// </summary>
        /// <param name="button">tlačítko</param>
        /// <param name="pressed">stisknuté/nestisknuté</param>
        private void buttonPressed(Button button, bool pressed) {
            if (pressed)
            {
                button.BackColor = SystemColors.ButtonHighlight;
            }
            else
            {
                button.BackColor = SystemColors.ButtonFace;
            }
        }
        
        /// <summary>
        /// Vytvoření softwarového joysticku
        /// </summary>
        private void createSoftwareJoystick()
        {
            panelForJoystick.Paint += new PaintEventHandler(joystickPaint);
            panelForJoystick.Paint += new PaintEventHandler(stickPaint);
        }

        /// <summary>
        /// Vykreslení softwarového joysticku
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void joystickPaint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Rectangle rectangle = new Rectangle(0, 0, 150, 150);

            // Create a path that consists of a single ellipse.
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(0, 0, 150, 150);

            // Use the path to construct a brush.
            PathGradientBrush pthGrBrush = new PathGradientBrush(path);

            // Set the color at the center of the path to blue.
            pthGrBrush.CenterColor = Color.FromArgb(255, 46, 65, 114);

            // Set the color along the entire boundary 
            // of the path to aqua.
            Color[] colors = { Color.FromArgb(255, 2, 30, 94) };
            pthGrBrush.SurroundColors = colors;

            g.FillEllipse(pthGrBrush, rectangle);
        }

        /// <summary>
        /// Vykreslení tečky představující joystickovou páčku
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stickPaint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Rectangle rectangle = new Rectangle(stickLocation.X, stickLocation.Y, 10, 10);
            g.FillEllipse(Brushes.Red, rectangle);
        }

        /// <summary>
        /// Vrátí kurzor postupně do středu joysticku
        /// </summary>
        private void cursorBackToMid()
        {
            stickLocation = new Point(joystickR, joystickR);
            panelForJoystick.Invalidate();
        }

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            Application.DoEvents();
            inicializeObserver();
            splitContainer1.Select();
        }

        private void panelForJoystick_MouseMove(object sender, MouseEventArgs e)
        {
            
            if (e.Button == MouseButtons.Left)
            {
                if (MathLibrary.isPointInCircle(e.X, e.Y, joystickR, joystickR, joystickR))
                {
                    stickLocation = e.Location;
                    panelForJoystick.Invalidate();
                    joystickObserver((int)Math.Floor((e.X - joystickR) / ((double)joystickR / 100)), (int)Math.Floor((e.Y - joystickR) / ((double)joystickR / 100)));
                }
                else {
                    Cursor.Position = panelForJoystick.PointToScreen(MathLibrary.convertPointToCircle(e.X, e.Y,joystickR, joystickR, joystickR-2));
                }
            }
        }

        private void panelForJoystick_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (MathLibrary.isPointInCircle(e.X, e.Y, joystickR, joystickR, joystickR))
                {
                    Cursor.Hide();
                    Cursor.Clip = new Rectangle(tableLayoutPanel2.PointToScreen(panelForJoystick.Location), panelForJoystick.Size);
                    stickLocation = e.Location;
                    panelForJoystick.Invalidate();
                    joystickObserver((int)Math.Floor((e.X - joystickR) / ((double)joystickR / 100)), (int)Math.Floor((e.Y - joystickR) / ((double)joystickR / 100)));
                }
            }
        }

        private void panelForJoystick_MouseUp(object sender, MouseEventArgs e)
        {
            Cursor.Show();
            Cursor.Clip = Rectangle.Empty;
            cursorBackToMid();
        }
    }
}