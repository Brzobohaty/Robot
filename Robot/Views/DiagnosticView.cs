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

        public DiagnosticView()
        {
            InitializeComponent();
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
        /// <param name="state">stav ["error", "enabled", "disabled", "running"]</param>
        /// <param name="message">zpráva ke stavu</param>
        /// <param name="motorId">id motoru ["PP_P", "LP_P", "LZ_P", "PZ_P", "PP_R", "LP_R", "LZ_R", "PZ_R", "PP_Z", "LP_Z", "LZ_Z", "PZ_Z", "PP_ZK", "LP_ZK", "LZ_ZK", "PZ_ZK"]</param>
        public void showMotorState(string state, string message, string motorId)
        {
            switch (motorId) {
                case "PP_P": break;
                case "LP_P": break;
                case "LZ_P": break;
                case "PZ_P": break;
                case "PP_R": break;
                case "LP_R": break;
                case "LZ_R": break;
                case "PZ_R": break;
                case "PP_Z": break;
                case "LP_Z": break;
                case "LZ_Z": break;
                case "PZ_Z": break;
                case "PP_ZK": break;
                case "LP_ZK": break;
                case "LZ_ZK": break;
                case "PZ_ZK": break;
            }
        }
    }
}
