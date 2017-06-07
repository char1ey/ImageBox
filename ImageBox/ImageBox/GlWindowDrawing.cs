using System.Drawing;

namespace ImageBox
{
    internal sealed partial class GlWindow
    {
        public void DrawImage(GlImage image, float x, float y, float w, float h, float ix, float iy, float iw, float ih)
        {
            if(ix < 0)
            {
                x += -w * ix / iw;
                ix = 0;
            }

            if(ix + iw > image.Width)
            {
                w -= (ix + iw - image.Width) * w / iw;
                iw = image.Width - ix;
            }

            if(iy < 0)
            {
                y += -h * iy / ih;
                iy = 0;
            }

            if(iy + ih > image.Height)
            {
                h -= (iy + ih - image.Height) * h / ih;
                ih = image.Height - iy;
            }

            Gl.UseProgram(image.ShaderProgram);

            Gl.ActiveTexture(Gl.GL_TEXTURE0);
            Gl.BindTexture(Gl.GL_TEXTURE_2D, image.Id);
            Gl.Uniform1(Gl.GetUniformLocation(image.ShaderProgram, "tex"), 0);

            var vert = (uint)Gl.GetAttribLocation(image.ShaderProgram, "vert");
            var tvert = (uint)Gl.GetAttribLocation(image.ShaderProgram, "vertTexCoord");

            var projectionMatrix = new float[16];
            Gl.GetFloat(Gl.GL_PROJECTION_MATRIX, projectionMatrix);
            Gl.UniformMatrix4(Gl.GetUniformLocation(image.ShaderProgram, "projectionMatrix"), 1, false,
                projectionMatrix);
             
            Gl.PolygonMode(Gl.GL_FRONT, Gl.GL_FILL);
            Gl.Color(Color.Transparent);
            Gl.Enable(Gl.GL_TEXTURE_2D);

            Gl.Begin(Gl.GL_QUADS);

                Gl.VertexAttrib2(vert, x, y);
                Gl.VertexAttrib2(tvert, ix/image.Width, iy/image.Height);

                Gl.VertexAttrib2(vert, x, y + h);
                Gl.VertexAttrib2(tvert, ix / image.Width, (iy + ih) / image.Height);

                Gl.VertexAttrib2(vert, x + w, y + h);
                Gl.VertexAttrib2(tvert, (ix + iw) / image.Width, (iy + ih) / image.Height);

                Gl.VertexAttrib2(vert, x + w, y);
                Gl.VertexAttrib2(tvert, (ix + iw) / image.Width, iy / image.Height);

            Gl.End();

            Gl.Disable(Gl.GL_TEXTURE_2D);
            Gl.BindTexture(Gl.GL_TEXTURE_2D, 0);

            Gl.UseProgram(0);

            Gl.Flush();
        }

        public void DrawImage(GlImage image, float x, float y, float width, float height)
        {
            DrawImage(image, x, y, width, height, 0, 0, image.Width, image.Height);
        }

        public void DrawImage(GlImage image, RectangleF r)
        {
            DrawImage(image, r.X, r.Y, r.Width, r.Height);
        }

        public void DrawImage(GlImage image, RectangleF r, RectangleF ir)
        {
            DrawImage(image, r.X, r.Y, r.Width, r.Height, ir.X, ir.Y, ir.Width, ir.Height);
        }

        public void DrawRectangle(Pen pen, RectangleF r)
        {
            Gl.PolygonMode(Gl.GL_FRONT, Gl.GL_LINE);
            Gl.LineWidth(pen.Width);
            Gl.Color(pen.Color);
            Gl.Begin(Gl.GL_QUADS);
            Gl.Vertex(r.Left, r.Top);
            Gl.Vertex(r.Left, r.Bottom);
            Gl.Vertex(r.Right, r.Bottom);
            Gl.Vertex(r.Right, r.Top);
            Gl.End();
        }
    }
}
