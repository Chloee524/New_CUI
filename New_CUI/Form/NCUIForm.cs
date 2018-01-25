using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Library;
using socket;
using DataStructure;
using System.IO;
using System.Net;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;


namespace New_CUI
{
    public partial class NCUIForm : Form
    {
        private ClientSocket client = new ClientSocket();
        private StatusBarPanel pnlStatus = new StatusBarPanel();
        private string ftpfileName = @"WinSCP.exe";
        private int i = 0;

        public NCUIForm()
        {
            InitializeComponent();
            Initialized();
            SetLogHandler();
            DispStandbyMsg();
        }
        private void Initialized()
        {
            ipAddressControl1.Focus();
            button_Discon.Enabled = false;
            button_INIT.Enabled = false;
            button_START.Enabled = false;
            button_STOP.Enabled = false;
            button_Send.Enabled = false;
        }
        private void DispLog(string msg)
        {
            if (listBoxLog.Items.Count >= 1000 && listBoxLog.Items[0] != null)
                listBoxLog.Items.RemoveAt(0);

            listBoxLog.TopIndex = listBoxLog.Items.Add(msg);
        }
        private void SetLogHandler()
        {
            Handler.LogMsg.DispHandler = DispLog;
        }
        private void DispStandbyMsg()
        {
            Handler.LogMsg.AddNShow("프로그램 준비가 완료되었습니다.");
            toolStripStatusLabel.Text = "Ready";
        }

        private void button_Send_Click(object sender, EventArgs e)
        {
            if (!chkTest.Checked)
            {
                try
                {
                    client.SendMessage(textBox_cmd.Text);
                }
                catch (Exception ex)
                {
                    Handler.LogMsg.AddNShow("Send Error : " + ex.Message);
                }
            }
            else
            {
                while (true)
                {
                    Handler.LogMsg.AddNShow(i.ToString() + "Command show Test" + listBoxLog.Items.Count.ToString());
                    i++;
                }
            }
        }

        private void button_Connect_Click(object sender, EventArgs e)
        {
            bool IsSuccess = false;

            if (string.IsNullOrEmpty(ipAddressControl1.Text) || string.IsNullOrEmpty(textBox_Port.Text))
            {
                toolStripStatusLabel.Text = "IP 주소 및 Port 번호를 확인 해 주세요.";
            }
            else
            {
                IsSuccess = client.Connect(ipAddressControl1.Text, int.Parse(textBox_Port.Text));
                if (IsSuccess)
                {
                    button_Connect.Enabled = false;
                    button_Discon.Enabled = true;
                    button_Send.Enabled = true;
                    button_INIT.Enabled = true;
                    toolStripStatusLabel.Text = "IP 주소 : " + ipAddressControl1.Text +
                        " 포트 번호 : " + textBox_Port.Text + "로 연결 완료";
                }
            }
        }

        private void button_START_Click(object sender, EventArgs e)
        {
            bool IsSuccess = false;
            IsSuccess = client.SendMessage(Command.cmd_Start);
            if (IsSuccess)
            {
                button_START.Enabled = false;
                button_STOP.Enabled = true;
                toolStripStatusLabel.Text = "TEM START 전송";
            }

        }

        private void button_STOP_Click(object sender, EventArgs e)
        {
            bool IsSuccess = false;
            IsSuccess = client.SendMessage(Command.cmd_Stop);
            if (IsSuccess)
            {
                button_START.Enabled = true;
                button_STOP.Enabled = false;
                toolStripStatusLabel.Text = "TEM STOP 전송";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process ps = new Process();
            try
            {
                ps.StartInfo.FileName = ftpfileName;
                ps.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
                ps.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void btn_INIT_Click(object sender, EventArgs e)
        {
            bool IsSuccess = false;
            
            IsSuccess = client.SendMessage(Command.cmd_Init);
            if (IsSuccess)
            {
                button_INIT.Enabled = false;
                button_START.Enabled = true;
                toolStripStatusLabel.Text = "TEM INIT 전송";
            }
        }

        private void button_Connect_EnabledChanged(object sender, EventArgs e)
        {
            if (button_Connect.Enabled) button_Connect.BackColor = Color.MediumAquamarine;
            else button_Connect.BackColor = SystemColors.InactiveCaption;
        }

        private void button_START_EnabledChanged(object sender, EventArgs e)
        {
            if (button_START.Enabled) button_START.BackColor = Color.MediumAquamarine;
            else button_START.BackColor = SystemColors.InactiveCaption;
        }

        private void button_INIT_EnabledChanged(object sender, EventArgs e)
        {
            if (button_INIT.Enabled) button_INIT.BackColor = Color.MediumAquamarine;
            else button_INIT.BackColor = SystemColors.InactiveCaption;
        }

        private void button_STOP_EnabledChanged(object sender, EventArgs e)
        {
            if (button_STOP.Enabled) button_STOP.BackColor = Color.MediumAquamarine;
            else button_STOP.BackColor = SystemColors.InactiveCaption;
        }

        private void button_Send_EnabledChanged(object sender, EventArgs e)
        {
            if (button_Send.Enabled) button_Send.BackColor = Color.MediumAquamarine;
            else button_Send.BackColor = SystemColors.InactiveCaption;
        }

        private void button_Discon_Click(object sender, EventArgs e)
        {
            bool IsSuccess = false;
            client.SendMessage(DataStructure.Command.cmd_Stop);
            IsSuccess = client.Disconnect();
            if (IsSuccess)
            {
                button_Connect.Enabled = true;
                button_Discon.Enabled = false;
                button_Send.Enabled = false;
                button_INIT.Enabled = false;
                button_START.Enabled = false;
                button_STOP.Enabled = false;
                toolStripStatusLabel.Text = "TEM Disconnect";
            }
        }

        private void button_Discon_EnabledChanged(object sender, EventArgs e)
        {
            if (button_Discon.Enabled) button_Discon.BackColor = Color.MediumAquamarine;
            else button_Discon.BackColor = SystemColors.InactiveCaption;
        }

        private void NCUIForm_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
