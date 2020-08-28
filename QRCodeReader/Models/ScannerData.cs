using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QRCodeReader.Models
{
    public class ScannerData
    {    
        public string type { get; set; }
        public Symbol[] symbol { get; set; }
    }
}