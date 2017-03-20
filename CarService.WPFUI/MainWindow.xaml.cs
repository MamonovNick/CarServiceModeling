using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CarService.Logic;
using DevExpress.Xpf.Editors;

namespace CarService.WPFUI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Model model;
        System.Windows.Forms.Timer stepTimer;

        public MainWindow()
        {
            model = new Model();
            stepTimer = new System.Windows.Forms.Timer();
            InitializeComponent();
            model.st = (Statistics)this.Resources["Rg"];
            model.create_objects();
            stepTimer.Enabled = false;
            stepTimer.Tick += OnStepTimer;
        }

        private void TS_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            model.numMasters[2] = (byte)((SpinEdit)sender).Value;
            this.pb2.Maximum = (byte)((SpinEdit)sender).Value;
        }

        private void BS_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            model.numMasters[1] = (byte)((SpinEdit)sender).Value;
            this.pb1.Maximum = (byte)((SpinEdit)sender).Value;
        }

        private void TI_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            model.numMasters[0] = (byte)((SpinEdit)sender).Value;
            this.pb0.Maximum = (byte)((SpinEdit)sender).Value;
        }

        private void RG_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            model.numMasters[3] = (byte)((SpinEdit)sender).Value;
            this.pb3.Maximum = (byte)((SpinEdit)sender).Value;
        }

        private void RE_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            model.numMasters[4] = (byte)((SpinEdit)sender).Value;
            this.pb4.Maximum = (byte)((SpinEdit)sender).Value;
        }

        private void SliderOverallTime_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //model.overallTime = (int)((Slider)sender).Value * 24 * 60;
        }

        private void SliderError_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
        
        private void SliderIntens_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //Txt2.Text = ((Slider)sender).Value.ToString();
            model.intensityRatio = ((Slider)sender).Value;
        }

        private void StartStopBtn_Click(object sender, RoutedEventArgs e)
        {
            model.modeling();
            ProfitTxt.Text = model.getCSProfit();
        }

        private void StepBtn_Click(object sender, RoutedEventArgs e)
        {
            model.simStep(int.Parse(stepNum.Text));
        }

        private void pb0_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double v = ((ProgressBar)sender).Value;
            double m = ((ProgressBar)sender).Maximum;

            if (v >= 0.75 * m)
                ((ProgressBar)sender).Foreground = new SolidColorBrush(Colors.Red);
            else
                ((ProgressBar)sender).Foreground = new SolidColorBrush(Colors.Green);
        }

        private void pb1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double v = ((ProgressBar)sender).Value;
            double m = ((ProgressBar)sender).Maximum;

            if (v >= 0.75 * m)
                ((ProgressBar)sender).Foreground = new SolidColorBrush(Colors.Red);
            else
                ((ProgressBar)sender).Foreground = new SolidColorBrush(Colors.Green);
        }

        private void pb2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double v = ((ProgressBar)sender).Value;
            double m = ((ProgressBar)sender).Maximum;

            if (v >= 0.75 * m)
                ((ProgressBar)sender).Foreground = new SolidColorBrush(Colors.Red);
            else
                ((ProgressBar)sender).Foreground = new SolidColorBrush(Colors.Green);
        }

        private void pb3_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double v = ((ProgressBar)sender).Value;
            double m = ((ProgressBar)sender).Maximum;

            if (v >= 0.75 * m)
                ((ProgressBar)sender).Foreground = new SolidColorBrush(Colors.Red);
            else
                ((ProgressBar)sender).Foreground = new SolidColorBrush(Colors.Green);
        }

        private void pb4_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double v = ((ProgressBar)sender).Value;
            double m = ((ProgressBar)sender).Maximum;

            if (v >= 0.75 * m)
                ((ProgressBar)sender).Foreground = new SolidColorBrush(Colors.Red);
            else
                ((ProgressBar)sender).Foreground = new SolidColorBrush(Colors.Green);
        }

        private void MDay_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {

        }

        private void MHour_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {

        }

        private void MMin_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {

        }

        private void StartHour_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            model.timeDay = byte.Parse(((TextBlock)sender).Text);
        }

        private void StartMin_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            model.timeHour = byte.Parse(((TextBlock)sender).Text);
        }

        private void StartDay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            model.timeMin = byte.Parse(((TextBlock)sender).Text);
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            stepTimer.Start();

        }

        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            stepTimer.Stop();
        }

        private void stepNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            stepTimer.Interval = int.Parse(((TextBox)sender).Text);
        }

        private void OnStepTimer(object sender, EventArgs e)
        {
            model.simStep();

            if (model.overallTime == 0)
            {
                stepTimer.Stop();
                MessageBox.Show("The End");
            }
        }

        private void stepNum_TextInput(object sender, TextCompositionEventArgs e)
        {
            MessageBox.Show("Input");
        }
    }
}
