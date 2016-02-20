﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Joystick
{
    /// <summary>
    /// Rozhraní pro abstrakci HW joysticku
    /// </summary>
    abstract class AbstractJoystick : IJoystick
    {
        protected Action<int, int> stickDirectMoveObserver; //callback pro změnu stavu páčky pro přímý pohyb (int x souřadnice páčky, int y souřadnice páčky)
        protected Action<int, int> stickMoveObserver; //callback pro změnu stavu páčky pro rádiusový pohyb (int x souřadnice páčky, int y souřadnice páčky)
        protected Action<bool> buttonMoveUpObserver; //callback pro změnu stavu tlačítka pro pohyb nahoru (bool pohyb nahoru)
        protected Action<bool> buttonMoveDownObserver; //callback pro změnu stavu tlačítka pro pohyb dolu (bool pohyb dolu)
        protected Action<bool> buttonNarrowObserver; //callback pro změnu stavu tlačítka pro zůžení (bool zůžit)
        protected Action<bool> buttonWidenObserver; //callback pro změnu stavu tlačítka pro rozšíření (bool rozšířit)
        protected Action<bool> buttonRotateLeftObserver; //callback pro změnu stavu tlačítka pro rotaci vlevo (bool stisknuto)
        protected Action<bool> buttonRotateRightObserver; //callback pro změnu stavu tlačítka pro rotaci vpravo (bool stisknuto)
        protected Action<bool> buttonDefaultPositionObserver; //callback pro změnu stavu tlačítka pro defaultní pozici (bool defaultní pozice)
        protected Action<bool> buttonStopObserver; //callback pro změnu stavu tlačítka pro zastavení všeho (bool stisknuto)
        protected Action errorObserver; //callback když nastane chyba
        protected bool enabled = true; //příznak vypnutého/zapnutého ovládání

        /// <summary>
        /// Inicializace gamepad
        /// </summary>
        /// <returns>true pokud se inicializace povedla</returns>
        abstract public bool inicialize();

        /// <summary>
        /// Vypne/zapne ovládání pomocí ovladače
        /// </summary>
        /// <param name="on">true pokud zapnout</param>
        public void onOff(bool on)
        {
            enabled = on;
        }

        /// <summary>
        /// Přiřazení posluchače pro změnu stavu joysticku
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu s parametry (int x souřadnice páčky, int y souřadnice páčky)</param>
        public void subscribeDirectMoveStickObserver(Action<int, int> observer)
        {
            stickDirectMoveObserver = observer;
        }

        /// <summary>
        /// Přiřazení posluchače pro změnu stavu joysticku pro rádiusový pohyb
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu s parametry (int x souřadnice páčky, int y souřadnice páčky)</param>
        public void subscribeMoveStickObserver(Action<int, int> observer) {
            stickMoveObserver = observer;
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

        /// <summary>
        /// Přiřazení posluchače pro změnu stavu tlačítka pro rotaci vlevo
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu s parametry (bool stisknuto)</param>
        public void subscribeButtonRotateLeftObserver(Action<bool> observer)
        {
            buttonRotateLeftObserver = observer;
        }

        /// <summary>
        /// Přiřazení posluchače pro změnu stavu tlačítka pro rotaci vpravo
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu s parametry (bool stisknuto)</param>
        public void subscribeButtonRotateRightObserver(Action<bool> observer)
        {
            buttonRotateRightObserver = observer;
        }

        /// <summary>
        /// Přiřazení posluchače pro změnu stavu tlačítka pro zastavení všeho
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu s parametry (bool stisknuto)</param>
        public void subscribeButtonStopObserver(Action<bool> observer) {
            buttonStopObserver = observer;
        }

        /// <summary>
        /// Přiřazení posluchače, když nastane chyba
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu</param>
        public void subscribeErrorObserver(Action observer)
        {
            errorObserver = observer;
        }

        /// <summary>
        /// Odstraní všechny posluchače na joysticku
        /// </summary>
        public void unsibscribeAllObservers()
        {
            stickDirectMoveObserver = null;
            buttonMoveUpObserver = null;
            buttonMoveDownObserver = null;
            buttonNarrowObserver = null;
            buttonWidenObserver = null;
            buttonDefaultPositionObserver = null;
            buttonRotateLeftObserver = null;
            buttonRotateRightObserver = null;
            stickMoveObserver = null;
            buttonStopObserver = null;
        }

        /// <summary>
        /// Přepravka pro zapamatování stavu
        /// </summary>
        protected class GamePadeState
        {
            public int stickDirectMoveX = 0; //souřadnice X joysticku pro přímý pohyb
            public int stickDirectMoveY = 0; //souřadnice Y joysticku pro přímý pohyb
            public int stickMoveX = 0; //souřadnice X joysticku pro pohyb v rádiusu
            public int stickMoveY = 0; //souřadnice Y joysticku pro pohyb v rádiusu
            public bool moveDown = false; //tlačítko pro pohyb dolu
            public bool moveUp = false; //tlačítko pro pohyb nahoru
            public bool narrow = false; //tlačítko pro zůžení
            public bool widen = false; //tlačítko pro rozšíření
            public bool defaultPosition = false; //tlačítko pro defaultní pozici
            public bool rotateLeft = false; //tlačítko pro rotaci vlevo
            public bool rotateRight = false; //tlačítko pro rotaci vpravo
            public bool stop = false; //tlačítko pro zastavení všeho
        }
    }
}
