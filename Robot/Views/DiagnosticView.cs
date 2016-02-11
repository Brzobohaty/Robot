using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Robot
{
    public partial class DiagnosticView : UserControl
    {
        private static DiagnosticView instance = new DiagnosticView();
        private Dictionary<MotorId, Label> motorViews = new Dictionary<MotorId, Label>(); //mapa view motorů

        public DiagnosticView()
        {
            InitializeComponent();
            motorViews.Add(MotorId.LP_P, labelLP_P);
            motorViews.Add(MotorId.PP_P, labelPP_P);
            motorViews.Add(MotorId.LZ_P, labelLZ_P);
            motorViews.Add(MotorId.PZ_P, labelPZ_P);
            motorViews.Add(MotorId.PP_R, labelPP_R);
            motorViews.Add(MotorId.LP_R, labelLP_R);
            motorViews.Add(MotorId.LZ_R, labelLZ_R);
            motorViews.Add(MotorId.PZ_R, labelPZ_R);
            motorViews.Add(MotorId.PP_Z, labelPP_Z);
            motorViews.Add(MotorId.LP_Z, labelLP_Z);
            motorViews.Add(MotorId.LZ_Z, labelLZ_Z);
            motorViews.Add(MotorId.PZ_Z, labelPZ_Z);
            motorViews.Add(MotorId.PP_ZK, labelPP_ZK);
            motorViews.Add(MotorId.LP_ZK, labelLP_ZK);
            motorViews.Add(MotorId.LZ_ZK, labelLZ_ZK);
            motorViews.Add(MotorId.PZ_ZK, labelPZ_ZK);
        }

        public static DiagnosticView getInstance()
        {
            return instance;
        }

        /// <summary>
        /// Zobrazí chybovou hlášku týkající se sběrnice
        /// </summary>
        /// <param name="message">text chyby</param>
        public void showBusError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            errorLabelMotors.Text = message;
        }

        /// <summary>
        /// Zobrazí stav konkrétního motoru
        /// </summary>
        /// <param name="state">stav</param>
        /// <param name="message">zpráva ke stavu</param>
        /// <param name="motorId">id motoru</param>
        /// <param name="withoutUpdate">příznak, zda se nemá updatvat okamžitě view (musí se udělat, pokud se přistupuje z jiného vlákna)</param>
        /// <param name="speed">aktuální rychlost motoru</param>
        public void showMotorState(MotorState state, string message, MotorId motorId, bool withoutUpdate, int speed)
        {
            foreach (KeyValuePair<MotorId, Label> motorView in motorViews)
            {
                _showMotorState(motorView.Value, state, message, motorView.Key, withoutUpdate, speed);
                
            }
        }

        /// <summary>
        /// Zobrazí stav konkrétního motoru
        /// </summary>
        /// <param name="motorView">vizualizace motoru</param>
        /// <param name="state">stav</param>
        /// <param name="message">zpráva ke stavu</param>
        /// <param name="motorId">id motoru</param>
        /// <param name="withoutUpdate">příznak, zda se nemá updatvat okamžitě view (musí se udělat, pokud se přistupuje z jiného vlákna)</param>
        private void _showMotorState(Label motorView, MotorState state, String message, MotorId motorId, bool withoutUpdate, int speed) {
            switch (state)
            {
                case MotorState.error:
                    motorView.BackColor = Color.Red;
                    break;
                case MotorState.enabled:
                    motorView.BackColor = Color.Green;
                    break;
                case MotorState.disabled:
                    motorView.BackColor = Color.LightSlateGray;
                    break;
                case MotorState.running:
                    motorView.BackColor = Color.Orange;
                    break;
            }
            if (!withoutUpdate) {
                motorView.Update();
            }
        }
    }
}
