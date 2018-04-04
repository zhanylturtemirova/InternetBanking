using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using System.Management.Automation;

namespace AcceptionsTest
{
    [Binding]
    public class AppPublishHook
    {
        [BeforeTestRun()]
        public static void Publish()
        {
            using (PowerShell PowerShellInstance = PowerShell.Create())
            {
                PowerShellInstance.AddScript(File.ReadAllText("D://C#course//C#ESDP//git_project_development//onlinebankingweb//InternetBanking//AcceptionsTest//PublishScript.ps1"));
                PowerShellInstance.Invoke();
            }
        }
    }
}
