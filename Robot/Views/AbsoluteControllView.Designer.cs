namespace Robot
{
    partial class AbsoluteControllView
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AbsoluteControllView));
            this.label1 = new System.Windows.Forms.Label();
            this.buttonJoystickPositioning = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkBoxLimitProtection = new System.Windows.Forms.CheckBox();
            this.buttonCancelRecalibration = new System.Windows.Forms.Button();
            this.buttonCalibr = new System.Windows.Forms.Button();
            this.buttonSetDefaultPosition = new System.Windows.Forms.Button();
            this.rightPZ_ZK = new System.Windows.Forms.Button();
            this.leftPZ_ZK = new System.Windows.Forms.Button();
            this.downPZ_Z = new System.Windows.Forms.Button();
            this.upPZ_Z = new System.Windows.Forms.Button();
            this.rightLZ_ZK = new System.Windows.Forms.Button();
            this.leftLZ_ZK = new System.Windows.Forms.Button();
            this.downLZ_Z = new System.Windows.Forms.Button();
            this.upLZ_Z = new System.Windows.Forms.Button();
            this.rightPP_ZK = new System.Windows.Forms.Button();
            this.leftPP_ZK = new System.Windows.Forms.Button();
            this.downPP_Z = new System.Windows.Forms.Button();
            this.upPP_Z = new System.Windows.Forms.Button();
            this.rightLP_ZK = new System.Windows.Forms.Button();
            this.leftLP_ZK = new System.Windows.Forms.Button();
            this.downLP_Z = new System.Windows.Forms.Button();
            this.upLP_Z = new System.Windows.Forms.Button();
            this.rightPZ_R = new System.Windows.Forms.Button();
            this.leftPZ_R = new System.Windows.Forms.Button();
            this.leftPZ_P = new System.Windows.Forms.Button();
            this.rightPZ_P = new System.Windows.Forms.Button();
            this.rightLZ_R = new System.Windows.Forms.Button();
            this.leftLZ_R = new System.Windows.Forms.Button();
            this.leftLZ_P = new System.Windows.Forms.Button();
            this.rightLZ_P = new System.Windows.Forms.Button();
            this.rightPP_R = new System.Windows.Forms.Button();
            this.leftPP_R = new System.Windows.Forms.Button();
            this.leftPP_P = new System.Windows.Forms.Button();
            this.rightPP_P = new System.Windows.Forms.Button();
            this.rightLP_R = new System.Windows.Forms.Button();
            this.leftLP_R = new System.Windows.Forms.Button();
            this.leftLP_P = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxStep = new System.Windows.Forms.ComboBox();
            this.rightLP_P = new System.Windows.Forms.Button();
            this.panelWithWarrning = new System.Windows.Forms.Panel();
            this.labelWarrning = new System.Windows.Forms.Label();
            this.labelWarningIcon = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            this.panelWithWarrning.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.GrayText;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(811, 55);
            this.label1.TabIndex = 11;
            this.label1.Text = "Ovládání";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonJoystickPositioning
            // 
            this.buttonJoystickPositioning.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonJoystickPositioning.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonJoystickPositioning.Location = new System.Drawing.Point(0, 945);
            this.buttonJoystickPositioning.Name = "buttonJoystickPositioning";
            this.buttonJoystickPositioning.Size = new System.Drawing.Size(811, 50);
            this.buttonJoystickPositioning.TabIndex = 15;
            this.buttonJoystickPositioning.Text = "Ovládání pomocí joystiku";
            this.toolTip1.SetToolTip(this.buttonJoystickPositioning, "Přepne ovládací mód na joystick.");
            this.buttonJoystickPositioning.UseVisualStyleBackColor = true;
            this.buttonJoystickPositioning.Click += new System.EventHandler(this.buttonJoystikPositioning_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.checkBoxLimitProtection);
            this.panel1.Controls.Add(this.buttonCancelRecalibration);
            this.panel1.Controls.Add(this.buttonCalibr);
            this.panel1.Controls.Add(this.buttonSetDefaultPosition);
            this.panel1.Controls.Add(this.rightPZ_ZK);
            this.panel1.Controls.Add(this.leftPZ_ZK);
            this.panel1.Controls.Add(this.downPZ_Z);
            this.panel1.Controls.Add(this.upPZ_Z);
            this.panel1.Controls.Add(this.rightLZ_ZK);
            this.panel1.Controls.Add(this.leftLZ_ZK);
            this.panel1.Controls.Add(this.downLZ_Z);
            this.panel1.Controls.Add(this.upLZ_Z);
            this.panel1.Controls.Add(this.rightPP_ZK);
            this.panel1.Controls.Add(this.leftPP_ZK);
            this.panel1.Controls.Add(this.downPP_Z);
            this.panel1.Controls.Add(this.upPP_Z);
            this.panel1.Controls.Add(this.rightLP_ZK);
            this.panel1.Controls.Add(this.leftLP_ZK);
            this.panel1.Controls.Add(this.downLP_Z);
            this.panel1.Controls.Add(this.upLP_Z);
            this.panel1.Controls.Add(this.rightPZ_R);
            this.panel1.Controls.Add(this.leftPZ_R);
            this.panel1.Controls.Add(this.leftPZ_P);
            this.panel1.Controls.Add(this.rightPZ_P);
            this.panel1.Controls.Add(this.rightLZ_R);
            this.panel1.Controls.Add(this.leftLZ_R);
            this.panel1.Controls.Add(this.leftLZ_P);
            this.panel1.Controls.Add(this.rightLZ_P);
            this.panel1.Controls.Add(this.rightPP_R);
            this.panel1.Controls.Add(this.leftPP_R);
            this.panel1.Controls.Add(this.leftPP_P);
            this.panel1.Controls.Add(this.rightPP_P);
            this.panel1.Controls.Add(this.rightLP_R);
            this.panel1.Controls.Add(this.leftLP_R);
            this.panel1.Controls.Add(this.leftLP_P);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.comboBoxStep);
            this.panel1.Controls.Add(this.rightLP_P);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 55);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(811, 890);
            this.panel1.TabIndex = 51;
            // 
            // checkBoxLimitProtection
            // 
            this.checkBoxLimitProtection.AutoSize = true;
            this.checkBoxLimitProtection.Checked = true;
            this.checkBoxLimitProtection.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxLimitProtection.Location = new System.Drawing.Point(338, 59);
            this.checkBoxLimitProtection.Name = "checkBoxLimitProtection";
            this.checkBoxLimitProtection.Size = new System.Drawing.Size(139, 21);
            this.checkBoxLimitProtection.TabIndex = 89;
            this.checkBoxLimitProtection.Text = "Ochrana dojezdů";
            this.toolTip1.SetToolTip(this.checkBoxLimitProtection, "Vypnutí/zapnutí ochrany dojezdů. Pokud je ochrana vypnutá, tak je možné, že se ně" +
        "které části robota dostanou do kolize.");
            this.checkBoxLimitProtection.UseVisualStyleBackColor = true;
            this.checkBoxLimitProtection.CheckedChanged += new System.EventHandler(this.checkBoxLimitProtection_CheckedChanged);
            // 
            // buttonCancelRecalibration
            // 
            this.buttonCancelRecalibration.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonCancelRecalibration.Location = new System.Drawing.Point(407, 763);
            this.buttonCancelRecalibration.Name = "buttonCancelRecalibration";
            this.buttonCancelRecalibration.Size = new System.Drawing.Size(329, 50);
            this.buttonCancelRecalibration.TabIndex = 88;
            this.buttonCancelRecalibration.Text = "Cancel";
            this.toolTip1.SetToolTip(this.buttonCancelRecalibration, "Ukončit proces kalibrace a neukládat žádná data.");
            this.buttonCancelRecalibration.UseVisualStyleBackColor = true;
            this.buttonCancelRecalibration.Visible = false;
            this.buttonCancelRecalibration.Click += new System.EventHandler(this.buttonCancelRecalibration_Click);
            // 
            // buttonCalibr
            // 
            this.buttonCalibr.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonCalibr.Location = new System.Drawing.Point(37, 763);
            this.buttonCalibr.Name = "buttonCalibr";
            this.buttonCalibr.Size = new System.Drawing.Size(327, 50);
            this.buttonCalibr.TabIndex = 87;
            this.buttonCalibr.Text = "Kalibrovat";
            this.toolTip1.SetToolTip(this.buttonCalibr, "Nastaví současné polohy motorů jako nulové, podle kterých se budou řídit veškeré " +
        "algoritmy pohybu.");
            this.buttonCalibr.UseVisualStyleBackColor = true;
            this.buttonCalibr.Visible = false;
            this.buttonCalibr.Click += new System.EventHandler(this.buttonCalibr_Click);
            // 
            // buttonSetDefaultPosition
            // 
            this.buttonSetDefaultPosition.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonSetDefaultPosition.Location = new System.Drawing.Point(37, 754);
            this.buttonSetDefaultPosition.Name = "buttonSetDefaultPosition";
            this.buttonSetDefaultPosition.Size = new System.Drawing.Size(327, 50);
            this.buttonSetDefaultPosition.TabIndex = 85;
            this.buttonSetDefaultPosition.Text = "Nastavit jako výchozí pozici";
            this.toolTip1.SetToolTip(this.buttonSetDefaultPosition, "Nastaví současnou polohu všech motorů jako výchozí.");
            this.buttonSetDefaultPosition.UseVisualStyleBackColor = true;
            this.buttonSetDefaultPosition.Click += new System.EventHandler(this.buttonSetDefaultPosition_Click);
            // 
            // rightPZ_ZK
            // 
            this.rightPZ_ZK.AccessibleName = "LP_";
            this.rightPZ_ZK.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.rightPZ_ZK.Image = ((System.Drawing.Image)(resources.GetObject("rightPZ_ZK.Image")));
            this.rightPZ_ZK.Location = new System.Drawing.Point(407, 504);
            this.rightPZ_ZK.Name = "rightPZ_ZK";
            this.rightPZ_ZK.Size = new System.Drawing.Size(70, 70);
            this.rightPZ_ZK.TabIndex = 84;
            this.rightPZ_ZK.UseVisualStyleBackColor = true;
            this.rightPZ_ZK.Click += new System.EventHandler(this.rightPZ_ZK_Click);
            // 
            // leftPZ_ZK
            // 
            this.leftPZ_ZK.AccessibleName = "LP_";
            this.leftPZ_ZK.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.leftPZ_ZK.Image = ((System.Drawing.Image)(resources.GetObject("leftPZ_ZK.Image")));
            this.leftPZ_ZK.Location = new System.Drawing.Point(483, 504);
            this.leftPZ_ZK.Name = "leftPZ_ZK";
            this.leftPZ_ZK.Size = new System.Drawing.Size(70, 70);
            this.leftPZ_ZK.TabIndex = 83;
            this.leftPZ_ZK.UseVisualStyleBackColor = true;
            this.leftPZ_ZK.Click += new System.EventHandler(this.leftPZ_ZK_Click);
            // 
            // downPZ_Z
            // 
            this.downPZ_Z.AccessibleName = "LP_";
            this.downPZ_Z.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.downPZ_Z.Image = ((System.Drawing.Image)(resources.GetObject("downPZ_Z.Image")));
            this.downPZ_Z.Location = new System.Drawing.Point(483, 428);
            this.downPZ_Z.Name = "downPZ_Z";
            this.downPZ_Z.Size = new System.Drawing.Size(70, 70);
            this.downPZ_Z.TabIndex = 82;
            this.downPZ_Z.UseVisualStyleBackColor = true;
            this.downPZ_Z.Click += new System.EventHandler(this.downPZ_Z_Click);
            // 
            // upPZ_Z
            // 
            this.upPZ_Z.AccessibleName = "LP_";
            this.upPZ_Z.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.upPZ_Z.Image = ((System.Drawing.Image)(resources.GetObject("upPZ_Z.Image")));
            this.upPZ_Z.Location = new System.Drawing.Point(407, 428);
            this.upPZ_Z.Name = "upPZ_Z";
            this.upPZ_Z.Size = new System.Drawing.Size(70, 70);
            this.upPZ_Z.TabIndex = 81;
            this.upPZ_Z.UseVisualStyleBackColor = true;
            this.upPZ_Z.Click += new System.EventHandler(this.upPZ_Z_Click);
            // 
            // rightLZ_ZK
            // 
            this.rightLZ_ZK.AccessibleName = "LP_";
            this.rightLZ_ZK.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.rightLZ_ZK.Image = ((System.Drawing.Image)(resources.GetObject("rightLZ_ZK.Image")));
            this.rightLZ_ZK.Location = new System.Drawing.Point(218, 504);
            this.rightLZ_ZK.Name = "rightLZ_ZK";
            this.rightLZ_ZK.Size = new System.Drawing.Size(70, 70);
            this.rightLZ_ZK.TabIndex = 80;
            this.rightLZ_ZK.UseVisualStyleBackColor = true;
            this.rightLZ_ZK.Click += new System.EventHandler(this.rightLZ_ZK_Click);
            // 
            // leftLZ_ZK
            // 
            this.leftLZ_ZK.AccessibleName = "LP_";
            this.leftLZ_ZK.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.leftLZ_ZK.Image = ((System.Drawing.Image)(resources.GetObject("leftLZ_ZK.Image")));
            this.leftLZ_ZK.Location = new System.Drawing.Point(294, 504);
            this.leftLZ_ZK.Name = "leftLZ_ZK";
            this.leftLZ_ZK.Size = new System.Drawing.Size(70, 70);
            this.leftLZ_ZK.TabIndex = 79;
            this.leftLZ_ZK.UseVisualStyleBackColor = true;
            this.leftLZ_ZK.Click += new System.EventHandler(this.leftLZ_ZK_Click);
            // 
            // downLZ_Z
            // 
            this.downLZ_Z.AccessibleName = "LP_";
            this.downLZ_Z.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.downLZ_Z.Image = ((System.Drawing.Image)(resources.GetObject("downLZ_Z.Image")));
            this.downLZ_Z.Location = new System.Drawing.Point(294, 428);
            this.downLZ_Z.Name = "downLZ_Z";
            this.downLZ_Z.Size = new System.Drawing.Size(70, 70);
            this.downLZ_Z.TabIndex = 78;
            this.downLZ_Z.UseVisualStyleBackColor = true;
            this.downLZ_Z.Click += new System.EventHandler(this.downLZ_Z_Click);
            // 
            // upLZ_Z
            // 
            this.upLZ_Z.AccessibleName = "LP_";
            this.upLZ_Z.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.upLZ_Z.Image = ((System.Drawing.Image)(resources.GetObject("upLZ_Z.Image")));
            this.upLZ_Z.Location = new System.Drawing.Point(218, 428);
            this.upLZ_Z.Name = "upLZ_Z";
            this.upLZ_Z.Size = new System.Drawing.Size(70, 70);
            this.upLZ_Z.TabIndex = 77;
            this.upLZ_Z.UseVisualStyleBackColor = true;
            this.upLZ_Z.Click += new System.EventHandler(this.upLZ_Z_Click);
            // 
            // rightPP_ZK
            // 
            this.rightPP_ZK.AccessibleName = "LP_";
            this.rightPP_ZK.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.rightPP_ZK.Image = ((System.Drawing.Image)(resources.GetObject("rightPP_ZK.Image")));
            this.rightPP_ZK.Location = new System.Drawing.Point(407, 319);
            this.rightPP_ZK.Name = "rightPP_ZK";
            this.rightPP_ZK.Size = new System.Drawing.Size(70, 70);
            this.rightPP_ZK.TabIndex = 76;
            this.rightPP_ZK.UseVisualStyleBackColor = true;
            this.rightPP_ZK.Click += new System.EventHandler(this.rightPP_ZK_Click);
            // 
            // leftPP_ZK
            // 
            this.leftPP_ZK.AccessibleName = "LP_";
            this.leftPP_ZK.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.leftPP_ZK.Image = ((System.Drawing.Image)(resources.GetObject("leftPP_ZK.Image")));
            this.leftPP_ZK.Location = new System.Drawing.Point(483, 319);
            this.leftPP_ZK.Name = "leftPP_ZK";
            this.leftPP_ZK.Size = new System.Drawing.Size(70, 70);
            this.leftPP_ZK.TabIndex = 75;
            this.leftPP_ZK.UseVisualStyleBackColor = true;
            this.leftPP_ZK.Click += new System.EventHandler(this.leftPP_ZK_Click);
            // 
            // downPP_Z
            // 
            this.downPP_Z.AccessibleName = "LP_";
            this.downPP_Z.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.downPP_Z.Image = ((System.Drawing.Image)(resources.GetObject("downPP_Z.Image")));
            this.downPP_Z.Location = new System.Drawing.Point(483, 243);
            this.downPP_Z.Name = "downPP_Z";
            this.downPP_Z.Size = new System.Drawing.Size(70, 70);
            this.downPP_Z.TabIndex = 74;
            this.downPP_Z.UseVisualStyleBackColor = true;
            this.downPP_Z.Click += new System.EventHandler(this.downPP_Z_Click);
            // 
            // upPP_Z
            // 
            this.upPP_Z.AccessibleName = "LP_";
            this.upPP_Z.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.upPP_Z.Image = ((System.Drawing.Image)(resources.GetObject("upPP_Z.Image")));
            this.upPP_Z.Location = new System.Drawing.Point(407, 243);
            this.upPP_Z.Name = "upPP_Z";
            this.upPP_Z.Size = new System.Drawing.Size(70, 70);
            this.upPP_Z.TabIndex = 73;
            this.upPP_Z.UseVisualStyleBackColor = true;
            this.upPP_Z.Click += new System.EventHandler(this.upPP_Z_Click);
            // 
            // rightLP_ZK
            // 
            this.rightLP_ZK.AccessibleName = "LP_";
            this.rightLP_ZK.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.rightLP_ZK.Image = ((System.Drawing.Image)(resources.GetObject("rightLP_ZK.Image")));
            this.rightLP_ZK.Location = new System.Drawing.Point(218, 319);
            this.rightLP_ZK.Name = "rightLP_ZK";
            this.rightLP_ZK.Size = new System.Drawing.Size(70, 70);
            this.rightLP_ZK.TabIndex = 72;
            this.rightLP_ZK.UseVisualStyleBackColor = true;
            this.rightLP_ZK.Click += new System.EventHandler(this.rightLP_ZK_Click);
            // 
            // leftLP_ZK
            // 
            this.leftLP_ZK.AccessibleName = "LP_";
            this.leftLP_ZK.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.leftLP_ZK.Image = ((System.Drawing.Image)(resources.GetObject("leftLP_ZK.Image")));
            this.leftLP_ZK.Location = new System.Drawing.Point(294, 319);
            this.leftLP_ZK.Name = "leftLP_ZK";
            this.leftLP_ZK.Size = new System.Drawing.Size(70, 70);
            this.leftLP_ZK.TabIndex = 71;
            this.leftLP_ZK.UseVisualStyleBackColor = true;
            this.leftLP_ZK.Click += new System.EventHandler(this.leftLP_ZK_Click);
            // 
            // downLP_Z
            // 
            this.downLP_Z.AccessibleName = "LP_";
            this.downLP_Z.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.downLP_Z.Image = ((System.Drawing.Image)(resources.GetObject("downLP_Z.Image")));
            this.downLP_Z.Location = new System.Drawing.Point(294, 243);
            this.downLP_Z.Name = "downLP_Z";
            this.downLP_Z.Size = new System.Drawing.Size(70, 70);
            this.downLP_Z.TabIndex = 70;
            this.downLP_Z.UseVisualStyleBackColor = true;
            this.downLP_Z.Click += new System.EventHandler(this.downLP_Z_Click);
            // 
            // upLP_Z
            // 
            this.upLP_Z.AccessibleName = "LP_";
            this.upLP_Z.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.upLP_Z.Image = ((System.Drawing.Image)(resources.GetObject("upLP_Z.Image")));
            this.upLP_Z.Location = new System.Drawing.Point(218, 243);
            this.upLP_Z.Name = "upLP_Z";
            this.upLP_Z.Size = new System.Drawing.Size(70, 70);
            this.upLP_Z.TabIndex = 69;
            this.upLP_Z.UseVisualStyleBackColor = true;
            this.upLP_Z.Click += new System.EventHandler(this.upLP_Z_Click);
            // 
            // rightPZ_R
            // 
            this.rightPZ_R.AccessibleName = "LP_";
            this.rightPZ_R.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.rightPZ_R.Image = ((System.Drawing.Image)(resources.GetObject("rightPZ_R.Image")));
            this.rightPZ_R.Location = new System.Drawing.Point(590, 627);
            this.rightPZ_R.Name = "rightPZ_R";
            this.rightPZ_R.Size = new System.Drawing.Size(70, 70);
            this.rightPZ_R.TabIndex = 68;
            this.rightPZ_R.UseVisualStyleBackColor = true;
            this.rightPZ_R.Click += new System.EventHandler(this.rightPZ_R_Click);
            // 
            // leftPZ_R
            // 
            this.leftPZ_R.AccessibleName = "LP_";
            this.leftPZ_R.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.leftPZ_R.Image = ((System.Drawing.Image)(resources.GetObject("leftPZ_R.Image")));
            this.leftPZ_R.Location = new System.Drawing.Point(666, 627);
            this.leftPZ_R.Name = "leftPZ_R";
            this.leftPZ_R.Size = new System.Drawing.Size(70, 70);
            this.leftPZ_R.TabIndex = 67;
            this.leftPZ_R.UseVisualStyleBackColor = true;
            this.leftPZ_R.Click += new System.EventHandler(this.leftPZ_R_Click);
            // 
            // leftPZ_P
            // 
            this.leftPZ_P.AccessibleName = "LP_";
            this.leftPZ_P.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.leftPZ_P.Image = ((System.Drawing.Image)(resources.GetObject("leftPZ_P.Image")));
            this.leftPZ_P.Location = new System.Drawing.Point(666, 551);
            this.leftPZ_P.Name = "leftPZ_P";
            this.leftPZ_P.Size = new System.Drawing.Size(70, 70);
            this.leftPZ_P.TabIndex = 66;
            this.leftPZ_P.UseVisualStyleBackColor = true;
            this.leftPZ_P.Click += new System.EventHandler(this.leftPZ_P_Click);
            // 
            // rightPZ_P
            // 
            this.rightPZ_P.AccessibleName = "LP_";
            this.rightPZ_P.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.rightPZ_P.Image = ((System.Drawing.Image)(resources.GetObject("rightPZ_P.Image")));
            this.rightPZ_P.Location = new System.Drawing.Point(590, 551);
            this.rightPZ_P.Name = "rightPZ_P";
            this.rightPZ_P.Size = new System.Drawing.Size(70, 70);
            this.rightPZ_P.TabIndex = 65;
            this.rightPZ_P.UseVisualStyleBackColor = true;
            this.rightPZ_P.Click += new System.EventHandler(this.rightPZ_P_Click);
            // 
            // rightLZ_R
            // 
            this.rightLZ_R.AccessibleName = "LP_";
            this.rightLZ_R.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.rightLZ_R.Image = ((System.Drawing.Image)(resources.GetObject("rightLZ_R.Image")));
            this.rightLZ_R.Location = new System.Drawing.Point(37, 627);
            this.rightLZ_R.Name = "rightLZ_R";
            this.rightLZ_R.Size = new System.Drawing.Size(70, 70);
            this.rightLZ_R.TabIndex = 64;
            this.rightLZ_R.UseVisualStyleBackColor = true;
            this.rightLZ_R.Click += new System.EventHandler(this.rightLZ_R_Click);
            // 
            // leftLZ_R
            // 
            this.leftLZ_R.AccessibleName = "LP_";
            this.leftLZ_R.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.leftLZ_R.Image = ((System.Drawing.Image)(resources.GetObject("leftLZ_R.Image")));
            this.leftLZ_R.Location = new System.Drawing.Point(113, 627);
            this.leftLZ_R.Name = "leftLZ_R";
            this.leftLZ_R.Size = new System.Drawing.Size(70, 70);
            this.leftLZ_R.TabIndex = 63;
            this.leftLZ_R.UseVisualStyleBackColor = true;
            this.leftLZ_R.Click += new System.EventHandler(this.leftLZ_R_Click);
            // 
            // leftLZ_P
            // 
            this.leftLZ_P.AccessibleName = "LP_";
            this.leftLZ_P.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.leftLZ_P.Image = ((System.Drawing.Image)(resources.GetObject("leftLZ_P.Image")));
            this.leftLZ_P.Location = new System.Drawing.Point(113, 551);
            this.leftLZ_P.Name = "leftLZ_P";
            this.leftLZ_P.Size = new System.Drawing.Size(70, 70);
            this.leftLZ_P.TabIndex = 62;
            this.leftLZ_P.UseVisualStyleBackColor = true;
            this.leftLZ_P.Click += new System.EventHandler(this.leftLZ_P_Click);
            // 
            // rightLZ_P
            // 
            this.rightLZ_P.AccessibleName = "LP_";
            this.rightLZ_P.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.rightLZ_P.Image = ((System.Drawing.Image)(resources.GetObject("rightLZ_P.Image")));
            this.rightLZ_P.Location = new System.Drawing.Point(37, 551);
            this.rightLZ_P.Name = "rightLZ_P";
            this.rightLZ_P.Size = new System.Drawing.Size(70, 70);
            this.rightLZ_P.TabIndex = 61;
            this.rightLZ_P.UseVisualStyleBackColor = true;
            this.rightLZ_P.Click += new System.EventHandler(this.rightLZ_P_Click);
            // 
            // rightPP_R
            // 
            this.rightPP_R.AccessibleName = "LP_";
            this.rightPP_R.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.rightPP_R.Image = ((System.Drawing.Image)(resources.GetObject("rightPP_R.Image")));
            this.rightPP_R.Location = new System.Drawing.Point(590, 192);
            this.rightPP_R.Name = "rightPP_R";
            this.rightPP_R.Size = new System.Drawing.Size(70, 70);
            this.rightPP_R.TabIndex = 60;
            this.rightPP_R.UseVisualStyleBackColor = true;
            this.rightPP_R.Click += new System.EventHandler(this.rightPP_R_Click);
            // 
            // leftPP_R
            // 
            this.leftPP_R.AccessibleName = "LP_";
            this.leftPP_R.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.leftPP_R.Image = ((System.Drawing.Image)(resources.GetObject("leftPP_R.Image")));
            this.leftPP_R.Location = new System.Drawing.Point(666, 192);
            this.leftPP_R.Name = "leftPP_R";
            this.leftPP_R.Size = new System.Drawing.Size(70, 70);
            this.leftPP_R.TabIndex = 59;
            this.leftPP_R.UseVisualStyleBackColor = true;
            this.leftPP_R.Click += new System.EventHandler(this.leftPP_R_Click);
            // 
            // leftPP_P
            // 
            this.leftPP_P.AccessibleName = "LP_";
            this.leftPP_P.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.leftPP_P.Image = ((System.Drawing.Image)(resources.GetObject("leftPP_P.Image")));
            this.leftPP_P.Location = new System.Drawing.Point(666, 116);
            this.leftPP_P.Name = "leftPP_P";
            this.leftPP_P.Size = new System.Drawing.Size(70, 70);
            this.leftPP_P.TabIndex = 58;
            this.leftPP_P.UseVisualStyleBackColor = true;
            this.leftPP_P.Click += new System.EventHandler(this.leftPP_P_Click);
            // 
            // rightPP_P
            // 
            this.rightPP_P.AccessibleName = "LP_";
            this.rightPP_P.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.rightPP_P.Image = ((System.Drawing.Image)(resources.GetObject("rightPP_P.Image")));
            this.rightPP_P.Location = new System.Drawing.Point(590, 116);
            this.rightPP_P.Name = "rightPP_P";
            this.rightPP_P.Size = new System.Drawing.Size(70, 70);
            this.rightPP_P.TabIndex = 57;
            this.rightPP_P.UseVisualStyleBackColor = true;
            this.rightPP_P.Click += new System.EventHandler(this.rightPP_P_Click);
            // 
            // rightLP_R
            // 
            this.rightLP_R.AccessibleName = "LP_";
            this.rightLP_R.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.rightLP_R.Image = ((System.Drawing.Image)(resources.GetObject("rightLP_R.Image")));
            this.rightLP_R.Location = new System.Drawing.Point(37, 192);
            this.rightLP_R.Name = "rightLP_R";
            this.rightLP_R.Size = new System.Drawing.Size(70, 70);
            this.rightLP_R.TabIndex = 56;
            this.rightLP_R.UseVisualStyleBackColor = true;
            this.rightLP_R.Click += new System.EventHandler(this.rightLP_R_Click);
            // 
            // leftLP_R
            // 
            this.leftLP_R.AccessibleName = "LP_";
            this.leftLP_R.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.leftLP_R.Image = ((System.Drawing.Image)(resources.GetObject("leftLP_R.Image")));
            this.leftLP_R.Location = new System.Drawing.Point(113, 192);
            this.leftLP_R.Name = "leftLP_R";
            this.leftLP_R.Size = new System.Drawing.Size(70, 70);
            this.leftLP_R.TabIndex = 55;
            this.leftLP_R.UseVisualStyleBackColor = true;
            this.leftLP_R.Click += new System.EventHandler(this.leftLP_R_Click);
            // 
            // leftLP_P
            // 
            this.leftLP_P.AccessibleName = "LP_";
            this.leftLP_P.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.leftLP_P.Image = ((System.Drawing.Image)(resources.GetObject("leftLP_P.Image")));
            this.leftLP_P.Location = new System.Drawing.Point(113, 116);
            this.leftLP_P.Name = "leftLP_P";
            this.leftLP_P.Size = new System.Drawing.Size(70, 70);
            this.leftLP_P.TabIndex = 54;
            this.leftLP_P.UseVisualStyleBackColor = true;
            this.leftLP_P.Click += new System.EventHandler(this.leftLP_P_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(32, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(155, 25);
            this.label2.TabIndex = 53;
            this.label2.Text = "Velikost kroku:";
            this.toolTip1.SetToolTip(this.label2, "Nastavení velikosti kroku, o který se budou otáčet motory při kliknutí na tlačítk" +
        "o.");
            // 
            // comboBoxStep
            // 
            this.comboBoxStep.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.comboBoxStep.Items.AddRange(new object[] {
            "nejmenší",
            "malý",
            "střední",
            "větší",
            "velký"});
            this.comboBoxStep.Location = new System.Drawing.Point(193, 52);
            this.comboBoxStep.Name = "comboBoxStep";
            this.comboBoxStep.Size = new System.Drawing.Size(121, 33);
            this.comboBoxStep.TabIndex = 52;
            this.comboBoxStep.Text = "střední";
            this.toolTip1.SetToolTip(this.comboBoxStep, "Nastavení velikosti kroku, o který se budou otáčet motory při kliknutí na tlačítk" +
        "o.");
            this.comboBoxStep.SelectedIndexChanged += new System.EventHandler(this.comboBoxStep_SelectedIndexChanged);
            // 
            // rightLP_P
            // 
            this.rightLP_P.AccessibleName = "LP_";
            this.rightLP_P.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.rightLP_P.Image = ((System.Drawing.Image)(resources.GetObject("rightLP_P.Image")));
            this.rightLP_P.Location = new System.Drawing.Point(37, 116);
            this.rightLP_P.Name = "rightLP_P";
            this.rightLP_P.Size = new System.Drawing.Size(70, 70);
            this.rightLP_P.TabIndex = 51;
            this.rightLP_P.UseVisualStyleBackColor = true;
            this.rightLP_P.Click += new System.EventHandler(this.rightLP_P_Click);
            // 
            // panelWithWarrning
            // 
            this.panelWithWarrning.Controls.Add(this.labelWarrning);
            this.panelWithWarrning.Controls.Add(this.labelWarningIcon);
            this.panelWithWarrning.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelWithWarrning.Location = new System.Drawing.Point(0, 995);
            this.panelWithWarrning.Name = "panelWithWarrning";
            this.panelWithWarrning.Size = new System.Drawing.Size(811, 56);
            this.panelWithWarrning.TabIndex = 87;
            // 
            // labelWarrning
            // 
            this.labelWarrning.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.labelWarrning.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelWarrning.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelWarrning.ForeColor = System.Drawing.Color.Red;
            this.labelWarrning.Location = new System.Drawing.Point(50, 0);
            this.labelWarrning.Name = "labelWarrning";
            this.labelWarrning.Size = new System.Drawing.Size(761, 56);
            this.labelWarrning.TabIndex = 53;
            this.labelWarrning.Text = "Při rekalibraci jsou vypnuta všechna omezení. \r\nDávejte pozor na dojezdy motorů d" +
    "o krajních poloh.";
            this.labelWarrning.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelWarrning.Visible = false;
            // 
            // labelWarningIcon
            // 
            this.labelWarningIcon.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.labelWarningIcon.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelWarningIcon.Image = ((System.Drawing.Image)(resources.GetObject("labelWarningIcon.Image")));
            this.labelWarningIcon.Location = new System.Drawing.Point(0, 0);
            this.labelWarningIcon.Name = "labelWarningIcon";
            this.labelWarningIcon.Size = new System.Drawing.Size(50, 56);
            this.labelWarningIcon.TabIndex = 52;
            this.labelWarningIcon.Visible = false;
            // 
            // AbsoluteControllView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.buttonJoystickPositioning);
            this.Controls.Add(this.panelWithWarrning);
            this.Controls.Add(this.label1);
            this.Name = "AbsoluteControllView";
            this.Size = new System.Drawing.Size(811, 1051);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panelWithWarrning.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonJoystickPositioning;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button rightPZ_ZK;
        private System.Windows.Forms.Button leftPZ_ZK;
        private System.Windows.Forms.Button downPZ_Z;
        private System.Windows.Forms.Button upPZ_Z;
        private System.Windows.Forms.Button rightLZ_ZK;
        private System.Windows.Forms.Button leftLZ_ZK;
        private System.Windows.Forms.Button downLZ_Z;
        private System.Windows.Forms.Button upLZ_Z;
        private System.Windows.Forms.Button rightPP_ZK;
        private System.Windows.Forms.Button leftPP_ZK;
        private System.Windows.Forms.Button downPP_Z;
        private System.Windows.Forms.Button upPP_Z;
        private System.Windows.Forms.Button rightLP_ZK;
        private System.Windows.Forms.Button leftLP_ZK;
        private System.Windows.Forms.Button downLP_Z;
        private System.Windows.Forms.Button upLP_Z;
        private System.Windows.Forms.Button rightPZ_R;
        private System.Windows.Forms.Button leftPZ_R;
        private System.Windows.Forms.Button leftPZ_P;
        private System.Windows.Forms.Button rightPZ_P;
        private System.Windows.Forms.Button rightLZ_R;
        private System.Windows.Forms.Button leftLZ_R;
        private System.Windows.Forms.Button leftLZ_P;
        private System.Windows.Forms.Button rightLZ_P;
        private System.Windows.Forms.Button rightPP_R;
        private System.Windows.Forms.Button leftPP_R;
        private System.Windows.Forms.Button leftPP_P;
        private System.Windows.Forms.Button rightPP_P;
        private System.Windows.Forms.Button rightLP_R;
        private System.Windows.Forms.Button leftLP_R;
        private System.Windows.Forms.Button leftLP_P;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxStep;
        private System.Windows.Forms.Button rightLP_P;
        private System.Windows.Forms.Button buttonSetDefaultPosition;
        private System.Windows.Forms.Label labelWarningIcon;
        private System.Windows.Forms.Panel panelWithWarrning;
        private System.Windows.Forms.Label labelWarrning;
        private System.Windows.Forms.Button buttonCalibr;
        private System.Windows.Forms.Button buttonCancelRecalibration;
        private System.Windows.Forms.CheckBox checkBoxLimitProtection;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
