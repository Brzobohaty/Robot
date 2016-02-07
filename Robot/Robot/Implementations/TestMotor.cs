using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Robot.Implementations
{
    /// <summary>
    /// Představuje softwarovou simulaci motoru
    /// </summary>
    class TestMotor : IMotor
    {
        private int speed = 0;
        private int position = 0;

        /// <summary>
        /// Inicializace motoru
        /// </summary>
        /// <returns>chybovou hlášku nebo prázdný řetězec pokud nenastala chyba</returns>
        public string inicialize()
        {
            return "";
        }

        /// <summary>
        /// Pohnutí s motorem
        /// </summary>
        /// <param name="speed">rychlost 0 až 100</param>
        public void move(int speed)
        {
            this.speed = speed;
        }

        /// <summary>
        /// Pohnutí s motorem
        /// </summary>
        /// <param name="speed">rychlost 0 až 100</param>
        /// <param name="position">pozice 0 až 360</param>
        public void move(int speed, int position)
        {
            this.speed = speed;
            this.position = position;
        }

        /// <summary>
        /// Vrátí aktuální reálnou rychlost motoru
        /// </summary>
        /// <returns>rychlost v otáčkách za sekundu</returns>
        public int getSpeed()
        {
            return speed;
        }

        /// <summary>
        /// Vrátí aktuální reálnou pozici
        /// </summary>
        /// <returns>pozice 0 až 360</returns>
        public int getPosition()
        {
            return position;
        }
    }
}
