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
            string path = Path.Combine(Environment.CurrentDirectory, @"Users\victo\source\repos\Study\SudyApi\Properties\Archives\", fileName);

            using (StreamReader reader = File.OpenText(path))
            {
                string contents = reader.ReadToEnd();
                MatchCollection matches = Regex.Matches(contents, "computer", RegexOptions.IgnoreCase);

                if (matches.Count > 0)
                    return true;
            }

            return false;
        }

    }
}
