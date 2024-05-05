using System.ComponentModel.DataAnnotations;

namespace Demo_Areas.Areas.LOC_Country.Models
{
    public class LOC_CountryModel
    {
        public int? CountryID { get; set; }
        [Required(ErrorMessage ="Required")]
        public string CountryName { get; set; }
        [Required(ErrorMessage = "Required")]
        public string CountryCode { get; set; }
    }

    public class LOC_CountryDropDownModel
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; }
    }
}
