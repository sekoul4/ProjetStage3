using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Timers;

public partial class Form1 : Form
{
    [DllImport("user32.dll")]
    static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    [DllImport("user32.dll", SetLastError = true)]
    static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll", SetLastError = true)]
    static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

    static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
    const uint SWP_NOSIZE = 0x0001;
    const uint SWP_NOMOVE = 0x0002;
    const uint SWP_SHOWWINDOW = 0x0040;

    private System.Timers.Timer aTimer;

    public Form1()
    {
        this.Deactivate += new EventHandler(Form1_Deactivate);
        aTimer = new System.Timers.Timer();
        aTimer.Interval = 10;
        aTimer.Elapsed += OnTimedEvent;
        aTimer.AutoReset = true;
        aTimer.Enabled = true;
    }
    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        SetTopMost();
    }
    private void Form1_Deactivate(object? sender, EventArgs e)
    {
        SetTopMost();
    }
    private void SetTopMost()
    {
        IntPtr keyboardHandle = FindWindow("IPTip_Main_Window", null);
        if (keyboardHandle != IntPtr.Zero)
        {
            SetParent(this.Handle, keyboardHandle);
        }
        this.TopMost = true;
        this.BringToFront();
        SetWindowPos(this.Handle, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);
    }
    private void OnTimedEvent(Object? source, ElapsedEventArgs e)
    {
        SetTopMost();
    }
}
