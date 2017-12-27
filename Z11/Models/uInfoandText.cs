using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Z11.Models
{
    public class uInfoandText
    {
        public  UserInfo userinfo { get; set; }
        public  IQueryable<Texts> texts { get; set; }
        public int mycount { get; set; }
        public int allcount { get; set; }
    }
}