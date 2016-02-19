using EposCmd.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Robot.Implementations.Test
{
    /// <summary>
    /// Představuje softwarovou simulaci motoru
    /// </summary>
    class TestMotor : IMotor
    {
        public MotorState state { get; private set; } //aktuální stav motoru
        public int angle { get; private set; } //aktuální úhel natočení motoru
        public int minAngle { get; private set; } //minimální úhel natočení motoru
        public int maxAngle { get; private set; } //maximální úhel natočení motoru

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
        /// <param name="positionVeocity">rychlost motoru v otáčkách při pozicování</param>
        /// <param name="positionAceleration">zrychlení motoru v otáčkách při pozicování</param>
        /// <param name="positionDeceleration">zpomalení motoru v otáčkách při pozicování</param>
        /// <param name="minPosition">minimální pozice motoru</param>
        /// <param name="maxPosition">maximální pozice motoru</param>
        public void inicialize(DeviceManager connector, StateObserver stateObserver, Action motorErrorOccuredObserver, int nodeNumber, MotorId id, MotorMode mode, bool reverse, int multiplier, uint positionVelocity, uint positionAceleration, uint positionDeceleration, int minPosition, int maxPosition, int minAngle, int maxAngle)
        {
            //hasPositionLimit = true;
            //this.maxPosition = maxPosition;
            //this.minPosition = minPosition;
            //this.minAngle = minAngle;
            //this.maxAngle = maxAngle;
            //inicialize(connector, stateObserver, motorErrorOccuredObserver, nodeNumber, id, mode, reverse, multiplier, positionVelocity, positionAceleration, positionDeceleration);
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
        /// <param name="positionVeocity">rychlost motoru v otáčkách při pozicování</param>
        /// <param name="positionAceleration">zrychlení motoru v otáčkách při pozicování</param>
        /// <param name="positionDeceleration">zpomalení motoru v otáčkách při pozicování</param>
        public void inicialize(DeviceManager connector, StateObserver stateObserver, Action motorErrorOccuredObserver, int nodeNumber, MotorId id, MotorMode mode, bool reverse, int multiplier, uint positionVelocity, uint positionAceleration, uint positionDeceleration)
        {
            //try
            //{
            //    this.mode = mode;
            //    this.stateObserver = stateObserver;
            //    this.motorErrorOccuredObserver = motorErrorOccuredObserver;
            //    this.id = id;
            //    this.multiplier = multiplier;

            //    if (reverse)
            //    {
            //        rev = -1;
            //    }

            //    motor = connector.CreateDevice(Convert.ToUInt16(nodeNumber));
            //    stateHandler = motor.Operation.MotionInfo;

            //    sm = motor.Operation.StateMachine;
            //    if (sm.GetFaultState())
            //        sm.ClearFault();
            //    sm.SetEnableState();

            //    velocityHandler = motor.Operation.ProfileVelocityMode;
            //    positionHandler = motor.Operation.ProfilePositionMode;
            //    positionHandler.SetPositionProfile(positionVelocity, positionAceleration, positionDeceleration);
            //    homingHandler = motor.Operation.HomingMode;
            //    changeMode(mode);

            //    setStateObserver();
            //    state = MotorState.enabled;
            //    stateObserver.motorStateChanged(MotorState.enabled, "", id, 0, 0, 0, 0);
            //}
            //catch (DeviceException e)
            //{
            //    sm = null;
            //    disableStateObserver();
            //    state = MotorState.error;
            //    stateObserver.motorStateChanged(MotorState.error, String.Format("{0}\nError: {1}", e.ErrorMessage, errorDictionary.getErrorMessage(e.ErrorCode)), id, 0, 0, 0, 0);
            //}
        }

        /// <summary>
        /// Přepnutí módu motoru
        /// </summary>
        /// <param name="mode">mód</param>
        public void changeMode(MotorMode mode)
        {
            //this.mode = mode;
            //if (velocityHandler != null && positionHandler != null && homingHandler != null)
            //{
            //    try
            //    {
            //        switch (mode)
            //        {
            //            case MotorMode.velocity: velocityHandler.ActivateProfileVelocityMode(); break;
            //            case MotorMode.position: positionHandler.ActivateProfilePositionMode(); break;
            //            case MotorMode.homing: homingHandler.ActivateHomingMode(); break;
            //        }
            //    }
            //    catch (DeviceException e)
            //    {
            //        state = MotorState.error;
            //        stateObserver.motorStateChanged(MotorState.error, String.Format("{0}\nError: {1}", e.ErrorMessage, errorDictionary.getErrorMessage(e.ErrorCode)), id, 0, 0, 0, 0);
            //        motorErrorOccuredObserver();
            //    }
            //}
        }

        /// <summary>
        /// Pohnutí s motorem
        /// </summary>
        /// <param name="speed">rychlost -100 až 100</param>
        public void moving(int speed)
        {
            //if (velocityHandler != null && stateHandler != null && !hasPositionLimit)
            //{
            //    try
            //    {
            //        velocityHandler.MoveWithVelocity(speed * rev * 10);
            //    }
            //    catch (DeviceException e)
            //    {
            //        state = MotorState.error;
            //        stateObserver.motorStateChanged(MotorState.error, String.Format("{0}\nError: {1}", e.ErrorMessage, errorDictionary.getErrorMessage(e.ErrorCode)), id, 0, 0, 0, 0);
            //        motorErrorOccuredObserver();
            //    }
            //}
        }

        /// <summary>
        /// Pohnutí s motorem o daný krok
        /// </summary>
        /// <param name="step">krok posunutí v qc</param>
        public void move(int step)
        {
            //if (positionHandler != null && stateHandler != null)
            //{
            //    try
            //    {
            //        int position = stateHandler.GetPositionIs() + (step * rev * multiplier);
            //        moveToPosition(position);
            //    }
            //    catch (DeviceException e)
            //    {
            //        state = MotorState.error;
            //        stateObserver.motorStateChanged(MotorState.error, String.Format("{0}\nError: {1}", e.ErrorMessage, errorDictionary.getErrorMessage(e.ErrorCode)), id, 0, 0, 0, 0);
            //        motorErrorOccuredObserver();
            //    }
            //}
        }

        /// <summary>
        /// Pohnutí s motorem na danou pozici vzhledem k homingu
        /// </summary>
        /// <param name="position">absolutní pozice</param>
        public void moveToPosition(int position)
        {
            //if (limitEnable && hasPositionLimit && position <= minPosition)
            //{
            //    position = minPosition;
            //    SystemSounds.Beep.Play();
            //}
            //if (limitEnable && hasPositionLimit && position >= maxPosition)
            //{
            //    position = maxPosition;
            //    SystemSounds.Beep.Play();
            //}
            //try
            //{
            //    positionHandler.MoveToPosition(Convert.ToInt32(position), true, true);
            //}
            //catch (DeviceException e)
            //{
            //    state = MotorState.error;
            //    stateObserver.motorStateChanged(MotorState.error, String.Format("{0}\nError: {1}", e.ErrorMessage, errorDictionary.getErrorMessage(e.ErrorCode)), id, 0, 0, 0, 0);
            //    motorErrorOccuredObserver();
            //}
        }

        /// <summary>
        /// Pohnutí s motorem do daného úhlu
        /// </summary>
        /// <param name="angle">úhel do kter0ho se m8 motor nastavit vyhledem k jeho 0</param>
        public void moveToAngle(int angle)
        {
            //if (!hasPositionLimit)
            //{
            //    return;
            //}
            //if (angle <= minAngle)
            //{
            //    angle = minAngle;
            //}
            //if (angle >= maxAngle)
            //{
            //    angle = maxAngle;
            //}
            //moveToPosition(MathLibrary.changeScale(angle, minAngle, maxAngle, minPosition, maxPosition));
        }

        /// <summary>
        /// Indikace, zda již motor dorazil do stanovené polohy
        /// </summary>
        /// <returns>true pokud se motor již dostal do cíle</returns>
        public bool isTargetReached()
        {
            //if (stateHandler != null)
            //{
            //    try
            //    {
            //        bool targetReached = false;
            //        stateHandler.GetMovementState(ref targetReached);
            //        return targetReached;
            //    }
            //    catch (DeviceException e)
            //    {
            //        state = MotorState.error;
            //        stateObserver.motorStateChanged(MotorState.error, String.Format("{0}\nError: {1}", e.ErrorMessage, errorDictionary.getErrorMessage(e.ErrorCode)), id, 0, 0, 0, 0);
            //        motorErrorOccuredObserver();
            //    }
            //}
            //return false;
            return false;
        }

        /// <summary>
        /// Vrátí aktuální reálnou pozici
        /// </summary>
        /// <exception cref="DeviceException">Pokud motor nedokáže získat hodnot, protože je v chybě.</exception>
        /// <returns>pozice 0 až 360</returns>
        public int getPosition()
        {
            //if (stateHandler != null)
            //{
            //    try
            //    {
            //        return stateHandler.GetPositionIs();
            //    }
            //    catch (DeviceException e)
            //    {
            //        state = MotorState.error;
            //        stateObserver.motorStateChanged(MotorState.error, String.Format("{0}\nError: {1}", e.ErrorMessage, errorDictionary.getErrorMessage(e.ErrorCode)), id, 0, 0, 0, 0);
            //        motorErrorOccuredObserver();
            //    }
            //}
            //throw new DeviceException("No handler");
            return 0;
        }

        /// <summary>
        /// Vypne motor
        /// </summary>
        public void disable()
        {
            //try
            //{
            //    if (timerObserver != null)
            //    {
            //        timerObserver.Stop();
            //    }
            //    if (sm != null)
            //    {
            //        if (sm.GetFaultState())
            //            sm.ClearFault();

            //        if (!sm.GetDisableState())
            //            sm.SetDisableState();
            //    }
            //    if (stateObserver != null)
            //    {
            //        if (state != MotorState.error)
            //        {
            //            stateObserver.motorStateChanged(MotorState.disabled, "", id, 0, 0, 0, 0);
            //        }
            //    }
            //}
            //catch (DeviceException e)
            //{
            //    state = MotorState.error;
            //    stateObserver.motorStateChanged(MotorState.error, String.Format("{0}\nError: {1}", e.ErrorMessage, errorDictionary.getErrorMessage(e.ErrorCode)), id, 0, 0, 0, 0);
            //}
        }

        /// <summary>
        /// Zapne motor
        /// </summary>
        public void enable()
        {
            //try
            //{
            //    if (timerObserver != null)
            //    {
            //        timerObserver.Start();
            //    }
            //    if (sm != null)
            //    {
            //        if (sm.GetFaultState())
            //            sm.ClearFault();

            //        if (!sm.GetEnableState())
            //            sm.SetEnableState();
            //    }
            //    if (stateObserver != null)
            //    {
            //        if (state != MotorState.error)
            //        {
            //            stateObserver.motorStateChanged(MotorState.enabled, "", id, 0, 0, 0, 0);
            //        }
            //    }
            //}
            //catch (DeviceException e)
            //{
            //    state = MotorState.error;
            //    stateObserver.motorStateChanged(MotorState.error, String.Format("{0}\nError: {1}", e.ErrorMessage, errorDictionary.getErrorMessage(e.ErrorCode)), id, 0, 0, 0, 0);
            //    motorErrorOccuredObserver();
            //}
        }

        /// <summary>
        /// Zapne periodický posluchač stavu motoru
        /// </summary>
        public void enableStateObserver()
        {
            //if (timerObserver != null)
            //{
            //    timerObserver.Enabled = true;
            //}
        }

        /// <summary>
        /// Vypne periodický posluchač stavu motoru
        /// </summary>
        public void disableStateObserver()
        {
            //if (timerObserver != null)
            //{
            //    timerObserver.Enabled = false;
            //}
        }

        /// <summary>
        /// Nastaví aktuální pozici motoru jako nulovou (počáteční)
        /// </summary>
        public void setActualPositionAsHoming()
        {
            //setHomingPosition(0);
        }

        /// <summary>
        /// Nastaví aktuální pozici motoru
        /// </summary>
        /// <param name="position">aktuální pozice motoru</param>
        public void setHomingPosition(int position)
        {
            //if (homingHandler != null)
            //{
            //    MotorMode previouseMode = mode;
            //    changeMode(MotorMode.homing);
            //    try
            //    {
            //        homingHandler.DefinePosition(position);
            //    }
            //    catch (DeviceException e)
            //    {
            //        state = MotorState.error;
            //        stateObserver.motorStateChanged(MotorState.error, String.Format("{0}\nError: {1}", e.ErrorMessage, errorDictionary.getErrorMessage(e.ErrorCode)), id, 0, 0, 0, 0);
            //        motorErrorOccuredObserver();
            //    }
            //    changeMode(previouseMode);
            //}
        }

        /// <summary>
        /// Nastaví motor do defaultní pozice
        /// </summary>
        public void setDefaultPosition()
        {
            //MotorMode previouseMode = mode;
            //changeMode(MotorMode.position);
            //try
            //{
            //    moveToPosition((int)Properties.Settings.Default[id.ToString() + "_default"]);
            //}
            //catch (DeviceException e)
            //{
            //    state = MotorState.error;
            //    stateObserver.motorStateChanged(MotorState.error, String.Format("{0}\nError: {1}", e.ErrorMessage, errorDictionary.getErrorMessage(e.ErrorCode)), id, 0, 0, 0, 0);
            //    motorErrorOccuredObserver();
            //}
            //changeMode(previouseMode);
        }

        /// <summary>
        /// Nastaví současnou polohu motoru jako defaultní
        /// </summary>
        public void setCurrentPositionAsDefault()
        {
            //if (stateHandler != null)
            //{
            //    try
            //    {
            //        Properties.Settings.Default[id.ToString() + "_default"] = stateHandler.GetPositionIs();
            //        Properties.Settings.Default.Save();
            //    }
            //    catch (DeviceException e)
            //    {
            //        state = MotorState.error;
            //        stateObserver.motorStateChanged(MotorState.error, String.Format("{0}\nError: {1}", e.ErrorMessage, errorDictionary.getErrorMessage(e.ErrorCode)), id, 0, 0, 0, 0);
            //        motorErrorOccuredObserver();
            //    }
            //}
        }

        /// <summary>
        /// Vypne/zapne ochranu dojezdu motoru
        /// </summary>
        /// <param name="on">true pokud zapnout</param>
        public void limitProtectionOnOff(bool on)
        {
            //limitEnable = on;
        }

        /// <summary>
        /// Zastaví motor
        /// </summary>
        public void halt()
        {
            //try
            //{
            //    switch (mode)
            //    {
            //        case MotorMode.position:
            //            positionHandler.HaltPositionMovement();
            //            break;
            //        case MotorMode.velocity:
            //            velocityHandler.HaltVelocityMovement();
            //            break;
            //    }
            //}
            //catch (DeviceException e)
            //{
            //    state = MotorState.error;
            //    stateObserver.motorStateChanged(MotorState.error, String.Format("{0}\nError: {1}", e.ErrorMessage, errorDictionary.getErrorMessage(e.ErrorCode)), id, 0, 0, 0, 0);
            //    motorErrorOccuredObserver();
            //}
        }
    }
}
