using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Num = System.Numerics;

namespace Program
{
    static class Transformation
    {
        //Метод перемножения вектора3 на матрицу4x4
        static public Num.Vector3 MultVectorMatrix(Num.Vector3 v, Num.Matrix4x4 m)
        {
            Num.Vector4 rez = new Num.Vector4(0);
            rez.X += v.X * m.M11;
            rez.X += v.Y * m.M21;
            rez.X += v.Z * m.M31;
            rez.X += m.M41;
            rez.Y += v.X * m.M12;
            rez.Y += v.Y * m.M22;
            rez.Y += v.Z * m.M32;
            rez.Y += m.M42;
            rez.Z += v.X * m.M13;
            rez.Z += v.Y * m.M23;
            rez.Z += v.Z * m.M33;
            rez.Z += m.M43;
            rez.W += v.X * m.M14;
            rez.W += v.Y * m.M24;
            rez.W += v.Z * m.M34;
            rez.W += m.M44;
            return new Num.Vector3(rez.X / rez.W, rez.Y / rez.W, rez.Z / rez.W);
        }

        //Метод обединения точек полигонов
        static public List<int> UnionPoints(List<Polygon> polygons)
        {
            List<int> rez = new List<int>();
            foreach (Polygon p in polygons)
                rez = rez.Union(p.Points).ToList();
            return rez;
        }

        static private Num.Vector3 SearchCenter(List<Polygon> polygons, List<Num.Vector3> points)
        {
            Num.Vector3 rez = new Num.Vector3(0);
            List<int> indexes = UnionPoints(polygons);
            foreach (int i in indexes)
                rez = Num.Vector3.Add(rez, points[i]);
            rez = new Num.Vector3(rez.X / indexes.Count, rez.Y / indexes.Count, rez.Z / indexes.Count);
            return Num.Vector3.Abs(rez);
        }

        //Метод перемещения точек полигонов
        static public void Translate(char axis,int step,List<Polygon> polygons, ref List<Num.Vector3> points)
        {
            int x = 0, y = 0, z = 0;
            if (axis == 'X')
                x = step;
            else if (axis == 'Y')
                y = step;
            else
                z = step;
            foreach (int i in UnionPoints(polygons))
                points[i] = new Num.Vector3(points[i].X + x, points[i].Y + y, points[i].Z + z);
        }

        //Перегрузка метода Translate для перемещения на вектор
        static public void Translate(Num.Vector3 vec, List<Polygon> polygons, ref List<Num.Vector3> points)
        {
            foreach (int i in UnionPoints(polygons))
                points[i] = Num.Vector3.Add(points[i],vec);
        }

        //Метод вращения точек полигонов
        static public void Rotate(char axis, float step, List<Polygon> polygons, ref List<Num.Vector3> points, bool mode)
        {
            Num.Matrix4x4 matrix;
            if (axis == 'X')
                matrix = Num.Matrix4x4.CreateRotationX(step);
            else if (axis == 'Y')
                matrix = Num.Matrix4x4.CreateRotationY(step);
            else
                matrix = Num.Matrix4x4.CreateRotationZ(step);
            Num.Vector3 center = SearchCenter(polygons, points);
            foreach (int i in UnionPoints(polygons))
            {
                if (mode)
                {
                    points[i] = MultVectorMatrix(Num.Vector3.Subtract(points[i], center), matrix);
                    points[i] = Num.Vector3.Add(points[i], center);
                }
                else
                    points[i] = MultVectorMatrix(points[i], matrix);
            }
        }

        //Метод масштабирования полигонов
        static public void Scale(float value, List<Num.Vector3> originPoints, List<Polygon> polygons, ref List<Num.Vector3> points)
        {
            Num.Vector3 center = SearchCenter(polygons, points);
            foreach (int i in UnionPoints(polygons))
            {
                Num.Vector3 vec = Num.Vector3.Subtract(originPoints[i], center);
                points[i] = new Num.Vector3(vec.X * value + center.X, vec.Y * value + center.Y, vec.Z * value + center.Z);
            }
        }
    }
}
