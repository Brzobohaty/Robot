using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EposCmd.Net;

namespace Robot
{
    /// <summary>
    /// Objekt představující abstrakci samotného robota
    /// </summary>
    class EposRobot : IRobot
    {
        private DeviceManager connector; // handler pro přopojení motorů

        /// <summary>
        /// Inicializace připojení k motorům
        /// </summary>
        /// <returns>chybovou hlášku nebo prázdný řetězec pokud nenastala chyba</returns>
        public string inicialize()
        {
            try
            {
                connector = new DeviceManager("EPOS2", "MAXON SERIAL V2", "USB", "USB0");
                return "";
            }
            catch (DeviceException e)
            {
                return String.Format("{0}\nErrorCode: {1:X8}", e.ErrorMessage, e.ErrorCode);
            }
            catch (Exception e)
            {
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
    }
}
