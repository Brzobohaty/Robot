using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;

namespace Robot
{
    public partial class MainWindow : Form
    {
        private static MainWindow instance = new MainWindow(); //jediná instance této třídy
        private Action inicializeObserver; //callback pro dokončení view
        private Action closeObserver; //callback pro ukončení aplikace

        private MainWindow()
        {
            InitializeComponent();

            ControllView controllView = ControllView.getInstance();
            splitContainer1.Panel1.Controls.Add(controllView);
            controllView.Dock = DockStyle.Fill;

            DiagnosticView diagnosticView = DiagnosticView.getInstance();
            splitContainer1.Panel2.Controls.Add(diagnosticView);
            diagnosticView.Dock = DockStyle.Fill;
        }

        public static MainWindow getInstance()
        {
            return instance;
        }
        
        /// <summary>
        /// Přiřazení posluchače pro dokončení a zobrazení view
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu</param>
        public void subscribeWindowShownObserver(Action observer)
        {
            inicializeObserver = observer;
        }

        /// <summary>
        /// Přiřazení posluchače pro ukončení aplikace
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu</param>
        public void subscribeWindowCloseObserver(Action observer)
        {
            closeObserver = observer;
        }

        //event listenery ================================================================

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            Application.DoEvents();
            inicializeObserver();
            splitContainer1.Select();
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            closeObserver();
        }
    }
}