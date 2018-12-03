using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TTG.Web.Models
{
    public class Prompt
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public List<string> Buttons { get; set; }

    }
}