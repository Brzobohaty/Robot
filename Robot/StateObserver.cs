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
        void motorStateChanged(MotorState state, string message, MotorId motorId, int speed);
    }
}
