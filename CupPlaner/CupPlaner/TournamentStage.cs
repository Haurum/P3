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
    
    public partial class TournamentStage
    {
        public TournamentStage()
        {
            this.Matches = new HashSet<Match>();
        }
    
        public int Id { get; set; }
        public TournamentStructure TournamentStructure { get; set; }
        public bool IsScheduled { get; set; }
    
        public virtual ICollection<Match> Matches { get; set; }
        public virtual Pool Pool { get; set; }
        public virtual DivisionTournament DivisionTournament { get; set; }
    }
}
