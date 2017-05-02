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
using Windows.Storage;
using Windows.Devices.Bluetooth;
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

        /**
         * start wifi test
         */
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
            StorageFolder storageFolder = KnownFolders.PicturesLibrary;
            StorageFile sampleFile = await storageFolder.CreateFileAsync("wifiTestLog.txt", CreationCollisionOption.GenerateUniqueName);
            Radio wifiRadio = radios.FirstOrDefault(radio => radio.Kind == RadioKind.WiFi);
            if (wifiRadio == null)
            {
                textBlock.Text = "wifi cannot open";
                button.IsEnabled = true;
                button1.IsEnabled = true;
                return;
            }
            // wifiRadio.StateChanged = Radio_StateChange
            textBlock.Text = "0";
            textBlock5.Text = "0";
            textBlock6.Text = "0";
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
                await Task.Delay(20000);
                //StorageFile sampleFile = await storageFolder.GetFileAsync("wifiTestLog.txt");
                await FileIO.AppendTextAsync(sampleFile , DateTime.Now.ToString() + " wifi open successfully,test count:" + (count+1)+"\r\n");
                //var Foder = KnownFolders.DocumentsLibrary.ToString() + "\\WifiTestLog.txt";
                //var Folder = "D:\\WifiTestLog.txt";
                /*
                await Task.Run(() =>
                {
                    var folder = KnownFolders.PicturesLibrary.ToString();
                    
                    //File.SetAttributes(Folder, System.IO.FileAttributes.Normal);
                    File.WriteAllText(folder, DateTime.Now.ToString() + "wifi open successfully,test count:" + count);                  
                });
                */
                var temp = NetworkInformation.GetInternetConnectionProfile();
                if (temp != null && temp.IsWlanConnectionProfile)
                {
                    sucesscount++;
                    textBlock5.Text = Convert.ToString(sucesscount);
                    // File.AppendAllText(Folder, DateTime.Now.ToString() + "wifi connected to the Internet,success count:" + count);
                    await FileIO.AppendTextAsync(sampleFile, DateTime.Now.ToString() + " wifi connected to the Internet,success count:" + sucesscount + "\r\n");
                }
                else
                {
                    failcount++;
                    textBlock6.Text = Convert.ToString(failcount);
                    // File.AppendAllText(Folder, DateTime.Now.ToString() + "wifi cannot connect to the Internet,fail count:" + count);
                    await FileIO.AppendTextAsync(sampleFile, DateTime.Now.ToString() + " wifi cannot connect to the Internet,fail count:" + failcount + "\r\n");
                }
                
            }
            button.IsEnabled = true;
            button1.IsEnabled = true;
        }
        /**
         * start bluetooth test
         */
        private async void button1_Click(object sender, RoutedEventArgs e)
        {
            button.IsEnabled = false;
            button1.IsEnabled = false;
            var i = textBox.Text;
            int number = Convert.ToInt32(i);
            var sucessCnt =0;
            var failCnt = 0;
            var accessLevel = await Radio.RequestAccessAsync();
            var radios = await Radio.GetRadiosAsync();
            StorageFolder storageFolder = KnownFolders.PicturesLibrary;
            StorageFile sampleFile = await storageFolder.CreateFileAsync("BTTestLog.txt", CreationCollisionOption.GenerateUniqueName);
            Radio blueRadio = radios.FirstOrDefault(radio => radio.Kind == RadioKind.Bluetooth);
            if (blueRadio == null)
            {
                textBlock.Text = "bluetooth cannot open, please turn on the bluetooth first";
                button.IsEnabled = true;
                button1.IsEnabled = true;
                return;
            }
            // wifiRadio.StateChanged = Radio_StateChange
            textBlock.Text = "0";
            textBlock5.Text = "0";
            textBlock6.Text = "0";
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
                await FileIO.AppendTextAsync(sampleFile, DateTime.Now.ToString() + " BT open successfully,test count:" + (count + 1) + "\r\n");
                await Task.Delay(20000);
                var selector = BluetoothDevice.GetDeviceSelectorFromConnectionStatus(BluetoothConnectionStatus.Connected);
                var btDevices = await DeviceInformation.FindAllAsync(selector);
                var btDevice = btDevices.FirstOrDefault();
                if (btDevice == null)
                {
                    failCnt++;
                    textBlock6.Text = Convert.ToString(failCnt);
                    await FileIO.AppendTextAsync(sampleFile, DateTime.Now.ToString() + " wifi cannot connect to the Internet,fail count:" + failCnt + "\r\n");
                }
                else
                {
                    sucessCnt++;
                    textBlock5.Text = Convert.ToString(sucessCnt);
                    await FileIO.AppendTextAsync(sampleFile, DateTime.Now.ToString() + " wifi connected to the Internet,success count:" + sucessCnt + "\r\n");
                }
                
            }
            button.IsEnabled = true;
            button1.IsEnabled = true;
        }    
    }   
}
