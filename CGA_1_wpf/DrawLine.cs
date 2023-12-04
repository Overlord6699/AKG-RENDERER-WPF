using CGA_1_wpf.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace CGA_1_wpf
{
    public class DrawLine : DrawMethodsAbstract
    {

        /*
			Метод DrawModel() класса LinearVisualisator перебирает все грани модели и 
			для каждой грани вызывает метод DrawFace(). Метод DrawFace() вызывает метод 
			ActionWithLine() для каждой точки, проходящей через грань. Метод ActionWithLine() 
			использует алгоритм DDA-линии для отрисовки линии, соединяющей две точки грани.
		 */
        public override void DrawModel(WriteableBitmap bitmap, Model model, Parameters parameters, Model worldModel)
        {
            foreach (var face in model.Edges)
                DrawFace(bitmap, model, face);
        }
    }
}
