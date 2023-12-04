using CGA_1_wpf.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows;
using CGA_1_wpf.CutoffPixelsManagers;
using CGA_1_wpf.LightningManagers;

namespace CGA_1_wpf
{
    public class DrawPlane : DrawMethodsAbstract
    {
        const int COLOR_MULTIPLIER = 200;
        
        private float[,] _zBuffer;

        private ICutoffPixelsManager _cutoffManager;
        private ILightningManager _lightningManager;

        public DrawPlane(ICutoffPixelsManager cutoffManager, ILightningManager lightningManager)
        {
            _cutoffManager = cutoffManager;
            _lightningManager = lightningManager;
        }

        public override void DrawModel(WriteableBitmap bitmap, Model model, Parameters parameters, Model worldModel) {
            _zBuffer = new float[(int) bitmap.Width, (int) bitmap.Height];

            for (int i = 0; i < _zBuffer.GetLength(0); i++) {
                for (int j = 0; j < _zBuffer.GetLength(1); j++) {
                    _zBuffer[i, j] = float.PositiveInfinity;
                }
            }
            foreach (var edge in model.Edges) {
                var cameraVector = Vector3.Normalize(
                    new Vector3(
                        parameters.Camera.Position.X,
                        parameters.Camera.Position.Y,
                        parameters.Camera.Position.Z)
                    - GetFaceFirstPoint(worldModel, edge)
                );
                var normal = GetNormal(model, edge);

                if (_cutoffManager.IsTriangleVisible(normal, cameraVector)) {
                    DrawEdge(bitmap, model, edge, normal);
                }
            }
        }


        protected struct BesenhamVariables {
            public int yGrowing;
            public float dz;
            public float z;
            public int err;
            public int y;
            public int dy;
            public int dx;

            public BesenhamVariables(Vertice pointFrom, Vertice pointTo) {
                yGrowing = pointTo.Y > pointFrom.Y ? 1 : -1;
                dz = (pointTo.Z - pointFrom.Z) / (pointTo.X - pointFrom.X);
                z = pointFrom.Z;
                err = 0;
                y = pointFrom.Y;
                dy = pointTo.Y - pointFrom.Y;
                dx = pointTo.X - pointFrom.X;
            }

            public void IncrementX() {
                err += yGrowing * dy;
                z += dz;
                while (err > dx) {
                    err -= dx;
                    y += yGrowing;
                }
            }
        }


        protected virtual Vector3 GetNormalInPoint(Vertice pixel, Vector3 face) {
            return Vector3.Zero;
        }

        protected Action<Vector3> EveryPointAction = (normal) => { };
        protected virtual Vector3 InterpolateNormals(Vector3 position, Vector3[] heights, Vector3[] normals) {
            return Vector3.Zero;
        }

        protected Vector3[] GetPointsFromFace(Model model, List<Vector3> face) {
            var result = new List<Vector3>();
            foreach (var element in face) {
                var point = model.Points[(int) element.X];
                result.Add(new Vector3(point.X, point.Y, point.Z * 1000));
            }

            return result.ToArray();
        }

        protected Vector3 GetCurrentPositionVector(int x, int y, float z) {
            return new Vector3(x, y, z * 1000);
        }

        /*
            1) Реализовать алгоритм растеризации треугольников
            */
        protected virtual void DrawEdge(WriteableBitmap bitmap, Model model, List<Vector3> face, Vector3 normal) {
            GetPixelColor = () => _lightningManager.GetColorWithLightEffects(normal, Vector3.UnitZ);

            var triangle = new Vertice[3];
            for (int i = 0; i < 3; i++) {
                triangle[i] = GetFacePoint(model, face, i);
            }
            triangle = triangle.OrderBy(p => p.X).ToArray();

            var besenham01 = new BesenhamVariables(triangle[0], triangle[1]);
            var besenham02 = new BesenhamVariables(triangle[0], triangle[2]);
            var besenham12 = new BesenhamVariables(triangle[1], triangle[2]);

            //угловые коэфы
            var k2 = besenham02.dx != 0 ? ((float) besenham02.dy) / besenham02.dx : besenham02.dy;
            var k1 = (besenham01.dx != 0) ? ((float) besenham01.dy) / besenham01.dx : besenham01.dy;
            var dy = k2 > k1 ? 1 : -1; //направление движения


            //отрисовка половины треугольника
            for (int x = triangle[0].X; x < triangle[1].X; x++) {
                var z = besenham01.z;
                var dz = (besenham01.y - besenham02.y) != 0 ? (besenham01.z - besenham02.z) / (besenham01.y - besenham02.y) : 0;
                for (int y = besenham01.y; dy * y <= dy * besenham02.y; y += dy) {
                    z += dz;

                    if (x >= 0 && x < bitmap.Width && y >= 0 && y < bitmap.Height && z < _zBuffer[x, y]) {
                        _zBuffer[x, y] = z; // отбраковка задних поверхностей объектов

                        EveryPointAction(InterpolateNormals(GetCurrentPositionVector(x, y, z), GetPointsFromFace(model, face), null));
                        DrawPixel(bitmap, new Vertice(x, y, z));
                    }
                }

                besenham01.IncrementX();
                besenham02.IncrementX();
            }

            for (int x = triangle[1].X; x < triangle[2].X; x++)
            {
                var z = besenham12.z;
                var dz = ((besenham12.y - besenham02.y) != 0) ? (besenham12.z - besenham02.z) / (besenham12.y - besenham02.y) : 0;
                for (int y = besenham12.y; dy * y <= dy * besenham02.y; y += dy)
                {
                    z += dz;
                    if (x >= 0 && x < bitmap.Width && y >= 0 && y < bitmap.Height && z < _zBuffer[x, y])
                    {
                        _zBuffer[x, y] = z;

                        EveryPointAction(InterpolateNormals(GetCurrentPositionVector(x, y, z), GetPointsFromFace(model, face), null));
                        DrawPixel(bitmap, new Vertice(x, y, z));
                    }
                }

                besenham12.IncrementX();
                besenham02.IncrementX();
            }

        }

        private Vector3 GetNormal(Model model, List<Vector3> triangle) {
            Vector3 normal1 = model.Normals[(int) triangle[0].Z];
            Vector3 normal2 = model.Normals[(int) triangle[1].Z];
            Vector3 normal3 = model.Normals[(int) triangle[2].Z];

            return Vector3.Normalize(normal1 + normal2 + normal3);
        }


    }
}
