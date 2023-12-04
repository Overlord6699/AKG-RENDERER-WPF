using CGA_1_wpf.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CGA_1_wpf
{
    public static class TransformationMatrices
    {
        public static Matrix4x4 ModelWorldMatrix { get; set; }

        /*
			2) Реализовать матричные преобразования координат из пространства модели в мировое пространство
			Это необходимо для перемещения или вращения модели в мировом пространстве.
		 */
        public static Model TransformFromModelToWorld(Model model, Parameters modelParams)
        {
            Matrix4x4 toWorldMatrix = GetTransformMatrix(modelParams);
            for (int i = 0; i < model.Points.Count; i++)
            {
                model.Points[i] = Vector4.Transform(model.Points[i], toWorldMatrix);

                model.Points[i] /= model.Points[i].W;
            }
            return model;
        }

        public static Model TransformFromModelToView(Model model, Parameters modelParams)
        {

            Matrix4x4 totalProjctionMatix = GetTotalMatrix(modelParams);
            float[] w = new float[model.Points.Count];
            for (int i = 0; i < model.Points.Count; i++)
            {
                model.Points[i] = Vector4.Transform(model.Points[i], totalProjctionMatix);

                w[i] = model.Points[i].W;
                model.Points[i] /= model.Points[i].W;
            }

            TransformNormals(model, modelParams);
            TransformToViewPort(model, modelParams, w);

            return model;
        }

        /*
			Матрица преобразования создается путем умножения трех матриц:
			Матрица масштабирования, которая увеличивает, уменьшает или сохраняет размер модели.
			Матрица вращения, которая поворачивает модель вокруг осей X, Y и Z.
			Матрица смещения, которая перемещает модель по осям X, Y и Z.
		 */
        private static Matrix4x4 GetTransformMatrix(Parameters modelParams)
        {
            ModelWorldMatrix = Matrix4x4.CreateScale(modelParams.Scaling)
                * Matrix4x4.CreateFromYawPitchRoll(modelParams.ModelYaw, modelParams.ModelPitch, modelParams.ModelRoll)
                * Matrix4x4.CreateTranslation(modelParams.TranslationX, modelParams.TranslationY, modelParams.TranslationZ);
            return ModelWorldMatrix;
        }

        /*
			3) Реализовать матричное преобразование координат из мирового пространства в пространство наблюдателя
			Это необходимо для упрощения вычисления проекции модели на экран.
		 */
        private static Matrix4x4 GetCameraMatrix(Parameters modelParams)
        {
            return
                 Matrix4x4.CreateTranslation(-new Vector3(modelParams.Camera.Position.X, modelParams.Camera.Position.Y, modelParams.Camera.Position.Z))
                 * Matrix4x4.Transpose(Matrix4x4.CreateFromYawPitchRoll(modelParams.Camera.Rotation.Y, modelParams.Camera.Rotation.X, modelParams.Camera.Rotation.Z));
        }

        /*
			4) Реализовать матричное преобразование координат из пространства наблюдателя в пространство проекции
			Проецируем координаты модели на экран.
		 */
        private static Matrix4x4 GetProjectionMatrix(Parameters modelParams)
        {
            return Matrix4x4.CreatePerspectiveFieldOfView(modelParams.FieldOfView, modelParams.AspectRatio,
                modelParams.NearPlaneDistance, modelParams.FarPlaneDistance);
        }

        private static Matrix4x4 GetWindowMatrix(Parameters modelParams)
        {
            return GetWindowMatrix(modelParams.XMin, modelParams.YMin, modelParams.Width, modelParams.Height);
        }
        //преобразование из пространства проекции в пространство окна
        public static Matrix4x4 GetWindowMatrix(int minX, int minY, int width, int height)
        {
            return new Matrix4x4(width / 2, 0, 0, 0,
                                 0, -height / 2, 0, 0,
                                 0, 0, 1, 0,
                                 minX + (width / 2), minY + (height / 2), 0, 1);
        }

        private static Matrix4x4 GetTotalMatrix(Parameters modelParams)
        {
            return GetTransformMatrix(modelParams) * GetCameraMatrix(modelParams) * GetProjectionMatrix(modelParams);
        }

        /*
			5) Реализовать матричное преобразование координат из пространства проекции в пространство окна просмотра
			Это необходимо для обрезки модели так, чтобы она отображалась только в области окна просмотра.
		 */
        private static void TransformToViewPort(Model model, Parameters modelParams, float[] w)
        {
            for (int i = 0; i < model.Points.Count; i++)
            {
                model.Points[i] = Vector4.Transform(model.Points[i], GetWindowMatrix(modelParams));
                model.Points[i] = new Vector4(model.Points[i].X, model.Points[i].Y, model.Points[i].Z, w[i]);
            }
        }

        private static void TransformNormals(Model model, Parameters modelParams)
        {
            for (int i = 0; i < model.Normals.Count; i++)
            {
                model.Normals[i] = Vector3.Normalize(Vector3.TransformNormal(model.Normals[i], GetTransformMatrix(modelParams)));
            }
        }
    }
}
