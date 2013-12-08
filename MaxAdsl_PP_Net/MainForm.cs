using MaxAdsl_PP_Net.Model;
using MaxAdsl_PP_Net.Utility;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;
using System.Windows.Forms;

namespace MaxAdsl_PP_Net
{
    public partial class MainForm : Form
    {
        private string webUsernameFieldName = Properties.Settings.Default.WebLoginUsernameFieldName;
        private string webPasswordFieldName = Properties.Settings.Default.WebLoginPasswordFieldName;
        private Utility.WebParser webParser;
        private Model.UserSettingsData userSettings;
        
        private string serviceId;
        private NameValueCollection webLoginCredidentials;
        private TrafficInfo trafficInfo;
        public MainForm()
        {
            InitializeComponent();

            lblResponse.Text = "";
            lblSettingsResponse.Text = "";
            cboWebType.DataSource = Enum.GetValues(typeof(WebParserFactory.WebParserTypes));
            
            userSettings = Model.UserSettingsData.GetUserSettingsInstance();
            txtUsername.Text = userSettings.Username;
            txtPassword.Text = Properties.Settings.Default.UnmodifiedPasswordDefinition;
            cboWebType.SelectedItem = userSettings.UseWebParser;
            cboCheckTrafficOnStartup.Checked = userSettings.CheckTrafficOnStartup;
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (userSettings.CheckTrafficOnStartup)
                bgwCheckTraffic.RunWorkerAsync();
        }

        private void btnCheckTraffic_Click(object sender, EventArgs e)
        {
            if (bgwCheckTraffic.IsBusy)
            {
                bgwCheckTraffic.CancelAsync();
                webParser.AbortAction = true;
                btnCheckTraffic.Enabled = false;
            }
            else
            {
                btnCheckTraffic.Text = "Cancel";
                if (webParser == null)
                {
                    webParser = Utility.WebParserFactory.GetWebParser(userSettings.UseWebParser);
                    webParser.ActionStart += delegate(object evSender, WebParser.ActionEventArgs evArgs)
                    {
                        appendResponseLabelText(evArgs.Message, false);
                    };
                    webParser.ActionEnd += delegate(object evSender, WebParser.ActionEventArgs evArgs)
                    {
                        appendResponseLabelText(evArgs.Message, true);
                    };
                }
                webParser.AbortAction = false;
                bgwCheckTraffic.RunWorkerAsync();
            }
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            userSettings.Username = txtUsername.Text;
            if (txtPassword.Text != Properties.Settings.Default.UnmodifiedPasswordDefinition)
                userSettings.Password = txtPassword.Text;
            userSettings.UseWebParser = (WebParserFactory.WebParserTypes)cboWebType.SelectedItem;
            userSettings.CheckTrafficOnStartup = cboCheckTrafficOnStartup.Checked;

            userSettings.StoreSettings();
            MessageBox.Show("Settings saved.");
        }

        private void bgwCheckTraffic_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            setResponseLabelText("Checking traffic...", true);
            if (bgwCheckTraffic.CancellationPending)
            {
                appendResponseLabelText(" Aborted", true);
                e.Cancel = true;
                return;
            }

            if (webLoginCredidentials == null)
            {
                webLoginCredidentials = webParser.GetLoginTokens();
                webLoginCredidentials.Add(webUsernameFieldName, userSettings.Username);
                webLoginCredidentials.Add(webPasswordFieldName, userSettings.Password);
            }

            if (bgwCheckTraffic.CancellationPending)
            {
                appendResponseLabelText(" Aborted", true);
                e.Cancel = true;
                return;
            }
            if (serviceId == null)
                serviceId = webParser.LoginAndGetServiceId(webLoginCredidentials);

            if (bgwCheckTraffic.CancellationPending)
            {
                appendResponseLabelText(" Aborted", true);
                e.Cancel = true;
                return;
            }
            trafficInfo = webParser.GetTrafficInfo(serviceId);
        }

        private void bgwCheckTraffic_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            
        }

        private void bgwCheckTraffic_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            btnCheckTraffic.Text = "Check";
            btnCheckTraffic.Enabled = true;
            if (trafficInfo != null)
            {
                lblResponse.Text += "D:" + trafficInfo.Downloaded + Environment.NewLine;
                lblResponse.Text += "U:" + trafficInfo.Uploaded + Environment.NewLine;
                lblResponse.Text += "T:" + trafficInfo.Total + Environment.NewLine;
            }
        }

        // ----------------- Helper methods ----------------------

        private void setResponseLabelText(string message, bool endLine)
        {
            if (endLine)
                message += Environment.NewLine;
            lblResponse.Invoke((MethodInvoker)delegate() { lblResponse.Text = message; });
        }

        private void appendResponseLabelText(string message, bool endLine)
        {
            if (endLine)
                message += Environment.NewLine;
            lblResponse.Invoke((MethodInvoker)delegate() { lblResponse.Text += message; });
        }

        
    }
}
