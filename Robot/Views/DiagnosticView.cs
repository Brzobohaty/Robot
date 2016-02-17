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

namespace Robot
{
    public partial class DiagnosticView : UserControl, StateObserver
    {
        private static DiagnosticView instance = new DiagnosticView();
        private Dictionary<MotorId, Label> motorViews = new Dictionary<MotorId, Label>(); //mapa view motorů
        private Point canvasMid; //prostředek plátna pro vizualizaci robota
        private const int baseWidth = 100; //šířka základny vizualizace robota
        private int LP_ZK = 0; //rotace levé přední nohy ve stupních  (-100 až 100)
        private int LP_R= 0; //rotace kola levé přední nohy ve stupních (-100 až 100)
        private int LP_P = 0; //rychlost kola levé přední nohy (0 až 100)
        private int LP_Z = 0; //zdvih kola levé přední nohy (-100 až 100)
        private int PP_ZK = 0; //rotace pravé přední nohy ve stupních  (-100 až 100)
        private int PP_R = 0; //rotace kola pravé přední nohy ve stupních  (-100 až 100)
        private int PP_P = 0; //rychlost kola pravé přední nohy (0 až 100)
        private int PP_Z = 0; //zdvih kola pravé přední nohy (-100 až 100)
        private int PZ_ZK = 0; //rotace pravé zadní nohy ve stupních  (-100 až 100)
        private int PZ_R = 0; //rotace kola pravé zadní nohy ve stupních  (-100 až 100)
        private int PZ_P = 0; //rychlost kola pravé zadní nohy (0 až 100)
        private int PZ_Z = 0; //zdvih kola pravé zadní nohy (-100 až 100)
        private int LZ_ZK = 0; //rotace levé zadní nohy ve stupních  (-100 až 100)
        private int LZ_R = 0; //rotace kola levé zadní nohy ve stupních  (-100 až 100)
        private int LZ_P = 0; //rychlost kola levé zadní nohy (0 až 100)
        private int LZ_Z = 0; //zdvih kola levé zadní nohy (-100 až 100)

        public DiagnosticView()
        {
            InitializeComponent();
            motorViews.Add(MotorId.LP_P, labelLP_P);
            motorViews.Add(MotorId.PP_P, labelPP_P);
            motorViews.Add(MotorId.LZ_P, labelLZ_P);
            motorViews.Add(MotorId.PZ_P, labelPZ_P);
            motorViews.Add(MotorId.PP_R, labelPP_R);
            motorViews.Add(MotorId.LP_R, labelLP_R);
            motorViews.Add(MotorId.LZ_R, labelLZ_R);
            motorViews.Add(MotorId.PZ_R, labelPZ_R);
            motorViews.Add(MotorId.PP_Z, labelPP_Z);
            motorViews.Add(MotorId.LP_Z, labelLP_Z);
            motorViews.Add(MotorId.LZ_Z, labelLZ_Z);
            motorViews.Add(MotorId.PZ_Z, labelPZ_Z);
            motorViews.Add(MotorId.PP_ZK, labelPP_ZK);
            motorViews.Add(MotorId.LP_ZK, labelLP_ZK);
            motorViews.Add(MotorId.LZ_ZK, labelLZ_ZK);
            motorViews.Add(MotorId.PZ_ZK, labelPZ_ZK);

            canvasMid = new Point(robotCanvas.Width / 2, robotCanvas.Height / 2);
            createVisualization();

            foreach (KeyValuePair<MotorId, Label> motorView in motorViews)
            {
                toolTip.SetToolTip(motorView.Value, "ID motoru: " + motorView.Key + "\nStav motoru: not inicialized");
            }
        }

        public static DiagnosticView getInstance()
        {
            return instance;
        }

        /// <summary>
        /// Zobrazí chybovou hlášku týkající se sběrnice
        /// </summary>
        /// <param name="type">typ hlášky</param>
        /// <param name="message">text chyby</param>
        public void showDisgnosticMessage(MessageTypeEnum type, string message)
        {
            switch (type)
            {
                case MessageTypeEnum.error:
                    MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    labelMessage.ForeColor = Color.Red;
                    break;
                case MessageTypeEnum.success:
                    labelMessage.ForeColor = Color.Green;
                    break;
                case MessageTypeEnum.progress:
                    labelMessage.ForeColor = Color.Blue;
                    break;
            }
            labelMessage.Text = message;
        }

