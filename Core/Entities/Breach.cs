using Core.Models;
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

        public static Breach FromPwnBreach(PwnBreach pwn)
        {
            if (pwn == null) return null;

            return new Breach
            {
                Id = pwn.Name, // Crucial: API 'Name' is our unique 'Id'
                Title = pwn.Title,
                Domain = pwn.Domain,
                BreachDate = pwn.BreachDate,
                AddedDate = pwn.AddedDate,
                ModifiedDate = pwn.ModifiedDate,
                PwnCount = pwn.PwnCount,
                Description = pwn.Description,
                LogoPath = pwn.LogoPath,
                Attribution = pwn.Attribution,
                DisclosureUrl = pwn.DisclosureUrl,
                // Ensure we create a new list instance to avoid reference issues
                DataClasses = pwn.DataClasses != null ? new List<string>(pwn.DataClasses) : new List<string>(),
                IsVerified = pwn.IsVerified,
                IsFabricated = pwn.IsFabricated,
                IsSensitive = pwn.IsSensitive,
                IsRetired = pwn.IsRetired,
                IsSpamList = pwn.IsSpamList,
                IsMalware = pwn.IsMalware,
                IsSubscriptionFree = pwn.IsSubscriptionFree,
                IsStealerLog = pwn.IsStealerLog
            };
        }

       
    }


}
