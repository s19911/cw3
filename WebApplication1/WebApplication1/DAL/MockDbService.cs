using System;
using System.Collections.Generic;
using Cwiczenie3.Models;

namespace Cwiczenie3.DAL
{
    public class MockDbService : IDbService
    {
        private static IEnumerable<Student> _students;


        public MockDbService() {

            _students=new List<Student>
            {
            new Student { IdStudent = 1, FirstName = "Jan", LastName = "Kowalski", IndexNumber = "s1234" },
            new Student { IdStudent = 2, FirstName = "Anna", LastName = "Malewski", IndexNumber = "s2342" },
            new Student { IdStudent = 3, FirstName = "Krzysztof", LastName = "Andrzejewski", IndexNumber = "s5432" },
            };
        }

        public string GetSemester(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Student> GetStudents()
        {
            return _students;
        }



    }
}
