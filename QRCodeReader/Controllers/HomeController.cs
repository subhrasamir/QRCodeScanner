using Newtonsoft.Json;
using QRCodeReader.Models;
using RestSharp;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace QRCodeReader.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadQRCode()
        {
            var strScannedData = string.Empty;
            if (Request.Files.Count > 0)
            {
                try
                {
                    string fileName = string.Empty;
                    HttpFileCollectionBase uploadedFile = Request.Files;
                    string[] strFileTypes = System.Configuration.ConfigurationManager.AppSettings["FileType"].ToString().Split(',');
                    string extension = System.IO.Path.GetExtension(Request.Files[0].FileName);
                    int isValidFile = Array.IndexOf(strFileTypes, extension.Replace(".", "").ToString().ToLower());
                    if (isValidFile > -1)
                    {
                        HttpPostedFileBase file = uploadedFile[0];

                        fileName = Path.Combine(Server.MapPath(Resource.UploadFilePath), Request.Files[0].FileName);
                        if (System.IO.File.Exists(fileName))
                            System.IO.File.Delete(fileName);

                        file.SaveAs(fileName);
                        var client = new RestClient(System.Configuration.ConfigurationManager.AppSettings["ScannerApi"].ToString());
                        client.Timeout = -1;
                        var request = new RestRequest(Method.POST);
                        request.AddFile("file", fileName);
                        IRestResponse response = client.Execute(request);

                        JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
                        ScannerData[] objScannerData = jsSerializer.Deserialize<ScannerData[]>(response.Content);
                        strScannedData = new JavaScriptSerializer().Serialize(objScannerData);                       
                    }
                    else
                    {
                        strScannedData = Resource.InvalidFile;
                    }
                }
                catch (Exception)
                {
                    strScannedData = Resource.Error;
                }
            }
            else
            {
                strScannedData = Resource.FileNotUploaded;
            }
            return Json(JsonConvert.SerializeObject(strScannedData));
        }
    }
}