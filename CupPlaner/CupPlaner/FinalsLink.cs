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
    
    public partial class FinalsLink
    {
        public int Id { get; set; }
        public int Finalstage { get; set; }
        public int PoolPlacement { get; set; }
    
        public virtual Division Division { get; set; }
    }
}
