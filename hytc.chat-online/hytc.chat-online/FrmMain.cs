using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace hytc.chat_online
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string ip=this.txtIp.Text;
            UdpClient uc = new UdpClient();
            string msg = this.txtMsg.Text;
            byte[] bmsg = Encoding.Default.GetBytes(msg);
            IPEndPoint ipep=new IPEndPoint(IPAddress.Parse(ip),9527);
            uc.Send(bmsg,bmsg.Length,ipep);
        }

        private void listen() {
            UdpClient uc = new UdpClient(9527);
            while (true)
            {
                IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 0);
                byte[] bmsg = uc.Receive(ref ipep);
                string msg = Encoding.Default.GetString(bmsg);
                this.txtHistory.Text += msg + "\r\n";
            }
        }
        private void FrmMain_Load(object sender, EventArgs e)
        {
            FrmMain.CheckForIllegalCrossThreadCalls = false;
            Thread th = new Thread(new ThreadStart(listen));
            th.IsBackground = false;
            th.Start();
        }
    }
}
