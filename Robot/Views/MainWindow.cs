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
        private bool absoluteControllMode = false; //příznak, zda je zapnut absolutní ovládácí mód
        private ControllView controllView; //panel s ovládáním robota pomocí joysticku
        private AbsoluteControllView absoluteControllView; //panel s absolutním ovládáním jednotlivých motorů robota

        private MainWindow()
        {
            InitializeComponent();

            controllView = ControllView.getInstance();
            controllView.Dock = DockStyle.Fill;

            absoluteControllView = AbsoluteControllView.getInstance();
            absoluteControllView.Dock = DockStyle.Fill;

            splitContainer1.Panel1.Controls.Add(controllView);

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

        /// <summary>
        /// Změní ovládací mód 
        /// </summary>
        public void changeControllMode() {
            if (absoluteControllMode)
            {
                absoluteControllMode = false;
                splitContainer1.Panel1.Controls.Clear();
                splitContainer1.Panel1.Controls.Add(controllView);
            }
            else {
                absoluteControllMode = true;
                splitContainer1.Panel1.Controls.Clear();
                splitContainer1.Panel1.Controls.Add(absoluteControllView);
            }
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