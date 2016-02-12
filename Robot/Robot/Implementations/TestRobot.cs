using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EposCmd.Net;

namespace Robot.Robot.Implementations
{
    /// <summary>
    /// Objekt představující testovací softwarovou simulaci robota
    /// </summary>
    class TestRobot : AbstractRobot
    {
        private IMotor frontLeftWheel;

        /// <summary>
        /// Inicializace připojení k motorům
        /// </summary>
        /// <param name="motorStateObserver">posluchač stavu motoru</param>
        /// <returns>chybovou hlášku nebo prázdný řetězec pokud nenastala chyba</returns>
        public override string inicialize(Action<MotorState, string, MotorId, int> motorStateObserver)
        {
            return "";
            frontLeftWheel = new TestMotor();
        }

        /// <summary>
        /// Pohne s robotem v daném směru a danou rychlostí
        /// </summary>
        /// <param name="direction">směr pohybu v úhlech -180 až 180</param>
        /// <param name="speed">rychlost pohybu od -100 do 100</param>
        public override void move(int direction, int speed)
        {
            Console.WriteLine("Move to direction: "+direction+" with speed: "+speed);
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
        /// <param name="direction">směr točení [-1,1]</param>
        /// <param name="step">krok motoru v qc</param>
        public override void moveWithMotor(MotorId motorId, int step)
        {

        }

        /// <summary>
        /// Vypne motor
        /// </summary>
        public override void disable()
        {
        }

        /// <summary>
        /// Změní ovládání robota (absolutní nebo joystikem)
        /// </summary>
        public override void changeControllMode()
        {
            
        }
    }
}
