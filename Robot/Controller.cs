using Robot.Joystick;
using Robot.Robot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace Robot
{
    class Controller
    {
        private MainWindow mainWindow; // hlavní okno apklikace
        private ControllView controllView; // view s ovládáním robota
        private DiagnosticView diagnosticView; // view s diagnstikou robota
        private AbsoluteControllView absoluteControllView; //view s absolutním ovládáním robota
        private IRobot robot; // instance představující robota
        private IJoystick joystick; // instance představující joystick
        private bool defaultPositionButtonWasPressed; //příznak, že bylo stlačeno tlačítko pro defaultní pozici

        //timery obstarávající periodické spouštění při držění tlačítka
        private System.Timers.Timer moveUpPeriodHandler;
        private System.Timers.Timer moveDownPeriodHandler;
        private System.Timers.Timer widenPeriodHandler;
        private System.Timers.Timer narrowPeriodHandler;
        private System.Timers.Timer defaultPositionPeriodHandler;

        /// <param name="mainWindow">hlavní okno aplikace</param>
        public Controller(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            controllView = ControllView.getInstance();
            absoluteControllView = AbsoluteControllView.getInstance();
            diagnosticView = DiagnosticView.getInstance();
            mainWindow.subscribeWindowShownObserver(inicialize);
            mainWindow.subscribeWindowCloseObserver(closeApplication);
            controllView.subscribeAbsolutePositioningObserver(buttonForChangeControllModePressed);
            absoluteControllView.subscribeJoystickPositioningObserver(buttonForChangeControllModePressed);
            absoluteControllView.subscribeButtonForAbsoluteMoveClickObserver(buttonForAbsoluteMoveClicked);
            absoluteControllView.subscribeButtonForRecalibrClickObserver(buttonForRecalibrClicked);
            absoluteControllView.subscribeButtonForSetDefaultPositionClickObserver(buttonForSetDefaultStateClicked);
            absoluteControllView.subscribeButtonForCalibrClickObserver(buttonForCalibrClicked);
        }

        /// <summary>
        /// Inicializace připojení k motorům
        /// </summary>
        private void inicialize()
        {
            RobotBridge robotBridge = new RobotBridge();
            robot = robotBridge.getRobot(diagnosticView);
            if (robotBridge.errorMessage.Length > 0)
            {
                diagnosticView.showBusError(robotBridge.errorMessage);
            }

            JoystickBridge joystickBridge = new JoystickBridge();
            joystick = joystickBridge.getJoystick();
            if (joystickBridge.errorMessage.Length > 0)
            {
                controllView.showControlError(joystickBridge.errorMessage);
            }
            joystick.subscribeStickObserver(joystickChanged);
            joystick.subscribeButtonMoveUpObserver(joystickButtonMoveUpChanged);
            joystick.subscribeButtonMoveDownObserver(joystickButtonMoveDownChanged);
            joystick.subscribeButtonNarrowObserver(joystickButtonNarrowChanged);
            joystick.subscribeButtonWidenObserver(joystickButtonWidenChanged);
            joystick.subscribeButtonDefaultPositionObserver(joystickButtonDefaultPositionChanged);
            moveUpPeriodHandler = getPeriodHandler();
            moveDownPeriodHandler = getPeriodHandler();
            widenPeriodHandler = getPeriodHandler();
            narrowPeriodHandler = getPeriodHandler();
            defaultPositionPeriodHandler = getPeriodHandler();
            checkHoming();
        }

        /// <summary>
        /// Callback pro změnu stavu páčky gamepadu
        /// </summary>
        /// <param name="x">x souřadnice joysticku od -100 do 100</param>
        /// <param name="y">y souřadnice joysticku od -100 do 100</param>
        private void joystickChanged(int x, int y)
        {
            Point corectedPoint = MathLibrary.convertPointToCircle(x, y, 0, 0, 101);
            moveRobot(corectedPoint.X, corectedPoint.Y);
            if (!(joystick is MainWindow))
            {
                controllView.moveJoystick(corectedPoint.X, corectedPoint.Y);
            }
        }

        /// <summary>
        /// Vytvoří timer, který bude periodicky spouštět akci robota.
        /// </summary>
        /// <returns>timer</returns>
        private System.Timers.Timer getPeriodHandler()
        {
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
        private void periodiclyAction(object sender, ElapsedEventArgs arg)
        {
            if (sender == moveUpPeriodHandler)
            {
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
        }

        /// <summary>
        /// Callback pro změnu stavu tlačítka pro pohyb nahoru 
        /// </summary>
        /// <param name="pressed">stisknuté tlačítko pro pohyb robota nahoru</param>
        private void joystickButtonMoveUpChanged(bool pressed)
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
            controllView.buttonMoveUpPressed(pressed);
        }

        /// <summary>
        /// Callback pro změnu stavu tlačítka pro pohyb dolu
        /// </summary>
        /// <param name="pressed">stisknuté tlačítko pro pohyb robota dolu</param>
        private void joystickButtonMoveDownChanged(bool pressed)
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
            controllView.buttonMoveDownPressed(pressed);
        }

        /// <summary>
        /// Callback pro změnu stavu tlačítka pro rozšíření
        /// </summary>
        /// <param name="pressed">stisknuté tlačítko pro rozšíření robota</param>
        private void joystickButtonWidenChanged(bool pressed)
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
            controllView.buttonWidenPressed(pressed);
        }

        /// <summary>
        /// Callback pro změnu stavu tlačítka pro zůžení
        /// </summary>
        /// <param name="pressed">stisknuté tlačítko pro zůžení robota</param>
        private void joystickButtonNarrowChanged(bool pressed)
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
            controllView.buttonNarrowPressed(pressed);
        }

        /// <summary>
        /// Callback pro změnu stavu tlačítka pro defaultní pozici
        /// </summary>
        /// <param name="pressed">stisknuté tlačítko pro návrat do původní polohy</param>
        private void joystickButtonDefaultPositionChanged(bool pressed)
        {
            if (pressed)
            {
                defaultPositionButtonWasPressed = true;
            }
            else
            {
                if (defaultPositionButtonWasPressed) {
                    robot.setDefaultPosition();
                }
                defaultPositionButtonWasPressed = false;
            }
            controllView.buttonDefaultPositionPressed(pressed);
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

        /// <summary>
        /// Callback při zavření celé aplikace
        /// </summary>
        private void closeApplication() {
            robot.disable();
        }

        /// <summary>
        /// Callback při stisknutí tačítka pro absolutní pozicování robota
        /// </summary>
        /// <param name="absoluteControllMode">true, pokud zobrazit absolutní pozicování</param>
        private void buttonForChangeControllModePressed(bool absoluteControllMode) {
            mainWindow.changeControllMode(absoluteControllMode);
            robot.changeControllMode(absoluteControllMode);
        }

        /// <summary>
        /// Callback při stisknutí tačítka pro otočení daného motoru v daném směru o daný krok
        /// </summary>
        /// <param name="motorId">id motoru</param>
        /// <param name="step">krok motoru v qc</param>
        private void buttonForAbsoluteMoveClicked(MotorId motorId, int step)
        {
            robot.moveWithMotor(motorId, step);
        }

        /// <summary>
        /// Callback při stisknutí tačítka pro rekalibraci
        /// </summary>
        private void buttonForRecalibrClicked()
        {
            prepareRecalibration();       
        }

        /// <summary>
        /// Callback při stisknutí tačítka pro kalibraci
        /// </summary>
        private void buttonForCalibrClicked()
        {
            mainWindow.changeDiagnosticView(true);
            absoluteControllView.stopRecalibr();
            robot.homing();
        }

        /// <summary>
        /// Příprava na rekalibraci robota
        /// </summary>
        private void prepareRecalibration() {
            mainWindow.changeControllMode(true);
            mainWindow.changeDiagnosticView(false);
            absoluteControllView.startRecalibr();
        }

        /// <summary>
        /// Callback při stisknutí tačítka pro nastavení současného stavu jako výchozího
        /// </summary>
        private void buttonForSetDefaultStateClicked()
        {
            robot.setCurrentPositionAsDefault();
        }

        /// <summary>
        /// Otestuje, zda jsou uloženy nulové pozice pro všechny motory a pokud ne, tak je uživatel požádán o rekalibraci robota
        /// </summary>
        private void checkHoming(){
            if (!robot.reHoming()){
                DialogResult dialogResult = MessageBox.Show("Bylo zjištěno, že v tomto počítači ještě nejsou uloženy referenční hodnoty motorů nebo došlo k jejich ztrátě při náhlém vypnutí robota. Je potřeba provést rekalibraci. Rekalibrace se provádí tak, že uživatel nastaví všechny motory do nulové polohy a následně jsou tyto polohy brány ve všech výpočtech jako referenční. Pokud uživatel nastaví hodnoty špatně, může dojít k poškození robota.", "Rekalibrace", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.OK)
                {
                    prepareRecalibration();
                }
            }
        }
    }
}
