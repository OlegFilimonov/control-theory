using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RNB_Analyzer
{
    public class Zveno : IZveno
    {
        public string Name;
        public Func<double, double> PF { get; set; }
        public Func<double, double> ACH { get; set; }
        public Func<double,double> FCH { get; set; }
        public Func<double, double> IF { get; set; }

    }
}
