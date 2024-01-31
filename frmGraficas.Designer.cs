namespace InverIoT
{
    partial class frmGraficas
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGraficas));
            chartSensores = new System.Windows.Forms.DataVisualization.Charting.Chart();
            grbFiltro = new GroupBox();
            btnFiltrar = new Button();
            lblFFin = new Label();
            dtpFin = new DateTimePicker();
            lblFInicio = new Label();
            dtpInicio = new DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)chartSensores).BeginInit();
            grbFiltro.SuspendLayout();
            SuspendLayout();
            // 
            // chartSensores
            // 
            chartSensores.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            chartArea1.Name = "ChartArea1";
            chartSensores.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            chartSensores.Legends.Add(legend1);
            chartSensores.Location = new Point(7, 120);
            chartSensores.Margin = new Padding(0);
            chartSensores.Name = "chartSensores";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            chartSensores.Series.Add(series1);
            chartSensores.Size = new Size(930, 480);
            chartSensores.TabIndex = 0;
            chartSensores.Text = "Gráfica";
            // 
            // grbFiltro
            // 
            grbFiltro.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grbFiltro.Controls.Add(btnFiltrar);
            grbFiltro.Controls.Add(lblFFin);
            grbFiltro.Controls.Add(dtpFin);
            grbFiltro.Controls.Add(lblFInicio);
            grbFiltro.Controls.Add(dtpInicio);
            grbFiltro.Location = new Point(12, 12);
            grbFiltro.Name = "grbFiltro";
            grbFiltro.Size = new Size(920, 102);
            grbFiltro.TabIndex = 3;
            grbFiltro.TabStop = false;
            grbFiltro.Text = " Filtrar ";
            // 
            // btnFiltrar
            // 
            btnFiltrar.Anchor = AnchorStyles.Top;
            btnFiltrar.Location = new Point(664, 54);
            btnFiltrar.Name = "btnFiltrar";
            btnFiltrar.Size = new Size(250, 34);
            btnFiltrar.TabIndex = 6;
            btnFiltrar.Text = "Filtrar";
            btnFiltrar.UseVisualStyleBackColor = true;
            btnFiltrar.Click += btnFiltrar_Click;
            // 
            // lblFFin
            // 
            lblFFin.Anchor = AnchorStyles.Top;
            lblFFin.AutoSize = true;
            lblFFin.Location = new Point(350, 31);
            lblFFin.Name = "lblFFin";
            lblFFin.Size = new Size(85, 23);
            lblFFin.TabIndex = 5;
            lblFFin.Text = "Fecha Fin:";
            // 
            // dtpFin
            // 
            dtpFin.Anchor = AnchorStyles.Top;
            dtpFin.Location = new Point(350, 58);
            dtpFin.Name = "dtpFin";
            dtpFin.Size = new Size(308, 30);
            dtpFin.TabIndex = 4;
            // 
            // lblFInicio
            // 
            lblFInicio.Anchor = AnchorStyles.Top;
            lblFInicio.AutoSize = true;
            lblFInicio.Location = new Point(6, 31);
            lblFInicio.Name = "lblFInicio";
            lblFInicio.Size = new Size(104, 23);
            lblFInicio.TabIndex = 3;
            lblFInicio.Text = "Fecha Inicio:";
            // 
            // dtpInicio
            // 
            dtpInicio.Anchor = AnchorStyles.Top;
            dtpInicio.Location = new Point(6, 57);
            dtpInicio.Name = "dtpInicio";
            dtpInicio.Size = new Size(308, 30);
            dtpInicio.TabIndex = 2;
            // 
            // frmGraficas
            // 
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(944, 605);
            Controls.Add(grbFiltro);
            Controls.Add(chartSensores);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "frmGraficas";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Inver IoT - Gráficas.";
            ((System.ComponentModel.ISupportInitialize)chartSensores).EndInit();
            grbFiltro.ResumeLayout(false);
            grbFiltro.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chartSensores;
        private GroupBox grbFiltro;
        private Label lblFFin;
        private DateTimePicker dtpFin;
        private Label lblFInicio;
        private DateTimePicker dtpInicio;
        private Button btnFiltrar;
    }
}