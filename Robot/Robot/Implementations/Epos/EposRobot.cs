﻿using System;
using System.Collections.Generic;
using EposCmd.Net;
using EposCmd.Net.DeviceCmdSet.Initialization;
using System.Threading;
using Robot.Robot.Implementations.Test;
using System.Windows.Forms;

namespace Robot.Robot.Implementations.Epos
{
    /// <summary>
    /// Objekt představující abstrakci samotného robota
    /// </summary>
    class EposRobot : IRobot
    {
        private DeviceManager connector; // handler pro přopojení motorů
        private Dictionary<MotorId, IMotor> motors = new Dictionary<MotorId, IMotor>(); //mapa motorů
        private EposErrorCode errorDictionary; //slovník pro překlad z error kódů do zpráv
        private Action motorErrorOccuredObserver; //Posluchač chyb motorů
        private System.Timers.Timer periodicChecker; //periodický vyvolávač určitých funkcí
        private bool test = false; //příznak, že se jedná o simulaci robota

        public EposRobot()
        {
            errorDictionary = EposErrorCode.getInstance();
            var motorsIds = MotorId.GetValues(typeof(MotorId));
            foreach (MotorId motorId in motorsIds)
            {
                motors.Add(motorId, new EposMotor());
            }
        }

        /// <summary>
        /// Inicializace připojení k motorům
        /// </summary>
        /// <param name="motorStateObserver">posluchač stavu motoru</param>
        /// <param name="withChooseOfBus">příznak, zda při inicilizaci nechat uživatele nastavit parametry připojení</param>
        /// <param name="motorErrorOccuredObserver">posluchač jakéhokoli eroru motoru</param>
        /// <returns>chybovou hlášku nebo prázdný řetězec pokud nenastala chyba</returns>
        public string inicialize(IStateObserver motorStateObserver, bool withChooseOfBus, Action motorErrorOccuredObserver)
        {
            try
            {
                if (withChooseOfBus)
                {
                    connector = new DeviceManager();
                }
                else
                {
                    connector = new DeviceManager("EPOS2", "CANopen", "IXXAT_USB-to-CAN compact 0", "CAN0");
                }
                inicializeMotors(motorStateObserver, motorErrorOccuredObserver);
            }
            catch (DeviceException)
            {
                inicializeSimulation(motorStateObserver, motorErrorOccuredObserver); //odkomentovat pro softwarovou simulaci (a zakomentovat následující řádek)
                //return String.Format("{0}\nError: {1}", e.ErrorMessage, errorDictionary.getErrorMessage(e.ErrorCode));
            }

            foreach (KeyValuePair<MotorId, IMotor> motor in motors)
            {
                motor.Value.enableStateObserver();
            }

            this.motorErrorOccuredObserver = motorErrorOccuredObserver;
            foreach (KeyValuePair<MotorId, IMotor> motor in motors)
            {
                if (motor.Value.state == MotorState.error)
                {
                    Thread thread = new Thread(motorErrorOccuredObserverFunction);
                    thread.Start();
                    break;
                }
            }

            return "";
        }

        /// <summary>
        /// Pohne s robotem v daném směru a danou rychlostí
        /// </summary>
        /// <param name="direction">směr pohybu v úhlech 0 až 359</param>
        /// <param name="speed">rychlost pohybu od -100 do 100</param>
        public void move(int direction, int speed)
        {
            if (speed == 0)
            {
                haltAll();
                return;
            }
            if (isHeightOk())
            {
                directMove(direction, speed);
            }
            else
            {
                setManipulativHeight();
                createPeriodicChecker();
                periodicChecker.Elapsed += delegate { directMovePeriodic(direction, speed); };
            }


            //if (speed != 0)
            //{
            //    if (Math.Abs(direction) > 90)
            //    {
            //        speed = -speed;
            //        //        //motors[MotorId.LZ_P].disable();
            //        //        //motors[MotorId.PZ_P].disable();
            //        //        //motors[MotorId.PP_P].enable();
            //        //        //motors[MotorId.LP_P].enable();
            //        //        //motors[MotorId.PP_P].moving(speed);
            //        //        //motors[MotorId.LP_P].moving(speed);
            //    }
            //    else
            //    {
            //        //        //motors[MotorId.PP_P].disable();
            //        //        //motors[MotorId.LP_P].disable();
            //        //        //motors[MotorId.LZ_P].enable();
            //        //        //motors[MotorId.PZ_P].enable();
            //        //        //motors[MotorId.LZ_P].moving(speed);
            //        //        //motors[MotorId.PZ_P].moving(speed);
            //    }
            //    motors[MotorId.PP_P].moving(speed);
            //    motors[MotorId.LP_P].moving(speed);
            //    motors[MotorId.LZ_P].moving(speed);
            //    motors[MotorId.PZ_P].moving(speed);
            //}
            //else
            //{
            //    motors[MotorId.PP_P].halt();
            //    motors[MotorId.LP_P].halt();
            //    motors[MotorId.LZ_P].halt();
            //    motors[MotorId.PZ_P].halt();
            //    motors[MotorId.PP_P].enable();
            //    motors[MotorId.LP_P].enable();
            //    motors[MotorId.LZ_P].enable();
            //    motors[MotorId.PZ_P].enable();
            //}

        }

        /// <summary>
        /// Sníží robota
        /// </summary>
        public void moveDown()
        {
            moveDownUp(-1);
        }

        /// <summary>
        /// Zvýší robota
        /// </summary>
        public void moveUp()
        {
            moveDownUp(1);
        }

        /// <summary>
        /// Rozšíří robota
        /// </summary>
        public void widen()
        {
            if (isHeightOk())
            {
                narrowWiden(-1);
            }
            else
            {
                setManipulativHeight();
                createPeriodicChecker();
                periodicChecker.Elapsed += delegate { narrowWiden(-1); };
            }
        }

        /// <summary>
        /// Zůží robota
        /// </summary>
        public void narrow()
        {
            if (isHeightOk())
            {
                narrowWiden(1);
            }
            else
            {
                setManipulativHeight();
                createPeriodicChecker();
                periodicChecker.Elapsed += delegate { narrowWiden(1); };
            }
        }

        /// <summary>
        /// Nastaví robota do defaultní pozice
        /// </summary>
        public void setDefaultPosition()
        {
            if (isHeightOk())
            {
                setDefaultPositionStep1();
            }
            else
            {
                setManipulativHeight();
                createPeriodicChecker();
                periodicChecker.Elapsed += delegate { setDefaultPositionStep1(); };
            }
        }

