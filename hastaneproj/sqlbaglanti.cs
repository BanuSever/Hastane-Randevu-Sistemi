using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace hastaneproj
{
    internal class SqlBaglanti
    {
        private NpgsqlConnection bag;

        public SqlBaglanti()
        {
            bag = new NpgsqlConnection("Server=localhost; Port=5432; Database=HastaneProje; User Id=postgres; Password=123;");
            bag.Open();
        }

        public NpgsqlConnection GetBaglanti()
        {
            return bag;
        }
    }
}
