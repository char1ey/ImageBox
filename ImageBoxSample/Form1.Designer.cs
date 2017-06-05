namespace ImageBoxSample
{
    partial class Form1
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

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.imageBox1 = new ImageBox.ImageBox();
            this.imageBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageBox1
            // 
            this.imageBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageBox1.HorizontalScrollBar = false;
            this.imageBox1.Image = ((System.Drawing.Image)(resources.GetObject("imageBox1.Image")));
            this.imageBox1.Location = new System.Drawing.Point(0, 0);
            // 
            // imageBox1.MiniMap
            // 
            this.imageBox1.MiniMap.BorderColor = System.Drawing.Color.Red;
            this.imageBox1.MiniMap.BorderWidth = 2;
            this.imageBox1.MiniMap.Location = new System.Drawing.Point(473, 194);
            this.imageBox1.MiniMap.Name = "MiniMap";
            this.imageBox1.MiniMap.Size = new System.Drawing.Size(154, 156);
            this.imageBox1.MiniMap.TabIndex = 3;
            this.imageBox1.MiniMap.Text = "imageBoxMiniMap";
            this.imageBox1.Name = "imageBox1";
            this.imageBox1.Size = new System.Drawing.Size(639, 362);
            this.imageBox1.TabIndex = 0;
            this.imageBox1.VerticalScrollBar = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(639, 362);
            this.Controls.Add(this.imageBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.imageBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ImageBox.ImageBox imageBox1;
    }
}

