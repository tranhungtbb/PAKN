//using Dapper;
//using DevExpress.XtraReports.UI;
//using PAKNAPI.ExportGrid;
//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;
//using System.IO;
//using System.Linq;
//using System.Reflection;
//using System.ServiceModel;
//using System.Web;
//using Newtonsoft.Json;
//using PAKNAPI.Common;
//using PAKNAPI.Models;

//namespace WebApi
//{
//    public class ReportStorageWebExtension1 : DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension
//    {
//        //private readonly IMapper _mapper;
//        public class ReportDetails
//        {
//            public byte[] Layout { get; set; }
//            public string DisplayName { get; set; }
//        }
//        public EmbeddedResourceReportStorageService EmbeddedResourceReportStorage { get; private set; }
//        public ConcurrentDictionary<string, ReportDetails> Reports { get; private set; }
//        IEnumerable<string> reportUrlsFromAssemblies;
//        IEnumerable<string> ReportsFromResources
//        {
//            get
//            {
//                if (reportUrlsFromAssemblies == null)
//                {
//                    reportUrlsFromAssemblies = EmbeddedResourceReportStorage.Assemblies.SelectMany(x => x.GetManifestResourceNames());
//                }
//                return reportUrlsFromAssemblies;
//            }
//        }

//        public ReportStorageWebExtension1()
//        {

//            Reports = new ConcurrentDictionary<string, ReportDetails>();
//            EmbeddedResourceReportStorage = new EmbeddedResourceReportStorageService { Assemblies = new Assembly[] { Assembly.GetExecutingAssembly() } };

//        }

//        public override bool CanSetData(string url)
//        {
//            // Determines whether or not it is possible to store a report by a given URL. 
//            // For instance, make the CanSetData method return false for reports that should be read-only in your storage. 
//            // This method is called only for valid URLs (i.e., if the IsValidUrl method returned true) before the SetData method is called.

//            return true;
//        }

//        public override bool IsValidUrl(string url)
//        {
//            // Determines whether or not the URL passed to the current Report Storage is valid. 
//            // For instance, implement your own logic to prohibit URLs that contain white spaces or some other special characters. 
//            // This method is called before the CanSetData and GetData methods.

//            return true;
//        }

//        public override byte[] GetData(string url)
//        {
//            // Returns report layout data stored in a Report Storage using the specified URL. 
//            // This method is called only for valid URLs after the IsValidUrl method is called.
//            string newUrl = "http://www.example.com/" + url;
//            Uri myUri = new Uri(newUrl);
//            var jss = new JsonSerializerSettings
//            {
//                DateFormatHandling = DateFormatHandling.IsoDateFormat,
//                DateTimeZoneHandling = DateTimeZoneHandling.Local,
//                DateParseHandling = DateParseHandling.DateTimeOffset
//            };

//            string reportname = myUri.AbsolutePath.ToString().Replace('/', ' ');
//            var assembly = typeof(WebApi.ReportStorageWebExtension1).Assembly;
//            string UserId = HttpUtility.ParseQueryString(myUri.Query).Get("UserId");
//            string objectReport = "";
//            Stream resource = null;
//            XtraReport result = null;
//            MemoryStream ms = new MemoryStream();
//            string Dates = "Ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year;
//            DynamicParameters parameters = new DynamicParameters();
//            switch (reportname.Trim())
//            {
//                case "Recommendation_ListGeneral":
//                    var paramExportNhatKyThanhTra = JsonConvert.DeserializeObject<ExportRecommendation>(objectReport, jss);



//                    resource = assembly.GetManifestResourceStream("ApiCentralProcessingSystem.ExportGrid.Recommendation_ListGeneral.repx");
//                    result = XtraReport.FromStream(resource);
//                    result.Parameters["Code"].Value = paramExportNhatKyThanhTra.Code;
//                    result.Parameters["SendName"].Value = paramExportNhatKyThanhTra.SendName;
//                    result.Parameters["Content"].Value = paramExportNhatKyThanhTra.Content;
//                    result.Parameters["UnitId"].Value = paramExportNhatKyThanhTra.UnitId;
//                    result.Parameters["Field"].Value = paramExportNhatKyThanhTra.Field;
//                    result.Parameters["Status"].Value = paramExportNhatKyThanhTra.Status;
//                    result.SaveLayoutToXml(ms);
//                    if (ms != null)
//                    {
//                        return ms.ToArray();
//                    }
//                    break;
//            }
//            ReportDetails details = null;
//            if (Reports.TryGetValue(url, out details))
//            {
//                return details.Layout;
//            }

//            throw new FaultException(new FaultReason(string.Format("Could not find report '{0}'.", url)), new FaultCode("Server"), "GetData");
//        }

//        public override Dictionary<string, string> GetUrls()
//        {
//            // Returns a dictionary of the existing report URLs and display names. 
//            // This method is called when running the Report Designer, 
//            // before the Open Report and Save Report dialogs are shown and after a new report is saved to a storage.

//            var dictionary = Reports.ToDictionary(x => x.Key, y => y.Value.DisplayName);
//            foreach (var resourceName in ReportsFromResources)
//            {
//                if (!dictionary.ContainsKey(resourceName))
//                {
//                    dictionary.Add(resourceName, resourceName);
//                }
//            }

//            return dictionary;
//        }

//        public override void SetData(XtraReport report, string url)
//        {
//            // Stores the specified report to a Report Storage using the specified URL. 
//            // This method is called only after the IsValidUrl and CanSetData methods are called.

//            SetDataInternal(report, url, false);
//        }

//        public override string SetNewData(XtraReport report, string defaultUrl)
//        {
//            // Stores the specified report using a new URL. 
//            // The IsValidUrl and CanSetData methods are never called before this method. 
//            // You can validate and correct the specified URL directly in the SetNewData method implementation 
//            // and return the resulting URL used to save a report in your storage.

//            while (Reports.ContainsKey(defaultUrl))
//            {
//                defaultUrl += "_1";
//            }
//            SetDataInternal(report, defaultUrl, true);
//            return defaultUrl;
//        }

//        void SetDataInternal(XtraReport report, string url, bool isNewOne)
//        {
//            if (!isNewOne && !Reports.ContainsKey(url))
//            {
//                throw new FaultException(new FaultReason(string.Format("Could not find report '{0}'.", url)), new FaultCode("Server"), "SetData");
//            }
//            using (var stream = new MemoryStream())
//            {
//                report.SaveLayoutToXml(stream);
//                var reportLayout = stream.ToArray();
//                var newReportDetails = new ReportDetails { DisplayName = url, Layout = reportLayout };
//                Reports.AddOrUpdate(url, newReportDetails, (currentUrl, existingReport) => { existingReport.Layout = reportLayout; return existingReport; });
//            }
//        }
//    }
//}
