using EposCmd.Net;
using EposCmd.Net.DeviceCmdSet.Operation;
using Robot.Robot.Implementations.Epos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Robot.Robot.Implementations.Epos
{
    /// <summary>
    /// Motor v HW implementací od firmy EPOS
    /// </summary>
    class EposMotor : IMotor
    {
        public MotorState state { get; private set; } //aktuální stav motoru
        public int angle { get; private set; } //aktuální úhel natočení motoru
        public int minAngle { get; private set; } //minimální úhel natočení motoru
        public int maxAngle { get; private set; } //maximální úhel natočení motoru
        public int targetPosition { get; private set; } //pozice, které má v současnou chvíli motor dosáhnout
        public int targetAngle { get; private set; } //úhel, kterého má v současnou chvíli motor dosáhnout
        private Device motor; //ovladač motoru
        private StateMachine sm; //ovladač stavu motoru
        private MotorMode mode; //mód ve kterém se zrovna nachází handler motoru
        private ProfileVelocityMode velocityHandler; //handler pro rychlostní operace s motorem 
        private ProfilePositionMode positionHandler; //handler pro pozicové operace s motorem
        private HomingMode homingHandler; //handler pro homing motoru
        private IStateObserver stateObserver; //posluchač pro stav motoru
        private Action motorErrorOccuredObserver; //posluchač chyb na motoru
        private int rev = 1; //modifikátor směru [1, -1]
        private MotorId id; //id motoru
        private MotionInfo stateHandler; //handler stavu motoru 
        private Timer timerObserver; //časovač pro spouštění posluchače stavu
        private int multiplier; //násobitel otáček
        private int maxPosition; //maximální pozice na motoru
        private int minPosition; //minimální pozice na motoru
        private bool hasPositionLimit = false; //příznak, zda má motor maximální a minimální hranici pohybu
        private bool limitEnable = true; //příznak, zda má motor zaplou kontrolu limitů
        private int maxSpeed; //maximální rychlost motoru
        private EposErrorCode errorDictionary; //slovník pro překlad z error kódů do zpráv
        private int lastPosition = 0; //poslední pozice motoru

        /// <summary>
        /// Pole definující vztah úhlu ze škály uvedené u motoru a opravdového úhlu a pozicí motoru
        /// Jeden řádek obsahuje {úhel ze škály ve stupních; opravdový úhel; pozice motoru}
        /// </summary>
        private int[,] angleMap;

        public EposMotor()
        {
            errorDictionary = EposErrorCode.getInstance();
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
        /// <param name="velocity">maximální rychlost motoru při rychlostním řízení</param>
        /// <param name="aceleration">zrychlení motoru při rychlostním řízení</param>
        /// <param name="deceleration">zpomalení motoru při rychlostním řízení</param>
        /// <param name="minPosition">minimální pozice motoru</param>
        /// <param name="maxPosition">maximální pozice motoru</param>
        /// <param name="angleMap">Pole definující vztah úhlu ze škály uvedené u motoru a opravdového úhlu a pozicí motoru. Jeden řádek obsahuje {úhel ze škály ve stupních; opravdový úhel; pozice motoru}</param>
        public void inicialize(DeviceManager connector, IStateObserver stateObserver, Action motorErrorOccuredObserver, int nodeNumber, MotorId id, MotorMode mode, bool reverse, int multiplier, uint positionVelocity, uint positionAceleration, uint positionDeceleration, uint velocity, uint aceleration, uint deceleration, int[,] angleMap)
        {
            this.angleMap = angleMap;
            inicialize(connector, stateObserver, motorErrorOccuredObserver, nodeNumber, id, mode, reverse, multiplier, positionVelocity, positionAceleration, positionDeceleration, velocity, aceleration, deceleration, angleMap[angleMap.GetLength(0) - 1, 2], angleMap[0, 2], angleMap[angleMap.GetLength(0) - 1, 1], angleMap[0, 1]);
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
        /// <param name="velocity">maximální rychlost motoru při rychlostním řízení</param>
        /// <param name="aceleration">zrychlení motoru při rychlostním řízení</param>
        /// <param name="deceleration">zpomalení motoru při rychlostním řízení</param>
        /// <param name="minPosition">minimální pozice motoru</param>
        /// <param name="maxPosition">maximální pozice motoru</param>
        public void inicialize(DeviceManager connector, IStateObserver stateObserver, Action motorErrorOccuredObserver, int nodeNumber, MotorId id, MotorMode mode, bool reverse, int multiplier, uint positionVelocity, uint positionAceleration, uint positionDeceleration, uint velocity, uint aceleration, uint deceleration, int minPosition, int maxPosition, int minAngle, int maxAngle)
        {
            hasPositionLimit = true;
            this.maxPosition = maxPosition;
            this.minPosition = minPosition;
            this.minAngle = minAngle;
            this.maxAngle = maxAngle;
            inicialize(connector, stateObserver, motorErrorOccuredObserver, nodeNumber, id, mode, reverse, multiplier, positionVelocity, positionAceleration, positionDeceleration, velocity, aceleration, deceleration);
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
        /// <param name="velocity">maximální rychlost motoru při rychlostním řízení</param>
        /// <param name="aceleration">zrychlení motoru při rychlostním řízení</param>
        /// <param name="deceleration">zpomalení motoru při rychlostním řízení</param>
        public void inicialize(DeviceManager connector, IStateObserver stateObserver, Action motorErrorOccuredObserver, int nodeNumber, MotorId id, MotorMode mode, bool reverse, int multiplier, uint positionVelocity, uint positionAceleration, uint positionDeceleration, uint velocity, uint aceleration, uint deceleration)
        {
            try
            {
                this.mode = mode;
                this.stateObserver = stateObserver;
                this.motorErrorOccuredObserver = motorErrorOccuredObserver;
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

                maxSpeed = (int)velocity;
                velocityHandler = motor.Operation.ProfileVelocityMode;
                velocityHandler.SetVelocityProfile(aceleration, deceleration);
                positionHandler = motor.Operation.ProfilePositionMode;
                positionHandler.SetPositionProfile(positionVelocity, positionAceleration, positionDeceleration);
                homingHandler = motor.Operation.HomingMode;
                changeMode(mode);

                setStateObserver();
                state = MotorState.enabled;
                stateObserver.motorStateChanged(MotorState.enabled, "", id, 0, 0, 0, 0);
                targetPosition = stateHandler.GetPositionIs();
                targetAngle = getAngleFromPosition(targetPosition);
            }
            catch (DeviceException e)
            {
                sm = null;
                disableStateObserver();
                state = MotorState.error;
                stateObserver.motorStateChanged(MotorState.error, String.Format("{0}\nError: {1}", e.ErrorMessage, errorDictionary.getErrorMessage(e.ErrorCode)), id, 0, 0, 0, 0);
            }
        }

        /// <summary>
        /// Nastavení parametrů motoru
        /// </summary>
        /// <param name="positionVeocity">rychlost motoru v otáčkách při pozicování</param>
        /// <param name="positionAceleration">zrychlení motoru v otáčkách při pozicování</param>
        /// <param name="positionDeceleration">zpomalení motoru v otáčkách při pozicování</param>
        /// <param name="velocity">maximální rychlost motoru při rychlostním řízení</param>
        /// <param name="aceleration">zrychlení motoru při rychlostním řízení</param>
        /// <param name="deceleration">zpomalení motoru při rychlostním řízení</param>
        public void setParameters(uint positionVelocity, uint positionAceleration, uint positionDeceleration, uint velocity, uint aceleration, uint deceleration)
        {
            maxSpeed = (int)velocity;
            if (velocityHandler != null)
            {
                velocityHandler.SetVelocityProfile(aceleration, deceleration);
            }
            if (positionHandler != null)
            {
                positionHandler.SetPositionProfile(positionVelocity, positionAceleration, positionDeceleration);
            }
        }

        /// <summary>
        /// Přepnutí módu motoru
        /// </summary>
        /// <param name="mode">mód</param>
        public void changeMode(MotorMode mode)
        {
            this.mode = mode;
            if (velocityHandler != null && positionHandler != null && homingHandler != null)
            {
                try
                {
                    switch (mode)
                    {
                        case MotorMode.velocity: velocityHandler.ActivateProfileVelocityMode(); break;
                        case MotorMode.position: positionHandler.ActivateProfilePositionMode(); break;
                        case MotorMode.homing: homingHandler.ActivateHomingMode(); break;
                    }
                }
                catch (DeviceException e)
                {
                    state = MotorState.error;
                    stateObserver.motorStateChanged(MotorState.error, String.Format("{0}\nError: {1}", e.ErrorMessage, errorDictionary.getErrorMessage(e.ErrorCode)), id, 0, 0, 0, 0);
                    motorErrorOccuredObserver();
                }
            }
        }

        /// <summary>
        /// Pohnutí s motorem
        /// </summary>
        /// <param name="speed">rychlost -100 až 100</param>
        public void moving(int speed)
        {
            if (velocityHandler != null && stateHandler != null && !hasPositionLimit)
            {
                speed = MathLibrary.changeScale(speed, -100, 100, -maxSpeed, maxSpeed);
                try
                {
                    velocityHandler.MoveWithVelocity(speed * rev);
                }
                catch (DeviceException e)
                {
                    state = MotorState.error;
                    stateObserver.motorStateChanged(MotorState.error, String.Format("{0}\nError: {1}", e.ErrorMessage, errorDictionary.getErrorMessage(e.ErrorCode)), id, 0, 0, 0, 0);
                    motorErrorOccuredObserver();
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
                try
                {
                    int position = stateHandler.GetPositionIs() + (step * rev * multiplier);
                    moveToPosition(position);
                }
                catch (DeviceException e)
                {
                    state = MotorState.error;
                    stateObserver.motorStateChanged(MotorState.error, String.Format("{0}\nError: {1}", e.ErrorMessage, errorDictionary.getErrorMessage(e.ErrorCode)), id, 0, 0, 0, 0);
                    motorErrorOccuredObserver();
                }
            }
        }

        /// <summary>
        /// Pohnutí s motorem na danou pozici vzhledem k homingu
        /// </summary>
        /// <param name="position">absolutní pozice</param>
        public void moveToPosition(int position)
        {
            if (limitEnable && hasPositionLimit && position <= minPosition)
            {
                position = minPosition;
                SystemSounds.Beep.Play();
            }
            if (limitEnable && hasPositionLimit && position >= maxPosition)
            {
                position = maxPosition;
                SystemSounds.Beep.Play();
            }
            try
            {
                targetPosition = position;
                targetAngle = getAngleFromPosition(targetPosition);
                positionHandler.MoveToPosition(Convert.ToInt32(position), true, true);
            }
            catch (DeviceException e)
            {
                state = MotorState.error;
                stateObserver.motorStateChanged(MotorState.error, String.Format("{0}\nError: {1}", e.ErrorMessage, errorDictionary.getErrorMessage(e.ErrorCode)), id, 0, 0, 0, 0);
                motorErrorOccuredObserver();
            }
        }

        /// <summary>
        /// Pohnutí s motorem do daného úhlu
        /// </summary>
        /// <param name="angle">úhel do kter0ho se m8 motor nastavit vyhledem k jeho 0</param>
        public void moveToAngle(int angle)
        {
            if (!hasPositionLimit)
            {
                return;
            }
            if (angle <= minAngle)
            {
                angle = minAngle;
            }
            if (angle >= maxAngle)
            {
                angle = maxAngle;
            }

            moveToPosition(getPositionFromAngle(angle));
        }

        /// <summary>
        /// Indikace, zda již motor dorazil do stanovené polohy
        /// </summary>
        /// <returns>true pokud se motor již dostal do cíle</returns>
        public bool isTargetReached()
        {
            if (stateHandler != null)
            {
                try
                {
                    bool targetReached = false;
                    stateHandler.GetMovementState(ref targetReached);
                    return targetReached;
                }
                catch (DeviceException e)
                {
                    state = MotorState.error;
                    stateObserver.motorStateChanged(MotorState.error, String.Format("{0}\nError: {1}", e.ErrorMessage, errorDictionary.getErrorMessage(e.ErrorCode)), id, 0, 0, 0, 0);
                    motorErrorOccuredObserver();
                }
            }
            return false;
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
                try
                {
                    return stateHandler.GetPositionIs();
                }
                catch (DeviceException e)
                {
                    state = MotorState.error;
                    stateObserver.motorStateChanged(MotorState.error, String.Format("{0}\nError: {1}", e.ErrorMessage, errorDictionary.getErrorMessage(e.ErrorCode)), id, 0, 0, 0, 0);
                    motorErrorOccuredObserver();
                }
            }
            throw new DeviceException("No handler");
        }

        /// <summary>
        /// Vypne motor
        /// </summary>
        public void disable()
        {
            try
            {
                if (timerObserver != null)
                {
                    timerObserver.Stop();
                }
                if (sm != null)
                {
                    if (sm.GetFaultState())
                        sm.ClearFault();

                    if (!sm.GetDisableState())
                    {
                        state = MotorState.disabled;
                        sm.SetDisableState();
                    }
                }
                if (stateObserver != null)
                {
                    if (state != MotorState.error)
                    {
                        stateObserver.motorStateChanged(MotorState.disabled, "", id, 0, 0, 0, 0);
                    }
                }
            }
            catch (DeviceException e)
            {
                state = MotorState.error;
                stateObserver.motorStateChanged(MotorState.error, String.Format("{0}\nError: {1}", e.ErrorMessage, errorDictionary.getErrorMessage(e.ErrorCode)), id, 0, 0, 0, 0);
            }
        }

        /// <summary>
        /// Zjistí, zda je motor vypnutý
        /// </summary>
        /// <returns>true pokud je vypnutý</returns>
        public bool isDisabled()
        {
            if (state == MotorState.disabled)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Zapne motor
        /// </summary>
        public void enable()
        {
            try
            {
                if (timerObserver != null)
                {
                    timerObserver.Start();
                }
                if (sm != null)
                {
                    if (sm.GetFaultState())
                        sm.ClearFault();

                    if (!sm.GetEnableState())
                        sm.SetEnableState();
                }
                if (stateObserver != null)
                {
                    if (state != MotorState.error)
                    {
                        stateObserver.motorStateChanged(MotorState.enabled, "", id, 0, 0, 0, 0);
                    }
                }
            }
            catch (DeviceException e)
            {
                state = MotorState.error;
                stateObserver.motorStateChanged(MotorState.error, String.Format("{0}\nError: {1}", e.ErrorMessage, errorDictionary.getErrorMessage(e.ErrorCode)), id, 0, 0, 0, 0);
                motorErrorOccuredObserver();
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
                try
                {
                    homingHandler.DefinePosition(position);
                }
                catch (DeviceException e)
                {
                    state = MotorState.error;
                    stateObserver.motorStateChanged(MotorState.error, String.Format("{0}\nError: {1}", e.ErrorMessage, errorDictionary.getErrorMessage(e.ErrorCode)), id, 0, 0, 0, 0);
                    motorErrorOccuredObserver();
                }
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
            try
            {
                moveToPosition((int)Properties.Settings.Default[id.ToString() + "_default"]);
            }
            catch (DeviceException e)
            {
                state = MotorState.error;
                stateObserver.motorStateChanged(MotorState.error, String.Format("{0}\nError: {1}", e.ErrorMessage, errorDictionary.getErrorMessage(e.ErrorCode)), id, 0, 0, 0, 0);
                motorErrorOccuredObserver();
            }
            changeMode(previouseMode);
        }

        /// <summary>
        /// Nastaví současnou polohu motoru jako defaultní
        /// </summary>
        public void setCurrentPositionAsDefault()
        {
            if (stateHandler != null)
            {
                try
                {
                    Properties.Settings.Default[id.ToString() + "_default"] = stateHandler.GetPositionIs();
                    Properties.Settings.Default.Save();
                }
                catch (DeviceException e)
                {
                    state = MotorState.error;
                    stateObserver.motorStateChanged(MotorState.error, String.Format("{0}\nError: {1}", e.ErrorMessage, errorDictionary.getErrorMessage(e.ErrorCode)), id, 0, 0, 0, 0);
                    motorErrorOccuredObserver();
                }
            }
        }

        /// <summary>
        /// Vypne/zapne ochranu dojezdu motoru
        /// </summary>
        /// <param name="on">true pokud zapnout</param>
        public void limitProtectionOnOff(bool on)
        {
            limitEnable = on;
        }

        /// <summary>
        /// Zastaví motor
        /// </summary>
        public void halt()
        {
            try
            {
                switch (mode)
                {
                    case MotorMode.position:
                        positionHandler.HaltPositionMovement();
                        break;
                    case MotorMode.velocity:
                        velocityHandler.HaltVelocityMovement();
                        break;
                }
            }
            catch (DeviceException e)
            {
                state = MotorState.error;
                stateObserver.motorStateChanged(MotorState.error, String.Format("{0}\nError: {1}", e.ErrorMessage, errorDictionary.getErrorMessage(e.ErrorCode)), id, 0, 0, 0, 0);
                motorErrorOccuredObserver();
            }
        }

        /// <summary>
        /// Pohnen s motorem do maximální koncové pozice
        /// </summary>
        public void moveToMaxPosition()
        {
            moveToPosition(maxPosition - 1);
        }

        /// <summary>
        /// Pohnen s motorem do minimální koncové pozice
        /// </summary>
        public void moveToMinPosition()
        {
            moveToPosition(minPosition + 1);
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
        /// <param name="ev"></param>
        private void stateHandle(object sender, EventArgs ev)
        {
            try
            {
                if (!sm.GetDisableState())
                {
                    if (sm.GetFaultState())
                    {
                        state = MotorState.error;
                        stateObserver.motorStateChanged(state, "Motor is in fault state", id, 0, 0, 0, 0);
                        motorErrorOccuredObserver();
                    }
                    else
                    {
                        int velocity = stateHandler.GetVelocityIs();
                        int position = stateHandler.GetPositionIs();
                        angle = getAngleFromPosition(position);
                        if (Math.Abs(velocity) > 50)
                        {
                            state = MotorState.running;
                        }
                        else
                        {
                            state = MotorState.enabled;
                        }
                        int speedRelative = MathLibrary.changeScale(velocity, 0, (int)maxSpeed, 0, 100);
                        stateObserver.motorStateChanged(state, "", id, velocity, position, speedRelative, angle);
                        lastPosition = position;
                    }
                }
                else
                {
                    state = MotorState.disabled;
                    stateObserver.motorStateChanged(state, "", id, 0, 0, 0, 0);
                }
            }
            catch (DeviceException e)
            {
                state = MotorState.error;
                stateObserver.motorStateChanged(MotorState.error, String.Format("{0}\nError: {1}", e.ErrorMessage, errorDictionary.getErrorMessage(e.ErrorCode)), id, 0, 0, 0, 0);
                motorErrorOccuredObserver();
            }
        }

        /// <summary>
        /// Vypočítá úhel motoru při dané pozici
        /// </summary>
        /// <param name="position">pozice motoru</param>
        /// <returns>úhel motoru</returns>
        private int getAngleFromPosition(int position)
        {
            for (int i = 0; angleMap != null && i < angleMap.GetLength(0); i++)
            {
                if (position == angleMap[i, 2])
                {
                    return angleMap[i, 1];
                }
                if (position < angleMap[i, 2] && position > angleMap[i + 1, 2])
                {
                    return (int)MathLibrary.linearInterpolation(position, angleMap[i, 2], angleMap[i + 1, 2], angleMap[i, 1], angleMap[i + 1, 1]);
                }
            }
            return MathLibrary.changeScale(position, minPosition, maxPosition, minAngle, maxAngle);
        }

        /// <summary>
        /// Vypočítá pozici motoru při daném úhlu
        /// </summary>
        /// <param name="angle">úhel motoru</param>
        /// <returns>pozice motoru</returns>
        private int getPositionFromAngle(int angle)
        {
            for (int i = 0; angleMap!=null && i < angleMap.GetLength(0); i++)
            {
                if (angle == angleMap[i, 1])
                {
                    return angleMap[i, 2];
                }
                if (angle < angleMap[i, 1] && angle > angleMap[i + 1, 1])
                {
                    return (int)MathLibrary.linearInterpolation(angle, angleMap[i, 1], angleMap[i + 1, 1], angleMap[i, 2], angleMap[i + 1, 2]);
                }
            }
            return MathLibrary.changeScale(angle, minAngle, maxAngle, minPosition, maxPosition);
        }
    }
}
