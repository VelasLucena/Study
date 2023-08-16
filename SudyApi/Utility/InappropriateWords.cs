using System.Text.RegularExpressions;

namespace SudyApi.Utility
{
    public class InappropriateWords
    {
        public static bool WordIsInappropriate(string word)
        {
            if (word == null)
                return false;

            string fileName = "InappropriateWords.txt";
            string path = Environment.CurrentDirectory + @"\Properties\Archives\" + fileName;

            using (StreamReader reader = File.OpenText(path))
            {
                string contents = reader.ReadToEnd();
                MatchCollection matches = Regex.Matches(contents, word.ToLower(), RegexOptions.IgnoreCase);

                if (matches.Count > 0)
                    return true;
            }

            return false;
        }

    }
}
