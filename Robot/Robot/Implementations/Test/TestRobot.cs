using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EposCmd.Net;

namespace Robot.Robot.Implementations.Test
{
    /// <summary>
    /// Objekt představující testovací softwarovou simulaci robota
    /// </summary>
    class TestRobot : IRobot
    {
        private IMotor frontLeftWheel;

        /// <summary>
        /// Inicializace připojení k motorům
        /// </summary>
        /// <param name="motorStateObserver">posluchač stavu motoru</param>
        /// <returns>chybovou hlášku nebo prázdný řetězec pokud nenastala chyba</returns>
        public string inicialize(StateObserver motorStateObserver)
        {
            frontLeftWheel = new TestMotor();
            return "";
        }

        /// <summary>
        /// Pohne s robotem v daném směru a danou rychlostí
        /// </summary>
        /// <param name="direction">směr pohybu v úhlech -180 až 180</param>
        /// <param name="speed">rychlost pohybu od -100 do 100</param>
        public void move(int direction, int speed)
        {
            Console.WriteLine("Move to direction: "+direction+" with speed: "+speed);
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
            Console.WriteLine("Default position");
        }

        /// <summary>
        /// Pohne s daným motorem v daném směru o daný krok
        /// </summary>
        /// <param name="motorId">id motoru</param>
        /// <param name="direction">směr točení [-1,1]</param>
        /// <param name="step">krok motoru v qc</param>
        public void moveWithMotor(MotorId motorId, int step)
        {

        }

        /// <summary>
        /// Vypne motor
        /// </summary>
        public void disable()
        {
        }

        /// <summary>
        /// Změní ovládání robota (absolutní nebo joystikem)
        /// </summary>
        /// <param name="absoluteControllMode">true, pokud zobrazit absolutní pozicování</param>
        public void changeControllMode(bool absoluteControllMode)
        {
            
        }

        /// <summary>
        /// Nastaví současný stav všech motorů jako nulovou pozici
        /// </summary>
        public void homing()
        {

        }

        /// <summary>
        /// Zkontroluje, zda jsou nastaveny poslední pozice motorů před vypnutím a pokud ano, tak je nahraje do motorů jako současnou pozici
        /// </summary>
        /// <returns>true, pokud se nahrání povedlo</returns>
        public bool reHoming()
        {
            return false;
        }

        /// <summary>
        /// Nastaví aktuální stav robota jako defaultní
        /// </summary>
        public void setCurrentPositionAsDefault()
        {
        }
    }
}
