﻿using System;
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
using FileManager;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;


namespace New_CUI
{
    public partial class NCUIForm : Form
    {
        private DataStructure.DS _ds = null;
        private ClientSocket client = new ClientSocket();
        private StatusBarPanel pnlStatus = new StatusBarPanel();
        private string ftpfileName = @"WinSCP.exe";
        private string logpath = string.Empty;
        private string logextension = ".log";
        private int configcheck =-1;
        private bool logfileopen = false;
        private FileManager.LogFileWrite logfile = new FileManager.LogFileWrite();
        private static object syncRoot = new object();

        public NCUIForm()
        {
            _ds = DS.Instance;
            InitializeComponent();
            SetLogHandler();
            DispStandbyMsg();
            Initialized();
        }
        private void Initialized()
        {
            //Control
            ipAddressControl1.Focus();
            button_Discon.Enabled = false;
            button_INIT.Enabled = false;
            button_START.Enabled = false;
            button_STOP.Enabled = false;
            button_Send.Enabled = false;
            button_1.Enabled = false;
            button_2.Enabled = false;
            button_3.Enabled = false;
            button_4.Enabled = false;
            button_5.Enabled = false;

            //Read and check Config file
            ConfigRead cf = new ConfigRead();
            cf.FilePath = Directory.GetCurrentDirectory();
            configcheck = cf.ConfigStart();
            if (configcheck == 0)
            {
                ipAddressControl1.Text = _ds.IPAddr;
                textBox_Port.Text = _ds.Port;
                listBox_cmd.Items.Add("자주 사용하는 CMD");
                foreach (var data in _ds._Cmd)
                    listBox_cmd.Items.Add(data.Key + " " + data.Value);
                Handler.LogMsg.AddNShow("[CUI] Config 파일을 성공적으로 읽었습니다.");
            }
            else if (configcheck == -1) Handler.LogMsg.AddNShow("[CUI] Config 경로가 제대로 설정되지 않았습니다.");
            else if (configcheck == -2) Handler.LogMsg.AddNShow("[CUI] Config 경로에 파일이 없습니다.");
            else Handler.LogMsg.AddNShow("[CUI] Config Read 중 Error 발생");
        }

