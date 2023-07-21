using Newtonsoft.Json;
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

            string fileName = "Courses.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"Properties\Archives\", fileName);

            using (StreamReader reader = File.OpenText(path))
            {
                string contents = reader.ReadToEnd();

                courses = JsonConvert.DeserializeObject<List<CourseModel>>(contents);
            }

            int id = 1;

            foreach (CourseModel item in courses)
            {
                item.CourseId = id++;
            }

            return courses;
        }

        public static List<InstitutionModel> GenerateInstitutions()
        {
            List<InstitutionModel> institutions = new List<InstitutionModel>();

            string fileName = "InstitutionsBrasil.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"Properties\Archives\", fileName);

            using (StreamReader reader = File.OpenText(path))
            {
                string contents = reader.ReadToEnd();

                institutions = JsonConvert.DeserializeObject<List<InstitutionModel>>(contents);
            }

            int id = 1;

            foreach(InstitutionModel item in institutions)
            {
                item.institutionId = id++;
            }

            return institutions;
        }

        public static List<DisciplineNameModel> GenerateDisciplineNames()
        {
            List<DisciplineNameModel> names = new List<DisciplineNameModel>();

            string fileName = "DisciplinesName.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"Properties\Archives\", fileName);

            using (StreamReader reader = File.OpenText(path))
            {
                string contents = reader.ReadToEnd();

                names = JsonConvert.DeserializeObject<List<DisciplineNameModel>>(contents);
            }

            int id = 1;

            foreach (DisciplineNameModel item in names)
            {
                item.DisciplineNameId = id++;
            }

            return names;
        }
    }
}
