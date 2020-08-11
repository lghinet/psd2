using System;
using System.Collections.Generic;

namespace psd2.web.Models
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
        public AccountLink()
        {
            balances = new Link();
            transactions = new Link();
        }

        public Link balances { set; get; }
        public Link transactions { set; get; }
    }

    public class Link
    {
        public string href { set; get; }
    }
}
