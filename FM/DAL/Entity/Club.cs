using System;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM.DAL.Entity
{
    class Club
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int League { get; set; }
        public int? Overall { get; set; }
        public double Budget { get; set; }
        public double? SalaryBudget { get; set; }
        public string Coach { get; set; }

        public Club(SQLiteDataReader reader)
        {
            Id = Convert.ToInt32(reader["id"].ToString());
            Name = reader["name"].ToString();
            League = Convert.ToInt32(reader["league"].ToString());
            Overall = Convert.ToInt32(reader["overall"] != DBNull.Value ? reader["overall"].ToString() : null);
            Budget = Convert.ToDouble(reader["budget"].ToString());
            SalaryBudget = Convert.ToDouble(reader["salarybudget"] != DBNull.Value ? reader["salarybudget"].ToString() : null);
            Coach = reader["coach"].ToString();
            
        }

        public Club(int id, string name, int league, int? overall, double budget, double salaryBudget, string coach)
        {
            Id = id;
            Name = name;
            League = league;
            Overall = overall;
            Budget = budget;
            SalaryBudget = salaryBudget;
            Coach = coach;
            
        }

    }
}
