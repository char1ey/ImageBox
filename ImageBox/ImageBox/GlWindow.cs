using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ImageBox
{
    internal sealed partial class GlWindow : NativeWindow, IDisposable
    {
        private readonly IntPtr m_hdc;
        private readonly IntPtr m_hglrc;

        private IntPtr m_previousHdc;
        private IntPtr m_previousHglrc;

        public IntPtr Hglrc
        {
            get { return m_hglrc; }
        }

        public GlWindow(IntPtr handle)
        {
            var createParams = new CreateParams
            {
                Parent = handle,
                Style = (int)(Win32.WindowStyles.WS_CHILD | Win32.WindowStyles.WS_VISIBLE | Win32.WindowStyles.WS_DISABLED)
            };
            
            CreateHandle(createParams);

            m_hdc = Win32.GetDC(Handle);

            if (m_hdc == null) return;

            var pfd = new Win32.PIXELFORMATDESCRIPTOR
            {
                nSize = (ushort)Marshal.SizeOf<Win32.PIXELFORMATDESCRIPTOR>(),
                nVersion = 1,
                dwFlags = Win32.PFD_DRAW_TO_WINDOW | Win32.PFD_SUPPORT_OPENGL | Win32.PFD_DOUBLEBUFFER,
                iPixelType = Win32.PFD_TYPE_RGBA,
                cColorBits = 32,
                cDepthBits = 24,
                cStencilBits = 8,
                iLayerType = Win32.PFD_MAIN_PLANE
            };
            var pixelFrormat = Win32.ChoosePixelFormat(m_hdc, pfd);
            Win32.SetPixelFormat(m_hdc, pixelFrormat, pfd);
            m_hglrc = Win32.wglCreateContext(m_hdc);
            Win32.wglMakeCurrent(m_hdc, m_hglrc);
        }

        public void ResizeWindow(int width, int height)
        {
            Begin();
            Win32.glViewport(0, 0, width, height);
            End();
            Win32.SetWindowPos(Handle, IntPtr.Zero, 0, 0, width, height, Win32.SetWindowPosFlags.SWP_NOMOVE
                                                                         | Win32.SetWindowPosFlags.SWP_NOZORDER
                                                                         | Win32.SetWindowPosFlags.SWP_NOACTIVATE);
        }

        public void Begin()
        {
            m_previousHdc = Win32.wglGetCurrentDC();
            m_previousHglrc = Win32.wglGetCurrentContext();

            if (m_previousHdc != m_hdc || m_previousHglrc != m_hglrc)
                Win32.wglMakeCurrent(m_hdc, m_hglrc);
        }

        public void End()
        {
            if (m_previousHdc != m_hdc || m_previousHglrc != m_hglrc)
                Win32.wglMakeCurrent(m_previousHdc, m_previousHglrc);
        }

        public void SwapBuffers()
        {
            Win32.SwapBuffers(m_hdc);
        }

        public void Dispose()
        {
            Win32.wglDeleteContext(m_hglrc);
        }
    }
}
