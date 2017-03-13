using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarService.Logic
{
    class CarWorld
    {
        //public double timeRatio { get; set; }
        //public double intensityRatio { get; set; }
        //private int numWorkshop { get; set; }
        Model modelObj;
        CarService serviceObj;
        Workshop[] wsObj;
        double prRequest;

        public CarWorld(Model m, CarService cs, Workshop[] ws)
        {
            modelObj = m;
            serviceObj = cs;
            wsObj = ws;
        }

        public void generateNewRequest()
        {
            //вероятность поступления заявки
            prRequest = modelObj.intensityRatio * timePr();
            if (Model.rng.NextDouble() <= prRequest)
            {
                //создание заявки
                Request tmp = new Request();
                tmp.numServ = Model.rng.Next(1, 6);
                int randomWs;
                int i = 0;
                //выбор цехов для посещения
                while (i < tmp.numServ)
                {
                    randomWs = Model.rng.Next(0, 5);
                    if (!tmp.needWs[randomWs])
                    {
                        tmp.needWs[randomWs] = true;
                        i++;
                    }
                }
                //генерация времени выполнения заявки
                for(int j = 0; j < 5; j++)
                {
                    if (tmp.needWs[j] == false)
                        continue;
                    tmp.timeForWs[j] = wsObj[j].getWorkTime();
                }
                //передача заявки в автосервис
                serviceObj.newRequest = tmp;
            }
            else
            {
                //заявки нет
                serviceObj.newRequest = null;
                return;
            }
        }

        //определение коэффициента времени суток
        private double timePr()
        {
            if ((modelObj.timeOfDay <= 10) && (modelObj.timeOfDay >= 18))
                return modelObj.ltRatio;
            else
                return modelObj.peakRatio;
        }
    }

    public class Request
    {
        public bool complete;
        public int numServ;
        public int? actualWorkshop;
        public bool[] needWs;
        public int[] timeForWs;

        public Request()
        {
            needWs = new bool[5] { false, false, false, false, false };
            timeForWs = new int[5];
            actualWorkshop = null;
        }
    }
}
