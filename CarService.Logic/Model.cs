using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarService.Logic
{
    public class Model
    {
        CarService cs;
        CarWorld cw;
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
        public Model()
        {
            peakRatio = 0.75;
            ltRatio = 0.5;
            cs = new CarService();
            cw = new CarWorld(this, cs);
        }

        void modeling()
        {

        }

        void simStep()
        {
            //генерация заявки
            cw.generateNewRequest();
            //работа а/с
            cs.work();
            //изменение счетчиков???

        }
    }
}
