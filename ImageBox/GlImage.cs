using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageBox
{
    internal class GlImage: IDisposable
    {
        private readonly uint[] m_tex;
        
        public uint Id { get { return m_tex[0]; } }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public readonly uint ShaderProgram;

        public GlImage(Bitmap bitmap)
        {
            Width = bitmap.Width;
            Height = bitmap.Height;

            m_tex = new uint[1];

            var d = bitmap.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            bitmap.UnlockBits(d);

            Gl.GenTextures(1, m_tex);
            Gl.BindTexture(Gl.GL_TEXTURE_2D, m_tex[0]);

            Gl.TexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGB, Width, Height, 0, Gl.GL_BGR, Gl.GL_UNSIGNED_BYTE, d.Scan0);
            Gl.TexParameter(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_NEAREST);
            Gl.TexParameter(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_NEAREST);
            Gl.BindTexture(Gl.GL_TEXTURE_2D, 0);

            ShaderProgram = Gl.CreateProgram();

            Gl.AttachShader(ShaderProgram, CreateShader(Gl.GL_VERTEX_SHADER, @"
                #version 150
                in vec3 vert;
                in vec2 vertTexCoord;
                out vec2 fragTexCoord;

                void main()
                {
                    fragTexCoord = vertTexCoord;

                    gl_Position = vec4(vert, 1);
                }
            "));
            Gl.AttachShader(ShaderProgram, CreateShader(Gl.GL_FRAGMENT_SHADER, @"
                #version 150
                uniform sampler2D tex; 
                in vec2 fragTexCoord;
                out vec4 finalColor;

                void main() {
                    finalColor = texture(tex, fragTexCoord);
                }
                "));

            Gl.LinkProgram(ShaderProgram);
            var success = new int[1];
            Gl.GetProgram(ShaderProgram, Gl.GL_LINK_STATUS, success);

            Gl.ValidateProgram(ShaderProgram);
            Gl.GetProgram(ShaderProgram, Gl.GL_LINK_STATUS, success);

            Gl.EnableVertexAttribArray((uint)Gl.GetAttribLocation(ShaderProgram, "Vert"));
            Gl.VertexAttribPointer((uint)Gl.GetAttribLocation(ShaderProgram, "Vert"), 3, Gl.GL_FLOAT, false, 3 * sizeof(float), IntPtr.Zero);
            Gl.EnableVertexAttribArray((uint)Gl.GetAttribLocation(ShaderProgram, "vertTexCoord"));
            Gl.VertexAttribPointer((uint)Gl.GetAttribLocation(ShaderProgram, "vertTexCoord"), 2, Gl.GL_FLOAT, false, 2 * sizeof(float), IntPtr.Zero);
            Gl.BindVertexArray(0);
        }

        private static uint CreateShader(uint type, string source)
        {
            var ret = Gl.CreateShader(type);

            Gl.ShaderSource(ret, source);
            Gl.CompileShader(ret);
            var success = new int[1];
            Gl.GetShader(ret, Gl.GL_COMPILE_STATUS, success);
            var str = new StringBuilder(1000);
            Gl.GetShaderInfoLog(ret, 1000, IntPtr.Zero, str);
            return ret;
        }

        public void SetImage(Bitmap bitmap)
        {
            Width = bitmap.Width;
            Height = bitmap.Height;

            var d = bitmap.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            bitmap.UnlockBits(d);

            Gl.BindTexture(Gl.GL_TEXTURE_2D, m_tex[0]);
            Gl.TexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGB, Width, Height, 0, Gl.GL_BGR_EXT, Gl.GL_UNSIGNED_BYTE, d.Scan0);
        }

        public void Dispose()
        {
            Gl.DeleteTextures(1, m_tex);
        }
    }
}
