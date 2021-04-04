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
        //Метод перемножения вектора и матрицы
        static Num.Vector3 MultVectorMatrix(Num.Vector3 vec, Num.Matrix4x4 m)
        {
            Num.Vector4 rez = new Num.Vector4();
            rez.X = vec.X * m.M11;
            rez.X += vec.Y * m.M21;
            rez.X += vec.Z * m.M31;
            rez.X += m.M41;
            rez.Y = vec.X * m.M12;
            rez.Y += vec.Y * m.M22;
            rez.Y += vec.Z * m.M32;
            rez.Y += m.M42;
            rez.Z = vec.X * m.M13;
            rez.Z += vec.Y * m.M23;
            rez.Z += vec.Z * m.M33;
            rez.Z += m.M43;
            rez.W = vec.X * m.M14;
            rez.W += vec.Y * m.M24;
            rez.W += vec.Z * m.M34;
            rez.W += m.M44;
            return new Num.Vector3(rez.X / rez.W, rez.Y / rez.W, rez.Z / rez.W);
        }

        static List<int> UnionPoints(List<Polygon> polygons)
        {
            List<int> rez = new List<int>();
            foreach (Polygon p in polygons)
                rez = rez.Union(p.Points).ToList();
            return rez;
        }

        static public void Translate(char axis, int step, List<Polygon> polygons, ref List<Num.Vector3> points)
        {
            int x = 0, y = 0, z = 0;
            if (axis == 'X')
                x = step;
            else if (axis == 'Y')
                y = step;
            else
                z = step;
            List<int> arr = UnionPoints(polygons);
            foreach (int i in arr)
                points[i] = new Num.Vector3(points[i].X + x, points[i].Y + y, points[i].Z + z);
        }
    }
}
