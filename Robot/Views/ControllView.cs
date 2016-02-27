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
        private Action<int, int> stickDirectMoveObserver; //callback pro změnu stavu páčky (int x souřadnice páčky, int y souřadnice páčky)
        private Action<int, int> stickMoveObserver; //callback pro změnu stavu páčky (int x souřadnice páčky, int y souřadnice páčky)
        private Action<int> frontNarrowObserver; //callback pro změnu stavu analogového tlačítka pro zůžení předku (míra zůžení)
        private Action<int> backNarrowObserver; //callback pro změnu stavu analogového tlačítka pro zůžení zadku (míra zůžení)
        private Action<bool> buttonMoveUpObserver; //callback pro změnu stavu tlačítka pro pohyb nahoru (bool stisknuto)
        private Action<bool> buttonMoveDownObserver; //callback pro změnu stavu tlačítka pro pohyb dolu (bool stisknuto)
        private Action<bool> buttonNarrowObserver; //callback pro změnu stavu tlačítka pro zůžení (bool stisknuto)
        private Action<bool> buttonWidenObserver; //callback pro změnu stavu tlačítka pro rozšíření (bool stisknuto)
        private Action<bool> buttonRotateLeftObserver; //callback pro změnu stavu tlačítka pro rotaci vlevo (bool stisknuto)
        private Action<bool> buttonRotateRightObserver; //callback pro změnu stavu tlačítka pro rotaci vpravo (bool stisknuto)
        private Action<bool> buttonStopObserver; //callback pro změnu stavu tlačítka pro zastavení všeho (bool stisknuto)
        private Action<bool> buttonDefaultPositionObserver; //callback pro změnu stavu tlačítka pro defaultní pozici (bool stisknuto)
        private Action<bool> buttonAbsolutePositioningObserver; //callback pro stisknutí tačítka pro absolutní pozicování robota
        protected Action<bool> buttonTiltLeftObserver; //callback pro změnu stavu tlačítka pro naklonění vlevo (bool stisknuto)
        protected Action<bool> buttonTiltRightObserver; //callback pro změnu stavu tlačítka pro naklonění vpravo (bool stisknuto)
        protected Action<bool> buttonTiltFrontObserver; //callback pro změnu stavu tlačítka pro naklonění dopředu (bool stisknuto)
        protected Action<bool> buttonTiltBackObserver; //callback pro změnu stavu tlačítka pro naklonění dozadu (bool stisknuto)
        private bool enabledStick = true; //příznak zapnutí/vypnutí ovládání páčkou
        private const int joystickR = 70; //poloměr kružnice joysticku
        private Point stickDirectMoveLocation = new Point(joystickR, joystickR); //pozice páčky pro přímý pohyb
        private Point stickMoveLocation = new Point(joystickR, joystickR); //pozice páčky pro rádiusový pohyb
        private bool trackBarsNarrowEnabled = true; //příznak zapnutí posuvníků pro zůžování zadku/předku

        private ControllView()
        {
            InitializeComponent();
            createSoftwareJoysticks();
        }

        public static ControllView getInstance()
        {
            return instance;
        }

        delegate void ShowControlMessageCallback(MessageTypeEnum type, string message);

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
                    //MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    messageLabelControl.ForeColor = Color.Red;
                    break;
                case MessageTypeEnum.success:
                    messageLabelControl.ForeColor = Color.Green;
                    break;
                case MessageTypeEnum.progress:
                    messageLabelControl.ForeColor = Color.Blue;
                    break;
            }
            if (messageLabelControl.InvokeRequired)
            {
                ShowControlMessageCallback cb = new ShowControlMessageCallback(showControlMessage);
                this.Invoke(cb, new object[] { type, message });
            }
            else
            {
                messageLabelControl.Text = message;
            }
        }

        /// <summary>
        /// Inicializace joysticku
        /// </summary>
        /// <returns>chybovou hlášku nebo prázdný řetězec pokud nenastala chyba</returns>
        public bool inicialize()
        {
            onOff(false);
            return true;
        }

        delegate void OnOffCallback(bool on);

        /// <summary>
        /// Vypne/zapne ovládání pomocí ovladače
        /// </summary>
        /// <param name="on">true pokud zapnout</param>
        public void onOff(bool on)
        {
            if (buttonAbsolutPositioning.InvokeRequired && buttonDefaultPosition.InvokeRequired && buttonMoveDown.InvokeRequired && buttonMoveUp.InvokeRequired && buttonNarrow.InvokeRequired && buttonWiden.InvokeRequired)
            {
                OnOffCallback cb = new OnOffCallback(onOff);
                this.Invoke(cb, new object[] { on });
            }
            else
            {
                buttonAbsolutPositioning.Enabled = on;
                buttonDefaultPosition.Enabled = on;
                buttonMoveDown.Enabled = on;
                buttonMoveUp.Enabled = on;
                buttonNarrow.Enabled = on;
                buttonWiden.Enabled = on;
                buttonRotateLeft.Enabled = on;
                buttonRotateRight.Enabled = on;
                buttonStop.Enabled = on;
                buttonTiltBack.Enabled = on;
                buttonTiltFront.Enabled = on;
                buttonTiltLeft.Enabled = on;
                buttonTiltRight.Enabled = on;
                trackBarsNarrowEnabled = on;
                enabledStick = on;
            }
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
        public void subscribeDirectMoveStickObserver(Action<int, int> observer)
        {
            stickDirectMoveObserver = observer;
            panelForDirectMoveJoystick.MouseMove += new MouseEventHandler(panelForJoystick_MouseMove);
            panelForDirectMoveJoystick.MouseDown += new MouseEventHandler(panelForJoystick_MouseDown);
            panelForDirectMoveJoystick.MouseUp += new MouseEventHandler(panelForJoystick_MouseUp);
        }

        /// <summary>
        /// Přiřazení posluchače pro změnu stavu joysticku pro rádiusový pohyb
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu s parametry (int x souřadnice páčky, int y souřadnice páčky)</param>
        public void subscribeMoveStickObserver(Action<int, int> observer) {
            stickMoveObserver = observer;
            panelForMoveJoystick.MouseMove += new MouseEventHandler(panelForJoystick_MouseMove);
            panelForMoveJoystick.MouseDown += new MouseEventHandler(panelForJoystick_MouseDown);
            panelForMoveJoystick.MouseUp += new MouseEventHandler(panelForJoystick_MouseUp);
        }

        /// <summary>
        /// Přiřazení posluchače pro změnu stavu analogového tlačítka zůžení předku
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu s parametry (int míra zůžení)</param>
        public void subscribeFrontNarrowObserver(Action<int> observer)
        {
            frontNarrowObserver = observer;
            trackBarFrontNarrow.Scroll += new EventHandler(this.trackBarFrontNarrow_Scroll);
        }

        /// <summary>
        /// Přiřazení posluchače pro změnu stavu analogového tlačítka zůžení zadku
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu s parametry (int míra zůžení)</param>
        public void subscribeBackNarrowObserver(Action<int> observer)
        {
            backNarrowObserver = observer;
            trackBarBackNarrow.Scroll += new EventHandler(this.trackBarBackNarrow_Scroll);
        }

        /// <summary>
        /// Přiřazení posluchače pro změnu stavu tlačítka pro rotaci vlevo
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu s parametry (bool stisknuto)</param>
        public void subscribeButtonRotateLeftObserver(Action<bool> observer) {
            buttonRotateLeftObserver = observer;
            buttonRotateLeft.MouseDown += new MouseEventHandler(buttonRotateLeft_MouseDown);
            buttonRotateLeft.MouseUp += new MouseEventHandler(buttonRotateLeft_MouseUp);
        }

        /// <summary>
        /// Přiřazení posluchače pro změnu stavu tlačítka pro rotaci vpravo
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu s parametry (bool stisknuto)</param>
        public void subscribeButtonRotateRightObserver(Action<bool> observer) {
            buttonRotateRightObserver = observer;
            buttonRotateRight.MouseDown += new MouseEventHandler(buttonRotateRight_MouseDown);
            buttonRotateRight.MouseUp += new MouseEventHandler(buttonRotateRight_MouseUp);
        }

        /// <summary>
        /// Přiřazení posluchače pro změnu stavu tlačítka pro rotaci vpravo
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu s parametry (bool stisknuto)</param>
        public void subscribeButtonStopObserver(Action<bool> observer) {
            buttonStopObserver = observer;
            buttonStop.MouseDown += new MouseEventHandler(buttonStop_MouseDown);
            buttonStop.MouseUp += new MouseEventHandler(buttonStop_MouseUp);
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
        /// Přiřazení posluchače pro změnu stavu tlačítka pro naklonění dopředu
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu s parametry (bool stisknuto)</param>
        public void subscribeButtonTiltFrontObserver(Action<bool> observer)
        {
            buttonTiltFrontObserver = observer;
            buttonTiltFront.MouseDown += new MouseEventHandler(buttonTiltFront_MouseDown);
            buttonTiltFront.MouseUp += new MouseEventHandler(buttonTiltFront_MouseUp);
        }

        /// <summary>
        /// Přiřazení posluchače pro změnu stavu tlačítka pro naklonění dozadu
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu s parametry (bool stisknuto)</param>
        public void subscribeButtonTiltBackObserver(Action<bool> observer)
        {
            buttonTiltBackObserver = observer;
            buttonTiltBack.MouseDown += new MouseEventHandler(buttonTiltBack_MouseDown);
            buttonTiltBack.MouseUp += new MouseEventHandler(buttonTiltBack_MouseUp);
        }

        /// <summary>
        /// Přiřazení posluchače pro změnu stavu tlačítka pro naklonění doprava
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu s parametry (bool stisknuto)</param>
        public void subscribeButtonTiltRightObserver(Action<bool> observer)
        {
            buttonTiltRightObserver = observer;
            buttonTiltRight.MouseDown += new MouseEventHandler(buttonTiltRight_MouseDown);
            buttonTiltRight.MouseUp += new MouseEventHandler(buttonTiltRight_MouseUp);
        }

        /// <summary>
        /// Přiřazení posluchače pro změnu stavu tlačítka pro naklonění doleva
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu s parametry (bool stisknuto)</param>
        public void subscribeButtonTiltLeftObserver(Action<bool> observer)
        {
            buttonTiltLeftObserver = observer;
            buttonTiltLeft.MouseDown += new MouseEventHandler(buttonTiltLeft_MouseDown);
            buttonTiltLeft.MouseUp += new MouseEventHandler(buttonTiltLeft_MouseUp);
        }

        /// <summary>
        /// Přiřazení posluchače, když nastane chyba
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu</param>
        public void subscribeErrorObserver(Action observer){}

        /// <summary>
        /// Pohne se softwarovým joystickem pro přímý pohyb na dané souřadnice
        /// </summary>
        /// <param name="x">x souřadnice od -100 do 100</param>
        /// <param name="y">y souřadnice od -100 do 100</param>
        public void moveDirectMoveJoystick(int x, int y)
        {
            stickDirectMoveLocation = new Point((int)Math.Floor((x + 100) * ((double)joystickR / 100)), (int)Math.Floor((y + 100) * ((double)joystickR / 100)));
            panelForDirectMoveJoystick.Invalidate();
        }

        /// <summary>
        /// Pohne se softwarovým joystickem pro rádiusový pohyb na dané souřadnice
        /// </summary>
        /// <param name="x">x souřadnice od -100 do 100</param>
        /// <param name="y">y souřadnice od -100 do 100</param>
        public void moveMoveJoystick(int x, int y)
        {
            stickMoveLocation = new Point((int)Math.Floor((x + 100) * ((double)joystickR / 100)), (int)Math.Floor((y + 100) * ((double)joystickR / 100)));
            panelForMoveJoystick.Invalidate();
        }

        /// <summary>
        /// Nastaví slider pro zůžení zadku na danou hodnotu
        /// </summary>
        /// <param name="value">hodnota od 0 do 100</param>
        public void setBackNarrowSlider(int value)
        {
            trackBarBackNarrow.Value = value;
        }

        /// <summary>
        /// Nastaví slider pro zůžení p5edku na danou hodnotu
        /// </summary>
        /// <param name="value">hodnota od 0 do 100</param>
        public void setFrontNarrowSlider(int value)
        {
            trackBarFrontNarrow.Value = value;
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
        /// Nastaví vzhled tlačítka pro rotaci vlevo jako stiknuté/nestiknuté podle daného parametru
        /// </summary>
        /// <param name="pressed">stiknuté/nestiknuté</param>
        public void buttonRotateLeftPressed(bool pressed)
        {
            buttonPressed(buttonRotateLeft, pressed);
        }

        /// <summary>
        /// Nastaví vzhled tlačítka pro rotaci vpravo jako stiknuté/nestiknuté podle daného parametru
        /// </summary>
        /// <param name="pressed">stiknuté/nestiknuté</param>
        public void buttonRotateRightPressed(bool pressed)
        {
            buttonPressed(buttonRotateRight, pressed);
        }

        /// <summary>
        /// Nastaví vzhled tlačítka pro zastavení jako stiknuté/nestiknuté podle daného parametru
        /// </summary>
        /// <param name="pressed">stiknuté/nestiknuté</param>
        public void buttonStopPressed(bool pressed)
        {
            buttonPressed(buttonStop, pressed);
        }

        /// <summary>
        /// Nastaví vzhled tlačítka pro naklonění dopředu jako stiknuté/nestiknuté podle daného parametru
        /// </summary>
        /// <param name="pressed">stiknuté/nestiknuté</param>
        public void buttonTiltFrontPressed(bool pressed)
        {
            buttonPressed(buttonTiltFront, pressed);
        }

        /// <summary>
        /// Nastaví vzhled tlačítka pro naklonění dozadu jako stiknuté/nestiknuté podle daného parametru
        /// </summary>
        /// <param name="pressed">stiknuté/nestiknuté</param>
        public void buttonTiltBackPressed(bool pressed)
        {
            buttonPressed(buttonTiltBack, pressed);
        }

        /// <summary>
        /// Nastaví vzhled tlačítka pro naklonění doleva jako stiknuté/nestiknuté podle daného parametru
        /// </summary>
        /// <param name="pressed">stiknuté/nestiknuté</param>
        public void buttonTiltLeftPressed(bool pressed)
        {
            buttonPressed(buttonTiltLeft, pressed);
        }

        /// <summary>
        /// Nastaví vzhled tlačítka pro naklonění doprava jako stiknuté/nestiknuté podle daného parametru
        /// </summary>
        /// <param name="pressed">stiknuté/nestiknuté</param>
        public void buttonTiltRightPressed(bool pressed)
        {
            buttonPressed(buttonTiltRight, pressed);
        }

        /// <summary>
        /// Odstraní všechny posluchače na joysticku
        /// </summary>
        public void unsibscribeAllObservers()
        {
            stickDirectMoveObserver = emptyMethod;
            stickMoveObserver = emptyMethod;
            buttonMoveUpObserver = emptyMethod;
            buttonMoveDownObserver = emptyMethod;
            buttonNarrowObserver = emptyMethod;
            buttonWidenObserver = emptyMethod;
            buttonDefaultPositionObserver = emptyMethod;
            buttonRotateLeftObserver = emptyMethod;
            buttonRotateRightObserver = emptyMethod;
            buttonStopObserver = emptyMethod;
            frontNarrowObserver = emptyMethod;
            backNarrowObserver = emptyMethod;
            buttonTiltLeftObserver = emptyMethod;
            buttonTiltRightObserver = emptyMethod;
            buttonTiltFrontObserver = emptyMethod;
            buttonTiltBackObserver = emptyMethod;
        }

        private void emptyMethod(bool a){}
        private void emptyMethod(int a, int b){}
        private void emptyMethod(int a) { }

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
        private void createSoftwareJoysticks()
        {
            panelForDirectMoveJoystick.Paint += new PaintEventHandler(joystickPaint);
            panelForDirectMoveJoystick.Paint += new PaintEventHandler(stickPaint);
            panelForMoveJoystick.Paint += new PaintEventHandler(joystickPaint);
            panelForMoveJoystick.Paint += new PaintEventHandler(stickPaint);
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
            Point location;
            if (sender.Equals(panelForDirectMoveJoystick))
            {
                location = stickDirectMoveLocation;
            }
            else {
                location = stickMoveLocation;
            }
            
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rectangle = new Rectangle(location.X, location.Y, 10, 10);
            g.FillEllipse(Brushes.Red, rectangle);
        }

        /// <summary>
        /// Vrátí kurzor postupně do středu joysticku
        /// </summary>
        private void cursorBackToMid(Point location, PictureBox box, Action<int, int> stickObserver)
        {
            location = new Point(joystickR, joystickR);
            box.Invalidate();
            if (enabledStick) {
                stickObserver(0, 0);
            }
        }

        //event listenery ================================================================
        private void panelForJoystick_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (sender.Equals(panelForDirectMoveJoystick))
                {
                    joystickMouseMoveHandler(e, stickDirectMoveLocation, panelForDirectMoveJoystick, stickDirectMoveObserver);
                }
                else
                {
                    joystickMouseMoveHandler(e, stickMoveLocation, panelForMoveJoystick, stickMoveObserver);
                }
            }
        }

        private void joystickMouseMoveHandler(MouseEventArgs e, Point location, PictureBox box, Action<int, int> stickObserver) {
            if (MathLibrary.isPointInCircle(e.X, e.Y, joystickR, joystickR, joystickR))
            {
                location = e.Location;
                box.Invalidate();
                if (enabledStick)
                {
                    stickObserver((int)Math.Floor((e.X - joystickR) / ((double)joystickR / 100)), (int)Math.Floor((e.Y - joystickR) / ((double)joystickR / 100)));
                }
            }
            else
            {
                Cursor.Position = box.PointToScreen(MathLibrary.convertPointToCircle(e.X, e.Y, joystickR, joystickR, joystickR - 2));
            }
        }

        private void panelForJoystick_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (MathLibrary.isPointInCircle(e.X, e.Y, joystickR, joystickR, joystickR))
                {
                    Cursor.Hide();
                    if (sender.Equals(panelForDirectMoveJoystick))
                    {
                        joystickMouseDownHandler(e, stickDirectMoveLocation, panelForDirectMoveJoystick, stickDirectMoveObserver);
                    }
                    else
                    {
                        joystickMouseDownHandler(e, stickMoveLocation, panelForMoveJoystick, stickMoveObserver);
                    }   
                }
            }
        }

        private void joystickMouseDownHandler(MouseEventArgs e, Point location, PictureBox box, Action<int, int> stickObserver) {
            Cursor.Clip = new Rectangle(tableLayoutPanel2.PointToScreen(box.Location), box.Size);
            location = e.Location;
            box.Invalidate();
            if (enabledStick)
            {
                stickObserver((int)Math.Floor((e.X - joystickR) / ((double)joystickR / 100)), (int)Math.Floor((e.Y - joystickR) / ((double)joystickR / 100)));
            }
        }

        private void panelForJoystick_MouseUp(object sender, MouseEventArgs e)
        {
            Cursor.Show();
            Cursor.Clip = Rectangle.Empty;
            if (sender.Equals(panelForDirectMoveJoystick))
            {
                cursorBackToMid(stickDirectMoveLocation, panelForDirectMoveJoystick, stickDirectMoveObserver);
            }
            else
            {
                cursorBackToMid(stickMoveLocation, panelForMoveJoystick, stickMoveObserver);
            }
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

        private void buttonRotateRight_MouseDown(object sender, MouseEventArgs e)
        {
            buttonRotateRightObserver(true);
        }

        private void buttonRotateRight_MouseUp(object sender, MouseEventArgs e)
        {
            buttonRotateRightObserver(false);
        }

        private void buttonRotateLeft_MouseDown(object sender, MouseEventArgs e)
        {
            buttonRotateLeftObserver(true);
        }

        private void buttonRotateLeft_MouseUp(object sender, MouseEventArgs e)
        {
            buttonRotateLeftObserver(false);
        }

        private void buttonStop_MouseDown(object sender, MouseEventArgs e)
        {
            buttonStopObserver(true);
        }

        private void buttonStop_MouseUp(object sender, MouseEventArgs e)
        {
            buttonStopObserver(false);
        }

        private void buttonTiltFront_MouseDown(object sender, MouseEventArgs e)
        {
            buttonTiltFrontObserver(true);
        }

        private void buttonTiltFront_MouseUp(object sender, MouseEventArgs e)
        {
            buttonTiltFrontObserver(false);
        }

        private void buttonTiltBack_MouseDown(object sender, MouseEventArgs e)
        {
            buttonTiltBackObserver(true);
        }

        private void buttonTiltBack_MouseUp(object sender, MouseEventArgs e)
        {
            buttonTiltBackObserver(false);
        }

        private void buttonTiltLeft_MouseDown(object sender, MouseEventArgs e)
        {
            buttonTiltLeftObserver(true);
        }

        private void buttonTiltLeft_MouseUp(object sender, MouseEventArgs e)
        {
            buttonTiltLeftObserver(false);
        }

        private void buttonTiltRight_MouseDown(object sender, MouseEventArgs e)
        {
            buttonTiltRightObserver(true);
        }

        private void buttonTiltRight_MouseUp(object sender, MouseEventArgs e)
        {
            buttonTiltRightObserver(false);
        }

        private void trackBarFrontNarrow_Scroll(object sender, EventArgs e)
        {
            if (trackBarsNarrowEnabled) {
                frontNarrowObserver(((TrackBar)sender).Value);
            }
        }

        private void trackBarBackNarrow_Scroll(object sender, EventArgs e)
        {
            if (trackBarsNarrowEnabled)
            {
                backNarrowObserver(((TrackBar)sender).Value);
            }
        }

        private void trackBarFrontNarrow_MouseUp(object sender, MouseEventArgs e)
        {
            if (trackBarsNarrowEnabled)
            {
                trackBarFrontNarrow.Value = 0;
            frontNarrowObserver(((TrackBar)sender).Value);
            }
        }

        private void trackBarBackNarrow_MouseUp(object sender, MouseEventArgs e)
        {
            if (trackBarsNarrowEnabled)
            {
                trackBarBackNarrow.Value = 0;
            backNarrowObserver(((TrackBar)sender).Value);
            }
        }
    }
}
