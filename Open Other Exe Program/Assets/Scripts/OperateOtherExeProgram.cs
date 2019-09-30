using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;        //需要引入命名空间
using System.IO;
using System;
using System.Runtime.InteropServices;

public class OperateOtherExeProgram : MonoBehaviour
{
    Process pro;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// 打开exe程序
    /// </summary>
    public void OpenExe()
    {
        //设置exe的相对路径
        string a = Application.dataPath;
        a=a.Replace("Assets",string.Empty);                //剔除掉Assets，在与Assets文件夹同级的目录下寻找
        string path = a+"AudioControl/AudioControl.exe";  //需要打开的exe的路径，根据实际填写

        print(path);
        if (!File.Exists(path)) { UnityEngine.Debug.LogError("没有找到该文件");return; }

        //if (pro!=null) { UnityEngine.Debug.LogWarning("静音程序已经打开，请勿重复操作"); return; }
        pro = new Process();
        pro.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Minimized;     //把窗口最小化，使其在后台运行。也可以选择把窗口隐藏。
        pro.StartInfo.FileName = path;       //设置要打开的exe程序的路径
        pro.Start();       //启动exe程序 
    }

    /// <summary>
    /// 关闭exe程序
    /// </summary>
    public void CloseExe()
    {
        if (pro != null)
        {
            //File.WriteAllText("123.txt","close");
            pro.Kill();                       //杀死所有的进程
            pro.Dispose();               //释放所有的资源
            pro.Close();                  //关闭exe程序
            pro = null;
        }  
    }

    public void Mute()
    {
        File.WriteAllText("./AudioControl/AppIsMute.txt", "true");
    }

    public void NotMute()
    {
        File.WriteAllText("./AudioControl/AppIsMute.txt", "false");
    }
}
