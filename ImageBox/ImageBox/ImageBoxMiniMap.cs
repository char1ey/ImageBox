using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ImageBox
{
    [Designer(typeof(ParentControlDesigner))]
    public partial class ImageBoxMiniMap : Control
    {
        #region constants

        private const int BorderSize = 10;

        #endregion


        #region variables

        private readonly GlWindow m_glWindow;

        private PointF m_previousMousePosition;
        private PointF m_previouViewPosition;

        private int m_borderWidth;
        private Color m_borderColor;

        private readonly ImageBoxWindow m_imageBoxWindow;

        #endregion


        #region properties


        public int BorderWidth
        {
            get { return m_borderWidth; }
            set
            {
                if (value < 0)
                    return;
                m_borderWidth = value;
                Invalidate();
            }
        }

        public Color BorderColor
        {
            get { return m_borderColor; }
            set
            {
                m_borderColor = value;
                Invalidate();
            }
        }

        #endregion


        #region constructors

        internal ImageBoxMiniMap(ImageBoxWindow imageBoxWindow)
        {
            InitializeComponent();
            m_glWindow = new GlWindow(Handle);

            m_glWindow.Begin();
            Gl.ClearColor(Color.WhiteSmoke);
            m_glWindow.End();

            m_imageBoxWindow = imageBoxWindow;
            Win32.wglShareLists(m_imageBoxWindow.GlWindow.Hglrc, m_glWindow.Hglrc);
        }

        #endregion


        #region view methods

        private RectangleF GetImageView()
        {
            RectangleF result;

            var controlRatio = (float)Height / Width;
            var imageRatio = (float)m_imageBoxWindow.GlImage.Height / m_imageBoxWindow.GlImage.Width;

            if (controlRatio < imageRatio)
            {
                var w = (float)m_imageBoxWindow.GlImage.Width * Height / m_imageBoxWindow.GlImage.Height;
                result = new RectangleF((Width - w) / 2, 0, w, Height);

                if (Height > 2 * BorderSize && Width > 2 * BorderSize / imageRatio)
                {
                    result.Height -= 2 * BorderSize;
                    result.Width -= 2 * BorderSize / imageRatio;

                    result.X = Width / 2f - result.Width / 2f;
                    result.Y = Height / 2f - result.Height / 2f;
                }
            }
            else
            {
                var h = (float)m_imageBoxWindow.GlImage.Height * Width / m_imageBoxWindow.GlImage.Width;
                result = new RectangleF(0, (Height - h) / 2, Width, h);

                if (Width > 2 * BorderSize && Height > 2 * BorderSize * imageRatio)
                {
                    result.Width -= 2 * BorderSize;
                    result.Height -= 2 * BorderSize * imageRatio;

                    result.X = Width / 2f - result.Width / 2f;
                    result.Y = Height / 2f - result.Height / 2f;
                }
            }

            return result;
        }

        private RectangleF GetImageRectangle()
        {
            var r = GetImageView();
            var density = r.Width / m_imageBoxWindow.GlImage.Width;
            var result = m_imageBoxWindow.CurrentImageView;

            result.X = result.X * density + r.X;
            result.Y = result.Y * density + r.Y;
            result.Width *= density;
            result.Height *= density;

            return result;
        }

        #endregion


        #region override methods

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (m_imageBoxWindow?.GlImage == null)
                return;

            if (e.Button == MouseButtons.Left)
            {
                var density = m_imageBoxWindow.GlImage.Width / GetImageView().Width;

                m_imageBoxWindow.Move(new PointF(
                    (m_previouViewPosition.X - m_imageBoxWindow.CurrentImageView.Location.X) / m_imageBoxWindow.Density
                    + (e.X - m_previousMousePosition.X) * density / m_imageBoxWindow.Density,
                    (m_previouViewPosition.Y - m_imageBoxWindow.CurrentImageView.Location.Y) / m_imageBoxWindow.Density
                    + (e.Y - m_previousMousePosition.Y) * density / m_imageBoxWindow.Density));
            }
            else
            {
                m_previouViewPosition = m_imageBoxWindow.CurrentImageView.Location;
                m_previousMousePosition = e.Location;
            }

        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (m_imageBoxWindow == null)
                return;

            m_glWindow.Begin();

            m_glWindow.ResizeWindow(Width, Height);
            Gl.MatrixMode(Gl.GL_PROJECTION);
            Gl.LoadIdentity();
            Gl.Ortho2D(0, Width, Height, 0);

            m_glWindow.End();

            base.OnSizeChanged(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (m_imageBoxWindow == null)
                return;

            m_glWindow.Begin();

            Gl.Clear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);

            if (m_imageBoxWindow.GlImage != null)
            {
                m_glWindow.DrawImage(m_imageBoxWindow.GlImage, GetImageView());
                m_glWindow.DrawRectangle(new Pen(BorderColor, BorderWidth), GetImageRectangle());
            }

            m_glWindow.End();

            m_glWindow.SwapBuffers();

            base.OnPaint(e);
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            m_glWindow.Dispose();
            base.OnHandleDestroyed(e);
        }

        #endregion
    }
}
