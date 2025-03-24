using DRF.SoilHealthCard.Report.Service;
using Microsoft.Reporting.WebForms;
using System.Collections;
using System.Configuration;
using System.IO;
using System.Web.Http;
using System.Web;
using DRF.SoilHealthCard.Report.ViewModels;


namespace DRF.SoilHealthCard.Report.Controllers
{
    [RoutePrefix("api/SoilHealthReport")]
    public class SoilHealthReportController : ApiController
    {
        private readonly IReportService _reportService;
        public SoilHealthReportController()
        {
            _reportService = new ReportService();
        }

        [Route("Generate")]
        [HttpPost]
        public IHttpActionResult Generate()
        {
            string FileName = string.Format("JSON.txt");
            string TempFilePath = HttpContext.Current.Server.MapPath(@System.Configuration.ConfigurationManager.AppSettings["TempFilePath"]) + "/" + FileName;
            //test
            //var output = JsonConvert.SerializeObject(model);
            //using (StreamWriter bw = new StreamWriter(File.Create(TempFilePath)))
            //{
            //    bw.Write(output);
            //    bw.Close();
            //}

            string myTempFile = Path.Combine(TempFilePath, FileName);
            if (File.Exists(myTempFile))
            {
                File.Delete(myTempFile);
            }
            //File.Create(TempFilePath);
            byte[] rdeder = new byte[] { 23, 34 };


            string contentType = string.Empty;
            Hashtable parms = new Hashtable();

            var reportViewer = new ReportViewer();
            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.LocalReport.ReportPath = HttpContext.Current.Server.MapPath("~" + ConfigurationManager.AppSettings["RootFilePath"] + "/SoilHealthReport_v1.rdlc");

           

            reportViewer.LocalReport.Refresh();
            reportViewer.LocalReport.EnableExternalImages = true;
            reportViewer.LocalReport.EnableHyperlinks = true;

            string deviceInfo = "<DeviceInfo>" +
                "  <OutputFormat>PDF</OutputFormat>" +
                "</DeviceInfo>";
            Warning[] warnings;
            string[] streams;
            string mimeType;
            byte[] renderedBytes;
            string encoding;
            string fileNameExtension;
            //Render the report           
            renderedBytes = reportViewer.LocalReport.Render("PDF", deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

            //if (File.Exists(myTempFile))
            //{
            //    File.Delete(myTempFile);
            //}
            //using (FileStream fs = new FileStream(myTempFile, FileMode.Create))
            //{
            //    fs.Write(renderedBytes, 0, renderedBytes.Length);
            //}
            string PDFFileName = string.Format("Sample.pdf");
            string PDFTempFilePath = HttpContext.Current.Server.MapPath(@System.Configuration.ConfigurationManager.AppSettings["TempFilePath"]);
            //test
            string PDFmyTempFile = Path.Combine(PDFTempFilePath, PDFFileName);
            if (File.Exists(PDFmyTempFile))
            {
                File.Delete(PDFmyTempFile);
            }
            using (FileStream fs = new FileStream(PDFmyTempFile, FileMode.Create))
            {
                fs.Write(renderedBytes, 0, renderedBytes.Length);
            }

            //return PDFmyTempFile;
            var response = new ReturnResponse() { Response = renderedBytes };
            return Json(response);
        }


        }
}
