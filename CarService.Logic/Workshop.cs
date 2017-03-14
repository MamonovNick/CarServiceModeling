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

    class Workshop
    {
        public CarService cs;
        protected byte numMasters;
        protected byte readyMasters;
        protected List<Request> queueWs;
        protected List<Request> requestInWork;
        protected List<Master> masterInWs;

        public Workshop(CarService c, byte nM)
        {
            cs = c;
            queueWs = new List<Request>();
            requestInWork = new List<Request>();
            masterInWs = new List<Master>();
            readyMasters = numMasters = nM;
        }

        //генерация времени выполнения заявки в текущем ws
        public virtual int getWorkTime() { return 0; }

        public virtual int getPrice() { return 0; }

        public virtual void procWork() { return; }

        protected void procCurWork(int cMW)
        {
            //работа с текущими заявками
            foreach (var m in masterInWs)
            {
                switch (m.workWithRequest(cMW))
                {
                    case 1:
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
        public TechInspection(CarService cs, byte c = 1) : base(cs, c) { }

        public override int getWorkTime() { return 0; }

        public override void procWork()
        {
            procCurWork(0);
        }
    }
    class BodyShops : Workshop
    {
        public BodyShops(CarService cs, byte c = 1) : base(cs, c) { }

        public override int getWorkTime() { return 0; }

        public override void procWork()
        {
            procCurWork(1);
        }
    }
    class TireService : Workshop
    {
        public TireService(CarService cs, byte c = 1) : base(cs, c) { }

        public override int getWorkTime() { return 0; }

        public override void procWork()
        {
            procCurWork(2);
        }
    }
    class GearboxService : Workshop
    {
        public GearboxService(CarService cs, byte c = 1) : base(cs, c) { }

        public override int getWorkTime() { return 0; }

        public override void procWork()
        {
            procCurWork(3);
        }
    }
    class EngineService : Workshop
    {
        public EngineService(CarService cs, byte c = 1) : base(cs, c) { }

        public override int getWorkTime() { return 0; }

        public override void procWork()
        {
            procCurWork(4);
        }
    }
}
