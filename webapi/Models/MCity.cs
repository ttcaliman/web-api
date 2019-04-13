using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace webapi.Models
{
    [DataContract]
    public partial class MCity
    {
       [IgnoreDataMember]
        public int Id { get; set; }
        [DataMember(Name ="city")]
        public string city { get; set; }
        [DataMember(Name ="temperatures")]
        public virtual List<MTemperature> temperatures { get; set; }
    }
}