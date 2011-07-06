namespace ModelosTP
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.bComenzar = new System.Windows.Forms.Button();
            this.bDetener = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rTotalClientes = new System.Windows.Forms.Label();
            this.rTiempoTramiteCliente = new System.Windows.Forms.Label();
            this.rTiempoAcumuladoOcioso = new System.Windows.Forms.Label();
            this.rTiempoEsperaMaximoCajas = new System.Windows.Forms.Label();
            this.rTiempoEsperaPromedioCajas = new System.Windows.Forms.Label();
            this.rColaMaximaTerminales = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cantidadTerminales = new System.Windows.Forms.NumericUpDown();
            this.cantidadCajas = new System.Windows.Forms.NumericUpDown();
            this.horasSimulacion = new System.Windows.Forms.NumericUpDown();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cantidadTerminales)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cantidadCajas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.horasSimulacion)).BeginInit();
            this.SuspendLayout();
            // 
            // bComenzar
            // 
            this.bComenzar.Location = new System.Drawing.Point(398, 166);
            this.bComenzar.Name = "bComenzar";
            this.bComenzar.Size = new System.Drawing.Size(75, 23);
            this.bComenzar.TabIndex = 0;
            this.bComenzar.Text = "Comenzar!";
            this.bComenzar.UseVisualStyleBackColor = true;
            this.bComenzar.Click += new System.EventHandler(this.bComenzar_Click);
            // 
            // bDetener
            // 
            this.bDetener.Location = new System.Drawing.Point(317, 166);
            this.bDetener.Name = "bDetener";
            this.bDetener.Size = new System.Drawing.Size(75, 23);
            this.bDetener.TabIndex = 1;
            this.bDetener.Text = "Limpiar";
            this.bDetener.UseVisualStyleBackColor = true;
            this.bDetener.Click += new System.EventHandler(this.bDetener_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Horas a simular";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Cantidad de cajas";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(114, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Cantidad de terminales";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rTotalClientes);
            this.groupBox1.Controls.Add(this.rTiempoTramiteCliente);
            this.groupBox1.Controls.Add(this.rTiempoAcumuladoOcioso);
            this.groupBox1.Controls.Add(this.rTiempoEsperaMaximoCajas);
            this.groupBox1.Controls.Add(this.rTiempoEsperaPromedioCajas);
            this.groupBox1.Controls.Add(this.rColaMaximaTerminales);
            this.groupBox1.Location = new System.Drawing.Point(153, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(378, 148);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Resultados";
            // 
            // rTotalClientes
            // 
            this.rTotalClientes.AutoSize = true;
            this.rTotalClientes.Location = new System.Drawing.Point(6, 118);
            this.rTotalClientes.Name = "rTotalClientes";
            this.rTotalClientes.Size = new System.Drawing.Size(137, 13);
            this.rTotalClientes.TabIndex = 5;
            this.rTotalClientes.Text = "Total de clientes atendidos:";
            // 
            // rTiempoTramiteCliente
            // 
            this.rTiempoTramiteCliente.AutoSize = true;
            this.rTiempoTramiteCliente.Location = new System.Drawing.Point(6, 99);
            this.rTiempoTramiteCliente.Name = "rTiempoTramiteCliente";
            this.rTiempoTramiteCliente.Size = new System.Drawing.Size(218, 13);
            this.rTiempoTramiteCliente.TabIndex = 4;
            this.rTiempoTramiteCliente.Text = "Tiempo promedio para trámites de un cliente:";
            // 
            // rTiempoAcumuladoOcioso
            // 
            this.rTiempoAcumuladoOcioso.AutoSize = true;
            this.rTiempoAcumuladoOcioso.Location = new System.Drawing.Point(6, 80);
            this.rTiempoAcumuladoOcioso.Name = "rTiempoAcumuladoOcioso";
            this.rTiempoAcumuladoOcioso.Size = new System.Drawing.Size(191, 13);
            this.rTiempoAcumuladoOcioso.TabIndex = 3;
            this.rTiempoAcumuladoOcioso.Text = "Tiempo acumulado de cajeros ociosos:";
            // 
            // rTiempoEsperaMaximoCajas
            // 
            this.rTiempoEsperaMaximoCajas.AutoSize = true;
            this.rTiempoEsperaMaximoCajas.Location = new System.Drawing.Point(6, 61);
            this.rTiempoEsperaMaximoCajas.Name = "rTiempoEsperaMaximoCajas";
            this.rTiempoEsperaMaximoCajas.Size = new System.Drawing.Size(241, 13);
            this.rTiempoEsperaMaximoCajas.TabIndex = 2;
            this.rTiempoEsperaMaximoCajas.Text = "Tiempo de espera máximo en la cola de las cajas:";
            // 
            // rTiempoEsperaPromedioCajas
            // 
            this.rTiempoEsperaPromedioCajas.AutoSize = true;
            this.rTiempoEsperaPromedioCajas.Location = new System.Drawing.Point(6, 41);
            this.rTiempoEsperaPromedioCajas.Name = "rTiempoEsperaPromedioCajas";
            this.rTiempoEsperaPromedioCajas.Size = new System.Drawing.Size(249, 13);
            this.rTiempoEsperaPromedioCajas.TabIndex = 1;
            this.rTiempoEsperaPromedioCajas.Text = "Tiempo de espera promedio en la cola de las cajas:";
            // 
            // rColaMaximaTerminales
            // 
            this.rColaMaximaTerminales.AutoSize = true;
            this.rColaMaximaTerminales.Location = new System.Drawing.Point(6, 22);
            this.rColaMaximaTerminales.Name = "rColaMaximaTerminales";
            this.rColaMaximaTerminales.Size = new System.Drawing.Size(201, 13);
            this.rColaMaximaTerminales.TabIndex = 0;
            this.rColaMaximaTerminales.Text = "Tamaño máximo de la cola en terminales:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cantidadTerminales);
            this.groupBox2.Controls.Add(this.cantidadCajas);
            this.groupBox2.Controls.Add(this.horasSimulacion);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(126, 148);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Variables";
            // 
            // cantidadTerminales
            // 
            this.cantidadTerminales.Location = new System.Drawing.Point(9, 110);
            this.cantidadTerminales.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.cantidadTerminales.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.cantidadTerminales.Name = "cantidadTerminales";
            this.cantidadTerminales.Size = new System.Drawing.Size(76, 20);
            this.cantidadTerminales.TabIndex = 0;
            this.cantidadTerminales.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cantidadCajas
            // 
            this.cantidadCajas.Location = new System.Drawing.Point(9, 71);
            this.cantidadCajas.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.cantidadCajas.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.cantidadCajas.Name = "cantidadCajas";
            this.cantidadCajas.Size = new System.Drawing.Size(76, 20);
            this.cantidadCajas.TabIndex = 1;
            this.cantidadCajas.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // horasSimulacion
            // 
            this.horasSimulacion.Location = new System.Drawing.Point(9, 32);
            this.horasSimulacion.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.horasSimulacion.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.horasSimulacion.Name = "horasSimulacion";
            this.horasSimulacion.Size = new System.Drawing.Size(76, 20);
            this.horasSimulacion.TabIndex = 5;
            this.horasSimulacion.Value = new decimal(new int[] {
            7,
            0,
            0,
            0});
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(543, 206);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.bDetener);
            this.Controls.Add(this.bComenzar);
            this.Name = "Form1";
            this.Text = "Simulador SEM";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cantidadTerminales)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cantidadCajas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.horasSimulacion)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bComenzar;
        private System.Windows.Forms.Button bDetener;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown horasSimulacion;
        private System.Windows.Forms.Label rTiempoEsperaMaximoCajas;
        private System.Windows.Forms.Label rTiempoEsperaPromedioCajas;
        private System.Windows.Forms.Label rColaMaximaTerminales;
        private System.Windows.Forms.Label rTiempoAcumuladoOcioso;
        private System.Windows.Forms.Label rTotalClientes;
        private System.Windows.Forms.Label rTiempoTramiteCliente;
        private System.Windows.Forms.NumericUpDown cantidadCajas;
        private System.Windows.Forms.NumericUpDown cantidadTerminales;
    }
}

