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
        /// <param name="motorStateObserver">posluchač stavu motoru</param>
        /// <returns>chybovou hlášku nebo prázdný řetězec pokud nenastala chyba</returns>
        string inicialize(Action<MotorState, string, MotorId, int> motorStateObserver);

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

        /// <summary>
        /// Vypne robota
        /// </summary>
        void disable();

        /// <summary>
        /// Pohne s daným motorem v daném směru o daný krok
        /// </summary>
        /// <param name="motorId">id motoru</param>
        /// <param name="step">krok motoru v qc</param>
        void moveWithMotor(MotorId motorId, int step);

        /// <summary>
        /// Změní ovládání robota (absolutní nebo joystikem)
        /// </summary>
        void changeControllMode();
    }
}
