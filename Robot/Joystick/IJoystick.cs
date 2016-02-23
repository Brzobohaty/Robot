using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Joystick
{
    /// <summary>
    /// Rozhraní pro abstrakci HW joysticku
    /// </summary>
    interface IJoystick
    {
        /// <summary>
        /// Inicializace gamepad
        /// </summary>
        /// <returns>true pokud se inicializace povedla</returns>
        bool inicialize();

        /// <summary>
        /// Přiřazení posluchače pro změnu stavu joysticku pro přímý pohyb
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu s parametry (int x souřadnice páčky, int y souřadnice páčky)</param>
        void subscribeDirectMoveStickObserver(Action<int, int> observer);

        /// <summary>
        /// Přiřazení posluchače pro změnu stavu joysticku pro rádiusový pohyb
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu s parametry (int x souřadnice páčky, int y souřadnice páčky)</param>
        void subscribeMoveStickObserver(Action<int, int> observer);

        /// <summary>
        /// Přiřazení posluchače pro změnu stavu analogového tlačítka zůžení předku
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu s parametry (int míra zůžení)</param>
        void subscribeFrontNarrowObserver(Action<int> observer);

        /// <summary>
        /// Přiřazení posluchače pro změnu stavu analogového tlačítka zůžení zadku
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu s parametry (int míra zůžení)</param>
        void subscribeBackNarrowObserver(Action<int> observer);

        /// <summary>
        /// Přiřazení posluchače pro změnu stavu tlačítka pro pohyb nahoru
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu s parametry (bool pohyb nahoru)</param>
        void subscribeButtonMoveUpObserver(Action<bool> observer);

        /// <summary>
        /// Přiřazení posluchače pro změnu stavu tlačítka pro pohyb dolu
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu s parametry (bool pohyb dolu)</param>
        void subscribeButtonMoveDownObserver(Action<bool> observer);

        /// <summary>
        /// Přiřazení posluchače pro změnu stavu tlačítka pro zůžení
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu s parametry (bool zůžit)</param>
        void subscribeButtonNarrowObserver(Action<bool> observer);

        /// <summary>
        /// Přiřazení posluchače pro změnu stavu tlačítka pro rozšíření
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu s parametry (bool rozšířit)</param>
        void subscribeButtonWidenObserver(Action<bool> observer);

        /// <summary>
        /// Přiřazení posluchače pro změnu stavu tlačítka pro defaultní pozici
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu s parametry (bool defaultní pozice)</param>
        void subscribeButtonDefaultPositionObserver(Action<bool> observer);

        /// <summary>
        /// Přiřazení posluchače pro změnu stavu tlačítka pro rotaci vlevo
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu s parametry (bool stisknuto)</param>
        void subscribeButtonRotateLeftObserver(Action<bool> observer);

        /// <summary>
        /// Přiřazení posluchače pro změnu stavu tlačítka pro rotaci vpravo
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu s parametry (bool stisknuto)</param>
        void subscribeButtonRotateRightObserver(Action<bool> observer);

        /// <summary>
        /// Přiřazení posluchače pro změnu stavu tlačítka pro zastavení všeho
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu s parametry (bool stisknuto)</param>
        void subscribeButtonStopObserver(Action<bool> observer);

        /// <summary>
        /// Přiřazení posluchače pro změnu stavu tlačítka pro naklonění dopředu
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu s parametry (bool stisknuto)</param>
        void subscribeButtonTiltFrontObserver(Action<bool> observer);

        /// <summary>
        /// Přiřazení posluchače pro změnu stavu tlačítka pro naklonění dozadu
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu s parametry (bool stisknuto)</param>
        void subscribeButtonTiltBackObserver(Action<bool> observer);

        /// <summary>
        /// Přiřazení posluchače pro změnu stavu tlačítka pro naklonění doprava
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu s parametry (bool stisknuto)</param>
        void subscribeButtonTiltRightObserver(Action<bool> observer);

        /// <summary>
        /// Přiřazení posluchače pro změnu stavu tlačítka pro naklonění doleva
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu s parametry (bool stisknuto)</param>
        void subscribeButtonTiltLeftObserver(Action<bool> observer);

        /// <summary>
        /// Přiřazení posluchače, když nastane chyba
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu</param>
        void subscribeErrorObserver(Action observer);

        /// <summary>
        /// Vypne/zapne ovládání pomocí ovladače
        /// </summary>
        /// <param name="on">true pokud zapnout</param>
        void onOff(bool on);

        /// <summary>
        /// Odstraní všechny posluchače na joysticku
        /// </summary>
        void unsibscribeAllObservers();
    }
}
