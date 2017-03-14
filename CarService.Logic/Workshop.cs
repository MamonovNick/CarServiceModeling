using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarService.Logic
{
    class Master
    {
        public int profit;
        public byte percentOfCP;
        public bool free;
        public Request actualTimeRequest;

        public Master(byte pCP = 35)
        {
            free = true;
            profit = 0;
            percentOfCP = pCP;
            actualTimeRequest = null;
        }

        public void workWithRequest()
        {
            //do smth with request
            
        }
    }

    class Workshop
    {
        protected byte numMasters;
        protected byte readyMasters;
        protected List<Request> queueWs;
        protected List<Request> requestInWork;
        protected List<Master> masterInWs;

        public Workshop(byte nM = 1)
        {
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
                m.workWithRequest();
                if (m.free)
                {
                    //delete request
                    readyMasters++;
                }
            }

            //набор заявок для свободных мастеров
            int i = 0;
            int j = 0;
            while (readyMasters != 0)
            {
                //take free request
                while (i < queueWs.Count)
                {
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
        public override int getWorkTime() { return 0; }

        public override void procWork()
        {
            procCurWork(0);
        }
    }
    class BodyShops : Workshop
    {
        
        public override int getWorkTime() { return 0; }

        public override void procWork()
        {
            procCurWork(1);
        }
    }
    class TireService : Workshop
    {
        public override int getWorkTime() { return 0; }

        public override void procWork()
        {
            procCurWork(2);
        }
    }
    class GearboxService : Workshop
    {
        public override int getWorkTime() { return 0; }

        public override void procWork()
        {
            procCurWork(3);
        }
    }
    class EngineService : Workshop
    {
        public override int getWorkTime() { return 0; }

        public override void procWork()
        {
            procCurWork(4);
        }
    }
}
