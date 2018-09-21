// Copyright Phillip Labar 2017
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Android.Bluetooth;
using Java.IO;

namespace StuartSurveying2.Android
{
    public class BluetoothServer
    {
        MainApp_Android _mainApp = null;
        public CarouselPage _mainPage = null;
        public BluetoothServerSocket _serverSocket = null;
        public BluetoothSocket _bluetoothSocket = null;
       
        // Server objects for listening on incoming data
        public BufferedReader _readerBuffer = null;
        public System.IO.Stream _inStream = null;
        public InputStreamReader _inStreamReader = null;

        public BluetoothServer(MainApp_Android MainApp, BluetoothServerSocket ServerSocket, CarouselPage mainPage)
        {
            _mainApp = MainApp;
            _serverSocket = ServerSocket;
            _mainPage = mainPage;
        }

        public void WaitForConnection()
        {
            _bluetoothSocket = _serverSocket.Accept();

            _serverSocket.Close();

            _inStream = _bluetoothSocket.InputStream;
            _inStreamReader = new InputStreamReader(_inStream);
            _readerBuffer = new BufferedReader(_inStreamReader);
        }

        public void ListenForBTData()
        {           
            while (true)
            {
                if (_readerBuffer != null)
                { 
                    byte[] data = new byte[10];
                    int numBytesRead = _bluetoothSocket.InputStream.Read(data, 0, 3);                  

                    if (numBytesRead > 0)
                    {
                        if (data[0] == 110 && data[1] == 112)
                        {
                            int newPage = data[2];
                            if ( (newPage > -1) && (newPage < _mainPage.Children.Count))
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    _mainPage.CurrentPage = _mainPage.Children[newPage];
                                });
                            }
                        }                        
                        else if(numBytesRead == 3)
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                char a = (char)data[0];
                                char b = (char)data[1];
                                char c = (char)data[2];
                                _mainApp.UpdateLabel(a.ToString() + b.ToString() + c.ToString());
                            });
                        }
                    }

                    Java.Lang.Thread.Sleep(100);
                }
                else
                {
                    Java.Lang.Thread.Sleep(1000);
                }
            }
        }
    }
}