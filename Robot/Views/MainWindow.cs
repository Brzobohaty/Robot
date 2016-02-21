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
        private Action buttonForRecalibrClickedObserver; //calback pro stisknutí tačítka pro rekalibraci
        private Action buttonForConnectionSettingsClickedObserver; //calback pro stisknutí tačítka pro nastavení připojení
        private Action buttonForReinicializeClickedObserver; //calback pro stisknutí tačítka pro reinicializaci robota

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

        delegate void ChangeControllModeCallback(bool absoluteControllMode);

        /// <summary>
        /// Změní ovládací mód 
        /// </summary>
        /// <param name="absoluteControllMode">true, pokud zobrazit absolutní pozicování</param>
        public void changeControllMode(bool absoluteControllMode) {
            if (splitContainer1.InvokeRequired)
            {
                ChangeControllModeCallback cb = new ChangeControllModeCallback(changeControllMode);
                this.Invoke(cb, new object[] { absoluteControllMode });
            }
            else
            {
                if (absoluteControllMode)
                {
                    splitContainer1.Panel1.Controls.Clear();
                    splitContainer1.Panel1.Controls.Add(absoluteControllView);
                }
                else
                {
                    splitContainer1.Panel1.Controls.Clear();
                    splitContainer1.Panel1.Controls.Add(controllView);
                }
            }
        }

        delegate void ChangeDiagnosticViewCallback(bool diagnosticViewVisible);
        
        /// <summary>
        /// Prohodí view diagnostiky za view rekalibrace nebo opačně
        /// </summary>
        /// <param name="diagnosticViewVisible">true, pokud zobrazit diagnostiku</param>
        public void changeDiagnosticView(bool diagnosticViewVisible)
        {
            if (splitContainer1.InvokeRequired)
            {
                ChangeDiagnosticViewCallback cb = new ChangeDiagnosticViewCallback(changeDiagnosticView);
                this.Invoke(cb, new object[] { diagnosticViewVisible });
            }
            else
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
        }

        /// <summary>
        /// Přiřazení posluchače pro stisknutí tačítka pro rekalibraci
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu</param>
        public void subscribeButtonForRecalibrClickObserver(Action observer)
        {
            buttonForRecalibrClickedObserver = observer;
        }

        /// <summary>
        /// Přiřazení posluchače pro stisknutí tačítka pro nastavení připojení
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu</param>
        public void subscribeButtonForConnectionSettingsClickObserver(Action observer)
        {
            buttonForConnectionSettingsClickedObserver = observer;
        }

        /// <summary>
        /// Přiřazení posluchače pro stisknutí tačítka pro reinicializaci robota
        /// </summary>
        /// <param name="observer">metoda vykonaná při eventu</param>
        public void subscribeButtonForReinicializeClickObserver(Action observer)
        {
            buttonForReinicializeClickedObserver = observer;
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

        private void menuConnectionItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Před nastavením připojení budou všechny motory automaticky vypnuty. Chcete pokračovat?", "Nastavení připojení", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                buttonForConnectionSettingsClickedObserver();
            }
        }

        private void menuRekalibrItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Rekalibrace se provádí tak, že uživatel nastaví všechny motory do nulové polohy a následně jsou tyto polohy brány ve všech výpočtech jako referenční. Pokud uživatel nastaví hodnoty špatně, může dojít k poškození robota. Nebylo detekováno narušení referenčních hodnot. Chcete přesto pokračovat v rekalibraci?", "Rekalibrace", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                buttonForRecalibrClickedObserver();
            }
        }

        private void menuReinicializeItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Všechny motory budou vypnuty a bude ukončena komunikace s robotem. Následně bude komunikace obnovena a znovu zapnuty všechny motory. Pokračovat?", "Reinicializace", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                buttonForReinicializeClickedObserver();
            }
        }

        private void menuItemMotorsSettings_Click(object sender, EventArgs e)
        {
            MotorsSettings.getInstance().ShowDialog();
        }
    }
}