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
using FileManager;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;

namespace New_CUI
{
    public partial class NCUIForm : Form
    {
        #region Private Member Variables
        private DataStructure.DS         _ds           = null;
        private int                      _configcheck  = -1;
        private int                      _symbolcheck  = -1;
        private bool                     _logfileopen  = false;
        private string                   _ftpfileName  = @"WinSCP.exe";
        private string                   _logpath      = string.Empty;
        private string                   _logextension = ".log";
        private List<string>             _listSymbol   = new List<string>();
        private ClientSocket             _client       = new ClientSocket();
        private StatusBarPanel           _pnlStatus    = new StatusBarPanel();
        private FileManager.LogFileWrite _logfile      = new FileManager.LogFileWrite();
        
        private static object            _syncRoot     = new object();
        #endregion Private Member Variables

        #region Constructor
        public NCUIForm()
        {
            _ds = DS.Instance;
            InitializeComponent();
            SetLogHandler();
            DispStandbyMsg();
            Initialized();
        }
        #endregion Constructor

        #region Private Member Functions
        private void Initialized()
        {
            //Control
            ipAddressControl1.Focus();
            button_Discon.Enabled     = false;
            button_INIT.Enabled       = false;
            button_START.Enabled      = false;
            button_STOP.Enabled       = false;
            button_Send.Enabled       = false;
            button_1.Enabled          = false;
            button_2.Enabled          = false;
            button_3.Enabled          = false;
            button_4.Enabled          = false;
            button_5.Enabled          = false;
            radNormal.Checked         = true;
            button_SymbolSend.Enabled = false;

            //Read and check Config file & Symbol file
            InitConfig();
            InitSymbol();
            InitGridView();
        }

        private void InitConfig()
        {
            Reader reader = new ConfigReader(new TextFile());

            if (!reader.Read())
            {
                Handler.LogMsg.AddNShow("[CUI] [CONFIG 설정]" + reader.GetLastErrMsg());
                return;
            }

            ipAddressControl1.Text = _ds.IpAddr;
            textBox_Port.Text = _ds.Port;
            listBox_cmd.Items.Add("자주 사용하는 CMD");

            foreach (var data in _ds.Cmd)
                listBox_cmd.Items.Add(data.Key + " " + data.Value);

            Handler.LogMsg.AddNShow("[CUI] [CONFIG 설정] 파일을 성공적으로 읽었습니다.");
        }

        private void InitSymbol()
        {
            Reader reader = new SymbolReader(new TextFile());

            if (!reader.Read())
            {
                Handler.LogMsg.AddNShow("[CUI] [SYMBOL 설정]" + reader.GetLastErrMsg());
                return;
            }

            foreach (var data in _ds.Symbol)
                listBox_Symbol.Items.Add(data);

            Handler.LogMsg.AddNShow("[CUI] [SYMBOL 설정] 파일을 성공적으로 읽었습니다.");
        }

