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
        public string message { get; private set; } //success nebo chybová hláška
        public bool success { get; private set; } //příznak, zda se povedlo připojit externí ovladač

        /// <summary>
        /// Vrátí první nejlépe vyhovující vnější ovládací prvek a pokud nenalezne žádný, tak vrátí softwarovou simulaci
        /// </summary>
        /// <returns>instanci představujícíc joystick</returns>
        public IJoystick getJoystick()
        {
            message = "";
            IJoystick joystick = new Xbox360Controller();
            success = joystick.inicialize();
            if (!success)
            {
                joystick = new Gamepad();
                success = joystick.inicialize();
            }
            else {
                message = "Byl úspěšně připojen plně kompatibilní externí ovladač.";
            }
            if (!success)
            {
                message = "Nepovedlo se najít žádné kompatibilní externí ovládací zařízení. Robota je možné ovládat pomocí softwarového ovladače.";
                joystick = ControllView.getInstance();
            }
            else if(message == ""){
                message = "Byl úspěšně připojen částečně kompatibilní externí ovladač. Některé funkce mohou být omezeny.";
            }
            return joystick;
        }
    }
}
