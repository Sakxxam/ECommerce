using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShoppingProject.Model
{
    public class Company
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string StreetAddress  { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Display(Name ="Postal Code")]
        [Required]
        public string PostalCode { get; set; }
        [Display(Name="Phone Number")]
        [Required]
        public string PhoneNumber { get; set; }
        [Display(Name ="Is Authorized Company")]
        public bool IsAuthorizedCompany { get; set; }
        [Display(Name = "Image URL")]
        public string ImageURL { get; set; }
    }
}
