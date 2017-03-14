using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarService.Logic
{
    class CarWorld
    {
        Model modelObj;
        CarService serviceObj;
        double prRequest;

        public CarWorld(Model m, CarService cs)
        {
            modelObj = m;
            serviceObj = cs;
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
                //генерация времени выполнения заявки + вычисление стоимости услуг для каждого цеха
                for (int j = 0; j < 5; j++)
                {
                    if (tmp.needWs[j] == false)
                        continue;
                    tmp.timeForWs[j] = serviceObj.workshop[j].getWorkTime();
                    tmp.priceForWs[j] = serviceObj.workshop[j].getPrice();
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
        public int? actualWorkshop; // 0 - TechInspection, 1 - BodyShops, 2 - TireService, 3 - GearboxService, 4 - EngineService
        public bool[] needWs;
        public int[] timeForWs;
        public int[] priceForWs;
        public int overallTimeInCarService;
        
        public Request()
        {
            needWs = new bool[5] { false, false, false, false, false };
            timeForWs = new int[5];
            priceForWs = new int[5];
            actualWorkshop = null;
            overallTimeInCarService = 7 * 24; //общее время нахождеия авто в сервисе

        }
    }
}
