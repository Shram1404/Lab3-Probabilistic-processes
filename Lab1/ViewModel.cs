using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace Lab1;

[ObservableObject]
public partial class ViewModel
{
    private readonly RandValuesStatisticCalculator rndVal = new();

    private readonly ObservableCollection<ObservableValue> _series1Values;
    private readonly ObservableCollection<ObservableValue> _series2Values;
    private int count = 10;
    public ObservableCollection<ISeries> Series { get; set; }
    public ObservableCollection<ISeries> Series2 { get; set; }
    [ObservableProperty]
    private string _series2Val;
    public string Series2Values
    {
        get => _series2Val;
        set => SetProperty(ref _series2Val, value);
    }



    public ViewModel()
    {
        // Use ObservableCollections to let the chart listen for changes (or any INotifyCollectionChanged). 
        _series1Values = new ObservableCollection<ObservableValue>();

        foreach (var value in rndVal.countArray)
            _series1Values.Add(new ObservableValue(value));

        Series = new ObservableCollection<ISeries>
        {
            new ColumnSeries<ObservableValue>
            {
                Values = _series1Values,
                Name = "Count of number",
                DataLabelsPaint = new SolidColorPaint(new SKColor(255, 255, 255)),
            },
        };

        _series2Values = new ObservableCollection<ObservableValue>();

        foreach (var value in rndVal.statistics)
            _series2Values.Add(new ObservableValue(value));

        Series2 = new ObservableCollection<ISeries>
        {
             new PieSeries<ObservableValue> { Values = new List<ObservableValue> { _series2Values[0] }, Pushout = 4, Name = "Expected value" },
             new PieSeries<ObservableValue> { Values = new List<ObservableValue> { _series2Values[2] }, Pushout = 4, Name = "Standard deviation" },
             new PieSeries<ObservableValue> { Values = new List<ObservableValue> { _series2Values[3] }, Pushout = 4, Name = "Log" },
         };

        UpdateSeries2Values();
    }


    public Axis[] XAxes { get; set; } =
    {
        new Axis
        {
            Labels = new string[] { "1", "2", "3","4", "5","6" },
            LabelsPaint = new SolidColorPaint(new SKColor(255, 255, 255)),
            LabelsRotation = 0,
            SeparatorsAtCenter = false,
            TicksAtCenter = true
        }
    };


    [ICommand]
    public void AddItem()
    {
        if (count < 10000)
        {
            count *= 10;
            UpdateItem();
        }
    }

    [ICommand]
    public void RemoveItem()
    {
        if (count > 10)
        {
            count /= 10;
            UpdateItem();
        }
    }

    [ICommand]
    public void UpdateItem()
    {
        ObservableValue currentInstance;

        rndVal.AddRandValues(count);
        for (int i = 0; i < rndVal.countArray.Length; i++)
        {
            currentInstance = _series1Values[i];
            currentInstance.Value = rndVal.countArray[i];
        }

        rndVal.CalculateStatistics(count);
        for (int i = 0; i < rndVal.statistics.Length; i++)
        {
            currentInstance = _series2Values[i];
            currentInstance.Value = rndVal.statistics[i];
            UpdateSeries2Values();
        }
    }
    private void UpdateSeries2Values()
    {
            Series2Values = $"N: {count} | Expected value: {_series2Values[0].Value:F2} | Mean: " +
            $"{_series2Values[1].Value:F2}\nStandard deviation: {_series2Values[2].Value:F2} | Log: {_series2Values[3].Value:F2}";
    }
}
