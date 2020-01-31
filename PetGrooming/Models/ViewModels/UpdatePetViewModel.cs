using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetGrooming.Models.ViewModels
{
    public class UpdatePetViewModel
    {
        // WHAT INFO DOES UPDATE PET NEED?
        // IT NEEDS LIST OF SPECIES AND INDIVIDUAL PET

        // Acts as a variable to contain the information that we need

        // IT NEEDS THE INFO OF A PET AND THE LIST OF SPECIES TO POPULATE THE DROP DOWN LIST:
        public Pet pet { get; set; }
        public List<Species> species { get; set; }
    }
}