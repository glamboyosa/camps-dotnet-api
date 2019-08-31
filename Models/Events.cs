using System;
using System.Collections.Generic;

namespace campsApi
{
    public partial class Events
    {
        public Events()
        {
            Camps = new HashSet<Camps>();
        }

        public long Id { get; set; }
        public string NameOfEvent { get; set; }
        public string Venue { get; set; }
        public long? Speaker { get; set; }

        public virtual Speakers SpeakerNavigation { get; set; }
        public virtual ICollection<Camps> Camps { get; set; }
    }
}
