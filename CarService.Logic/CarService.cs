using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarService.Logic
{
    class CarService
    {
        public Request newRequest;
        public Workshop[] workshop;

        public CarService()
        {
            workshop = new Workshop[5];
            workshop[0] = new TechInspection();
            workshop[1] = new BodyShops();
            workshop[2] = new TireService();
            workshop[3] = new GearboxService();
            workshop[4] = new EngineService();
        }

        void checkAddRequest()
        {
            if (newRequest == null)
                return;
            //есть новая заявка

        }



        public void work()
        {
            //проверка текущего времени??? ночь - не работаем
            //проверка наличия новой заявки
            checkAddRequest();
            foreach(var w in workshop)
            {
                w.procWork();
            }
        }

    }
}
