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
        bool modelingState;
        System.Windows.Forms.Timer stepTimer;

        public MainWindow()
        {
            model = new Model();
            modelingState = true;
            stepTimer = new System.Windows.Forms.Timer();
            InitializeComponent();
            model.st = (Statistics)this.Resources["Rg"];
            model.create_objects();
            stepTimer.Enabled = false;
            stepTimer.Tick += OnStepTimer;
        }



        void trueModelingState()
        {
            StartBtn.IsEnabled = true;
            StopBtn.IsEnabled = true;
            StepBox.IsEnabled = true;
        }

        void falseModelingState()
        {
            StartBtn.IsEnabled = false;
            StopBtn.IsEnabled = false;
            StepBox.IsEnabled = false;
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
        
        private void SliderIntens_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            model.intensityRatio = ((Slider)sender).Value;
        }

        private void StepBtn_Click(object sender, RoutedEventArgs e)
        {
            //model.simStep(int.Parse(stepNum.Text));
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
            model.ovDay = (int)((SpinEdit)sender).Value;
        }

        private void MHour_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            model.ovHour = (byte)((SpinEdit)sender).Value;
        }

        private void MMin_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            model.ovMin = (byte)((SpinEdit)sender).Value;
        }

        private void StartHour_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            model.timeHour = (byte)((SpinEdit)sender).Value;
        }

        private void StartMin_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            model.timeMin = (byte)((SpinEdit)sender).Value;
        }

        private void StartDay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            model.timeDay = (byte)((ComboBox)sender).SelectedIndex;
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
                modelingState = false;
                falseModelingState();
                MessageBox.Show("The End");
            }
        }

        private void ws0ErrorPr_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            model.wsErrorPr[0] = ((Slider)sender).Value / 100;
        }

        private void ws1ErrorPr_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            model.wsErrorPr[1] = ((Slider)sender).Value / 100;
        }

        private void ws2ErrorPr_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            model.wsErrorPr[2] = ((Slider)sender).Value / 100;
        }

        private void ws3ErrorPr_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            model.wsErrorPr[3] = ((Slider)sender).Value / 100;
        }

        private void ws4ErrorPr_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            model.wsErrorPr[4] = ((Slider)sender).Value / 100;
        }

        private void SliderPeakIntense_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            model.peakRatio = ((Slider)sender).Value / 100;
        }

        private void SliderLtIntense_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            model.ltRatio = ((Slider)sender).Value / 100;
        }

        private void SliderTotalCarTime_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            model.totalCarTimeInService = (int)(((Slider)sender).Value) * 24 * 60;
        }

        private void StepBox_TextChanged(object sender, TextChangedEventArgs e)
        {
           // if()
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
