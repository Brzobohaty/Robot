using EposCmd.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

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
        public int targetPosition { get; private set; } //pozice, které má v současnou chvíli motor dosáhnout
        public int targetAngle { get; private set; } //úhel, kterého má v současnou chvíli motor dosáhnout
        private MotorMode mode; //mód ve kterém se zrovna nachází handler motoru
        private IStateObserver stateObserver; //posluchač pro stav motoru
        private Action motorErrorOccuredObserver; //posluchač chyb na motoru
        private int rev = 1; //modifikátor směru [1, -1]
        private MotorId id; //id motoru
        private Timer timerObserver; //časovač pro spouštění posluchače stavu
        private int multiplier; //násobitel otáček
        private int maxPosition; //maximální pozice na motoru
        private int minPosition; //minimální pozice na motoru
        private bool hasPositionLimit = false; //příznak, zda má motor maximální a minimální hranici pohybu
        private bool limitEnable = true; //příznak, zda má motor zaplou kontrolu limitů
        private const int maxSpeed = 5000; //maximální rychlost motoru
        private int lastPosition = 0; //poslední pozice motoru
        private Timer simulateTicker;

        //simulovací proměnné
        int speed = 0;
        int position = 0;
        bool targetReached = true;

        /// <summary>
        /// Pole definující vztah úhlu ze škály uvedené u motoru a opravdového úhlu a pozicí motoru
        /// Jeden řádek obsahuje {úhel ze škály ve stupních; opravdový úhel; pozice motoru}
        /// </summary>
        private int[,] angleMap;

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
            this.mode = mode;
            this.stateObserver = stateObserver;
            this.motorErrorOccuredObserver = motorErrorOccuredObserver;
            this.id = id;
            this.multiplier = multiplier;

            if (reverse)
            {
                rev = -1;
            }

            changeMode(mode);

            setStateObserver();
            state = MotorState.enabled;
            stateObserver.motorStateChanged(MotorState.enabled, "", id, 0, 0, 0, 0);
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
            
        }

        /// <summary>
        /// Přepnutí módu motoru
        /// </summary>
        /// <param name="mode">mód</param>
        public void changeMode(MotorMode mode)
        {
            this.mode = mode;
        }

        /// <summary>
        /// Pohnutí s motorem
        /// </summary>
        /// <param name="speed">rychlost -100 až 100</param>
        public void moving(int speed)
        {
            if (speed != 0)
            {
                createSimulateTicker();
                simulateTicker.Elapsed += delegate { simulateMove(Math.Sign(speed*rev)); };
            }
            else
            {
                disposeSimulateTicker();
            }
        }

        /// <summary>
        /// Pohnutí s motorem o daný krok
        /// </summary>
        /// <param name="step">krok posunutí v qc</param>
        public void move(int step)
        {
            int position = this.position + (step * rev * multiplier);
            moveToPosition(position);
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
            createSimulateTicker();
            targetPosition = position;
            targetAngle = getAngleFromPosition(targetPosition);
            simulateTicker.Elapsed += delegate { simulateMoveToPosition(position); };
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
            return targetReached;
        }

        /// <summary>
        /// Vrátí aktuální reálnou pozici
        /// </summary>
        /// <exception cref="DeviceException">Pokud motor nedokáže získat hodnot, protože je v chybě.</exception>
        /// <returns>pozice 0 až 360</returns>
        public int getPosition()
        {
            return position;
        }

        /// <summary>
        /// Vypne motor
        /// </summary>
        public void disable()
        {
            disposeSimulateTicker();

            if (timerObserver != null)
            {
                timerObserver.Stop();
            }
            state = MotorState.disabled;
            stateObserver.motorStateChanged(MotorState.disabled, "", id, 0, 0, 0, 0);
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

            if (timerObserver != null)
            {
                timerObserver.Start();
            }
            state = MotorState.enabled;
            stateObserver.motorStateChanged(MotorState.enabled, "", id, 0, 0, 0, 0);
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
            MotorMode previouseMode = mode;
            changeMode(MotorMode.homing);
            this.position = position;
            changeMode(previouseMode);
        }

        /// <summary>
        /// Nastaví motor do defaultní pozice
        /// </summary>
        public void setDefaultPosition()
        {
            MotorMode previouseMode = mode;
            changeMode(MotorMode.position);
            moveToPosition((int)Properties.Settings.Default[id.ToString() + "_default"]);
            changeMode(previouseMode);
        }

        /// <summary>
        /// Nastaví současnou polohu motoru jako defaultní
        /// </summary>
        public void setCurrentPositionAsDefault()
        {
            Properties.Settings.Default[id.ToString() + "_default"] = position;
            Properties.Settings.Default.Save();
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
            disposeSimulateTicker();
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
            int velocity = speed;
            angle = getAngleFromPosition(position);
            if (Math.Abs(velocity) > 50)
            {
                state = MotorState.running;
            }
            else
            {
                state = MotorState.enabled;
            }
            int speedRelative = MathLibrary.changeScale(velocity, 0, maxSpeed, 0, 100);
            if (lastPosition > position)
            {
                speedRelative = -speedRelative;
            }
            stateObserver.motorStateChanged(state, "", id, velocity, position, speedRelative, angle);
            lastPosition = position;
        }

        /// <summary>
        /// Vypočítá úhel motoru při dané pozici
        /// </summary>
        /// <param name="position">pozice motoru</param>
        /// <returns>úhel motoru</returns>
        private int getAngleFromPosition(int position)
        {
            for (int i = 0; angleMap!=null && i < angleMap.GetLength(0); i++)
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
            for (int i = 0; angleMap != null && i < angleMap.GetLength(0); i++)
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

        /// <summary>
        /// Nastaví čítač pro simulaci pohybu
        /// </summary>
        private void createSimulateTicker()
        {
            disposeSimulateTicker();
            targetReached = false;
            simulateTicker = new Timer();
            simulateTicker.Interval = 80;
            simulateTicker.Enabled = true;
        }

        /// <summary>
        /// Zruší čítač pro simulaci pohybu
        /// </summary>
        private void disposeSimulateTicker()
        {
            if (simulateTicker != null)
            {
                simulateTicker.Dispose();
            }
            speed = 0;
            targetReached = true;
        }

        /// <summary>
        /// Pohne s motorem v daném směru
        /// </summary>
        /// <param name="direction">směr pohybu 1 nebo -1</param>
        private void simulateMove(int direction)
        {
            targetReached = false;
            speed = 2000 * 10 * multiplier;
            position += 3000 * direction;
            if (speed > maxSpeed)
            {
                speed = maxSpeed;
            }
        }

        /// <summary>
        /// Pohne s robotem v daném směru dokud nedojde do dané pozice
        /// </summary>
        /// <param name="toPosition">do jaké pozice se má motor dostat</param>
        private void simulateMoveToPosition(int toPosition)
        {
            if (position < toPosition)
            {
                if (position + 3000 >= toPosition)
                {
                    position = toPosition;
                    disposeSimulateTicker();
                }
                else
                {
                    simulateMove(1);
                }
            }
            else if (position > toPosition)
            {
                if (position - 3000 <= toPosition)
                {
                    position = toPosition;
                    disposeSimulateTicker();
                }
                else
                {
                    simulateMove(-1);
                }
            }
            else
            {
                disposeSimulateTicker();
            }
        }
    }
}