        /// <summary>
        /// Zobrazí stav konkrétního motoru
        /// </summary>
        /// <param name="state">stav</param>
        /// <param name="message">zpráva ke stavu</param>
        /// <param name="motorId">id motoru</param>
        /// <param name="speed">aktuální rychlost motoru</param>
        /// <param name="position">aktuální pozice motoru</param>
        /// <param name="speedRelative">relativní rychlost (0 až 100)</param>
        /// <param name="positionRelative">relativní pozice (-100 až 100)</param>
        public void motorStateChanged(MotorState state, string message, MotorId motorId, int speed, int position, int speedRelative, int positionRelative)
        {
            showMotorState(motorViews[motorId], state, message, motorId, speed, position);
            changeVisualization(motorId, speedRelative, positionRelative);
        }

        /// <summary>
        /// Změní parametry pro vizualizaci
        /// </summary>
        /// <param name="motorId">id motoru jehož parametry byly změněny</param>
        /// <param name="speed">rychlost motoru (0 až 100)</param>
        /// <param name="position">pozice motoru (-100 až 100)</param>
        private void changeVisualization(MotorId motorId, int speed, int position) {
            switch (motorId) {
                case MotorId.LP_P:
                    LP_P = speed;
                    break;
                case MotorId.LP_R:
                    LP_R = position;
                    break;
                case MotorId.LP_Z:
                    LP_Z = position;
                    break;
                case MotorId.LP_ZK:
                    LP_ZK = position;
                    break;
                case MotorId.PP_P:
                    PP_P = speed;
                    break;
                case MotorId.PP_R:
                    PP_R = position;
                    break;
                case MotorId.PP_Z:
                    PP_Z = position;
                    break;
                case MotorId.PP_ZK:
                    PP_ZK = position;
                    break;
                case MotorId.LZ_P:
                    LZ_P = speed;
                    break;
                case MotorId.LZ_R:
                    LZ_R = position;
                    break;
                case MotorId.LZ_Z:
                    LZ_Z = position;
                    break;
                case MotorId.LZ_ZK:
                    LZ_ZK = position;
                    break;
                case MotorId.PZ_P:
                    PZ_P = speed;
                    break;
                case MotorId.PZ_R:
                    PZ_R = position;
                    break;
                case MotorId.PZ_Z:
                    PZ_Z = position;
                    break;
                case MotorId.PZ_ZK:
                    PZ_ZK = position;
                    break;
            }
            robotCanvas.Invalidate();
        }

        /// <summary>
        /// Zobrazí stav konkrétního motoru
        /// </summary>
        /// <param name="motorView">vizualizace motoru</param>
        /// <param name="state">stav</param>
        /// <param name="message">zpráva ke stavu</param>
        /// <param name="motorId">id motoru</param>
        private void showMotorState(Label motorView, MotorState state, String message, MotorId motorId, int speed, int position) {
            switch (state)
            {
                case MotorState.error:
                    motorView.BackColor = Color.Red;
                    break;
                case MotorState.enabled:
                    motorView.BackColor = Color.Green;
                    break;
                case MotorState.disabled:
                    motorView.BackColor = Color.LightSlateGray;
                    break;
                case MotorState.running:
                    motorView.BackColor = Color.Orange;
                    break;
            }
            this.Invoke((MethodInvoker)delegate
            {
                motorView.Text = speed.ToString() + "\n" + position.ToString();
                toolTip.SetToolTip(motorView, "ID motoru: " + motorId + "\nStav motoru: " + state + "\nZpráva: " + message);
                motorView.Update();
            });
        }

        /// <summary>
        /// Vytvoření vizualizace robota
        /// </summary>
        private void createVisualization()
        {
            robotCanvas.Paint += new PaintEventHandler(basePaint);
            robotCanvas.Paint += new PaintEventHandler(LP_legPaint);
            robotCanvas.Paint += new PaintEventHandler(PP_legPaint);
            robotCanvas.Paint += new PaintEventHandler(PZ_legPaint);
            robotCanvas.Paint += new PaintEventHandler(LZ_legPaint);
        }

