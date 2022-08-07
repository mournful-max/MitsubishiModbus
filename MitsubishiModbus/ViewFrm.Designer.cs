namespace MitsubishiModbus
{
    partial class ViewFrm
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
            this.txtOutReg = new System.Windows.Forms.TextBox();
            this.txtOutVal = new System.Windows.Forms.TextBox();
            this.txtInReg = new System.Windows.Forms.TextBox();
            this.txtInVal = new System.Windows.Forms.TextBox();
            this.btnSet = new System.Windows.Forms.Button();
            this.btnGet = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.lblRegisters = new System.Windows.Forms.Label();
            this.lblValues = new System.Windows.Forms.Label();
            this.cbInitOutputs = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // txtOutReg
            // 
            this.txtOutReg.Location = new System.Drawing.Point(75, 75);
            this.txtOutReg.Name = "txtOutReg";
            this.txtOutReg.Size = new System.Drawing.Size(100, 23);
            this.txtOutReg.TabIndex = 0;
            this.txtOutReg.TabStop = false;
            // 
            // txtOutVal
            // 
            this.txtOutVal.Location = new System.Drawing.Point(202, 75);
            this.txtOutVal.Name = "txtOutVal";
            this.txtOutVal.Size = new System.Drawing.Size(100, 23);
            this.txtOutVal.TabIndex = 0;
            this.txtOutVal.TabStop = false;
            // 
            // txtInReg
            // 
            this.txtInReg.Location = new System.Drawing.Point(75, 121);
            this.txtInReg.Name = "txtInReg";
            this.txtInReg.Size = new System.Drawing.Size(100, 23);
            this.txtInReg.TabIndex = 0;
            this.txtInReg.TabStop = false;
            // 
            // txtInVal
            // 
            this.txtInVal.Location = new System.Drawing.Point(202, 121);
            this.txtInVal.Name = "txtInVal";
            this.txtInVal.ReadOnly = true;
            this.txtInVal.Size = new System.Drawing.Size(100, 23);
            this.txtInVal.TabIndex = 0;
            this.txtInVal.TabStop = false;
            // 
            // btnSet
            // 
            this.btnSet.Location = new System.Drawing.Point(330, 74);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(75, 23);
            this.btnSet.TabIndex = 0;
            this.btnSet.TabStop = false;
            this.btnSet.Text = "Set";
            this.btnSet.UseVisualStyleBackColor = true;
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // btnGet
            // 
            this.btnGet.Location = new System.Drawing.Point(330, 121);
            this.btnGet.Name = "btnGet";
            this.btnGet.Size = new System.Drawing.Size(75, 23);
            this.btnGet.TabIndex = 0;
            this.btnGet.TabStop = false;
            this.btnGet.Text = "Get";
            this.btnGet.UseVisualStyleBackColor = true;
            this.btnGet.Click += new System.EventHandler(this.btnGet_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(227, 171);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.TabStop = false;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(330, 171);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(75, 23);
            this.btnDisconnect.TabIndex = 0;
            this.btnDisconnect.TabStop = false;
            this.btnDisconnect.Text = "Disconn";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // lblRegisters
            // 
            this.lblRegisters.AutoSize = true;
            this.lblRegisters.Location = new System.Drawing.Point(72, 37);
            this.lblRegisters.Name = "lblRegisters";
            this.lblRegisters.Size = new System.Drawing.Size(57, 15);
            this.lblRegisters.TabIndex = 8;
            this.lblRegisters.Text = "Registers";
            // 
            // lblValues
            // 
            this.lblValues.AutoSize = true;
            this.lblValues.Location = new System.Drawing.Point(199, 37);
            this.lblValues.Name = "lblValues";
            this.lblValues.Size = new System.Drawing.Size(43, 15);
            this.lblValues.TabIndex = 9;
            this.lblValues.Text = "Values";
            // 
            // cbInitOutputs
            // 
            this.cbInitOutputs.AutoSize = true;
            this.cbInitOutputs.Checked = true;
            this.cbInitOutputs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbInitOutputs.Location = new System.Drawing.Point(131, 174);
            this.cbInitOutputs.Name = "cbInitOutputs";
            this.cbInitOutputs.Size = new System.Drawing.Size(90, 19);
            this.cbInitOutputs.TabIndex = 10;
            this.cbInitOutputs.Text = "Init outputs";
            this.cbInitOutputs.UseVisualStyleBackColor = true;
            // 
            // ViewFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 221);
            this.Controls.Add(this.cbInitOutputs);
            this.Controls.Add(this.lblValues);
            this.Controls.Add(this.lblRegisters);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btnGet);
            this.Controls.Add(this.btnSet);
            this.Controls.Add(this.txtInVal);
            this.Controls.Add(this.txtInReg);
            this.Controls.Add(this.txtOutVal);
            this.Controls.Add(this.txtOutReg);
            this.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ViewFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mitsubishi Modbus FC1, FC5 testing";
            this.Load += new System.EventHandler(this.ViewFrm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtOutReg;
        private System.Windows.Forms.TextBox txtOutVal;
        private System.Windows.Forms.TextBox txtInReg;
        private System.Windows.Forms.TextBox txtInVal;
        private System.Windows.Forms.Button btnSet;
        private System.Windows.Forms.Button btnGet;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Label lblRegisters;
        private System.Windows.Forms.Label lblValues;
        private System.Windows.Forms.CheckBox cbInitOutputs;
    }
}

