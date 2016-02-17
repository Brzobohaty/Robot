using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using Robot.Joystick;

namespace Robot
{
    public partial class ControllView : UserControl, IJoystick
    {
        private static ControllView instance = new ControllView();
        private Action<int, int> stickObserver; //callback pro změnu stavu páčky (int x souřadnice páčky, int y souřadnice páčky)
        private Action<bool> buttonMoveUpObserver; //callback pro změnu stavu tlačítka pro pohyb nahoru (bool pohyb nahoru)
        private Action<bool> buttonMoveDownObserver; //callback pro změnu stavu tlačítka pro pohyb dolu (bool pohyb dolu)
        private Action<bool> buttonNarrowObserver; //callback pro změnu stavu tlačítka pro zůžení (bool zůžit)
        private Action<bool> buttonWidenObserver; //callback pro změnu stavu tlačítka pro rozšíření (bool rozšířit)
        private Action<bool> buttonDefaultPositionObserver; //callback pro změnu stavu tlačítka pro defaultní pozici (bool defaultní pozice)
        private Action<bool> buttonAbsolutePositioningObserver; //callback pro stisknutí tačítka pro absolutní pozicování robota
        private bool enabledStick = true; //příznak zapnutí/vypnutí ovládání páčkou

        private const int joystickR = 70; //poloměr kružnice joysticku
        private Point stickLocation = new Point(joystickR, joystickR); //pozice páčky joysticku

        private ControllView()
        {
            InitializeComponent();
            createSoftwareJoystick();
        }

        public static ControllView getInstance()
        {
            return instance;
        }

        /// <summary>
        /// Zobrazí hlášku týkající se ovládání
        /// </summary>
        /// <param name="type">typ hlášky</param>
        /// <param name="message">text hlášky</param>
        public void showControlMessage(MessageTypeEnum type, string message)
        {
            switch (type)
            {
                case MessageTypeEnum.error:
                    MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    messageLabelControl.ForeColor = Color.Red;
                    break;
                case MessageTypeEnum.success:
                    messageLabelControl.ForeColor = Color.Green;
                    break;
                case MessageTypeEnum.progress:
                    messageLabelControl.ForeColor = Color.Blue;
                    break;
            }
            messageLabelControl.Text = message;
        }

        /// <summary>
        /// Inicializace joysticku
        /// </summary>
        /// <returns>chybovou hlášku nebo prázdný řetězec pokud nenastala chyba</returns>
        public string inicialize()
        {
            onOff(false);
            return "";
        }

        /// <summary>
        /// Vypne/zapne ovládání pomocí ovladače
        /// </summary>
        /// <param name="on">true pokud zapnout</param>
        public void onOff(bool on)
        {
            buttonAbsolutPositioning.Enabled = on;
            buttonDefaultPosition.Enabled = on;
            buttonMoveDown.Enabled = on;
            buttonMoveUp.Enabled = on;
            buttonNarrow.Enabled = on;
            buttonWiden.Enabled = on;
            enabledStick = on;
        }

        /// <summary>
        /// Přiřazení posluchače pro stisknutí tačítka pro absolutní pozicování robota
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu</param>
        public void subscribeAbsolutePositioningObserver(Action<bool> observer)
        {
            buttonAbsolutePositioningObserver = observer;
        }

        /// <summary>
        /// Přiřazení posluchače pro změnu stavu joysticku
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu s parametry (int x souřadnice páčky, int y souřadnice páčky)</param>
        public void subscribeStickObserver(Action<int, int> observer)
        {
            stickObserver = observer;
            panelForJoystick.MouseMove += new MouseEventHandler(panelForJoystick_MouseMove);
            panelForJoystick.MouseDown += new MouseEventHandler(panelForJoystick_MouseDown);
            panelForJoystick.MouseUp += new MouseEventHandler(panelForJoystick_MouseUp);
        }

        /// <summary>
        /// Přiřazení posluchače pro změnu stavu tlačítka pro pohyb nahoru
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu s parametry (bool pohyb nahoru)</param>
        public void subscribeButtonMoveUpObserver(Action<bool> observer)
        {
            buttonMoveUpObserver = observer;
            buttonMoveUp.MouseDown += new MouseEventHandler(buttonMoveUp_MouseDown);
            buttonMoveUp.MouseUp += new MouseEventHandler(buttonMoveUp_MouseUp);
        }

        /// <summary>
        /// Přiřazení posluchače pro změnu stavu tlačítka pro pohyb dolu
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu s parametry (bool pohyb dolu)</param>
        public void subscribeButtonMoveDownObserver(Action<bool> observer)
        {
            buttonMoveDownObserver = observer;
            buttonMoveDown.MouseDown += new MouseEventHandler(buttonMoveDown_MouseDown);
            buttonMoveDown.MouseUp += new MouseEventHandler(buttonMoveDown_MouseUp);
        }

