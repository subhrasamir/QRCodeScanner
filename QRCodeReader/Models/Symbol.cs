using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QRCodeReader.Models
{
    public class Symbol
    {     
        public int seq { get; set; }      
        public string data { get; set; }       
        public string error { get; set; }
    }
}