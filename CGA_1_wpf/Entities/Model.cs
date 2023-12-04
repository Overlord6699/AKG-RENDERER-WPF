using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CGA_1_wpf.Entities
{
    public class Model : ICloneable
    {
        public List<Vector4> Points { get; set; }   // точки
        public List<List<Vector3>> Edges { get; set; }  // грани
        public List<Vector3> Normals { get; set; }  // нормали (нужны для определения освещенности грани)

        public Model(List<Vector4> points, List<List<Vector3>> edges, List<Vector3> normals)
        {
            Points = points;
            Edges = SplitFacesOnTriangles(edges);
            Normals = normals;
        }

        // для оптимизации нужно чтобы все грани были треугольниками
        private static List<List<Vector3>> SplitFacesOnTriangles(List<List<Vector3>> edges)
        {
            List<List<Vector3>> triangleEdges = new List<List<Vector3>>();
            foreach (List<Vector3> edge in edges)
            {
                if (edge.Count < 3)
                {
                    throw new ArgumentException("The face should include 3 parameters.");
                }

                for (int i = 1; i < edge.Count - 1; i++)
                {
                    List<Vector3> triangleFace = new List<Vector3>
                    {
                        edge[0],
                        edge[i],
                        edge[i + 1]
                    };

                    triangleEdges.Add(triangleFace);
                }
            }

            return triangleEdges;
        }

        public object Clone()
        {
            var newPoints = new List<Vector4>();
            foreach (var p in Points)
            {
                newPoints.Add(new Vector4(p.X, p.Y, p.Z, p.W));
            }

            var newEdges = new List<List<Vector3>>();
            foreach (var f in Edges)
            {
                var newList = new List<Vector3>();
                foreach (var v in f)
                {
                    newList.Add(new Vector3(v.X, v.Y, v.Z));
                }
                newEdges.Add(newList);
            }

            var newNormals = new List<Vector3>();
            foreach (var n in Normals)
            {
                newNormals.Add(n);
            }
            return new Model(newPoints, newEdges, newNormals);
        }
    }
}