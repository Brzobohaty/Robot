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
    abstract class AbstractRobot : IRobot
    {
        /// <summary>
        /// Inicializace připojení k motorům
        /// </summary>
        /// <param name="motorStateObserver">posluchač stavu motoru</param>
        /// <returns>chybovou hlášku nebo prázdný řetězec pokud nenastala chyba</returns>
        public abstract string inicialize(Action<string, string, string> motorStateObserver);

        /// <summary>
        /// Pohne s robotem v daném směru a danou rychlostí
        /// </summary>
        /// <param name="direction">směr pohybu v úhlech -180 až 180</param>
        /// <param name="speed">rychlost pohybu od -100 do 100</param>
        public abstract void move(int direction, int speed);

        /// <summary>
        /// Sníží robota
        /// </summary>
        public abstract void moveDown();

        /// <summary>
        /// Zvýší robota
        /// </summary>
        public abstract void moveUp();

        /// <summary>
        /// Rozšíří robota
        /// </summary>
        public abstract void widen();

        /// <summary>
        /// Zůží robota
        /// </summary>
        public abstract void narrow();

        /// <summary>
        /// Nastaví robota do defaultní pozice
        /// </summary>
        public abstract void setDefaultPosition();

        /// <summary>
        /// Vypne robota
        /// </summary>
        public abstract void disable();
    }
}
