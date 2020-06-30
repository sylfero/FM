﻿using System;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM.DAL.Entity
{
    class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Nationality { get; set; }
        public string Position { get; set; }
        public string Club { get; set; }
        public DateTime ContractTerminates { get; set; }
        public int Offense { get; set; }
        public int Defence { get; set; }
        public int Overall { get; set; }
        public int Potential { get; set; }
        public int Value { get; set; }
        public int Salary { get; set; }

        public Player(SQLiteDataReader reader)
        {
            Id = Convert.ToInt32(reader["id"].ToString());
            Name = reader["name"].ToString();
            Surname = reader["surname"].ToString();
            DateOfBirth = Convert.ToDateTime(reader["dateofbirth"].ToString());
            Nationality = reader["nationality"].ToString();
            Club = reader["club"].ToString();
            ContractTerminates = Convert.ToDateTime(reader["contract_terminates"].ToString());
            Offense = Convert.ToInt32(reader["offense"].ToString());
            Defence = Convert.ToInt32(reader["defence"].ToString());
            Overall = Convert.ToInt32(reader["overall"].ToString());
            Potential = Convert.ToInt32(reader["potential"].ToString());
            Position = reader["position"].ToString();
            Value = Convert.ToInt32(reader["value"].ToString());
            Salary = Convert.ToInt32(reader["salary"].ToString());
        }

        public Player(string name, string surname, string club, DateTime dateofbirth, string nationality, string position, DateTime contractTerminates, int offense, int defence, int overall, int potential)
        {
            Name = name;
            Surname = surname;
            Club = club;
            DateOfBirth = dateofbirth;
            Nationality = nationality;
            ContractTerminates = contractTerminates;
            Offense = offense;
            Defence = defence;
            Overall = overall;
            Potential = potential;
            Position = position;
        }

        public Player(Player player)
        {
            Name = player.Name;
            Surname = player.Surname;
            Club = player.Club;
            DateOfBirth = player.DateOfBirth;
            Nationality = player.Nationality;
            ContractTerminates = player.ContractTerminates;
            Offense = player.Offense;
            Defence = player.Defence;
            Overall = player.Overall;
            Potential = player.Potential;
            Position = player.Position;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(Name);
            stringBuilder.Append(" ");
            stringBuilder.Append(Surname);
            stringBuilder.Append(" ");
            stringBuilder.Append(Club);
            stringBuilder.Append(" ");
            stringBuilder.Append(Nationality);
            stringBuilder.Append(" ");
            stringBuilder.Append(Position);
            stringBuilder.Append(" ");
            stringBuilder.Append(Overall);

            return stringBuilder.ToString();
        }
    }
}
