using StudandoApi.Properties.Enuns;
using SudyApi.Models;
using System;
using System.Text.RegularExpressions;

namespace SudyApi.Utility
{
    public class DefaultValues
    {
        public static List<CourseModel> GenerateCourses()
        {
            List<CourseModel> courses = new List<CourseModel>();

            string fileNameBachelor = "CoursesBachelor.txt";
            string pathBachelor = Path.Combine(Environment.CurrentDirectory, @"Properties\Archives\", fileNameBachelor);

            int id = 1;

            using (StreamReader reader = File.OpenText(pathBachelor))
            {
                string contents = reader.ReadToEnd();

                var myList = new List<string>(contents.Split(','));

                foreach(string line in myList)
                {
                    courses.Add(new CourseModel(line, GraduationLevel.Bachelor, id));
                    id++;
                }
            }

            string fileNameTechnological = "CoursesTechnological.txt";
            string pathTechnological = Path.Combine(Environment.CurrentDirectory, @"Properties\Archives\", fileNameTechnological);

            using (StreamReader reader = File.OpenText(pathTechnological))
            {
                string contents = reader.ReadToEnd();

                var myList = new List<string>(contents.Split(','));

                foreach (string line in myList)
                {
                    courses.Add(new CourseModel(line, GraduationLevel.Technological, id));
                    id++;
                }
            }

            string fileNameGraduation = "CoursesTechnological.txt";
            string pathGraduation = Path.Combine(Environment.CurrentDirectory, @"Properties\Archives\", fileNameGraduation);

            using (StreamReader reader = File.OpenText(pathGraduation))
            {
                string contents = reader.ReadToEnd();

                var myList = new List<string>(contents.Split(','));

                foreach (string line in myList)
                {
                    courses.Add(new CourseModel(line, GraduationLevel.Graduation, id));
                    id++;
                }
            }

            return courses;
        }
    }
}
