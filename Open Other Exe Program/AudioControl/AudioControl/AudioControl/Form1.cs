using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CoreAudioApi;

namespace AudioControl
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        MMDevice device;
        AudioSessionManager audioSessionManager;
        SessionCollection sessions;

        /// <summary>
        /// 开启程序时就执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            //初始化设备
            MMDeviceEnumerator devEnum = new MMDeviceEnumerator();
            device = devEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia);

            //label1.Text = "当前音量：" + Convert.ToInt32(device.AudioEndpointVolume.MasterVolumeLevelScalar * 100.0f); 

            
            //获取所有程序声音的管理器
            audioSessionManager = device.AudioSessionManager;
            sessions = audioSessionManager.Sessions;

            //显示所有程序的SessionIdentifier
            label1.Text ="当前应用音量的个数"+ audioSessionManager.Sessions.Count+
                "。当前所有音频程序的SessionIdentifier:\n";
            for (int i = 0; i < audioSessionManager.Sessions.Count; i++)
            {
                AudioSessionControl app = sessions[i];
                string sIdentifier = app.SessionIdentifier;
                label1.Text += app.SessionIdentifier + "\n";
            }

            
            Thread a = new Thread(CheckTxT);
            a.IsBackground = true;
            a.Start();
            //CheckTxT();
        }

        //检查txt文件用来
        private void CheckTxT()
        {
            while (true)
            {
                if (File.ReadAllText("AppIsMute.txt") == "true")
                {
                    /*
                    Thread b = new Thread(()=> { FindTheApplication(textBox1.Text, true); });
                    b.IsBackground = true;
                    b.Start();*/
                    FindTheApplication(textBox1.Text, true);
                }
                else if(File.ReadAllText("AppIsMute.txt") == "false")
                {
                    /*
                    Thread b = new Thread(() => { FindTheApplication(textBox1.Text, false); });
                    b.IsBackground = true;
                    b.Start();*/
                    FindTheApplication(textBox1.Text, false);
                }

                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// 找到我们要找的程序
        /// </summary>
        /// <param name="exeName">exe程序的名字</param>
        /// <param name="isMute">静音还是不静音</param>
        private void FindTheApplication(string exeName,bool isMute)
        {
            
            //判断这个程序是不是我们要的
            for (int i=0;i< audioSessionManager.Sessions.Count;i++)
            {
                AudioSessionControl app = sessions[i];
                string sIdentifier = app.SessionIdentifier;
                
                if (isTheAppWeNeed(sIdentifier,exeName))
                {
                    //app.SimpleAudioVolume.MasterVolume=0;

                    app.SimpleAudioVolume.Mute = isMute;//设置静音状态
                    if (isMute)
                    {
                        //label3.Text = "静音成功!";
                    }
                    else
                    {
                        //label3.Text = "取消静音成功!";
                    }
                    
                    return;
                }
            }
            //label3.Text = "没有找到该exe程序的音量控制实体";
        }

        /// <summary>
        /// 根据identify判断是不是我们要找的程序
        /// </summary>
        /// <param name="sIdentifier"></param>
        /// <returns></returns>
        private bool isTheAppWeNeed(string sIdentifier,string exeName)
        {
            if (exeName== "") { return false; }
            if(sIdentifier.Contains(exeName+ ".exe"))//判断Identifier中是不是我们需要的程序
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 静音指定的程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button1_Click(object sender, EventArgs e)
        {
            FindTheApplication(textBox1.Text,true);
        }

        /// <summary>
        /// 取消静音指定的程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button2_Click(object sender, EventArgs e)
        {
            FindTheApplication(textBox1.Text, false);
        }
    }
}
