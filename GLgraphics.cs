using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.IO;


namespace SannikovaVika_Shaders
{
    class GLgraphics  
    {

        int BasicProgramID;
        int BasicVertexShader;
        int BasicFragmentShader;

        float[] positionData = { -0.8f, -0.8f, 0.0f, 0.8f, -0.8f, 0.0f, 0.0f, 0.8f, 0.0f };
        float[] colorData = { 1.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 1.0f };
        int vaoHandle;
        int[] vboHandlers = new int[2];

        private void initBuffers()
        {
            GL.GenBuffers(2, vboHandlers);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboHandlers[0]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(sizeof(float) * positionData.Length),
                                                 positionData, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboHandlers[1]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(sizeof(float) * colorData.Length),
                                                colorData, BufferUsageHint.StaticDraw);


            vaoHandle = GL.GenVertexArray();
            GL.BindVertexArray(vaoHandle);
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboHandlers[0]);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboHandlers[1]);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 0, 0);
        }

        void loadShader(String filename, ShaderType type, int program, out int address)
        {
            address = GL.CreateShader(type);
            using (System.IO.StreamReader sr = new StreamReader(filename))
            {
                GL.ShaderSource(address, sr.ReadToEnd());
            }
            GL.CompileShader(address);
            GL.AttachShader(program, address);
            Console.WriteLine(GL.GetShaderInfoLog(address));
        }

        private void InitShaders()
        {
            // создание объекта программы
            BasicProgramID = GL.CreateProgram();
            loadShader("C:\\Users\\PC_Lenovo\\Documents\\GitHub\\SannikovaVika_Shaders\\basic.vert", ShaderType.VertexShader, BasicProgramID, out BasicVertexShader);
            loadShader("C:\\Users\\PC_Lenovo\\Documents\\GitHub\\SannikovaVika_Shaders\\basic.frag", ShaderType.FragmentShader, BasicProgramID, out BasicFragmentShader);
            //Компановка программы
            GL.LinkProgram(BasicProgramID);
            // Проверить успех компановки
            int status = 0;
            GL.GetProgram(BasicProgramID, GetProgramParameterName.LinkStatus, out status);
            Console.WriteLine(GL.GetProgramInfoLog(BasicProgramID));
        }

        private void drawFigure()
        {
            //GL.Begin(PrimitiveType.Triangles);
            //GL.Vertex3(-1.0f, -1.0f, 0.0f);
            //GL.Vertex3(-1.0f, 1.0f, 0.0f);
            //GL.Vertex3(1.0f, 1.0f, 0.0f);
            //GL.End();
            GL.UseProgram(BasicProgramID);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            GL.UseProgram(0);
        }

        public void Setup(int width, int height)
        {
            GL.ClearColor(Color.DarkGray);
            GL.ShadeModel(ShadingModel.Smooth);

            Matrix4 perspectiveMat = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,  width / (float)height,  1, 64);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspectiveMat);

            Matrix4 viewMat = Matrix4.LookAt(0, 0, 3, 0, 0, 0, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref viewMat);

            InitShaders();
            initBuffers();
        }

        public void Update()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            drawFigure();
        }

    }
}
