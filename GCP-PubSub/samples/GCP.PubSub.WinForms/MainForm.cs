namespace GCP.PubSub.WinForms;

public partial class MainForm : Form
{
    private readonly IPubSubPublisher _publisher;
    private readonly IPubSubSubscriber _subscriber;
    private readonly IPubSubAdmin _admin;

    public MainForm(IPubSubPublisher publisher, IPubSubSubscriber subscriber, IPubSubAdmin admin)
    {
        _publisher = publisher;
        _subscriber = subscriber;
        _admin = admin;
        InitializeComponent();
    }

    private async void BtnPublish_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtMessage.Text))
            return;

        btnPublish.Enabled = false;
        statusLabel.Text = "Publishing...";

        try
        {
            var messageId = await _publisher.PublishAsync(txtMessage.Text);
            lstMessages.Items.Add($"[PUB] ID: {messageId} - {txtMessage.Text}");
            txtMessage.Clear();
            statusLabel.Text = $"Published: {messageId}";
        }
        catch (Exception ex)
        {
            statusLabel.Text = $"Error: {ex.Message}";
        }
        finally
        {
            btnPublish.Enabled = true;
        }
    }

    private async void BtnSubscribe_Click(object sender, EventArgs e)
    {
        btnSubscribe.Enabled = false;
        statusLabel.Text = "Listening for messages (10s)...";

        try
        {
            var count = await _subscriber.PullMessagesAsync(
                async (text, attributes, ct) =>
                {
                    Invoke(() => lstMessages.Items.Add($"[SUB] {text}"));
                    return true; // ACK
                },
                TimeSpan.FromSeconds(10));

            statusLabel.Text = $"Received {count} messages.";
        }
        catch (Exception ex)
        {
            statusLabel.Text = $"Error: {ex.Message}";
        }
        finally
        {
            btnSubscribe.Enabled = true;
        }
    }

    private async void BtnListTopics_Click(object sender, EventArgs e)
    {
        btnListTopics.Enabled = false;
        statusLabel.Text = "Listing topics...";

        try
        {
            var topics = await _admin.ListTopicsAsync();
            lstMessages.Items.Add($"--- {topics.Count} topics ---");
            foreach (var topic in topics)
                lstMessages.Items.Add($"  [TOPIC] {topic}");
            statusLabel.Text = $"Found {topics.Count} topics.";
        }
        catch (Exception ex)
        {
            statusLabel.Text = $"Error: {ex.Message}";
        }
        finally
        {
            btnListTopics.Enabled = true;
        }
    }

    private async void BtnListSubs_Click(object sender, EventArgs e)
    {
        btnListSubs.Enabled = false;
        statusLabel.Text = "Listing subscriptions...";

        try
        {
            var subs = await _admin.ListSubscriptionsAsync();
            lstMessages.Items.Add($"--- {subs.Count} subscriptions ---");
            foreach (var sub in subs)
                lstMessages.Items.Add($"  [SUB] {sub}");
            statusLabel.Text = $"Found {subs.Count} subscriptions.";
        }
        catch (Exception ex)
        {
            statusLabel.Text = $"Error: {ex.Message}";
        }
        finally
        {
            btnListSubs.Enabled = true;
        }
    }
}
