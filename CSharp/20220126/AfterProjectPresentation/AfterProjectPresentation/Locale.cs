﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfterProjectPresentation
{
    public class Locale
    {
        public string Name { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }

        //alt + enter
        public Locale(string name, double lat, double lng)
        {
            Name = name;
            Lat = lat;
            Lng = lng;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
