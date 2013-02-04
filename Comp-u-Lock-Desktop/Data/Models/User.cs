using Newtonsoft.Json;

namespace Data.Models
{
    public class User : IParser
    {
        private int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
