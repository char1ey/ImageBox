using System;
using System.Drawing;
using System.Windows.Forms;

namespace ImageBox
{
    internal partial class ImageBoxWindow : Control
    {
        #region constants

        private const float ZoomSpeed = 0.1f;
        private const int MinImageSize = 8;
        private const float Eps = 1e-9f;

        #endregion
        

        #region variables 

        private Image m_image;
        internal GlImage GlImage;

        private int m_previousControlWidth;
        private int m_previousControlHeight;

        private PointF m_previousMousePosition;
        private PointF m_previousDelta;

        private RectangleF m_currentImageView;
        internal float Density;

        #endregion


        #region properties

        internal RectangleF CurrentImageView
        {
            get { return m_currentImageView; }
        }

        internal GlWindow GlWindow { get; }

        public bool ZoomOnWheel
        {
            get;
            set;
        }

        public bool MouseControl
        {
            get;
            set;
        }

        public MouseButtons MouseMoveButton
        {
            get;
            set;
        }

        private RectangleF ControlView
        {
            get
            {
                return new RectangleF(0, 0, Width, Height);
            }
        }

        public Image Image
        {
            get { return m_image; }
            set
            {
                GlImage?.Dispose();
                m_image?.Dispose();
                m_image = value;

                if (m_image != null)
                {
                    GlWindow.Begin();
                    GlImage = new GlImage((Bitmap) m_image);
                    GlWindow.End();

                    CreateImageView();
                    Invalidate();
                }
            }
        }

        #endregion


        #region constructors

        public ImageBoxWindow()
        {
            InitializeComponent();

            GlWindow = new GlWindow(Handle);

            GlWindow.Begin();
            Gl.ClearColor(Color.WhiteSmoke);
            GlWindow.End();
        }

        #endregion


        #region change view methods

        internal void CreateImageView()
        {
            if (Width == 0 || Height == 0 || GlImage == null)
                return;

            var controlRatio = (float)Height / Width;
            var imageRatio = (float)GlImage.Height / GlImage.Width;

            if (controlRatio < imageRatio)
            {
                var density = (float)GlImage.Height / Height;
                m_currentImageView = new RectangleF(-density * (Width / 2f - GlImage.Width / 2f / density),
                                                    0, Width * density, Height * density);
            }
            else
            {
                var density = (float)GlImage.Width / Width;
                m_currentImageView = new RectangleF(0, -density * (Height / 2f - GlImage.Height / 2f / density),
                                                       Width * density, Height * density);
            }

            if (GlImage.Width < Width && GlImage.Height < Height)
            {
                m_currentImageView.X = -Width / 2f + GlImage.Width / 2f;
                m_currentImageView.Y = -Height / 2f + GlImage.Height / 2f;
                m_currentImageView.Width = Width;
                m_currentImageView.Height = Height;
            }

            Density = m_currentImageView.Width / Width;
        }

        private void FixImageView()
        {
            if (GlImage == null)
                return;

            if (m_currentImageView.Width > GlImage.Width)
                m_currentImageView.X = -(m_currentImageView.Width - GlImage.Width) / 2;
            else if (m_currentImageView.Right > GlImage.Width)
                m_currentImageView.X -= m_currentImageView.Right - GlImage.Width;
            else if (m_currentImageView.Left < 0)
                m_currentImageView.X = 0;

            if (m_currentImageView.Height > GlImage.Height)
                m_currentImageView.Y = -(m_currentImageView.Height - GlImage.Height) / 2;
            else if (m_currentImageView.Bottom > GlImage.Height)
                m_currentImageView.Y -= m_currentImageView.Bottom - GlImage.Height;
            else if (m_currentImageView.Top < 0)
                m_currentImageView.Y = 0;
        }

        public void Zoom(float delta)
        {
            Zoom(delta, new PointF(Width / 2f, Height / 2f));
        }

        public void Zoom(float delta, PointF position)
        {
            if (Width == 0 && Height == 0 || GlImage == null)
                return;

            var dw = (m_currentImageView.Right - m_currentImageView.Left) * ZoomSpeed * delta;
            var dh = (m_currentImageView.Bottom - m_currentImageView.Top) * ZoomSpeed * delta;

            var left = m_currentImageView.Left + dw * position.X / Width;
            var right = m_currentImageView.Right - dw * (1 - position.X / Width);
            var top = m_currentImageView.Top + dh * position.Y / Height;
            var bottom = m_currentImageView.Bottom - dh * (1 - position.Y / Height);

            if (Math.Min(right - left, bottom - top) < MinImageSize)
                return;

            if (Math.Max(right - left, bottom - top) > 4 * Math.Max(GlImage.Height, GlImage.Width))
                return;

            m_currentImageView.X = left;
            m_currentImageView.Y = top;

            m_currentImageView.Width = right - left;
            m_currentImageView.Height = bottom - top;

            if (Width != 0)
                Density = m_currentImageView.Width / Width;
            else if (Height != 0)
                Density = m_currentImageView.Height / Height;

            FixImageView();

            Invalidate();
        }

        public new void Move(PointF d)
        {
            if (GlImage == null)
                return;
            m_currentImageView.X += d.X * Density;
            m_currentImageView.Y += d.Y * Density;
            FixImageView();

            Invalidate();
        }

        #endregion


        #region override methods

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (ZoomOnWheel)
                Zoom((float)e.Delta / SystemInformation.MouseWheelScrollDelta, e.Location);

            base.OnMouseWheel(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (MouseControl)
            {
                if (e.Button == MouseMoveButton && e.Button != MouseButtons.None)
                {
                    Move(new PointF(-m_previousDelta.X + m_previousMousePosition.X - e.Location.X, 
                                    -m_previousDelta.Y + m_previousMousePosition.Y - e.Location.Y));

                    m_previousDelta = new PointF(m_previousMousePosition.X - e.Location.X,
                                                 m_previousMousePosition.Y - e.Location.Y);
                }
                else
                {
                    m_previousDelta = PointF.Empty;
                    m_previousMousePosition = e.Location;
                }
            }

            base.OnMouseMove(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (GlImage != null)
            {
                var dw = (Width - m_previousControlWidth) * Density;
                var dh = (Height - m_previousControlHeight) * Density;

                m_currentImageView.X -= dw / 2f;
                m_currentImageView.Width += dw;

                m_currentImageView.Y -= dh / 2f;
                m_currentImageView.Height += dh;
            }

            GlWindow.Begin();

            GlWindow.ResizeWindow(Width, Height);
            Gl.MatrixMode(Gl.GL_PROJECTION);
            Gl.LoadIdentity();
            Gl.Ortho2D(0, Width, Height, 0);

            GlWindow.End();

            m_previousControlWidth = Width;
            m_previousControlHeight = Height;

            FixImageView();

            base.OnSizeChanged(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (GlImage == null)
                return;

            if (Math.Abs(m_currentImageView.Width) < Eps)
                CreateImageView();

            GlWindow.Begin();

            Gl.Clear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            GlWindow.DrawImage(GlImage, ControlView, m_currentImageView);

            GlWindow.End();
            GlWindow.SwapBuffers();

            base.OnPaint(e);
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            m_image?.Dispose();
            GlImage?.Dispose();
            GlWindow?.Dispose();
            base.OnHandleDestroyed(e);
        }

        #endregion
    }
}
