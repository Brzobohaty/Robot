using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot
{
    /// <summary>
    /// Objekt představující abstrakci samotného robota
    /// </summary>
    interface IRobot
    {
        /// <summary>
        /// Inicializace připojení k motorům
        /// </summary>
        /// <returns>chybovou hlášku nebo prázdný řetězec pokud nenastala chyba</returns>
        string inicialize();

        /// <summary>
        /// Pohne s robotem v daném směru a danou rychlostí
        /// </summary>
        /// <param name="direction">směr pohybu v úhlech -180 až 180</param>
        /// <param name="speed">rychlost pohybu od -100 do 100</param>
        void move(int direction, int speed);

        /// <summary>
        /// Sníží robota
        /// </summary>
        void moveDown();

        /// <summary>
        /// Zvýší robota
        /// </summary>
        void moveUp();

        /// <summary>
        /// Rozšíří robota
        /// </summary>
        void widen();

        /// <summary>
        /// Zůží robota
        /// </summary>
        void narrow();

        /// <summary>
        /// Nastaví robota do defaultní pozice
        /// </summary>
        void setDefaultPosition();
    }
}
