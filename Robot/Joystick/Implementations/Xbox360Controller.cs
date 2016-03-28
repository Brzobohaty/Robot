using System;
using System.Timers;
using SlimDX;
using SlimDX.XInput;

namespace Robot.Joystick.Implementations
{
    /// <summary>
    /// Abstrakce HW gamepadu kompatibilního s XBOX rozhraním připojeného k PC
    /// </summary>
    class Xbox360Controller : AbstractJoystick
    {
        private SlimDX.XInput.Controller gamepad = new SlimDX.XInput.Controller(0); //připojený gamepad
        private GamePadeState state = new GamePadeState(); //stav gamepadu
        private Timer periodicChecker; //periodický kontorler stavu gamepadu

        /// <summary>
        /// Inicializace gamepad
        /// </summary>
        /// <returns>true pokud se inicializace povedla</returns>
        public override bool inicialize()
        {
            onOff(false);
            if (gamepad.IsConnected) {
                setJoystickObserver();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Nastavení timeru pro dotazování stavu gamepadu
        /// </summary>
        private void setJoystickObserver()
        {
            periodicChecker = new Timer();
            periodicChecker.Elapsed += new ElapsedEventHandler(gamepadHandle);
            periodicChecker.Interval = 200;
            periodicChecker.Enabled = true;
        }

        /// <summary>
        /// Handler stavu gamepadu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gamepadHandle(object sender, EventArgs e)
        {
            if (enabled)
            {
                try {
                    State stateNow = gamepad.GetState();
                    GamepadButtonFlags buttons = stateNow.Gamepad.Buttons;

                    if (state.moveDown != buttons.HasFlag(GamepadButtonFlags.Y) && buttonMoveDownObserver != null)
                    {
                        buttonMoveDownObserver(buttons.HasFlag(GamepadButtonFlags.Y));
                        state.moveDown = buttons.HasFlag(GamepadButtonFlags.Y);
                    }

                    if (state.moveUp != buttons.HasFlag(GamepadButtonFlags.A) && buttonMoveUpObserver != null)
                    {
                        buttonMoveUpObserver(buttons.HasFlag(GamepadButtonFlags.A));
                        state.moveUp = buttons.HasFlag(GamepadButtonFlags.A);
                    }

                    if (state.narrow != buttons.HasFlag(GamepadButtonFlags.X) && buttonNarrowObserver != null)
                    {
                        buttonNarrowObserver(buttons.HasFlag(GamepadButtonFlags.X));
                        state.narrow = buttons.HasFlag(GamepadButtonFlags.X);
                    }

                    if (state.widen != buttons.HasFlag(GamepadButtonFlags.B) && buttonWidenObserver != null)
                    {
                        buttonWidenObserver(buttons.HasFlag(GamepadButtonFlags.B));
                        state.widen = buttons.HasFlag(GamepadButtonFlags.B);
                    }

                    if (state.defaultPosition != buttons.HasFlag(GamepadButtonFlags.Start) && buttonDefaultPositionObserver != null)
                    {
                        buttonDefaultPositionObserver(buttons.HasFlag(GamepadButtonFlags.Start));
                        state.defaultPosition = buttons.HasFlag(GamepadButtonFlags.Start);
                    }

                    if (state.rotateRight != buttons.HasFlag(GamepadButtonFlags.RightShoulder) && buttonRotateRightObserver != null)
                    {
                        buttonRotateRightObserver(buttons.HasFlag(GamepadButtonFlags.RightShoulder));
                        state.rotateRight = buttons.HasFlag(GamepadButtonFlags.RightShoulder);
                    }

                    if (state.rotateLeft != buttons.HasFlag(GamepadButtonFlags.LeftShoulder) && buttonRotateLeftObserver != null)
                    {
                        buttonRotateLeftObserver(buttons.HasFlag(GamepadButtonFlags.LeftShoulder));
                        state.rotateLeft = buttons.HasFlag(GamepadButtonFlags.LeftShoulder);
                    }

                    if (state.stop != buttons.HasFlag(GamepadButtonFlags.Back) && buttonStopObserver != null)
                    {
                        buttonStopObserver(buttons.HasFlag(GamepadButtonFlags.Back));
                        state.stop = buttons.HasFlag(GamepadButtonFlags.Back);
                    }

                    if (state.tiltFront != buttons.HasFlag(GamepadButtonFlags.DPadUp) && buttonTiltFrontObserver != null)
                    {
                        buttonTiltFrontObserver(buttons.HasFlag(GamepadButtonFlags.DPadUp));
                        state.tiltFront = buttons.HasFlag(GamepadButtonFlags.DPadUp);
                    }

                    if (state.tiltBack != buttons.HasFlag(GamepadButtonFlags.DPadDown) && buttonTiltBackObserver != null)
                    {
                        buttonTiltBackObserver(buttons.HasFlag(GamepadButtonFlags.DPadDown));
                        state.tiltBack = buttons.HasFlag(GamepadButtonFlags.DPadDown);
                    }

                    if (state.tiltLeft != buttons.HasFlag(GamepadButtonFlags.DPadLeft) && buttonTiltLeftObserver != null)
                    {
                        buttonTiltLeftObserver(buttons.HasFlag(GamepadButtonFlags.DPadLeft));
                        state.tiltLeft = buttons.HasFlag(GamepadButtonFlags.DPadLeft);
                    }

                    if (state.tiltRight != buttons.HasFlag(GamepadButtonFlags.DPadRight) && buttonTiltRightObserver != null)
                    {
                        buttonTiltRightObserver(buttons.HasFlag(GamepadButtonFlags.DPadRight));
                        state.tiltRight = buttons.HasFlag(GamepadButtonFlags.DPadRight);
                    }

                    if ((Math.Abs(state.stickDirectMoveX - stateNow.Gamepad.LeftThumbX) > sensitivityJoystick || Math.Abs(state.stickDirectMoveY - stateNow.Gamepad.LeftThumbY) > sensitivityJoystick) && stickDirectMoveObserver != null)
                    {
                        state.stickDirectMoveX = stateNow.Gamepad.LeftThumbX;
                        state.stickDirectMoveY = stateNow.Gamepad.LeftThumbY;
                        int x = MathLibrary.changeScale(state.stickDirectMoveX, -32768, 32767, -100, 100);
                        int y = MathLibrary.changeScale(state.stickDirectMoveY, 32767, -32768, -100, 100);
                        if (Math.Abs(x) < sensitivityJoystick && Math.Abs(y) < sensitivityJoystick)
                        {
                            x = 0;
                            y = 0;
                        }
                        stickDirectMoveObserver(x, y);
                    }

                    if ((Math.Abs(state.stickMoveX - stateNow.Gamepad.RightThumbX) > sensitivityJoystick || Math.Abs(state.stickMoveY - stateNow.Gamepad.RightThumbY) > sensitivityJoystick) && stickMoveObserver != null)
                    {
                        state.stickMoveX = stateNow.Gamepad.RightThumbX;
                        state.stickMoveY = stateNow.Gamepad.RightThumbY;
                        int x = MathLibrary.changeScale(state.stickMoveX, -32768, 32767, -100, 100);
                        int y = MathLibrary.changeScale(state.stickMoveY, 32767, -32768, -100, 100);
                        if (Math.Abs(x) < sensitivityJoystick && Math.Abs(y) < sensitivityJoystick)
                        {
                            x = 0;
                            y = 0;
                        }
                        stickMoveObserver(x, y);
                    }

                    if ((Math.Abs(state.frontNarrow - stateNow.Gamepad.LeftTrigger) > sensitivityAnalogButtons) && frontNarrowObserver != null)
                    {
                        state.frontNarrow = stateNow.Gamepad.LeftTrigger;
                        int frontNarrow = MathLibrary.changeScale(state.frontNarrow, 0, 255, 0, 100);
                        if (state.frontNarrow < sensitivityAnalogButtons)
                        {
                            frontNarrow = 0;
                        }
                        frontNarrowObserver(frontNarrow);
                    }

                    if ((Math.Abs(state.backNarrow - stateNow.Gamepad.RightTrigger) > sensitivityAnalogButtons) && backNarrowObserver != null)
                    {
                        state.backNarrow = stateNow.Gamepad.RightTrigger;
                        int backNarrow = MathLibrary.changeScale(state.backNarrow, 0, 255, 0, 100);
                        if (state.backNarrow < sensitivityAnalogButtons)
                        {
                            backNarrow = 0;
                        }
                        backNarrowObserver(backNarrow);
                    }
                }
                catch (XInputException)
                {
                    periodicChecker.Dispose();
                    errorObserver();
                }
            }
        }
    }
}
