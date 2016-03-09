using EposCmd.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Robot
{
    /// <summary>
    /// Představuje abstrakci motoru
    /// </summary>
    interface IMotor
    {
        MotorState state { get;} //aktuální stav motoru
        int angle { get;} //aktuální úhel natočení motoru
        int minAngle { get; } //minimální úhel natočení motoru
        int maxAngle { get; } //maximální úhel natočení motoru
        int targetPosition { get; } //pozice, které má v současnou chvíli motor dosáhnout
        int targetAngle { get; } //úhel, kterého má v současnou chvíli motor dosáhnout

        /// <summary>
        /// Inicializace motoru
        /// </summary>
        /// <param name="connector">connector sběrnice</param>
        /// <param name="stateObserver">posluchač stavu motoru</param>
        /// <param name="nodeNumber">číslo node</param>
        /// <param name="id">id motoru</param>
        /// <param name="mode">defaultní nastavení módu</param>
        /// <param name="reverse">příznak obrácení směru točení</param>
        /// <param name="multiplier">násobitel otáček v případě, že je motor za převodovkou</param>
        /// <param name="positionVeocity">rychlost motoru v otáčkách při pozicování</param>
        /// <param name="positionAceleration">zrychlení motoru v otáčkách při pozicování</param>
        /// <param name="positionDeceleration">zpomalení motoru v otáčkách při pozicování</param>
        /// <param name="velocity">maximální rychlost motoru při rychlostním řízení</param>
        /// <param name="aceleration">zrychlení motoru při rychlostním řízení</param>
        /// <param name="deceleration">zpomalení motoru při rychlostním řízení</param>
        /// <param name="minPosition">minimální pozice motoru</param>
        /// <param name="maxPosition">maximální pozice motoru</param>
        /// <param name="angleMap">Pole definující vztah úhlu ze škály uvedené u motoru a opravdového úhlu a pozicí motoru. Jeden řádek obsahuje {úhel ze škály ve stupních; opravdový úhel; pozice motoru}</param>
        void inicialize(DeviceManager connector, IStateObserver stateObserver, Action motorErrorOccuredObserver, int nodeNumber, MotorId id, MotorMode mode, bool reverse, int multiplier, uint positionVelocity, uint positionAceleration, uint positionDeceleration, uint velocity, uint aceleration, uint deceleration, int[,] angleMap);

        /// <summary>
        /// Inicializace motoru
        /// </summary>
        /// <param name="connector">connector sběrnice</param>
        /// <param name="stateObserver">posluchač stavu motoru</param>
        /// <param name="nodeNumber">číslo node</param>
        /// <param name="id">id motoru</param>
        /// <param name="mode">defaultní nastavení módu</param>
        /// <param name="reverse">příznak obrácení směru točení</param>
        /// <param name="multiplier">násobitel otáček v případě, že je motor za převodovkou</param>
        /// <param name="positionVeocity">rychlost motoru v otáčkách při pozicování</param>
        /// <param name="positionAceleration">zrychlení motoru v otáčkách při pozicování</param>
        /// <param name="positionDeceleration">zpomalení motoru v otáčkách při pozicování</param>
        /// <param name="velocity">maximální rychlost motoru při rychlostním řízení</param>
        /// <param name="aceleration">zrychlení motoru při rychlostním řízení</param>
        /// <param name="deceleration">zpomalení motoru při rychlostním řízení</param>
        /// <param name="minPosition">minimální pozice motoru</param>
        /// <param name="maxPosition">maximální pozice motoru</param>
        void inicialize(DeviceManager connector, IStateObserver stateObserver, Action motorErrorOccuredObserver, int nodeNumber, MotorId id, MotorMode mode, bool reverse, int multiplier, uint positionVelocity, uint positionAceleration, uint positionDeceleration, uint velocity, uint aceleration, uint deceleration, int minPosition, int maxPosition, int minAngle, int maxAngle);

        /// <summary>
        /// Inicializace motoru
        /// </summary>
        /// <param name="connector">connector sběrnice</param>
        /// <param name="stateObserver">posluchač stavu motoru</param>
        /// <param name="nodeNumber">číslo node</param>
        /// <param name="id">id motoru</param>
        /// <param name="mode">defaultní nastavení módu</param>
        /// <param name="reverse">příznak obrácení směru točení</param>
        /// <param name="multiplier">násobitel otáček v případě, že je motor za převodovkou</param>
        /// <param name="positionVeocity">rychlost motoru v otáčkách při pozicování</param>
        /// <param name="positionAceleration">zrychlení motoru v otáčkách při pozicování</param>
        /// <param name="positionDeceleration">zpomalení motoru v otáčkách při pozicování</param>
        /// <param name="velocity">maximální rychlost motoru při rychlostním řízení</param>
        /// <param name="aceleration">zrychlení motoru při rychlostním řízení</param>
        /// <param name="deceleration">zpomalení motoru při rychlostním řízení</param>
        void inicialize(DeviceManager connector, IStateObserver stateObserver, Action motorErrorOccuredObserver, int nodeNumber, MotorId id, MotorMode mode, bool reverse, int multiplier, uint positionVelocity, uint positionAceleration, uint positionDeceleration, uint velocity, uint aceleration, uint deceleration);

        /// <summary>
        /// Nastavení parametrů motoru
        /// </summary>
        /// <param name="positionVeocity">rychlost motoru v otáčkách při pozicování</param>
        /// <param name="positionAceleration">zrychlení motoru v otáčkách při pozicování</param>
        /// <param name="positionDeceleration">zpomalení motoru v otáčkách při pozicování</param>
        /// <param name="velocity">maximální rychlost motoru při rychlostním řízení</param>
        /// <param name="aceleration">zrychlení motoru při rychlostním řízení</param>
        /// <param name="deceleration">zpomalení motoru při rychlostním řízení</param>
        void setParameters(uint positionVelocity, uint positionAceleration, uint positionDeceleration, uint velocity, uint aceleration, uint deceleration);

        /// <summary>
        /// Přepnutí módu motoru
        /// </summary>
        /// <param name="mode">mód</param>
        void changeMode(MotorMode mode);

        /// <summary>
        /// Pohnutí s motorem
        /// </summary>
        /// <param name="speed">rychlost -100 až 100</param>
        void moving(int speed);

        /// <summary>
        /// Pohnutí s motorem o daný krok
        /// </summary>
        /// <param name="step">krok posunutí v qc</param>
        void move(int step);

        /// <summary>
        /// Pohnutí s motorem na danou pozici vzhledem k homingu
        /// </summary>
        /// <param name="position">absolutní pozice</param>
        void moveToPosition(int position);

        /// <summary>
        /// Pohnutí s motorem do daného úhlu
        /// </summary>
        /// <param name="angle">úhel do kter0ho se m8 motor nastavit vyhledem k jeho 0</param>
        void moveToAngle(int angle);

        /// <summary>
        /// Indikace, zda již motor dorazil do stanovené polohy
        /// </summary>
        /// <returns>true pokud se motor již dostal do cíle</returns>
        bool isTargetReached();

        /// <summary>
        /// Vrátí aktuální reálnou pozici
        /// </summary>
        /// <exception cref="DeviceException">Pokud motor nedokáže získat hodnot, protože je v chybě.</exception>
        /// <returns>pozice 0 až 360</returns>
        int getPosition();

        /// <summary>
        /// Vypne motor
        /// </summary>
        void disable();

        /// <summary>
        /// Zjistí, zda je motor vypnutý
        /// </summary>
        /// <returns>true pokud je vypnutý</returns>
        bool isDisabled();

        /// <summary>
        /// Zapne motor
        /// </summary>
        void enable();

        /// <summary>
        /// Zapne periodický posluchač stavu motoru
        /// </summary>
        void enableStateObserver();

        /// <summary>
        /// Vypne periodický posluchač stavu motoru
        /// </summary>
        void disableStateObserver();

        /// <summary>
        /// Nastaví aktuální pozici motoru jako nulovou (počáteční)
        /// </summary>
        void setActualPositionAsHoming();

        /// <summary>
        /// Nastaví aktuální pozici motoru
        /// </summary>
        /// <param name="position">aktuální pozice motoru</param>
        void setHomingPosition(int position);

        /// <summary>
        /// Nastaví motor do defaultní pozice
        /// </summary>
        void setDefaultPosition();

        /// <summary>
        /// Nastaví současnou polohu motoru jako defaultní
        /// </summary>
        void setCurrentPositionAsDefault();

        /// <summary>
        /// Vypne/zapne ochranu dojezdu motoru
        /// </summary>
        /// <param name="on">true pokud zapnout</param>
        void limitProtectionOnOff(bool on);

        /// <summary>
        /// Zastaví motor
        /// </summary>
        void halt();

        /// <summary>
        /// Pohnen s motorem do maximální koncové pozice
        /// </summary>
        void moveToMaxPosition();

        /// <summary>
        /// Pohnen s motorem do minimální koncové pozice
        /// </summary>
        void moveToMinPosition();
    }
}
