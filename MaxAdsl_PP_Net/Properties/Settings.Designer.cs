﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MaxAdsl_PP_Net.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://localhost:3512/Home/Start")]
        public string MockStartUrl {
            get {
                return ((string)(this["MockStartUrl"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://localhost:3512/Home/Login")]
        public string MockLoginUrl {
            get {
                return ((string)(this["MockLoginUrl"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://localhost:3512/Home/TrafficInfo?serviceId=")]
        public string MockTrafficUrl {
            get {
                return ((string)(this["MockTrafficUrl"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://moj.hrvatskitelekom.hr/SSO/Prijava")]
        public string WebStartUrl {
            get {
                return ((string)(this["WebStartUrl"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://servisi.tportal.hr/servisi/rest/oauth1/clientprocess")]
        public string WebLoginUrl {
            get {
                return ((string)(this["WebLoginUrl"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://moj.hrvatskitelekom.hr/internet/ispis-spajanja?serviceid=")]
        public string WebTrafficUrl {
            get {
                return ((string)(this["WebTrafficUrl"]));
            }
        }

        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://moj.hrvatskitelekom.hr/App_Modules__SnT.THTCms.CSC.Modules.Package__SnT.T" +
            "HTCms.CSC.Modules.Profile.MojTProfileService.asmx/VerifyService")]
        public string WebTrafficReadyUrl
        {
            get
            {
                return ((string)(this["WebTrafficReadyUrl"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://m.moj.hrvatskitelekom.hr")]
        public string WebMobileStartUrl {
            get {
                return ((string)(this["WebMobileStartUrl"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://m.moj.hrvatskitelekom.hr/internet/pregled?serviceid=")]
        public string WebMobileTraficUrl {
            get {
                return ((string)(this["WebMobileTraficUrl"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome" +
            "/29.0.1547.57 Safari/537.36")]
        public string EmulateUserAgent {
            get {
                return ((string)(this["EmulateUserAgent"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("username")]
        public string WebLoginUsernameFieldName {
            get {
                return ((string)(this["WebLoginUsernameFieldName"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("password")]
        public string WebLoginPasswordFieldName {
            get {
                return ((string)(this["WebLoginPasswordFieldName"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("unmodified")]
        public string UnmodifiedPasswordDefinition {
            get {
                return ((string)(this["UnmodifiedPasswordDefinition"]));
            }
        }
    }
}
