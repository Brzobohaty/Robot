using EposCmd.Net;
using EposCmd.Net.DeviceCmdSet.Operation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private Action<string, string, string> stateObserver; //callback pro stav motoru
        private int rev = 1; //modifikátor směru [1, -1]
        private string id; //id motoru

        /// <summary>
        /// Inicializace motoru
        /// </summary>
        /// <param name="connector">connector sběrnice</param>
        /// <param name="stateObserver">posluchač stavu motoru</param>
        /// <param name="nodeNumber">číslo node</param>
        /// <param name="id">id motoru ["PP_P", "LP_P", "LZ_P", "PZ_P", "PP_R", "LP_R", "LZ_R", "PZ_R", "PP_Z", "LP_Z", "LZ_Z", "PZ_Z", "PP_ZK", "LP_ZK", "LZ_ZK", "PZ_ZK"]</param>
        /// <param name="mode">defaultní nastavení módu ["velocity","position"]</param>
        /// <param name="reverse">příznak obrácení směru točení</param>
        public void inicialize(DeviceManager connector, Action<string, string, string> stateObserver, int nodeNumber, string id, string mode, bool reverse)
        {
            try
            {
                this.mode = mode;
                this.stateObserver = stateObserver;
                this.id = id;

                if (reverse) {
                    rev = -1;
                }

                motor = connector.CreateDevice(Convert.ToUInt16(nodeNumber));

                sm = motor.Operation.StateMachine;
                if (sm.GetFaultState())
                    sm.ClearFault();
                sm.SetEnableState();

                velocityHandler = motor.Operation.ProfileVelocityMode;
                positionHandler = motor.Operation.ProfilePositionMode;
                switch (mode)
                {
                    case "velocity": velocityHandler.ActivateProfileVelocityMode(); break;
                    case "position": positionHandler.ActivateProfilePositionMode(); break;
                }

                stateObserver("enabled", "", id);
            }
            catch (DeviceException e)
            {
                stateObserver("error", String.Format("{0}\nErrorCode: {1:X8}", e.ErrorMessage, e.ErrorCode), id);
            }
            catch (Exception e)
            {
                stateObserver("error", e.Message, id);
            }
        }

        /// <summary>
        /// Pohnutí s motorem
        /// </summary>
        /// <param name="speed">rychlost -100 až 100</param>
        public void move(int speed)
        {
            try
            {
                velocityHandler.MoveWithVelocity(speed * rev * 10);
            }
            catch (DeviceException e)
            {
                stateObserver("error", String.Format("{0}\nErrorCode: {1:X8}", e.ErrorMessage, e.ErrorCode), id);
            }
            catch (Exception e)
            {
                stateObserver("error", e.Message, id);
            }
        }

        /// <summary>
        /// Pohnutí s motorem
        /// </summary>
        /// <param name="speed">rychlost -100 až 100</param>
        /// <param name="position">pozice 0 až 360</param>
        public void move(int speed, int position)
        {
            try
            {
                positionHandler.MoveToPosition(Convert.ToInt32(position), true, true);
            }
            catch (DeviceException e)
            {
                stateObserver("error", String.Format("{0}\nErrorCode: {1:X8}", e.ErrorMessage, e.ErrorCode), id);
            }
            catch (Exception e)
            {
                stateObserver("error", e.Message, id);
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
            if (sm != null){
                if (sm.GetFaultState())
                    sm.ClearFault();

                if (!sm.GetDisableState())
                    sm.SetDisableState();
            }
        }
    }
}
