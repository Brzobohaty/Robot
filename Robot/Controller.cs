using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace Robot
{
    class Controller
    {
        private MainWindow mainWindow; // hlavní okno apklikace
        private IRobot robot; // instance představující robota
        private AbstractJoystick joystick; // instance představující HW joystick

        //timery obstarávající periodické spouštění při držění tlačítka
        private System.Timers.Timer moveUpPeriodHandler; 
        private System.Timers.Timer moveDownPeriodHandler;
        private System.Timers.Timer widenPeriodHandler;
        private System.Timers.Timer narrowPeriodHandler;
        private System.Timers.Timer defaultPositionPeriodHandler;


        /// <param name="mainWindow">hlavní okno aplikace</param>
        public Controller(MainWindow mainWindow)
        {
            this.robot = new EposRobot();
            this.joystick = new Gamepad();
            this.mainWindow = mainWindow;
            mainWindow.subscribeWindowShownObserver(inicialize);
        }

        /// <summary>
        /// Inicializace připojení k motorům
        /// </summary>
        private void inicialize()
        {
            mainWindow.clearErrorText();
            string errorMessageMotor = robot.inicialize();
            if (errorMessageMotor.Length > 0)
            {
                mainWindow.showMotorsError(errorMessageMotor);
            }
            string errorMessageControll = joystick.inicialize();
            if (errorMessageControll.Length > 0)
            {
                mainWindow.showControlError(errorMessageControll);
                mainWindow.subscribeJoystickObserver(softwareJoystickMoved);
            }
            else
            {
                joystick.subscribeStickObserver(hardwareJoystickChanged);
                joystick.subscribeButtonMoveUpObserver(hardwareJoystickButtonMoveUpChanged);
                joystick.subscribeButtonMoveDownObserver(hardwareJoystickButtonMoveDownChanged);
                joystick.subscribeButtonNarrowObserver(hardwareJoystickButtonNarrowChanged);
                joystick.subscribeButtonWidenObserver(hardwareJoystickButtonWidenChanged);
                joystick.subscribeButtonDefaultPositionObserver(hardwareJoystickButtonDefaultPositionChanged);
                moveUpPeriodHandler = getPeriodHandler();
                moveDownPeriodHandler = getPeriodHandler();
                widenPeriodHandler = getPeriodHandler();
                narrowPeriodHandler = getPeriodHandler();
                defaultPositionPeriodHandler = getPeriodHandler();
            }
        }

        /// <summary>
        /// Callback pro změnu stavu páčky gamepadu
        /// </summary>
        /// <param name="x">x souřadnice joysticku od -100 do 100</param>
        /// <param name="y">y souřadnice joysticku od -100 do 100</param>
        private void hardwareJoystickChanged(int x, int y)
        {
            Point corectedPoint = MathLibrary.convertPointToCircle(x, y, 0, 0, 101);
            moveRobot(corectedPoint.X, corectedPoint.Y);
            mainWindow.moveJoystick(corectedPoint.X, corectedPoint.Y);
        }

        /// <summary>
        /// Vytvoří timer, který bude periodicky spouštět akci robota.
        /// </summary>
        /// <returns>timer</returns>
        private System.Timers.Timer getPeriodHandler() {
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(periodiclyAction);
            aTimer.Interval = 500;
            aTimer.Stop();
            return aTimer;
        }

        /// <summary>
        /// Akce s robotem vykonávána periodicky
        /// </summary>
        /// <param name="sender">timer, který se stará o periodu</param>
        /// <param name="arg">argumenty</param>
        private void periodiclyAction(object sender, ElapsedEventArgs arg) {
            if (sender == moveUpPeriodHandler) {
                robot.moveUp();
            }
            if (sender == moveDownPeriodHandler)
            {
                robot.moveDown();
            }
            if (sender == widenPeriodHandler)
            {
                robot.widen();
            }
            if (sender == narrowPeriodHandler)
            {
                robot.narrow();
            }
            if (sender == defaultPositionPeriodHandler)
            {
                robot.setDefaultPosition();
            }
        }

        /// <summary>
        /// Callback pro změnu stavu tlačítka pro pohyb nahoru 
        /// </summary>
        /// <param name="pressed">stisknuté tlačítko pro pohyb robota nahoru</param>
        private void hardwareJoystickButtonMoveUpChanged(bool pressed)
        {
            if (pressed)
            {
                periodiclyAction(moveUpPeriodHandler, null);
                moveUpPeriodHandler.Start();
            }
            else
            {
                moveUpPeriodHandler.Stop();
            }
            mainWindow.buttonMoveUpPressed(pressed);
        }

        /// <summary>
        /// Callback pro změnu stavu tlačítka pro pohyb dolu
        /// </summary>
        /// <param name="pressed">stisknuté tlačítko pro pohyb robota dolu</param>
        private void hardwareJoystickButtonMoveDownChanged(bool pressed)
        {
            if (pressed)
            {
                periodiclyAction(moveDownPeriodHandler, null);
                moveDownPeriodHandler.Start();
            }
            else
            {
                moveDownPeriodHandler.Stop();
            }
            mainWindow.buttonMoveDownPressed(pressed);
        }

        /// <summary>
        /// Callback pro změnu stavu tlačítka pro rozšíření
        /// </summary>
        /// <param name="pressed">stisknuté tlačítko pro rozšíření robota</param>
        private void hardwareJoystickButtonWidenChanged(bool pressed)
        {
            if (pressed)
            {
                periodiclyAction(widenPeriodHandler, null);
                widenPeriodHandler.Start();
            }
            else
            {
                widenPeriodHandler.Stop();
            }
            mainWindow.buttonWidenPressed(pressed);
        }

        /// <summary>
        /// Callback pro změnu stavu tlačítka pro zůžení
        /// </summary>
        /// <param name="pressed">stisknuté tlačítko pro zůžení robota</param>
        private void hardwareJoystickButtonNarrowChanged(bool pressed)
        {
            if (pressed)
            {
                periodiclyAction(narrowPeriodHandler, null);
                narrowPeriodHandler.Start();
            }
            else
            {
                narrowPeriodHandler.Stop();
            }
            mainWindow.buttonNarrowPressed(pressed);
        }

        /// <summary>
        /// Callback pro změnu stavu tlačítka pro defaultní pozici
        /// </summary>
        /// <param name="pressed">stisknuté tlačítko pro návrat do původní polohy</param>
        private void hardwareJoystickButtonDefaultPositionChanged(bool pressed)
        {
            if (pressed)
            {
                periodiclyAction(defaultPositionPeriodHandler, null);
                defaultPositionPeriodHandler.Start();
            }
            else
            {
                defaultPositionPeriodHandler.Stop();
            }
            mainWindow.buttonDefaultPositionPressed(pressed);
        }

        /// <summary>
        /// Callback pro změnu stavu sfotwarového joysticku
        /// </summary>
        /// <param name="x">x souřadnice od -100 do 100</param>
        /// <param name="y">y souřadnice od -100 do 100</param>
        private void softwareJoystickMoved(int x, int y)
        {
            moveRobot(x, y);
        }

        /// <summary>
        /// Pohne s robotem směrem a rychlostí podle dané polohy joysticku
        /// </summary>
        /// <param name="x">x souřadnice od -100 do 100</param>
        /// <param name="y">y souřadnice od -100 do 100</param>
        private void moveRobot(int x, int y)
        {
            int angle = (int)MathLibrary.getAngle(0, 0, 0, -100, x, y);
            if (x < 0)
            {
                angle = -angle;
            }
            robot.move(angle, (int)MathLibrary.getPointsDistance(0, 0, x, y));
        }
    }
}
