using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlimDX.DirectInput;
using System.Timers;

namespace Robot.Joystick.Implementations
{
    /// <summary>
    /// Abstrakce HW gamepadu připojeného k PC
    /// </summary>
    class Gamepad : AbstractJoystick
    {
        private SlimDX.DirectInput.Joystick gamepad; //připojený gamepad
        private GamePadeState state = new GamePadeState(); //stav gamepadu
        private const int sensitivity = 5; //citlivost joysticku

        /// <summary>
        /// Inicializace gamepad
        /// </summary>
        /// <returns>true pokud se inicializace povedla</returns>
        public override bool inicialize() {
            onOff(false);
            try
            {
                gamepad = getGamepad();
                setJoystickObserver();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Získání prvního gamepadu, ketrý je připojený k počítači
        /// </summary>
        /// <returns>zařízení</returns>
        /// <exception>Pokud se nepodařilo najít žádné funkční zařízení</exception>
        private SlimDX.DirectInput.Joystick getGamepad()
        {
            DirectInput dinput = new DirectInput();
            SlimDX.DirectInput.Joystick gamepad;
            string errorMessage = "Nebylo nalezeno žádné vnější ovládací zařízení.";

            foreach (DeviceInstance device in dinput.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly))
            {
                try
                {
                    gamepad = new SlimDX.DirectInput.Joystick(dinput, device.InstanceGuid);
                    gamepad.Acquire();

                    foreach (DeviceObjectInstance deviceObject in gamepad.GetObjects())
                    {
                        if ((deviceObject.ObjectType & ObjectDeviceType.Axis) != 0)
                        {
                            gamepad.GetObjectPropertiesById((int)deviceObject.ObjectType).SetRange(-100, 100);
                        }
                    }
                    return gamepad;
                }
                catch (DirectInputException e)
                {
                    errorMessage = e.Message;
                }
            }
            throw new Exception(errorMessage);
        }

        /// <summary>
        /// Nastavení timeru pro dotazování stavu gamepadu
        /// </summary>
        private void setJoystickObserver()
        {
            Timer aTimer = new Timer();
            aTimer.Elapsed += new ElapsedEventHandler(gamepadHandle);
            aTimer.Interval = 200;
            aTimer.Enabled = true;
        }

        /// <summary>
        /// Handler stavu gamepadu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gamepadHandle(object sender, EventArgs e)
        {
            if (enabled) {
                JoystickState stateNow = new JoystickState();
                stateNow = gamepad.GetCurrentState();
                bool[] buttons = stateNow.GetButtons();

                if ((Math.Abs(state.stickDirectMoveX - stateNow.X) > sensitivity || Math.Abs(state.stickDirectMoveY - stateNow.Y) > sensitivity) && stickDirectMoveObserver != null)
                {
                    state.stickDirectMoveX = stateNow.X;
                    state.stickDirectMoveY = stateNow.Y;
                    int x = state.stickDirectMoveX;
                    int y = state.stickDirectMoveY;
                    if (Math.Abs(x) < 10 && Math.Abs(y) < 10)
                    {
                        x = 0;
                        y = 0;
                    }
                    stickDirectMoveObserver(x, y);
                }

                if (state.moveDown != buttons[0] && buttonMoveDownObserver != null)
                {
                    buttonMoveDownObserver(buttons[0]);
                    state.moveDown = buttons[0];
                }

                if (state.moveUp != buttons[3] && buttonMoveUpObserver != null)
                {
                    buttonMoveUpObserver(buttons[3]);
                    state.moveUp = buttons[3];
                }

                if (state.narrow != buttons[2] && buttonNarrowObserver != null)
                {
                    buttonNarrowObserver(buttons[2]);
                    state.narrow = buttons[2];
                }

                if (state.widen != buttons[1] && buttonWidenObserver != null)
                {
                    buttonWidenObserver(buttons[1]);
                    state.widen = buttons[1];
                }

                if (state.defaultPosition != buttons[7] && buttonDefaultPositionObserver != null)
                {
                    buttonDefaultPositionObserver(buttons[7]);
                    state.defaultPosition = buttons[7];
                }
            }
        }
    }
}
