//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DisplayMonkey.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Location
    {
        public Location()
        {
            this.Displays = new HashSet<Display>();
            this.Frames = new HashSet<Frame>();
            this.Sublocations = new HashSet<Location>();
        }
    
        public int LocationId { get; set; }
        public int LevelId { get; set; }
        public string Name { get; set; }
        public Nullable<int> AreaId { get; set; }
        public string TemperatureUnit { get; set; }
        public Nullable<double> Latitude { get; set; }
        public Nullable<double> Longitude { get; set; }
        public string DateFormat { get; set; }
        public string TimeFormat { get; set; }
        public Nullable<int> OffsetGMT { get; set; }
        public Nullable<int> Woeid { get; set; }
        public byte[] Version { get; set; }
        public string Culture { get; set; }
    
        public virtual ICollection<Display> Displays { get; set; }
        public virtual Level Level { get; set; }
        public virtual ICollection<Frame> Frames { get; set; }
        public virtual ICollection<Location> Sublocations { get; set; }
        public virtual Location Area { get; set; }
    }
}