        /// <summary>
        /// Nahne robota dozadu
        /// </summary>
        public void tiltBack() {
            tiltStep0(tiltBackStep1);   
        }

        /// <summary>
        /// Nahne robota dopředu
        /// </summary>
        public void tiltFront()
        {
            tiltStep0(tiltFrontStep1);
        }

        /// <summary>
        /// Nahne robota doleva
        /// </summary>
        public void tiltLeft()
        {
            tiltStep0(tiltLeftStep1);
        }

        /// <summary>
        /// Nahne robota doprava
        /// </summary>
        public void tiltRight()
        {
            tiltStep0(tiltRightStep1);
        }

        /// <summary>
        /// Zůžit předek a jet dopředu
        /// </summary>
        /// <param name="measure">míra zůžení od 0 do 100</param>
        public void narrowFront(int measure)
        {
            narrowFrontBackStep0(true, measure);
        }

        /// <summary>
        /// Zůžit zadek a jet dopředu
        /// </summary>
        /// <param name="measure">míra zůžení od 0 do 100</param>
        public void narrowBack(int measure)
        {
            narrowFrontBackStep0(false, measure);
        }

        /// <summary>
        /// Pohne s daným motorem v daném směru o daný krok
        /// </summary>
        /// <param name="motorId">id motoru</param>
        /// <param name="step">krok motoru v qc</param>
        public void moveWithMotor(MotorId motorId, int step)
        {
            motors[motorId].move(step);
        }

        /// <summary>
        /// Vypne celého robota
        /// </summary>
        /// <param name="savePosition">příznak, zda uložit pozici</param>
        public void disable(bool savePosition)
        {
            if ((connector != null || test) && savePosition)
            {
                saveCurrentPositions();
            }

            foreach (KeyValuePair<MotorId, IMotor> motor in motors)
            {
                motor.Value.disableStateObserver();
            }

            foreach (KeyValuePair<MotorId, IMotor> motor in motors)
            {
                motor.Value.disable();
            }
        }

        /// <summary>
        /// Změní ovládání robota (absolutní nebo joystikem)
        /// </summary>
        /// <param name="absoluteControllMode">true, pokud zobrazit absolutní pozicování</param>
        public void changeControllMode(bool absoluteControllMode)
        {
            if (!absoluteControllMode)
            {
                motors[MotorId.PP_P].changeMode(MotorMode.velocity);
                motors[MotorId.LP_P].changeMode(MotorMode.velocity);
                motors[MotorId.LZ_P].changeMode(MotorMode.velocity);
                motors[MotorId.PZ_P].changeMode(MotorMode.velocity);
            }
            else
            {
                motors[MotorId.PP_P].changeMode(MotorMode.position);
                motors[MotorId.LP_P].changeMode(MotorMode.position);
                motors[MotorId.LZ_P].changeMode(MotorMode.position);
                motors[MotorId.PZ_P].changeMode(MotorMode.position);
            }
        }

        /// <summary>
        /// Nastaví současný stav všech motorů jako nulovou pozici
        /// </summary>
        public void homing()
        {
            foreach (KeyValuePair<MotorId, IMotor> motor in motors)
            {
                motor.Value.setActualPositionAsHoming();
            }
        }

