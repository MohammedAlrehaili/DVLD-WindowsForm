namespace PresentationLayer
{
    partial class frmVisionScheduleTest
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
            this.ucScheduleTest1 = new PresentationLayer.ucScheduleTest();
            this.SuspendLayout();
            // 
            // ucScheduleTest1
            // 
            this.ucScheduleTest1.App = null;
            this.ucScheduleTest1.LDLApp = null;
            this.ucScheduleTest1.Location = new System.Drawing.Point(12, 12);
            this.ucScheduleTest1.Name = "ucScheduleTest1";
            this.ucScheduleTest1.Size = new System.Drawing.Size(592, 744);
            this.ucScheduleTest1.TabIndex = 0;
            // 
            // frmVisionScheduleTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(605, 792);
            this.Controls.Add(this.ucScheduleTest1);
            this.Name = "frmVisionScheduleTest";
            this.Text = "frmScheduleVisionTest";
            this.Load += new System.EventHandler(this.frmVisionScheduleTest_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ucScheduleTest ucScheduleTest1;
    }
}