        /// <summary>
        /// Přiřazení posluchače pro změnu stavu tlačítka pro zůžení
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu s parametry (bool zůžit)</param>
        public void subscribeButtonNarrowObserver(Action<bool> observer)
        {
            buttonNarrowObserver = observer;
            buttonNarrow.MouseDown += new MouseEventHandler(buttonNarrow_MouseDown);
            buttonNarrow.MouseUp += new MouseEventHandler(buttonNarrow_MouseUp);
        }

        /// <summary>
        /// Přiřazení posluchače pro změnu stavu tlačítka pro rozšíření
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu s parametry (bool rozšířit)</param>
        public void subscribeButtonWidenObserver(Action<bool> observer)
        {
            buttonWidenObserver = observer;
            buttonWiden.MouseDown += new MouseEventHandler(buttonWiden_MouseDown);
            buttonWiden.MouseUp += new MouseEventHandler(buttonWiden_MouseUp);
        }

        /// <summary>
        /// Přiřazení posluchače pro změnu stavu tlačítka pro defaultní pozici
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu s parametry (bool defaultní pozice)</param>
        public void subscribeButtonDefaultPositionObserver(Action<bool> observer)
        {
            buttonDefaultPositionObserver = observer;
            buttonDefaultPosition.MouseDown += new MouseEventHandler(buttonDefaultPosition_MouseDown);
            buttonDefaultPosition.MouseUp += new MouseEventHandler(buttonDefaultPosition_MouseUp);
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
        /// Odstraní všechny posluchače na joysticku
        /// </summary>
        public void unsibscribeAllObservers()
        {
            stickObserver = emptyMethod;
            buttonMoveUpObserver = emptyMethod;
            buttonMoveDownObserver = emptyMethod;
            buttonNarrowObserver = emptyMethod;
            buttonWidenObserver = emptyMethod;
            buttonDefaultPositionObserver = emptyMethod;
        }

        private void emptyMethod(bool a){}
        private void emptyMethod(int a, int b){}

        /// <summary>
        /// Udělá vzhled daného tlačítko jako stisknuté nebo nestisknuté podle daného parametru
        /// </summary>
        /// <param name="button">tlačítko</param>
        /// <param name="pressed">stisknuté/nestisknuté</param>
        private void buttonPressed(Button button, bool pressed)
        {
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
            g.SmoothingMode = SmoothingMode.AntiAlias;
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
            g.SmoothingMode = SmoothingMode.AntiAlias;
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
            if (enabledStick) {
                stickObserver(0, 0);
            }
        }

        //event listenery ================================================================
        private void panelForJoystick_MouseMove(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                if (MathLibrary.isPointInCircle(e.X, e.Y, joystickR, joystickR, joystickR))
                {
                    stickLocation = e.Location;
                    panelForJoystick.Invalidate();
                    if (enabledStick)
                    {
                        stickObserver((int)Math.Floor((e.X - joystickR) / ((double)joystickR / 100)), (int)Math.Floor((e.Y - joystickR) / ((double)joystickR / 100)));
                    }
                }
                else
                {
                    Cursor.Position = panelForJoystick.PointToScreen(MathLibrary.convertPointToCircle(e.X, e.Y, joystickR, joystickR, joystickR - 2));
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
                    if (enabledStick)
                    {
                        stickObserver((int)Math.Floor((e.X - joystickR) / ((double)joystickR / 100)), (int)Math.Floor((e.Y - joystickR) / ((double)joystickR / 100)));
                    }
                }
            }
        }

        private void panelForJoystick_MouseUp(object sender, MouseEventArgs e)
        {
            Cursor.Show();
            Cursor.Clip = Rectangle.Empty;
            cursorBackToMid();
        }

        private void buttonDefaultPosition_MouseUp(object sender, MouseEventArgs e)
        {
            buttonDefaultPositionObserver(false);
        }

        private void buttonDefaultPosition_MouseDown(object sender, MouseEventArgs e)
        {
            buttonDefaultPositionObserver(true);
        }

        private void buttonWiden_MouseUp(object sender, MouseEventArgs e)
        {
            buttonWidenObserver(false);
        }

        private void buttonWiden_MouseDown(object sender, MouseEventArgs e)
        {
            buttonWidenObserver(true);
        }

        private void buttonNarrow_MouseUp(object sender, MouseEventArgs e)
        {
            buttonNarrowObserver(false);
        }

        private void buttonNarrow_MouseDown(object sender, MouseEventArgs e)
        {
            buttonNarrowObserver(true);
        }

        private void buttonMoveDown_MouseUp(object sender, MouseEventArgs e)
        {
            buttonMoveDownObserver(false);
        }

        private void buttonMoveDown_MouseDown(object sender, MouseEventArgs e)
        {
            buttonMoveDownObserver(true);
        }

        private void buttonMoveUp_MouseUp(object sender, MouseEventArgs e)
        {
            buttonMoveUpObserver(false);
        }

        private void buttonMoveUp_MouseDown(object sender, MouseEventArgs e)
        {
            buttonMoveUpObserver(true);
        }

        private void buttonAbsolutPositioning_Click(object sender, EventArgs e)
        {
            buttonAbsolutePositioningObserver(true);
        }
    }
}
