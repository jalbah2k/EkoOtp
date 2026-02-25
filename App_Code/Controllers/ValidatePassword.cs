using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PasswordValidatorApi
{
    public class ValidatePasswordController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public async Task<string> Post([FromBody] dynamic value)
        {
            string passw = value["Password"];
            string message = "";

            //result = EncDec.DESDecrypt(passw);

            //if (result.IndexOf("Error: ") >= 0)
            //    return result;

            Microsoft.AspNet.Identity.PasswordValidator pv = new Microsoft.AspNet.Identity.PasswordValidator();

            pv.RequiredLength = Rules.RequiredLength;
            pv.RequireNonLetterOrDigit = Rules.RequireNonLetterOrDigit;
            pv.RequireDigit = Rules.RequireDigit;
            pv.RequireLowercase = Rules.RequireLowercase;
            pv.RequireUppercase = Rules.RequireUppercase;

            IdentityResult x = await pv.ValidateAsync(passw);
            if (!x.Succeeded)
            {
                foreach (var e in x.Errors)
                {
                    message = e.ToString();
                }

                string[] errors = message.ToString().Split(new char[] { '.' });
                message = "<ul>";
                foreach (var s in errors)
                {
                    if (s != "")
                        message += "<li>" + s.Trim() + "</li>";
                }
                message += "</ul>";

                //message += "id: " + id.ToString() + "<br />";
                //message += "passw: " + passw + "<br />";

                //message += Request.RequestUri;
            }


            return message;

        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }

    public class Rules
    {
        public Rules()
        {

        }

        public static bool RequireDigit = true;
        public static int RequiredLength = 8;
        public static bool RequireLowercase = true;
        public static bool RequireNonLetterOrDigit = true;
        public static bool RequireUppercase = true;
    }

}