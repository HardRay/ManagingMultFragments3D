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
        const int step = 1;
        bool loaded = false;
        List<Num.Vector3> points;
        List<Polygon> polygons;
        List<Polygon> activePolygons;
        int actionMode;
        char activeAxis;

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

            points = new List<Num.Vector3>();
            polygons = new List<Polygon>();
            activePolygons = new List<Polygon>();
            actionMode = 0;
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
            foreach (Polygon polygon in activePolygons)
                    polygon.Draw();
            //Отрисовка неактивных полигонов
            foreach (Polygon polygon in polygons)
                if (!polygon.isActive)
                    polygon.Draw();
            glControl1.SwapBuffers();
        }

        //Метод изменения типа действия
        private void ChangeActionMode(int action)
        {
            textBoxX.Text = "0";
            textBoxY.Text = "0";
            textBoxZ.Text = "0";
            textBoxScale.Text = "1";
            radioButton1.Visible = false;
            radioButton2.Visible = false;
            if (actionMode == action)
            {
                actionMode = 0;
                textBoxX.Enabled = false;
                textBoxY.Enabled = false;
                textBoxZ.Enabled = false;
            }
            else
            {
                actionMode = action;
                textBoxX.Enabled = true;
                textBoxY.Enabled = true;
                textBoxZ.Enabled = true;
            }
                
            if (actionMode == 3)
            {
                ActionLabel.Text = "Масштабирование";
                textBoxX.Visible = false;
                textBoxY.Visible = false;
                textBoxZ.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                label5.Visible = true;
                textBoxScale.Visible = true;
            }
            else
            {
                textBoxX.Visible = true;
                textBoxY.Visible = true;
                textBoxZ.Visible = true;
                label2.Visible = true;
                label3.Visible = true;
                label4.Visible = true;
                label5.Visible = false;
                textBoxScale.Visible = false;
                if (actionMode == 2)
                {
                    ActionLabel.Text = "Вращение";
                    radioButton1.Visible = true;
                    radioButton2.Visible = true;
                }   
                else if (actionMode == 1)
                    ActionLabel.Text = "Перемещение";
                else
                    ActionLabel.Text = "Выделение";
            }
        }

        private void ChangeActionAxis(char axis)
        {
            activeAxis = axis;
            label2.ForeColor = Color.Black;
            label3.ForeColor = Color.Black;
            label4.ForeColor = Color.Black;
            if (activeAxis == 'X')
                label2.ForeColor = Color.Firebrick;
            else if (activeAxis == 'Y')
                label3.ForeColor = Color.Firebrick;
            else
                label4.ForeColor = Color.Firebrick;
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
            activePolygons.Clear();
            for (int i = 0; i < listBox1.Items.Count; i++)
                if (listBox1.GetSelected(i))
                {
                    polygons[i].Select();
                    activePolygons.Add(polygons[i]);
                }
                else
                    polygons[i].NotSelect();
            glControl1.Invalidate();
        }

        //Метод обработки изменений
        private void Transform(int step)
        {
            if (actionMode != 0 && activeAxis != '\0')
            {
                //Изменение значения в поле "Масштаб"
                if (actionMode == 3)
                    textBoxScale.Text = Convert.ToString(Convert.ToInt32(textBoxScale.Text) + step);
                //Изменение значения в координатных полях
                else
                {
                    Control textbox = this.Controls.Find("textBox" + activeAxis, false).First();
                    textbox.Text = Convert.ToString(Convert.ToInt32(textbox.Text) + step);
                }
                //Перемещение
                if (actionMode == 1)
                    Transformation.Translate(activeAxis, step, activePolygons, ref points);
                //Вращение
                else if (actionMode == 2)
                {
                    bool RotateMode = false;
                    if (radioButton1.Checked)
                        RotateMode = false;
                    else if (radioButton2.Checked)
                        RotateMode = true;
                    Transformation.Rotate(activeAxis, (float)(step * Math.PI / 180), activePolygons, ref points, RotateMode);
                }
                    
            }
        }

        //Обработка клавиш
        private void Form1_KeyDown(object sender, KeyEventArgs e)
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
                //case (Keys.S):
                //    {
                //        GL.MatrixMode(MatrixMode.Modelview);
                //        GL.Rotate(-2, 1, 0, 0);
                //        break;
                //    }
                case (Keys.G):
                    {
                        ChangeActionMode(1);
                        break;
                    }
                case (Keys.R):
                    {
                        ChangeActionMode(2);
                        break;
                    }
                case (Keys.S):
                    {
                        ChangeActionMode(3);
                        break;
                    }
                case (Keys.X):
                    {
                        ChangeActionAxis('X');
                        break;
                    }
                case (Keys.Y):
                    {
                        ChangeActionAxis('Y');
                        break;
                    }
                case (Keys.Z):
                    {
                        ChangeActionAxis('Z');
                        break;
                    }
                case (Keys.Left):
                    {
                        Transform(-step);
                        break;
                    }
                case (Keys.Right):
                    {
                        Transform(step);
                        break;
                    }
                default: break;
            }
            glControl1.Invalidate();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBoxX.Text = "0";
            textBoxY.Text = "0";
            textBoxZ.Text = "0";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBoxX.Text = "0";
            textBoxY.Text = "0";
            textBoxZ.Text = "0";
        }
    }
}