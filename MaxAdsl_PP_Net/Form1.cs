using MaxAdsl_PP_Net.Model;
using MaxAdsl_PP_Net.Utility;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;
using System.Windows.Forms;

namespace MaxAdsl_PP_Net
{
    public partial class Form1 : Form
    {

        private string webUsernameFieldName = Properties.Settings.Default.WebLoginUsernameFieldName;
        private string webPasswordFieldName = Properties.Settings.Default.WebLoginPasswordFieldName;
        string serviceId;

        private Utility.CookieAwareWebClient client;
        private Utility.WebParser webParser;

        private Model.UserSettingsData userSettings;
        
        public Form1()
        {
            InitializeComponent();

            lblResponse.Text = "";
            lblSettingsResponse.Text = "";
            cboWebType.DataSource = Enum.GetValues(typeof(WebParserFactory.WebParserTypes));

            userSettings = Model.UserSettingsData.GetUserSettingsInstance();
            txtUsername.Text = userSettings.Username;
            txtPassword.Text = Properties.Settings.Default.UnmodifiedPasswordDefinition;
            cboWebType.SelectedItem = userSettings.UseWebParser;

        }

        private void btnCheckTraffic_Click(object sender, EventArgs e)
        {
            if (webParser == null)
                webParser = Utility.WebParserFactory.GetWebParser(userSettings.UseWebParser);
            
            NameValueCollection webLoginCredidentials = webParser.GetLoginTokens();
            webLoginCredidentials.Add(webUsernameFieldName, userSettings.Username);
            webLoginCredidentials.Add(webPasswordFieldName, userSettings.Password);

            if (serviceId == null)
                serviceId = webParser.LoginAndGetServiceId(webLoginCredidentials);

            TrafficInfo trafficInfo = webParser.GetTrafficInfo(serviceId);

            lblResponse.Text += "D:" + trafficInfo.Downloaded + Environment.NewLine;
            lblResponse.Text += "U:" + trafficInfo.Uploaded + Environment.NewLine;
            lblResponse.Text += "T:" + trafficInfo.Total + Environment.NewLine;

        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            userSettings.Username = txtUsername.Text;
            if (txtPassword.Text != Properties.Settings.Default.UnmodifiedPasswordDefinition)
                userSettings.Password = txtPassword.Text;
            userSettings.UseWebParser = (WebParserFactory.WebParserTypes)cboWebType.SelectedItem;

            userSettings.StoreSettings();
            MessageBox.Show("Settings saved.");
        }


    }
}