        /// <summary>
        /// Vykreslení základny robota
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void basePaint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rectangle = new Rectangle(canvasMid.X- (baseWidth/2), canvasMid.Y - (baseWidth / 2), baseWidth, baseWidth);
            g.FillRectangle(Brushes.LightSlateGray, rectangle);
            g.FillEllipse(Brushes.LightSlateGray, canvasMid.X - (baseWidth / 2) - ((baseWidth / 6)/2), canvasMid.Y - (baseWidth / 2) - ((baseWidth / 6) / 2), baseWidth / 6, baseWidth / 6);
            g.FillEllipse(Brushes.LightSlateGray, canvasMid.X + (baseWidth / 2) - ((baseWidth / 6) / 2), canvasMid.Y + (baseWidth / 2) - ((baseWidth / 6) / 2), baseWidth / 6, baseWidth / 6);
            g.FillEllipse(Brushes.LightSlateGray, canvasMid.X - (baseWidth / 2) - ((baseWidth / 6) / 2), canvasMid.Y + (baseWidth / 2) - ((baseWidth / 6) / 2), baseWidth / 6, baseWidth / 6);
            g.FillEllipse(Brushes.LightSlateGray, canvasMid.X + (baseWidth / 2) - ((baseWidth / 6) / 2), canvasMid.Y - (baseWidth / 2) - ((baseWidth / 6) / 2), baseWidth / 6, baseWidth / 6);
        }

        /// <summary>
        /// Vykreslení levé přední nohy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LP_legPaint(object sender, PaintEventArgs e)
        {
            legPaint(e, LP_Z, LP_ZK, LP_R, LP_P, -1, -1, -135);
        }

        /// <summary>
        /// Vykreslení pravé přední nohy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PP_legPaint(object sender, PaintEventArgs e)
        {
            legPaint(e, PP_Z, PP_ZK, PP_R, PP_P, 1, -1, -45);
        }

        /// <summary>
        /// Vykreslení pravé zadní nohy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PZ_legPaint(object sender, PaintEventArgs e)
        {
            legPaint(e, PZ_Z, PZ_ZK, PZ_R, PZ_P, 1, 1, 45);
        }

        /// <summary>
        /// Vykreslení levé zadní nohy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LZ_legPaint(object sender, PaintEventArgs e)
        {
            legPaint(e, LZ_Z, LZ_ZK, LZ_R, LZ_P, -1, 1, -225);
        }

        /// <summary>
        /// Vykreslení celého nohy robota
        /// </summary>
        /// <param name="e">Paint event</param>
        /// <param name="Z">zdvih</param>
        /// <param name="ZK">rotace nohy</param>
        /// <param name="R">rotace kola</param>
        /// <param name="P">rychlost kola</param>
        /// <param name="xC">korelace umístění na x souřadnici (-1,1)</param>
        /// <param name="yC">korelace umístění na y souřadnici (-1,1)</param>
        /// <param name="rC">korelace otočení nohy ve stupních</param>
        private void legPaint(PaintEventArgs e, int Z, int ZK, int R, int P, int xC, int yC, int rC) {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Pen pen = new Pen(Color.LightSlateGray, 5 + (Z / 40));
            Point firstPoint = new Point(canvasMid.X + ((baseWidth / 2)*xC), canvasMid.Y + ((baseWidth / 2)*yC));
            Point secondPoint = MathLibrary.getPointOnLine(canvasMid.X + ((baseWidth / 2) * xC), canvasMid.Y + ((baseWidth / 2) * yC), ZK + rC, baseWidth);
            g.DrawLine(pen, firstPoint, secondPoint);
            wheelPaint(g, secondPoint, ZK + rC - R, P);
            legLimitLinePaint(g, firstPoint, rC-70);
            legLimitLinePaint(g, firstPoint, rC+70);
            wheelLimitLinePaint(g, secondPoint, rC-150);
            wheelLimitLinePaint(g, secondPoint, rC+150);
        }

