using System;
using System.Runtime.InteropServices;

public class MyProcess
{
    private bool haveMainWindow = false;
    private IntPtr mainWindowHandle = IntPtr.Zero;
    private int processId = 0;

    public delegate bool EnumThreadWindowsCallback(IntPtr hWnd, IntPtr lParam);
    [DllImport("user32.dll", EntryPoint = "ShowWindow", CharSet = CharSet.Auto)]
    public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);
    public IntPtr GetMainWindowHandle(int processId)
    {
        if (!this.haveMainWindow)
        {
            this.mainWindowHandle = IntPtr.Zero;
            this.processId = processId;
            EnumThreadWindowsCallback callback = new EnumThreadWindowsCallback(this.EnumWindowsCallback);
            EnumWindows(callback, IntPtr.Zero);
            GC.KeepAlive(callback);

            this.haveMainWindow = true;
        }
        return this.mainWindowHandle;
    }

    private bool EnumWindowsCallback(IntPtr handle, IntPtr extraParameter)
    {
        int num;
        GetWindowThreadProcessId(new HandleRef(this, handle), out num);
        if ((num == this.processId) && this.IsMainWindow(handle))
        {
            this.mainWindowHandle = handle;
            return false;
        }
        return true;
    }

    private bool IsMainWindow(IntPtr handle)
    {
        return (!(GetWindow(new HandleRef(this, handle), 4) != IntPtr.Zero) && IsWindowVisible(new HandleRef(this, handle)));
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern bool EnumWindows(EnumThreadWindowsCallback callback, IntPtr extraData);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern int GetWindowThreadProcessId(HandleRef handle, out int processId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    public static extern IntPtr GetWindow(HandleRef hWnd, int uCmd);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern bool IsWindowVisible(HandleRef hWnd);
}