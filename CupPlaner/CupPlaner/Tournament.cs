//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CupPlaner
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tournament
    {
        public Tournament()
        {
            this.Divisions = new HashSet<Division>();
            this.TimeIntervals = new HashSet<TimeInterval>();
            this.Field = new HashSet<Field>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    
        public virtual ICollection<Division> Divisions { get; set; }
        public virtual ICollection<TimeInterval> TimeIntervals { get; set; }
        public virtual ICollection<Field> Field { get; set; }
    }
}
