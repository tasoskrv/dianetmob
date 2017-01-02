using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DianetMob.Model
{
    [DataContract]
    public class InAppPurchaseList
    {
        [DataMember]
        public List<InAppPurchase> Purchases { get; set; }
    }
}
