﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CupDBContainer : DbContext
    {
        public CupDBContainer()
            : base("name=CupDBContainer")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Team> TeamSet { get; set; }
        public virtual DbSet<Pool> PoolSet { get; set; }
        public virtual DbSet<Division> DivisionSet { get; set; }
        public virtual DbSet<DivisionTournament> DivisionTournamentSet { get; set; }
        public virtual DbSet<Field> FieldSet { get; set; }
        public virtual DbSet<TimeInterval> TimeIntervalSet { get; set; }
        public virtual DbSet<Tournament> TournamentSet { get; set; }
        public virtual DbSet<Match> MatchSet { get; set; }
        public virtual DbSet<TournamentStage> TournamentStageSet { get; set; }
    }
}