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
    
    public partial class Pool
    {
        public Pool()
        {
            this.Teams = new HashSet<Team>();
            this.FavoriteFields = new HashSet<Field>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsAuto { get; set; }
    
        public virtual ICollection<Team> Teams { get; set; }
        public virtual Division Division { get; set; }
        public virtual ICollection<Field> FavoriteFields { get; set; }
        public virtual TournamentStage TournamentStage { get; set; }
    }
}
