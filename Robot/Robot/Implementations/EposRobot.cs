using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EposCmd.Net;
using EposCmd.Net.DeviceCmdSet.Operation;
using Robot.Robot;
using Robot.Robot.Implementations;

namespace Robot
{
    /// <summary>
    /// Objekt představující abstrakci samotného robota
    /// </summary>
    class EposRobot : AbstractRobot
    {
        private DeviceManager connector; // handler pro přopojení motorů
        private IMotor movePP = new EposMotor(); //motor pravé přední koule pro pohyb
        private IMotor moveLP = new EposMotor(); //motor levé přední koule pro pohyb
        private IMotor moveLZ = new EposMotor(); //motor levé zadní koule pro pohyb
        private IMotor movePZ = new EposMotor(); //motor pravé zadní koule pro pohyb
        private IMotor rotatePP = new EposMotor(); //motor pro otáčení pravé přední nohy
        private IMotor rotateLP = new EposMotor(); //motor pro otáčení levé přední nohy
        private IMotor rotateLZ = new EposMotor(); //motor pro otáčení levé zadní nohy
        private IMotor rotatePZ = new EposMotor(); //motor pro otáčení pravé zadní nohy
        private IMotor liftPP = new EposMotor(); //motor pro zvedání pravé přední nohy
        private IMotor liftLP = new EposMotor(); //motor pro zvedání levé přední nohy
        private IMotor liftLZ = new EposMotor(); //motor pro zvedání levé zadní nohy
        private IMotor liftPZ = new EposMotor(); //motor pro zvedání pravé zadní nohy
        private IMotor rotateWheelPP = new EposMotor(); //motor pro otáčení kola pravé přední nohy
        private IMotor rotateWheelLP = new EposMotor(); //motor pro otáčení kola levé přední nohy
        private IMotor rotateWheelLZ = new EposMotor(); //motor pro otáčení kola levé zadní nohy
        private IMotor rotateWheelPZ = new EposMotor(); //motor pro otáčení kola pravé zadní nohy

        /// <summary>
        /// Inicializace připojení k motorům
        /// </summary>
        /// <param name="motorStateObserver">posluchač stavu motoru</param>
        /// <returns>chybovou hlášku nebo prázdný řetězec pokud nenastala chyba</returns>
        public override string inicialize(Action<string, string, string> motorStateObserver)
        {
            try
            {
                connector = new DeviceManager("EPOS2", "MAXON_RS232", "RS232", "COM3");
                movePP.inicialize(connector, motorStateObserver, 4, "PP_P", "velocity", false);
                moveLP.inicialize(connector, motorStateObserver, 8, "LP_P", "velocity", true);
                moveLZ.inicialize(connector, motorStateObserver, 12, "LZ_P", "velocity", false);
                movePZ.inicialize(connector, motorStateObserver, 16, "PZ_P", "velocity", true);
                rotatePP.inicialize(connector, motorStateObserver, 3, "PP_R", "position", false);
                rotateLP.inicialize(connector, motorStateObserver, 7, "LP_R", "position", false);
                rotateLZ.inicialize(connector, motorStateObserver, 11, "LZ_R", "position", false);
                rotatePZ.inicialize(connector, motorStateObserver, 15, "PZ_R", "position", false);
                liftPP.inicialize(connector, motorStateObserver, 2, "PP_Z", "position", false);
                liftLP.inicialize(connector, motorStateObserver, 6, "LP_Z", "position", false);
                liftLZ.inicialize(connector, motorStateObserver, 10, "LZ_Z", "position", false);
                liftPZ.inicialize(connector, motorStateObserver, 14, "PZ_Z", "position", false);
                rotateWheelPP.inicialize(connector, motorStateObserver, 1, "PP_ZK", "position", false);
                rotateWheelLP.inicialize(connector, motorStateObserver, 5, "LP_ZK", "position", false);
                rotateWheelLZ.inicialize(connector, motorStateObserver, 9, "LZ_ZK", "position", false);
                rotateWheelPZ.inicialize(connector, motorStateObserver, 13, "PZ_ZK", "position", false);
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
            movePP.move(speed);
            moveLP.move(speed);
            moveLZ.move(speed);
            movePZ.move(speed);
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
        /// Vypne celého robota
        /// </summary>
        public override void disable() {
            movePP.disable();
            moveLP.disable();
            moveLZ.disable();
            movePZ.disable();
            rotatePP.disable();
            rotateLP.disable();
            rotateLZ.disable();
            rotatePZ.disable();
            liftPP.disable();
            liftLP.disable();
            liftLZ.disable();
            liftPZ.disable();
            rotateWheelPP.disable();
            rotateWheelLP.disable();
            rotateWheelLZ.disable();
            rotateWheelPZ.disable();
            if (connector != null)
            {
                connector.Dispose();
            }
        }
    }
}
