namespace PerfAnalyzers.Benchmarks
{
    using System;
    using System.Linq;
    using System.Reflection;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Columns;
    using BenchmarkDotNet.Configs;
    using BenchmarkDotNet.Diagnosers;
    using BenchmarkDotNet.Diagnostics.Windows;
    using BenchmarkDotNet.Environments;
    using BenchmarkDotNet.Exporters;
    using BenchmarkDotNet.Jobs;
    using BenchmarkDotNet.Loggers;
    using BenchmarkDotNet.Running;
    using BenchmarkDotNet.Validators;

    public class Program
    {
        public static void Main(string[] args)
        {
#if DEBUG
            throw new InvalidOperationException("Only run from Release builds!");
#endif

            Type[] benchmarks = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetMethods(BindingFlags.Instance | BindingFlags.Public).Any(m => m.GetCustomAttributes(typeof(BenchmarkAttribute), false).Any()))
                .OrderBy(t => t.Name)
                .ToArray();

            new BenchmarkSwitcher(benchmarks).Run();
        }
    }

    public class DefaultConfig : ManualConfig
    {
        public DefaultConfig()
        {
            Add(this.GetColumnProviders().ToArray());

            this.UnionRule = ConfigUnionRule.AlwaysUseLocal;

            Add(JitOptimizationsValidator.FailOnError);
            Add(ExecutionValidator.FailOnError);
            Add(TargetMethodColumn.Method);
            Add(new ParamColumn("DataSource"));
            Add(BaselineScaledColumn.Scaled);
            Add(StatisticColumn.Median);
            Add(StatisticColumn.P90);
            Add(MemoryDiagnoser.Default);

            // HACK
            Add(new MetricColumn(MemoryDiagnoser.Default.ProcessResults(new DiagnoserResults(null, 0, new BenchmarkDotNet.Engines.GcStats())).Single(x => x.Descriptor.Id.Equals("Allocated Memory")).Descriptor));
            Add(new InliningDiagnoser());
            Add(DisassemblyDiagnoser.Create(new DisassemblyDiagnoserConfig(false, true, true)));

            Add(Job.Dry
                .With(Platform.X64)
                .With(Jit.RyuJit)
                .With(Runtime.Clr)
                .WithGcServer(true)
                .WithWarmupCount(1)
                .WithLaunchCount(1)
                .WithTargetCount(50)
                .WithRemoveOutliers(true)
                .WithAnalyzeLaunchVariance(true)
                .WithEvaluateOverhead(true));

            Add(ConsoleLogger.Default);
            Add(AsciiDocExporter.Default);
        }
    }
}