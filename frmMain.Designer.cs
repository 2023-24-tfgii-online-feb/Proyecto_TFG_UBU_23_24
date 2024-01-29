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
            grbDatosSensores = new GroupBox();
            this.grbUmbrales = new GroupBox();
            lblUmbrales = new Label();
            lblTemperatura = new Label();
            lblHumAmb = new Label();
            lblLuminosidad = new Label();
            lblHumSuelo = new Label();
            txtTemperatura = new TextBox();
            txtHumAmb = new TextBox();
            txtLuminosidad = new TextBox();
            txtHumSuelo = new TextBox();
            btnConectar = new Button();
            grbMonitor.SuspendLayout();
            grbDatosSensores.SuspendLayout();
            this.grbUmbrales.SuspendLayout();
            SuspendLayout();
            // 
            // grbMonitor
            // 
            resources.ApplyResources(grbMonitor, "grbMonitor");
            grbMonitor.Controls.Add(btnConectar);
            grbMonitor.Controls.Add(this.grbUmbrales);
            grbMonitor.Controls.Add(grbDatosSensores);
            grbMonitor.Name = "grbMonitor";
            grbMonitor.TabStop = false;
            // 
            // grbDatosSensores
            // 
            resources.ApplyResources(grbDatosSensores, "grbDatosSensores");
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
            // grbUmbrales
            // 
            resources.ApplyResources(this.grbUmbrales, "grbUmbrales");
            this.grbUmbrales.Controls.Add(lblUmbrales);
            this.grbUmbrales.Name = "grbUmbrales";
            this.grbUmbrales.TabStop = false;
            // 
            // lblUmbrales
            // 
            resources.ApplyResources(lblUmbrales, "lblUmbrales");
            lblUmbrales.Name = "lblUmbrales";
            // 
            // lblTemperatura
            // 
            resources.ApplyResources(lblTemperatura, "lblTemperatura");
            lblTemperatura.Name = "lblTemperatura";
            // 
            // lblHumAmb
            // 
            resources.ApplyResources(lblHumAmb, "lblHumAmb");
            lblHumAmb.Name = "lblHumAmb";
            // 
            // lblLuminosidad
            // 
            resources.ApplyResources(lblLuminosidad, "lblLuminosidad");
            lblLuminosidad.Name = "lblLuminosidad";
            // 
            // lblHumSuelo
            // 
            resources.ApplyResources(lblHumSuelo, "lblHumSuelo");
            lblHumSuelo.Name = "lblHumSuelo";
            // 
            // txtTemperatura
            // 
            resources.ApplyResources(txtTemperatura, "txtTemperatura");
            txtTemperatura.Name = "txtTemperatura";
            // 
            // txtHumAmb
            // 
            resources.ApplyResources(txtHumAmb, "txtHumAmb");
            txtHumAmb.Name = "txtHumAmb";
            // 
            // txtLuminosidad
            // 
            resources.ApplyResources(txtLuminosidad, "txtLuminosidad");
            txtLuminosidad.Name = "txtLuminosidad";
            // 
            // txtHumSuelo
            // 
            resources.ApplyResources(txtHumSuelo, "txtHumSuelo");
            txtHumSuelo.Name = "txtHumSuelo";
            // 
            // btnConectar
            // 
            resources.ApplyResources(btnConectar, "btnConectar");
            btnConectar.Name = "btnConectar";
            btnConectar.UseVisualStyleBackColor = true;
            btnConectar.Click += btnConectar_Click;
            // 
            // frmMain
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(grbMonitor);
            Name = "frmMain";
            Load += frmMain_Load;
            grbMonitor.ResumeLayout(false);
            grbDatosSensores.ResumeLayout(false);
            grbDatosSensores.PerformLayout();
            this.grbUmbrales.ResumeLayout(false);
            this.grbUmbrales.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox grbMonitor;
        private GroupBox groupBox2;
        private GroupBox grbDatosSensores;
        private Label lblUmbrales;
        private Label lblHumSuelo;
        private Label lblLuminosidad;
        private Label lblHumAmb;
        private Label lblTemperatura;
        private TextBox txtHumSuelo;
        private TextBox txtLuminosidad;
        private TextBox txtHumAmb;
        private TextBox txtTemperatura;
        private Button btnConectar;
    }
}
