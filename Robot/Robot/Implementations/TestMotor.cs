using EposCmd.Net;
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
        /// <param name="connector">connector sběrnice</param>
        /// <param name="stateObserver">posluchač stavu motoru</param>
        /// <param name="nodeNumber">číslo node</param>
        /// <param name="id">id motoru ["PP_P", "LP_P", "LZ_P", "PZ_P", "PP_R", "LP_R", "LZ_R", "PZ_R", "PP_Z", "LP_Z", "LZ_Z", "PZ_Z", "PP_ZK", "LP_ZK", "LZ_ZK", "PZ_ZK"]</param>
        /// <param name="mode">defaultní nastavení módu ["velocity","position"]</param>
        /// <param name="reverse">příznak obrácení směru točení</param>
        public void inicialize(DeviceManager connector, Action<MotorState, string, MotorId, bool, int> stateObserver, int nodeNumber, MotorId id, string mode, bool reverse)
        {
        }

        /// <summary>
        /// Pohnutí s motorem
        /// </summary>
        /// <param name="speed">rychlost -100 až 100</param>
        public void moving(int speed)
        {
            this.speed = speed;
        }

        /// <summary>
        /// Pohnutí s motorem
        /// </summary>
        /// <param name="speed">rychlost -100 až 100</param>
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

        /// <summary>
        /// Vypne motor
        /// </summary>
        public void disable()
        {

        }
    }
}
