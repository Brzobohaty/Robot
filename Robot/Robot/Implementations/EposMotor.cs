﻿using EposCmd.Net;
using EposCmd.Net.DeviceCmdSet.Operation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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
        private MotorMode mode; //mód ve kterém se zrovna nachází handler motoru
        private ProfileVelocityMode velocityHandler; //handler pro rychlostní operace s motorem 
        private ProfilePositionMode positionHandler; //handler pro pozicové operace s motorem
        private HomingMode homingHandler; //handler pro homing motoru
        private StateObserver stateObserver; //posluchač pro stav motoru
        private int rev = 1; //modifikátor směru [1, -1]
        private MotorId id; //id motoru
        private MotorState state; //aktuální stav motoru
        private MotionInfo stateHandler; //handler stavu motoru 
        private Timer timerObserver; //časovač pro spouštění posluchače stavu
        private int multiplier; //násobitel otáček
        private int oldVelocity = 0; //stará rychlost
        private int defaultPosition = 0; //defaultní pozice
        private int maxPosition; //maximální pozice na motoru
        private int minPosition; //minimální pozice na motoru
        private bool hasLimit = false; //příznak, zda má motor maximální a minimální hranici pohybu

        /// <summary>
        /// Inicializace motoru
        /// </summary>
        /// <param name="connector">connector sběrnice</param>
        /// <param name="stateObserver">posluchač stavu motoru</param>
        /// <param name="nodeNumber">číslo node</param>
        /// <param name="id">id motoru</param>
        /// <param name="mode">defaultní nastavení módu</param>
        /// <param name="reverse">příznak obrácení směru točení</param>
        /// <param name="multiplier">násobitel otáček v případě, že je motor za převodovkou</param>
        /// <param name="minPosition">minimální pozice motoru</param>
        /// <param name="maxPosition">maximální pozice motoru</param>
        public void inicialize(DeviceManager connector, StateObserver stateObserver, int nodeNumber, MotorId id, MotorMode mode, bool reverse, int multiplier, int minPosition, int maxPosition)
        {
            hasLimit = true;
            this.maxPosition = maxPosition;
            this.minPosition = minPosition;
            inicialize(connector, stateObserver, nodeNumber, id, mode, reverse, multiplier);
        }

        /// <summary>
        /// Inicializace motoru
        /// </summary>
        /// <param name="connector">connector sběrnice</param>
        /// <param name="stateObserver">posluchač stavu motoru</param>
        /// <param name="nodeNumber">číslo node</param>
        /// <param name="id">id motoru</param>
        /// <param name="mode">defaultní nastavení módu</param>
        /// <param name="reverse">příznak obrácení směru točení</param>
        /// <param name="multiplier">násobitel otáček v případě, že je motor za převodovkou</param>
        public void inicialize(DeviceManager connector, StateObserver stateObserver, int nodeNumber, MotorId id, MotorMode mode, bool reverse, int multiplier)
        {
            try
            {
                this.mode = mode;
                this.stateObserver = stateObserver;
                this.id = id;
                this.multiplier = multiplier;

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
                homingHandler = motor.Operation.HomingMode;
                changeMode(mode);

                setStateObserver();
                stateObserver.motorStateChanged(MotorState.enabled, "", id, 0);
            }
            catch (DeviceException e)
            {
                sm = null;
                disableStateObserver();
                stateObserver.motorStateChanged(MotorState.error, String.Format("{0}\nErrorCode: {1:X8}", e.ErrorMessage, e.ErrorCode), id, 0);
            }
            catch (Exception e)
            {
                sm = null;
                disableStateObserver();
                stateObserver.motorStateChanged(MotorState.error, e.Message, id, 0);
            }
        }

        /// <summary>
        /// Přepnutí módu motoru
        /// </summary>
        /// <param name="mode">mód</param>
        public void changeMode(MotorMode mode)
        {
            if (velocityHandler != null && positionHandler != null && homingHandler != null)
            {
                switch (mode)
                {
                    case MotorMode.velocity: velocityHandler.ActivateProfileVelocityMode(); break;
                    case MotorMode.position: positionHandler.ActivateProfilePositionMode(); break;
                    case MotorMode.homing: homingHandler.ActivateHomingMode(); break;
                }
            }
        }

        /// <summary>
        /// Pohnutí s motorem
        /// </summary>
        /// <param name="speed">rychlost -100 až 100</param>
        public void moving(int speed)
        {
            if (velocityHandler != null && stateHandler != null && !hasLimit)
            {
                    try
                    {
                        velocityHandler.MoveWithVelocity(speed * rev * 10);
                    }
                    catch (DeviceException e)
                    {
                        stateObserver.motorStateChanged(MotorState.error, String.Format("{0}\nErrorCode: {1:X8}", e.ErrorMessage, e.ErrorCode), id, 0);
                    }
                    catch (Exception e)
                    {
                        stateObserver.motorStateChanged(MotorState.error, e.Message, id, 0);
                    }
            }
        }

        /// <summary>
        /// Pohnutí s motorem o daný krok
        /// </summary>
        /// <param name="step">krok posunutí v qc</param>
        public void move(int step)
        {
            if (positionHandler != null && stateHandler != null)
            {
                int position = stateHandler.GetPositionIs() + step;
                if (!hasLimit || (hasLimit && position > minPosition && position < maxPosition))
                {
                    try
                    {
                        positionHandler.MoveToPosition(Convert.ToInt32(step * multiplier), false, false);
                    }
                    catch (DeviceException e)
                    {
                        stateObserver.motorStateChanged(MotorState.error, String.Format("{0}\nErrorCode: {1:X8}", e.ErrorMessage, e.ErrorCode), id, 0);
                    }
                    catch (Exception e)
                    {
                        stateObserver.motorStateChanged(MotorState.error, e.Message, id, 0);
                    }
                }
                else
                {
                    SystemSounds.Beep.Play();
                }
            }
        }

        /// <summary>
        /// Pohnutí s motorem na danou pozici vzhledem k homingu
        /// </summary>
        /// <param name="position">absolutní pozice</param>
        public void moveToPosition(int position)
        {
            if (positionHandler != null && stateHandler != null)
            {
                if (!hasLimit || (hasLimit && position > minPosition && position < maxPosition))
                {
                    try
                    {
                        positionHandler.MoveToPosition(Convert.ToInt32(position), true, false);
                    }
                    catch (DeviceException e)
                    {
                        stateObserver.motorStateChanged(MotorState.error, String.Format("{0}\nErrorCode: {1:X8}", e.ErrorMessage, e.ErrorCode), id, 0);
                    }
                    catch (Exception e)
                    {
                        stateObserver.motorStateChanged(MotorState.error, e.Message, id, 0);
                    }
                }
                else
                {
                    SystemSounds.Beep.Play();
                }
            }
        }

        /// <summary>
        /// Vrátí aktuální reálnou pozici
        /// </summary>
        /// <exception cref="DeviceException">Pokud motor nedokáže získat hodnot, protože je v chybě.</exception>
        /// <returns>pozice 0 až 360</returns>
        public int getPosition()
        {
            if (stateHandler != null)
            {
                return stateHandler.GetPositionIs();
            }
            else
            {
                throw new DeviceException("No handler");
            }
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
            if (stateObserver != null)
            {
                stateObserver.motorStateChanged(MotorState.disabled, "", id, 0);
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
        /// Nastaví aktuální pozici motoru jako nulovou (počáteční)
        /// </summary>
        public void setActualPositionAsHoming()
        {
            setHomingPosition(0);
        }

        /// <summary>
        /// Nastaví aktuální pozici motoru
        /// </summary>
        /// <param name="position">aktuální pozice motoru</param>
        public void setHomingPosition(int position)
        {
            if (homingHandler != null)
            {
                MotorMode previouseMode = mode;
                changeMode(MotorMode.homing);
                homingHandler.DefinePosition(position);
                changeMode(previouseMode);
            }
        }

        /// <summary>
        /// Nastaví motor do defaultní pozice
        /// </summary>
        public void setDefaultPosition()
        {
            MotorMode previouseMode = mode;
            changeMode(MotorMode.position);
            moveToPosition(defaultPosition);
            changeMode(previouseMode);
        }

        /// <summary>
        /// Nastaví současnou polohu motoru jako defaultní
        /// </summary>
        public void setCurrentPositionAsDefault()
        {
            if (stateHandler != null)
            {
                defaultPosition = stateHandler.GetPositionIs();
            }
        }

        /// <summary>
        /// Nastavení timeru pro dotazování stavu motoru
        /// </summary>
        private void setStateObserver()
        {
            timerObserver = new Timer();
            timerObserver.Elapsed += new ElapsedEventHandler(stateHandle);
            timerObserver.Interval = 100;
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
                if (sm.GetFaultState())
                {
                    newState = MotorState.error;
                    if (newState != state)
                    {
                        state = newState;
                        stateObserver.motorStateChanged(state, "", id, 0);
                    }
                }
                else
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
                    if (newState != state || Math.Abs(velocity) > 50)
                    {
                        state = newState;
                        oldVelocity = velocity;
                        stateObserver.motorStateChanged(state, "", id, velocity);
                    }
                }
            }
            else
            {
                newState = MotorState.disabled;
                if (newState != state)
                {
                    state = newState;
                    stateObserver.motorStateChanged(state, "", id, 0);
                }
            }
        }
    }
}
