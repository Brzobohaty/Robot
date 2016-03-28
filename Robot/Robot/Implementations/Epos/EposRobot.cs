using System;
using System.Collections.Generic;
using EposCmd.Net;
using System.Threading;
using Robot.Robot.Implementations.Test;
using System.Windows.Forms;

namespace Robot.Robot.Implementations.Epos
{
    /// <summary>
    /// Objekt představující abstrakci samotného robota
    /// </summary>
    class EposRobot : IRobot
    {
        private const double widthOfBase = 32; //šířka základny robota v půdorysu (cm)
        private const double heightOfBase = 29; //výška základny robota v půdorysu (cm)
        private const double leangthOfLeg = 35; //délka od nohy od osy Z motoru po spodek kola (cm)
        private const int ZKTolerance = 30; //úhel ve stupních o který když se pohne otáčení kola, tak se nevypne pohon kol 
        private const int maxManipulativeHeightAngle = 60; //maximální úhel zdvihu nohy, kdy je ještě možné otáčet kola 
        private DeviceManager connector; // handler pro přopojení motorů
        private Dictionary<MotorId, IMotor> motors = new Dictionary<MotorId, IMotor>(); //mapa motorů
        private EposErrorCode errorDictionary; //slovník pro překlad z error kódů do zpráv
        private Action motorErrorOccuredObserver; //Posluchač chyb motorů
        private System.Timers.Timer periodicChecker; //periodický vyvolávač určitých funkcí
        private System.Timers.Timer periodicCheckerBackNarrow; //periodický vyvolávač kroků pro zužování zadku
        private System.Timers.Timer periodicCheckerFrontNarrow; //periodický vyvolávač kroků pro zužování předku
        private bool test = false; //příznak, že se jedná o simulaci robota
        private bool radiusMoving = false; //příznak, že probíhá pohyb robota v rádiusu
        private int lastNarrowFrontMeasure = 0; //poslední hodnota na kterou se zůžil předek
        private int lastNarrowBackMeasure = 0; //poslední hodnota na kterou se zůžil zadek
        private bool narrowFrontMoving = false; //příznak, že probíhá pohyb při zůženém předku
        private bool narrowBackMoving = false; //příznak, že probíhá pohyb při zůženém zadku
        private bool directMoving = false; //příznak, že probíhá přímý pohyb

        //pozice zdvihových motorů před nahnutím
        private int lastPositionBeforeTiltLP = 0;
        private int lastPositionBeforeTiltPP = 0;
        private int lastPositionBeforeTiltLZ = 0;
        private int lastPositionBeforeTiltPZ = 0;

        /// <summary>
        /// Pole definující vztah úhlu ze škály uvedené u zdvihového motoru a opravdového úhlu a pozicí motoru
        /// Jeden řádek obsahuje {úhel ze škály ve stupních; opravdový úhel, který svírá přímka vedoucí od osy Z motoru k spodní hraně kola s kolmicí k zemi z osy motoru Z; pozice motoru}
        /// </summary>
        private int[,] motorZAngleMap = new int[17, 3] {
            {40, 15, 147465},
            {35, 17, 123465},
            {30, 23, 107465},
            {25, 28, 84215},
            {20, 31, 71919},
            {15, 35, 52633},
            {10, 43, 30435},
            {5, 47, 15425},
            {0, 56, 0},
            {-5, 60, -24000},
            {-10, 62, -47152},
            {-15, 69, -63152},
            {-20, 75, -83133},
            {-25, 79, -107406},
            {-30, 84, -125934},
            {-35, 89, -145483},
            {-40, 92, -160000},
        };

        public EposRobot()
        {
            errorDictionary = EposErrorCode.getInstance();
            var motorsIds = MotorId.GetValues(typeof(MotorId));
            foreach (MotorId motorId in motorsIds)
            {
                motors.Add(motorId, new EposMotor());
            }
        }

        /// <summary>
        /// Inicializace připojení k motorům
        /// </summary>
        /// <param name="motorStateObserver">posluchač stavu motoru</param>
        /// <param name="withChooseOfBus">příznak, zda při inicilizaci nechat uživatele nastavit parametry připojení</param>
        /// <param name="motorErrorOccuredObserver">posluchač jakéhokoli eroru motoru</param>
        /// <returns>chybovou hlášku nebo prázdný řetězec pokud nenastala chyba</returns>
        public string inicialize(IStateObserver motorStateObserver, bool withChooseOfBus, Action motorErrorOccuredObserver)
        {
            try
            {
                if (withChooseOfBus)
                {
                    connector = new DeviceManager();
                }
                else
                {
                    connector = new DeviceManager("EPOS2", "CANopen", "IXXAT_USB-to-CAN compact 0", "CAN0");
                }
                inicializeMotors(motorStateObserver, motorErrorOccuredObserver);
            }
            catch (DeviceException e)
            {
                inicializeSimulation(motorStateObserver, motorErrorOccuredObserver); //odkomentovat pro softwarovou simulaci (a zakomentovat následující řádek)
                //return String.Format("{0}\nError: {1}", e.ErrorMessage, errorDictionary.getErrorMessage(e.ErrorCode));
            }

            foreach (KeyValuePair<MotorId, IMotor> motor in motors)
            {
                motor.Value.enableStateObserver();
            }

            this.motorErrorOccuredObserver = motorErrorOccuredObserver;
            foreach (KeyValuePair<MotorId, IMotor> motor in motors)
            {
                if (motor.Value.state == MotorState.error)
                {
                    Thread thread = new Thread(motorErrorOccuredObserverFunction);
                    thread.Start();
                    break;
                }
            }

            return "";
        }

        /// <summary>
        /// Pohne s robotem v daném směru a danou rychlostí
        /// </summary>
        /// <param name="direction">směr pohybu v úhlech 0 až 359</param>
        /// <param name="speed">rychlost pohybu od -100 do 100</param>
        public void move(int direction, int speed)
        {
            if (speed == 0)
            {
                haltAll();
                return;
            }
            if (isHeightOk())
            {
                directMove(direction, speed);
            }
            else
            {
                setManipulativHeight();
                createPeriodicChecker();
                periodicChecker.Elapsed += delegate { directMovePeriodic(direction, speed); };
            }
        }

        /// <summary>
        /// Pohne s robotem v daném rádiusu
        /// </summary>
        /// <param name="radiusCircleDistance">vzdálenost rádiusové kružnice (0 - 2000) pro >2000 bere jako přímý pohyb</param>
        /// <param name="speed">rychlost pohybu od -100 do 100</param>
        public void moveInRadius(double radiusCircleDistance, double speed)
        {
            if (speed == 0)
            {
                radiusMoving = false;
                haltAll();
                return;
            }
            if (radiusMoving)
            {
                moveInRadiusFluently(radiusCircleDistance, speed);
            }
            else
            {
                if (isHeightOk())
                {
                    moveInRadiusStep0(radiusCircleDistance, speed);
                }
                else
                {
                    setManipulativHeight();
                    createPeriodicChecker();
                    periodicChecker.Elapsed += delegate { moveInRadiusStep0(radiusCircleDistance, speed); };
                }
            }
        }

        /// <summary>
        /// Sníží robota
        /// </summary>
        public void moveDown()
        {
            haltAll();
            moveDownUp(-1);
        }

        /// <summary>
        /// Zvýší robota
        /// </summary>
        public void moveUp()
        {
            haltAll();
            moveDownUp(1);
        }

        /// <summary>
        /// Rozšíří robota
        /// </summary>
        public void widen()
        {
            haltAll();
            if (isHeightOk())
            {
                narrowWiden(-1);
            }
            else
            {
                setManipulativHeight();
                createPeriodicChecker();
                periodicChecker.Elapsed += delegate { narrowWiden(-1); };
            }
        }

        /// <summary>
        /// Zůží robota
        /// </summary>
        public void narrow()
        {
            haltAll();
            if (isHeightOk())
            {
                narrowWiden(1);
            }
            else
            {
                setManipulativHeight();
                createPeriodicChecker();
                periodicChecker.Elapsed += delegate { narrowWiden(1); };
            }
        }

        /// <summary>
        /// Nastaví robota do defaultní pozice
        /// </summary>
        public void setDefaultPosition()
        {
            haltAll();
            if (isHeightOk())
            {
                setDefaultPositionStep1();
            }
            else
            {
                setManipulativHeight();
                createPeriodicChecker();
                periodicChecker.Elapsed += delegate { setDefaultPositionStep1(); };
            }
        }

        /// <summary>
        /// Nahne robota dozadu
        /// </summary>
        public void tiltBack()
        {
            haltAll();
            tiltStep0(tiltBackStep1);
        }

        /// <summary>
        /// Nahne robota dopředu
        /// </summary>
        public void tiltFront()
        {
            haltAll();
            tiltStep0(tiltFrontStep1);
        }

        /// <summary>
        /// Nahne robota doleva
        /// </summary>
        public void tiltLeft()
        {
            haltAll();
            tiltStep0(tiltLeftStep1);
        }

        /// <summary>
        /// Nahne robota doprava
        /// </summary>
        public void tiltRight()
        {
            haltAll();
            tiltStep0(tiltRightStep1);
        }

        /// <summary>
        /// Zůžit předek a jet dopředu
        /// </summary>
        /// <param name="measure">míra zůžení od 0 do 100</param>
        public void narrowFront(int measure)
        {
            if (!(narrowFrontMoving || narrowBackMoving))
            {
                haltAll();
            }
            narrowFrontStep0(measure);
        }

        /// <summary>
        /// Zůžit zadek a jet dopředu
        /// </summary>
        /// <param name="measure">míra zůžení od 0 do 100</param>
        public void narrowBack(int measure)
        {
            if (!(narrowFrontMoving || narrowBackMoving))
            {
                haltAll();
            }
            narrowBackStep0(measure);
        }

        /// <summary>
        /// Pohne s daným motorem v daném směru o daný krok
        /// </summary>
        /// <param name="motorId">id motoru</param>
        /// <param name="step">krok motoru v qc</param>
        public void moveWithMotor(MotorId motorId, int step)
        {
            motors[motorId].move(step);
        }

