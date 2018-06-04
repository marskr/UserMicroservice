using System.Linq;
using System.Text.RegularExpressions;

namespace UsersMicroservice.Security
{
    public class XSS
    {
        public static string[] forbiddenList_s = { "*", "/", "#", "=", "+", "&", "|" };

        public static bool CheckIfContains(string receivedMsg_s, string[] forbiddenList_s)
        {
            return forbiddenList_s.Any(receivedMsg_s.Contains);  
        }

        public static bool CheckIfAlphaNum(string receivedMsg_s)
        {
            return Regex.IsMatch(receivedMsg_s, @"^[\p{L}]+$");
            // check if there are only letters in string
        }

        public static bool CheckIfTooLong(string receivedMsg_s, int lengthLimit_i)
        {
            return receivedMsg_s.Length > lengthLimit_i;
        }
    }
}
