using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot
{
    /// <summary>
    /// Rozhraní pro abstrakci HW joysticku
    /// </summary>
    abstract class AbstractJoystick
    {
        protected Action<int, int> stickObserver; //callback pro změnu stavu páčky (int x souřadnice páčky, int y souřadnice páčky)
        protected Action<bool> buttonMoveUpObserver; //callback pro změnu stavu tlačítka pro pohyb nahoru (bool pohyb nahoru)
        protected Action<bool> buttonMoveDownObserver; //callback pro změnu stavu tlačítka pro pohyb dolu (bool pohyb dolu)
        protected Action<bool> buttonNarrowObserver; //callback pro změnu stavu tlačítka pro zůžení (bool zůžit)
        protected Action<bool> buttonWidenObserver; //callback pro změnu stavu tlačítka pro rozšíření (bool rozšířit)
        protected Action<bool> buttonDefaultPositionObserver; //callback pro změnu stavu tlačítka pro defaultní pozici (bool defaultní pozice)


        /// <summary>
        /// Inicializace gamepad
        /// </summary>
        /// <returns>chybovou hlášku nebo prázdný řetězec pokud nenastala chyba</returns>
        abstract public string inicialize();

        /// <summary>
        /// Přiřazení posluchače pro změnu stavu joysticku
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu s parametry (int x souřadnice páčky, int y souřadnice páčky)</param>
        public void subscribeStickObserver(Action<int, int> observer)
        {
            stickObserver = observer;
        }

        /// <summary>
        /// Přiřazení posluchače pro změnu stavu tlačítka pro pohyb nahoru
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu s parametry (bool pohyb nahoru)</param>
        public void subscribeButtonMoveUpObserver(Action<bool> observer)
        {
            buttonMoveUpObserver = observer;
        }

        /// <summary>
        /// Přiřazení posluchače pro změnu stavu tlačítka pro pohyb dolu
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu s parametry (bool pohyb dolu)</param>
        public void subscribeButtonMoveDownObserver(Action<bool> observer)
        {
            buttonMoveDownObserver = observer;
        }

        /// <summary>
        /// Přiřazení posluchače pro změnu stavu tlačítka pro zůžení
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu s parametry (bool zůžit)</param>
        public void subscribeButtonNarrowObserver(Action<bool> observer)
        {
            buttonNarrowObserver = observer;
        }

        /// <summary>
        /// Přiřazení posluchače pro změnu stavu tlačítka pro rozšíření
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu s parametry (bool rozšířit)</param>
        public void subscribeButtonWidenObserver(Action<bool> observer)
        {
            buttonWidenObserver = observer;
        }

        /// <summary>
        /// Přiřazení posluchače pro změnu stavu tlačítka pro defaultní pozici
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu s parametry (bool defaultní pozice)</param>
        public void subscribeButtonDefaultPositionObserver(Action<bool> observer)
        {
            buttonDefaultPositionObserver = observer;
        }
    }
}
