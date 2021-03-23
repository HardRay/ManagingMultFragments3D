using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Program
{
    public partial class Form1 : Form
    {
        bool loaded = false;
        public Form1()
        {
            InitializeComponent();
        }

        double k = 0, c = 0;

        private void glControl1_Load(object sender, EventArgs e)
        {
            loaded = true;
            GL.ClearColor(Color.White);
            GL.Enable(EnableCap.DepthTest);
            Matrix4 p = Matrix4.CreatePerspectiveFieldOfView((float)(80 * Math.PI / 180), 1, 20, 500);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref p);

            Matrix4 modelview = Matrix4.LookAt(100, 100, 100, 0, 0, 0, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
            GL.Light(LightName.Light0, LightParameter.Position, new float[4] { 150, 150, 150, 1 });
            GL.Light(LightName.Light0, LightParameter.Diffuse, new float[3] { 1, 1, 1 });
            GL.Enable(EnableCap.Light1);
            GL.Light(LightName.Light1, LightParameter.Position, new float[4] { 50, 0, 0, 1 });
            GL.Light(LightName.Light1, LightParameter.Diffuse, new float[3] { 1, 1, 1 });
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (!loaded)
                return;
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Color3(Color.Black);
            GL.Disable(EnableCap.Lighting);
            GL.Begin(BeginMode.Lines);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(150, 0, 0);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 150, 0);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, 150);
            GL.End();
            GL.Enable(EnableCap.Lighting);

            int height = 25;
            double radius = 5;

            GL.PushMatrix();
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.AmbientAndDiffuse, Color.Chocolate);
            GL.Translate(50, 30, 50);
            GL.Rotate(k, 0, 1, 0);
            GL.Rotate(c, 1, 0, 0);
            GL.Begin(BeginMode.QuadStrip);
            double angle = 0;
            while (angle < 2 * Math.PI)
            {
                double xt = radius * Math.Cos(angle);
                double yt = radius * Math.Sin(angle);
                GL.Vertex3(xt, yt, height);
                GL.Vertex3(xt, yt, 0.0);
                angle = angle + 0.1;
            }
            GL.Vertex3(radius, 0.0, height);
            GL.Vertex3(radius, 0.0, 0.0);
            GL.End();

            GL.Begin(BeginMode.Polygon);
            angle = 0.0;
            while (angle < 2 * Math.PI)
            {
                double xt = radius * Math.Cos(angle);
                double yt = radius * Math.Sin(angle);
                GL.Vertex3(xt, yt, height);
                angle = angle + 0.1;
            }
            GL.Vertex3(radius, 0.0, height);
            GL.End();
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Disable(EnableCap.Lighting);
            GL.Translate(0, 30, 50);
            GL.Scale(0.8, 0.8, 0.8);
            Matrix4 matrix = new Matrix4( 0, 0, 0, 0, 0, 1, 0, 0,0, 0, 1, 0,0, 0, 0, 1);
            GL.MultMatrix(ref matrix);
            GL.Rotate(k, 0, 1, 0);
            GL.Rotate(c, 1, 0, 0);
            GL.Begin(BeginMode.QuadStrip);
            angle = 0;
            while (angle < 2 * Math.PI)
            {
                double xt = radius * Math.Cos(angle);
                double yt = radius * Math.Sin(angle);
                GL.Vertex3(xt, yt, height);
                GL.Vertex3(xt, yt, 0.0);
                angle = angle + 0.1;
            }
            GL.Vertex3(radius, 0.0, height);
            GL.Vertex3(radius, 0.0, 0.0);
            GL.End();

            GL.Begin(BeginMode.Polygon);
            angle = 0.0;
            while (angle < 2 * Math.PI)
            {
                double xt = radius * Math.Cos(angle);
                double yt = radius * Math.Sin(angle);
                GL.Vertex3(xt, yt, height);
                angle = angle + 0.1;
            }
            GL.Vertex3(radius, 0.0, height);
            GL.End();
            //glEnable(GL_LIGHT0)
            GL.Enable(EnableCap.Lighting);
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Disable(EnableCap.Lighting);
            GL.Translate(50, 30, 0);
            GL.Scale(0.8, 0.8, 0.8);
            matrix = new Matrix4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);
            GL.MultMatrix(ref matrix);
            GL.Rotate(k, 0, 1, 0);
            GL.Rotate(c, 1, 0, 0);
            GL.Begin(BeginMode.QuadStrip);
            angle = 0;
            while (angle < 2 * Math.PI)
            {
                double xt = radius * Math.Cos(angle);
                double yt = radius * Math.Sin(angle);
                GL.Vertex3(xt, yt, height);
                GL.Vertex3(xt, yt, 0.0);
                angle = angle + 0.1;
            }
            GL.Vertex3(radius, 0.0, height);
            GL.Vertex3(radius, 0.0, 0.0);
            GL.End();

            GL.Begin(BeginMode.Polygon);
            angle = 0.0;
            while (angle < 2 * Math.PI)
            {
                double xt = radius * Math.Cos(angle);
                double yt = radius * Math.Sin(angle);
                GL.Vertex3(xt, yt, height);
                angle = angle + 0.1;
            }
            GL.Vertex3(radius, 0.0, height);
            GL.End();
            //glEnable(GL_LIGHT0)
            GL.Enable(EnableCap.Lighting);
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.AmbientAndDiffuse, Color.Aqua);
            GL.Begin(BeginMode.Polygon);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(1000, 0, 0);
            GL.Vertex3(1000, 1000, 0);
            GL.Vertex3(0, 1000, 0);
            GL.End();
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Begin(BeginMode.Polygon);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 1000, 0);
            GL.Vertex3(0, 1000, 1000);
            GL.Vertex3(0, 0, 1000);
            GL.End();
            GL.PopMatrix();
            glControl1.SwapBuffers();
        }

        private void glControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!loaded) return;

            if (e.KeyCode == Keys.S)
            {
                GL.MatrixMode(MatrixMode.Modelview);
                GL.Rotate(-1, 0, 1, 0);
            }
            if (e.KeyCode == Keys.A)
            {
                GL.MatrixMode(MatrixMode.Modelview);
                GL.Rotate(1, 0, 1, 0);
            }
            if (e.KeyCode == Keys.Z)
                k--;
            if (e.KeyCode == Keys.X)
                k++;
            if (e.KeyCode == Keys.Q)
                GL.Scale(1.1, 1.1, 1.1);
            if (e.KeyCode == Keys.W)
                GL.Scale(0.9,0.9,0.9);
            if (e.KeyCode == Keys.D)
            {
                c--;
            }
            if (e.KeyCode == Keys.F)
            {
                c++;
            }
            glControl1.Invalidate();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
        }
    }
}
