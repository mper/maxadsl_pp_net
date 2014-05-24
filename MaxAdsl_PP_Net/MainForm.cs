using MaxAdsl_PP_Net.Model;
using MaxAdsl_PP_Net.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;
using System.Windows.Forms;

namespace MaxAdsl_PP_Net
{
    public partial class MainForm : Form
    {
        private Utility.WebParser webParser;
        private Model.UserSettingsData userSettings;
        
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
                checkTraffic();
        }

        private void btnCheckTraffic_Click(object sender, EventArgs e)
        {
            if (webParser != null && webParser.IsRunning)
            {
                webParser.AbortAction = true;
                btnCheckTraffic.Enabled = false;
            }
            else
            {
                checkTraffic();
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

        /*
        private void bgwCheckTraffic_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            setResponseLabelText("Checking traffic...", true);
            if (bgwCheckTraffic.CancellationPending)
            {
                appendResponseLabelText(" Aborted", true);
                e.Cancel = true;
                return;
            }

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

            if (webLoginCredentials == null)
            {
                webLoginCredentials = webParser.GetLoginTokensStep();
                webLoginCredentials.Add(webUsernameFieldName, userSettings.Username);
                webLoginCredentials.Add(webPasswordFieldName, userSettings.Password);
            }

            if (bgwCheckTraffic.CancellationPending)
            {
                appendResponseLabelText(" Aborted", true);
                e.Cancel = true;
                return;
            }
            if (serviceId == null)
                serviceId = webParser.LoginAndGetServiceIdStep(webLoginCredentials);

            if (bgwCheckTraffic.CancellationPending)
            {
                appendResponseLabelText(" Aborted", true);
                e.Cancel = true;
                return;
            }
            trafficInfo = webParser.GetTrafficInfoStep(serviceId);
        }
         */


        // ----------------- Helper methods ----------------------

        private void checkTraffic()
        {
            btnCheckTraffic.Text = "Cancel";
            setResponseLabelText("Checking traffic...", true);

            if (webParser == null)
            {
                webParser = Utility.WebParserFactory.GetWebParser(userSettings.UseWebParser);
                webParser.OnActionStart += delegate(object sender, WebParser.ActionEventArgs args)
                {
                    appendResponseLabelText(args.Message, false);
                };
                webParser.OnActionEnd += delegate(object sender, WebParser.ActionEventArgs args)
                {
                    appendResponseLabelText(args.Message, true);
                };
                webParser.OnTrafficInfoComplete += delegate(object sender, WebParser.TrafficInfoEventArgs args)
                {
                    lblResponse.Invoke((MethodInvoker)delegate()
                    {
                        btnCheckTraffic.Text = "Check";
                        btnCheckTraffic.Enabled = true;
                        appendTrafficInfo(args.TrafficInfo);
                    });
                };
            }

            webParser.GetTrafficInfoAsync(userSettings.Username, userSettings.Password);
        }

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

        private void appendTrafficInfo(TrafficInfo trafficInfo)
        {
            if (trafficInfo != null)
            {
                    lblResponse.Text += "D:" + trafficInfo.Downloaded + Environment.NewLine;
                    lblResponse.Text += "U:" + trafficInfo.Uploaded + Environment.NewLine;
                    lblResponse.Text += "T:" + trafficInfo.Total + Environment.NewLine;
            }
        }

        
    }
}
