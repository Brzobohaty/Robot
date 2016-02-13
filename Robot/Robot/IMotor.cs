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
        void inicialize(DeviceManager connector, StateObserver stateObserver, int nodeNumber, MotorId id, MotorMode mode, bool reverse, int multiplier);

        /// <summary>
        /// Pohnutí s motorem
        /// </summary>
        /// <param name="speed">rychlost -100 až 100</param>
        void moving(int speed);

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
    }
}
