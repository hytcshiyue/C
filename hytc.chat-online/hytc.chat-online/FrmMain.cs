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
            //UdpClient uc = new UdpClient();
            string msg ="PUBLIC|"+ this.txtMsg.Text+"|shiyue";
            //byte[] bmsg = Encoding.Default.GetBytes(msg);
            //IPEndPoint ipep=new IPEndPoint(IPAddress.Parse(ip),9527);
            //uc.Send(bmsg,bmsg.Length,ipep);
            SendMsg(ip, msg);
        }

        private void listen() {
            UdpClient uc = new UdpClient(9527);
            while (true)
            {
                IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 0);
                byte[] bmsg = uc.Receive(ref ipep);
                string scontent = Encoding.Default.GetString(bmsg);
                string[] data = scontent.Split('|');
                if(data[0]=="INROOM")
                {
                    this.txt_person.Text += data[1] + "上线了\r\n";
                }
                if(data[0]=="PUBLIC")
                {
                    int l = data.Count();
                    if(l>3)
                    {
                        this.txtHistory.Text += data[2] + ":\r\n";
                        this.txtHistory.Text += data[2] + "\r\n";
                    }
                }
                
            }
        }
        private void SendMsg(string ipAll, string msgAll)
        {
            UdpClient uc = new UdpClient();
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(ipAll), 9527);
            string inmsg = msgAll;
            byte[] binmsg = Encoding.Default.GetBytes(inmsg);
            uc.Send(binmsg, binmsg.Length, ipep);
            this.txtMsg .Text = "";
        }
        private void FrmMain_Load(object sender, EventArgs e)
        {
            FrmMain.CheckForIllegalCrossThreadCalls = false;
            Thread th = new Thread(new ThreadStart(listen));
            th.IsBackground = false;
            th.Start();
            SendMsg("255.255.255.255","INROOM|shiyue|192.168.1.91");
        }
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            //SendMsg("255.255.255.255", "OUTROOM|冯莹|192.168.1.123");
            Application.Exit();
            //flag = false;
            //th.Abort();

        }
    }
}
