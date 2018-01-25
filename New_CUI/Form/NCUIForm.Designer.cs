namespace New_CUI
{
    partial class NCUIForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            client.SendMessage(DataStructure.Command.cmd_Stop);
            client.Disconnect();
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_Port = new System.Windows.Forms.TextBox();
            this.button_Connect = new System.Windows.Forms.Button();
            this.button_START = new System.Windows.Forms.Button();
            this.button_STOP = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.button_Send = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkTest = new System.Windows.Forms.CheckBox();
            this.textBox_cmd = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button_Discon = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.button_INIT = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.ipAddressControl1 = new IPAddressControlLib.IPAddressControl();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.listBoxLog = new System.Windows.Forms.ListBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(11, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP Address";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(11, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 25);
            this.label2.TabIndex = 2;
            this.label2.Text = "PORT";
            // 
            // textBox_Port
            // 
            this.textBox_Port.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_Port.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_Port.Location = new System.Drawing.Point(120, 42);
            this.textBox_Port.Name = "textBox_Port";
            this.textBox_Port.Size = new System.Drawing.Size(225, 27);
            this.textBox_Port.TabIndex = 3;
            this.textBox_Port.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // button_Connect
            // 
            this.button_Connect.BackColor = System.Drawing.Color.MediumAquamarine;
            this.button_Connect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Connect.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Connect.Location = new System.Drawing.Point(10, 87);
            this.button_Connect.Name = "button_Connect";
            this.button_Connect.Size = new System.Drawing.Size(106, 95);
            this.button_Connect.TabIndex = 4;
            this.button_Connect.Text = "Connect";
            this.button_Connect.UseVisualStyleBackColor = false;
            this.button_Connect.EnabledChanged += new System.EventHandler(this.button_Connect_EnabledChanged);
            this.button_Connect.Click += new System.EventHandler(this.button_Connect_Click);
            // 
            // button_START
            // 
            this.button_START.BackColor = System.Drawing.Color.MediumAquamarine;
            this.button_START.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_START.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_START.Location = new System.Drawing.Point(126, 188);
            this.button_START.Name = "button_START";
            this.button_START.Size = new System.Drawing.Size(106, 95);
            this.button_START.TabIndex = 5;
            this.button_START.Text = "START";
            this.button_START.UseVisualStyleBackColor = false;
            this.button_START.EnabledChanged += new System.EventHandler(this.button_START_EnabledChanged);
            this.button_START.Click += new System.EventHandler(this.button_START_Click);
            // 
            // button_STOP
            // 
            this.button_STOP.BackColor = System.Drawing.Color.MediumAquamarine;
            this.button_STOP.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.button_STOP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_STOP.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_STOP.Location = new System.Drawing.Point(240, 188);
            this.button_STOP.Name = "button_STOP";
            this.button_STOP.Size = new System.Drawing.Size(106, 95);
            this.button_STOP.TabIndex = 6;
            this.button_STOP.Text = "STOP";
            this.button_STOP.UseVisualStyleBackColor = false;
            this.button_STOP.EnabledChanged += new System.EventHandler(this.button_STOP_EnabledChanged);
            this.button_STOP.Click += new System.EventHandler(this.button_STOP_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(120, 2);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(114, 30);
            this.label3.TabIndex = 7;
            this.label3.Text = "Command";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // button_Send
            // 
            this.button_Send.BackColor = System.Drawing.Color.MediumAquamarine;
            this.button_Send.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Send.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Send.Location = new System.Drawing.Point(9, 208);
            this.button_Send.Name = "button_Send";
            this.button_Send.Size = new System.Drawing.Size(336, 56);
            this.button_Send.TabIndex = 8;
            this.button_Send.Text = "SEND";
            this.button_Send.UseVisualStyleBackColor = false;
            this.button_Send.EnabledChanged += new System.EventHandler(this.button_Send_EnabledChanged);
            this.button_Send.Click += new System.EventHandler(this.button_Send_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.chkTest);
            this.panel1.Controls.Add(this.textBox_cmd);
            this.panel1.Controls.Add(this.button_Send);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Location = new System.Drawing.Point(503, 354);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(358, 277);
            this.panel1.TabIndex = 11;
            // 
            // chkTest
            // 
            this.chkTest.Enabled = false;
            this.chkTest.Location = new System.Drawing.Point(9, 11);
            this.chkTest.Name = "chkTest";
            this.chkTest.Size = new System.Drawing.Size(49, 16);
            this.chkTest.TabIndex = 11;
            this.chkTest.Text = "Test";
            this.chkTest.UseVisualStyleBackColor = true;
            this.chkTest.Visible = false;
            // 
            // textBox_cmd
            // 
            this.textBox_cmd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_cmd.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_cmd.Location = new System.Drawing.Point(9, 39);
            this.textBox_cmd.Multiline = true;
            this.textBox_cmd.Name = "textBox_cmd";
            this.textBox_cmd.Size = new System.Drawing.Size(336, 162);
            this.textBox_cmd.TabIndex = 10;
            this.textBox_cmd.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.button_Discon);
            this.panel2.Controls.Add(this.statusStrip);
            this.panel2.Controls.Add(this.button_INIT);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.button_STOP);
            this.panel2.Controls.Add(this.ipAddressControl1);
            this.panel2.Controls.Add(this.textBox_Port);
            this.panel2.Controls.Add(this.button_Connect);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.button_START);
            this.panel2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panel2.Location = new System.Drawing.Point(503, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(358, 324);
            this.panel2.TabIndex = 9;
            // 
            // button_Discon
            // 
            this.button_Discon.BackColor = System.Drawing.Color.MediumAquamarine;
            this.button_Discon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Discon.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Discon.Location = new System.Drawing.Point(126, 87);
            this.button_Discon.Name = "button_Discon";
            this.button_Discon.Size = new System.Drawing.Size(106, 95);
            this.button_Discon.TabIndex = 18;
            this.button_Discon.Text = "Discon";
            this.button_Discon.UseVisualStyleBackColor = false;
            this.button_Discon.EnabledChanged += new System.EventHandler(this.button_Discon_EnabledChanged);
            this.button_Discon.Click += new System.EventHandler(this.button_Discon_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 300);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(356, 22);
            this.statusStrip.TabIndex = 17;
            this.statusStrip.Text = "statusStrip";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // button_INIT
            // 
            this.button_INIT.BackColor = System.Drawing.Color.MediumAquamarine;
            this.button_INIT.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_INIT.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_INIT.Location = new System.Drawing.Point(240, 87);
            this.button_INIT.Name = "button_INIT";
            this.button_INIT.Size = new System.Drawing.Size(106, 95);
            this.button_INIT.TabIndex = 16;
            this.button_INIT.Text = "INIT";
            this.button_INIT.UseVisualStyleBackColor = false;
            this.button_INIT.EnabledChanged += new System.EventHandler(this.button_INIT_EnabledChanged);
            this.button_INIT.Click += new System.EventHandler(this.btn_INIT_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.MediumAquamarine;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button1.Location = new System.Drawing.Point(10, 188);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(106, 95);
            this.button1.TabIndex = 15;
            this.button1.Text = "FTP";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ipAddressControl1
            // 
            this.ipAddressControl1.AllowInternalTab = false;
            this.ipAddressControl1.AutoHeight = true;
            this.ipAddressControl1.BackColor = System.Drawing.SystemColors.Window;
            this.ipAddressControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ipAddressControl1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.ipAddressControl1.Font = new System.Drawing.Font("맑은 고딕", 11.25F);
            this.ipAddressControl1.Location = new System.Drawing.Point(120, 7);
            this.ipAddressControl1.Name = "ipAddressControl1";
            this.ipAddressControl1.ReadOnly = false;
            this.ipAddressControl1.Size = new System.Drawing.Size(225, 27);
            this.ipAddressControl1.TabIndex = 1;
            this.ipAddressControl1.Text = "...";
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.label6);
            this.panel3.Location = new System.Drawing.Point(9, 12);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(479, 35);
            this.panel3.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(194, 4);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(114, 25);
            this.label6.TabIndex = 13;
            this.label6.Text = "로그 메시지";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.listBoxLog);
            this.panel4.Location = new System.Drawing.Point(9, 44);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(479, 587);
            this.panel4.TabIndex = 14;
            // 
            // listBoxLog
            // 
            this.listBoxLog.Font = new System.Drawing.Font("맑은 고딕", 11.25F);
            this.listBoxLog.FormattingEnabled = true;
            this.listBoxLog.ItemHeight = 20;
            this.listBoxLog.Location = new System.Drawing.Point(0, 3);
            this.listBoxLog.Name = "listBoxLog";
            this.listBoxLog.ScrollAlwaysVisible = true;
            this.listBoxLog.Size = new System.Drawing.Size(479, 584);
            this.listBoxLog.TabIndex = 15;
            // 
            // NCUIForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(873, 646);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel4);
            this.Name = "NCUIForm";
            this.Text = "New CUI";
            this.Load += new System.EventHandler(this.NCUIForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_Connect;
        private System.Windows.Forms.TextBox textBox_Port;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_STOP;
        private System.Windows.Forms.Button button_START;
        private System.Windows.Forms.Button button_Send;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TextBox textBox_cmd;
        private IPAddressControlLib.IPAddressControl ipAddressControl1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox chkTest;
        private System.Windows.Forms.Button button_INIT;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ListBox listBoxLog;
        private System.Windows.Forms.Button button_Discon;

    }
}

