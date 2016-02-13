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
using Robot.Views;

namespace Robot
{
    public partial class MainWindow : Form
    {
        private static MainWindow instance = new MainWindow(); //jediná instance této třídy
        private Action inicializeObserver; //callback pro dokončení view
        private Action closeObserver; //callback pro ukončení aplikace
        private ControllView controllView; //panel s ovládáním robota pomocí joysticku
        private AbsoluteControllView absoluteControllView; //panel s absolutním ovládáním jednotlivých motorů robota
        private DiagnosticView diagnosticView; //panel s diagnostikou robota
        private RecalibrView recalibrView; //panel s návodem k rekalibraci

        private MainWindow()
        {
            InitializeComponent();

            controllView = ControllView.getInstance();
            controllView.Dock = DockStyle.Fill;

            absoluteControllView = AbsoluteControllView.getInstance();
            absoluteControllView.Dock = DockStyle.Fill;

            diagnosticView = DiagnosticView.getInstance();
            diagnosticView.Dock = DockStyle.Fill;

            recalibrView = RecalibrView.getInstance();
            recalibrView.Dock = DockStyle.Fill;

            splitContainer1.Panel1.Controls.Add(controllView);
            splitContainer1.Panel2.Controls.Add(diagnosticView);
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
        /// <param name="absoluteControllMode">true, pokud zobrazit absolutní pozicování</param>
        public void changeControllMode(bool absoluteControllMode) {
            if (absoluteControllMode)
            {
                splitContainer1.Panel1.Controls.Clear();
                splitContainer1.Panel1.Controls.Add(absoluteControllView);
            }
            else {
                splitContainer1.Panel1.Controls.Clear();
                splitContainer1.Panel1.Controls.Add(controllView);
            }
        }

        /// <summary>
        /// Prohodí view diagnostiky za view rekalibrace nebo opačně
        /// </summary>
        /// <param name="diagnosticViewVisible">true, pokud zobrazit diagnostiku</param>
        public void changeDiagnosticView(bool diagnosticViewVisible)
        {
            if (diagnosticViewVisible)
            {
                splitContainer1.Panel2.Controls.Clear();
                splitContainer1.Panel2.Controls.Add(diagnosticView);
            }
            else
            {
                splitContainer1.Panel2.Controls.Clear();
                splitContainer1.Panel2.Controls.Add(recalibrView);
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