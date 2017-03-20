using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarService.Logic
{
    class CarService
    {
        public Statistics st;
        public Random rd;
        public double servicepCP;
        public Request newRequest;
        public Workshop[] workshop;
        public double overallCarServiceProfit;
        public double[] wsProfit;
        public int numCompleteRequest;
        public int numFailedRequest;
        public bool[] wWs;

        public CarService(Random r, byte[] nM, Statistics s)
        {
            rd = r;
            st = s;
            servicepCP = 0.75;
            overallCarServiceProfit = 0;
            numCompleteRequest = 0;
            numFailedRequest = 0;
            wsProfit = new double[5] { 0, 0, 0, 0, 0 };
            wWs = new bool[5];
            workshop = new Workshop[5];
            workshop[0] = new TechInspection(r, this, st, nM[0]);
            workshop[1] = new BodyShops(r, this, st, nM[1]);
            workshop[2] = new TireService(r, this, st, nM[2]);
            workshop[3] = new GearboxService(r, this, st, nM[3]);
            workshop[4] = new EngineService(r, this, st, nM[4]);
        }

        void checkAddRequest()
        {
            if (newRequest == null)
                return;
            //есть новая заявка
            for(int i = 0; i < 5; i++)
            {
                if(newRequest.needWs[i])
                {
                    workshop[i].addRequest(newRequest);
                }
            }
            newRequest = null;
        }

        public void work(bool weekend, byte h, byte min)
        {
            //проверка текущего времени??? ночь - не работаем
            if (weekend && ((h < 9) || (h > 17)) || !weekend && ((h < 8) || (h > 20)))
            {
                //st.Ws0 = -1;
                //st.Ws1 = -1;
                //st.Ws2 = -1;
                //st.Ws3 = -1;
                //st.Ws4 = -1;
                return;
            }
            Console.WriteLine(h+ "   " + min + "   " + weekend);
            //проверка наличия новой заявки
            checkAddRequest();
            //random change of ws?????
            wWs = new bool[5] { false, false, false, false, false };

            int j = 0;
            int tmpIndex;
            while(j < 5)
            {
                tmpIndex = rd.Next(0, 5);
                if (!wWs[tmpIndex])
                {
                    j++;
                    wWs[tmpIndex] = true;
                    workshop[tmpIndex].procWork();
                }
            }
        }

        public void procFinRWs(int b, int ws, Request r)
        {
            if (b != 1)
                return;
            overallCarServiceProfit += r.priceForWs[ws] * servicepCP;
            wsProfit[ws] += r.priceForWs[ws] * servicepCP;
        }

        public void procFinRWs(int b)
        {
            if (b == -1)
                numFailedRequest++;
        }
    }
}
