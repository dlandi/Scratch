using AppSysMetrics.Models;

namespace AppSysMetrics.Collection;

public interface IMetricsCollector
{
    MetricsSnapshot Collect();
}
