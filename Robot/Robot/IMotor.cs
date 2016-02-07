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
        /// <returns>chybovou hlášku nebo prázdný řetězec pokud nenastala chyba</returns>
        string inicialize();

        /// <summary>
        /// Pohnutí s motorem
        /// </summary>
        /// <param name="speed">rychlost 0 až 100</param>
        void move(int speed);

        /// <summary>
        /// Pohnutí s motorem
        /// </summary>
        /// <param name="speed">rychlost 0 až 100</param>
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
    }
}
