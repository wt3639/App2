using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.Radios;
using Windows.Networking.Connectivity;
using Windows.Devices.Enumeration;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace App2
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();          
        }      

        private async void button_Click(object sender, RoutedEventArgs e)
        {            
            button.IsEnabled = false;
            button1.IsEnabled = false;
            var i = textBox.Text;
            int number = Convert.ToInt32(i);
            var accessLevel = await Radio.RequestAccessAsync();
            var radios = await Radio.GetRadiosAsync();
            var sucesscount = 0;
            var failcount = 0;
            Radio wifiRadio = radios.FirstOrDefault(radio => radio.Kind == RadioKind.WiFi);
            if (wifiRadio == null)
            {
                textBlock.Text = "wifi cannot open";
                button.IsEnabled = true;
                button1.IsEnabled = true;
                return;
            }
            // wifiRadio.StateChanged = Radio_StateChange
            if (wifiRadio.State == RadioState.Off)
            {
                await wifiRadio.SetStateAsync(RadioState.On);
               // textBlock.Text = Convert.ToString(count + 1);
            }
            await Task.Delay(10000);
            for (int count = 0; count < number; count++)
            {
                await wifiRadio.SetStateAsync(RadioState.Off);
                await Task.Delay(5000);
                await wifiRadio.SetStateAsync(RadioState.On);
                textBlock.Text = Convert.ToString(count+1);                 
                await Task.Delay(10000);
                var temp = NetworkInformation.GetInternetConnectionProfile();
                if (temp != null && temp.IsWlanConnectionProfile)
                {
                    sucesscount++;
                    textBlock5.Text = Convert.ToString(sucesscount);
                }
                else
                {
                    failcount++;
                    textBlock6.Text = Convert.ToString(failcount);
                }
            }
            button.IsEnabled = true;
            button1.IsEnabled = true;
        }

        private async void button1_Click(object sender, RoutedEventArgs e)
        {
            button.IsEnabled = false;
            button1.IsEnabled = false;
            var i = textBox.Text;
            int number = Convert.ToInt32(i);
            var accessLevel = await Radio.RequestAccessAsync();
            var radios = await Radio.GetRadiosAsync();
            Radio blueRadio = radios.FirstOrDefault(radio => radio.Kind == RadioKind.Bluetooth);
            if (blueRadio == null)
            {
                textBlock.Text = "bluetooth cannot open";
                button.IsEnabled = true;
                button1.IsEnabled = true;
                return;
            }            
            // wifiRadio.StateChanged = Radio_StateChange
            if (blueRadio.State == RadioState.Off)
            {
                await blueRadio.SetStateAsync(RadioState.On);
                //textBlock.Text = Convert.ToString(count + 1);
            }
            for (int count = 0; count < number; count++)
            {         
                await blueRadio.SetStateAsync(RadioState.Off);
                await Task.Delay(5000);
                await blueRadio.SetStateAsync(RadioState.On);
                textBlock.Text = Convert.ToString(count + 1);                          
                await Task.Delay(10000);
                
                
                textBlock5.Text = Convert.ToString(count + 1);
            }
            button.IsEnabled = true;
            button1.IsEnabled = true;
        }    
    }   
}
