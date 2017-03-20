using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CarService.Logic
{
    public class Statistics : DependencyObject
    {
        public static readonly DependencyProperty RequestGenProperty;

        public static readonly DependencyProperty Ws0Property;

        public static readonly DependencyProperty Ws1Property;

        public static readonly DependencyProperty Ws2Property;

        public static readonly DependencyProperty Ws3Property;

        public static readonly DependencyProperty Ws4Property;

        static Statistics()
        {
            RequestGenProperty = DependencyProperty.Register("RequestGen", typeof(string), typeof(Statistics));

            Ws0Property = DependencyProperty.Register("Ws0", typeof(int), typeof(Statistics));
            Ws1Property = DependencyProperty.Register("Ws1", typeof(int), typeof(Statistics));
            Ws2Property = DependencyProperty.Register("Ws2", typeof(int), typeof(Statistics));
            Ws3Property = DependencyProperty.Register("Ws3", typeof(int), typeof(Statistics));
            Ws4Property = DependencyProperty.Register("Ws4", typeof(int), typeof(Statistics));
        }

        public string RequestGen
        {
            get { return (string)GetValue(RequestGenProperty); }
            set { SetValue(RequestGenProperty, value); }
        }

        public int Ws0
        {
            get { return (int)GetValue(Ws0Property); }
            set { SetValue(Ws0Property, value); }
        }
        public int Ws1
        {
            get { return (int)GetValue(Ws1Property); }
            set { SetValue(Ws1Property, value); }
        }
        public int Ws2
        {
            get { return (int)GetValue(Ws2Property); }
            set { SetValue(Ws2Property, value); }
        }
        public int Ws3
        {
            get { return (int)GetValue(Ws3Property); }
            set { SetValue(Ws3Property, value); }
        }

        public int Ws4
        {
            get { return (int)GetValue(Ws4Property); }
            set { SetValue(Ws4Property, value); }
        }
    }

    public class Model
    {
        CarService cs;
        CarWorld cw;
        public Statistics st;

        //текущие параметры
        public int overallTime; //общее время моделирования
        public int realTime; //оставшееся время моделирования
        public bool isWeekend;
        public byte timeDay;
        public byte timeHour; //время
        public byte timeMin;

        //
        //параметры моделирования
        public double timeRatio { get; set; }
        public double intensityRatio { get; set; }
        public double errorIntensityRatio { get; set; }
        public int numWorkshop { get; set; }

        public readonly double peakRatio;
        public readonly double ltRatio;

        public byte[] numMasters;
        //

        public static Random rng = new Random();

        public Model(Statistics s = null)
        {
            peakRatio = 0.75;
            ltRatio = 0.5;
            numMasters = new byte[5] { 5, 5, 5, 5, 5 };
            st = s;
        }

        public void create_objects()
        {
            cs = new CarService(rng, numMasters, st);
            cw = new CarWorld(this, cs);
            //set_times(0, 0, 0, 4560);
            set_times(0, 0, 0, 4*24*60);
        }

        public void set_times(byte d, byte h, byte m, int all)
        {
            overallTime = realTime = all;
            timeDay = d;
            timeHour = h;
            timeMin = m;
            if ((timeDay == 5) || (timeDay == 6))
                isWeekend = true;
            else
                isWeekend = false;
        }

        public void modeling()
        {
            Console.WriteLine(overallTime);
            //моделирование работы СТО
            while (overallTime != 0)
            { 
                simStep();
            }
        }

        public string getCSProfit()
        {
            return cs.overallCarServiceProfit.ToString();
        }

        public void simStep(int i = 1)
        {
            for (int j = 0; j < i; j++)
            {
                //генерация заявки
                cw.generateNewRequest();
                if (cs.newRequest != null)
                {
                    st.RequestGen = cs.newRequest.numServ.ToString();
                    Console.WriteLine("Номер заявки: " + cs.newRequest.numberRequest);
                }
                else
                    st.RequestGen = "No request";
                //работа а/с
                cs.work(isWeekend, timeHour, timeMin);
                //изменение счетчиков
                //st.RequestGen = cs.workshop.Count() + " - " + cs.workshop[0].masterInWs.Count + " - " + cs.workshop[0]
                if (++timeMin >= 60)
                {
                    timeMin = 0;
                    if (++timeHour >= 24)
                    {
                        timeHour = 0;
                        if (++timeDay >= 7)
                            timeDay = 0;
                    }
                }
                if ((timeDay == 5) || (timeDay == 6))
                    isWeekend = true;
                else
                    isWeekend = false;
                realTime--;
                overallTime--;
            }
        }
    }
}
