using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Robot
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
        /// <param name="withChooseOfBus">příznak, zda při inicilizaci nechat uživatele nastavit parametry připojení</param>
        /// <returns>chybovou hlášku nebo prázdný řetězec pokud nenastala chyba</returns>
        string inicialize(StateObserver motorStateObserver, bool withChooseOfBus);

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
        /// <param name="absoluteControllMode">true, pokud zobrazit absolutní pozicování</param>
        void changeControllMode(bool absoluteControllMode);

        /// <summary>
        /// Nastaví aktuální stav robota jako defaultní
        /// </summary>
        void setCurrentPositionAsDefault();

        /// <summary>
        /// Zkontroluje, zda jsou nastaveny poslední pozice motorů před vypnutím a pokud ano, tak je nahraje do motorů jako současnou pozici
        /// </summary>
        /// <returns>true, pokud se nahrání povedlo</returns>
        bool reHoming();

        /// <summary>
        /// Nastaví současný stav všech motorů jako nulovou pozici
        /// </summary>
        void homing();
    }
}
