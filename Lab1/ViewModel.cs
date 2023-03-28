using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace Lab3;

[ObservableObject]
public partial class ViewModel
{
    private readonly RandValuesStatisticCalculator rndVal = new();

    private readonly ObservableCollection<ObservableValue> _series1Values;
    private readonly ObservableCollection<ObservableValue> _series2Values;
    public double[,] _tableData { get; set; }

    private int count = 1000;

    public ObservableCollection<ISeries> Series { get; set; }

    [ObservableProperty]
    private string _series2Val;
    public string Series2Values
    {
        get => _series2Val;
        set => SetProperty(ref _series2Val, value);
    }

    public ViewModel()
    {
        _tableData = rndVal.Table;

        // Use ObservableCollections to let the chart listen for changes (or any INotifyCollectionChanged). 
        _series1Values = new ObservableCollection<ObservableValue>();

        foreach (var value in rndVal.TableSecondCol)
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

        Series2Values = $"N: {count} | Expected value: {_series2Values[0].Value:F2} | Mean: " +
            $"{_series2Values[1].Value:F2}";
    }

    public Axis[] XAxes { get; set; } =
    {
        new Axis
        {
            Labels = RandValuesStatisticCalculator.Labels,
            LabelsPaint = new SolidColorPaint(new SKColor(255, 255, 255)),
            LabelsRotation = 0,
            SeparatorsAtCenter = false,
            TicksAtCenter = true
        }
    };



}
