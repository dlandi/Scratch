using System.ComponentModel.DataAnnotations;

namespace GCP.PubSub;

public class PubSubOptions
{
    [Required(AllowEmptyStrings = false)]
    public string ProjectId { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false)]
    public string TopicId { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false)]
    public string SubscriptionId { get; set; } = string.Empty;
}
