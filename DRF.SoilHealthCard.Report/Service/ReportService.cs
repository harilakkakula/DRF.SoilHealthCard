using Microsoft.Reporting.WebForms;
using System;
using System.Reflection;

namespace DRF.SoilHealthCard.Report.Service
{

    public interface IReportService
    {
        byte[] GenerateReportAsync(string reportName);
    }
    public class ReportService : IReportService
    {
        public byte[] GenerateReportAsync(string reportName)
        {
            using (var localReport = new LocalReport())
            {
                string fileDirPath = Assembly.GetExecutingAssembly().Location.Replace("ReportAPI.dll", string.Empty);
                string rdlcFilePath = string.Format("{0}Report\\{1}.rdlc", fileDirPath, reportName);
                localReport.ReportPath = rdlcFilePath;
                byte[] pdf = localReport.Render("PDF"); //rendering
                var Result = Convert.ToBase64String(pdf);
                return pdf;
            }
        }
    }
}