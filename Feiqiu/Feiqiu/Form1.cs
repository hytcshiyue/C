using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Feiqiu
{
    public partial class FrmMain : Form
    {
        public delegate void delAddFriend(Friend friend); 
        public FrmMain()
        {
            InitializeComponent();
        }

      
        private void listen() 
        {
            UdpClient uc = new UdpClient(9527);
            while(true)
            {
                IPEndPoint ipep = new IPEndPoint(IPAddress.Any,0);
                byte[] bmsg = uc.Receive(ref ipep);
                string msg = Encoding.Default.GetString(bmsg);
                //MessageBox.Show(msg);
                string[] datas = msg.Split('|');
                if (datas.Length != 4)
                {
                    continue;
                }
                if(datas[0]=="LOGIN")
                {
                    //MessageBox.Show(datas[1]);
                    Friend friend = new Friend();
                    int curIndex=Convert.ToInt32( datas[2]);
                    if (curIndex<0 || curIndex>this.ilHeadImages.Images.Count) 
                    {
                        curIndex = 0;
                    }
                    friend.HeadImageIndex = curIndex;
                    friend.NickName = datas[1];
                    friend.Shuoshuo = datas[3];
                    object[] pars=new object[1];
                    pars[0] = friend;
                    this.Invoke(new delAddFriend(this.addUcf),pars);
                    
                }

            }
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            FrmMain.CheckForIllegalCrossThreadCalls = false;
            //侦听
            Thread th = new Thread(new ThreadStart(listen));
            Thread.Sleep(100);
            th.IsBackground = true;
            th.Start();

            //for (int i = 0; i < 100;i++ ) 
            //{
            //     UcFriend ucf = new UcFriend();
            //     ucf.Top = i * ucf.Height;
            //    //ucf.Width=;
            //    this.pnFriendsList.Controls.Add(ucf);
            //}
           
            
        }
        public void addUcf(Friend f)
        {
            UcFriend ucf = new UcFriend();
            ucf.Frm=this;
            ucf.CurFriend = f;
            ucf.Top = this.pnFriendsList.Controls.Count * ucf.Height;
            this.pnFriendsList.Controls.Add(ucf);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //发广播
            UdpClient uc = new UdpClient();
            string myNickName = this.txtNickName.Text;
            string msg = "LOGIN|"+myNickName+"|1|大家来找我吧";
            byte[] bmsg = Encoding.Default.GetBytes(msg);
            uc.Send(bmsg, bmsg.Length, new IPEndPoint (IPAddress.Parse("255.255.255.255"),9527));
        }
            
    }
}
