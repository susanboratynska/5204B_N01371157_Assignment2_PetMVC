using System;
using System.Collections.Generic;
using System.Data;
//required for SqlParameter class
using System.Data.SqlClient;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PetGrooming.Data;
using PetGrooming.Models;
using System.Diagnostics;

namespace PetGrooming.Controllers
{
    public class SpeciesController : Controller
    {
        private PetGroomingContext db = new PetGroomingContext();

        // GET: Species
        public ActionResult List()
        {
            List<Species> species = db.Species.SqlQuery("Select * from Species").ToList();
            return View(species);
        }

        //TODO: Each line should be a separate method in this class
        // List
        // Show
        // Add
        // [HttpPost] Add
        // Update
        // [HttpPost] Update
        // (optional) delete
        // [HttpPost] Delete

        // GET: Species Details
        public ActionResult Show(int id)
        {
            string query = "select * from species where SpeciesID = @id";
       
            SqlParameter param = new SqlParameter("@id", id);

            Species selectedspecies = db.Species.SqlQuery(query, param).FirstOrDefault();
            return View(selectedspecies);



            
           /* if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Species species = db.Species.SqlQuery("Select * from species where speciesid=@SpeciesID", new SqlParameter("@SpeciesID", id)).FirstOrDefault();
           
            if (species == null)
            {
                return HttpNotFound();
            }
            return View(species);*/
        }

        // URL: Species/Add

        [HttpPost] // Method will only be activated on a POST form submit to the URL
        public ActionResult Add(string SpeciesName)
        {
            string query = "Insert into species (Name) values (@SpeciesName)";

            // SqlParameter[] sqlparams = new SqlParameter[1]; // DON'T NEED TO CREATE AN ARRAY

            // sqlparams[0] = new SqlParameter("@SpeciesName", SpeciesName);

            SqlParameter param = new SqlParameter("@SpeciesName", SpeciesName);

            db.Database.ExecuteSqlCommand(query, param);
            return RedirectToAction("List");

        }

        public ActionResult Add()
        {
            List<Species> species = db.Species.SqlQuery("Select * from Species").ToList();
            return View(species); //Going to View, going to species, going to add
        }

        // URL: /Species/Update/2 ; Update Species with ID 2

        public ActionResult Update (int id)
        {
            // Get species name: my own code below
            //Species selectedspecies = db.Species.SqlQuery("Select * from Species where SpeciesID = @SpeciesID", new SqlParameter("@SpeciesID", SpeciesID)).FirstOrDefault();

            string query = "select * from species where SpeciesID = @id";
            SqlParameter param = new SqlParameter("@id", id);

            Species selectedspecies = db.Species.SqlQuery(query, param).FirstOrDefault();
            return View(selectedspecies);
        }

        [HttpPost]
        public ActionResult Update (int id, string SpeciesName)
        {
            string query = "Update species set Name=@SpeciesName where SpeciesID=@id";
            SqlParameter[] sqlparams = new SqlParameter[2];
            sqlparams[0] = new SqlParameter("@SpeciesName", SpeciesName);
            sqlparams[1] = new SqlParameter("@id", id);

            db.Database.ExecuteSqlCommand(query, sqlparams);

            Debug.WriteLine("I'm pulling data of " + SpeciesName);
            return RedirectToAction("List");
        }

        public ActionResult Delete (int id)
        {
            db.Database.ExecuteSqlCommand("Delete from Species where SpeciesID = @SpeciesID", new SqlParameter("@SpeciesID", id));
            return RedirectToAction("List");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


    }
}