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
            lblUmbrales = new Label();
            btnConectar = new Button();
            grbDatosSensores = new GroupBox();
            txtHumSuelo = new TextBox();
            txtLuminosidad = new TextBox();
            txtHumAmb = new TextBox();
            txtTemperatura = new TextBox();
            lblHumSuelo = new Label();
            lblLuminosidad = new Label();
            lblHumAmb = new Label();
            lblTemperatura = new Label();
            btnTemp = new Button();
            btnHumAmb = new Button();
            btnLuminosidad = new Button();
            btnHumSuelo = new Button();
            grbMonitor.SuspendLayout();
            grbUmbrales.SuspendLayout();
            grbDatosSensores.SuspendLayout();
            SuspendLayout();
            // 
            // grbMonitor
            // 
            resources.ApplyResources(grbMonitor, "grbMonitor");
            grbMonitor.Controls.Add(grbUmbrales);
            grbMonitor.Controls.Add(btnConectar);
            grbMonitor.Controls.Add(grbDatosSensores);
            grbMonitor.Name = "grbMonitor";
            grbMonitor.TabStop = false;
            // 
            // grbUmbrales
            // 
            resources.ApplyResources(grbUmbrales, "grbUmbrales");
            grbUmbrales.Controls.Add(lblUmbrales);
            grbUmbrales.Name = "grbUmbrales";
            grbUmbrales.TabStop = false;
            // 
            // lblUmbrales
            // 
            resources.ApplyResources(lblUmbrales, "lblUmbrales");
            lblUmbrales.Name = "lblUmbrales";
            // 
            // btnConectar
            // 
            resources.ApplyResources(btnConectar, "btnConectar");
            btnConectar.Name = "btnConectar";
            btnConectar.UseVisualStyleBackColor = true;
            btnConectar.Click += btnConectar_Click;
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
            // txtHumSuelo
            // 
            resources.ApplyResources(txtHumSuelo, "txtHumSuelo");
            txtHumSuelo.Name = "txtHumSuelo";
            // 
            // txtLuminosidad
            // 
            resources.ApplyResources(txtLuminosidad, "txtLuminosidad");
            txtLuminosidad.Name = "txtLuminosidad";
            // 
            // txtHumAmb
            // 
            resources.ApplyResources(txtHumAmb, "txtHumAmb");
            txtHumAmb.Name = "txtHumAmb";
            // 
            // txtTemperatura
            // 
            resources.ApplyResources(txtTemperatura, "txtTemperatura");
            txtTemperatura.Name = "txtTemperatura";
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
            // btnTemp
            // 
            resources.ApplyResources(btnTemp, "btnTemp");
            btnTemp.Name = "btnTemp";
            btnTemp.UseVisualStyleBackColor = true;
            // 
            // btnHumAmb
            // 
            resources.ApplyResources(btnHumAmb, "btnHumAmb");
            btnHumAmb.Name = "btnHumAmb";
            btnHumAmb.UseVisualStyleBackColor = true;
            // 
            // btnLuminosidad
            // 
            resources.ApplyResources(btnLuminosidad, "btnLuminosidad");
            btnLuminosidad.Name = "btnLuminosidad";
            btnLuminosidad.UseVisualStyleBackColor = true;
            // 
            // btnHumSuelo
            // 
            resources.ApplyResources(btnHumSuelo, "btnHumSuelo");
            btnHumSuelo.Name = "btnHumSuelo";
            btnHumSuelo.UseVisualStyleBackColor = true;
            // 
            // frmMain
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(grbMonitor);
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
        private Button btnConectar;
        private GroupBox grbUmbrales;
        private Label lblUmbrales;
        private Button btnHumSuelo;
        private Button btnLuminosidad;
        private Button btnHumAmb;
        private Button btnTemp;
    }
}