        /// <summary>
        /// Vypne celého robota
        /// </summary>
        /// <param name="savePosition">příznak, zda uložit pozici</param>
        public void disable(bool savePosition)
        {
            if ((connector != null || test) && savePosition)
            {
                saveCurrentPositions();
            }

            foreach (KeyValuePair<MotorId, IMotor> motor in motors)
            {
                motor.Value.disableStateObserver();
            }

            foreach (KeyValuePair<MotorId, IMotor> motor in motors)
            {
                motor.Value.disable();
            }
        }

        /// <summary>
        /// Změní ovládání robota (absolutní nebo joystikem)
        /// </summary>
        /// <param name="absoluteControllMode">true, pokud zobrazit absolutní pozicování</param>
        public void changeControllMode(bool absoluteControllMode)
        {
            if (!absoluteControllMode)
            {
                motors[MotorId.PP_P].changeMode(MotorMode.velocity);
                motors[MotorId.LP_P].changeMode(MotorMode.velocity);
                motors[MotorId.LZ_P].changeMode(MotorMode.velocity);
                motors[MotorId.PZ_P].changeMode(MotorMode.velocity);
            }
            else
            {
                motors[MotorId.PP_P].changeMode(MotorMode.position);
                motors[MotorId.LP_P].changeMode(MotorMode.position);
                motors[MotorId.LZ_P].changeMode(MotorMode.position);
                motors[MotorId.PZ_P].changeMode(MotorMode.position);
            }
        }

        /// <summary>
        /// Nastaví současný stav všech motorů jako nulovou pozici
        /// </summary>
        public void homing()
        {
            foreach (KeyValuePair<MotorId, IMotor> motor in motors)
            {
                motor.Value.setActualPositionAsHoming();
            }
        }

