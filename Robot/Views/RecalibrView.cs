using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Robot.Views
{
    public partial class RecalibrView : UserControl
    {
        private static RecalibrView instance = new RecalibrView();

        private RecalibrView()
        {
            InitializeComponent();
        }

        public static RecalibrView getInstance()
        {
            return instance;
        }
    }
}
