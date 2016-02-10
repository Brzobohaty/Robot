﻿using Robot.Robot.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot
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
        /// <returns>instanci představujícíc robota</returns>
        public IRobot getRobot(Action<string, string, string> motorStateObserver)
        {
            IRobot robot = new EposRobot();
            errorMessage = robot.inicialize(motorStateObserver);
            if (errorMessage.Length > 0)
            {
                robot = new TestRobot();
            }
            return robot;
        }
    }
}
