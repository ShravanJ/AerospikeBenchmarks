using System;
using System.Collections.Generic;
using System.Text;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;

namespace AerospikeBenchmarks
{
    public class BenchmarkConfig : ManualConfig
    {
        public BenchmarkConfig()
        {
            AddColumn(StatisticColumn.P0,
                StatisticColumn.P50,
                StatisticColumn.P90,
                StatisticColumn.P95,
                StatisticColumn.P100);

            AddDiagnoser(MemoryDiagnoser.Default);

        }
    }
}
