using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EposCmd.Net;
using EposCmd.Net.DeviceCmdSet.Operation;
using Robot.Robot;
using Robot.Robot.Implementations;
using System.Timers;
using System.Collections.Generic;

namespace Robot
{
    /// <summary>
    /// Objekt představující abstrakci samotného robota
    /// </summary>
    class EposRobot : AbstractRobot
    {
        private DeviceManager connector; // handler pro přopojení motorů
        private bool absoluteControllMode = false; //příznak, zda se bude robot ovládat absolutně
        private Dictionary<MotorId, EposMotor> motors = new Dictionary<MotorId, EposMotor>(); //mapa motorů

        public EposRobot() {
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
        public override string inicialize(Action<MotorState, string, MotorId, int> motorStateObserver)
        {
            try
            {
                connector = new DeviceManager("EPOS2", "CANopen", "IXXAT_USB-to-CAN compact 0", "CAN0");
                motors[MotorId.PP_P].inicialize(connector, motorStateObserver, 4, MotorId.PP_P, "velocity", true, 1);
                motors[MotorId.LP_P].inicialize(connector, motorStateObserver, 8, MotorId.LP_P, "velocity", false, 1);
                motors[MotorId.LZ_P].inicialize(connector, motorStateObserver, 12, MotorId.LZ_P, "velocity", false, 1);
                motors[MotorId.PZ_P].inicialize(connector, motorStateObserver, 16, MotorId.PZ_P, "velocity", true, 1);
                motors[MotorId.PP_R].inicialize(connector, motorStateObserver, 3, MotorId.PP_R, "position", false, 4);
                motors[MotorId.LP_R].inicialize(connector, motorStateObserver, 7, MotorId.LP_R, "position", false, 4);
                motors[MotorId.LZ_R].inicialize(connector, motorStateObserver, 11, MotorId.LZ_R, "position", false, 4);
                motors[MotorId.PZ_R].inicialize(connector, motorStateObserver, 15, MotorId.PZ_R, "position", false, 4);
                motors[MotorId.PP_Z].inicialize(connector, motorStateObserver, 2, MotorId.PP_Z, "position", false, 4);
                motors[MotorId.LP_Z].inicialize(connector, motorStateObserver, 6, MotorId.LP_Z, "position", false, 4);
                motors[MotorId.LZ_Z].inicialize(connector, motorStateObserver, 10, MotorId.LZ_Z, "position", false, 4);
                motors[MotorId.PZ_Z].inicialize(connector, motorStateObserver, 14, MotorId.PZ_Z, "position", false, 4);
                motors[MotorId.PP_ZK].inicialize(connector, motorStateObserver, 1, MotorId.PP_ZK, "position", false, 4);
                motors[MotorId.LP_ZK].inicialize(connector, motorStateObserver, 5, MotorId.LP_ZK, "position", false, 4);
                motors[MotorId.LZ_ZK].inicialize(connector, motorStateObserver, 9, MotorId.LZ_ZK, "position", false, 4);
                motors[MotorId.PZ_ZK].inicialize(connector, motorStateObserver, 13, MotorId.PZ_ZK, "position", false, 4);

                foreach (KeyValuePair<MotorId, EposMotor> motor in motors)
                {
                    motor.Value.enableStateObserver();
                }

                return "";
            }
            catch (DeviceException e)
            {
                disable();
                return String.Format("{0}\nErrorCode: {1:X8}", e.ErrorMessage, e.ErrorCode);
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
        public override void move(int direction, int speed)
        {
            //Console.WriteLine("Move to direction: "+direction+" with speed: "+speed);
            if (Math.Abs(direction) > 90) {
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
        public override void moveDown()
        {
            Console.WriteLine("Move down");
        }

        /// <summary>
        /// Zvýší robota
        /// </summary>
        public override void moveUp()
        {
            Console.WriteLine("Move up");
        }

        /// <summary>
        /// Rozšíří robota
        /// </summary>
        public override void widen()
        {
            Console.WriteLine("Rozšířit");
        }

        /// <summary>
        /// Zůží robota
        /// </summary>
        public override void narrow()
        {
            Console.WriteLine("Zůžit");
        }

        /// <summary>
        /// Nastaví robota do defaultní pozice
        /// </summary>
        public override void setDefaultPosition()
        {
            Console.WriteLine("Default position");
        }

        /// <summary>
        /// Pohne s daným motorem v daném směru o daný krok
        /// </summary>
        /// <param name="motorId">id motoru</param>
        /// <param name="step">krok motoru v qc</param>
        public override void moveWithMotor(MotorId motorId, int step) {
            motors[motorId].move(step);
        }

        /// <summary>
        /// Vypne celého robota
        /// </summary>
        public override void disable() {
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
        public override void changeControllMode() {
            if (absoluteControllMode) {
                absoluteControllMode = false;
                motors[MotorId.PP_P].changeMode("velocity");
                motors[MotorId.LP_P].changeMode("velocity");
                motors[MotorId.LZ_P].changeMode("velocity");
                motors[MotorId.PZ_P].changeMode("velocity");
            }
            else {
                absoluteControllMode = true;
                motors[MotorId.PP_P].changeMode("position");
                motors[MotorId.LP_P].changeMode("position");
                motors[MotorId.LZ_P].changeMode("position");
                motors[MotorId.PZ_P].changeMode("position");
            }
        }
    }
}
