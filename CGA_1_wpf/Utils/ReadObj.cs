using CGA_1_wpf.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace CGA_1_wpf
{
    internal class ReadObj
    {
        public static Model ReadObjFile(string fileAddress)
        {
            try
            {
                // файл считывается в массив строк
                var fileLines = File.ReadAllLines(fileAddress, Encoding.UTF8);
                if (fileLines is null)
                {
                    throw new ArgumentNullException(nameof(fileLines));
                }

                var points = new List<Vector4>();
                var edges = new List<List<Vector3>>();
                var normals = new List<Vector3>();
                foreach (var line in fileLines)
                {
                    if (line.Length > 2)
                        switch (line.Substring(0, 2))
                        {   
                            case "v ":
                                points.Add(ToPoint(line));
                                break;
                            case "f ":
                                edges.Add(ToEdge(line));
                                break;
                            case "vn":  
                                normals.Add(ToNormale(line));
                                break;
                        }
                }

                return new Model(points, edges, normals);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.Yes);
                return null;
            }
        }

        private static List<Vector3> ToEdge(string line)
        {
            var res = new List<Vector3>();
            string[] values = line.Replace("//", "/0/").Split(' ', (char)StringSplitOptions.RemoveEmptyEntries);
            for (int i = 1; i < values.Length; i++)
            {
                string[] parameters = values[i].Split(new char[] { ' ', '/' }, StringSplitOptions.RemoveEmptyEntries);
                var v = new Vector3(float.Parse(parameters[0]) - 1, float.Parse(parameters[1]) - 1, float.Parse(parameters[2]) - 1);
                res.Add(v);
            }

            return res;
        }

        private static Vector4 ToPoint(string line)
        {
            string[] values = line.Replace('.', ',').Split(' ', (char)StringSplitOptions.RemoveEmptyEntries);
            return new Vector4(float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3]), 1f);
        }

        private static Vector3 ToNormale(string line)
        {
            string[] values = line.Replace('.', ',').Split(' ', (char)StringSplitOptions.RemoveEmptyEntries);
            return new Vector3(float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3]));
        }

    }
}
