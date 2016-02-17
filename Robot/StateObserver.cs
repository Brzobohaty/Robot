using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot
{
    /// <summary>
    /// Rozhraní pro posluchač stavu motoru 
    /// </summary>
    public interface StateObserver
    {
        /// <summary>
        /// Nahlásí event změny stavu motoru
        /// </summary>
        /// <param name="state">stav</param>
        /// <param name="message">zpráva ke stavu</param>
        /// <param name="motorId">id motoru</param>
        /// <param name="speed">aktuální rychlost motoru</param>
        /// <param name="position">aktuální pozice motoru</param>
        /// <param name="speedRelative">relativní rychlost (0 až 100)</param>
        /// <param name="positionRelative">relativní pozice (-100 až 100)</param>
        void motorStateChanged(MotorState state, string message, MotorId motorId, int speed, int position, int speedRelative, int positionRelative);
    }
}
