namespace GCP.PubSub.WinForms;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null!;

    private TextBox txtMessage = null!;
    private Button btnPublish = null!;
    private Button btnSubscribe = null!;
    private Button btnListTopics = null!;
    private Button btnListSubs = null!;
    private ListBox lstMessages = null!;
    private StatusStrip statusStrip = null!;
    private ToolStripStatusLabel statusLabel = null!;
    private Label lblMessage = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        txtMessage = new TextBox();
        btnPublish = new Button();
        btnSubscribe = new Button();
        btnListTopics = new Button();
        btnListSubs = new Button();
        lstMessages = new ListBox();
        statusStrip = new StatusStrip();
        statusLabel = new ToolStripStatusLabel();
        lblMessage = new Label();

        SuspendLayout();

        // lblMessage
        lblMessage.AutoSize = true;
        lblMessage.Location = new Point(12, 15);
        lblMessage.Text = "Message:";

        // txtMessage
        txtMessage.Location = new Point(80, 12);
        txtMessage.Size = new Size(350, 23);

        // btnPublish
        btnPublish.Location = new Point(440, 11);
        btnPublish.Size = new Size(80, 25);
        btnPublish.Text = "Publish";
        btnPublish.Click += BtnPublish_Click;

        // btnSubscribe
        btnSubscribe.Location = new Point(530, 11);
        btnSubscribe.Size = new Size(100, 25);
        btnSubscribe.Text = "Subscribe (10s)";
        btnSubscribe.Click += BtnSubscribe_Click;

        // btnListTopics
        btnListTopics.Location = new Point(12, 45);
        btnListTopics.Size = new Size(100, 25);
        btnListTopics.Text = "List Topics";
        btnListTopics.Click += BtnListTopics_Click;

        // btnListSubs
        btnListSubs.Location = new Point(120, 45);
        btnListSubs.Size = new Size(130, 25);
        btnListSubs.Text = "List Subscriptions";
        btnListSubs.Click += BtnListSubs_Click;

        // lstMessages
        lstMessages.Location = new Point(12, 80);
        lstMessages.Size = new Size(618, 270);

        // statusStrip
        statusStrip.Items.Add(statusLabel);
        statusLabel.Text = "Ready";

        // MainForm
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(644, 381);
        Controls.Add(lblMessage);
        Controls.Add(txtMessage);
        Controls.Add(btnPublish);
        Controls.Add(btnSubscribe);
        Controls.Add(btnListTopics);
        Controls.Add(btnListSubs);
        Controls.Add(lstMessages);
        Controls.Add(statusStrip);
        Text = "GCP Pub/Sub Demo";

        ResumeLayout(false);
        PerformLayout();
    }
}
