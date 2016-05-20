using System;

namespace RNB_Analyzer
{
    public interface IZveno
    {
        Func<double,double> PF { get; set; }
        Func<double,double> ACH { get; set; }
        Func<double, double> FCH { get; set; }
        Func<double, double> IF { get; set; }
    
    }
}
