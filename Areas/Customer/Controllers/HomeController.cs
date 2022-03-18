using AspNetCore.Reporting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OnlineStore.Areas.Customer.Models;
using OnlineStore.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Controllers
{
    //[AllowAnonymous]
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        

        public HomeController(ILogger<HomeController> logger,IWebHostEnvironment webHostEnvironment,IConfiguration configuration)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public JsonResult GetChartInfo([FromBody]dat viewmodel) {
            PdfReports pdfReports = new PdfReports(_configuration);

            var dt = new DataTable();
            dt = pdfReports.GetSummaryReports("spStockSummary");

            List<Report> ReportList = new List<Report>();
            ReportList = (from DataRow dr in dt.Rows
                          select new Report() { 
                          Code=dr["Code"].ToString(),
                          Quantity=Convert.ToInt32( dr["Quantity"])
                          
                          }
                          ).ToList();
           
            return Json (new { success = false, responseText =ReportList });
        }

        private JsonResult Json(string v, object allowGet)
        {
            throw new NotImplementedException();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult GetBillInfo(string Date, string Bill) {

            if (string.IsNullOrEmpty(Date) || string.IsNullOrEmpty(Bill)) {

                ModelState.AddModelError("Date", "Date IS MUSt Required");
            }
            PdfReports pdfReports = new PdfReports(_configuration);

            var dt = new DataTable();
            dt = pdfReports.GetBillReports(Date,Bill);

            string mimeType = "";
            int extension = 1;
            var path = $"{ _webHostEnvironment.WebRootPath}\\Reports\\rptBillReport.rdlc";

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("prm1", "Bill # "+Bill+" Items Quantity , Bill Date "+Date+"");
            parameters.Add("prm2", DateTime.Now.ToString());
            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("DataSet1", dt);
            var res = localReport.Execute(RenderType.Pdf, extension, parameters, mimeType);
            return File(res.MainStream, "application/pdf");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult StockInfo() {
            DateTime mydate = new DateTime(2022, 6, 21);
            var dt = new DataTable();
            PdfReports pdfReports = new PdfReports(_configuration);
            dt = pdfReports.GetSummaryReports("spStockSummary");

            if (mydate > DateTime.Today)
            {
                string mimeType = "";
                int extension = 1;
                var path = $"{ _webHostEnvironment.WebRootPath}\\Reports\\rptStockInfo.rdlc";

                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("prm1", "Stocked Items Balance Quantity");
                parameters.Add("prm2", DateTime.Now.ToString());
                LocalReport localReport = new LocalReport(path);
                localReport.AddDataSource("dsStock", dt);
                var res = localReport.Execute(RenderType.Pdf, extension, parameters, mimeType);
                return File(res.MainStream, "application/pdf");
            }

            return null;
            //string mimeType;
            //string encoding;
            //string filenameExtension;
            //string[] streams;
            //Warning[] warnings;
            //var rv = new ReportViewer();
            //rv.ProcessingMode = ProcessingMode.Local;
            //rv.LocalReport.ReportPath = Path.Combine(environment.ContentRootPath, "Reports", "Report1.rdlc");
            //rv.LocalReport.Refresh();
            //var bytes = rv.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streams, out warnings);
            //return File(bytes, mimeType);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ColorCodeInfo()
        {
            var dt = new DataTable();
            PdfReports pdfReports = new PdfReports(_configuration);
            dt = pdfReports.GetSummaryReports("spColorCodeReport");

            string mimeType = "";
            int extension = 1;
            var path = $"{ _webHostEnvironment.WebRootPath}\\Reports\\rptCodeColorReport.rdlc";

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("prm1", "Items Color & Code");
            parameters.Add("prm2", DateTime.Now.ToString());
            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("DataSet1", dt);
            var res = localReport.Execute(RenderType.Pdf, extension, parameters, mimeType);
            return File(res.MainStream, "application/pdf");


            //string mimeType;
            //string encoding;
            //string filenameExtension;
            //string[] streams;
            //Warning[] warnings;
            //var rv = new ReportViewer();
            //rv.ProcessingMode = ProcessingMode.Local;
            //rv.LocalReport.ReportPath = Path.Combine(environment.ContentRootPath, "Reports", "Report1.rdlc");
            //rv.LocalReport.Refresh();
            //var bytes = rv.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streams, out warnings);
            //return File(bytes, mimeType);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }

    public class dat {
        public string name { get; set; }
    }
}
