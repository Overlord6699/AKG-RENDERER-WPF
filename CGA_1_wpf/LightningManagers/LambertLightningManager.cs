using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CGA_1_wpf.LightningManagers
{
    public class LambertLightningManager: ILightningManager
    {
        private int _colorMultiplier = 200;


        public LambertLightningManager(in int colorMultiplier) {
            _colorMultiplier = colorMultiplier;
        }

        /*
     3) Реализовать плоское затенение и модель освещения Ламберта
     */
        public byte[] GetColorWithLightEffects(in Vector3 normal, in Vector3 lightDir)
        {

            byte blue = 60;
            byte green = 60;
            byte red = 60;

            //скалярное произведение нормали и обратного направления света
            byte alpha = (byte) (_colorMultiplier * Math.Max(Vector3.Dot(normal, lightDir), 0)); // |a| * |b| * cos(x) 

            byte[] colorData = { blue, green, red, alpha };
            return colorData; //(от до 255)
        }

        /*
        //На несколько источников (foreach для всех)
        private void ApplyLambertModel(Rectangle surface, LightSource light)
        {
            // Получаем нормаль к поверхности (в данном случае, вектор Z)
            Vector3 normal = new Vector3(0, 0, 1);

            // Получаем вектор направления к свету
            Vector3 lightDirection = light.Position - new Vector3(Canvas.GetLeft(surface), Canvas.GetTop(surface), 0);

            // Нормализуем вектор направления к свету
            lightDirection.Normalize();

            // Вычисляем интенсивность диффузного отражения по модели Ламберта
            double intensity = light.Intensity * Math.Max(0, Vector3.Dot(normal, lightDirection));

            // Применяем интенсивность к цвету поверхности
            var originalColor = ((SolidColorBrush)surface.Fill).Color;
            byte newRed = (byte)(originalColor.R * intensity);
            byte newGreen = (byte)(originalColor.G * intensity);
            byte newBlue = (byte)(originalColor.B * intensity);

            surface.Fill = new SolidColorBrush(Color.FromRgb(newRed, newGreen, newBlue));
        }*/
    }
}
