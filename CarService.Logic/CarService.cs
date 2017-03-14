﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarService.Logic
{
    class CarService
    {
        public double servicepCP;
        public Request newRequest;
        public Workshop[] workshop;
        public double overallCarServiceProfit;
        public double[] wsProfit;

        public CarService()
        {
            servicepCP = 0.75;
            overallCarServiceProfit = 0;
            wsProfit = new double[5] { 0, 0, 0, 0, 0 };
            workshop = new Workshop[5];
            workshop[0] = new TechInspection(this);
            workshop[1] = new BodyShops(this);
            workshop[2] = new TireService(this);
            workshop[3] = new GearboxService(this);
            workshop[4] = new EngineService(this);
        }

        void checkAddRequest()
        {
            if (newRequest == null)
                return;
            //есть новая заявка

            newRequest = null;
        }



        public void work()
        {
            //проверка текущего времени??? ночь - не работаем
            //проверка наличия новой заявки
            checkAddRequest();
            //random change of ws?????
            foreach(var w in workshop)
            {
                w.procWork();
            }
        }

        public void procFinRWs(int ws, Request r)
        {
            overallCarServiceProfit += r.priceForWs[ws] * servicepCP;
            wsProfit[ws] += r.priceForWs[ws] * servicepCP;

            bool c = true;
            foreach (var t in r.timeForWs)
                if (t != 0)
                {
                    c = false;
                    break;
                }
            if (c)
                r.complete = true;
        }
    }
}
