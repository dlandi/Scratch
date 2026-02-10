namespace AppSysMetrics.Diagnostics;

/// <summary>
/// Apply this attribute at the assembly level to explicitly mark an assembly for
/// memory leak analysis. When any loaded assembly has this attribute, only attributed
/// assemblies are considered "user code" during GC root analysis.
/// If no assemblies have this attribute, auto-discovery is used instead.
/// </summary>
/// <example>
/// <code>[assembly: AppSysMetrics.Diagnostics.AnalyzeMemoryLeaks]</code>
/// </example>
[AttributeUsage(AttributeTargets.Assembly)]
public sealed class AnalyzeMemoryLeaksAttribute : Attribute { }
