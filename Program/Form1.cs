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
using Num = System.Numerics;


namespace Program
{
    public partial class Form1 : Form
    {
        bool loaded = false;
        List<Num.Vector3> points;
        List<Polygon> polygons;

        public Form1()
        {
            InitializeComponent();
        }
        private void glControl1_Load(object sender, EventArgs e)
        {
            loaded = true;
            GL.ClearColor(Color.FromArgb(60,60,60));
            GL.Enable(EnableCap.DepthTest);
            Matrix4 p = Matrix4.CreatePerspectiveFieldOfView((float)(80 * Math.PI / 180), 1, 20, 500);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref p);
            Matrix4 modelview = Matrix4.LookAt(70, 70, 70, 0, 0, 0, 0, 0, 1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
            GL.Light(LightName.Light0, LightParameter.Position, new float[4] { 0, 0, 100, 1 });
            GL.Light(LightName.Light0, LightParameter.Diffuse, new float[3] { 1, 1, 1 });

            //Нужно перенести в какой-нибудь другой Load
            points = new List<Num.Vector3>();
            polygons = new List<Polygon>();
            CubeInit();
        }

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (!loaded)
                return;
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            //Оси
            GL.Disable(EnableCap.Lighting);
            //Ox
            GL.Color3(Color.Blue);            
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(1000, 0, 0);
            GL.End();
            //Oy
            GL.Color3(Color.Red);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 1000, 0);
            GL.End();
            //Oz
            GL.Color3(Color.Green);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, 1000);
            GL.End();

            GL.Enable(EnableCap.Lighting);
            //Куб
            //Отрисовка активных полигонов
            foreach (Polygon polygon in polygons)
                if(polygon.isActive)
                    polygon.Draw();
            //Отрисовка неактивных полигонов
            foreach (Polygon polygon in polygons)
                if (!polygon.isActive)
                    polygon.Draw();
            glControl1.SwapBuffers();
        }

        private void glControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!loaded) return;

            switch (e.KeyCode)
            {
                case (Keys.A):
                    {
                        GL.MatrixMode(MatrixMode.Modelview);
                        GL.Rotate(2, 0, 0, 1);
                        break;
                    }
                case (Keys.D):
                    {
                        GL.MatrixMode(MatrixMode.Modelview);
                        GL.Rotate(-2, 0, 0, 1);
                        break;
                    }
                case (Keys.W):
                    {
                        GL.MatrixMode(MatrixMode.Modelview);
                        GL.Rotate(2, 1, 0, 0);
                        break;
                    }
                case (Keys.S):
                    {
                        GL.MatrixMode(MatrixMode.Modelview);
                        GL.Rotate(-2, 1, 0, 0);
                        break;
                    }
                default: break;
            }

            glControl1.Invalidate();
        }

        private void glControl1_MouseMove(object sender, MouseEventArgs e)
        {

            glControl1.Invalidate();
        }

        //Инициализация куба
        private void CubeInit()
        {
            float width = 20;
            points.Add(new Num.Vector3(0, 0, 0)); //0
            points.Add(new Num.Vector3(0, 0, width)); //1
            points.Add(new Num.Vector3(width, 0, width)); //2
            points.Add(new Num.Vector3(width, 0, 0)); //3
            points.Add(new Num.Vector3(0, width, 0)); //4
            points.Add(new Num.Vector3(0, width, width)); //5
            points.Add(new Num.Vector3(width, width, width)); //6
            points.Add(new Num.Vector3(width, width, 0)); //7

            //нижняя
            polygons.Add(new Polygon(new List<int>() { 0, 1, 2, 3 }, ref points));
            //верхняя
            polygons.Add(new Polygon(new List<int>() { 4, 5, 6, 7 }, ref points));
            /*левая*/
            polygons.Add(new Polygon(new List<int>() { 0, 1, 5, 4 }, ref points));
            /*правая*/
            polygons.Add(new Polygon(new List<int>() { 3, 2, 6, 7 }, ref points));
            /*передняя*/
            polygons.Add(new Polygon(new List<int>() { 1, 2, 6, 5 }, ref points));
            /*задняя*/
            polygons.Add(new Polygon(new List<int>() { 0, 3, 7, 4 }, ref points));

            for (int i = 0; i < polygons.Count; i++)
                listBox1.Items.Add(i);
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i<listBox1.Items.Count; i++)
                if (listBox1.GetSelected(i))
                    polygons[i].Select();
                else
                    polygons[i].NotSelect();
            glControl1.Invalidate();
        }
    }
}