        /// <summary>
        /// Zkontroluje, zda jsou nastaveny poslední pozice motorů před vypnutím a pokud ano, tak je nahraje do motorů jako současnou pozici
        /// </summary>
        /// <returns>true, pokud se nahrání povedlo</returns>
        public bool reHoming()
        {
            if (allMotorsOK())
            {
                if (Properties.Settings.Default.correctlyEnded)
                {
                    Properties.Settings.Default.correctlyEnded = false;
                    Properties.Settings.Default.Save();
                    foreach (KeyValuePair<MotorId, IMotor> motor in motors)
                    {
                        motor.Value.setHomingPosition((int)Properties.Settings.Default[motor.Key.ToString()]);
                    }
                    motors[MotorId.LZ_P].setActualPositionAsHoming();
                    motors[MotorId.PZ_P].setActualPositionAsHoming();
                    motors[MotorId.PP_P].setActualPositionAsHoming();
                    motors[MotorId.LP_P].setActualPositionAsHoming();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Nastaví aktuální stav robota jako defaultní
        /// </summary>
        public void setCurrentPositionAsDefault()
        {
            if (!isHeightOk())
            {
                DialogResult dialogResult = MessageBox.Show("Výška jedné nebo více nohou je nastavena příliš vysoko (nad 40 °) a takovou polohu nelze z manipulačních důvodů nastavit jako výchozí.", "Zakázaná výška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                foreach (KeyValuePair<MotorId, IMotor> motor in motors)
                {
                    motor.Value.setCurrentPositionAsDefault();
                }
            }
        }

        /// <summary>
        /// Vypne/zapne ochranu dojezdů motorů
        /// </summary>
        /// <param name="on">true pokud zapnout</param>
        public void limitProtectionEnable(bool on)
        {
            foreach (KeyValuePair<MotorId, IMotor> motor in motors)
            {
                motor.Value.limitProtectionOnOff(on);
            }
        }

        /// <summary>
        /// Zastaví všechny motory.
        /// </summary>
        public void haltAll()
        {
            radiusMoving = false;
            narrowFrontMoving = false;
            narrowBackMoving = false;
            directMoving = false;
            if (periodicChecker != null)
            {
                periodicChecker.Dispose();
            }
            if (periodicCheckerFrontNarrow != null)
            {
                periodicCheckerFrontNarrow.Dispose();
            }
            if (periodicCheckerBackNarrow != null)
            {
                periodicCheckerBackNarrow.Dispose();
            }
            foreach (KeyValuePair<MotorId, IMotor> motor in motors)
            {
                motor.Value.halt();
            }
        }

        /// <summary>
        /// Rotuje robota kolem jeho středu
        /// </summary>
        /// <param name="left">příznak, zda rotovat doleva</param>
        public void rotate(bool left)
        {
            haltAll();
            if (isHeightOk())
            {
                rotateStep1(left);
            }
            else
            {
                setManipulativHeight();
                createPeriodicChecker();
                periodicChecker.Elapsed += delegate { rotateStep1(left); };
            }
        }

        /// <summary>
        /// Zastaví naklánění dopředu
        /// </summary>
        public void stopTiltFront()
        {
            if (periodicChecker != null)
            {
                periodicChecker.Dispose();
            }
            motors[MotorId.LP_Z].moveToPosition(lastPositionBeforeTiltLP);
            motors[MotorId.PP_Z].moveToPosition(lastPositionBeforeTiltPP);
        }

        /// <summary>
        /// Zastaví naklánění dozadu
        /// </summary>
        public void stopTiltBack()
        {
            if (periodicChecker != null)
            {
                periodicChecker.Dispose();
            }
            motors[MotorId.LZ_Z].moveToPosition(lastPositionBeforeTiltLZ);
            motors[MotorId.PZ_Z].moveToPosition(lastPositionBeforeTiltPZ);
        }

        /// <summary>
        /// Zastaví naklánění doleva
        /// </summary>
        public void stopTiltLeft()
        {
            if (periodicChecker != null)
            {
                periodicChecker.Dispose();
            }
            motors[MotorId.LZ_Z].moveToPosition(lastPositionBeforeTiltLZ);
            motors[MotorId.LP_Z].moveToPosition(lastPositionBeforeTiltLP);
        }

        /// <summary>
        /// Zastaví naklánění doprava
        /// </summary>
        public void stopTiltRight()
        {
            if (periodicChecker != null)
            {
                periodicChecker.Dispose();
            }
            motors[MotorId.PP_Z].moveToPosition(lastPositionBeforeTiltPP);
            motors[MotorId.PZ_Z].moveToPosition(lastPositionBeforeTiltPZ);
        }

        /// <summary>
        /// Zastaví zvyšování robota
        /// </summary>
        public void stopMoveUp()
        {
            if (periodicChecker != null)
            {
                periodicChecker.Dispose();
            }
            motors[MotorId.LP_Z].halt();
            motors[MotorId.PP_Z].halt();
            motors[MotorId.LZ_Z].halt();
            motors[MotorId.PZ_Z].halt();

            motors[MotorId.PP_P].halt();
            motors[MotorId.LP_P].halt();
            motors[MotorId.LZ_P].halt();
            motors[MotorId.PZ_P].halt();

            motors[MotorId.PP_P].enable();
            motors[MotorId.LP_P].enable();
            motors[MotorId.LZ_P].enable();
            motors[MotorId.PZ_P].enable();
        }

        /// <summary>
        /// Zastaví snižování robota
        /// </summary>
        public void stopMoveDown()
        {
            if (periodicChecker != null)
            {
                periodicChecker.Dispose();
            }
            motors[MotorId.LP_Z].halt();
            motors[MotorId.PP_Z].halt();
            motors[MotorId.LZ_Z].halt();
            motors[MotorId.PZ_Z].halt();

            motors[MotorId.PP_P].enable();
            motors[MotorId.LP_P].enable();
            motors[MotorId.LZ_P].enable();
            motors[MotorId.PZ_P].enable();
        }

        /// <summary>
        /// Zastaví rozšiřování robota
        /// </summary>
        public void stopWiden()
        {
            if (periodicChecker != null)
            {
                periodicChecker.Dispose();
            }
            motors[MotorId.LP_ZK].halt();
            motors[MotorId.PP_ZK].halt();
            motors[MotorId.LZ_ZK].halt();
            motors[MotorId.PZ_ZK].halt();
        }

        /// <summary>
        /// Zastaví zužování robota
        /// </summary>
        public void stopNarrow()
        {
            if (periodicChecker != null)
            {
                periodicChecker.Dispose();
            }
            motors[MotorId.LP_ZK].halt();
            motors[MotorId.PP_ZK].halt();
            motors[MotorId.LZ_ZK].halt();
            motors[MotorId.PZ_ZK].halt();
        }

        /// <summary>
        /// Zastaví rotování robota
        /// </summary>
        public void stopRotate()
        {
            haltAll();
        }

        /// <summary>
        /// Nastaví motorům prametry podle aktuálně nastavených hodnot v settings
        /// </summary>
        public void refreshMototrsParameters()
        {
            motors[MotorId.PP_P].setParameters((uint)Properties.Settings.Default["P_positionVelocity"], (uint)Properties.Settings.Default["P_positionAceleration"], (uint)Properties.Settings.Default["P_positionDeceleration"], (uint)Properties.Settings.Default["P_maxVelocity"], (uint)Properties.Settings.Default["P_aceleration"], (uint)Properties.Settings.Default["P_deceleration"]);
            motors[MotorId.LP_P].setParameters((uint)Properties.Settings.Default["P_positionVelocity"], (uint)Properties.Settings.Default["P_positionAceleration"], (uint)Properties.Settings.Default["P_positionDeceleration"], (uint)Properties.Settings.Default["P_maxVelocity"], (uint)Properties.Settings.Default["P_aceleration"], (uint)Properties.Settings.Default["P_deceleration"]);
            motors[MotorId.LZ_P].setParameters((uint)Properties.Settings.Default["P_positionVelocity"], (uint)Properties.Settings.Default["P_positionAceleration"], (uint)Properties.Settings.Default["P_positionDeceleration"], (uint)Properties.Settings.Default["P_maxVelocity"], (uint)Properties.Settings.Default["P_aceleration"], (uint)Properties.Settings.Default["P_deceleration"]);
            motors[MotorId.PZ_P].setParameters((uint)Properties.Settings.Default["P_positionVelocity"], (uint)Properties.Settings.Default["P_positionAceleration"], (uint)Properties.Settings.Default["P_positionDeceleration"], (uint)Properties.Settings.Default["P_maxVelocity"], (uint)Properties.Settings.Default["P_aceleration"], (uint)Properties.Settings.Default["P_deceleration"]);
            motors[MotorId.PP_R].setParameters((uint)Properties.Settings.Default["R_positionVelocity"], (uint)Properties.Settings.Default["R_positionAceleration"], (uint)Properties.Settings.Default["R_positionDeceleration"], 1, 1, 1);
            motors[MotorId.LP_R].setParameters((uint)Properties.Settings.Default["R_positionVelocity"], (uint)Properties.Settings.Default["R_positionAceleration"], (uint)Properties.Settings.Default["R_positionDeceleration"], 1, 1, 1);
            motors[MotorId.LZ_R].setParameters((uint)Properties.Settings.Default["R_positionVelocity"], (uint)Properties.Settings.Default["R_positionAceleration"], (uint)Properties.Settings.Default["R_positionDeceleration"], 1, 1, 1);
            motors[MotorId.PZ_R].setParameters((uint)Properties.Settings.Default["R_positionVelocity"], (uint)Properties.Settings.Default["R_positionAceleration"], (uint)Properties.Settings.Default["R_positionDeceleration"], 1, 1, 1);
            motors[MotorId.PP_Z].setParameters((uint)Properties.Settings.Default["Z_positionVelocity"], (uint)Properties.Settings.Default["Z_positionAceleration"], (uint)Properties.Settings.Default["Z_positionDeceleration"], 1, 1, 1);
            motors[MotorId.LP_Z].setParameters((uint)Properties.Settings.Default["Z_positionVelocity"], (uint)Properties.Settings.Default["Z_positionAceleration"], (uint)Properties.Settings.Default["Z_positionDeceleration"], 1, 1, 1);
            motors[MotorId.LZ_Z].setParameters((uint)Properties.Settings.Default["Z_positionVelocity"], (uint)Properties.Settings.Default["Z_positionAceleration"], (uint)Properties.Settings.Default["Z_positionDeceleration"], 1, 1, 1);
            motors[MotorId.PZ_Z].setParameters((uint)Properties.Settings.Default["Z_positionVelocity"], (uint)Properties.Settings.Default["Z_positionAceleration"], (uint)Properties.Settings.Default["Z_positionDeceleration"], 1, 1, 1);
            motors[MotorId.PP_ZK].setParameters((uint)Properties.Settings.Default["ZK_positionVelocity"], (uint)Properties.Settings.Default["ZK_positionAceleration"], (uint)Properties.Settings.Default["ZK_positionDeceleration"], 1, 1, 1);
            motors[MotorId.LP_ZK].setParameters((uint)Properties.Settings.Default["ZK_positionVelocity"], (uint)Properties.Settings.Default["ZK_positionAceleration"], (uint)Properties.Settings.Default["ZK_positionDeceleration"], 1, 1, 1);
            motors[MotorId.LZ_ZK].setParameters((uint)Properties.Settings.Default["ZK_positionVelocity"], (uint)Properties.Settings.Default["ZK_positionAceleration"], (uint)Properties.Settings.Default["ZK_positionDeceleration"], 1, 1, 1);
            motors[MotorId.PZ_ZK].setParameters((uint)Properties.Settings.Default["ZK_positionVelocity"], (uint)Properties.Settings.Default["ZK_positionAceleration"], (uint)Properties.Settings.Default["ZK_positionDeceleration"], 1, 1, 1);
        }

        /// <summary>
        /// Vypočítá délku nohy při pohledu ze zhora
        /// </summary>
        /// <param name="motorZ">motor zdvihu nohy</param>
        /// <returns>délku nohy při pohledu ze zhora</returns>
        private double getLegLeangthFromUpView(IMotor motorZ)
        {
            int angle = motorZ.angle;
            if (angle > 90)
            {
                angle %= 90;
            }
            return (leangthOfLeg * Math.Sin(angle)) + 10.5;
        }

        /// <summary>
        /// Pohne s robotem v daném rádiusu a danou rychlostí - krok 0 - nastavení směru kol
        /// </summary>
        /// <param name="radiusCircleDistance">vzdálenost rádiusové kružnice (0 - 2000) pro >2000 bere jako přímý pohyb</param>
        /// <param name="speed">rychlost pohybu od -100 do 100</param>
        private void moveInRadiusStep0(double radiusCircleDistance, double speed)
        {
            if (motors[MotorId.PP_Z].isTargetReached() && motors[MotorId.LP_Z].isTargetReached() && motors[MotorId.LZ_Z].isTargetReached() && motors[MotorId.PZ_Z].isTargetReached())
            {
                if (periodicChecker != null)
                {
                    periodicChecker.Dispose();
                }

                motors[MotorId.LP_P].disable();
                motors[MotorId.PP_P].disable();
                motors[MotorId.LZ_P].disable();
                motors[MotorId.PZ_P].disable();

                motors[MotorId.LP_R].moveToAngle((int)getWheelAngleForRadiusMove(radiusCircleDistance, MathLibrary.changeScale(motors[MotorId.LP_ZK].angle, motors[MotorId.LP_ZK].minAngle, motors[MotorId.LP_ZK].maxAngle, 0, 90), getLegLeangthFromUpView(motors[MotorId.LP_Z]), -widthOfBase / 2, heightOfBase / 2));
                motors[MotorId.PP_R].moveToAngle((int)getWheelAngleForRadiusMove(radiusCircleDistance, MathLibrary.changeScale(motors[MotorId.PP_ZK].angle, motors[MotorId.PP_ZK].minAngle, motors[MotorId.PP_ZK].maxAngle, 0, 90), getLegLeangthFromUpView(motors[MotorId.PP_Z]), widthOfBase / 2, heightOfBase / 2));
                motors[MotorId.LZ_R].moveToAngle((int)getWheelAngleForRadiusMove(radiusCircleDistance, MathLibrary.changeScale(motors[MotorId.LZ_ZK].angle, motors[MotorId.LZ_ZK].minAngle, motors[MotorId.LZ_ZK].maxAngle, 0, 90), getLegLeangthFromUpView(motors[MotorId.LZ_Z]), -widthOfBase / 2, -heightOfBase / 2));
                motors[MotorId.PZ_R].moveToAngle((int)getWheelAngleForRadiusMove(radiusCircleDistance, MathLibrary.changeScale(motors[MotorId.PZ_ZK].angle, motors[MotorId.PZ_ZK].minAngle, motors[MotorId.PZ_ZK].maxAngle, 0, 90), getLegLeangthFromUpView(motors[MotorId.PZ_Z]), widthOfBase / 2, -heightOfBase / 2));

                createPeriodicChecker();
                periodicChecker.Elapsed += delegate { moveInRadiusStep1(radiusCircleDistance, speed); };
            }
        }

        /// <summary>
        /// Pohne s robotem v daném rádiusu a danou rychlostí - krok 1 - pohon kol
        /// </summary>
        /// <param name="radiusCircleDistance">vzdálenost rádiusové kružnice (0 - 2000) pro >2000 bere jako přímý pohyb</param>
        /// <param name="speed">rychlost pohybu od -100 do 100</param>
        private void moveInRadiusStep1(double radiusCircleDistance, double speed)
        {
            if (motors[MotorId.PP_R].isTargetReached() && motors[MotorId.LP_R].isTargetReached() && motors[MotorId.LZ_R].isTargetReached() && motors[MotorId.PZ_R].isTargetReached())
            {
                periodicChecker.Dispose();

                motors[MotorId.LP_P].enable();
                motors[MotorId.PP_P].enable();
                motors[MotorId.LZ_P].enable();
                motors[MotorId.PZ_P].enable();

                moveInRadiusLastStep(radiusCircleDistance, speed);
            }
        }

        /// <summary>
        /// Pohne s robotem v daném rádiusu a danou rychlostí (bez čekání na nastavení kol => směr nastaví za jízdy)
        /// </summary>
        /// <param name="radiusCircleDistance">vzdálenost rádiusové kružnice (0 - 2000) pro >2000 bere jako přímý pohyb</param>
        /// <param name="speed">rychlost pohybu od -100 do 100</param>
        private void moveInRadiusFluently(double radiusCircleDistance, double speed)
        {
            motors[MotorId.LP_R].moveToAngle((int)getWheelAngleForRadiusMove(radiusCircleDistance, MathLibrary.changeScale(motors[MotorId.LP_ZK].angle, motors[MotorId.LP_ZK].minAngle, motors[MotorId.LP_ZK].maxAngle, 0, 90), getLegLeangthFromUpView(motors[MotorId.LP_Z]), -widthOfBase / 2, heightOfBase / 2));
            motors[MotorId.PP_R].moveToAngle((int)getWheelAngleForRadiusMove(radiusCircleDistance, MathLibrary.changeScale(motors[MotorId.PP_ZK].angle, motors[MotorId.PP_ZK].minAngle, motors[MotorId.PP_ZK].maxAngle, 0, 90), getLegLeangthFromUpView(motors[MotorId.PP_Z]), widthOfBase / 2, heightOfBase / 2));
            motors[MotorId.LZ_R].moveToAngle((int)getWheelAngleForRadiusMove(radiusCircleDistance, MathLibrary.changeScale(motors[MotorId.LZ_ZK].angle, motors[MotorId.LZ_ZK].minAngle, motors[MotorId.LZ_ZK].maxAngle, 0, 90), getLegLeangthFromUpView(motors[MotorId.LZ_Z]), -widthOfBase / 2, -heightOfBase / 2));
            motors[MotorId.PZ_R].moveToAngle((int)getWheelAngleForRadiusMove(radiusCircleDistance, MathLibrary.changeScale(motors[MotorId.PZ_ZK].angle, motors[MotorId.PZ_ZK].minAngle, motors[MotorId.PZ_ZK].maxAngle, 0, 90), getLegLeangthFromUpView(motors[MotorId.PZ_Z]), widthOfBase / 2, -heightOfBase / 2));

            moveInRadiusLastStep(radiusCircleDistance, speed);
        }

        /// <summary>
        /// Pohne s robotem v daném rádiusu a danou rychlostí - poslední krok - pohon kol
        /// </summary>
        /// <param name="radiusCircleDistance">vzdálenost rádiusové kružnice (0 - 2000) pro >2000 bere jako přímý pohyb</param>
        /// <param name="speed">rychlost pohybu od -100 do 100</param>
        private void moveInRadiusLastStep(double radiusCircleDistance, double speed)
        {
            double arcLeangthLP = getStandardizedArcLeangth(radiusCircleDistance, MathLibrary.changeScale(motors[MotorId.LP_ZK].angle, motors[MotorId.LP_ZK].minAngle, motors[MotorId.LP_ZK].maxAngle, 0, 90), getLegLeangthFromUpView(motors[MotorId.LP_Z]), -widthOfBase / 2, heightOfBase / 2);
            double arcLeangthPP = getStandardizedArcLeangth(radiusCircleDistance, MathLibrary.changeScale(motors[MotorId.PP_ZK].angle, motors[MotorId.PP_ZK].minAngle, motors[MotorId.PP_ZK].maxAngle, 0, 90), getLegLeangthFromUpView(motors[MotorId.PP_Z]), widthOfBase / 2, heightOfBase / 2);
            double arcLeangthLZ = getStandardizedArcLeangth(radiusCircleDistance, MathLibrary.changeScale(motors[MotorId.LZ_ZK].angle, motors[MotorId.LZ_ZK].minAngle, motors[MotorId.LZ_ZK].maxAngle, 0, 90), getLegLeangthFromUpView(motors[MotorId.LZ_Z]), -widthOfBase / 2, -heightOfBase / 2);
            double arcLeangthPZ = getStandardizedArcLeangth(radiusCircleDistance, MathLibrary.changeScale(motors[MotorId.PZ_ZK].angle, motors[MotorId.PZ_ZK].minAngle, motors[MotorId.PZ_ZK].maxAngle, 0, 90), getLegLeangthFromUpView(motors[MotorId.PZ_Z]), widthOfBase / 2, -heightOfBase / 2);

            double maxArcLeangth = Math.Max(Math.Max(arcLeangthLP, arcLeangthPP), Math.Max(arcLeangthLZ, arcLeangthPZ));

            motors[MotorId.LP_P].moving((int)(speed * (arcLeangthLP / maxArcLeangth)));
            motors[MotorId.PP_P].moving((int)(speed * (arcLeangthPP / maxArcLeangth)));
            motors[MotorId.LZ_P].moving((int)(speed * (arcLeangthLZ / maxArcLeangth)));
            motors[MotorId.PZ_P].moving((int)(speed * (arcLeangthPZ / maxArcLeangth)));

            radiusMoving = true;
        }

        /// <summary>
        /// Zůží předek robota a pojede pomalu dopředu - krok 0 - nastavení manipulativní výšky
        /// </summary>
        /// <param name="measure">hodnota o kolik zůžit 0 až 100</param>
        private void narrowFrontStep0(int measure)
        {
            if ((narrowFrontMoving || narrowBackMoving) && directMoving && Math.Abs(measure - lastNarrowFrontMeasure) < 15)
            {
                narrowFrontStep2(measure);
                return;
            }
            else {
                if (periodicChecker != null)
                {
                    periodicChecker.Dispose();
                    motors[MotorId.LP_P].halt();
                    motors[MotorId.PP_P].halt();
                    motors[MotorId.LZ_P].halt();
                    motors[MotorId.PZ_P].halt();
                }
            }
            if (isHeightOk())
            {
                narrowFrontStep1(measure);
            }
            else
            {
                setManipulativHeight();
                createPeriodicCheckerFrontNarrow();
                periodicCheckerFrontNarrow.Elapsed += delegate
                {
                    narrowFrontStep1(measure);
                };
            }
        }

        /// <summary>
        /// Zůží zadek robota a pojede pomalu dopředu - krok 0 - nastavení manipulativní výšky
        /// </summary>
        /// <param name="measure">hodnota o kolik zůžit 0 až 100</param>
        private void narrowBackStep0(int measure)
        {
            if ((narrowFrontMoving || narrowBackMoving) && directMoving && Math.Abs(measure - lastNarrowBackMeasure) < 15)
            {
                narrowBackStep2(measure);
                return;
            }
            else {
                if (periodicChecker != null) {
                    periodicChecker.Dispose();
                    motors[MotorId.LP_P].halt();
                    motors[MotorId.PP_P].halt();
                    motors[MotorId.LZ_P].halt();
                    motors[MotorId.PZ_P].halt();
                }
            }
            if (isHeightOk())
            {
                narrowBackStep1(measure);
            }
            else
            {
                setManipulativHeight();
                createPeriodicCheckerBackNarrow();
                periodicCheckerBackNarrow.Elapsed += delegate
                {
                    narrowBackStep1(measure);
                };
            }
        }

        /// <summary>
        /// Zůží předek/zadek robota a pojede pomalu dopředu - krok 1 - otočení kol pro otočení nohou
        /// </summary>
        /// <param name="measure">hodnota o kolik zůžit 0 až 100</param>
        private void narrowFrontStep1(int measure)
        {
            if (motors[MotorId.PP_Z].isTargetReached() && motors[MotorId.LP_Z].isTargetReached() && motors[MotorId.LZ_Z].isTargetReached() && motors[MotorId.PZ_Z].isTargetReached())
            {
                if (periodicCheckerFrontNarrow != null)
                {
                    periodicCheckerFrontNarrow.Dispose();
                }

                motors[MotorId.LP_P].disable();
                motors[MotorId.PP_P].disable();

                motors[MotorId.LP_R].moveToPosition(0);
                motors[MotorId.PP_R].moveToPosition(0);

                createPeriodicCheckerFrontNarrow();
                periodicCheckerFrontNarrow.Elapsed += delegate { narrowFrontStep2(measure); };
            }
        }

        /// <summary>
        /// Zůží předek/zadek robota a pojede pomalu dopředu - krok 1 - otočení kol pro otočení nohou
        /// </summary>
        /// <param name="measure">hodnota o kolik zůžit 0 až 100</param>
        private void narrowBackStep1(int measure)
        {
            if (motors[MotorId.PP_Z].isTargetReached() && motors[MotorId.LP_Z].isTargetReached() && motors[MotorId.LZ_Z].isTargetReached() && motors[MotorId.PZ_Z].isTargetReached())
            {
                if (periodicCheckerBackNarrow != null)
                {
                    periodicCheckerBackNarrow.Dispose();
                }
                
                motors[MotorId.LZ_P].disable();
                motors[MotorId.PZ_P].disable();
                
                motors[MotorId.LZ_R].moveToPosition(0);
                motors[MotorId.PZ_R].moveToPosition(0);

                createPeriodicCheckerBackNarrow();
                periodicCheckerBackNarrow.Elapsed += delegate { narrowBackStep2(measure); };
            }
        }

        /// <summary>
        /// Zůží předek/zadek robota a pojede pomalu dopředu - krok 2 - otočení nohou
        /// </summary>
        /// <param name="measure">hodnota o kolik zůžit 0 až 100</param>
        private void narrowFrontStep2(int measure)
        {
            if (motors[MotorId.PP_R].isTargetReached() && motors[MotorId.LP_R].isTargetReached() && motors[MotorId.LZ_R].isTargetReached() && motors[MotorId.PZ_R].isTargetReached())
            {
                if (periodicCheckerFrontNarrow != null) {
                    periodicCheckerFrontNarrow.Dispose();
                }

                int angle = MathLibrary.changeScale(measure, 0, 100, 0, 45);


                motors[MotorId.LP_ZK].moveToAngle(angle);
                motors[MotorId.PP_ZK].moveToAngle(angle);

                createPeriodicCheckerFrontNarrow();
                periodicCheckerFrontNarrow.Elapsed += delegate { narrowFrontStep3(measure); };
            }
        }

        /// <summary>
        /// Zůží předek/zadek robota a pojede pomalu dopředu - krok 2 - otočení nohou
        /// </summary>
        /// <param name="measure">hodnota o kolik zůžit 0 až 100</param>
        private void narrowBackStep2(int measure)
        {
            if (motors[MotorId.PP_R].isTargetReached() && motors[MotorId.LP_R].isTargetReached() && motors[MotorId.LZ_R].isTargetReached() && motors[MotorId.PZ_R].isTargetReached())
            {
                if (periodicCheckerBackNarrow != null)
                {
                    periodicCheckerBackNarrow.Dispose();
                }

                int angle = MathLibrary.changeScale(measure, 0, 100, 0, 45);

                motors[MotorId.LZ_ZK].moveToAngle(angle);
                motors[MotorId.PZ_ZK].moveToAngle(angle);

                createPeriodicCheckerBackNarrow();
                periodicCheckerBackNarrow.Elapsed += delegate { narrowBackStep3(measure); };
            }
        }

        /// <summary>
        /// Zůží předek/zadek robota a pojede pomalu dopředu - krok 3 - pojezd
        /// </summary>
        /// <param name="measure">hodnota o kolik zůžit 0 až 100</param>
        private void narrowFrontStep3(int measure)
        {
            if (motors[MotorId.PP_ZK].isTargetReached() && motors[MotorId.LP_ZK].isTargetReached() && motors[MotorId.LZ_ZK].isTargetReached() && motors[MotorId.PZ_ZK].isTargetReached())
            {
                periodicCheckerFrontNarrow.Dispose();

                if (measure == 0)
                {
                    narrowFrontMoving = false;
                    if (!narrowBackMoving)
                    {
                        haltAll();
                    }
                    else {
                        lastNarrowFrontMeasure = measure;
                        narrowFrontMoving = true;
                        directMove(90, 30);
                    }
                }
                else
                {
                    lastNarrowFrontMeasure = measure;
                    narrowFrontMoving = true;
                    directMove(90, 30);
                }
            }
        }

        /// <summary>
        /// Zůží předek/zadek robota a pojede pomalu dopředu - krok 3 - pojezd
        /// </summary>
        /// <param name="measure">hodnota o kolik zůžit 0 až 100</param>
        private void narrowBackStep3(int measure)
        {
            if (motors[MotorId.PP_ZK].isTargetReached() && motors[MotorId.LP_ZK].isTargetReached() && motors[MotorId.LZ_ZK].isTargetReached() && motors[MotorId.PZ_ZK].isTargetReached())
            {
                periodicCheckerBackNarrow.Dispose();

                if (measure == 0)
                {
                    narrowBackMoving = false;
                    if (!narrowFrontMoving)
                    {
                        haltAll();
                    }
                    else {
                        lastNarrowBackMeasure = measure;
                        narrowBackMoving = true;
                        directMove(90, 30);
                    }
                }
                else
                {
                    lastNarrowBackMeasure = measure;
                    narrowBackMoving = true;
                    directMove(90, 30);
                }
            }
        }

        /// <summary>
        /// Nahne robota (step 0 - natočení kol)
        /// </summary>
        /// <param name="tiltStep1">metoda pro nahnutí ve správném směru</param>
        private void tiltStep0(Action tiltStep1)
        {
            motors[MotorId.LP_R].moveToAngle(90);
            motors[MotorId.PP_R].moveToAngle(90);
            motors[MotorId.LZ_R].moveToAngle(90);
            motors[MotorId.PZ_R].moveToAngle(90);

            createPeriodicChecker();
            periodicChecker.Elapsed += delegate { tiltStep1(); };
        }

        /// <summary>
        /// Nahne robota dozadu (step 1 - nastavení výšky nohou)
        /// </summary>
        private void tiltBackStep1()
        {
            if (motors[MotorId.PP_R].isTargetReached() && motors[MotorId.LP_R].isTargetReached() && motors[MotorId.LZ_R].isTargetReached() && motors[MotorId.PZ_R].isTargetReached())
            {
                periodicChecker.Dispose();

                lastPositionBeforeTiltLZ = motors[MotorId.LZ_Z].getPosition();
                lastPositionBeforeTiltPZ = motors[MotorId.PZ_Z].getPosition();

                motors[MotorId.LZ_Z].moveToMaxPosition();
                motors[MotorId.PZ_Z].moveToMaxPosition();
            }
        }

        /// <summary>
        /// Nahne robota dopředu (step 1 - nastavení výšky nohou)
        /// </summary>
        private void tiltFrontStep1()
        {
            if (motors[MotorId.PP_R].isTargetReached() && motors[MotorId.LP_R].isTargetReached() && motors[MotorId.LZ_R].isTargetReached() && motors[MotorId.PZ_R].isTargetReached())
            {
                periodicChecker.Dispose();

                lastPositionBeforeTiltPP = motors[MotorId.PP_Z].getPosition();
                lastPositionBeforeTiltLP = motors[MotorId.LP_Z].getPosition();

                motors[MotorId.PP_Z].moveToMaxPosition();
                motors[MotorId.LP_Z].moveToMaxPosition();
            }
        }

        /// <summary>
        /// Nahne robota doleva (step 1 - nastavení výšky nohou)
        /// </summary>
        private void tiltLeftStep1()
        {
            if (motors[MotorId.PP_R].isTargetReached() && motors[MotorId.LP_R].isTargetReached() && motors[MotorId.LZ_R].isTargetReached() && motors[MotorId.PZ_R].isTargetReached())
            {
                periodicChecker.Dispose();

                lastPositionBeforeTiltLP = motors[MotorId.LP_Z].getPosition();
                lastPositionBeforeTiltLZ = motors[MotorId.LZ_Z].getPosition();

                motors[MotorId.LP_Z].moveToMaxPosition();
                motors[MotorId.LZ_Z].moveToMaxPosition();
            }
        }

        /// <summary>
        /// Nahne robota doprava (step 1 - nastavení výšky nohou)
        /// </summary>
        private void tiltRightStep1()
        {
            if (motors[MotorId.PP_R].isTargetReached() && motors[MotorId.LP_R].isTargetReached() && motors[MotorId.LZ_R].isTargetReached() && motors[MotorId.PZ_R].isTargetReached())
            {
                periodicChecker.Dispose();

                lastPositionBeforeTiltPP = motors[MotorId.PP_Z].getPosition();
                lastPositionBeforeTiltPZ = motors[MotorId.PZ_Z].getPosition();

                motors[MotorId.PP_Z].moveToMaxPosition();
                motors[MotorId.PZ_Z].moveToMaxPosition();
            }
        }

        /// <summary>
        /// Rotuje robota kolem jeho středu (krok 1 - natočení kol)
        /// </summary>
        /// <param name="left">příznak, zda rotovat doleva</param>
        private void rotateStep1(bool left)
        {
            if (motors[MotorId.PP_Z].isTargetReached() && motors[MotorId.LP_Z].isTargetReached() && motors[MotorId.LZ_Z].isTargetReached() && motors[MotorId.PZ_Z].isTargetReached())
            {
                if (periodicChecker != null)
                {
                    periodicChecker.Dispose();
                }

                motors[MotorId.LP_R].moveToPosition(0);
                motors[MotorId.PP_R].moveToPosition(0);
                motors[MotorId.LZ_R].moveToPosition(0);
                motors[MotorId.PZ_R].moveToPosition(0);

                createPeriodicChecker();
                periodicChecker.Elapsed += delegate { rotateStep2(left); };
            }
        }

        /// <summary>
        /// Rotuje robota kolem jeho středu (krok 2 - pohon kol)
        /// </summary>
        /// <param name="left">příznak, zda rotovat doleva</param>
        private void rotateStep2(bool left)
        {
            if (motors[MotorId.PP_R].isTargetReached() && motors[MotorId.LP_R].isTargetReached() && motors[MotorId.LZ_R].isTargetReached() && motors[MotorId.PZ_R].isTargetReached())
            {
                periodicChecker.Dispose();

                int rev = 1;
                if (left)
                {
                    rev = -1;
                }

                motors[MotorId.LP_P].enable();
                motors[MotorId.PP_P].enable();
                motors[MotorId.LZ_P].enable();
                motors[MotorId.PZ_P].enable();

                motors[MotorId.LP_P].moving(100 * rev);
                motors[MotorId.PP_P].moving(-100 * rev);
                motors[MotorId.LZ_P].moving(100 * rev);
                motors[MotorId.PZ_P].moving(-100 * rev);
            }
        }

        /// <summary>
        /// Nastaví robota do defaultní pozice (krok 1 - nastavení výšky)
        /// </summary>
        private void setDefaultPositionStep1()
        {
            if (motors[MotorId.PP_Z].isTargetReached() && motors[MotorId.LP_Z].isTargetReached() && motors[MotorId.LZ_Z].isTargetReached() && motors[MotorId.PZ_Z].isTargetReached())
            {
                if (periodicChecker != null)
                {
                    periodicChecker.Dispose();
                }

                motors[MotorId.PP_P].disable();
                motors[MotorId.LP_P].disable();
                motors[MotorId.LZ_P].disable();
                motors[MotorId.PZ_P].disable();

                motors[MotorId.LP_Z].setDefaultPosition();
                motors[MotorId.PP_Z].setDefaultPosition();
                motors[MotorId.LZ_Z].setDefaultPosition();
                motors[MotorId.PZ_Z].setDefaultPosition();

                createPeriodicChecker();
                periodicChecker.Elapsed += delegate { setDefaultPositionStep2(); };
            }
        }

        /// <summary>
        /// Nastaví robota do defaultní pozice (krok 2 - nastavení rotace kol pro pohyb nohou do stran)
        /// </summary>
        private void setDefaultPositionStep2()
        {
            if (motors[MotorId.PP_Z].isTargetReached() && motors[MotorId.LP_Z].isTargetReached() && motors[MotorId.LZ_Z].isTargetReached() && motors[MotorId.PZ_Z].isTargetReached())
            {
                periodicChecker.Dispose();

                motors[MotorId.LP_R].moveToPosition(0);
                motors[MotorId.PP_R].moveToPosition(0);
                motors[MotorId.LZ_R].moveToPosition(0);
                motors[MotorId.PZ_R].moveToPosition(0);

                createPeriodicChecker();
                periodicChecker.Elapsed += delegate { setDefaultPositionStep3(); };
            }
        }

        /// <summary>
        /// Nastaví robota do defaultní pozice (krok 3 - nastavení otočení nohou)
        /// </summary>
        private void setDefaultPositionStep3()
        {
            if (motors[MotorId.PP_R].isTargetReached() && motors[MotorId.LP_R].isTargetReached() && motors[MotorId.LZ_R].isTargetReached() && motors[MotorId.PZ_R].isTargetReached())
            {
                periodicChecker.Dispose();

                motors[MotorId.LP_ZK].setDefaultPosition();
                motors[MotorId.PP_ZK].setDefaultPosition();
                motors[MotorId.LZ_ZK].setDefaultPosition();
                motors[MotorId.PZ_ZK].setDefaultPosition();

                createPeriodicChecker();
                periodicChecker.Elapsed += delegate { setDefaultPositionStep4(); };
            }
        }

        /// <summary>
        /// Nastaví robota do defaultní pozice (krok 4 - nastavení otočení kol)
        /// </summary>
        private void setDefaultPositionStep4()
        {
            if (motors[MotorId.PP_ZK].isTargetReached() && motors[MotorId.LP_ZK].isTargetReached() && motors[MotorId.LZ_ZK].isTargetReached() && motors[MotorId.PZ_ZK].isTargetReached())
            {
                periodicChecker.Dispose();

                motors[MotorId.LP_R].setDefaultPosition();
                motors[MotorId.PP_R].setDefaultPosition();
                motors[MotorId.LZ_R].setDefaultPosition();
                motors[MotorId.PZ_R].setDefaultPosition();

                createPeriodicChecker();
                periodicChecker.Elapsed += delegate { setDefaultPositionStep5(); };
            }
        }

        /// <summary>
        /// Nastaví robota do defaultní pozice (krok 5 - zapnutí pohonu)
        /// </summary>
        private void setDefaultPositionStep5()
        {
            if (motors[MotorId.PP_R].isTargetReached() && motors[MotorId.LP_R].isTargetReached() && motors[MotorId.LZ_R].isTargetReached() && motors[MotorId.PZ_R].isTargetReached())
            {
                periodicChecker.Dispose();

                motors[MotorId.PP_P].enable();
                motors[MotorId.LP_P].enable();
                motors[MotorId.LZ_P].enable();
                motors[MotorId.PZ_P].enable();
            }
        }

        /// <summary>
        /// Nastaví robotoj výšku, v které se s ním  dá libovolně manipulovat.
        /// </summary>
        private void setManipulativHeight()
        {
            motors[MotorId.PP_Z].moveToAngle(maxManipulativeHeightAngle);
            motors[MotorId.LP_Z].moveToAngle(maxManipulativeHeightAngle);
            motors[MotorId.LZ_Z].moveToAngle(maxManipulativeHeightAngle);
            motors[MotorId.PZ_Z].moveToAngle(maxManipulativeHeightAngle);
        }

        /// <summary>
        /// Zjistí, zda je robot ve výšce, v které je možné s ním libovolně manipulovat
        /// </summary>
        /// <returns>true pokud je v manipulační výšce</returns>
        private bool isHeightOk()
        {
            if (motors[MotorId.PP_Z].angle < maxManipulativeHeightAngle && motors[MotorId.LP_Z].angle < maxManipulativeHeightAngle && motors[MotorId.LZ_Z].angle < maxManipulativeHeightAngle && motors[MotorId.PZ_Z].angle < maxManipulativeHeightAngle)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Přímý pohyb kol krok 0 - kontrola výšky robota
        /// </summary>
        private void directMovePeriodic(int direction, int speed)
        {
            if (motors[MotorId.PP_Z].isTargetReached() && motors[MotorId.LP_Z].isTargetReached() && motors[MotorId.LZ_Z].isTargetReached() && motors[MotorId.PZ_Z].isTargetReached())
            {
                periodicChecker.Dispose();
                directMove(direction, speed);
            }
        }

        /// <summary>
        /// Přímý pohyb kol krok 1 - natočení kol
        /// </summary>
        /// <param name="direction">směr pohybu v úhlech 0 až 359</param>
        /// <param name="speed">rychlost pohybu od -100 do 100</param>
        private void directMove(int direction, int speed)
        {
            int speedLP = setWheelToDirection(motors[MotorId.LP_ZK], motors[MotorId.LP_R], motors[MotorId.LP_P], direction, speed, 360 - 90);
            int speedPP = setWheelToDirection(motors[MotorId.PP_ZK], motors[MotorId.PP_R], motors[MotorId.PP_P], 180 - direction, speed, -90);
            int speedLZ = setWheelToDirection(motors[MotorId.LZ_ZK], motors[MotorId.LZ_R], motors[MotorId.LZ_P], 180 - direction, speed, -90);
            int speedPZ = setWheelToDirection(motors[MotorId.PZ_ZK], motors[MotorId.PZ_R], motors[MotorId.PZ_P], direction, speed, 360 - 90);

            if (Math.Abs(motors[MotorId.LP_R].targetAngle - motors[MotorId.LP_R].angle) < ZKTolerance && Math.Abs(motors[MotorId.PP_R].targetAngle - motors[MotorId.PP_R].angle) < ZKTolerance && Math.Abs(motors[MotorId.LZ_R].targetAngle - motors[MotorId.LZ_R].angle) < ZKTolerance && Math.Abs(motors[MotorId.PZ_R].targetAngle - motors[MotorId.PZ_R].angle) < ZKTolerance)
            {
                motors[MotorId.LP_P].enable();
                motors[MotorId.PP_P].enable();
                motors[MotorId.LZ_P].enable();
                motors[MotorId.PZ_P].enable();

                motors[MotorId.LP_P].moving(speedLP);
                motors[MotorId.PP_P].moving(speedPP);
                motors[MotorId.LZ_P].moving(speedLZ);
                motors[MotorId.PZ_P].moving(speedPZ);
            }
            else
            {
                motors[MotorId.LP_P].disable();
                motors[MotorId.PP_P].disable();
                motors[MotorId.LZ_P].disable();
                motors[MotorId.PZ_P].disable();
                createPeriodicChecker();
                periodicChecker.Elapsed += delegate { directMovePeriodic(speedLP, speedPP, speedLZ, speedPZ); };
            }
        }

        /// <summary>
        /// Přímý pohyb kol krok 2 - pohon kol
        /// </summary>
        /// <param name="speedLP">rychlost kola (-100 až 100)</param>
        /// <param name="speedPP">rychlost kola (-100 až 100)</param>
        /// <param name="speedLZ">rychlost kola (-100 až 100)</param>
        /// <param name="speedPZ">rychlost kola (-100 až 100)</param>
        private void directMovePeriodic(int speedLP, int speedPP, int speedLZ, int speedPZ)
        {
            if (motors[MotorId.PP_R].isTargetReached() && motors[MotorId.LP_R].isTargetReached() && motors[MotorId.LZ_R].isTargetReached() && motors[MotorId.PZ_R].isTargetReached())
            {
                periodicChecker.Dispose();

                directMoving = true;

                motors[MotorId.LP_P].enable();
                motors[MotorId.PP_P].enable();
                motors[MotorId.LZ_P].enable();
                motors[MotorId.PZ_P].enable();

                motors[MotorId.LP_P].moving(speedLP);
                motors[MotorId.PP_P].moving(speedPP);
                motors[MotorId.LZ_P].moving(speedLZ);
                motors[MotorId.PZ_P].moving(speedPZ);
            }
        }

        /// <summary>
        /// Bude pohybovat kolem v daném směru a vrátí rychlost ve správném směru
        /// </summary>
        /// <param name="motorZK">motor pro otáčení nohy kola</param>
        /// <param name="motorR">motor pro otáčení kola</param>
        /// <param name="motorP">motor pro pohon kola</param>
        /// <param name="direction">směrm kterým se má pohybovat 0 až 359</param>
        /// <param name="speed">rychlost jakou se má pohybovat -100 až 100</param>
        /// <param name="angleCorection">korekce úhlu nohy...</param>
        /// <return>rychlost, jakou se má motor pohybovat</return>
        private int setWheelToDirection(IMotor motorZK, IMotor motorR, IMotor motorP, int direction, int speed, int angleCorection)
        {
            bool reverseWheelSpeed = rotateWheelForMove(direction, motorZK, motorR, motorP, angleCorection);
            int rev = 1;
            if (reverseWheelSpeed)
            {
                rev = -1;
            }
            return speed * rev;
        }

        /// <summary>
        /// Otočí kolo nohy ve směru pohybu
        /// </summary>
        /// <param name="direction">směr ve stupních 0 až 359</param>
        /// <param name="motorZK">motor pro otáčení nohy</param>
        /// <param name="motorR">motor pro otáčení kola nohy</param>
        /// <param name="angleCorection">korekce úhlu nohy...</param>
        /// <returns>true pokud obrátit směr pohybu kola</returns>
        private bool rotateWheelForMove(int direction, IMotor motorZK, IMotor motorR, IMotor motorP, int angleCorection)
        {
            bool reverseWheelSpeed = false;
            int kartezLegAngle = MathLibrary.changeScale(motorZK.angle, motorZK.minAngle, motorZK.maxAngle, 0, 90);
            if (angleCorection < 0)
            {
                if (direction - (angleCorection - kartezLegAngle) < 0)
                {
                    reverseWheelSpeed = !reverseWheelSpeed;
                }
            }
            else
            {
                if (direction - (angleCorection - kartezLegAngle) > 0)
                {
                    reverseWheelSpeed = !reverseWheelSpeed;
                }
            }

            int kartezWheelAngle = 180 - 90 - (180 - direction - kartezLegAngle);
            kartezWheelAngle = kartezWheelAngle % 180;
            if (kartezWheelAngle < 0)
            {
                kartezWheelAngle += 180;
                reverseWheelSpeed = !reverseWheelSpeed;
            }
            int wheelAngle = kartezWheelAngle;
            if (Math.Abs(wheelAngle - motorR.angle) > ZKTolerance)
            {
                motorP.disable();
            }
            motorR.moveToAngle(wheelAngle);
            return reverseWheelSpeed;
        }

        /// <summary>
        /// Inicializuje softwarovou simulaci robota
        /// </summary>
        /// <param name="motorStateObserver">posluchač stavu motoru</param>
        /// <param name="motorErrorOccuredObserver">posluchač jakéhokoli eroru motoru</param>
        private void inicializeSimulation(IStateObserver motorStateObserver, Action motorErrorOccuredObserver)
        {
            test = true;
            motors.Clear();
            var motorsIds = MotorId.GetValues(typeof(MotorId));
            foreach (MotorId motorId in motorsIds)
            {
                motors.Add(motorId, new TestMotor());
            }
            inicializeMotors(motorStateObserver, motorErrorOccuredObserver);
        }

        /// <summary>
        /// Inicializace motorů
        /// </summary>
        /// <param name="motorStateObserver">posluchač stavu motoru</param>
        /// <param name="motorErrorOccuredObserver">posluchač jakéhokoli eroru motoru</param>
        private void inicializeMotors(IStateObserver motorStateObserver, Action motorErrorOccuredObserver)
        {
            motors[MotorId.PP_P].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 4, MotorId.PP_P, MotorMode.velocity, true, 1, (uint)Properties.Settings.Default["P_positionVelocity"], (uint)Properties.Settings.Default["P_positionAceleration"], (uint)Properties.Settings.Default["P_positionDeceleration"], (uint)Properties.Settings.Default["P_maxVelocity"], (uint)Properties.Settings.Default["P_aceleration"], (uint)Properties.Settings.Default["P_deceleration"]);
            motors[MotorId.LP_P].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 8, MotorId.LP_P, MotorMode.velocity, false, 1, (uint)Properties.Settings.Default["P_positionVelocity"], (uint)Properties.Settings.Default["P_positionAceleration"], (uint)Properties.Settings.Default["P_positionDeceleration"], (uint)Properties.Settings.Default["P_maxVelocity"], (uint)Properties.Settings.Default["P_aceleration"], (uint)Properties.Settings.Default["P_deceleration"]);
            motors[MotorId.LZ_P].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 12, MotorId.LZ_P, MotorMode.velocity, false, 1, (uint)Properties.Settings.Default["P_positionVelocity"], (uint)Properties.Settings.Default["P_positionAceleration"], (uint)Properties.Settings.Default["P_positionDeceleration"], (uint)Properties.Settings.Default["P_maxVelocity"], (uint)Properties.Settings.Default["P_aceleration"], (uint)Properties.Settings.Default["P_deceleration"]);
            motors[MotorId.PZ_P].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 16, MotorId.PZ_P, MotorMode.velocity, true, 1, (uint)Properties.Settings.Default["P_positionVelocity"], (uint)Properties.Settings.Default["P_positionAceleration"], (uint)Properties.Settings.Default["P_positionDeceleration"], (uint)Properties.Settings.Default["P_maxVelocity"], (uint)Properties.Settings.Default["P_aceleration"], (uint)Properties.Settings.Default["P_deceleration"]);
            motors[MotorId.PP_R].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 3, MotorId.PP_R, MotorMode.position, false, 4, (uint)Properties.Settings.Default["R_positionVelocity"], (uint)Properties.Settings.Default["R_positionAceleration"], (uint)Properties.Settings.Default["R_positionDeceleration"], 1, 1, 1, -216502, 433004, -90, 180);
            motors[MotorId.LP_R].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 7, MotorId.LP_R, MotorMode.position, false, 4, (uint)Properties.Settings.Default["R_positionVelocity"], (uint)Properties.Settings.Default["R_positionAceleration"], (uint)Properties.Settings.Default["R_positionDeceleration"], 1, 1, 1, -216502, 433004, -90, 180);
            motors[MotorId.LZ_R].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 11, MotorId.LZ_R, MotorMode.position, false, 4, (uint)Properties.Settings.Default["R_positionVelocity"], (uint)Properties.Settings.Default["R_positionAceleration"], (uint)Properties.Settings.Default["R_positionDeceleration"], 1, 1, 1, -216502, 433004, -90, 180);
            motors[MotorId.PZ_R].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 15, MotorId.PZ_R, MotorMode.position, false, 4, (uint)Properties.Settings.Default["R_positionVelocity"], (uint)Properties.Settings.Default["R_positionAceleration"], (uint)Properties.Settings.Default["R_positionDeceleration"], 1, 1, 1, -216502, 433004, -90, 180);
            motors[MotorId.PP_Z].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 2, MotorId.PP_Z, MotorMode.position, false, 4, (uint)Properties.Settings.Default["Z_positionVelocity"], (uint)Properties.Settings.Default["Z_positionAceleration"], (uint)Properties.Settings.Default["Z_positionDeceleration"], 1, 1, 1, motorZAngleMap);
            motors[MotorId.LP_Z].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 6, MotorId.LP_Z, MotorMode.position, false, 4, (uint)Properties.Settings.Default["Z_positionVelocity"], (uint)Properties.Settings.Default["Z_positionAceleration"], (uint)Properties.Settings.Default["Z_positionDeceleration"], 1, 1, 1, motorZAngleMap);
            motors[MotorId.LZ_Z].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 10, MotorId.LZ_Z, MotorMode.position, false, 4, (uint)Properties.Settings.Default["Z_positionVelocity"], (uint)Properties.Settings.Default["Z_positionAceleration"], (uint)Properties.Settings.Default["Z_positionDeceleration"], 1, 1, 1, motorZAngleMap);
            motors[MotorId.PZ_Z].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 14, MotorId.PZ_Z, MotorMode.position, false, 4, (uint)Properties.Settings.Default["Z_positionVelocity"], (uint)Properties.Settings.Default["Z_positionAceleration"], (uint)Properties.Settings.Default["Z_positionDeceleration"], 1, 1, 1, motorZAngleMap);
            motors[MotorId.PP_ZK].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 1, MotorId.PP_ZK, MotorMode.position, false, 4, (uint)Properties.Settings.Default["ZK_positionVelocity"], (uint)Properties.Settings.Default["ZK_positionAceleration"], (uint)Properties.Settings.Default["ZK_positionDeceleration"], 1, 1, 1, -110000, 108000, -45, 45);
            motors[MotorId.LP_ZK].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 5, MotorId.LP_ZK, MotorMode.position, false, 4, (uint)Properties.Settings.Default["ZK_positionVelocity"], (uint)Properties.Settings.Default["ZK_positionAceleration"], (uint)Properties.Settings.Default["ZK_positionDeceleration"], 1, 1, 1, -110000, 108000, -45, 45);
            motors[MotorId.LZ_ZK].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 9, MotorId.LZ_ZK, MotorMode.position, false, 4, (uint)Properties.Settings.Default["ZK_positionVelocity"], (uint)Properties.Settings.Default["ZK_positionAceleration"], (uint)Properties.Settings.Default["ZK_positionDeceleration"], 1, 1, 1, -110000, 108000, -45, 45);
            motors[MotorId.PZ_ZK].inicialize(connector, motorStateObserver, motorErrorOccuredObserver, 13, MotorId.PZ_ZK, MotorMode.position, false, 4, (uint)Properties.Settings.Default["ZK_positionVelocity"], (uint)Properties.Settings.Default["ZK_positionAceleration"], (uint)Properties.Settings.Default["ZK_positionDeceleration"], 1, 1, 1, -110000, 108000, -45, 45);
        }

        /// <summary>
        /// Zvýšit/snížit robota krok 1 : narovnat kola
        /// </summary>
        /// <param name="direction">-1 = down, 1 = up</param>
        private void moveDownUp(int direction)
        {
            motors[MotorId.PP_P].disable();
            motors[MotorId.LP_P].disable();
            motors[MotorId.LZ_P].disable();
            motors[MotorId.PZ_P].disable();
            motors[MotorId.PP_R].moveToAngle(90);
            motors[MotorId.LP_R].moveToAngle(90);
            motors[MotorId.LZ_R].moveToAngle(90);
            motors[MotorId.PZ_R].moveToAngle(90);
            createPeriodicChecker();
            periodicChecker.Elapsed += delegate { moveDownUpPeriodic(direction); };

        }

        /// <summary>
        /// Zvýšit/snížit robota krok 2 : pohnut se Z
        /// </summary>
        /// <param name="direction">-1 = down, 1 = up</param>
        private void moveDownUpPeriodic(int direction)
        {
            if (motors[MotorId.PP_R].isTargetReached() && motors[MotorId.LP_R].isTargetReached() && motors[MotorId.LZ_R].isTargetReached() && motors[MotorId.PZ_R].isTargetReached())
            {
                periodicChecker.Dispose();

                if (direction > 0)
                {
                    motors[MotorId.PP_Z].moveToMinPosition();
                    motors[MotorId.LP_Z].moveToMinPosition();
                    motors[MotorId.LZ_Z].moveToMinPosition();
                    motors[MotorId.PZ_Z].moveToMinPosition();
                }
                else {
                    motors[MotorId.PP_P].enable();
                    motors[MotorId.LP_P].enable();
                    motors[MotorId.LZ_P].enable();
                    motors[MotorId.PZ_P].enable();

                    motors[MotorId.PP_P].moving(-50);
                    motors[MotorId.LP_P].moving(-50);
                    motors[MotorId.LZ_P].moving(50);
                    motors[MotorId.PZ_P].moving(50);

                    motors[MotorId.PP_Z].moveToMaxPosition();
                    motors[MotorId.LP_Z].moveToMaxPosition();
                    motors[MotorId.LZ_Z].moveToMaxPosition();
                    motors[MotorId.PZ_Z].moveToMaxPosition();
                }
                createPeriodicChecker();
                periodicChecker.Elapsed += delegate { enablePAfterZPeriodic(); };
            }
        }

        /// <summary>
        /// Zvýšit/snížit robota krok 3 : zapnout P
        /// </summary>
        private void enablePAfterZPeriodic()
        {
            if (motors[MotorId.PP_Z].isTargetReached() && motors[MotorId.LP_Z].isTargetReached() && motors[MotorId.LZ_Z].isTargetReached() && motors[MotorId.PZ_Z].isTargetReached())
            {
                periodicChecker.Dispose();

                motors[MotorId.PP_P].halt();
                motors[MotorId.LP_P].halt();
                motors[MotorId.LZ_P].halt();
                motors[MotorId.PZ_P].halt();

                motors[MotorId.PP_P].enable();
                motors[MotorId.LP_P].enable();
                motors[MotorId.LZ_P].enable();
                motors[MotorId.PZ_P].enable();
            }
        }

        /// <summary>
        /// Rozšíří, zůží robota
        /// </summary>
        /// <param name="direction">-1 = zůžit, 1 = rozšířit</param>
        private void narrowWiden(int direction)
        {
            if (motors[MotorId.PP_Z].isTargetReached() && motors[MotorId.LP_Z].isTargetReached() && motors[MotorId.LZ_Z].isTargetReached() && motors[MotorId.PZ_Z].isTargetReached())
            {
                if (periodicChecker != null)
                {
                    periodicChecker.Dispose();
                }

                motors[MotorId.PP_P].disable();
                motors[MotorId.LP_P].disable();
                motors[MotorId.LZ_P].disable();
                motors[MotorId.PZ_P].disable();
                motors[MotorId.PP_R].moveToPosition(0);
                motors[MotorId.LP_R].moveToPosition(0);
                motors[MotorId.LZ_R].moveToPosition(0);
                motors[MotorId.PZ_R].moveToPosition(0);
                createPeriodicChecker();
                periodicChecker.Elapsed += delegate { narrowWidenPeriodic(direction); };
            }
        }

        /// <summary>
        /// Rozšíří, zůží robota
        /// </summary>
        /// <param name="direction">-1 = zůžit, 1 = rozšířit</param>
        private void narrowWidenPeriodic(int direction)
        {
            if (motors[MotorId.PP_R].isTargetReached() && motors[MotorId.LP_R].isTargetReached() && motors[MotorId.LZ_R].isTargetReached() && motors[MotorId.PZ_R].isTargetReached())
            {
                periodicChecker.Dispose();

                if (direction > 0)
                {
                    motors[MotorId.PP_ZK].moveToMaxPosition();
                    motors[MotorId.LP_ZK].moveToMaxPosition();
                    motors[MotorId.LZ_ZK].moveToMaxPosition();
                    motors[MotorId.PZ_ZK].moveToMaxPosition();
                }
                else
                {
                    motors[MotorId.PP_ZK].moveToMinPosition();
                    motors[MotorId.LP_ZK].moveToMinPosition();
                    motors[MotorId.LZ_ZK].moveToMinPosition();
                    motors[MotorId.PZ_ZK].moveToMinPosition();

                }

                createPeriodicChecker();
                periodicChecker.Elapsed += delegate { enablePAfterZKPeriodic(); };
            }
        }

        /// <summary>
        /// Zvýšit/snížit robota krok 3 : zapnout P
        /// </summary>
        private void enablePAfterZKPeriodic()
        {
            if (motors[MotorId.PP_ZK].isTargetReached() && motors[MotorId.LP_ZK].isTargetReached() && motors[MotorId.LZ_ZK].isTargetReached() && motors[MotorId.PZ_ZK].isTargetReached())
            {
                periodicChecker.Dispose();
                motors[MotorId.PP_P].enable();
                motors[MotorId.LP_P].enable();
                motors[MotorId.LZ_P].enable();
                motors[MotorId.PZ_P].enable();
            }
        }

        /// <summary>
        /// Vypočítá úhel natočení kola po daném rádiusu a podle stavu nohy
        /// </summary>
        /// <param name="radiusCircleDistance">vzdálenost rádiusové kružnice (0 - 2000) pro >2000 bere jako přímý pohyb</param>
        /// <param name="legAngle">úhel natočení nohy</param>
        /// <param name="legLength">délka nohy</param>
        /// <param name="xOrigin">x souřadnice počátku nohy</param>
        /// <param name="yOrigin">y souřadnice počátku nohy</param>
        /// <returns>úhel, do kterého se má natočit kolo</returns>
        private double getWheelAngleForRadiusMove(double radiusCircleDistance, double legAngle, double legLength, double xOrigin, double yOrigin)
        {
            MathLibrary.Line legLine = getLegLine(legAngle, xOrigin, yOrigin);//přímka nohy
            MathLibrary.Point endPoint = MathLibrary.getPointOnLineInDistance(legLine, new MathLibrary.Point((int)xOrigin, (int)yOrigin), legLength); // koncový bod nohy
            double radiusCircleDistanceCorected = radiusCircleDistance + Math.Sign(radiusCircleDistance) * Math.Sign(xOrigin) * endPoint.X; //x souřadnice středu rádiusové kružnice
            MathLibrary.Circle radiusCircle = new MathLibrary.Circle(new MathLibrary.Point(radiusCircleDistanceCorected, 0), endPoint); //rádiusová kružnice 
            MathLibrary.Line tangent = radiusCircle.getTangent(endPoint); //tečna k rádiusové kružnici na konci nohy
            if (radiusCircleDistance > 2000)
            { //rovně
                tangent = new MathLibrary.Line(endPoint.X, endPoint.Y, -90);
            }
            MathLibrary.Line legEndLine = legLine.getNormal(endPoint); //přímka konce nohy (kolmá na nohu)
            double angle = MathLibrary.getDeviation(tangent, legEndLine);
            if (radiusCircleDistance < 0)
            { //zatáčení doleva
                angle = 180 - angle;
            }
            if ((xOrigin < 0 && yOrigin > 0 && ((radiusCircleDistance > 0 && legEndLine.k > tangent.k) || (radiusCircleDistance < 0 && legEndLine.k < tangent.k)) && !tangent.vertical) ||
                (xOrigin < 0 && yOrigin < 0 && ((radiusCircleDistance > 0 && legEndLine.k < tangent.k) || (radiusCircleDistance < 0 && legEndLine.k > tangent.k)) && !tangent.vertical) ||
                (xOrigin > 0 && yOrigin > 0 && ((radiusCircleDistance > 0 && legEndLine.k > tangent.k) || (radiusCircleDistance < 0 && legEndLine.k < tangent.k)) && !tangent.vertical) ||
                (xOrigin > 0 && yOrigin < 0 && ((radiusCircleDistance > 0 && legEndLine.k < tangent.k) || (radiusCircleDistance < 0 && legEndLine.k > tangent.k)) && !tangent.vertical) ||
                (xOrigin < 0 && yOrigin < 0 && radiusCircleDistance > 0 && legEndLine.vertical) ||
                (xOrigin < 0 && yOrigin > 0 && radiusCircleDistance > 0 && legEndLine.vertical) ||
                (xOrigin > 0 && yOrigin < 0 && radiusCircleDistance < 0 && legEndLine.vertical) ||
                (xOrigin > 0 && yOrigin > 0 && radiusCircleDistance < 0 && legEndLine.vertical))
            {
                angle = -angle;
            }
            if (xOrigin < 0 && yOrigin > 0 && legLine.vertical)
            {
                angle = 180 - angle;
            }
            return angle;
        }

        /// <summary>
        /// Vrátí přímku představující nohu
        /// </summary>
        /// <param name="legAngle">úhel natočení nohy</param>
        /// <param name="xOrigin">x souřadnice počátku nohy</param>
        /// <param name="yOrigin">y souřadnice počátku nohy</param>
        /// <returns>přímku představující nohu</returns>
        private MathLibrary.Line getLegLine(double legAngle, double xOrigin, double yOrigin)
        {
            if (legAngle == 0)
            {
                legAngle = 0.001;
            }
            double angleRelativeToXAxis = legAngle;

            if (xOrigin < 0 && yOrigin > 0)
            {
                angleRelativeToXAxis = 180 - angleRelativeToXAxis;
            }
            else if (xOrigin < 0 && yOrigin < 0)
            {
                angleRelativeToXAxis = 180 + angleRelativeToXAxis;
            }
            else if (xOrigin > 0 && yOrigin < 0)
            {
                angleRelativeToXAxis = -angleRelativeToXAxis;
            }

            return new MathLibrary.Line(xOrigin, yOrigin, angleRelativeToXAxis);
        }

        /// <summary>
        /// Vypočítá délku standartizovaného oblouku na dané rádiusové kružnici pro danou nohu
        /// </summary>
        /// <param name="radiusCircleDistance">vzdálenost rádiusové kružnice (0 - 2000) pro >2000 bere jako přímý pohyb</param>
        /// <param name="legAngle">úhel natočení nohy</param>
        /// <param name="legLength">délka nohy</param>
        /// <param name="xOrigin">x souřadnice počátku nohy</param>
        /// <param name="yOrigin">y souřadnice počátku nohy</param>
        /// <returns>úhel, do kterého se má natočit kolo</returns>
        private double getStandardizedArcLeangth(double radiusCircleDistance, double legAngle, double legLength, double xOrigin, double yOrigin)
        {
            MathLibrary.Line legLine = getLegLine(legAngle, xOrigin, yOrigin);//přímka nohy
            MathLibrary.Point endPoint = MathLibrary.getPointOnLineInDistance(legLine, new MathLibrary.Point((int)xOrigin, (int)yOrigin), legLength); // koncový bod nohy
            double radiusCircleDistanceCorected = radiusCircleDistance + Math.Sign(radiusCircleDistance) * Math.Sign(xOrigin) * endPoint.X; //x souřadnice středu rádiusové kružnice
            MathLibrary.Circle radiusCircle = new MathLibrary.Circle(new MathLibrary.Point(radiusCircleDistanceCorected, 0), endPoint); //rádiusová kružnice
            return MathLibrary.getArcLength(radiusCircle.r, 90);
        }

        /// <summary>
        /// Uloží současné pozice motorů
        /// </summary>
        private void saveCurrentPositions()
        {
            bool allMotorsOk = true;
            foreach (KeyValuePair<MotorId, IMotor> motor in motors)
            {
                try
                {
                    Properties.Settings.Default[motor.Key.ToString()] = motor.Value.getPosition();
                }
                catch (DeviceException)
                {
                    allMotorsOk = false;
                    break;
                }
            }

            Properties.Settings.Default.correctlyEnded = allMotorsOk;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Zkontroluje, zda jsou všechny motory v pořádku
        /// </summary>
        /// <returns>true pokud josu</returns>
        private bool allMotorsOK()
        {
            foreach (KeyValuePair<MotorId, IMotor> motor in motors)
            {
                if (motor.Value.state == MotorState.error || motor.Value.state == MotorState.disabled)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Nahlášení chyby motoru
        /// </summary>
        private void motorErrorOccuredObserverFunction()
        {
            motorErrorOccuredObserver();
        }

        /// <summary>
        /// Nastaví periodický vyvolávač různých funkcí
        /// </summary>
        private void createPeriodicChecker()
        {
            if (periodicChecker != null)
            {
                periodicChecker.Dispose();
            }
            periodicChecker = new System.Timers.Timer();
            periodicChecker.Interval = 50;
            periodicChecker.Enabled = true;
        }

        /// <summary>
        /// Nastaví periodický vyvolávač kroků pro zužování předku
        /// </summary>
        private void createPeriodicCheckerFrontNarrow()
        {
            if (periodicCheckerFrontNarrow != null)
            {
                periodicCheckerFrontNarrow.Dispose();
            }
            periodicCheckerFrontNarrow = new System.Timers.Timer();
            periodicCheckerFrontNarrow.Interval = 50;
            periodicCheckerFrontNarrow.Enabled = true;
        }

        /// <summary>
        /// Nastaví periodický vyvolávač kroků pro zužování zadku
        /// </summary>
        private void createPeriodicCheckerBackNarrow()
        {
            if (periodicCheckerBackNarrow != null)
            {
                periodicCheckerBackNarrow.Dispose();
            }
            periodicCheckerBackNarrow = new System.Timers.Timer();
            periodicCheckerBackNarrow.Interval = 50;
            periodicCheckerBackNarrow.Enabled = true;
        }
    }
}