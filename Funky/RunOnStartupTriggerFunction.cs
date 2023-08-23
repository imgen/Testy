using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading;
using IronPdf;
using IronPdf.Signing;

namespace Funky
{
    public static class RunOnStartupTriggerFunction
    {
        [FunctionName("RunOnStartupTriggerFunction")]
        public static async Task Run(
            [TimerTrigger("0 0 0 * * 0", RunOnStartup = true)] TimerInfo myTimer, ILogger log, CancellationToken cancellationToken = default)
        {
            License.LicenseKey = "IRONSUITE.HS.TOPO.CC.7939-D602816371-A2C3AACCEBDBX7I6-EJCPHVYJRYUB-HUUWDDTJLIQP-J6NFTDAYOARM-S63MFWW22PKM-PENZXCKAQDEX-M3R4WJ-T4BRFMOBNECKUA-DEPLOYMENT.TRIAL-6RRGIE.TRIAL.EXPIRES.14.SEP.2023";

            var renderer = new ChromePdfRenderer();
            var pdf = renderer.RenderHtmlAsPdf("<h1>Hello World<h1>");
            pdf.SaveAs("HelloWorld.pdf");
        }
    }
}
