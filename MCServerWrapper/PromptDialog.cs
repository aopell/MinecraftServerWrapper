using System.Windows.Forms;

namespace MCServerWrapper
{
    /// <summary>
    /// Dialog box containing a text box and a label
    /// </summary>
    public partial class PromptDialog : Form
    {
        private string Result { get; set; }
        private int maxCharCount;

        private PromptDialog(string description, string title, string initialText, bool multiline, int maxCharCount)
        {
            InitializeComponent();

            textLabel.Text = description;
            Text = title;
            textBox.Multiline = multiline;
            textBox.Text = initialText;
            textBox.MaxLength = maxCharCount;
            Height = multiline ? 200 : 130;
            charCount.Text = $"{textBox.Text.Length} / {maxCharCount}";
            charCount.Visible = maxCharCount != 32767;
            this.maxCharCount = maxCharCount;
        }

        /// <summary>
        /// Creates a new <see cref="PromptDialog"/> and returns the contents of
        /// the text box or <see langword="null"/> if the user cancels the dialog.
        /// </summary>
        /// <param name="prompt">Text to show above the text box; should prompt the user to enter a certain value</param>
        /// <param name="title">Title of the dialog box</param>
        /// <param name="initialText">Initial text of the text box</param>
        /// <param name="multiline">Whether multiline text should be allowed</param>
        /// <param name="maxCharCount">Maximum number of characters allowed in the result</param>
        /// <returns>Contents of the text box or <see langword="null"/> if the user selects cancel</returns>
        public static string Show(string prompt = "", string title = "", string initialText = "", bool multiline = false, int maxCharCount = 32767)
        {
            var dialog = new PromptDialog(prompt, title, initialText, multiline, maxCharCount);
            dialog.ShowDialog();
            return dialog.Result;
        }

        private void cancelButton_Click(object sender, System.EventArgs e)
        {
            Result = null;
            Close();
        }

        private void submitButton_Click(object sender, System.EventArgs e)
        {
            Result = textBox.Text;
            Close();
        }

        private void textBox_TextChanged(object sender, System.EventArgs e)
        {
            charCount.Text = $"{textBox.Text.Length} / {maxCharCount}";
        }
    }
}
