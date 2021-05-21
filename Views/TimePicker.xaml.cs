using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FortalezaDesktop.Views
{
    /// <summary>
    /// Interaction logic for TimePicker.xaml
    /// </summary>
    public partial class TimePicker : UserControl
    {
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                "Value",
                typeof(DateTime),
                typeof(TimePicker),
                new FrameworkPropertyMetadata(
                    DateTime.Now,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(
                        (DependencyObject obj, DependencyPropertyChangedEventArgs e) =>
                        {
                            TimePicker timePicker = obj as TimePicker;
                            DateTime newTime = (DateTime) e.NewValue;
                            timePicker.TextHour.Text = newTime.Hour.ToString("00");
                            timePicker.TextMinute.Text = newTime.Minute.ToString("00");
                            timePicker.TextSecond.Text = newTime.Second.ToString("00");
                        })
                    )
                );

        public DateTime Value {
            get { return (DateTime)GetValue(ValueProperty); }
            set {  SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ShowSecondsProperty =
            DependencyProperty.Register(
                "ShowSeconds",
                typeof(bool),
                typeof(TimePicker),
                new FrameworkPropertyMetadata(
                    false,
                    new PropertyChangedCallback(
                        (DependencyObject obj, DependencyPropertyChangedEventArgs e) =>
                        {
                            TimePicker timePicker = obj as TimePicker;
                            if(timePicker.ShowSeconds)
                            {
                                timePicker.TextblockSeconds.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                timePicker.TextblockSeconds.Visibility = Visibility.Collapsed;
                            }
                        })
                    )
                );

        public bool ShowSeconds
        {
            get { return (bool)GetValue(ShowSecondsProperty); }
            set { SetValue(ShowSecondsProperty, value); }
        }

        public static readonly DependencyProperty AllowChangesProperty =
            DependencyProperty.Register(
                "AllowChanges",
                typeof(bool),
                typeof(TimePicker),
                new FrameworkPropertyMetadata(
                    true,
                    new PropertyChangedCallback(
                        (DependencyObject obj, DependencyPropertyChangedEventArgs e) =>
                        {
                            TimePicker timePicker = obj as TimePicker;
                            if(timePicker.AllowChanges)
                            {
                                timePicker.ColumnAdjust.Width = new GridLength(20);
                                timePicker.ButtonIncrease.IsEnabled = true;
                                timePicker.ButtonDecrease.IsEnabled = true;
                            }
                            else
                            {
                                timePicker.ColumnAdjust.Width = new GridLength(0);
                                timePicker.ButtonIncrease.IsEnabled = false;
                                timePicker.ButtonDecrease.IsEnabled = false;
                            }
                        })
                    )
                );

        public bool AllowChanges
        {
            get { return (bool)GetValue(AllowChangesProperty); }
            set { SetValue(AllowChangesProperty, value); }
        }

        public TimePicker()
        {
            InitializeComponent();
            TextblockSeconds.Visibility = Visibility.Collapsed;
        }


        private void ButtonIncrease_Click(object sender, RoutedEventArgs e)
        {
            Value.AddHours(1);
        }

        private void ButtonDecrease_Click(object sender, RoutedEventArgs e)
        {
            Value.AddHours(-1);
        }
    }
}
