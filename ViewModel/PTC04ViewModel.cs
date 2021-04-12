using AUA.AiS_FruiT;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;
using CargoLampTest.Model;

namespace CargoLampTest.ViewModel
{ 
    public class PTC04ViewModel : ViewModelBase
    {
        public DevicePTC04 _devicePTC04 = new DevicePTC04();

    }
}
