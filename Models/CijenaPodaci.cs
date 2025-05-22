using System;
using System.Collections.Generic;

namespace YourApp.Models
{
    public class CijenaPodaci
    {
        public string Proizvod { get; set; }
        public List<DataPoint> Podaci { get; set; }
    }

    public class DataPoint
    {
        public DateTime Datum { get; set; }
        public double Cijena { get; set; }
    }
}