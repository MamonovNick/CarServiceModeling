using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarService.Logic
{
    public class Model
    {
        //текущие параметры
        public int overallTime; //общее время моделирования
        public int realTime; //оставшееся время моделирования
        public bool isWeekend; 
        public byte timeOfDay; //время
        public byte minutesH;

        //
        //параметры моделирования
        public double timeRatio { get; set; }
        public double intensityRatio { get; set; }
        public double errorIntensityRatio { get; set; }
        public int numWorkshop { get; set; }
        
        public readonly double peakRatio;
        public readonly double ltRatio;
        //
        
        public static Random rng = new Random();
        Model()
        {
            peakRatio = 0.75;
            ltRatio = 0.5;
        }

        void modeling()
        {

        }

        void simStep()
        {
            //генерация заявки
            //работа а/с
            //изменение счетчиков???

        }
    }
}
