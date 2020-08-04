using System;
using System.Collections.Generic;

namespace ing_psd2.Models
{
    public class IngModel
    {
        public List<IngAccountModel> accounts { set; get; }
    }

    public class IngAccountModel
    {
        public string resourceId { get; set; }
        public string product { get; set; }
        public string iban { get; set; }
        public string name { get; set; }
        public string currency { get; set; }
        public AccountLink _links { set; get; }
    }

    public class AccountLink
    {
        public Link balances { set; get; }
        public Link transactions { set; get; }
    }

    public class Link
    {
        public string href { set; get; }
    }
}
