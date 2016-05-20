using System;
using LiveCharts;

public class ViewModel : IObservableChartPoint
{
    private double _yValue;
    private double _xValue;

    public double YValue
    {
        get { return _yValue; }
        set
        {
            _yValue =value;
            PointChanged?.Invoke(this);
        }
    }

    public double XValue
    {
        get { return _xValue; }
        set
        {
            _xValue = value;

            PointChanged?.Invoke(this);
        }
    }

    public event Action<object> PointChanged;
}