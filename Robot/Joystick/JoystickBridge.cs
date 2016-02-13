using Robot.Joystick.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Joystick
{
    /// <summary>
    /// Slouží pro získání instance nejlépe vyhovujícího ovládacího zařízení
    /// </summary>
    class JoystickBridge
    {
        public string errorMessage { get; private set; } //chybová hláška, která nastala při řipojová vnějšího ovládacího zařízení

        /// <summary>
        /// Vrátí první nejlépe vyhovující vnější ovládací prvek a pokud nenalezne žádný, tak vrátí softwarovou simulaci
        /// </summary>
        /// <returns>instanci představujícíc joystick</returns>
        public IJoystick getJoystick()
        {
            IJoystick joystick = new Gamepad();
            errorMessage = joystick.inicialize();
            if (errorMessage.Length > 0)
            {
                joystick = ControllView.getInstance();
            }
            return joystick;
        }
    }
}
