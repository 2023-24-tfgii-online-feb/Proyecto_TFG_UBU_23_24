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
            grbActUmbrales = new GroupBox();
            txtHumSueloMax = new TextBox();
            txtLuminosidadMax = new TextBox();
            txtHumAmbMax = new TextBox();
            txtTemperaturaMax = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            groupBox1 = new GroupBox();
            btnActualizaUmb = new Button();
            txtTextoActUmbrales = new TextBox();
            txtHumSueloMin = new TextBox();
            txtLuminosidadMin = new TextBox();
            txtHumAmbMin = new TextBox();
            txtTemperaturaMin = new TextBox();
            lblUHs = new Label();
            lblULum = new Label();
            lblUHa = new Label();
            lblUTemp = new Label();
            grbUmbrales = new GroupBox();
            btnAutor = new Button();
            btnGraficas = new Button();
            btnHistorico = new Button();
            txtUmbralesI = new TextBox();
            grbDatosSensores = new GroupBox();
            btnHumSueloOff = new Button();
            btnLuminosidadOff = new Button();
            btnHumAmbOff = new Button();
            btnTempOff = new Button();
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
            grbActUmbrales.SuspendLayout();
            groupBox1.SuspendLayout();
            grbUmbrales.SuspendLayout();
            grbDatosSensores.SuspendLayout();
            SuspendLayout();
            // 
            // grbMonitor
            // 
            resources.ApplyResources(grbMonitor, "grbMonitor");
            grbMonitor.Controls.Add(grbActUmbrales);
            grbMonitor.Controls.Add(grbUmbrales);
            grbMonitor.Controls.Add(grbDatosSensores);
            grbMonitor.Name = "grbMonitor";
            grbMonitor.TabStop = false;
            // 
            // grbActUmbrales
            // 
            resources.ApplyResources(grbActUmbrales, "grbActUmbrales");
            grbActUmbrales.Controls.Add(txtHumSueloMax);
            grbActUmbrales.Controls.Add(txtLuminosidadMax);
            grbActUmbrales.Controls.Add(txtHumAmbMax);
            grbActUmbrales.Controls.Add(txtTemperaturaMax);
            grbActUmbrales.Controls.Add(label1);
            grbActUmbrales.Controls.Add(label2);
            grbActUmbrales.Controls.Add(label3);
            grbActUmbrales.Controls.Add(label4);
            grbActUmbrales.Controls.Add(groupBox1);
            grbActUmbrales.Controls.Add(txtHumSueloMin);
            grbActUmbrales.Controls.Add(txtLuminosidadMin);
            grbActUmbrales.Controls.Add(txtHumAmbMin);
            grbActUmbrales.Controls.Add(txtTemperaturaMin);
            grbActUmbrales.Controls.Add(lblUHs);
            grbActUmbrales.Controls.Add(lblULum);
            grbActUmbrales.Controls.Add(lblUHa);
            grbActUmbrales.Controls.Add(lblUTemp);
            grbActUmbrales.Name = "grbActUmbrales";
            grbActUmbrales.TabStop = false;
            // 
            // txtHumSueloMax
            // 
            resources.ApplyResources(txtHumSueloMax, "txtHumSueloMax");
            txtHumSueloMax.Name = "txtHumSueloMax";
            txtHumSueloMax.KeyPress += txtHumSueloMax_KeyPress;
            // 
            // txtLuminosidadMax
            // 
            resources.ApplyResources(txtLuminosidadMax, "txtLuminosidadMax");
            txtLuminosidadMax.Name = "txtLuminosidadMax";
            txtLuminosidadMax.KeyPress += txtLuminosidadMax_KeyPress;
            // 
            // txtHumAmbMax
            // 
            resources.ApplyResources(txtHumAmbMax, "txtHumAmbMax");
            txtHumAmbMax.Name = "txtHumAmbMax";
            txtHumAmbMax.KeyPress += txtHumAmbMax_KeyPress;
            // 
            // txtTemperaturaMax
            // 
            resources.ApplyResources(txtTemperaturaMax, "txtTemperaturaMax");
            txtTemperaturaMax.Name = "txtTemperaturaMax";
            txtTemperaturaMax.KeyPress += txtTemperaturaMax_KeyPress;
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(label4, "label4");
            label4.Name = "label4";
            // 
            // groupBox1
            // 
            resources.ApplyResources(groupBox1, "groupBox1");
            groupBox1.Controls.Add(btnActualizaUmb);
            groupBox1.Controls.Add(txtTextoActUmbrales);
            groupBox1.Name = "groupBox1";
            groupBox1.TabStop = false;
            // 
            // btnActualizaUmb
            // 
            resources.ApplyResources(btnActualizaUmb, "btnActualizaUmb");
            btnActualizaUmb.Name = "btnActualizaUmb";
            btnActualizaUmb.UseVisualStyleBackColor = true;
            btnActualizaUmb.Click += btnActualizaUmb_Click;
            // 
            // txtTextoActUmbrales
            // 
            resources.ApplyResources(txtTextoActUmbrales, "txtTextoActUmbrales");
            txtTextoActUmbrales.BorderStyle = BorderStyle.FixedSingle;
            txtTextoActUmbrales.Name = "txtTextoActUmbrales";
            txtTextoActUmbrales.ReadOnly = true;
            // 
            // txtHumSueloMin
            // 
            resources.ApplyResources(txtHumSueloMin, "txtHumSueloMin");
            txtHumSueloMin.Name = "txtHumSueloMin";
            txtHumSueloMin.KeyPress += txtHumSueloMin_KeyPress;
            // 
            // txtLuminosidadMin
            // 
            resources.ApplyResources(txtLuminosidadMin, "txtLuminosidadMin");
            txtLuminosidadMin.Name = "txtLuminosidadMin";
            txtLuminosidadMin.KeyPress += txtLuminosidadMin_KeyPress;
            // 
            // txtHumAmbMin
            // 
            resources.ApplyResources(txtHumAmbMin, "txtHumAmbMin");
            txtHumAmbMin.Name = "txtHumAmbMin";
            txtHumAmbMin.KeyPress += txtHumAmbMin_KeyPress;
            // 
            // txtTemperaturaMin
            // 
            resources.ApplyResources(txtTemperaturaMin, "txtTemperaturaMin");
            txtTemperaturaMin.Name = "txtTemperaturaMin";
            txtTemperaturaMin.KeyPress += txtTemperaturaMin_KeyPress;
            // 
            // lblUHs
            // 
            resources.ApplyResources(lblUHs, "lblUHs");
            lblUHs.Name = "lblUHs";
            // 
            // lblULum
            // 
            resources.ApplyResources(lblULum, "lblULum");
            lblULum.Name = "lblULum";
            // 
            // lblUHa
            // 
            resources.ApplyResources(lblUHa, "lblUHa");
            lblUHa.Name = "lblUHa";
            // 
            // lblUTemp
            // 
            resources.ApplyResources(lblUTemp, "lblUTemp");
            lblUTemp.Name = "lblUTemp";
            // 
            // grbUmbrales
            // 
            resources.ApplyResources(grbUmbrales, "grbUmbrales");
            grbUmbrales.Controls.Add(btnAutor);
            grbUmbrales.Controls.Add(btnGraficas);
            grbUmbrales.Controls.Add(btnHistorico);
            grbUmbrales.Controls.Add(txtUmbralesI);
            grbUmbrales.Name = "grbUmbrales";
            grbUmbrales.TabStop = false;
            // 
            // btnAutor
            // 
            resources.ApplyResources(btnAutor, "btnAutor");
            btnAutor.BackColor = SystemColors.InactiveCaption;
            btnAutor.Name = "btnAutor";
            btnAutor.UseVisualStyleBackColor = false;
            btnAutor.Click += btnAutor_Click;
            // 
            // btnGraficas
            // 
            resources.ApplyResources(btnGraficas, "btnGraficas");
            btnGraficas.Name = "btnGraficas";
            btnGraficas.UseVisualStyleBackColor = true;
            btnGraficas.Click += btnGraficas_Click;
            // 
            // btnHistorico
            // 
            resources.ApplyResources(btnHistorico, "btnHistorico");
            btnHistorico.Name = "btnHistorico";
            btnHistorico.UseVisualStyleBackColor = true;
            btnHistorico.Click += btnHistorico_Click;
            // 
            // txtUmbralesI
            // 
            resources.ApplyResources(txtUmbralesI, "txtUmbralesI");
            txtUmbralesI.BorderStyle = BorderStyle.FixedSingle;
            txtUmbralesI.Name = "txtUmbralesI";
            txtUmbralesI.ReadOnly = true;
            // 
            // grbDatosSensores
            // 
            resources.ApplyResources(grbDatosSensores, "grbDatosSensores");
            grbDatosSensores.Controls.Add(btnHumSueloOff);
            grbDatosSensores.Controls.Add(btnLuminosidadOff);
            grbDatosSensores.Controls.Add(btnHumAmbOff);
            grbDatosSensores.Controls.Add(btnTempOff);
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
            // btnHumSueloOff
            // 
            resources.ApplyResources(btnHumSueloOff, "btnHumSueloOff");
            btnHumSueloOff.Name = "btnHumSueloOff";
            btnHumSueloOff.UseVisualStyleBackColor = true;
            btnHumSueloOff.Click += btnHumSueloOff_Click;
            // 
            // btnLuminosidadOff
            // 
            resources.ApplyResources(btnLuminosidadOff, "btnLuminosidadOff");
            btnLuminosidadOff.Name = "btnLuminosidadOff";
            btnLuminosidadOff.UseVisualStyleBackColor = true;
            btnLuminosidadOff.Click += btnLuminosidadOff_Click;
            // 
            // btnHumAmbOff
            // 
            resources.ApplyResources(btnHumAmbOff, "btnHumAmbOff");
            btnHumAmbOff.Name = "btnHumAmbOff";
            btnHumAmbOff.UseVisualStyleBackColor = true;
            btnHumAmbOff.Click += btnHumAmbOff_Click;
            // 
            // btnTempOff
            // 
            resources.ApplyResources(btnTempOff, "btnTempOff");
            btnTempOff.Name = "btnTempOff";
            btnTempOff.UseVisualStyleBackColor = true;
            btnTempOff.Click += btnTempOff_Click;
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
            txtHumSuelo.TextChanged += txtHumSuelo_TextChanged;
            // 
            // txtLuminosidad
            // 
            resources.ApplyResources(txtLuminosidad, "txtLuminosidad");
            txtLuminosidad.BackColor = SystemColors.ButtonFace;
            txtLuminosidad.BorderStyle = BorderStyle.FixedSingle;
            txtLuminosidad.Name = "txtLuminosidad";
            txtLuminosidad.TextChanged += txtLuminosidad_TextChanged;
            // 
            // txtHumAmb
            // 
            resources.ApplyResources(txtHumAmb, "txtHumAmb");
            txtHumAmb.BackColor = SystemColors.ButtonFace;
            txtHumAmb.BorderStyle = BorderStyle.FixedSingle;
            txtHumAmb.Name = "txtHumAmb";
            txtHumAmb.TextChanged += txtHumAmb_TextChanged;
            // 
            // txtTemperatura
            // 
            resources.ApplyResources(txtTemperatura, "txtTemperatura");
            txtTemperatura.BackColor = SystemColors.ButtonFace;
            txtTemperatura.BorderStyle = BorderStyle.FixedSingle;
            txtTemperatura.Name = "txtTemperatura";
            txtTemperatura.TextChanged += txtTemperatura_TextChanged;
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
            grbActUmbrales.ResumeLayout(false);
            grbActUmbrales.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
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
        private TextBox txtUmbralesI;
        private GroupBox grbActUmbrales;
        private Label lblUHs;
        private Label lblULum;
        private Label lblUHa;
        private Label lblUTemp;
        private GroupBox groupBox1;
        private Button btnActualizaUmb;
        private TextBox txtTextoActUmbrales;
        private TextBox txtHumSueloMin;
        private TextBox txtLuminosidadMin;
        private TextBox txtHumAmbMin;
        private TextBox txtTemperaturaMin;
        private TextBox txtHumSueloMax;
        private TextBox txtLuminosidadMax;
        private TextBox txtHumAmbMax;
        private TextBox txtTemperaturaMax;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Button btnTempOff;
        private Button btnHumSueloOff;
        private Button btnLuminosidadOff;
        private Button btnHumAmbOff;
        private Button btnHistorico;
        private Button btnGraficas;
        private Button btnAutor;
    }
}
