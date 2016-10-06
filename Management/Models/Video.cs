/*!
* DisplayMonkey source file
* http://displaymonkey.org
*
* Copyright (c) 2015 Fuel9 LLC and contributors
*
* Released under the MIT license:
* http://opensource.org/licenses/MIT
*/

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
    
    public partial class Video
    {
        public Video()
        {
            this.Contents = new HashSet<Content>();
        }
    
        public int FrameId { get; set; }
        public bool PlayMuted { get; set; }
        public bool AutoLoop { get; set; }
    
        public virtual Frame Frame { get; set; }
        public virtual ICollection<Content> Contents { get; set; }
    }
}
