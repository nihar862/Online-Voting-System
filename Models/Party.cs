//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OVS.Models
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.ComponentModel.DataAnnotations;

    public partial class Party
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Party()
        {
            this.Candidates = new HashSet<Candidate>();
        }
        
        //[Required(ErrorMessage ="Enter PartyID")]
        //[RegularExpression("",ErrorMessage ="Enter Valid PartyID")]
        public string Party_ID { get; set; }

        //[Required(ErrorMessage ="Enter Password")]
        public string Password { get; set; }

        //[Required(ErrorMessage ="Enter Your Party Name")]
        public string Motto { get; set; }

        public string Logo { get; set; }

        public HttpPostedFileBase ImageFile3 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Candidate> Candidates { get; set; }
    }
}
