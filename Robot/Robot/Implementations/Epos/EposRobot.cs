using System;
using System.Collections.Generic;
using EposCmd.Net;
using EposCmd.Net.DeviceCmdSet.Initialization;
using System.Threading;
using Robot.Robot.Implementations.Test;

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
        public string inicialize(StateObserver motorStateObserver, bool withChooseOfBus, Action motorErrorOccuredObserver)
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
            catch (DeviceException e)
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
            moveWithWheelInDirection(motors[MotorId.LP_ZK], motors[MotorId.LP_R], motors[MotorId.LP_P], direction, speed, 360-90);
            moveWithWheelInDirection(motors[MotorId.PP_ZK], motors[MotorId.PP_R], motors[MotorId.PP_P], 180 - direction, speed, -90);
            moveWithWheelInDirection(motors[MotorId.LZ_ZK], motors[MotorId.LZ_R], motors[MotorId.LZ_P], 180 - direction, speed, -90);
            moveWithWheelInDirection(motors[MotorId.PZ_ZK], motors[MotorId.PZ_R], motors[MotorId.PZ_P], direction, speed, 360 - 90);

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
        /// Bude pohybovat kolem v daném směru a s danou rychlostí
        /// </summary>
        /// <param name="motorZK">motor pro otáčení nohy kola</param>
        /// <param name="motorR">motor pro otáčení kola</param>
        /// <param name="motorP">motor pro pohon kola</param>
        /// <param name="direction">směrm kterým se má pohybovat 0 až 359</param>
        /// <param name="speed">rychlost jakou se má pohybovat -100 až 100</param>
        /// <param name="angleCorection">korekce úhlu nohy...</param>
        private void moveWithWheelInDirection(IMotor motorZK, IMotor motorR, IMotor motorP, int direction, int speed, int angleCorection)
        {
            bool reverseWheelSpeed = rotateWheelForMove(direction, motorZK, motorR, angleCorection);
            int rev = 1;
            if (reverseWheelSpeed)
            {
                rev = -1;
            }
            motorP.moving(speed * rev);
        }

        /// <summary>
        /// Otočí kolo nohy ve směru pohybu
        /// </summary>
        /// <param name="direction">směr ve stupních 0 až 359</param>
        /// <param name="motorZK">motor pro otáčení nohy</param>
        /// <param name="motorR">motor pro otáčení kola nohy</param>
        /// <param name="angleCorection">korekce úhlu nohy...</param>
        /// <returns>true pokud obrátit směr pohybu kola</returns>
        private bool rotateWheelForMove(int direction, IMotor motorZK, IMotor motorR, int angleCorection)
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
            else {
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
            motorR.moveToAngle(wheelAngle);
            return reverseWheelSpeed;
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
            narrowWiden(1);
        }

        /// <summary>
        /// Zůží robota
        /// </summary>
        public void narrow()
        {
            narrowWiden(-1);
        }

        /// <summary>
        /// Nastaví robota do defaultní pozice
        /// </summary>
        public void setDefaultPosition()
        {
            motors[MotorId.PP_P].disable();
            motors[MotorId.LP_P].disable();
            motors[MotorId.LZ_P].disable();
            motors[MotorId.PZ_P].disable();
            foreach (KeyValuePair<MotorId, IMotor> motor in motors)
            {
                motor.Value.setDefaultPosition();
            }
            motors[MotorId.PP_P].enable();
            motors[MotorId.LP_P].enable();
            motors[MotorId.LZ_P].enable();
            motors[MotorId.PZ_P].enable();
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
            foreach (KeyValuePair<MotorId, IMotor> motor in motors)
            {
                motor.Value.setCurrentPositionAsDefault();
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
            foreach (KeyValuePair<MotorId, IMotor> motor in motors)
            {
                motor.Value.halt();
            }
        }

        /// <summary>
        /// Inicializuje softwarovou simulaci robota
        /// </summary>
        /// <param name="motorStateObserver">posluchač stavu motoru</param>
        /// <param name="motorErrorOccuredObserver">posluchač jakéhokoli eroru motoru</param>
        private void inicializeSimulation(StateObserver motorStateObserver, Action motorErrorOccuredObserver)
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
        private void inicializeMotors(StateObserver motorStateObserver, Action motorErrorOccuredObserver)
        {
            motors[MotorId.PP_P].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 4, MotorId.PP_P, MotorMode.velocity, true, 1, 5000, 2000, 2000);
            motors[MotorId.LP_P].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 8, MotorId.LP_P, MotorMode.velocity, false, 1, 5000, 2000, 2000);
            motors[MotorId.LZ_P].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 12, MotorId.LZ_P, MotorMode.velocity, false, 1, 5000, 2000, 2000);
            motors[MotorId.PZ_P].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 16, MotorId.PZ_P, MotorMode.velocity, true, 1, 5000, 2000, 2000);
            motors[MotorId.PP_R].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 3, MotorId.PP_R, MotorMode.position, false, 4, 4000, 4000, 4000, -241562, 216502, -90, 90);
            motors[MotorId.LP_R].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 7, MotorId.LP_R, MotorMode.position, false, 4, 4000, 4000, 4000, -241562, 216502, -90, 90);
            motors[MotorId.LZ_R].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 11, MotorId.LZ_R, MotorMode.position, false, 4, 4000, 4000, 4000, -241562, 216502, -90, 90);
            motors[MotorId.PZ_R].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 15, MotorId.PZ_R, MotorMode.position, false, 4, 4000, 4000, 4000, -241562, 216502, -90, 90);
            motors[MotorId.PP_Z].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 2, MotorId.PP_Z, MotorMode.position, false, 4, 1000, 2000, 2000, -160000, 158000, -40, 40);
            motors[MotorId.LP_Z].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 6, MotorId.LP_Z, MotorMode.position, false, 4, 1000, 2000, 2000, -160000, 158000, -40, 40);
            motors[MotorId.LZ_Z].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 10, MotorId.LZ_Z, MotorMode.position, false, 4, 1000, 2000, 2000, -160000, 158000, -40, 40);
            motors[MotorId.PZ_Z].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 14, MotorId.PZ_Z, MotorMode.position, false, 4, 1000, 2000, 2000, -160000, 158000, -40, 40);
            motors[MotorId.PP_ZK].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 1, MotorId.PP_ZK, MotorMode.position, false, 4, 5000, 2500, 2500, -110000, 108000, -45, 45);
            motors[MotorId.LP_ZK].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 5, MotorId.LP_ZK, MotorMode.position, false, 4, 5000, 2500, 2500, -110000, 108000, -45, 45);
            motors[MotorId.LZ_ZK].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 9, MotorId.LZ_ZK, MotorMode.position, false, 4, 5000, 2500, 2500, -110000, 108000, -45, 45);
            motors[MotorId.PZ_ZK].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 13, MotorId.PZ_ZK, MotorMode.position, false, 4, 5000, 2500, 2500, -110000, 108000, -45, 45);
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
            motors[MotorId.PP_R].moveToPosition(-241500);
            motors[MotorId.LP_R].moveToPosition(-241500);
            motors[MotorId.LZ_R].moveToPosition(-241500);
            motors[MotorId.PZ_R].moveToPosition(-241500);
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