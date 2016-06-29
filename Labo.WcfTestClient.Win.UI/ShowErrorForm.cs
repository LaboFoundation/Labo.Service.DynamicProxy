using System;
using System.Windows.Forms;

namespace Labo.WcfTestClient.Win.UI
{
    using System.Text;

    public partial class ShowErrorForm : Form
    {
        public ShowErrorForm(Form owner, Exception exception)
        {
            InitializeComponent();

            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;

            Owner = owner;

            txtError.Text = GetExceptionDetails(exception);
        }

        public static DialogResult ShowDialog(Form owner, Exception exception)
        {
            ShowErrorForm showErrorForm = new ShowErrorForm(owner, exception);
            return showErrorForm.ShowDialog(owner);
        }

        private static string GetExceptionDetails(Exception exception)
        {
            Exception currentException = exception;

            StringBuilder sb = new StringBuilder();

            while (currentException != null)
            {
                if (!ReferenceEquals(currentException, exception))
                {
                    sb.AppendLine("Inner Exception: ");
                }

                sb.AppendLine("Message: ");
                sb.AppendLine(exception.Message);
                sb.AppendLine();
                sb.AppendLine("Stack Trace: ");
                sb.AppendLine(exception.StackTrace);
                sb.AppendLine();

                currentException = currentException.InnerException;
            }

            return sb.ToString();
        }
    }
}
