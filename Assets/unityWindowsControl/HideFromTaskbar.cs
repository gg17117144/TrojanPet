using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace unityWindowsControl
{
    public class HideFromTaskbar : MonoBehaviour
    {
        const int GWL_EXSTYLE = -20;
        const int GWL_STYLE = -16;
        const int WS_EX_TOOLWINDOW = 0x00000080;
        const int WS_EX_NOACTIVATE = 0x08000000;
        const int WS_EX_APPWINDOW = 0x00040000;
        const int WS_POPUP = unchecked((int)0x80000000);

        [DllImport("user32.dll")]
        static extern IntPtr GetActiveWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        void Awake()
        {
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
            Debug.Log("編輯狀態不會跑");
            IntPtr hwnd = GetActiveWindow();

            // 設定為 ToolWindow 且不顯示在任務列，也不出現在 ALT+TAB
            int exStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            exStyle = (exStyle & ~WS_EX_APPWINDOW) | WS_EX_TOOLWINDOW | WS_EX_NOACTIVATE;
            SetWindowLong(hwnd, GWL_EXSTYLE, exStyle);

            // 把視窗變成 Popup Window（無標題列）
            int style = GetWindowLong(hwnd, GWL_STYLE);
            style = (style & ~0xCF0000) | WS_POPUP;
            SetWindowLong(hwnd, GWL_STYLE, style);
#endif
        }
    }
}