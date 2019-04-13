using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace webapi.Models
{
    [DataContract]
    public partial class MTemperature
    {
        [IgnoreDataMember]
        public int Id { get; set; }
        [DataMember(Name ="date")]
        public string date { get; set; }
        [DataMember(Name ="temperature")]
        public Nullable<int> temperature { get; set; }
        [IgnoreDataMember]
        public Nullable<int> City_id { get; set; }

    }
}