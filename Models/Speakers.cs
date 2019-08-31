using System;
using System.Collections.Generic;

namespace campsApi
{
    public partial class Speakers
    {
        public Speakers()
        {
            Events = new HashSet<Events>();
        }

        public long Id { get; set; }
        public string Fullname { get; set; }

        public virtual ICollection<Events> Events { get; set; }
    }
}
