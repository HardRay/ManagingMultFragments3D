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

        //Конструктор
        public Polygon(List<int> arr, ref List<Num.Vector3> allPoints)
        {
            points = new List<int>();
            foreach (int i in arr)
                points.Add(i);
            polygonColor = defaultColor;
            active = false;
            this.pointsList = allPoints;
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

        public void Draw()
        {
            GL.PushMatrix();
            //Полигоны
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.AmbientAndDiffuse, polygonColor);
            GL.Begin(PrimitiveType.Polygon);
            foreach (int i in points)
                GL.Vertex3(pointsList[i].X, pointsList[i].Y, pointsList[i].Z);
            GL.End();
            //Рёбра
            GL.Disable(EnableCap.Lighting);
            GL.Color3(edgeColor);
            GL.Begin(PrimitiveType.LineLoop);
            foreach (int i in points)
                GL.Vertex3(pointsList[i].X, pointsList[i].Y, pointsList[i].Z);
            GL.End();
            GL.Enable(EnableCap.Lighting);
            GL.PopMatrix();
        }
    }
}
