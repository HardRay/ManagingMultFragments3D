using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Program
{
    class Polygon
    {
        //Список точек
        private List<int> points;
        private Color color;

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
                return color;
            }
            set
            {
                this.color = value;
            }
        }

        //Конструктор
        public Polygon(List<int> arr)
        {
            points = new List<int>();
            foreach (int i in arr)
                points.Add(i);
            color = Color.Cyan;
        }

    }
}
