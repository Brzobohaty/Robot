using System;
using System.Collections.Generic;
using EposCmd.Net;

namespace Robot.Robot.Implementations.Epos
{
    /// <summary>
    /// Objekt představující abstrakci samotného robota
    /// </summary>
    class EposRobot : IRobot
    {
        private DeviceManager connector; // handler pro přopojení motorů
        private Dictionary<MotorId, EposMotor> motors = new Dictionary<MotorId, EposMotor>(); //mapa motorů
        private EposErrorCode errorDictionary; //slovník pro překlad z error kódů do zpráv

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
        /// <returns>chybovou hlášku nebo prázdný řetězec pokud nenastala chyba</returns>
        public string inicialize(StateObserver motorStateObserver)
        {
            try
            {
                connector = new DeviceManager("EPOS2", "CANopen", "IXXAT_USB-to-CAN compact 0", "CAN0");
                motors[MotorId.PP_P].inicialize(connector, motorStateObserver, 4, MotorId.PP_P, MotorMode.velocity, true, 1);
                motors[MotorId.LP_P].inicialize(connector, motorStateObserver, 8, MotorId.LP_P, MotorMode.velocity, false, 1);
                motors[MotorId.LZ_P].inicialize(connector, motorStateObserver, 12, MotorId.LZ_P, MotorMode.velocity, false, 1);
                motors[MotorId.PZ_P].inicialize(connector, motorStateObserver, 16, MotorId.PZ_P, MotorMode.velocity, true, 1);
                motors[MotorId.PP_R].inicialize(connector, motorStateObserver, 3, MotorId.PP_R, MotorMode.position, false, 4, -3000, 3000);
                motors[MotorId.LP_R].inicialize(connector, motorStateObserver, 7, MotorId.LP_R, MotorMode.position, false, 4, -3000, 3000);
                motors[MotorId.LZ_R].inicialize(connector, motorStateObserver, 11, MotorId.LZ_R, MotorMode.position, false, 4, -3000, 3000);
                motors[MotorId.PZ_R].inicialize(connector, motorStateObserver, 15, MotorId.PZ_R, MotorMode.position, false, 4, -3000, 3000);
                motors[MotorId.PP_Z].inicialize(connector, motorStateObserver, 2, MotorId.PP_Z, MotorMode.position, false, 4, -3000, 3000);
                motors[MotorId.LP_Z].inicialize(connector, motorStateObserver, 6, MotorId.LP_Z, MotorMode.position, false, 4, -3000, 3000);
                motors[MotorId.LZ_Z].inicialize(connector, motorStateObserver, 10, MotorId.LZ_Z, MotorMode.position, false, 4, -3000, 3000);
                motors[MotorId.PZ_Z].inicialize(connector, motorStateObserver, 14, MotorId.PZ_Z, MotorMode.position, false, 4, -3000, 3000);
                motors[MotorId.PP_ZK].inicialize(connector, motorStateObserver, 1, MotorId.PP_ZK, MotorMode.position, false, 4, -3000, 3000);
                motors[MotorId.LP_ZK].inicialize(connector, motorStateObserver, 5, MotorId.LP_ZK, MotorMode.position, false, 4, -3000, 3000);
                motors[MotorId.LZ_ZK].inicialize(connector, motorStateObserver, 9, MotorId.LZ_ZK, MotorMode.position, false, 4, -3000, 3000);
                motors[MotorId.PZ_ZK].inicialize(connector, motorStateObserver, 13, MotorId.PZ_ZK, MotorMode.position, false, 4, -3000, 3000);

                foreach (KeyValuePair<MotorId, EposMotor> motor in motors)
                {
                    motor.Value.enableStateObserver();
                }

                return "";
            }
            catch (DeviceException e)
            {
                disable();
                return String.Format("{0}\nError: {1}", e.ErrorMessage, errorDictionary.getErrorMessage(e.ErrorCode));
            }
            catch (Exception e)
            {
                disable();
                return e.Message;
            }
        }

        /// <summary>
        /// Pohne s robotem v daném směru a danou rychlostí
        /// </summary>
        /// <param name="direction">směr pohybu v úhlech -180 až 180</param>
        /// <param name="speed">rychlost pohybu od -100 do 100</param>
        public void move(int direction, int speed)
        {
            //Console.WriteLine("Move to direction: "+direction+" with speed: "+speed);
            if (Math.Abs(direction) > 90)
            {
                speed = -speed;
            }
            motors[MotorId.PP_P].moving(speed);
            motors[MotorId.LP_P].moving(speed);
            motors[MotorId.LZ_P].moving(speed);
            motors[MotorId.PZ_P].moving(speed);
        }

        /// <summary>
        /// Sníží robota
        /// </summary>
        public void moveDown()
        {
            Console.WriteLine("Move down");
        }

        /// <summary>
        /// Zvýší robota
        /// </summary>
        public void moveUp()
        {
            Console.WriteLine("Move up");
        }

        /// <summary>
        /// Rozšíří robota
        /// </summary>
        public void widen()
        {
            Console.WriteLine("Rozšířit");
        }

        /// <summary>
        /// Zůží robota
        /// </summary>
        public void narrow()
        {
            Console.WriteLine("Zůžit");
        }

        /// <summary>
        /// Nastaví robota do defaultní pozice
        /// </summary>
        public void setDefaultPosition()
        {
            foreach (KeyValuePair<MotorId, EposMotor> motor in motors)
            {
                motor.Value.setDefaultPosition();
            }
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
        public void disable()
        {
            if (connector != null)
            {
                saveCurrentPositions();
            }

            foreach (KeyValuePair<MotorId, EposMotor> motor in motors)
            {
                motor.Value.disableStateObserver();
            }

            foreach (KeyValuePair<MotorId, EposMotor> motor in motors)
            {
                motor.Value.disable();
            }

            if (connector != null)
            {
                connector.Dispose();
            }
        }

        /// <summary>
        /// Změní ovládání robota (absolutní nebo joystikem)
        /// </summary>
        /// <param name="absoluteControllMode">true, pokud zobrazit absolutní pozicování</param>
        public void changeControllMode(bool absoluteControllMode)
        {
            if (absoluteControllMode)
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
            foreach (KeyValuePair<MotorId, EposMotor> motor in motors)
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
            if (Properties.Settings.Default.correctlyEnded)
            {
                Properties.Settings.Default.correctlyEnded = false;
                foreach (KeyValuePair<MotorId, EposMotor> motor in motors)
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

        /// <summary>
        /// Nastaví aktuální stav robota jako defaultní
        /// </summary>
        public void setCurrentPositionAsDefault()
        {
            foreach (KeyValuePair<MotorId, EposMotor> motor in motors)
            {
                motor.Value.setCurrentPositionAsDefault();
            }
        }

        /// <summary>
        /// Uloží současné pozice motorů
        /// </summary>
        private void saveCurrentPositions()
        {
            bool allMotorsOk = true;
            foreach (KeyValuePair<MotorId, EposMotor> motor in motors)
            {
                try {
                    Properties.Settings.Default[motor.Key.ToString()] = motor.Value.getPosition();
                }
                catch (DeviceException) {
                    allMotorsOk = false;
                }
            }
            Properties.Settings.Default.correctlyEnded = allMotorsOk;
            Properties.Settings.Default.Save();
        }
    }
}
