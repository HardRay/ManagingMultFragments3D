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
        bool flag = false;
        List<Num.Vector3> points;
        List<Polygon> polygons;

        public Form1()
        {
            InitializeComponent();
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            loaded = true;
            GL.ClearColor(Color.DarkMagenta);
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

            int x = OpenTK.Input.Mouse.GetState().X;
            int y = OpenTK.Input.Mouse.GetState().Y;
            int z = OpenTK.Input.Mouse.GetState().Wheel;
            GL.Color3(Color.White);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(x, y, z);
            GL.End();

            GL.Enable(EnableCap.Lighting);
            int q = 0;
            //Куб
            foreach (Polygon polygon in polygons)
            {
                GL.PushName(q++);
                //Полигоны
                GL.Material(MaterialFace.FrontAndBack, MaterialParameter.AmbientAndDiffuse, polygon.PolygonColor);
                GL.Begin(PrimitiveType.Polygon);
                foreach (int i in polygon.Points)
                    GL.Vertex3(points[i].X, points[i].Y, points[i].Z);
                GL.End();
                //Рёбра
                GL.Disable(EnableCap.Lighting);
                GL.Color3(Color.Black);
                GL.Begin(PrimitiveType.LineLoop);
                foreach (int i in polygon.Points)
                    GL.Vertex3(points[i].X, points[i].Y, points[i].Z);
                GL.End();
                GL.Enable(EnableCap.Lighting);
                GL.PopName();
            }
            glControl1.SwapBuffers();
            
        }

        private void glControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!loaded) return;
            string s = "";

            switch (e.KeyCode)
            {
                case (Keys.A):
                    {
                        GL.MatrixMode(MatrixMode.Modelview);
                        GL.Rotate(2, 0, 1, 0);
                        s += 'A';
                        break;
                    }
                case (Keys.D):
                    {
                        GL.MatrixMode(MatrixMode.Modelview);
                        GL.Rotate(-2, 0, 1, 0);
                        s += 'D';
                        break;
                    }
                case (Keys.W):
                    {
                        //GL.MatrixMode(MatrixMode.Modelview);
                        //GL.Rotate(2, 1, 0, 0);
                        //s += 'A';
                        flag = true;
                        break;
                    }
                case (Keys.S):
                    {
                        //GL.MatrixMode(MatrixMode.Modelview);
                        //GL.Rotate(-2, 1, 0, 0);
                        //s += 'D';
                        flag = false;
                        break;
                    }
                default: break;
            }

            textBox1.Text = s;
            glControl1.Invalidate();
        }

        private void glControl1_MouseMove(object sender, MouseEventArgs e)
        {
            textBox2.Text = Convert.ToString(e.Y);
            int x = OpenTK.Input.Mouse.GetState().X;
            int y = OpenTK.Input.Mouse.GetState().Y;
            textBox3.Text = Convert.ToString(x);
            textBox4.Text = Convert.ToString(y);
            //textBox5.Text = Convert.ToString();
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
            polygons.Add(new Polygon(new List<int>() { 0, 1, 2, 3 }));
            //верхняя
            polygons.Add(new Polygon(new List<int>() { 4, 5, 6, 7 }));
            /*левая*/
            polygons.Add(new Polygon(new List<int>() { 0, 1, 5, 4 }));
            /*правая*/
            polygons.Add(new Polygon(new List<int>() { 3, 2, 6, 7 }));
            /*передняя*/
            polygons.Add(new Polygon(new List<int>() { 1, 2, 6, 5 }));
            /*задняя*/
            polygons.Add(new Polygon(new List<int>() { 0, 3, 7, 4 }));
        }

        //int RetrieveObjectID(int x, int y)
        //{
        //    int objectsFound = 0;   // Общее количество кликнутых обьектов
        //    int[] viewportCoords = new int[4];    // Массив для хранения экранных координат
        //    // Переменная для хранения ID обьектов, на которые мы кликнули.
        //    // Мы делаем массив в 32 элемента, т.к. OpenGL также сохраняет другую
        //    // информацию, которая нам сейчас не нужна. Для каждого обьекта нужно
        //    // 4 слота.
        //    uint[] selectBuffer = new uint[32];

        //    // glSelectBuffer регистрирует массив как буфер выбора обьектов. Первый параметр — размер
        //    // массива. Второй — сам массив для хранения информации.

        //    GL.SelectBuffer(32, selectBuffer);   // Регистрируем буфер для хранения выбранных обьектов

        //    // Эта функция возвращает информацию о многих вещах в OpenGL. Мы передаём GL_VIEWPOR,
        //    // чтобы получить координаты экрана. Функция сохранит их в переданном вторым параметром массиве
        //    // в виде top,left,bottom,right.
        //    GL.GetInteger(GetPName.Viewport, viewportCoords); // Получаем текущие координаты экрана
        //    // Теперь выходим из матрицы GL_MODELVIEW и переходим в матрицу GL_PROJECTION.
        //    // Это даёт возможность использовать X и Y координаты вместо 3D.
           
        //    GL.MatrixMode(MatrixMode.Projection);    // Переходим в матрицу проекции

        //    GL.PushMatrix();         // Переходим в новые экранные координаты

        //    // Эта функция делает так, что фреймбуфер не изменяется при рендере в него, вместо этого
        //    // происходит запись имён (ID) примитивов, которые были бы отрисованы при режиме
        //    // GL_RENDER. Информация помещается в selectBuffer.

        //    GL.RenderMode(RenderingMode.Select);    // Позволяет рендерить обьекты без изменения фреймбуфера

        //    GL.LoadIdentity();       // Сбросим матрицу проекции

        //    // gluPickMatrix позволяет создавать матрицу проекции около нашего курсора. Проще говоря,
        //    // рендерится только область, которую мы укажем (вокруг курсора). Если обьект рендерится
        //    // в этой области, его ID сохраняется (Вот он, смысл всей функции).
        //    // Первые 2 параметра — X и Y координаты начала, следующие 2 — ширина и высота области
        //    // отрисовки. Последний параметр — экранные координаты. Заметьте, мы вычитаем ‘y’ из
        //    // НИЖНЕЙ экранной координаты. Мы сделали это, чтобы перевернуть Y координаты.
        //    // В 3д-пространстве нулевые y-координаты начинаются внизу, а в экранных координатах
        //    // 0 по y находится вверху. Также передаём регион 2 на 2 пиксела для поиска в нём обьекта.
        //    // Это может быть изменено как вам удобнее.

        //    GL.Matr(x, viewportCoords[3] — y, 2, 2, viewportCoords);

        //    // Далее просто вызываем нашу нормальную функцию gluPerspective, точно так же, как
        //    // делали при инициализации.

        //    gluPerspective(45.0f, (float)g_rRect.right / (float)g_rRect.bottom, 0.1f, 150.0f);

        //    glMatrixMode(GL_MODELVIEW); // Возвращаемся в матрицу GL_MODELVIEW

        //    RenderScene();          // Теперь рендерим выбранную зону для выбора обьекта

        //    // Если мы вернёмся в нормальный режим рендеринга из режима выбора, glRenderMode
        //    // возвратит число обьектов, найденных в указанном регионе (в gluPickMatrix()).

        //    objectsFound = GL.RenderMode(RenderingMode.Render); // Вернемся в режим отрисовки и получим число обьектов

        //    glMatrixMode(GL_PROJECTION);    // Вернемся в привычную матрицу проекции
        //    glPopMatrix();              // Выходим из матрицы

        //    glMatrixMode(GL_MODELVIEW);     // Вернемся в матрицу GL_MODELVIEW

        //    // УФФ! Это было немного сложно. Теперь нам нужно выяснить ID выбранных обьектов.
        //    // Если они есть — objectsFound должно быть как минимум 1.

        //    if (objectsFound > 0)
        //    {
        //        // Если мы нашли более 1 обьекта, нужно проверить значения глубины всех
        //        // выбоанных обьектов. Обьект с МЕНЬШИМ значением глубины — ближайший
        //        // к нам обьект, значит и щелкнули мы на него. В зависимости от того, что
        //        // мы программируем, нам могут понадобится и ВСЕ выбранные обьекты (если
        //        // некоторые были за ближайшим), но в этом уроке мы позаботимся только о
        //        // переднем обьекте. Итак, как нам получить значение глубины? Оно сохранено
        //        // в буфере выбора (selectionBuffer). Для каждого обьекта в нем 4 значения.
        //        // Первое — «число имен в массиве имен на момент события, далее минимум и
        //        // максимум значений глубины для всех вершин, которые были выбраны при прошлом
        //        // событии, далее по содержимое массива имен, нижнее имя — первое;
        //        // («the number of names in the name stack at the time of the event, followed
        //        // by the minimum and maximum depth values of all vertices that hit since the
        //        // previous event, then followed by the name stack contents, bottom name first.») — MSDN.
        //        // Единстве, что нам нужно — минимальное значение глубины (второе значение) и
        //        // ID обьекта, переданного в glLoadName() (четвертое значение).
        //        // Итак, [0-3] — данные первого обьекта, [4-7] — второго, и т.д…
        //        // Будте осторожны, так как если вы отображаете на экране 2Д текст, он будет
        //        // всегда находится как ближайший обьект. Так что убедитесь, что отключили вывод
        //        // текста при рендеринге в режиме GL_SELECT. Я для этого использую флаг, передаваемый
        //        // в RenderScene(). Итак, получим обьект с минимальной глубиной!

        //        // При старте установим ближайшую глубину как глубину первого обьекта.
        //        // 1 — это минимальное Z-значение первого обьекта.
        //        unsigned int lowestDepth = selectBuffer[1];

        //        // Установим выбранный обьект как первый при старте.
        //        // 3 — ID первого обьекта, переданный в glLoadName().
        //        int selectedObject = selectBuffer[3];

        //        // Проходим через все найденные обьекты, начиная со второго (значения первого
        //        // мы присвоили изначально).
        //        for (int i = 1; i < objectsFound; i++)
        //        {
        //            // Проверяем, не ниже ли значение глубины текущего обьекта, чем предидущего.
        //            // Заметьте, мы умножаем i на 4 (4 значения на каждый обьект) и прибавляем 1 для глубины.
        //            if (selectBuffer[(i * 4) + 1] < lowestDepth)
        //            {
        //                // Установим новое низшее значение
        //                lowestDepth = selectBuffer[(i * 4) + 1];

        //                // Установим текущий ID обьекта
        //                selectedObject = selectBuffer[(i * 4) + 3];
        //            }
        //        }

        //        // Вернем выбранный обьект
        //        return selectedObject;
        //    }

        //    // Если не щелкнули ни на 1 обьект, вернём 0
        //    return 0;
        //}
    }
}