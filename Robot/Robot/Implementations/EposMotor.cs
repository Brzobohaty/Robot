using EposCmd.Net;
using EposCmd.Net.DeviceCmdSet.Operation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Robot.Robot.Implementations
{
    /// <summary>
    /// Motor v HW implementací od firmy EPOS
    /// </summary>
    class EposMotor : IMotor
    {
        private Device motor; //ovladač motoru
        private StateMachine sm; //ovladač stavu motoru
        private string mode; //mód ve kterém se zrovna nachází handler motoru ["velocity","position"] 
        private ProfileVelocityMode velocityHandler; //handler pro rychlostní operace s motorem 
        private ProfilePositionMode positionHandler; //handler pro pozicové operace s motorem
        private Action<MotorState, string, MotorId, bool, int> stateObserver; //callback pro stav motoru
        private int rev = 1; //modifikátor směru [1, -1]
        private MotorId id; //id motoru
        private MotorState state; //aktuální stav motoru
        private MotionInfo stateHandler; //handler stavu motoru 
        private Timer timerObserver; //časovač pro spouštění posluchače stavu

        /// <summary>
        /// Inicializace motoru
        /// </summary>
        /// <param name="connector">connector sběrnice</param>
        /// <param name="stateObserver">posluchač stavu motoru</param>
        /// <param name="nodeNumber">číslo node</param>
        /// <param name="id">id motoru</param>
        /// <param name="mode">defaultní nastavení módu ["velocity","position"]</param>
        /// <param name="reverse">příznak obrácení směru točení</param>
        public void inicialize(DeviceManager connector, Action<MotorState, string, MotorId, bool, int> stateObserver, int nodeNumber, MotorId id, string mode, bool reverse)
        {
            try
            {
                this.mode = mode;
                this.stateObserver = stateObserver;
                this.id = id;

                if (reverse)
                {
                    rev = -1;
                }

                motor = connector.CreateDevice(Convert.ToUInt16(nodeNumber));
                stateHandler = motor.Operation.MotionInfo;

                sm = motor.Operation.StateMachine;
                if (sm.GetFaultState())
                    sm.ClearFault();
                sm.SetEnableState();

                velocityHandler = motor.Operation.ProfileVelocityMode;
                positionHandler = motor.Operation.ProfilePositionMode;
                changeMode(mode);

                setStateObserver();
                stateObserver(MotorState.enabled, "", id, false, 0);
            }
            catch (DeviceException e)
            {
                disableStateObserver();
                stateObserver(MotorState.error, String.Format("{0}\nErrorCode: {1:X8}", e.ErrorMessage, e.ErrorCode), id, false, 0);
            }
            catch (Exception e)
            {
                disableStateObserver();
                stateObserver(MotorState.error, e.Message, id, false, 0);
            }
        }

        /// <summary>
        /// Přepnutí módu motoru
        /// </summary>
        /// <param name="mode">mód ["velocity","position"]</param>
        public void changeMode(string mode)
        {
            switch (mode)
            {
                case "velocity": velocityHandler.ActivateProfileVelocityMode(); break;
                case "position": positionHandler.ActivateProfilePositionMode(); break;
            }
        }

        /// <summary>
        /// Pohnutí s motorem
        /// </summary>
        /// <param name="speed">rychlost -100 až 100</param>
        public void moving(int speed)
        {
            try
            {
                velocityHandler.MoveWithVelocity(speed * rev * 10);
            }
            catch (DeviceException e)
            {
                stateObserver(MotorState.error, String.Format("{0}\nErrorCode: {1:X8}", e.ErrorMessage, e.ErrorCode), id, false, 0);
            }
            catch (Exception e)
            {
                stateObserver(MotorState.error, e.Message, id, false, 0);
            }
        }

        /// <summary>
        /// Pohnutí s motorem o daný krok
        /// </summary>
        /// <param name="step">krok posunutí v qc</param>
        public void move(int step)
        {
            try
            {
                positionHandler.MoveToPosition(Convert.ToInt32(step), false, false);
            }
            catch (DeviceException e)
            {
                stateObserver(MotorState.error, String.Format("{0}\nErrorCode: {1:X8}", e.ErrorMessage, e.ErrorCode), id, false, 0);
            }
            catch (Exception e)
            {
                stateObserver(MotorState.error, e.Message, id, false, 0);
            }
        }

        /// <summary>
        /// Pohnutí s motorem danou rychlostí na danou absolutní pozici
        /// </summary>
        /// <param name="speed">rychlost -100 až 100</param>
        /// <param name="position">pozice 0 až 360</param>
        public void move(int speed, int position)
        {
            try
            {
                positionHandler.MoveToPosition(Convert.ToInt32(position), false, false);
            }
            catch (DeviceException e)
            {
                stateObserver(MotorState.error, String.Format("{0}\nErrorCode: {1:X8}", e.ErrorMessage, e.ErrorCode), id, false, 0);
            }
            catch (Exception e)
            {
                stateObserver(MotorState.error, e.Message, id, false, 0);
            }
        }

        /// <summary>
        /// Vrátí aktuální reálnou rychlost motoru
        /// </summary>
        /// <returns>rychlost v otáčkách za sekundu</returns>
        public int getSpeed()
        {
            return 0;
        }

        /// <summary>
        /// Vrátí aktuální reálnou pozici
        /// </summary>
        /// <returns>pozice 0 až 360</returns>
        public int getPosition()
        {
            return 0;
        }

        /// <summary>
        /// Vypne motor
        /// </summary>
        public void disable()
        {
            if (timerObserver != null)
            {
                timerObserver.Dispose();
            }
            if (sm != null)
            {
                if (sm.GetFaultState())
                    sm.ClearFault();

                if (!sm.GetDisableState())
                    sm.SetDisableState();
            }
            if (stateObserver != null) {
                stateObserver(MotorState.disabled, "", id, false, 0);
            }
        }

        /// <summary>
        /// Zapne periodický posluchač stavu motoru
        /// </summary>
        public void enableStateObserver()
        {
            if (timerObserver != null)
            {
                timerObserver.Enabled = true;
            }
        }

        /// <summary>
        /// Vypne periodický posluchač stavu motoru
        /// </summary>
        public void disableStateObserver()
        {
            if (timerObserver != null)
            {
                timerObserver.Enabled = false;
            }
        }

        /// <summary>
        /// Nastavení timeru pro dotazování stavu motoru
        /// </summary>
        private void setStateObserver()
        {
            timerObserver = new Timer();
            timerObserver.Elapsed += new ElapsedEventHandler(stateHandle);
            timerObserver.Interval = 300;
            timerObserver.Enabled = false;
        }

        /// <summary>
        /// Handler stavu motoru
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stateHandle(object sender, EventArgs e)
        {
            MotorState newState;
            if (!sm.GetDisableState())
            {
                int velocity = stateHandler.GetVelocityIs();
                if (Math.Abs(velocity) > 50)
                {
                    newState = MotorState.running;
                }
                else
                {
                    newState = MotorState.enabled;
                }
                if (newState != state)
                {
                    state = newState;
                    stateObserver(state, "", id, true, velocity);
                }
            }
        }
    }
}
