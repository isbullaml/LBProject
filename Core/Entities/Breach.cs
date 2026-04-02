using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class Breach
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Domain { get; set; }
        public DateTime BreachDate { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public long PwnCount { get; set; }
        public string Description { get; set; }
        public string LogoPath { get; set; }
        public string Attribution { get; set; }
        public string DisclosureUrl { get; set; }
        public List<string> DataClasses { get; set; }
        public bool IsVerified { get; set; }
        public bool IsFabricated { get; set; }
        public bool IsSensitive { get; set; }
        public bool IsRetired { get; set; }
        public bool IsSpamList { get; set; }
        public bool IsMalware { get; set; }
        public bool IsSubscriptionFree { get; set; }
        public bool IsStealerLog { get; set; }
    }
}
