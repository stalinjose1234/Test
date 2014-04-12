using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using System.Windows.Controls.Primitives;
using myanumber.Resources;
using System.Windows.Media.Animation;
using System.Windows.Input;


namespace myanumber
{
    public partial class Twinning : PhoneApplicationPage
    {
        public Twinning()
        {
            InitializeComponent();
            //VisualStateManager.GoToState(this, "Normal", false); 
        }

        private void Menu_Tap(object sender, RoutedEventArgs e)
        {
            var left = Canvas.GetLeft(LayoutRoot);
            if (left > -100)
            {
                MoveViewWindow(-420);
            }
            else
            {
                MoveViewWindow(0);
            }
        }

        void MoveViewWindow(double left)
        {
            _viewMoved = true;
            ((Storyboard)canvas.Resources["moveAnimation"]).SkipToFill();
            ((DoubleAnimation)((Storyboard)canvas.Resources["moveAnimation"]).Children[0]).To = left;
            ((Storyboard)canvas.Resources["moveAnimation"]).Begin();
        }

        private void canvas_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            if (e.DeltaManipulation.Translation.X != 0)
                Canvas.SetLeft(LayoutRoot, Math.Min(Math.Max(-420, Canvas.GetLeft(LayoutRoot) + e.DeltaManipulation.Translation.X), 0));
        }

        double initialPosition;
        bool _viewMoved = false;

        private void canvas_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            _viewMoved = false;
            initialPosition = Canvas.GetLeft(LayoutRoot);
        }

        private void canvas_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            var left = Canvas.GetLeft(LayoutRoot);
            if (_viewMoved)
                return;
            if (Math.Abs(initialPosition - left) < 100)
            {
                //bouncing back
                MoveViewWindow(initialPosition);
                return;
            }
            //change of state
            if (initialPosition - left > 0)
            {
                //slide to the left
                if (initialPosition > -420)
                    MoveViewWindow(-420);
                else
                    MoveViewWindow(-420);
            }
            else
            {
                //slide to the right
                if (initialPosition < -420)
                    MoveViewWindow(-420);
                else
                    MoveViewWindow(0);
            }
        }

        private void TwinningToggleSwitch_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                TwinningToggleSwitch.Content = "On";
                TwinningStatusPanel.Background =  new SolidColorBrush(Color.FromArgb(255, 59, 206, 32));
                TwinningStatusTextBlock.Text = AppResources.TwinningOnStatus;
            }
            catch (Exception ex) 
            {
                throw;
            }
        }

        private void TwinningToggleSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                TwinningToggleSwitch.Content = "Off";
                TwinningStatusPanel.Background = new SolidColorBrush(Color.FromArgb(255, 71, 71, 71));
                TwinningStatusTextBlock.Text = AppResources.TwinningOffStatus;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void TwinningToggleSwitch_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Click");
        }

        private void TwinningToggleSwitch_Indeterminate(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Its intermidiate");
        }

        private void SettingsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // MessageBox.Show("List Item " + (SettingsList.SelectedIndex + 1) + " pressed.");
            //SettingsList.Background = new SolidColorBrush(Colors.Gray);            
        }
        private void Unlink_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                CustomMessageBox messageBox = new CustomMessageBox()
                {
                    Caption = "Unlink Number?",
                    Message = "Are you sure you want to unlink?You will need to start the twinning process in order to link your devices again.",
                    LeftButtonContent = "Cancel",
                    RightButtonContent = "Unlink",
                    Background = new SolidColorBrush(Colors.White),
                    VerticalAlignment = System.Windows.VerticalAlignment.Center,
                    Foreground = new SolidColorBrush(Colors.Black)
                };

                messageBox.Dismissed += (s1, e1) =>
                {
                    switch (e1.Result)
                    {
                        case CustomMessageBoxResult.LeftButton:
                            // Do something.
                            break;
                        case CustomMessageBoxResult.RightButton:
                            // Do something.
                            break;
                        case CustomMessageBoxResult.None:
                            // Do something.
                            break;
                        default:
                            break;
                    }
                };
                messageBox.Show();
            }
            catch (Exception ex) 
            {
 
            }
        }

        private void TwinningPage_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new Uri("/Twinning.xaml", UriKind.Relative));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void FAQPage_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                FAQLabel.Foreground = new SolidColorBrush(Color.FromArgb(255,255,255,255));
                FAQPage.Background = new SolidColorBrush(Color.FromArgb(255, 51, 51, 51));
                NavigationService.Navigate(new Uri("/FAQ.xaml", UriKind.Relative));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void AboutPage_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new Uri("/About.xaml", UriKind.Relative));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}