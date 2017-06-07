namespace ImageBox
{
    partial class ImageBox
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.vScrollBar = new System.Windows.Forms.VScrollBar();
            this.hScrollBar = new System.Windows.Forms.HScrollBar();
            this.imageBoxWindow = new ImageBoxWindow();
            this.imageBoxMiniMap = new ImageBoxMiniMap(this.imageBoxWindow);
            this.SuspendLayout();
            // 
            // vScrollBar
            // 
            this.vScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScrollBar.Location = new System.Drawing.Point(810, 0);
            this.vScrollBar.Name = "vScrollBar";
            this.vScrollBar.Size = new System.Drawing.Size(17, 523);
            this.vScrollBar.TabIndex = 0;
            // 
            // hScrollBar
            // 
            this.hScrollBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hScrollBar.Location = new System.Drawing.Point(0, 523);
            this.hScrollBar.Name = "hScrollBar";
            this.hScrollBar.Size = new System.Drawing.Size(827, 17);
            this.hScrollBar.TabIndex = 1;
            // 
            // imageBoxWindow
            // 
            this.imageBoxWindow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageBoxWindow.Image = null;
            this.imageBoxWindow.Location = new System.Drawing.Point(0, 0);
            this.imageBoxWindow.MouseControl = true;
            this.imageBoxWindow.MouseMoveButton = System.Windows.Forms.MouseButtons.Left;
            this.imageBoxWindow.Name = "imageBoxWindow";
            this.imageBoxWindow.Size = new System.Drawing.Size(810, 523);
            this.imageBoxWindow.TabIndex = 2;
            this.imageBoxWindow.Text = "imageBoxWindow";
            this.imageBoxWindow.ZoomOnWheel = true;
            // 
            // imageBoxMiniMap
            // 
            this.imageBoxMiniMap.BorderColor = System.Drawing.Color.Empty;
            this.imageBoxMiniMap.BorderWidth = 0;
            this.imageBoxMiniMap.Location = new System.Drawing.Point(559, 282);
            this.imageBoxMiniMap.Name = "imageBoxMiniMap";
            this.imageBoxMiniMap.Size = new System.Drawing.Size(248, 238);
            this.imageBoxMiniMap.TabIndex = 3;
            this.imageBoxMiniMap.Text = "imageBoxMiniMap";
            // 
            // ImageBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.imageBoxMiniMap);
            this.Controls.Add(this.imageBoxWindow);
            this.Controls.Add(this.vScrollBar);
            this.Controls.Add(this.hScrollBar);
            this.Name = "ImageBox";
            this.Size = new System.Drawing.Size(827, 540);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.VScrollBar vScrollBar;
        internal System.Windows.Forms.HScrollBar hScrollBar;
        private ImageBoxWindow imageBoxWindow;
        private ImageBoxMiniMap imageBoxMiniMap;
    }
}
