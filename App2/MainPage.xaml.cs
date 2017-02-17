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
            Radio wifiRadio = radios.FirstOrDefault(radio => radio.Kind == RadioKind.WiFi);
            if (wifiRadio == null)
            {
                textBlock.Text = "wifi cannot open";
                return;
            }
            // wifiRadio.StateChanged = Radio_StateChange
            for (int count = 0; count < number; count++)
            {  
                if (wifiRadio.State == RadioState.On)
                {
                    await wifiRadio.SetStateAsync(RadioState.Off);
                    textBlock.Text = Convert.ToString(count+1);
                }
                else
                { 
                    await wifiRadio.SetStateAsync(RadioState.On);
                    textBlock.Text = Convert.ToString(count+1);                 
                }
                await Task.Delay(5000);
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
                return;
            }
            // wifiRadio.StateChanged = Radio_StateChange
            for (int count = 0; count < number; count++)
            {
                if (blueRadio.State == RadioState.On)
                {
                    await blueRadio.SetStateAsync(RadioState.Off);
                    textBlock.Text = Convert.ToString(count + 1);                
                }
                else
                {
                    await blueRadio.SetStateAsync(RadioState.On);
                    textBlock.Text = Convert.ToString(count + 1);                  
                }
                await Task.Delay(5000);
            }
            button.IsEnabled = true;
            button1.IsEnabled = true;
        }    
    }   
}
