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
        /// Zobrazí chybovou hlášku týkající se motorů
        /// </summary>
        /// <param name="message">text chyby</param>
        public void showMotorsError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            errorLabelMotors.Text = message;
        }
    }
}