        private void InitGridView()
        {
            _listSymbol = new List<string>();

            dataGridView_param.Rows.Clear();
            dataGridView_param.ColumnCount      = 2;
            dataGridView_param.Columns[0].Width = 180;
            dataGridView_param.Columns[1].Width = 300;
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

        private string WriteDateTime()
        {
            string revalue = DateTime.Now.ToString("yyMMdd_HHmmss");
            return revalue;
        }

        private void DoSTARTNormal()
        {
            if (_configcheck == 0)
            {
                _logfile.FilePath = Path.Combine(_ds.Dir, WriteDateTime() + _logextension);
                _logfileopen      = _logfile.OpenLogFile();
            }

            button_START.Enabled      = false;
            button_STOP.Enabled       = true;
            toolStripStatusLabel.Text = "TEM Normal Mode START 전송";
        }

        private void DoSTARTFault()
        {
            if (_configcheck == 0)
            {
                _logfile.FilePath = Path.Combine(_ds.Dir, WriteDateTime() + _logextension);
                _logfileopen      = _logfile.OpenLogFile();
            }

            button_START.Enabled      = false;
            button_STOP.Enabled       = true;
            toolStripStatusLabel.Text = "TEM Fault Mode START 전송";
        }

        private void DoSTOP()
        {
            if (_logfileopen)
            {
                _logfile.CloseLogFile();
                _logfileopen = false;
            }

            button_START.Enabled      = true;
            button_STOP.Enabled       = false;
            toolStripStatusLabel.Text = "TEM STOP 전송";
        }

        private void DoINIT()
        {
            button_INIT.Enabled       = false;
            button_START.Enabled      = true;
            toolStripStatusLabel.Text = "TEM INIT 전송";
        }

        private void CheckCmdfromTEM(string msg)
        {
            int value = 0;
            string cmd = CheckCmdstring(msg);
            
            int.TryParse(cmd, out value);

            if (_configcheck == 0 && _ds.Cmd.ContainsKey(value))
            {
                _ds.Cmd.TryGetValue(value, out cmd);
                _client.SendMessage(cmd);
            }
            else
            {
                if (cmd.Equals(Command.CmdStartNormal)) 
                    DoSTARTNormal();
                else if (cmd.Equals(Command.CmdStartFault)) 
                    DoSTARTFault();
                else if (cmd.Equals(Command.CmdStop)) 
                    DoSTOP();
                else if (cmd.Equals(Command.CmdInit)) 
                    DoINIT();
            }
        }

        private string CheckCmdstring(string cmd)
        {
            string rtn   = string.Empty;
            string[] del = { "[CMD]", "\0" };

            string[] data = cmd.Split(del, StringSplitOptions.RemoveEmptyEntries);

            if (data.Length == 3)
                rtn = data[1];

            return rtn;
        }

        private void ChangeButtonColor(Button button)
        {
            if (button.Enabled)
                button.BackColor = Color.MediumAquamarine;
            else
                button.BackColor = SystemColors.InactiveCaption;
        }

        private void SendCommand(int cmdNum)
        {
            bool isSuccess = false;
            string cmd = string.Empty;

            try
            {
                _ds.Cmd.TryGetValue(cmdNum, out cmd);
                isSuccess = _client.SendMessage(cmd);
            }
            catch (Exception ex)
            {
                Handler.LogMsg.AddNShow("[CUI] Send Error : " + ex.Message);
            }
        }
        #endregion Private Member Functions

        #region Public Member Functions
        public void DispLog(string msg)
        {
            lock (_syncRoot)
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new MethodInvoker(delegate()
                    {
                        if (listBox_log.Items.Count >= 10 && listBox_log.Items[0] != null)
                            listBox_log.Items.RemoveAt(0);

                        if (_logfileopen) _logfile.WriteLogFile(msg);

                        ///TEM에서 [CMD]START, [CMD]STOP을 보낼 시
                        ///CUI에서 User가 START 또는 STOP을 누른 것 처럼 눌림.
                        CheckCmdfromTEM(msg);
                        listBox_log.TopIndex = listBox_log.Items.Add(msg);
                    }
                    ));

                }
                else
                {
                    if (listBox_log.Items.Count >= 10 && listBox_log.Items[0] != null)
                        listBox_log.Items.RemoveAt(0);

                    if (_logfileopen) _logfile.WriteLogFile(msg);

                    ///TEM에서 [CMD]START, [CMD]STOP을 보낼 시
                    ///CUI에서 User가 START 또는 STOP을 누른 것 처럼 눌림.
                    CheckCmdfromTEM(msg);
                    listBox_log.TopIndex = listBox_log.Items.Add(msg);
                }
            }
        }
        #endregion Public Member Functions

        #region Windows Component Events
        private void button_STOP_Click(object sender, EventArgs e)
        {
            bool isSuccess = false;
            isSuccess = _client.SendMessage(Command.CmdStop);

            if (isSuccess)
            {
                DoSTOP();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process ps = new Process();

            try
            {
                ps.StartInfo.FileName = _ftpfileName;
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
            bool isSuccess = false;
            isSuccess = _client.SendMessage(Command.CmdInit);

            if (isSuccess)
            {
                DoINIT();
            }
        }

        private void button_Connect_Click(object sender, EventArgs e)
        {
            bool isSuccess = false;

            if (string.IsNullOrEmpty(ipAddressControl1.Text) || string.IsNullOrEmpty(textBox_Port.Text))
            {
                toolStripStatusLabel.Text = "IP 주소 및 Port 번호를 확인 해 주세요.";
            }
            else
            {
                isSuccess = _client.Connect(ipAddressControl1.Text, int.Parse(textBox_Port.Text));

                if (isSuccess)
                {
                    button_Connect.Enabled = false;
                    button_Discon.Enabled = true;
                    button_Send.Enabled = true;
                    button_SymbolSend.Enabled = true;
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

        private void button_Discon_Click(object sender, EventArgs e)
        {
            bool isSuccess = false;
            _client.SendMessage(DataStructure.Command.CmdStop);
            isSuccess = _client.Disconnect();

            if (isSuccess)
            {
                button_Connect.Enabled    = true;
                button_Discon.Enabled     = false;
                button_Send.Enabled       = false;
                button_SymbolSend.Enabled = false;
                button_INIT.Enabled       = false;
                button_START.Enabled      = false;
                button_STOP.Enabled       = false;
                button_1.Enabled          = false;
                button_2.Enabled          = false;
                button_3.Enabled          = false;
                button_4.Enabled          = false;
                button_5.Enabled          = false;
                toolStripStatusLabel.Text = "TEM Disconnect";
            }
        }

        private void button_Connect_EnabledChanged(object sender, EventArgs e)
        {
            ChangeButtonColor((Button)sender);
        }

        private void button_START_EnabledChanged(object sender, EventArgs e)
        {
            ChangeButtonColor((Button)sender);
        }

        private void button_INIT_EnabledChanged(object sender, EventArgs e)
        {
            ChangeButtonColor((Button)sender);
        }

        private void button_STOP_EnabledChanged(object sender, EventArgs e)
        {
            ChangeButtonColor((Button)sender);
        }

        private void button_Send_EnabledChanged(object sender, EventArgs e)
        {
            ChangeButtonColor((Button)sender);
        }

        private void button_Discon_EnabledChanged(object sender, EventArgs e)
        {
            ChangeButtonColor((Button)sender);
        }

        private void button_1_EnabledChanged(object sender, EventArgs e)
        {
            ChangeButtonColor((Button)sender);
        }

        private void button_2_EnabledChanged(object sender, EventArgs e)
        {
            ChangeButtonColor((Button)sender);
        }

        private void button_3_EnabledChanged(object sender, EventArgs e)
        {
            ChangeButtonColor((Button)sender);
        }

        private void button_4_EnabledChanged(object sender, EventArgs e)
        {
            ChangeButtonColor((Button)sender);
        }

        private void button_5_EnabledChanged(object sender, EventArgs e)
        {
            ChangeButtonColor((Button)sender);
        }

        private void button_SymbolSend_EnabledChanged(object sender, EventArgs e)
        {
            ChangeButtonColor((Button)sender);
        }

        private void btn_delete_EnabledChanged(object sender, EventArgs e)
        {
            ChangeButtonColor((Button)sender);
        }

        private void button_1_Click(object sender, EventArgs e)
        {
            SendCommand(1);
        }

        private void button_2_Click(object sender, EventArgs e)
        {
            SendCommand(2);
        }

        private void button_3_Click(object sender, EventArgs e)
        {
            SendCommand(3);
        }

        private void button_4_Click(object sender, EventArgs e)
        {
            SendCommand(4);
        }

        private void button_5_Click(object sender, EventArgs e)
        {
            SendCommand(5);
        }

        private void button_Send_Click(object sender, EventArgs e)
        {
            string cmd = string.Empty;
            int value  = 0;

            try
            {
                int.TryParse(textBox_cmd.Text, out value);

                if (_configcheck == 0 && _ds.Cmd.ContainsKey(value))
                {
                    _ds.Cmd.TryGetValue(value, out cmd);
                    _client.SendMessage(cmd);
                }
                else 
                    _client.SendMessage(textBox_cmd.Text);
            }
            catch (Exception ex)
            {
                Handler.LogMsg.AddNShow("[CUI] Send Error : " + ex.Message);
            }
        }

        private void radNormal_CheckedChanged(object sender, EventArgs e)
        {
            if (button_STOP.Enabled && radNormal.Checked)
            {
                _client.SendMessage(Command.CmdTmNormal);
            }
        }

        private void radFault_CheckedChanged(object sender, EventArgs e)
        {
            if (button_STOP.Enabled && radFault.Checked)
            {
                _client.SendMessage(Command.CmdTmFault);
            }
        }

        private void btn_SymbolSend_Click(object sender, EventArgs e)
        {
            string cmd        = string.Empty;
            List<string> list = new List<string>();

            for (int j = 0; j < dataGridView_param.RowCount - 1; j++)
            {

                string symbol = dataGridView_param[0, j].Value.ToString();
                string param = dataGridView_param[1, j].Value.ToString();

                if (string.IsNullOrEmpty(param))
                    cmd = symbol;
                else
                    cmd = symbol + "[" + param + "]";

                list.Add(cmd);
            }

            foreach (string eachcmd in list)
            {
                _client.SendMessage(eachcmd);
            }
        }

        private void listBox_Symbol_DoubleClick(object sender, EventArgs e)
        {
            string selected = listBox_Symbol.SelectedItem.ToString();

            if (_listSymbol.Contains(selected))
                return;

            _listSymbol.Add(selected);
            dataGridView_param.Rows.Add(selected, string.Empty);
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (dataGridView_param.SelectedRows.Count != 0)
            {
                foreach (DataGridViewRow item in dataGridView_param.SelectedRows)
                {
                    dataGridView_param.Rows.Remove(item);
                }
            }
            else if (dataGridView_param.SelectedCells.Count != 0)
            {
                int index = dataGridView_param.CurrentCell.RowIndex;
                if (dataGridView_param.Rows.Count > 1)
                    dataGridView_param.Rows.Remove(dataGridView_param.Rows[index]);
            }
        }

        private void button_START_Click(object sender, EventArgs e)
        {
            bool isSuccess = false;

            if (radNormal.Checked)
            {
                isSuccess = _client.SendMessage(Command.CmdStartNormal);
                DoSTARTNormal();
            }
            else
            {
                isSuccess = _client.SendMessage(Command.CmdStartFault);
                DoSTARTFault();
            }
        }
        #endregion Windows Component Events
    }
}
