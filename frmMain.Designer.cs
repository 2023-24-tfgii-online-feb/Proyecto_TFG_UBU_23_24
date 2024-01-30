namespace InverIoT
{
    partial class frmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            grbMonitor = new GroupBox();
            grbUmbrales = new GroupBox();
            txtUmbrales = new TextBox();
            grbDatosSensores = new GroupBox();
            btnHumSuelo = new Button();
            btnLuminosidad = new Button();
            btnHumAmb = new Button();
            btnTemp = new Button();
            txtHumSuelo = new TextBox();
            txtLuminosidad = new TextBox();
            txtHumAmb = new TextBox();
            txtTemperatura = new TextBox();
            lblHumSuelo = new Label();
            lblLuminosidad = new Label();
            lblHumAmb = new Label();
            lblTemperatura = new Label();
            grbMonitor.SuspendLayout();
            grbUmbrales.SuspendLayout();
            grbDatosSensores.SuspendLayout();
            SuspendLayout();
            // 
            // grbMonitor
            // 
            resources.ApplyResources(grbMonitor, "grbMonitor");
            grbMonitor.Controls.Add(grbUmbrales);
            grbMonitor.Controls.Add(grbDatosSensores);
            grbMonitor.Name = "grbMonitor";
            grbMonitor.TabStop = false;
            // 
            // grbUmbrales
            // 
            resources.ApplyResources(grbUmbrales, "grbUmbrales");
            grbUmbrales.Controls.Add(txtUmbrales);
            grbUmbrales.Name = "grbUmbrales";
            grbUmbrales.TabStop = false;
            // 
            // txtUmbrales
            // 
            resources.ApplyResources(txtUmbrales, "txtUmbrales");
            txtUmbrales.BorderStyle = BorderStyle.FixedSingle;
            txtUmbrales.Name = "txtUmbrales";
            txtUmbrales.ReadOnly = true;
            // 
            // grbDatosSensores
            // 
            resources.ApplyResources(grbDatosSensores, "grbDatosSensores");
            grbDatosSensores.Controls.Add(btnHumSuelo);
            grbDatosSensores.Controls.Add(btnLuminosidad);
            grbDatosSensores.Controls.Add(btnHumAmb);
            grbDatosSensores.Controls.Add(btnTemp);
            grbDatosSensores.Controls.Add(txtHumSuelo);
            grbDatosSensores.Controls.Add(txtLuminosidad);
            grbDatosSensores.Controls.Add(txtHumAmb);
            grbDatosSensores.Controls.Add(txtTemperatura);
            grbDatosSensores.Controls.Add(lblHumSuelo);
            grbDatosSensores.Controls.Add(lblLuminosidad);
            grbDatosSensores.Controls.Add(lblHumAmb);
            grbDatosSensores.Controls.Add(lblTemperatura);
            grbDatosSensores.Name = "grbDatosSensores";
            grbDatosSensores.TabStop = false;
            // 
            // btnHumSuelo
            // 
            resources.ApplyResources(btnHumSuelo, "btnHumSuelo");
            btnHumSuelo.Name = "btnHumSuelo";
            btnHumSuelo.UseVisualStyleBackColor = true;
            btnHumSuelo.Click += btnHumSuelo_Click;
            // 
            // btnLuminosidad
            // 
            resources.ApplyResources(btnLuminosidad, "btnLuminosidad");
            btnLuminosidad.Name = "btnLuminosidad";
            btnLuminosidad.UseVisualStyleBackColor = true;
            btnLuminosidad.Click += btnLuminosidad_Click;
            // 
            // btnHumAmb
            // 
            resources.ApplyResources(btnHumAmb, "btnHumAmb");
            btnHumAmb.Name = "btnHumAmb";
            btnHumAmb.UseVisualStyleBackColor = true;
            btnHumAmb.Click += btnHumAmb_Click;
            // 
            // btnTemp
            // 
            resources.ApplyResources(btnTemp, "btnTemp");
            btnTemp.Name = "btnTemp";
            btnTemp.UseVisualStyleBackColor = true;
            btnTemp.Click += btnTemp_Click;
            // 
            // txtHumSuelo
            // 
            resources.ApplyResources(txtHumSuelo, "txtHumSuelo");
            txtHumSuelo.BackColor = SystemColors.ButtonFace;
            txtHumSuelo.BorderStyle = BorderStyle.FixedSingle;
            txtHumSuelo.Name = "txtHumSuelo";
            txtHumSuelo.ReadOnly = true;
            // 
            // txtLuminosidad
            // 
            resources.ApplyResources(txtLuminosidad, "txtLuminosidad");
            txtLuminosidad.BackColor = SystemColors.ButtonFace;
            txtLuminosidad.BorderStyle = BorderStyle.FixedSingle;
            txtLuminosidad.Name = "txtLuminosidad";
            txtLuminosidad.ReadOnly = true;
            // 
            // txtHumAmb
            // 
            resources.ApplyResources(txtHumAmb, "txtHumAmb");
            txtHumAmb.BackColor = SystemColors.ButtonFace;
            txtHumAmb.BorderStyle = BorderStyle.FixedSingle;
            txtHumAmb.Name = "txtHumAmb";
            txtHumAmb.ReadOnly = true;
            // 
            // txtTemperatura
            // 
            resources.ApplyResources(txtTemperatura, "txtTemperatura");
            txtTemperatura.BackColor = SystemColors.ButtonFace;
            txtTemperatura.BorderStyle = BorderStyle.FixedSingle;
            txtTemperatura.Name = "txtTemperatura";
            txtTemperatura.ReadOnly = true;
            // 
            // lblHumSuelo
            // 
            resources.ApplyResources(lblHumSuelo, "lblHumSuelo");
            lblHumSuelo.Name = "lblHumSuelo";
            // 
            // lblLuminosidad
            // 
            resources.ApplyResources(lblLuminosidad, "lblLuminosidad");
            lblLuminosidad.Name = "lblLuminosidad";
            // 
            // lblHumAmb
            // 
            resources.ApplyResources(lblHumAmb, "lblHumAmb");
            lblHumAmb.Name = "lblHumAmb";
            // 
            // lblTemperatura
            // 
            resources.ApplyResources(lblTemperatura, "lblTemperatura");
            lblTemperatura.Name = "lblTemperatura";
            // 
            // frmMain
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(grbMonitor);
            MaximizeBox = false;
            Name = "frmMain";
            grbMonitor.ResumeLayout(false);
            grbUmbrales.ResumeLayout(false);
            grbUmbrales.PerformLayout();
            grbDatosSensores.ResumeLayout(false);
            grbDatosSensores.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox grbMonitor;
        private GroupBox groupBox2;
        private GroupBox grbDatosSensores;
        private Label lblHumSuelo;
        private Label lblLuminosidad;
        private Label lblHumAmb;
        private Label lblTemperatura;
        private TextBox txtHumSuelo;
        private TextBox txtLuminosidad;
        private TextBox txtHumAmb;
        private TextBox txtTemperatura;
        private GroupBox grbUmbrales;
        private Button btnHumSuelo;
        private Button btnLuminosidad;
        private Button btnHumAmb;
        private Button btnTemp;
        private TextBox txtUmbrales;
    }
}
