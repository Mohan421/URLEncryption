using SecureQuerystring.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SecureQuerystring.Controllers
{
    public class DemoController : Controller
    {
        //
        // GET: /EncDec/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Demo(int id)
        {
            string str = Request.QueryString["Jack"];
            int res = CustomURLHelper.EnsureURLNotTampered(string.Format("id={0}", id.ToString()), str);
            if (res == 1 || string.IsNullOrEmpty(str))
            {
                return View("Error");
            }
            return View();
        }
    }
}
