using System;
using System.Collections.Generic;

namespace campsApi
{
    public partial class Camps
    {
        public long Id { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Name { get; set; }
        public long? Events { get; set; }

        public virtual Events EventsNavigation { get; set; }
    }
}
