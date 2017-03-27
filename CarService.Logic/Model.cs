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
        public static readonly DependencyProperty StepMProperty;

        public static readonly DependencyProperty RequestGenProperty;

        public static readonly DependencyProperty Ws0Property; //состояние 1 цеха

        public static readonly DependencyProperty Ws1Property; //состояние 2 цеха

        public static readonly DependencyProperty Ws2Property; //состояние 3 цеха

        public static readonly DependencyProperty Ws3Property; //состояние 4 цеха

        public static readonly DependencyProperty Ws4Property; //состояние 5 цеха

        static Statistics()
        {
            PropertyMetadata metadata = new PropertyMetadata();

            metadata.DefaultValue = "3";

            StepMProperty = DependencyProperty.Register("StepM", typeof(string), typeof(Statistics), metadata, new ValidateValueCallback(ValidateStepValue));

            RequestGenProperty = DependencyProperty.Register("RequestGen", typeof(string), typeof(Statistics));

            Ws0Property = DependencyProperty.Register("Ws0", typeof(int), typeof(Statistics));
            Ws1Property = DependencyProperty.Register("Ws1", typeof(int), typeof(Statistics));
            Ws2Property = DependencyProperty.Register("Ws2", typeof(int), typeof(Statistics));
            Ws3Property = DependencyProperty.Register("Ws3", typeof(int), typeof(Statistics));
            Ws4Property = DependencyProperty.Register("Ws4", typeof(int), typeof(Statistics));
        }

        public static bool ValidateStepValue(object value)
        {
            string currentValue = (string)value;
            int i;
            return (int.TryParse(currentValue, out i));
        }

        public string StepM
        {
            get { return (string)GetValue(StepMProperty); }
            set { SetValue(StepMProperty, value); }
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

        //параметры
        public int overallTime; //общее время моделирования
        public int realTime; //оставшееся время моделирования
        public int ovDay;
        public byte ovHour;
        public byte ovMin;
        public bool isWeekend;
        public byte timeDay;
        public byte timeHour; //время
        public byte timeMin;
        public int stepModel;
        public double timeRatio;
        public double intensityRatio;
        public double[] wsErrorPr;
        public int numWorkshop;
        public double peakRatio;
        public double ltRatio;
        public int totalCarTimeInService;
        public byte[] numMasters;
        //

        public static Random rng = new Random();

        public Model(Statistics s = null)
        {
            peakRatio = 0.75;
            ltRatio = 0.5;
            totalCarTimeInService = 7 * 24 * 60;
            stepModel = 3;
            numMasters = new byte[5] { 5, 5, 5, 5, 5 };
            wsErrorPr = new double[5] { 0.5, 0.5, 0.5, 0.5, 0.5 };
            st = s;
        }

        public void create_objects()
        {
            cs = new CarService(rng, numMasters, st);
            cw = new CarWorld(this, cs);
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
                }
                else
                    st.RequestGen = "No request";
                //работа а/с
                cs.work(isWeekend, timeHour, timeMin);
                //изменение счетчиков
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
