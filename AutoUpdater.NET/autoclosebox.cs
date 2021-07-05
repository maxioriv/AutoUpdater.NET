using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace AutoUpdaterDotNET
{
    public static class ShowMsg
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string IpClassName, string IpWindowName);

        [DllImport("user32.dll")]
        static extern bool EndDialog(IntPtr hDlg, out IntPtr nResult);
        //三个参数：1、文本提示-text，2、提示框标题-caption，3、按钮类型-MessageBoxButtons ，4、自动消失时间设置-timeout
        public static void ShowMessageBoxTimeout(string text, string caption,
            MessageBoxButtons buttons,  int timeout)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(CloseMessageBox),
                new CloseState(caption, timeout));
            MessageBox.Show(text, caption, buttons,MessageBoxIcon.Information);
        }

        private static void CloseMessageBox(object state)
        {
            CloseState closeState = state as CloseState;

            Thread.Sleep(closeState.Timeout);
            IntPtr dlg = FindWindow(null, closeState.Caption);

            if (dlg != IntPtr.Zero)
            {
                IntPtr result;
                EndDialog(dlg, out result);
            }
        }

    }
    public class CloseState
    {
        private int _Timeout;
        public int Timeout
        {
            get
            {
                return _Timeout;
            }
        }
        private string _Caption;
        /// <summary>
        /// Caption of dialog
        /// </summary>
        public string Caption
        {
            get
            {
                return _Caption;
            }
        }
        public CloseState(string caption, int timeout)
        {
            _Timeout = timeout;
            _Caption = caption;
        }
    }
}
