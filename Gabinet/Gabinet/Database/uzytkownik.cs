//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Gabinet.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class uzytkownik
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public uzytkownik()
        {
            this.godziny = new HashSet<godziny>();
            this.uzytkownik_usluga = new HashSet<uzytkownik_usluga>();
            this.wizyty = new HashSet<wizyty>();
        }
    
        public int uzytkownik_id { get; set; }
        public string login { get; set; }
        public string imie { get; set; }
        public string nazwisko { get; set; }
        public string haslo { get; set; }
        public Nullable<bool> pracownik { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<godziny> godziny { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<uzytkownik_usluga> uzytkownik_usluga { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<wizyty> wizyty { get; set; }
    }
}
