//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Data.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class History
    {
        public int Id { get; set; }
        public string Domain { get; set; }
        public string Url { get; set; }
        public System.DateTime LastVisited { get; set; }
        public int VisitCount { get; set; }
        public int AccountId { get; set; }
    
        public virtual Account Account { get; set; }
    }
}
