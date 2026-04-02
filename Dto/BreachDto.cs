using Core.Entities;

namespace LBProject.Dto
{
    public class BreachDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Domain { get; set; }
        public DateTime BreachDate { get; set; }
        public long PwnCount { get; set; }
        public string Description { get; set; }
        public List<string> DataClasses { get; set; }

        public string LogoPath { get; set; }
        public string Attribution { get; set; }
        public string DisclosureUrl { get; set; }

        public static BreachDto FromEntity(Breach breach)
        {
            BreachDto dto = new BreachDto();
            dto.Id = breach.Id; 
            dto.Title = breach.Title;
            dto.Domain = breach.Domain;
            dto.BreachDate = breach.BreachDate;
            dto.PwnCount = breach.PwnCount;
            dto.Description = breach.Description;

            dto.DataClasses = new List<string>();
            if (breach.DataClasses != null)
            {
                foreach (string s in breach.DataClasses)
                {
                    dto.DataClasses.Add(s);
                }
            }

            dto.LogoPath = breach.LogoPath;
            dto.Attribution = breach.Attribution;
            dto.DisclosureUrl = breach.DisclosureUrl;

            return dto;
        }

        // Map list of Breach to list of BreachDto
        public static List<BreachDto> FromEntityList(IEnumerable<Breach> breaches)
        {
            List<BreachDto> dtoList = new List<BreachDto>();
            foreach (Breach b in breaches)
            {
                dtoList.Add(FromEntity(b));
            }
            return dtoList;
        }
    }
}
