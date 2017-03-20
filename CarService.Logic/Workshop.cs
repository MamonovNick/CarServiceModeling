using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarService.Logic
{
    class Master
    {
        Statistics st;
        public double profit;
        public double percentOfCP;
        public bool free;
        public Request actualTimeRequest;

        public Master(Statistics s, double pCP = 0.35)
        {
            st = s;
            free = true;
            profit = 0;
            percentOfCP = pCP;
            actualTimeRequest = null;
        }

        //работа с заявкой
        public int workWithRequest(int cMW)
        {
            if (actualTimeRequest == null)
                return 0;
            Console.WriteLine("Master of " + cMW + " work with " + actualTimeRequest.numberRequest);
            //проверка на истечение срока нахождения на СТО
            if (--actualTimeRequest.overallTimeInCarService == 0)
                return -1;
            //"выполнение" работы, проверка на кончание работы
            if (--actualTimeRequest.timeForWs[cMW] == 0)
            {
                free = true;
                switch (cMW)
                {
                    case 0:
                        st.Ws0--;
                        break;
                    case 1:
                        st.Ws1--;
                        break;
                    case 2:
                        st.Ws2--;
                        break;
                    case 3:
                        st.Ws3--;
                        break;
                    case 4:
                        st.Ws4--;
                        break;
                }
                //начисление зарплаты
                profit += actualTimeRequest.priceForWs[cMW] * percentOfCP;
                return 1;
            }
            return 0;
        }
    }

    abstract class Workshop
    {
        public Statistics st;
        public Random rnd;
        public CarService cs;
        public byte numMasters;
        public byte readyMasters;
        public List<Request> queueWs;
        public List<Request> requestInWork;
        public List<Master> masterInWs;

        public Workshop(Random m, CarService c, byte nM, Statistics s)
        {
            st = s;
            rnd = m;
            cs = c;
            queueWs = new List<Request>();
            requestInWork = new List<Request>();
            masterInWs = new List<Master>();
            readyMasters = numMasters = nM;
            for (int i = 0; i < nM; i++)
                masterInWs.Add(new Master(st));
        }

        //генерация времени выполнения заявки в текущем ws
        public abstract int getWorkTime();

        //генерация стоимости услуг + временной погрешности выполнения работ
        public abstract double getPrice(int time);

        //выполнение работ цехом
        public virtual void procWork() { return; }

        public void addRequest(Request r)
        {
            queueWs.Add(r);
        }

        protected void procCurWork(int cMW)
        {
            //набор заявок для свободных мастеров
            int i = 0; //по заявкам
            int j = 0; //по мастерам
            //выбираем свободные заявки
            while (i < queueWs.Count)
            {
                //проверка на наличие свободных мастеров
                if (readyMasters == 0)
                    break;

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
                    readyMasters--;
                    switch (cMW)
                    {
                        case 0:
                            st.Ws0++;
                            break;
                        case 1:
                            st.Ws1++;
                            break;
                        case 2:
                            st.Ws2++;
                            break;
                        case 3:
                            st.Ws3++;
                            break;
                        case 4:
                            st.Ws4++;
                            break;
                    }
                    masterInWs[j].actualTimeRequest = queueWs[i];
                    requestInWork.Add(queueWs[i]);
                    queueWs.RemoveAt(i);
                    continue;
                }
                i++;
            }

            //работа с текущими заявками
            foreach (var m in masterInWs)
            {
                switch (m.workWithRequest(cMW))
                {
                    case 1:
                        m.actualTimeRequest.numServ--;
                        cs.procFinRWs(1, cMW, m.actualTimeRequest);
                        m.actualTimeRequest.actualWorkshop = null;
                        requestInWork.Remove(m.actualTimeRequest);
                        m.actualTimeRequest = null;
                        readyMasters++;
                        break;
                    case 0:
                        break;
                    case -1:
                        cs.procFinRWs(-1);
                        m.actualTimeRequest.actualWorkshop = null;
                        m.actualTimeRequest.outOfTime = true;
                        requestInWork.Remove(m.actualTimeRequest);
                        m.actualTimeRequest = null;
                        readyMasters++;
                        break;
                }
            }
        }
    }

    class TechInspection : Workshop
    {
        public TechInspection(Random m, CarService cs, Statistics s, byte c = 1) : base(m, cs, c, s) { }

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
            Console.WriteLine("Workshop #0 of" + numMasters);
            procCurWork(0);
        }
    }
    class BodyShops : Workshop
    {
        public BodyShops(Random m, CarService cs, Statistics s, byte c = 1) : base(m, cs, c, s) { }

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
            Console.WriteLine("Workshop #1 of" + numMasters);
            procCurWork(1);
        }
    }
    class TireService : Workshop
    {
        public TireService(Random m, CarService cs, Statistics s, byte c = 1) : base(m, cs, c, s) { }

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
            Console.WriteLine("Workshop #2 of" + numMasters);
            procCurWork(2);
        }
    }
    class GearboxService : Workshop
    {
        public GearboxService(Random m, CarService cs, Statistics s, byte c = 1) : base(m, cs, c, s) { }

        public override int getWorkTime()
        {
            //время выполнения + погрешность 
            return rnd.Next(60, 2160) + rnd.Next(15, 720);
        }

        public override double getPrice(int time)
        {
            //стоимость выполнения 
            return 1500 * (time / 60);
        }

        public override void procWork()
        {
            Console.WriteLine("Workshop #3 of" + numMasters);
            procCurWork(3);
        }
    }
    class EngineService : Workshop
    {
        public EngineService(Random m, CarService cs, Statistics s, byte c = 1) : base(m, cs, c, s) { }

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
            Console.WriteLine("Workshop #4 of" + numMasters);
            procCurWork(4);
        }
    }
}
