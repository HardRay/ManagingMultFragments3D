using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Num = System.Numerics;

namespace Program
{
    class Polygon
    {
        private List<Num.Vector3> pointsList;
        static private Color defaultColor = Color.FromArgb(131, 132, 134);
        static private Color activeColor = Color.FromArgb(163, 152, 152);
        static private Color defaultEdgeColor = Color.Black;
        static private Color activeEdgeColor = Color.White;
        //Список точек
        private List<int> points;
        private Color polygonColor;
        private Color edgeColor;
        private bool active;
        private string name;
        private Num.Vector3 normal;

        public List<int> Points
        {
            get
            {
                return points;
            }
        }
        
        public Color PolygonColor
        {
            get
            {
                return polygonColor;
            }
            set
            {
                this.polygonColor = value;
            }
        }

        public bool isActive
        {
            get
            {
                return active;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                this.name = value;
            }
        }

        //Конструктор
        public Polygon(List<int> arr, ref List<Num.Vector3> allPoints)
        {
            points = new List<int>();
            foreach (int i in arr)
                points.Add(i);
            polygonColor = defaultColor;
            active = false;
            this.pointsList = allPoints;
            //Расчёт нормали
            normal = Num.Vector3.Cross(Num.Vector3.Subtract(allPoints[points[0]], allPoints[points[1]]), Num.Vector3.Subtract(allPoints[points[1]], allPoints[points[2]]));
        }

        public Polygon(List<int> arr, ref List<Num.Vector3> allPoints, string name) : this(arr,ref allPoints)
        {
            this.name = name;
        }

        //Выбор
        public void Select()
        {
            polygonColor = activeColor;
            edgeColor = activeEdgeColor;
            active = true;
        }

        public void NotSelect()
        {
            polygonColor = defaultColor;
            edgeColor = defaultEdgeColor;
            active = false;
        }

        public void Draw(bool edgeMode)
        {
            GL.PushMatrix();
            //Полигоны
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.AmbientAndDiffuse, polygonColor);
            GL.Begin(PrimitiveType.Polygon);
            GL.Normal3(normal.X, normal.Y, normal.Z);
            Num.Matrix4x4 matrix = Num.Matrix4x4.CreateTranslation(10,10,10);
            Num.Vector4 p = new Num.Vector4(pointsList[0], 1);
            foreach (int i in points)
                GL.Vertex3(pointsList[i].X, pointsList[i].Y, pointsList[i].Z);
            GL.End();
            //Рёбра
            if (edgeMode)
            {
                GL.Disable(EnableCap.Lighting);
                GL.Color3(edgeColor);
                GL.Begin(PrimitiveType.LineLoop);
                foreach (int i in points)
                    GL.Vertex3(pointsList[i].X, pointsList[i].Y, pointsList[i].Z);
                GL.End();
                GL.Enable(EnableCap.Lighting);
            }
            GL.PopMatrix();
        }
    }
}
