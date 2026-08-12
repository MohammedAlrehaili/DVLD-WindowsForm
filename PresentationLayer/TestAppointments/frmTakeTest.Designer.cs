namespace PresentationLayer.TestAppointments
{
    partial class frmTakeTest
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ucTakeTest1 = new PresentationLayer.Controls.ucTakeTest();
            this.SuspendLayout();
            // 
            // ucTakeTest1
            // 
            this.ucTakeTest1.Location = new System.Drawing.Point(12, 23);
            this.ucTakeTest1.Name = "ucTakeTest1";
            this.ucTakeTest1.Size = new System.Drawing.Size(661, 787);
            this.ucTakeTest1.TabIndex = 0;
            // 
            // frmTakeTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(679, 803);
            this.Controls.Add(this.ucTakeTest1);
            this.Name = "frmTakeTest";
            this.Text = "frmTakeTest";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.ucTakeTest ucTakeTest1;
    }
}