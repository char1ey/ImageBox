using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ImageBox
{
    [Designer(typeof(MiniMapControlDesigner))]
    public partial class ImageBox : UserControl
    {
        #region variables

        private bool m_enableVerticalScrollBar;
        private bool m_enableHorizontalScrollBar;

        #endregion


        #region properties

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ImageBoxMiniMap MiniMap
        {
            get { return imageBoxMiniMap; }
        }

        public bool VerticalScrollBar
        {
            get { return m_enableVerticalScrollBar; }
            set
            {
                m_enableVerticalScrollBar = value;
                ConfigureVerticalScrollBar();
            }
        }

        public bool HorizontalScrollBar
        {
            get { return m_enableHorizontalScrollBar; }
            set
            {
                m_enableHorizontalScrollBar = value;
                ConfigureHorizontalScrollBar();
            }
        }

        public Image Image
        {
            get { return imageBoxWindow.Image; }
            set { imageBoxWindow.Image = value; }
        }

        #endregion


        #region constructors

        public ImageBox()
        {
            InitializeComponent();
          
            imageBoxWindow.Paint += ImageBoxWindow_Paint;
            vScrollBar.Scroll += VerticalScrollBarChanged;
            hScrollBar.Scroll += HorizontalScrollBarChanged;
        }


        #endregion


        #region events
        
        private void ImageBoxWindow_Paint(object sender, PaintEventArgs e)
        {
            UpdateVerticalScrollBar();
            UpdateHorizontalScrollBar();

            imageBoxMiniMap.Invalidate();

            Update();
        }

        private void VerticalScrollBarChanged(object sender, ScrollEventArgs e)
        {
            imageBoxWindow.Move(new PointF(0, (e.NewValue - imageBoxWindow.CurrentImageView.Y)/ imageBoxWindow.Density));
        }

        private void HorizontalScrollBarChanged(object sender, ScrollEventArgs e)
        {
            imageBoxWindow.Move(new PointF((e.NewValue - imageBoxWindow.CurrentImageView.X)/ imageBoxWindow.Density, 0));
        }

        private void imageBoxWindow_Layout(object sender, LayoutEventArgs e)
        {
            imageBoxWindow.CreateImageView();
        }

        #endregion


        #region visual

        private void ConfigureHorizontalScrollBar()
        {
            if (hScrollBar != null)
                hScrollBar.Visible = HorizontalScrollBar;

            OnSizeChanged(null);
            UpdateHorizontalScrollBar();
        }

        private void ConfigureVerticalScrollBar()
        {
            if (vScrollBar != null)
                vScrollBar.Visible = VerticalScrollBar;

            OnSizeChanged(null);
            UpdateVerticalScrollBar();
        }

        private void UpdateVerticalScrollBar()
        {
            if (imageBoxWindow.GlImage == null)
                return;

            if (imageBoxWindow.CurrentImageView.Height >= imageBoxWindow.GlImage.Height)
            {
                vScrollBar.LargeChange = 1;
                vScrollBar.Maximum = 0;
                return;
            }

            vScrollBar.Maximum = imageBoxWindow.GlImage.Height;

            vScrollBar.LargeChange = Math.Max(0, (int)imageBoxWindow.CurrentImageView.Height);
            vScrollBar.Value = Math.Max(0, (int)imageBoxWindow.CurrentImageView.Y);
        }

        private void UpdateHorizontalScrollBar()
        {
            if (imageBoxWindow.GlImage == null)
                return;

            if (imageBoxWindow.CurrentImageView.Width >= imageBoxWindow.GlImage.Width)
            {
                hScrollBar.LargeChange = 1;
                hScrollBar.Maximum = 0;
                return;
            }
           
            hScrollBar.Maximum = imageBoxWindow.GlImage.Width;

            hScrollBar.LargeChange = (int)imageBoxWindow.CurrentImageView.Width;
            hScrollBar.Value = (int)imageBoxWindow.CurrentImageView.X;
        }

        #endregion
    }
}
