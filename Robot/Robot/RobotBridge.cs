using Robot.Robot.Implementations;
using Robot.Robot.Implementations.Epos;
using Robot.Robot.Implementations.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Robot
{
    /// <summary>
    /// Slouží pro získání instance robota
    /// </summary>
    class RobotBridge
    {
        public string errorMessage { get; private set; } //chybová hláška, která nastala při řipojová vnějšího ovládacího zařízení

        /// <summary>
        /// Vrátí instanci ovládající fyzického robota nebo v případě chyby vrátí softwarovou simulaci robota
        /// </summary>
        /// <param name="motorStateObserver">posluchač stavu motoru</param>
        /// <param name="withChooseOfBus">příznak, zda při inicilizaci nechat uživatele nastavit parametry připojení</param>
        /// <param name="motorErrorOccuredObserver">posluchač jakéhokoli eroru motoru</param>
        /// <returns>instanci představujícíc robota</returns>
        public IRobot getRobot(StateObserver motorStateObserver, bool withChooseOfBus, Action motorErrorOccuredObserver)
        {
            IRobot robot = new EposRobot();
            errorMessage = robot.inicialize(motorStateObserver, withChooseOfBus, motorErrorOccuredObserver);
            if (errorMessage.Length > 0)
            {
                robot = new TestRobot();
            }
            return robot;
        }
    }
}
