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
        private Utility.IWebParser webParser;
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

            if (userSettings.CheckTrafficOnStartup)
                bgwCheckTraffic.RunWorkerAsync();

        }

        private void btnCheckTraffic_Click(object sender, EventArgs e)
        {
            bgwCheckTraffic.RunWorkerAsync();
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
            btnCheckTraffic.Invoke((MethodInvoker)delegate { btnCheckTraffic.Text = "Cancel"; });

            if (webParser == null)
                webParser = Utility.WebParserFactory.GetWebParser(userSettings.UseWebParser);

            setResponseLabelText("Checking traffic..." + Environment.NewLine);

            if (webLoginCredidentials == null)
            {
                appendResponseLabelText("Getting login tokens...");
                webLoginCredidentials = webParser.GetLoginTokens();
                appendResponseLabelText(" OK" + Environment.NewLine);
                webLoginCredidentials.Add(webUsernameFieldName, userSettings.Username);
                webLoginCredidentials.Add(webPasswordFieldName, userSettings.Password);
            }

            if (serviceId == null)
            {
                appendResponseLabelText("Getting service id...");
                serviceId = webParser.LoginAndGetServiceId(webLoginCredidentials);
                appendResponseLabelText(" OK" + Environment.NewLine);
            }

            appendResponseLabelText("Getting traffic info...");
            trafficInfo = webParser.GetTrafficInfo(serviceId);
            appendResponseLabelText(" OK" + Environment.NewLine);
        }

        private void bgwCheckTraffic_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            
        }

        private void bgwCheckTraffic_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            btnCheckTraffic.Text = "Check";
            lblResponse.Text += "D:" + trafficInfo.Downloaded + Environment.NewLine;
            lblResponse.Text += "U:" + trafficInfo.Uploaded + Environment.NewLine;
            lblResponse.Text += "T:" + trafficInfo.Total + Environment.NewLine;
        }

        private void setResponseLabelText(string message)
        {
            lblResponse.Invoke((MethodInvoker)delegate() { lblResponse.Text = message; });
        }

        private void appendResponseLabelText(string message)
        {
            lblResponse.Invoke((MethodInvoker)delegate() { lblResponse.Text += message; });
        }

    }
}
