using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities
{
    public class Result
    {
        public string codeResult {get;set;}
        public string messageResult { get; set; }
        public string detailResult {get;set;}

        public Result()
        {}

        public Result(bool value)
        {
            this.codeResult = value ? "Success" : "Error";
        }

        public Result(Exception e)
        {
            this.codeResult = "Error";
            this.messageResult = "Error";
            this.detailResult = e.Message;
        }

        public static implicit operator bool(Result r)
        {
            return r.codeResult=="Success";
        }

    }
}
