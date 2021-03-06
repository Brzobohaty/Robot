﻿using Robot.Joystick;
using Robot.Robot;
using Robot.Robot.Implementations.Epos;
using Robot.Views;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
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
        private bool recalibrationInProgrress; //příznak, že probíhý rekalibrace
        private Thread inicializeRobotThread; //vlákno pro inicializaci robota
        private Thread inicializeJoystickThread; //vlákno pro inicializaci joysticku
        private System.Timers.Timer finishInicializationObserver; // posluchač ukončení vláken inicializace
        private System.Timers.Timer gamePadChecker; // kontrololor připojeného externího ovladače

        /// <param name="mainWindow">hlavní okno aplikace</param>
        public Controller(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            controllView = ControllView.getInstance();
            absoluteControllView = AbsoluteControllView.getInstance();
            diagnosticView = DiagnosticView.getInstance();
            mainWindow.subscribeWindowShownObserver(inicialize);
            mainWindow.subscribeWindowCloseObserver(closeApplication);
            mainWindow.subscribeButtonForRecalibrClickObserver(buttonForRecalibrClicked);
            mainWindow.subscribeButtonForConnectionSettingsClickObserver(buttonForConnectionSettingsClicked);
            mainWindow.subscribeButtonForReinicializeClickObserver(buttonForReinicializeClicked);
            controllView.subscribeAbsolutePositioningObserver(buttonForChangeControllModePressed);
            absoluteControllView.subscribeJoystickPositioningObserver(buttonForChangeControllModePressed);
            absoluteControllView.subscribeButtonForAbsoluteMoveClickObserver(buttonForAbsoluteMoveClicked);
            absoluteControllView.subscribeButtonForSetDefaultPositionClickObserver(buttonForSetDefaultStateClicked);
            absoluteControllView.subscribeButtonForCalibrClickObserver(buttonForCalibrClicked);
            absoluteControllView.subscribeButtonForCancelCalibrationClickObserver(buttonForCancelCalibrationClicked);
            absoluteControllView.subscribecheckBoxLimitProtectionObserver(checkBoxLimitProtectionChanged);
            MotorsSettings.getInstance().subscribeMotorsSetingsChanged(refreshMotorsParameters);
            createGamePadChecker();
        }

        /// <summary>
        /// Inicializace připojení k motorům a připojení gamepadu
        /// </summary>
        private void inicialize()
        {
            controllOnOff(false);
            inicializeRobotThread = new Thread(delegate () { inicializeRobot(false); });
            inicializeRobotThread.Start();
            inicializeJoystickThread = new Thread(inicializeJoystick);
            inicializeJoystickThread.Start();

            finishInicializationObserver = new System.Timers.Timer();
            finishInicializationObserver.Elapsed += new ElapsedEventHandler(finishInicialization);
            finishInicializationObserver.Interval = 100;
            finishInicializationObserver.Enabled = true;
        }

        /// <summary>
        /// Dokončení inicializace po tom co bude inicializován robot a joystick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ev"></param>
        private void finishInicialization(object sender, EventArgs ev)
        {
            if (!inicializeRobotThread.IsAlive && !inicializeJoystickThread.IsAlive)
            {
                finishInicializationObserver.Stop();
                controllOnOff(true);
            }
        }

        /// <summary>
        /// Inicializace robota
        /// </summary>
        /// <param name="withChooseOfBus">příznak, zda při inicilizaci nechat uživatele nastavit parametry připojení</param>
        private void inicializeRobot(bool withChooseOfBus)
        {
            diagnosticView.showDisgnosticMessage(MessageTypeEnum.progress, "Probíhá inicializace robota.");
            robot = new EposRobot();
            string errorMessage = robot.inicialize(diagnosticView, withChooseOfBus, motorErrorOccured);
            if (errorMessage.Length > 0)
            {
                diagnosticView.showDisgnosticMessage(MessageTypeEnum.error, errorMessage);
            }
            else
            {
                diagnosticView.showDisgnosticMessage(MessageTypeEnum.success, "Připojení ke sběrnici proběhlo v pořádku.");
                checkHoming();
            }
        }

        /// <summary>
        /// Inicializace joysticku/gamepadu
        /// </summary>
        private void inicializeJoystick()
        {
            controllView.showControlMessage(MessageTypeEnum.progress, "Probíhá inicializace ovládacího zařízení.");
            JoystickBridge joystickBridge = new JoystickBridge();
            joystick = joystickBridge.getJoystick();
            joystick.onOff(false);
            joystick.subscribeDirectMoveStickObserver(joystickDirectMoveChanged);
            joystick.subscribeMoveStickObserver(joystickMoveChanged);
            joystick.subscribeButtonMoveUpObserver(joystickButtonMoveUpChanged);
            joystick.subscribeButtonMoveDownObserver(joystickButtonMoveDownChanged);
            joystick.subscribeButtonNarrowObserver(joystickButtonNarrowChanged);
            joystick.subscribeButtonWidenObserver(joystickButtonWidenChanged);
            joystick.subscribeButtonDefaultPositionObserver(joystickButtonDefaultPositionChanged);
            joystick.subscribeButtonRotateLeftObserver(joystickButtonRotateLeftChanged);
            joystick.subscribeButtonRotateRightObserver(joystickButtonRotateRightChanged);
            joystick.subscribeButtonStopObserver(joystickButtonStopChanged);
            joystick.subscribeButtonTiltBackObserver(joystickButtonTiltBackChanged);
            joystick.subscribeButtonTiltFrontObserver(joystickButtonTiltFrontChanged);
            joystick.subscribeButtonTiltLeftObserver(joystickButtonTiltLeftChanged);
            joystick.subscribeButtonTiltRightObserver(joystickButtonTiltRightChanged);
            joystick.subscribeFrontNarrowObserver(joysticFrontNarrowChanged);
            joystick.subscribeBackNarrowObserver(joystickBackNarrowChanged);
            joystick.subscribeErrorObserver(refindJoystick);
            if (!joystickBridge.success)
            {
                gamePadChecker.Enabled = true;
                controllView.showControlMessage(MessageTypeEnum.error, joystickBridge.message);
            }
            else
            {
                controllView.showControlMessage(MessageTypeEnum.success, joystickBridge.message);
            }
        }

        /// <summary>
        /// Callback pro změnu stavu analogového tlačítka gamepadu pro zůžení předku
        /// </summary>
        /// <param name="measure">míra zůžení od 0 do 100</param>
        private void joysticFrontNarrowChanged(int measure){
            robot.narrowFront(measure);
            if (!(joystick is MainWindow))
            {
                controllView.setFrontNarrowSlider(measure);
            }
        }

        /// <summary>
        /// Callback pro změnu stavu analogového tlačítka gamepadu pro zůžení zadku
        /// </summary>
        /// <param name="measure">míra zůžení od 0 do 100</param>
        private void joystickBackNarrowChanged(int measure)
        {
            robot.narrowBack(measure);
            if (!(joystick is MainWindow))
            {
                controllView.setBackNarrowSlider(measure);
            }
        }

        /// <summary>
        /// Callback pro změnu stavu tlačítka pro nahnutí dopředu 
        /// </summary>
        /// <param name="pressed">příznak stisknutí</param>
        private void joystickButtonTiltFrontChanged(bool pressed) {
            if (pressed)
            {
                robot.tiltFront();
            }
            else
            {
                robot.stopTiltFront();
            }
            controllView.buttonTiltFrontPressed(pressed);
        }

        /// <summary>
        /// Callback pro změnu stavu tlačítka pro nahnutí dozadu 
        /// </summary>
        /// <param name="pressed">příznak stisknutí</param>
        private void joystickButtonTiltBackChanged(bool pressed)
        {
            if (pressed)
            {
                robot.tiltBack();
            }
            else
            {
                robot.stopTiltBack();
            }
            controllView.buttonTiltBackPressed(pressed);
        }

        /// <summary>
        /// Callback pro změnu stavu tlačítka pro nahnutí doleva 
        /// </summary>
        /// <param name="pressed">příznak stisknutí</param>
        private void joystickButtonTiltLeftChanged(bool pressed)
        {
            if (pressed)
            {
                robot.tiltLeft();
            }
            else
            {
                robot.stopTiltLeft();
            }
            controllView.buttonTiltLeftPressed(pressed);
        }

        /// <summary>
        /// Callback pro změnu stavu tlačítka pro nahnutí doprava 
        /// </summary>
        /// <param name="pressed">příznak stisknutí</param>
        private void joystickButtonTiltRightChanged(bool pressed)
        {
            if (pressed)
            {
                robot.tiltRight();
            }
            else
            {
                robot.stopTiltRight();
            }
            controllView.buttonTiltRightPressed(pressed);
        }

        /// <summary>
        /// Callback pro změnu stavu páčky gamepadu pro přímý pohyb
        /// </summary>
        /// <param name="x">x souřadnice joysticku od -100 do 100</param>
        /// <param name="y">y souřadnice joysticku od -100 do 100</param>
        private void joystickDirectMoveChanged(int x, int y)
        {
            Point corectedPoint = MathLibrary.convertPointToCircle(x, y, 0, 0, 101);
            moveRobot(corectedPoint.X, corectedPoint.Y);
            if (!(joystick is MainWindow))
            {
                controllView.moveDirectMoveJoystick(corectedPoint.X, corectedPoint.Y);
            }
        }

        /// <summary>
        /// Callback pro změnu stavu páčky gamepadu pro rádiusový pohyb
        /// </summary>
        /// <param name="x">x souřadnice joysticku od -100 do 100</param>
        /// <param name="y">y souřadnice joysticku od -100 do 100</param>
        private void joystickMoveChanged(int x, int y)
        {
            Point corectedPoint = MathLibrary.convertPointToCircle(x, y, 0, 0, 101);
            moveRobotInRadius(corectedPoint.X, corectedPoint.Y);

            if (!(joystick is MainWindow))
            {
                controllView.moveMoveJoystick(corectedPoint.X, corectedPoint.Y);
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
                robot.moveUp();
            }
            else
            {
                robot.stopMoveUp();
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
                robot.moveDown();
            }
            else
            {
                robot.stopMoveDown();
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
                robot.widen();
            }
            else
            {
                robot.stopWiden();
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
                robot.narrow();
            }
            else
            {
                robot.stopNarrow();
            }
            controllView.buttonNarrowPressed(pressed);
        }

        /// <summary>
        /// Callback pro změnu stavu tlačítka pro rotaci vlevo
        /// </summary>
        /// <param name="pressed">příznak stisknutí</param>
        private void joystickButtonRotateLeftChanged(bool pressed)
        {
            if (pressed)
            {
                robot.rotate(true);
            }
            else
            {
                robot.stopRotate();
            }
            controllView.buttonRotateLeftPressed(pressed);
        }

        /// <summary>
        /// Callback pro změnu stavu tlačítka pro rotaci vpravo
        /// </summary>
        /// <param name="pressed">příznak stisknutí</param>
        private void joystickButtonRotateRightChanged(bool pressed)
        {
            if (pressed)
            {
                robot.rotate(false);
            }
            else
            {
                robot.stopRotate();
            }
            controllView.buttonRotateRightPressed(pressed);
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
                if (defaultPositionButtonWasPressed)
                {
                    robot.setDefaultPosition();
                }
                defaultPositionButtonWasPressed = false;
            }
            controllView.buttonDefaultPositionPressed(pressed);
        }

        /// <summary>
        /// Callback pro změnu stavu tlačítka pro zastavení
        /// </summary>
        /// <param name="pressed">příznak stisknutí</param>
        private void joystickButtonStopChanged(bool pressed)
        {
            if (pressed)
            {
                robot.haltAll();
            }
            controllView.buttonStopPressed(pressed);
        }

        /// <summary>
        /// Pohne s robotem směrem a rychlostí podle dané polohy joysticku
        /// </summary>
        /// <param name="x">x souřadnice od -100 do 100</param>
        /// <param name="y">y souřadnice od -100 do 100</param>
        private void moveRobot(int x, int y)
        {
            int angle = (int)MathLibrary.getAngle(0, 0, 100, 0, x, y);
            robot.move(angle, (int)MathLibrary.getPointsDistance(0, 0, x, y));
        }

        /// <summary>
        /// Pohne s robotem v daném rádiusu
        /// </summary>
        /// <param name="x">x souřadnice joysticku od -100 do 100</param>
        /// <param name="y">y souřadnice joysticku od -100 do 100</param>
        private void moveRobotInRadius(int x, int y)
        {
            double radiusCircleDistance = x;

            if (radiusCircleDistance > 5 || radiusCircleDistance < -5)
            {
                radiusCircleDistance = MathLibrary.changeScaleLog(radiusCircleDistance, 0, Math.Sign(radiusCircleDistance) * 100, Math.Sign(radiusCircleDistance) * 2000, Math.Sign(radiusCircleDistance) * 60);
            }
            else
            {
                radiusCircleDistance = 99999;
            }
            double speed = MathLibrary.getPointsDistance(0, 0, x, y);
            if (y>0){
                speed = -speed;
            }
            robot.moveInRadius(radiusCircleDistance, speed);
        }

        /// <summary>
        /// Callback při zavření celé aplikace
        /// </summary>
        private void closeApplication()
        {
            if (recalibrationInProgrress)
            {
                robot.disable(false);
            }
            else
            {
                robot.disable(true);
            }
        }

        /// <summary>
        /// Callback při stisknutí tačítka pro absolutní pozicování robota
        /// </summary>
        /// <param name="absoluteControllMode">true, pokud zobrazit absolutní pozicování</param>
        private void buttonForChangeControllModePressed(bool absoluteControllMode)
        {
            robot.limitProtectionEnable(true);
            absoluteControllView.limitProtectionEnable(true);
            joystick.onOff(!absoluteControllMode);
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
            prepareRecalibration(true);
        }

        /// <summary>
        /// Callback při stisknutí tačítka pro kalibraci
        /// </summary>
        private void buttonForCalibrClicked()
        {
            recalibrationInProgrress = false;
            diagnosticView.showDisgnosticMessage(MessageTypeEnum.progress, "Probíhá kalibrace motorů ...");
            controllOnOff(false);
            mainWindow.changeDiagnosticView(true);
            absoluteControllView.stopRecalibr();
            robot.homing();
            controllOnOff(true);
            robot.limitProtectionEnable(true);
            diagnosticView.showDisgnosticMessage(MessageTypeEnum.success, "Kalibrace proběhla v pořádku.");
        }

        /// <summary>
        /// Příprava na rekalibraci robota
        /// </summary>
        /// <param name="byUser">true pokud kalibraci vyvolal uživatel</param>
        private void prepareRecalibration(bool byUser)
        {
            recalibrationInProgrress = true;
            joystick.onOff(false);
            mainWindow.changeControllMode(true);
            mainWindow.changeDiagnosticView(false);
            absoluteControllView.startRecalibr(byUser);
            robot.limitProtectionEnable(false);
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
        private void checkHoming()
        {
            if (!robot.reHoming())
            {
                DialogResult dialogResult = MessageBox.Show("Bylo zjištěno, že v tomto počítači ještě nejsou uloženy referenční hodnoty motorů nebo došlo k jejich ztrátě při náhlém vypnutí robota. Je potřeba provést rekalibraci. Rekalibrace se provádí tak, že uživatel nastaví všechny motory do nulové polohy a následně jsou tyto polohy brány ve všech výpočtech jako referenční. Pokud uživatel nastaví hodnoty špatně, může dojít k poškození robota.", "Rekalibrace", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.OK)
                {
                    prepareRecalibration(false);
                }
            }
        }

        /// <summary>
        /// Callback při stisknutí tačítka pro nastavení připojení
        /// </summary>
        private void buttonForConnectionSettingsClicked()
        {
            controllOnOff(false);
            robot.disable(true);
            inicializeRobot(true);
            controllOnOff(true);
        }

        /// <summary>
        /// Callback při stisknutí tačítka pro reinicializaci robota
        /// </summary>
        private void buttonForReinicializeClicked()
        {
            robot.disable(true);
            inicialize();
        }

        /// <summary>
        /// Callback při stisknutí tačítka pro zrušení kalibrace
        /// </summary>
        private void buttonForCancelCalibrationClicked()
        {
            recalibrationInProgrress = false;
            mainWindow.changeDiagnosticView(true);
            absoluteControllView.stopRecalibr();
            robot.limitProtectionEnable(true);
        }

        /// <summary>
        /// Odpojí/připojí všechna ovládání od robota
        /// </summary>
        /// <param name="on">true pokud připojit</param>
        private void controllOnOff(bool on)
        {
            if (recalibrationInProgrress && joystick != null)
            {
                joystick.onOff(false);
            }
            else
            {
                if (joystick != null)
                {
                    joystick.onOff(on);
                }
            }

            controllView.onOff(on);
            absoluteControllView.onOff(on);
        }

        /// <summary>
        /// Změna stavu check boxu pro zanutí´/vypnutí ochrany pro dojezdy
        /// </summary>
        /// <param name="checkedd">příznak zaškrtnutí checkboxu</param>
        private void checkBoxLimitProtectionChanged(bool checkedd)
        {
            robot.limitProtectionEnable(checkedd);
        }

        /// <summary>
        /// Callback, pokud nastala chyba na nějakém motoru
        /// </summary>
        private void motorErrorOccured()
        {
            diagnosticView.showDisgnosticMessage(MessageTypeEnum.error, "Na některých motorech došlo k chybě. Motory byly preventivně vypnuty. Pro pokračování je potřeba reinicializovat obvod (menu - nastavení).");
            robot.disable(true);
        }

        /// <summary>
        /// Znovu vyhledá externí ovládací zařízení
        /// </summary>
        private void refindJoystick() {
            controllOnOff(false);
            gamePadChecker.Enabled = false;
            if (joystick != null)
            {
                joystick.unsibscribeAllObservers();
                joystick.onOff(true);
            }
            inicializeJoystick();

            controllOnOff(true);
        }

        /// <summary>
        /// Vytvoření periodického kontrolora připojeného gamepadu
        /// </summary>
        private void createGamePadChecker()
        {
            gamePadChecker = new System.Timers.Timer();
            gamePadChecker.Elapsed += delegate { refindJoystick(); };
            gamePadChecker.Interval = 10000;
            gamePadChecker.Enabled = false;
        }

        /// <summary>
        /// Nastaví motorům prametry podle aktuálně nastavených hodnot v settings
        /// </summary>
        private void refreshMotorsParameters() {
            robot.refreshMototrsParameters();
        }
    }
}
