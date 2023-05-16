using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIClientApp
{

    public class SingleOutcodeResponse
    {
        public int status { get; set; }
        public Result1 result { get; set; }
    }

    public class Result1
    {
        public string outcode { get; set; }
        public float longitude { get; set; }
        public float latitude { get; set; }
        public int northings { get; set; }
        public int eastings { get; set; }
        public string[] admin_district { get; set; }
        public string[] parish { get; set; }
        public object[] admin_county { get; set; }
        public string[] admin_ward { get; set; }
        public string[] country { get; set; }
        public string[] parliamentary_constituency { get; set; }
    }

    internal class OutcodeModel
    {
    }
}
