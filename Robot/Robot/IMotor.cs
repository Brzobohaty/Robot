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
        /// <param name="id">id motoru ["PP_P", "LP_P", "LZ_P", "PZ_P", "PP_R", "LP_R", "LZ_R", "PZ_R", "PP_Z", "LP_Z", "LZ_Z", "PZ_Z", "PP_ZK", "LP_ZK", "LZ_ZK", "PZ_ZK"]</param>
        /// <param name="mode">defaultní nastavení módu ["velocity","position"]</param>
        /// <param name="reverse">příznak obrácení směru točení</param>
        void inicialize(DeviceManager connector, Action<string, string, string> stateObserver, int nodeNumber, string id, string mode, bool reverse);

        /// <summary>
        /// Pohnutí s motorem
        /// </summary>
        /// <param name="speed">rychlost -100 až 100</param>
        void move(int speed);

        /// <summary>
        /// Pohnutí s motorem
        /// </summary>
        /// <param name="speed">rychlost -100 až 100</param>
        /// <param name="position">pozice 0 až 360</param>
        void move(int speed, int position);

        /// <summary>
        /// Vrátí aktuální reálnou rychlost motoru
        /// </summary>
        /// <returns>rychlost v otáčkách za sekundu</returns>
        int getSpeed();

        /// <summary>
        /// Vrátí aktuální reálnou pozici
        /// </summary>
        /// <returns>pozice 0 až 360</returns>
        int getPosition();

        /// <summary>
        /// Vypne motor
        /// </summary>
        void disable();
    }
}
