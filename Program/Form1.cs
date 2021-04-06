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
using System.IO;
using Num = System.Numerics;


namespace Program
{
    public partial class Form1 : Form
    {
        struct dpoint
        {
            public float d;
            public Vector3 n;
        }

        const int step = 1;
        bool loaded = false;
        List<Num.Vector3> points;
        List<Polygon> polygons;
        List<Polygon> activePolygons;
        List<Num.Vector3> originPoints;
        int scaleValue;
        int actionMode;
        char activeAxis;
        bool edgeMode;

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
            GL.Enable(EnableCap.Normalize);
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
            GL.Light(LightName.Light0, LightParameter.Position, new float[4] { 0, 30, 70, 1 });
            GL.Light(LightName.Light0, LightParameter.Diffuse, new float[3] { 1, 1, 1 });

            points = new List<Num.Vector3>();
            polygons = new List<Polygon>();
            activePolygons = new List<Polygon>();
            actionMode = 0;
            scaleValue = 100;
            originPoints = new List<Num.Vector3>();
            edgeMode = true;
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
            if (activeAxis == 'X')
                GL.Color3(Color.Khaki);
            else
                GL.Color3(Color.Blue);            
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(1000, 0, 0);
            GL.End();
            //Oy
            if (activeAxis == 'Y')
                GL.Color3(Color.Khaki);
            else
                GL.Color3(Color.Red);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 1000, 0);
            GL.End();
            //Oz
            if (activeAxis == 'Z')
                GL.Color3(Color.Khaki);
            else
                GL.Color3(Color.Green);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, 1000);
            GL.End();

            GL.Enable(EnableCap.Lighting);
            //Куб
            //Отрисовка активных полигонов
            foreach (Polygon polygon in activePolygons)
                    polygon.Draw(edgeMode, false);
            //Отрисовка неактивных полигонов
            foreach (Polygon polygon in polygons)
                if (!polygon.isActive)
                    polygon.Draw(edgeMode);
            glControl1.SwapBuffers();
        }

        //Метод изменения типа действия
        private void ChangeActionMode(int action)
        {
            textBoxX.Text = "0";
            textBoxY.Text = "0";
            textBoxZ.Text = "0";
            textBoxScale.Text = "100%";
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
                scaleValue = 100;
                foreach (Num.Vector3 vec in points)
                    originPoints.Add(vec);
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

            //Задняя
            polygons.Add(new Polygon(new List<int>() { 0, 1, 2, 3 }, ref points, "Задняя грань"));
            //Передняя
            polygons.Add(new Polygon(new List<int>() { 4, 5, 6, 7 }, ref points, "Передняя грань"));
            /*Праваяя*/
            polygons.Add(new Polygon(new List<int>() { 0, 1, 5, 4 }, ref points, "Правая грань"));
            /*Левая*/
            polygons.Add(new Polygon(new List<int>() { 3, 2, 6, 7 }, ref points, "Левая грань"));
            /*Верхняя*/
            polygons.Add(new Polygon(new List<int>() { 1, 2, 6, 5 }, ref points, "Верхняя грань"));
            /*Нижняя*/
            polygons.Add(new Polygon(new List<int>() { 0, 3, 7, 4 }, ref points, "Нижняя грань"));

            listBoxRefresh();
        }

        private void listBoxRefresh()
        {
            listBox1.Items.Clear();
            for (int i = 0; i < polygons.Count; i++)
                if (polygons[i].Name != null)
                    listBox1.Items.Add(polygons[i].Name);
                else
                    listBox1.Items.Add(i);
        }

        //Выбор фрагмента
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
                {
                    scaleValue += step;
                    textBoxScale.Text = String.Format("{0}%",scaleValue);
                }
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
                //Масштабирование
                else if (actionMode == 3)
                {
                    Transformation.Scale((float)(scaleValue/100.0), originPoints, activePolygons, ref points);
                }
            }
        }

        private void RotateCamera(int value)
        {
            GL.MatrixMode(MatrixMode.Modelview);
            if (activeAxis == 'X')
                GL.Rotate(value, 1, 0, 0);
            else if (activeAxis == 'Y')
                GL.Rotate(value, 0, 1, 0);
            else
                GL.Rotate(value, 0, 0, 1);
        }

        private void TranslateCamera(int value)
        {
            GL.MatrixMode(MatrixMode.Modelview);
            if (activeAxis == 'X')
                GL.Translate(value, 0, 0);
            else if (activeAxis == 'Y')
                GL.Translate(0, value, 0);
            else
                GL.Translate(0, 0, value);
        }

        //Обработка клавиш
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!loaded) return;

            switch (e.KeyCode)
            {
                case (Keys.A): // вращение камеры +
                    {
                        RotateCamera(2);
                        break;
                    }
                case (Keys.D): // вращение камеры -
                    {
                        RotateCamera(-2);
                        break;
                    }
                case (Keys.W): // перемещение камеры +
                    {
                        TranslateCamera(2);
                        break;
                    }
                case (Keys.S): // перемещение камеры -
                    {
                        TranslateCamera(-2);
                        break;
                    }
                case (Keys.D1): // Активация мода перемещения
                    {
                        ChangeActionMode(1);
                        break;
                    }
                case (Keys.D2): // Активация мода вращения
                    {
                        ChangeActionMode(2);
                        break;
                    }
                case (Keys.D3): // Активация мода масштабирования
                    {
                        ChangeActionMode(3);
                        break;
                    }
                case (Keys.X): //Активация оси X
                    {
                        ChangeActionAxis('X');
                        break;
                    }
                case (Keys.Y): //Активация оси Y
                    {
                        ChangeActionAxis('Y');
                        break;
                    }
                case (Keys.Z): //Активация оси Z
                    {
                        ChangeActionAxis('Z');
                        break;
                    }
                case (Keys.Left): //Трансформация объекта +
                    {
                        Transform(-step);
                        break;
                    }
                case (Keys.Right): //Трансформация объекта -
                    {
                        Transform(step);
                        break;
                    }
                case (Keys.N): //Переименование фрагмента
                    {
                        if (activePolygons.Count == 1)
                        {
                            GetNameForm form = new GetNameForm();
                            form.ShowDialog();
                            if (form.DialogResult == DialogResult.OK)
                            {
                                foreach (Polygon p in polygons)
                                    if (p.isActive)
                                        p.Name = form.getFragmentName();
                                listBoxRefresh();
                            }                            
                        }
                        else
                            MessageBox.Show("Выберете только 1 фрагмент", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                case (Keys.M): //Смена режима отображения граней
                    {
                        edgeMode = !edgeMode;
                        break;
                    }
                case (Keys.Q):
                    {
                        GL.MatrixMode(MatrixMode.Modelview);
                        GL.Scale(0.9, 0.9, 0.9);
                        break;
                    }
                case (Keys.E):
                    {
                        GL.MatrixMode(MatrixMode.Modelview);
                        GL.Scale(1.1, 1.1, 1.1);
                        break;
                    }
                default: break;
            }
            glControl1.Invalidate();
        }

        //Включение режима вращения относительно центра координат
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBoxX.Text = "0";
            textBoxY.Text = "0";
            textBoxZ.Text = "0";
        }

        //Включение режима вращения относительно центра фрагмента
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBoxX.Text = "0";
            textBoxY.Text = "0";
            textBoxZ.Text = "0";
        }

        // Чтение из файла
        private void ReadFromFile()
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            
            points.Clear();
            polygons.Clear();
            // чтение карты глубины в двумерный массив
            using (Bitmap n = new Bitmap(openFileDialog1.FileName))
            {
                int DataHeight = n.Size.Height;
                int DataWidth = n.Size.Width;

                dpoint[,] Data = new dpoint[DataHeight, DataWidth];
                for (int i = 0; i < DataHeight; i++)
                    for (int j = 0; j < DataWidth; j++)
                    {
                        float valuePixel = n.GetPixel(i, j).R;
                        if (valuePixel == 255)
                            Data[i, j].d = valuePixel;
                        else
                            Data[i, j].d = -valuePixel/20;
                        Data[i, j].n = Vector3.Zero;
                    }

                // Запись точек объекта в список для дальнейшей работы
                for (int i = 1; i < DataHeight; i++)
                    for (int j = 1; j < DataWidth; j++)
                        if (Data[i, j].d != 255)
                            points.Add(new Num.Vector3(i, j, Data[i, j].d));

                // построение полигонов
                float Qz, Pz;
                for (int i = 1; i < DataHeight - 2; i++)
                    for (int j = 1; j < DataWidth - 2; j++)
                        if (Data[i, j].d != 255 && Data[i + 1, j + 1].d != 255)
                        {
                            // если есть "левый" треуголиник
                            if (Data[i + 1, j].d != 255)
                            {
                                Qz = Data[i + 1, j].d - Data[i, j].d;
                                Pz = Data[i + 1, j + 1].d - Data[i, j].d;
                                Num.Vector3 N = new Num.Vector3(Pz - Qz, Qz, 1);
                                List<int> ind1 = new List<int>();
                                ind1.Add(points.FindIndex(s => s.X == i && s.Y == j));
                                ind1.Add(points.FindIndex(ind1[0], s => s.X == i + 1 && s.Y == j));
                                ind1.Add(ind1[1] + 1);
                                polygons.Add(new Polygon(ind1, ref points, N));
                            }
                            // если есть "правый" треуголиник
                            if (Data[i, j + 1].d != 255)
                            {
                                Qz = Data[i, j + 1].d - Data[i, j].d;
                                Pz = Data[i + 1, j + 1].d - Data[i, j].d;
                                Num.Vector3 N = new Num.Vector3(Qz, Qz - Pz, 1);
                                List<int> ind2 = new List<int>();
                                ind2.Add(points.FindIndex(s => s.X == i && s.Y == j));
                                ind2.Add(points.FindIndex(ind2[0], s => s.X == i + 1 && s.Y == j + 1));
                                ind2.Add(ind2[0] + 1);
                                polygons.Add(new Polygon(ind2, ref points, N));
                            }
                        }

                // сдвиг геометрического центра объекта в начало координат
                //float Xoffset = points.Average(p => p.X);
                //float Yoffset = points.Average(p => p.Y);
                //float Zoffset = points.Average(p => p.Z);
                //for (int i = 0; i < points.Count; i++)
                //    points[i] = new Num.Vector3(points[i].X - Xoffset, points[i].Y - Yoffset, points[i].Z - Zoffset);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReadFromFile();
            listBoxRefresh();
            glControl1.Invalidate();
        }
    }
}