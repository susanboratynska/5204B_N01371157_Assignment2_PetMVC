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
using PetGrooming.Models.ViewModels; // New Folder
using System.Diagnostics;

namespace PetGrooming.Controllers
{
    public class PetController : Controller
    {
        /*
        These reading resources will help you understand and navigate the MVC environment
 
        Q: What is an MVC controller?

        - https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions-1/controllers-and-routing/aspnet-mvc-controllers-overview-cs

        Q: What does it mean to "Pass Data" from the Controller to the View?

        - http://www.webdevelopmenthelp.net/2014/06/using-model-pass-data-asp-net-mvc.html

        Q: What is an SQL injection attack?

        - https://www.w3schools.com/sql/sql_injection.asp

        Q: How can we prevent SQL injection attacks?

        - https://www.completecsharptutorial.com/ado-net/insert-records-using-simple-and-parameterized-query-c-sql.php

        Q: How can I run an SQL query against a database inside a controller file?

        - https://www.entityframeworktutorial.net/EntityFramework4.3/raw-sql-query-in-entity-framework.aspx
 
         */
        private PetGroomingContext db = new PetGroomingContext();

        // GET: Pet
        public ActionResult List()
        {
            //How could we modify this to include a search bar?
            List<Pet> pets = db.Pets.SqlQuery("Select * from Pets").ToList();
            return View(pets); 
            // THE LIST METHOD IN THE CONTROLLER WILL NAVIGATE TO THE VIEWS FOLDER, WILL NAVIGATE TO TEH PET FOLDER AND ATTEMPT TO FIND A FILE CALLED LIST.CSHTML
        }

        // GET: Pet/Details/5
        public ActionResult Show(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Pet pet = db.Pets.Find(id); //EF 6 technique
            Pet pet = db.Pets.SqlQuery("select * from pets where petid=@PetID", new SqlParameter("@PetID",id)).FirstOrDefault();
            if (pet == null)
            {
                return HttpNotFound();
            }
            return View(pet);
        }

        //THE [HttpPost] Means that this method will only be activated on a POST form submit to the following URL
        //URL: /Pet/Add
        [HttpPost]
        public ActionResult Add(string PetName, Double PetWeight, String PetColor, int SpeciesID, string PetNotes)
        {
            //STEP 1: PULL DATA! The data is access as arguments to the method. Make sure the datatype is correct!
            //The variable name  MUST match the name attribute described in Views/Pet/Add.cshtml in the view

            //Tests are very useul to determining if you are pulling data correctly!
            //Debug.WriteLine("Want to create a pet with name " + PetName + " and weight " + PetWeight.ToString()) ;

            //STEP 2: FORMAT QUERY! the query will look something like "insert into () values ()"...
            string query = "insert into pets (PetName, Weight, color, SpeciesID, Notes) values (@PetName,@PetWeight,@PetColor,@SpeciesID,@PetNotes)"; //MUST MATCH DATABASE NAMES
            SqlParameter[] sqlparams = new SqlParameter[5]; //0,1,2,3,4 pieces of information to add // CREATE AN ARRAY, EACH OF THE ITEMS IS GOING TO CORRESPOND TO THE COLUMNS
            //each piece of information is a key and value pair
            sqlparams[0] = new SqlParameter("@PetName", PetName); //MATCHES LINES 78
            sqlparams[1] = new SqlParameter("@PetWeight", PetWeight);
            sqlparams[2] = new SqlParameter("@PetColor", PetColor);
            sqlparams[3] = new SqlParameter("@SpeciesID", SpeciesID);
            sqlparams[4] = new SqlParameter("@PetNotes", PetNotes);

            //db.Database.ExecuteSqlCommand will run insert, update, delete statements
            //db.Pets.SqlCommand will run a select statement, for example.
            db.Database.ExecuteSqlCommand(query, sqlparams); // TAKES IN 1 ARGRUMENTS, ONE OF THEM IS THE QUERY (LINE 81), THE SECOND IS THE SQL PARAMS (AN ARRAY OF SQL PARAMS)
            // db SEE LINE 43
            
            //run the list method to return to a list of pets so we can see our new one! REDIRECT 
            return RedirectToAction("List"); //GO TO THE LIST OF PETS SO I CAN SEE THE PET I JUST ADDED, IT WILL GO THE FUNCTION CALL LIST ON LINE 46
        }


        public ActionResult Add()
        {
            //STEP 1: PUSH DATA!
            //What data does the Add.cshtml page need to display the interface?
            //A list of species to choose for a pet

            //alternative way of writing SQL -- will learn more about this week 4
            //List<Species> Species = db.Species.ToList();

            List<Species> species = db.Species.SqlQuery("Select * from Species").ToList();

            return View(species);
        }

        // URL: /Pet/Update/2 -> update pet with ID 2
        public ActionResult Update (int id) // GET REQUEST, NOT A POST REQUEST
        {
            // HOW TO GET PET DATA?
            // RUN QUERY
            Pet selectedpet = db.Pets.SqlQuery("select * from pets where petid = @PetID", new SqlParameter("@PetID", id)).FirstOrDefault(); //MUST TELL IT THAT YOU WANT THE FIRST OR DEFAULT RESULT (ONLY 1)
            // NEED THE PET DATA
            //  NEED INFORMATION ABOUT ALL SPECIES

            string query = "select * from species";
            List<Species> selectedspecies = db.Species.SqlQuery(query).ToList();

            // CREATE AN INSTANCE OF ORU ViewModel:
            UpdatePetViewModel viewmodel = new UpdatePetViewModel();
            viewmodel.pet = selectedpet;
            viewmodel.species = selectedspecies;

            return View(viewmodel); // THE PET IS OUR FOOTBALL 
        }

        [HttpPost]
        public ActionResult Update( int id, string PetName, string PetColor, double PetWeight, string PetNotes)
        {
            // STEP 1: PULL DATA:
           // Debug.WriteLine("I'm pulling data of " + PetName + " and " + PetColor + " and " + PetWeight.ToString());

            string query = "Update pets set PetName=@PetName, Weight=@PetWeight, color=@PetColor, Notes=@PetNotes where PetID=@id";
            SqlParameter[] parameters = new SqlParameter[5]; // 5 PIECES OF INFO TO PASS THROUGH

            Debug.WriteLine(query);

            parameters[0] = new SqlParameter("@PetName", PetName); // SECOND VALUE GETS THE VALUE FROM LINE 126
            parameters[1] = new SqlParameter("@PetWeight", PetWeight);
            parameters[2] = new SqlParameter("@PetColor", PetColor);
            parameters[3] = new SqlParameter("@PetNotes", PetNotes);
            parameters[4] = new SqlParameter("@id", id);

            db.Database.ExecuteSqlCommand(query, parameters);

            return RedirectToAction("List");
        }

        //TODO:bad
        //Update
        //[HttpPost] Update
        //[HttpPost] Delete
        //(optional) Delete

        public ActionResult Delete(int id) 
        {
            db.Database.ExecuteSqlCommand("Delete from Pets where PetID = @PetID", new SqlParameter("@PetID", id));
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
