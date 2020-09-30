﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BiStatApp.Views
{

    public class MainPageMasterMenuItem
    {
        public MainPageMasterMenuItem()
        {
            TargetType = typeof(MainPageMasterMenuItem);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }

        public ImageSource Icon { get; set; }
    }
}