        public void DispLog(string msg)
        {
            lock (syncRoot)
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate()
                    {
                        ///TEM에서 [CMD]START, [CMD]STOP을 보낼 시
                        ///CUI에서 User가 START 또는 STOP을 누른 것 처럼 눌림.
                        CheckCmdfromTEM(msg);
                        if (listBoxLog.Items.Count >= 30 && listBoxLog.Items[0] != null)
                            listBoxLog.Items.RemoveAt(0);

                        if (logfileopen) logfile.logfilewrite(msg);

                        listBoxLog.TopIndex = listBoxLog.Items.Add(msg);
                    }
                    ));

                }
                else
                {
                    CheckCmdfromTEM(msg);
                    if (listBoxLog.Items.Count >= 30 && listBoxLog.Items[0] != null)
                        listBoxLog.Items.RemoveAt(0);

                    if (logfileopen) logfile.logfilewrite(msg);

                    listBoxLog.TopIndex = listBoxLog.Items.Add(msg);
                }
            }
        }
        private void SetLogHandler()
        {
            Handler.LogMsg.DispHandler = DispLog;
        }
        private void DispStandbyMsg()
        {
            Handler.LogMsg.AddNShow("[CUI] 프로그램 준비가 완료되었습니다.");
            toolStripStatusLabel.Text = "Ready";
        }

        private void button_Send_Click(object sender, EventArgs e)
        {
            string cmd = string.Empty;
            int value = 0;
            try
            {
                int.TryParse(textBox_cmd.Text, out value);
                if (configcheck == 0 && _ds._Cmd.ContainsKey(value))
                {
                    _ds._Cmd.TryGetValue(value, out cmd);
                    client.SendMessage(cmd);
                }
                else client.SendMessage(textBox_cmd.Text);
            }
            catch (Exception ex)
            {
                Handler.LogMsg.AddNShow("[CUI] Send Error : " + ex.Message);
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
                    button_1.Enabled = true;
                    button_2.Enabled = true;
                    button_3.Enabled = true;
                    button_4.Enabled = true;
                    button_5.Enabled = true;
                    toolStripStatusLabel.Text = "IP 주소 : " + ipAddressControl1.Text +
                        " 포트 번호 : " + textBox_Port.Text + "로 연결 완료";
                }
            }
        }

        private string WriteDateTime()
        {
            string revalue = DateTime.Now.ToString("yyMMdd_HHmmss");
            return revalue;
        }

        private void button_START_Click(object sender, EventArgs e)
        {
            bool IsSuccess = false;
            IsSuccess = client.SendMessage(Command.cmd_Start);
            if (IsSuccess)
            {
                DoSTART();
            }
        }

        public void DoSTART()
        {
            if (configcheck == 0)
            {
                logfile.FilePath = Path.Combine(_ds.DIR, WriteDateTime() + logextension);
                logfileopen = logfile.logfileopen();
            }
            button_START.Enabled = false;
            button_STOP.Enabled = true;
            toolStripStatusLabel.Text = "TEM START 전송";
        }

        public void DoSTOP()
        {
            if (logfileopen)
            {
                logfile.logfileclose();
                logfileopen = false;
            }
            button_START.Enabled = true;
            button_STOP.Enabled = false;
            toolStripStatusLabel.Text = "TEM STOP 전송";
        }

        private void button_STOP_Click(object sender, EventArgs e)
        {
            bool IsSuccess = false;
            IsSuccess = client.SendMessage(Command.cmd_Stop);
            if (IsSuccess)
            {
                DoSTOP();
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
                button_1.Enabled = false;
                button_2.Enabled = false;
                button_3.Enabled = false;
                button_4.Enabled = false;
                button_5.Enabled = false;
                toolStripStatusLabel.Text = "TEM Disconnect";
            }
        }

        private void button_Discon_EnabledChanged(object sender, EventArgs e)
        {
            if (button_Discon.Enabled) button_Discon.BackColor = Color.MediumAquamarine;
            else button_Discon.BackColor = SystemColors.InactiveCaption;
        }

        private void button_1_EnabledChanged(object sender, EventArgs e)
        {
            if (button_1.Enabled) button_1.BackColor = Color.MediumAquamarine;
            else button_1.BackColor = SystemColors.InactiveCaption;
        }

        private void button_2_EnabledChanged(object sender, EventArgs e)
        {
            if (button_2.Enabled) button_2.BackColor = Color.MediumAquamarine;
            else button_2.BackColor = SystemColors.InactiveCaption;
        }

        private void button_3_EnabledChanged(object sender, EventArgs e)
        {
            if (button_3.Enabled) button_3.BackColor = Color.MediumAquamarine;
            else button_3.BackColor = SystemColors.InactiveCaption;
        }

        private void button_4_EnabledChanged(object sender, EventArgs e)
        {
            if (button_4.Enabled) button_4.BackColor = Color.MediumAquamarine;
            else button_4.BackColor = SystemColors.InactiveCaption;
        }

        private void button_5_EnabledChanged(object sender, EventArgs e)
        {
            if (button_5.Enabled) button_5.BackColor = Color.MediumAquamarine;
            else button_5.BackColor = SystemColors.InactiveCaption;
        }

        private void button_1_Click(object sender, EventArgs e)
        {
            bool IsSuccess = false;
            string cmd = string.Empty;
            try
            {
                _ds._Cmd.TryGetValue(1, out cmd);
                IsSuccess = client.SendMessage(cmd);
            }
            catch (Exception ex)
            {
                Handler.LogMsg.AddNShow("[CUI] Send Error : " + ex.Message);
            }
        }

        private void button_2_Click(object sender, EventArgs e)
        {
            bool IsSuccess = false;
            string cmd = string.Empty;
            try
            {
                _ds._Cmd.TryGetValue(2, out cmd);
                IsSuccess = client.SendMessage(cmd);
            }
            catch (Exception ex)
            {
                Handler.LogMsg.AddNShow("[CUI] Send Error : " + ex.Message);
            }
        }

        private void button_3_Click(object sender, EventArgs e)
        {
            bool IsSuccess = false;
            string cmd = string.Empty;
            try
            {
                _ds._Cmd.TryGetValue(3, out cmd);
                IsSuccess = client.SendMessage(cmd);
            }
            catch (Exception ex)
            {
                Handler.LogMsg.AddNShow("[CUI] Send Error : " + ex.Message);
            }
        }

        private void button_4_Click(object sender, EventArgs e)
        {
            bool IsSuccess = false;
            string cmd = string.Empty;
            try
            {
                _ds._Cmd.TryGetValue(4, out cmd);
                IsSuccess = client.SendMessage(cmd);
            }
            catch (Exception ex)
            {
                Handler.LogMsg.AddNShow("[CUI] Send Error : " + ex.Message);
            }
        }

        private void button_5_Click(object sender, EventArgs e)
        {
            bool IsSuccess = false;
            string cmd = string.Empty;
            try
            {
                _ds._Cmd.TryGetValue(5, out cmd);
                IsSuccess = client.SendMessage(cmd);
            }
            catch (Exception ex)
            {
                Handler.LogMsg.AddNShow("[CUI] Send Error : " + ex.Message);
            }
        }

        private void CheckCmdfromTEM(string msg)
        {
            string cmd = CheckCmdstring(msg);
            
            if (cmd.Equals(DataStructure.Command.cmd_Start))
            {
                DoSTART();
            }
            else if (cmd.Equals(DataStructure.Command.cmd_Stop))
            {
                DoSTOP();
            }
        }

        private string CheckCmdstring(string cmd)
        {
            string rtn = string.Empty;
            string[] del = { "[CMD]", "\0" };

            string[] data = cmd.Split(del, StringSplitOptions.RemoveEmptyEntries);
            if (data.Length == 3)
                rtn = data[1];

            return rtn;
        }
    }
}