        /// <summary>
        /// Zkontroluje, zda jsou nastaveny poslední pozice motorů před vypnutím a pokud ano, tak je nahraje do motorů jako současnou pozici
        /// </summary>
        /// <returns>true, pokud se nahrání povedlo</returns>
        public bool reHoming()
        {
            if (allMotorsOK())
            {
                if (Properties.Settings.Default.correctlyEnded)
                {
                    Properties.Settings.Default.correctlyEnded = false;
                    Properties.Settings.Default.Save();
                    foreach (KeyValuePair<MotorId, IMotor> motor in motors)
                    {
                        motor.Value.setHomingPosition((int)Properties.Settings.Default[motor.Key.ToString()]);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Nastaví aktuální stav robota jako defaultní
        /// </summary>
        public void setCurrentPositionAsDefault()
        {
            if (!isHeightOk())
            {
                DialogResult dialogResult = MessageBox.Show("Výška jedné nebo více nohou je nastavena příliš vysoko (nad 40 °) a takovou polohu nelze z manipulačních důvodů nastavit jako výchozí.", "Zakázaná výška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                foreach (KeyValuePair<MotorId, IMotor> motor in motors)
                {
                    motor.Value.setCurrentPositionAsDefault();
                }
            }
        }

        /// <summary>
        /// Vypne/zapne ochranu dojezdů motorů
        /// </summary>
        /// <param name="on">true pokud zapnout</param>
        public void limitProtectionEnable(bool on)
        {
            foreach (KeyValuePair<MotorId, IMotor> motor in motors)
            {
                motor.Value.limitProtectionOnOff(on);
            }
        }

        /// <summary>
        /// Zastaví všechny motory.
        /// </summary>
        public void haltAll()
        {
            if (periodicChecker != null)
            {
                periodicChecker.Dispose();
            }
            foreach (KeyValuePair<MotorId, IMotor> motor in motors)
            {
                motor.Value.halt();
            }
        }

        /// <summary>
        /// Rotuje robota kolem jeho středu
        /// </summary>
        /// <param name="left">příznak, zda rotovat doleva</param>
        public void rotate(bool left)
        {
            if (isHeightOk())
            {
                rotateStep1(left);
            }
            else
            {
                setManipulativHeight();
                createPeriodicChecker();
                periodicChecker.Elapsed += delegate { rotateStep1(left); };
            }
        }

        /// <summary>
        /// Nastaví motorům prametry podle aktuálně nastavených hodnot v settings
        /// </summary>
        public void refreshMototrsParameters()
        {
            motors[MotorId.PP_P].setParameters((uint)Properties.Settings.Default["P_positionVelocity"], (uint)Properties.Settings.Default["P_positionAceleration"], (uint)Properties.Settings.Default["P_positionDeceleration"], (uint)Properties.Settings.Default["P_maxVelocity"], (uint)Properties.Settings.Default["P_aceleration"], (uint)Properties.Settings.Default["P_deceleration"]);
            motors[MotorId.LP_P].setParameters((uint)Properties.Settings.Default["P_positionVelocity"], (uint)Properties.Settings.Default["P_positionAceleration"], (uint)Properties.Settings.Default["P_positionDeceleration"], (uint)Properties.Settings.Default["P_maxVelocity"], (uint)Properties.Settings.Default["P_aceleration"], (uint)Properties.Settings.Default["P_deceleration"]);
            motors[MotorId.LZ_P].setParameters((uint)Properties.Settings.Default["P_positionVelocity"], (uint)Properties.Settings.Default["P_positionAceleration"], (uint)Properties.Settings.Default["P_positionDeceleration"], (uint)Properties.Settings.Default["P_maxVelocity"], (uint)Properties.Settings.Default["P_aceleration"], (uint)Properties.Settings.Default["P_deceleration"]);
            motors[MotorId.PZ_P].setParameters((uint)Properties.Settings.Default["P_positionVelocity"], (uint)Properties.Settings.Default["P_positionAceleration"], (uint)Properties.Settings.Default["P_positionDeceleration"], (uint)Properties.Settings.Default["P_maxVelocity"], (uint)Properties.Settings.Default["P_aceleration"], (uint)Properties.Settings.Default["P_deceleration"]);
            motors[MotorId.PP_R].setParameters((uint)Properties.Settings.Default["R_positionVelocity"], (uint)Properties.Settings.Default["R_positionAceleration"], (uint)Properties.Settings.Default["R_positionDeceleration"], 1, 1, 1);
            motors[MotorId.LP_R].setParameters((uint)Properties.Settings.Default["R_positionVelocity"], (uint)Properties.Settings.Default["R_positionAceleration"], (uint)Properties.Settings.Default["R_positionDeceleration"], 1, 1, 1);
            motors[MotorId.LZ_R].setParameters((uint)Properties.Settings.Default["R_positionVelocity"], (uint)Properties.Settings.Default["R_positionAceleration"], (uint)Properties.Settings.Default["R_positionDeceleration"], 1, 1, 1);
            motors[MotorId.PZ_R].setParameters((uint)Properties.Settings.Default["R_positionVelocity"], (uint)Properties.Settings.Default["R_positionAceleration"], (uint)Properties.Settings.Default["R_positionDeceleration"], 1, 1, 1);
            motors[MotorId.PP_Z].setParameters((uint)Properties.Settings.Default["Z_positionVelocity"], (uint)Properties.Settings.Default["Z_positionAceleration"], (uint)Properties.Settings.Default["Z_positionDeceleration"], 1, 1, 1);
            motors[MotorId.LP_Z].setParameters((uint)Properties.Settings.Default["Z_positionVelocity"], (uint)Properties.Settings.Default["Z_positionAceleration"], (uint)Properties.Settings.Default["Z_positionDeceleration"], 1, 1, 1);
            motors[MotorId.LZ_Z].setParameters((uint)Properties.Settings.Default["Z_positionVelocity"], (uint)Properties.Settings.Default["Z_positionAceleration"], (uint)Properties.Settings.Default["Z_positionDeceleration"], 1, 1, 1);
            motors[MotorId.PZ_Z].setParameters((uint)Properties.Settings.Default["Z_positionVelocity"], (uint)Properties.Settings.Default["Z_positionAceleration"], (uint)Properties.Settings.Default["Z_positionDeceleration"], 1, 1, 1);
            motors[MotorId.PP_ZK].setParameters((uint)Properties.Settings.Default["ZK_positionVelocity"], (uint)Properties.Settings.Default["ZK_positionAceleration"], (uint)Properties.Settings.Default["ZK_positionDeceleration"], 1, 1, 1);
            motors[MotorId.LP_ZK].setParameters((uint)Properties.Settings.Default["ZK_positionVelocity"], (uint)Properties.Settings.Default["ZK_positionAceleration"], (uint)Properties.Settings.Default["ZK_positionDeceleration"], 1, 1, 1);
            motors[MotorId.LZ_ZK].setParameters((uint)Properties.Settings.Default["ZK_positionVelocity"], (uint)Properties.Settings.Default["ZK_positionAceleration"], (uint)Properties.Settings.Default["ZK_positionDeceleration"], 1, 1, 1);
            motors[MotorId.PZ_ZK].setParameters((uint)Properties.Settings.Default["ZK_positionVelocity"], (uint)Properties.Settings.Default["ZK_positionAceleration"], (uint)Properties.Settings.Default["ZK_positionDeceleration"], 1, 1, 1);
        }

        /// <summary>
        /// Zůží předek/zadek robota a pojede pomalu dopředu - krok 0 - nastavení manipulativní výšky
        /// </summary>
        /// <param name="front">příznak, zda se jedná u zůžení předku nebo zadku</param>
        /// <param name="measure">hodnota o kolik zůžit 0 až 100</param>
        private void narrowFrontBackStep0(bool front, int measure) {
            if (isHeightOk())
            {
                narrowFrontBackStep1(front, measure);
            }
            else
            {
                setManipulativHeight();
                createPeriodicChecker();
                periodicChecker.Elapsed += delegate {
                    narrowFrontBackStep1(front, measure);
                };
            }
        }

        /// <summary>
        /// Zůží předek/zadek robota a pojede pomalu dopředu - krok 1 - otočení kol pro otočení nohou
        /// </summary>
        /// <param name="front">příznak, zda se jedná u zůžení předku nebo zadku</param>
        /// <param name="measure">hodnota o kolik zůžit 0 až 100</param>
        private void narrowFrontBackStep1(bool front, int measure) {
            if (motors[MotorId.PP_Z].isTargetReached() && motors[MotorId.LP_Z].isTargetReached() && motors[MotorId.LZ_Z].isTargetReached() && motors[MotorId.PZ_Z].isTargetReached())
            {
                if (periodicChecker != null)
                {
                    periodicChecker.Dispose();
                }

                motors[MotorId.LP_P].disable();
                motors[MotorId.PP_P].disable();
                motors[MotorId.LZ_P].disable();
                motors[MotorId.PZ_P].disable();

                motors[MotorId.LP_R].moveToPosition(0);
                motors[MotorId.PP_R].moveToPosition(0);
                motors[MotorId.LZ_R].moveToPosition(0);
                motors[MotorId.PZ_R].moveToPosition(0);

                createPeriodicChecker();
                periodicChecker.Elapsed += delegate { narrowFrontBackStep2(front, measure); };
            }
        }

        /// <summary>
        /// Zůží předek/zadek robota a pojede pomalu dopředu - krok 2 - otočení nohou
        /// </summary>
        /// <param name="front">příznak, zda se jedná u zůžení předku nebo zadku</param>
        /// <param name="measure">hodnota o kolik zůžit 0 až 100</param>
        private void narrowFrontBackStep2(bool front, int measure)
        {
            if (motors[MotorId.PP_R].isTargetReached() && motors[MotorId.LP_R].isTargetReached() && motors[MotorId.LZ_R].isTargetReached() && motors[MotorId.PZ_R].isTargetReached())
            {
                periodicChecker.Dispose();

                int angle = MathLibrary.changeScale(measure, 0, 100, 0, 45);

                if (front) {
                    motors[MotorId.LP_ZK].moveToAngle(angle);
                    motors[MotorId.PP_ZK].moveToAngle(angle);
                }
                else {
                    motors[MotorId.LZ_ZK].moveToAngle(angle);
                    motors[MotorId.PZ_ZK].moveToAngle(angle);
                }

                createPeriodicChecker();
                periodicChecker.Elapsed += delegate { narrowFrontBackStep3(measure); };
            }
        }

        /// <summary>
        /// Zůží předek/zadek robota a pojede pomalu dopředu - krok 3 - pojezd
        /// </summary>
        /// <param name="measure">hodnota o kolik zůžit 0 až 100</param>
        private void narrowFrontBackStep3(int measure)
        {
            if (motors[MotorId.PP_ZK].isTargetReached() && motors[MotorId.LP_ZK].isTargetReached() && motors[MotorId.LZ_ZK].isTargetReached() && motors[MotorId.PZ_ZK].isTargetReached())
            {
                periodicChecker.Dispose();

                if (measure == 0)
                {
                    haltAll();
                }
                else
                {
                    directMove(90, 30);
                }
            }
        }

        /// <summary>
        /// Nahne robota (step 0 - natočení kol)
        /// </summary>
        /// <param name="tiltStep1">metoda pro nahnutí ve správném směru</param>
        private void tiltStep0(Action tiltStep1)
        {
            motors[MotorId.LP_R].moveToAngle(90);
            motors[MotorId.PP_R].moveToAngle(90);
            motors[MotorId.LZ_R].moveToAngle(90);
            motors[MotorId.PZ_R].moveToAngle(90);

            createPeriodicChecker();
            periodicChecker.Elapsed += delegate { tiltStep1(); };
        }

        /// <summary>
        /// Nahne robota dozadu (step 1 - nastavení výšky nohou)
        /// </summary>
        private void tiltBackStep1()
        {
            if (motors[MotorId.PP_R].isTargetReached() && motors[MotorId.LP_R].isTargetReached() && motors[MotorId.LZ_R].isTargetReached() && motors[MotorId.PZ_R].isTargetReached())
            {
                periodicChecker.Dispose();
                
                motors[MotorId.LZ_Z].move(2000);
                motors[MotorId.PZ_Z].move(2000);
            }
        }

        /// <summary>
        /// Nahne robota dopředu (step 1 - nastavení výšky nohou)
        /// </summary>
        private void tiltFrontStep1()
        {
            if (motors[MotorId.PP_R].isTargetReached() && motors[MotorId.LP_R].isTargetReached() && motors[MotorId.LZ_R].isTargetReached() && motors[MotorId.PZ_R].isTargetReached())
            {
                periodicChecker.Dispose();

                motors[MotorId.PP_Z].move(2000);
                motors[MotorId.LP_Z].move(2000);
            }
        }

        /// <summary>
        /// Nahne robota doleva (step 1 - nastavení výšky nohou)
        /// </summary>
        private void tiltLeftStep1()
        {
            if (motors[MotorId.PP_R].isTargetReached() && motors[MotorId.LP_R].isTargetReached() && motors[MotorId.LZ_R].isTargetReached() && motors[MotorId.PZ_R].isTargetReached())
            {
                periodicChecker.Dispose();
                
                motors[MotorId.LP_Z].move(2000);
                motors[MotorId.LZ_Z].move(2000);
            }
        }

        /// <summary>
        /// Nahne robota doprava (step 1 - nastavení výšky nohou)
        /// </summary>
        private void tiltRightStep1()
        {
            if (motors[MotorId.PP_R].isTargetReached() && motors[MotorId.LP_R].isTargetReached() && motors[MotorId.LZ_R].isTargetReached() && motors[MotorId.PZ_R].isTargetReached())
            {
                periodicChecker.Dispose();

                motors[MotorId.PP_Z].move(2000);
                motors[MotorId.PZ_Z].move(2000);
            }
        }

        /// <summary>
        /// Rotuje robota kolem jeho středu (krok 1 - natočení kol)
        /// </summary>
        /// <param name="left">příznak, zda rotovat doleva</param>
        private void rotateStep1(bool left)
        {
            if (motors[MotorId.PP_Z].isTargetReached() && motors[MotorId.LP_Z].isTargetReached() && motors[MotorId.LZ_Z].isTargetReached() && motors[MotorId.PZ_Z].isTargetReached())
            {
                if (periodicChecker != null)
                {
                    periodicChecker.Dispose();
                }

                motors[MotorId.LP_R].moveToPosition(0);
                motors[MotorId.PP_R].moveToPosition(0);
                motors[MotorId.LZ_R].moveToPosition(0);
                motors[MotorId.PZ_R].moveToPosition(0);

                createPeriodicChecker();
                periodicChecker.Elapsed += delegate { rotateStep2(left); };
            }
        }

        /// <summary>
        /// Rotuje robota kolem jeho středu (krok 2 - pohon kol)
        /// </summary>
        /// <param name="left">příznak, zda rotovat doleva</param>
        private void rotateStep2(bool left)
        {
            if (motors[MotorId.PP_R].isTargetReached() && motors[MotorId.LP_R].isTargetReached() && motors[MotorId.LZ_R].isTargetReached() && motors[MotorId.PZ_R].isTargetReached())
            {
                periodicChecker.Dispose();

                int rev = 1;
                if (left)
                {
                    rev = -1;
                }

                motors[MotorId.LP_P].enable();
                motors[MotorId.PP_P].enable();
                motors[MotorId.LZ_P].enable();
                motors[MotorId.PZ_P].enable();

                motors[MotorId.LP_P].move(4000 * rev);
                motors[MotorId.PP_P].move(4000 * rev);
                motors[MotorId.LZ_P].move(4000 * rev);
                motors[MotorId.PZ_P].move(4000 * rev);
            }
        }

        /// <summary>
        /// Nastaví robota do defaultní pozice (krok 1 - nastavení výšky)
        /// </summary>
        private void setDefaultPositionStep1()
        {
            if (motors[MotorId.PP_Z].isTargetReached() && motors[MotorId.LP_Z].isTargetReached() && motors[MotorId.LZ_Z].isTargetReached() && motors[MotorId.PZ_Z].isTargetReached())
            {
                if (periodicChecker != null)
                {
                    periodicChecker.Dispose();
                }

                motors[MotorId.PP_P].disable();
                motors[MotorId.LP_P].disable();
                motors[MotorId.LZ_P].disable();
                motors[MotorId.PZ_P].disable();

                motors[MotorId.LP_Z].setDefaultPosition();
                motors[MotorId.PP_Z].setDefaultPosition();
                motors[MotorId.LZ_Z].setDefaultPosition();
                motors[MotorId.PZ_Z].setDefaultPosition();

                createPeriodicChecker();
                periodicChecker.Elapsed += delegate { setDefaultPositionStep2(); };
            }
        }

        /// <summary>
        /// Nastaví robota do defaultní pozice (krok 2 - nastavení rotace kol pro pohyb nohou do stran)
        /// </summary>
        private void setDefaultPositionStep2()
        {
            if (motors[MotorId.PP_Z].isTargetReached() && motors[MotorId.LP_Z].isTargetReached() && motors[MotorId.LZ_Z].isTargetReached() && motors[MotorId.PZ_Z].isTargetReached())
            {
                periodicChecker.Dispose();

                motors[MotorId.LP_R].moveToPosition(0);
                motors[MotorId.PP_R].moveToPosition(0);
                motors[MotorId.LZ_R].moveToPosition(0);
                motors[MotorId.PZ_R].moveToPosition(0);

                createPeriodicChecker();
                periodicChecker.Elapsed += delegate { setDefaultPositionStep3(); };
            }
        }

        /// <summary>
        /// Nastaví robota do defaultní pozice (krok 3 - nastavení otočení nohou)
        /// </summary>
        private void setDefaultPositionStep3()
        {
            if (motors[MotorId.PP_R].isTargetReached() && motors[MotorId.LP_R].isTargetReached() && motors[MotorId.LZ_R].isTargetReached() && motors[MotorId.PZ_R].isTargetReached())
            {
                periodicChecker.Dispose();

                motors[MotorId.LP_ZK].setDefaultPosition();
                motors[MotorId.PP_ZK].setDefaultPosition();
                motors[MotorId.LZ_ZK].setDefaultPosition();
                motors[MotorId.PZ_ZK].setDefaultPosition();

                createPeriodicChecker();
                periodicChecker.Elapsed += delegate { setDefaultPositionStep4(); };
            }
        }

        /// <summary>
        /// Nastaví robota do defaultní pozice (krok 4 - nastavení otočení kol)
        /// </summary>
        private void setDefaultPositionStep4()
        {
            if (motors[MotorId.PP_ZK].isTargetReached() && motors[MotorId.LP_ZK].isTargetReached() && motors[MotorId.LZ_ZK].isTargetReached() && motors[MotorId.PZ_ZK].isTargetReached())
            {
                periodicChecker.Dispose();

                motors[MotorId.LP_R].setDefaultPosition();
                motors[MotorId.PP_R].setDefaultPosition();
                motors[MotorId.LZ_R].setDefaultPosition();
                motors[MotorId.PZ_R].setDefaultPosition();

                createPeriodicChecker();
                periodicChecker.Elapsed += delegate { setDefaultPositionStep5(); };
            }
        }

        /// <summary>
        /// Nastaví robota do defaultní pozice (krok 5 - zapnutí pohonu)
        /// </summary>
        private void setDefaultPositionStep5()
        {
            if (motors[MotorId.PP_R].isTargetReached() && motors[MotorId.LP_R].isTargetReached() && motors[MotorId.LZ_R].isTargetReached() && motors[MotorId.PZ_R].isTargetReached())
            {
                periodicChecker.Dispose();

                motors[MotorId.PP_P].enable();
                motors[MotorId.LP_P].enable();
                motors[MotorId.LZ_P].enable();
                motors[MotorId.PZ_P].enable();
            }
        }

        /// <summary>
        /// Nastaví robotoj výšku, v které se s ním  dá libovolně manipulovat.
        /// </summary>
        private void setManipulativHeight()
        {
            motors[MotorId.PP_Z].moveToAngle(10);
            motors[MotorId.LP_Z].moveToAngle(10);
            motors[MotorId.LZ_Z].moveToAngle(10);
            motors[MotorId.PZ_Z].moveToAngle(10);
        }

        /// <summary>
        /// Zjistí, zda je robot ve výšce, v které je možné s ním libovolně manipulovat
        /// </summary>
        /// <returns>true pokud je v manipulační výšce</returns>
        private bool isHeightOk()
        {
            if (motors[MotorId.PP_Z].angle < 10 && motors[MotorId.LP_Z].angle < 10 && motors[MotorId.LZ_Z].angle < 10 && motors[MotorId.PZ_Z].angle < 10)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Přímý pohyb kol krok 0 - kontrola výšky robota
        /// </summary>
        private void directMovePeriodic(int direction, int speed)
        {
            if (motors[MotorId.PP_Z].isTargetReached() && motors[MotorId.LP_Z].isTargetReached() && motors[MotorId.LZ_Z].isTargetReached() && motors[MotorId.PZ_Z].isTargetReached())
            {
                periodicChecker.Dispose();
                directMove(direction, speed);
            }
        }

        /// <summary>
        /// Přímý pohyb kol krok 1 - natočení kol
        /// </summary>
        /// <param name="direction">směr pohybu v úhlech 0 až 359</param>
        /// <param name="speed">rychlost pohybu od -100 do 100</param>
        private void directMove(int direction, int speed)
        {
            int speedLP = setWheelToDirection(motors[MotorId.LP_ZK], motors[MotorId.LP_R], motors[MotorId.LP_P], direction, speed, 360 - 90);
            int speedPP = setWheelToDirection(motors[MotorId.PP_ZK], motors[MotorId.PP_R], motors[MotorId.PP_P], 180 - direction, speed, -90);
            int speedLZ = setWheelToDirection(motors[MotorId.LZ_ZK], motors[MotorId.LZ_R], motors[MotorId.LZ_P], 180 - direction, speed, -90);
            int speedPZ = setWheelToDirection(motors[MotorId.PZ_ZK], motors[MotorId.PZ_R], motors[MotorId.PZ_P], direction, speed, 360 - 90);

            if (!motors[MotorId.LP_P].isDisabled() && !motors[MotorId.PP_P].isDisabled() && !motors[MotorId.LZ_P].isDisabled() && !motors[MotorId.PZ_P].isDisabled())
            {
                motors[MotorId.LP_P].moving(speedLP);
                motors[MotorId.PP_P].moving(speedPP);
                motors[MotorId.LZ_P].moving(speedLZ);
                motors[MotorId.PZ_P].moving(speedPZ);
            }
            else
            {
                createPeriodicChecker();
                periodicChecker.Elapsed += delegate { directMovePeriodic(speedLP, speedPP, speedLZ, speedPZ); };
            }
        }

        /// <summary>
        /// Přímý pohyb kol krok 2 - pohon kol
        /// </summary>
        /// <param name="speedLP">rychlost kola (-100 až 100)</param>
        /// <param name="speedPP">rychlost kola (-100 až 100)</param>
        /// <param name="speedLZ">rychlost kola (-100 až 100)</param>
        /// <param name="speedPZ">rychlost kola (-100 až 100)</param>
        private void directMovePeriodic(int speedLP, int speedPP, int speedLZ, int speedPZ)
        {
            if (motors[MotorId.PP_R].isTargetReached() && motors[MotorId.LP_R].isTargetReached() && motors[MotorId.LZ_R].isTargetReached() && motors[MotorId.PZ_R].isTargetReached())
            {
                periodicChecker.Dispose();

                motors[MotorId.LP_P].enable();
                motors[MotorId.PP_P].enable();
                motors[MotorId.LZ_P].enable();
                motors[MotorId.PZ_P].enable();

                motors[MotorId.LP_P].moving(speedLP);
                motors[MotorId.PP_P].moving(speedPP);
                motors[MotorId.LZ_P].moving(speedLZ);
                motors[MotorId.PZ_P].moving(speedPZ);

                createPeriodicChecker();
                periodicChecker.Elapsed += delegate { enablePAfterZPeriodic(); };
            }
        }

        /// <summary>
        /// Bude pohybovat kolem v daném směru a vrátí rychlost ve správném směru
        /// </summary>
        /// <param name="motorZK">motor pro otáčení nohy kola</param>
        /// <param name="motorR">motor pro otáčení kola</param>
        /// <param name="motorP">motor pro pohon kola</param>
        /// <param name="direction">směrm kterým se má pohybovat 0 až 359</param>
        /// <param name="speed">rychlost jakou se má pohybovat -100 až 100</param>
        /// <param name="angleCorection">korekce úhlu nohy...</param>
        /// <return>rychlost, jakou se má motor pohybovat</return>
        private int setWheelToDirection(IMotor motorZK, IMotor motorR, IMotor motorP, int direction, int speed, int angleCorection)
        {
            bool reverseWheelSpeed = rotateWheelForMove(direction, motorZK, motorR, motorP, angleCorection);
            int rev = 1;
            if (reverseWheelSpeed)
            {
                rev = -1;
            }
            return speed * rev;
        }

        /// <summary>
        /// Otočí kolo nohy ve směru pohybu
        /// </summary>
        /// <param name="direction">směr ve stupních 0 až 359</param>
        /// <param name="motorZK">motor pro otáčení nohy</param>
        /// <param name="motorR">motor pro otáčení kola nohy</param>
        /// <param name="angleCorection">korekce úhlu nohy...</param>
        /// <returns>true pokud obrátit směr pohybu kola</returns>
        private bool rotateWheelForMove(int direction, IMotor motorZK, IMotor motorR, IMotor motorP, int angleCorection)
        {
            bool reverseWheelSpeed = false;
            int kartezLegAngle = MathLibrary.changeScale(motorZK.angle, motorZK.minAngle, motorZK.maxAngle, 0, 90);
            if (angleCorection < 0)
            {
                if (direction - (angleCorection - kartezLegAngle) < 0)
                {
                    reverseWheelSpeed = !reverseWheelSpeed;
                }
            }
            else
            {
                if (direction - (angleCorection - kartezLegAngle) > 0)
                {
                    reverseWheelSpeed = !reverseWheelSpeed;
                }
            }

            int kartezWheelAngle = 180 - 90 - (180 - direction - kartezLegAngle);
            kartezWheelAngle = kartezWheelAngle % 180;
            if (kartezWheelAngle < 0)
            {
                kartezWheelAngle += 180;
                reverseWheelSpeed = !reverseWheelSpeed;
            }
            int wheelAngle = kartezWheelAngle;
            if (kartezWheelAngle > 90)
            {
                reverseWheelSpeed = !reverseWheelSpeed;
                wheelAngle = MathLibrary.changeScale(kartezWheelAngle, 180, 90, 0, motorR.minAngle);
            }
            if (wheelAngle != motorR.angle)
            {
                motorP.disable();
            }
            motorR.moveToAngle(wheelAngle);
            return reverseWheelSpeed;
        }

        /// <summary>
        /// Inicializuje softwarovou simulaci robota
        /// </summary>
        /// <param name="motorStateObserver">posluchač stavu motoru</param>
        /// <param name="motorErrorOccuredObserver">posluchač jakéhokoli eroru motoru</param>
        private void inicializeSimulation(IStateObserver motorStateObserver, Action motorErrorOccuredObserver)
        {
            test = true;
            motors.Clear();
            var motorsIds = MotorId.GetValues(typeof(MotorId));
            foreach (MotorId motorId in motorsIds)
            {
                motors.Add(motorId, new TestMotor());
            }
            inicializeMotors(motorStateObserver, motorErrorOccuredObserver);
        }

        /// <summary>
        /// Inicializace motorů
        /// </summary>
        /// <param name="motorStateObserver">posluchač stavu motoru</param>
        /// <param name="motorErrorOccuredObserver">posluchač jakéhokoli eroru motoru</param>
        private void inicializeMotors(IStateObserver motorStateObserver, Action motorErrorOccuredObserver)
        {
            motors[MotorId.PP_P].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 4, MotorId.PP_P, MotorMode.velocity, true, 1, (uint)Properties.Settings.Default["P_positionVelocity"], (uint)Properties.Settings.Default["P_positionAceleration"], (uint)Properties.Settings.Default["P_positionDeceleration"], (uint)Properties.Settings.Default["P_maxVelocity"], (uint)Properties.Settings.Default["P_aceleration"], (uint)Properties.Settings.Default["P_deceleration"]);
            motors[MotorId.LP_P].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 8, MotorId.LP_P, MotorMode.velocity, false, 1, (uint)Properties.Settings.Default["P_positionVelocity"], (uint)Properties.Settings.Default["P_positionAceleration"], (uint)Properties.Settings.Default["P_positionDeceleration"], (uint)Properties.Settings.Default["P_maxVelocity"], (uint)Properties.Settings.Default["P_aceleration"], (uint)Properties.Settings.Default["P_deceleration"]);
            motors[MotorId.LZ_P].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 12, MotorId.LZ_P, MotorMode.velocity, false, 1, (uint)Properties.Settings.Default["P_positionVelocity"], (uint)Properties.Settings.Default["P_positionAceleration"], (uint)Properties.Settings.Default["P_positionDeceleration"], (uint)Properties.Settings.Default["P_maxVelocity"], (uint)Properties.Settings.Default["P_aceleration"], (uint)Properties.Settings.Default["P_deceleration"]);
            motors[MotorId.PZ_P].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 16, MotorId.PZ_P, MotorMode.velocity, true, 1, (uint)Properties.Settings.Default["P_positionVelocity"], (uint)Properties.Settings.Default["P_positionAceleration"], (uint)Properties.Settings.Default["P_positionDeceleration"], (uint)Properties.Settings.Default["P_maxVelocity"], (uint)Properties.Settings.Default["P_aceleration"], (uint)Properties.Settings.Default["P_deceleration"]);
            motors[MotorId.PP_R].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 3, MotorId.PP_R, MotorMode.position, false, 4, (uint)Properties.Settings.Default["R_positionVelocity"], (uint)Properties.Settings.Default["R_positionAceleration"], (uint)Properties.Settings.Default["R_positionDeceleration"], 1, 1, 1, -241562, 216502, -90, 90);
            motors[MotorId.LP_R].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 7, MotorId.LP_R, MotorMode.position, false, 4, (uint)Properties.Settings.Default["R_positionVelocity"], (uint)Properties.Settings.Default["R_positionAceleration"], (uint)Properties.Settings.Default["R_positionDeceleration"], 1, 1, 1, -241562, 216502, -90, 90);
            motors[MotorId.LZ_R].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 11, MotorId.LZ_R, MotorMode.position, false, 4, (uint)Properties.Settings.Default["R_positionVelocity"], (uint)Properties.Settings.Default["R_positionAceleration"], (uint)Properties.Settings.Default["R_positionDeceleration"], 1, 1, 1, -241562, 216502, -90, 90);
            motors[MotorId.PZ_R].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 15, MotorId.PZ_R, MotorMode.position, false, 4, (uint)Properties.Settings.Default["R_positionVelocity"], (uint)Properties.Settings.Default["R_positionAceleration"], (uint)Properties.Settings.Default["R_positionDeceleration"], 1, 1, 1, -241562, 216502, -90, 90);
            motors[MotorId.PP_Z].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 2, MotorId.PP_Z, MotorMode.position, false, 4, (uint)Properties.Settings.Default["Z_positionVelocity"], (uint)Properties.Settings.Default["Z_positionAceleration"], (uint)Properties.Settings.Default["Z_positionDeceleration"], 1, 1, 1, -160000, 158000, -40, 40);
            motors[MotorId.LP_Z].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 6, MotorId.LP_Z, MotorMode.position, false, 4, (uint)Properties.Settings.Default["Z_positionVelocity"], (uint)Properties.Settings.Default["Z_positionAceleration"], (uint)Properties.Settings.Default["Z_positionDeceleration"], 1, 1, 1, -160000, 158000, -40, 40);
            motors[MotorId.LZ_Z].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 10, MotorId.LZ_Z, MotorMode.position, false, 4, (uint)Properties.Settings.Default["Z_positionVelocity"], (uint)Properties.Settings.Default["Z_positionAceleration"], (uint)Properties.Settings.Default["Z_positionDeceleration"], 1, 1, 1, -160000, 158000, -40, 40);
            motors[MotorId.PZ_Z].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 14, MotorId.PZ_Z, MotorMode.position, false, 4, (uint)Properties.Settings.Default["Z_positionVelocity"], (uint)Properties.Settings.Default["Z_positionAceleration"], (uint)Properties.Settings.Default["Z_positionDeceleration"], 1, 1, 1, -160000, 158000, -40, 40);
            motors[MotorId.PP_ZK].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 1, MotorId.PP_ZK, MotorMode.position, false, 4, (uint)Properties.Settings.Default["ZK_positionVelocity"], (uint)Properties.Settings.Default["ZK_positionAceleration"], (uint)Properties.Settings.Default["ZK_positionDeceleration"], 1, 1, 1, -110000, 108000, -45, 45);
            motors[MotorId.LP_ZK].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 5, MotorId.LP_ZK, MotorMode.position, false, 4, (uint)Properties.Settings.Default["ZK_positionVelocity"], (uint)Properties.Settings.Default["ZK_positionAceleration"], (uint)Properties.Settings.Default["ZK_positionDeceleration"], 1, 1, 1, -110000, 108000, -45, 45);
            motors[MotorId.LZ_ZK].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 9, MotorId.LZ_ZK, MotorMode.position, false, 4, (uint)Properties.Settings.Default["ZK_positionVelocity"], (uint)Properties.Settings.Default["ZK_positionAceleration"], (uint)Properties.Settings.Default["ZK_positionDeceleration"], 1, 1, 1, -110000, 108000, -45, 45);
            motors[MotorId.PZ_ZK].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 13, MotorId.PZ_ZK, MotorMode.position, false, 4, (uint)Properties.Settings.Default["ZK_positionVelocity"], (uint)Properties.Settings.Default["ZK_positionAceleration"], (uint)Properties.Settings.Default["ZK_positionDeceleration"], 1, 1, 1, -110000, 108000, -45, 45);
        }

        /// <summary>
        /// Zvýšit/snížit robota krok 1 : narovnat kola
        /// </summary>
        /// <param name="direction">-1 = down, 1 = up</param>
        private void moveDownUp(int direction)
        {
            motors[MotorId.PP_P].disable();
            motors[MotorId.LP_P].disable();
            motors[MotorId.LZ_P].disable();
            motors[MotorId.PZ_P].disable();
            motors[MotorId.PP_R].moveToAngle(90);
            motors[MotorId.LP_R].moveToAngle(90);
            motors[MotorId.LZ_R].moveToAngle(90);
            motors[MotorId.PZ_R].moveToAngle(90);
            createPeriodicChecker();
            periodicChecker.Elapsed += delegate { moveDownUpPeriodic(direction); };

        }

        /// <summary>
        /// Zvýšit/snížit robota krok 2 : pohnut se Z
        /// </summary>
        /// <param name="direction">-1 = down, 1 = up</param>
        private void moveDownUpPeriodic(int direction)
        {
            if (motors[MotorId.PP_R].isTargetReached() && motors[MotorId.LP_R].isTargetReached() && motors[MotorId.LZ_R].isTargetReached() && motors[MotorId.PZ_R].isTargetReached())
            {
                periodicChecker.Dispose();

                motors[MotorId.PP_Z].move(-2000 * direction);
                motors[MotorId.LP_Z].move(-2000 * direction);
                motors[MotorId.LZ_Z].move(-2000 * direction);
                motors[MotorId.PZ_Z].move(-2000 * direction);

                createPeriodicChecker();
                periodicChecker.Elapsed += delegate { enablePAfterZPeriodic(); };
            }
        }

        /// <summary>
        /// Zvýšit/snížit robota krok 3 : zapnout P
        /// </summary>
        private void enablePAfterZPeriodic()
        {
            if (motors[MotorId.PP_Z].isTargetReached() && motors[MotorId.LP_Z].isTargetReached() && motors[MotorId.LZ_Z].isTargetReached() && motors[MotorId.PZ_Z].isTargetReached())
            {
                periodicChecker.Dispose();
                motors[MotorId.PP_P].enable();
                motors[MotorId.LP_P].enable();
                motors[MotorId.LZ_P].enable();
                motors[MotorId.PZ_P].enable();
            }
        }

        /// <summary>
        /// Rozšíří, zůží robota
        /// </summary>
        /// <param name="direction">-1 = zůžit, 1 = rozšířit</param>
        private void narrowWiden(int direction)
        {
            if (motors[MotorId.PP_Z].isTargetReached() && motors[MotorId.LP_Z].isTargetReached() && motors[MotorId.LZ_Z].isTargetReached() && motors[MotorId.PZ_Z].isTargetReached())
            {
                if (periodicChecker != null)
                {
                    periodicChecker.Dispose();
                }

                motors[MotorId.PP_P].disable();
                motors[MotorId.LP_P].disable();
                motors[MotorId.LZ_P].disable();
                motors[MotorId.PZ_P].disable();
                motors[MotorId.PP_R].moveToPosition(0);
                motors[MotorId.LP_R].moveToPosition(0);
                motors[MotorId.LZ_R].moveToPosition(0);
                motors[MotorId.PZ_R].moveToPosition(0);
                createPeriodicChecker();
                periodicChecker.Elapsed += delegate { narrowWidenPeriodic(direction); };
            }
        }

        /// <summary>
        /// Rozšíří, zůží robota
        /// </summary>
        /// <param name="direction">-1 = zůžit, 1 = rozšířit</param>
        private void narrowWidenPeriodic(int direction)
        {
            if (motors[MotorId.PP_R].isTargetReached() && motors[MotorId.LP_R].isTargetReached() && motors[MotorId.LZ_R].isTargetReached() && motors[MotorId.PZ_R].isTargetReached())
            {
                periodicChecker.Dispose();
                motors[MotorId.PP_ZK].move(2000 * direction);
                motors[MotorId.LP_ZK].move(2000 * direction);
                motors[MotorId.LZ_ZK].move(2000 * direction);
                motors[MotorId.PZ_ZK].move(2000 * direction);
                createPeriodicChecker();
                periodicChecker.Elapsed += delegate { enablePAfterZKPeriodic(); };
            }
        }

        /// <summary>
        /// Zvýšit/snížit robota krok 3 : zapnout P
        /// </summary>
        private void enablePAfterZKPeriodic()
        {
            if (motors[MotorId.PP_ZK].isTargetReached() && motors[MotorId.LP_ZK].isTargetReached() && motors[MotorId.LZ_ZK].isTargetReached() && motors[MotorId.PZ_ZK].isTargetReached())
            {
                periodicChecker.Dispose();
                motors[MotorId.PP_P].enable();
                motors[MotorId.LP_P].enable();
                motors[MotorId.LZ_P].enable();
                motors[MotorId.PZ_P].enable();
            }
        }

        /// <summary>
        /// Uloží současné pozice motorů
        /// </summary>
        private void saveCurrentPositions()
        {
            bool allMotorsOk = true;
            foreach (KeyValuePair<MotorId, IMotor> motor in motors)
            {
                try
                {
                    Properties.Settings.Default[motor.Key.ToString()] = motor.Value.getPosition();
                }
                catch (DeviceException)
                {
                    allMotorsOk = false;
                    break;
                }
            }

            Properties.Settings.Default.correctlyEnded = allMotorsOk;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Zkontroluje, zda jsou všechny motory v pořádku
        /// </summary>
        /// <returns>true pokud josu</returns>
        private bool allMotorsOK()
        {
            foreach (KeyValuePair<MotorId, IMotor> motor in motors)
            {
                if (motor.Value.state == MotorState.error || motor.Value.state == MotorState.disabled)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Nahlášení chyby motoru
        /// </summary>
        private void motorErrorOccuredObserverFunction()
        {
            motorErrorOccuredObserver();
        }

        /// <summary>
        /// Nastaví periodický vyvolávač různých funkcí
        /// </summary>
        private void createPeriodicChecker()
        {
            if (periodicChecker != null)
            {
                periodicChecker.Dispose();
            }
            periodicChecker = new System.Timers.Timer();
            periodicChecker.Interval = 50;
            periodicChecker.Enabled = true;
        }
    }
}