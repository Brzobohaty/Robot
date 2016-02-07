namespace Robot
{
    partial class DiagnosticView
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
            this.errorLabelMotors = new System.Windows.Forms.Label();
            this.labelDiagnostic = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // errorLabelMotors
            // 
            this.errorLabelMotors.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.errorLabelMotors.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.errorLabelMotors.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.errorLabelMotors.ForeColor = System.Drawing.Color.Red;
            this.errorLabelMotors.Location = new System.Drawing.Point(0, 515);
            this.errorLabelMotors.Name = "errorLabelMotors";
            this.errorLabelMotors.Size = new System.Drawing.Size(412, 55);
            this.errorLabelMotors.TabIndex = 3;
            this.errorLabelMotors.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelDiagnostic
            // 
            this.labelDiagnostic.BackColor = System.Drawing.SystemColors.GrayText;
            this.labelDiagnostic.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDiagnostic.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelDiagnostic.Location = new System.Drawing.Point(0, 0);
            this.labelDiagnostic.Name = "labelDiagnostic";
            this.labelDiagnostic.Size = new System.Drawing.Size(412, 55);
            this.labelDiagnostic.TabIndex = 2;
            this.labelDiagnostic.Text = "Diagnostika";
            this.labelDiagnostic.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DiagnosticView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.errorLabelMotors);
            this.Controls.Add(this.labelDiagnostic);
            this.Name = "DiagnosticView";
            this.Size = new System.Drawing.Size(412, 570);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label errorLabelMotors;
        private System.Windows.Forms.Label labelDiagnostic;
    }
}
