using System;
using System.Collections.Generic;
using Cwiczenie3.Models;

namespace Cwiczenie3.DAL
{
    public interface IDbService

    {
        public IEnumerable<Student> GetStudents();
        public String GetSemester(int id);
    }

}