        /// <summary>
        /// Vykreslení limitních čar pro otáčení kola
        /// </summary>
        /// <param name="g">grafika</param>
        /// <param name="mid">střed kola</param>
        /// <param name="rotation">rotace ve stupních</param>
        private void wheelLimitLinePaint(Graphics g, Point mid, int rotation)
        {
            Point startPoint = MathLibrary.getPointOnLine(mid.X, mid.Y, rotation, baseWidth / 3);
            Point endPoint = MathLibrary.getPointOnLine(mid.X, mid.Y, rotation, baseWidth / 6);
            Pen pen = new Pen(Color.Black, 2);
            pen.DashStyle = DashStyle.Dot;
            g.DrawLine(pen, startPoint, endPoint);
        }

        /// <summary>
        /// Vykreslení limitních čar pro otáčení nohy
        /// </summary>
        /// <param name="g">grafika</param>
        /// <param name="mid">střed otáčení nohy</param>
        /// <param name="rotation">rotace ve stupnich</param>
        private void legLimitLinePaint(Graphics g, Point mid, int rotation)
        {
            Point startPoint = MathLibrary.getPointOnLine(mid.X, mid.Y, rotation, baseWidth / 12);
            Point endPoint = MathLibrary.getPointOnLine(mid.X, mid.Y, rotation, baseWidth / 2);
            Pen pen = new Pen(Color.Black, 2);
            pen.DashStyle = DashStyle.Dot;
            g.DrawLine(pen, startPoint, endPoint);
        }

        /// <summary>
        /// Vykreslení kola
        /// </summary>
        /// <param name="g">grafika</param>
        /// <param name="mid">střed kola</param>
        /// <param name="rotation">rotace kola ve stupnich</param>
        /// <param name="speed">rychlost táčení kola</param>
        private void wheelPaint(Graphics g, Point mid, int rotation, int speed)
        {
            g.FillEllipse(Brushes.LightSlateGray, mid.X - (baseWidth / 6), mid.Y - (baseWidth / 6), baseWidth/3, baseWidth / 3);
            arrowPaint(g, mid, rotation, baseWidth / 6, speed);
        }

        /// <summary>
        /// Vykreslení šipky kol znázorňující pohyb
        /// </summary>
        /// <param name="g">grafika</param>
        /// <param name="mid">střed kola</param>
        /// <param name="rotation">rotace kola ve stupnich</param>
        /// <param name="circleWidth">průměr kola</param>
        /// <param name="speed">rychlost pohybu kola</param>
        private void arrowPaint(Graphics g, Point mid, int rotation, int circleWidth, int speed)
        {
            Color color = Color.Black;
            if (speed > 0)
            {
                color = Color.Orange;
                endOfArrowPaint(g, mid, rotation, circleWidth, speed);
            }
            Point startPoint = MathLibrary.getPointOnLine(mid.X, mid.Y, rotation, circleWidth-1);
            Point endPoint = MathLibrary.getPointOnLine(mid.X, mid.Y, rotation, circleWidth*3+ (speed / 5) - 20);
            g.DrawLine(new Pen(color, 4), startPoint, endPoint);
        }

        /// <summary>
        /// Vykreslení konce šipky kol znázorňující pohyb
        /// </summary>
        /// <param name="g">grafika</param>
        /// <param name="mid">střed kola</param>
        /// <param name="rotation">rotace kola ve stupnich</param>
        /// <param name="circleWidth">průměr kola</param>
        /// <param name="speed">rychlost pohybu kola</param>
        private void endOfArrowPaint(Graphics g, Point mid, int rotation, int circleWidth, int speed) {
            Point endPointArrow = MathLibrary.getPointOnLine(mid.X, mid.Y, rotation, circleWidth * 3 + (speed / 5) + 4 - 20);
            Point arrowLeftPoint = MathLibrary.getPointOnLine(mid.X, mid.Y, rotation - 15, circleWidth * 2 + (speed / 5) - 20);
            Point arrowRightPoint = MathLibrary.getPointOnLine(mid.X, mid.Y, rotation + 15, circleWidth * 2 + (speed / 5) - 20);
            Point[] triangl = new Point[3];
            triangl[0] = endPointArrow;
            triangl[1] = arrowLeftPoint;
            triangl[2] = arrowRightPoint;
            g.FillPolygon(Brushes.Orange, triangl);
        }
    }
}
