using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarService.Logic
{
    class Master
    {
        public double profit;
        public double percentOfCP;
        public bool free;
        public Request actualTimeRequest;

        public Master(double pCP = 0.35)
        {
            free = true;
            profit = 0;
            percentOfCP = pCP;
            actualTimeRequest = null;
        }

        //работа с заявкой
        public int workWithRequest(int cMW)
        {
            //проверка на истечение срока нахождения на СТО
            if (--actualTimeRequest.overallTimeInCarService == 0)
                return -1;
            //"выполнение" работы, проверка на кончание работы
            if (--actualTimeRequest.timeForWs[cMW] == 0)
            {
                free = true;
                //начисление зарплаты
                profit += actualTimeRequest.priceForWs[cMW] * percentOfCP;
                return 1;
            }
            return 0;
        }
    }

    abstract class Workshop
    {
        protected Random rnd;
        protected CarService cs;
        protected byte numMasters;
        protected byte readyMasters;
        protected List<Request> queueWs;
        protected List<Request> requestInWork;
        protected List<Master> masterInWs;

        public Workshop(Random m, CarService c, byte nM)
        {
            rnd = m;
            cs = c;
            queueWs = new List<Request>();
            requestInWork = new List<Request>();
            masterInWs = new List<Master>();
            readyMasters = numMasters = nM;
        }

        //генерация времени выполнения заявки в текущем ws
        public abstract int getWorkTime();

        //генерация стоимости услуг + временной погрешности выполнения работ
        public abstract double getPrice(int time);

        //выполнение работ цехом
        public virtual void procWork() { return; }

        protected void procCurWork(int cMW)
        {
            //работа с текущими заявками
            foreach (var m in masterInWs)
            {
                switch (m.workWithRequest(cMW))
                {
                    case 1:
                        m.actualTimeRequest.numServ--;
                        cs.procFinRWs(cMW, m.actualTimeRequest);
                        m.actualTimeRequest.actualWorkshop = null;
                        requestInWork.Remove(m.actualTimeRequest);
                        m.actualTimeRequest = null;
                        break;
                    case 0:
                        break;
                    case -1:
                        m.actualTimeRequest.actualWorkshop = null;
                        m.actualTimeRequest.outOfTime = true;
                        requestInWork.Remove(m.actualTimeRequest);
                        m.actualTimeRequest = null;
                        break;
                }
            }

            //набор заявок для свободных мастеров
            int i = 0;
            int j = 0;
            while (readyMasters != 0)
            {
                //take free request >
                while (i < queueWs.Count)
                {
                    if (queueWs[i].outOfTime)
                    {
                        queueWs.RemoveAt(i);
                        continue;
                    }

                    if (queueWs[i].actualWorkshop == null)
                    {
                        queueWs[i].actualWorkshop = cMW;
                        while (j < masterInWs.Count)
                            if (masterInWs[j].free)
                                break;
                            else j++;
                        masterInWs[j].free = false;
                        masterInWs[j].actualTimeRequest = queueWs[i];
                        requestInWork.Add(queueWs[i]);
                        queueWs.RemoveAt(i);
                    }
                    i++;
                }
                readyMasters--;
            }
        }
    }

    class TechInspection : Workshop
    {
        public TechInspection(Random m, CarService cs, byte c = 1) : base(m, cs, c) { }

        public override int getWorkTime()
        {
            //время выполнения + погрешность 
            return rnd.Next(25, 90) + rnd.Next(10, 25);
        }

        public override double getPrice(int time)
        {
            //стоимость выполнения 
            return 500 * (time / 60);
        }

        public override void procWork()
        {
            procCurWork(0);
        }
    }
    class BodyShops : Workshop
    {
        public BodyShops(Random m, CarService cs, byte c = 1) : base(m, cs, c) { }

        public override int getWorkTime()
        {
            //время выполнения + погрешность 
            return rnd.Next(120, 2160) + rnd.Next(10, 720);
        }

        public override double getPrice(int time)
        {
            //стоимость выполнения
            return 2000 * (time / 60);
        }

        public override void procWork()
        {
            procCurWork(1);
        }
    }
    class TireService : Workshop
    {
        public TireService(Random m, CarService cs, byte c = 1) : base(m, cs, c) { }

        public override int getWorkTime()
        {
            //время выполнения + погрешность 
            return rnd.Next(5, 20) + rnd.Next(2, 5);
        }

        public override double getPrice(int time)
        {
            //стоимость выполнения
            return 200 * (time / 60);
        }

        public override void procWork()
        {
            procCurWork(2);
        }
    }
    class GearboxService : Workshop
    {
        public GearboxService(Random m, CarService cs, byte c = 1) : base(m, cs, c) { }

        public override int getWorkTime()
        {
            //время выполнения + погрешность 
            return rnd.Next(60, 2160) + rnd.Next(15, 720);
        }

        public override double getPrice(int time)
        {
            //стоимость выполнения 
            return 1500*(time / 60);
        }

        public override void procWork()
        {
            procCurWork(3);
        }
    }
    class EngineService : Workshop
    {
        public EngineService(Random m, CarService cs, byte c = 1) : base(m, cs, c) { }

        public override int getWorkTime()
        {
            //время выполнения + погрешность 
            return rnd.Next(20, 2880) + rnd.Next(5, 720);
        }

        public override double getPrice(int time)
        {
            //стоимость выполнения
            return 2500 * (time / 60);
        }

        public override void procWork()
        {
            procCurWork(4);
        }
    }
}
