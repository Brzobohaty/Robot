using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Robot.Views
{
    public partial class MotorsSettings : Form
    {
        private static MotorsSettings instance = new MotorsSettings(); //jediná instance této třídy
        private Action setingsChangedObserver; //callback pro případ, že byly změněny parametry motorů

        public static MotorsSettings getInstance()
        {
            return instance;
        }

        private MotorsSettings()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Přiřadí posluchač pro událost změny parametrů motorů
        /// </summary>
        /// <param name="observer">posluchač</param>
        public void subscribeMotorsSetingsChanged(Action observer)
        {
            setingsChangedObserver = observer;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Touto operací nastavíte permanentně parametry motorů na vámi určené hodnoty. Pokud jste nastavili hodnoty špatně, může dojít k poškození robota nebo motorů. Opravdu si přejete hodnoty uložit.", "Nastavení motorů", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                Properties.Settings.Default["P_positionVelocity"] = (uint)P_positionVelocity.Value;
                Properties.Settings.Default["P_positionAceleration"] = (uint)P_positionAceleration.Value;
                Properties.Settings.Default["P_positionDeceleration"] = (uint)P_positionDeceleration.Value;
                Properties.Settings.Default["P_maxVelocity"] = (uint)P_velocity.Value;
                Properties.Settings.Default["P_aceleration"] = (uint)P_aceleration.Value;
                Properties.Settings.Default["P_deceleration"] = (uint)P_deceleration.Value;
                Properties.Settings.Default["R_positionVelocity"] = (uint)R_positionVelocity.Value;
                Properties.Settings.Default["R_positionAceleration"] = (uint)R_positionAceleration.Value;
                Properties.Settings.Default["R_positionDeceleration"] = (uint)R_positionDeceleration.Value;
                Properties.Settings.Default["Z_positionVelocity"] = (uint)Z_positionVelocity.Value;
                Properties.Settings.Default["Z_positionAceleration"] = (uint)Z_positionAceleration.Value;
                Properties.Settings.Default["Z_positionDeceleration"] = (uint)Z_positionDeceleration.Value;
                Properties.Settings.Default["ZK_positionVelocity"] = (uint)ZK_positionVelocity.Value;
                Properties.Settings.Default["ZK_positionAceleration"] = (uint)ZK_positionAceleration.Value;
                Properties.Settings.Default["ZK_positionDeceleration"] = (uint)ZK_positionDeceleration.Value;
                Properties.Settings.Default.Save();
                setingsChangedObserver();
                Close();
            }
        }

        private void MotorsSettings_Activated(object sender, EventArgs e)
        {
            Console.WriteLine("ssssssss");
            P_positionVelocity.Value = (uint)Properties.Settings.Default["P_positionVelocity"];
            P_positionAceleration.Value = (uint)Properties.Settings.Default["P_positionAceleration"];
            P_positionDeceleration.Value = (uint)Properties.Settings.Default["P_positionDeceleration"];
            P_velocity.Value = (uint)Properties.Settings.Default["P_maxVelocity"];
            P_aceleration.Value = (uint)Properties.Settings.Default["P_aceleration"];
            P_deceleration.Value = (uint)Properties.Settings.Default["P_deceleration"];
            R_positionVelocity.Value = (uint)Properties.Settings.Default["R_positionVelocity"];
            R_positionAceleration.Value = (uint)Properties.Settings.Default["R_positionAceleration"];
            R_positionDeceleration.Value = (uint)Properties.Settings.Default["R_positionDeceleration"];
            Z_positionVelocity.Value = (uint)Properties.Settings.Default["Z_positionVelocity"];
            Z_positionAceleration.Value = (uint)Properties.Settings.Default["Z_positionAceleration"];
            Z_positionDeceleration.Value = (uint)Properties.Settings.Default["Z_positionDeceleration"];
            ZK_positionVelocity.Value = (uint)Properties.Settings.Default["ZK_positionVelocity"];
            ZK_positionAceleration.Value = (uint)Properties.Settings.Default["ZK_positionAceleration"];
            ZK_positionDeceleration.Value = (uint)Properties.Settings.Default["ZK_positionDeceleration"];
        }

        private void buttonDefault_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default["P_positionVelocity"] = (uint)5000;
            Properties.Settings.Default["P_positionAceleration"] = (uint)2000;
            Properties.Settings.Default["P_positionDeceleration"] = (uint)2000;
            Properties.Settings.Default["P_maxVelocity"] = (uint)5000;
            Properties.Settings.Default["P_aceleration"] = (uint)2500;
            Properties.Settings.Default["P_deceleration"] = (uint)2500;
            Properties.Settings.Default["R_positionVelocity"] = (uint)4000;
            Properties.Settings.Default["R_positionAceleration"] = (uint)4000;
            Properties.Settings.Default["R_positionDeceleration"] = (uint)4000;
            Properties.Settings.Default["Z_positionVelocity"] = (uint)1000;
            Properties.Settings.Default["Z_positionAceleration"] = (uint)2000;
            Properties.Settings.Default["Z_positionDeceleration"] = (uint)2000;
            Properties.Settings.Default["ZK_positionVelocity"] = (uint)5000;
            Properties.Settings.Default["ZK_positionAceleration"] = (uint)2500;
            Properties.Settings.Default["ZK_positionDeceleration"] = (uint)2500;
            Properties.Settings.Default.Save();
            setingsChangedObserver();
            Close();
        }
    }
}
