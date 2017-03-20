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

            prRequest = 0.25;

            if (Model.rng.NextDouble() <= prRequest)
            {
                //создание заявки
                Request tmp = new Request();
                tmp.numServ = Model.rng.Next(1, 6);
                Console.WriteLine(tmp.numServ);
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
                        Console.Write(randomWs + "    ");
                    }   
                }
                Console.WriteLine();
                //генерация времени выполнения заявки + вычисление стоимости услуг для каждого цеха
                for (int j = 0; j < 5; j++)
                {
                    if (tmp.needWs[j] == false)
                        continue;
                    tmp.timeForWs[j] = serviceObj.workshop[j].getWorkTime();
                    tmp.priceForWs[j] = serviceObj.workshop[j].getPrice(tmp.timeForWs[j]);
                    Console.Write(tmp.timeForWs[j] + "    ");
                }
                Console.WriteLine();
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
            if ((modelObj.timeHour <= 10) && (modelObj.timeHour >= 18))
                return modelObj.ltRatio;
            else
                return modelObj.peakRatio;
        }
    }

    public class Request
    {
        public static int amountRequest = 0;
        public int numberRequest;
        public bool outOfTime;
        public int numServ;
        public int? actualWorkshop; // 0 - TechInspection, 1 - BodyShops, 2 - TireService, 3 - GearboxService, 4 - EngineService
        public bool[] needWs;
        public int[] timeForWs; //мин
        public double[] priceForWs;
        public int overallTimeInCarService; //мин
        
        public Request()
        {
            numberRequest = amountRequest++;
            outOfTime = false;
            needWs = new bool[5] { false, false, false, false, false };
            timeForWs = new int[5];
            priceForWs = new double[5];
            actualWorkshop = null;
            overallTimeInCarService = 7 * 24 * 60; //общее время нахождеия авто в сервисе
        }
    }
}
