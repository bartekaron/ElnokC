using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace elnok_BA
{
    class dbClass
    {
        private string dbhost;
        private string dbuser;
        private string dbpass;
        private string dbname;
        public MySqlConnection conn;
        public MySqlDataReader results;
        public int AffectedRows;
        public string msg;
        public bool debug = true;

        public dbClass(string dbhost, string dbuser, string dbpass, string dbname)
        {
            this.dbhost = dbhost;
            this.dbuser = dbuser;
            this.dbpass = dbpass;
            this.dbname = dbname;
            string connectionString = "server=" + this.dbhost + ";uid=" + this.dbuser + ";pwd=" + this.dbpass + ";database=" + this.dbname;
            this.conn = new MySqlConnection(connectionString);
        }

        // insert
        public void insert(string table, string[] fields, string[] values)
        {
            try
            {
                this.conn.Close();
                this.conn.Open();
                string fieldsList = string.Join(", ", fields);
                string valuesList = string.Join("', '", values);
                string sql = $"INSERT INTO {table} ({fieldsList}) VALUES('{valuesList}')";
                MySqlCommand cmd = new MySqlCommand(sql, this.conn);
                this.AffectedRows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                this.AffectedRows = -1;
                if (this.debug)
                {
                    this.msg = ex.Message;
                }
            }
        }

        // selectAll
        public void selectAll(string table)
        {
            try
            {
                this.conn.Close();
                this.conn.Open();
                string sql = $"SELECT * FROM {table}";
                MySqlCommand cmd = new MySqlCommand(sql, this.conn);
                this.results = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                this.AffectedRows = -1;
                if (this.debug)
                {
                    this.msg = ex.Message;
                }
            }

        }

        // select
        public void select(string table, string field, string value, string op = "eq")
        {
            try
            {
                this.conn.Close();
                this.conn.Open();
                op = this.GetOp(op);
                if (op == "like")
                {
                    value = "%" + value + "%";
                }
                string sql = $"SELECT * FROM {table} WHERE {field} {op} '{value}'";
                MySqlCommand cmd = new MySqlCommand(sql, this.conn);
                this.results = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                this.AffectedRows = -1;
                if (this.debug)
                {
                    this.msg = ex.Message;
                }
            }

        }

        // update
        public void update(string table, string field, string value, string[] fields, string[] values, string op = "eq")
        {
            try
            {
                this.conn.Close();
                this.conn.Open();
                string newdata = "";

                for (int i = 0; i < fields.Length; i++)
                {
                    newdata += fields[i] + "='" + values[i] + "',";
                }
                newdata = newdata.TrimEnd(',');
                op = this.GetOp(op);
                if (op == "like")
                {
                    value = "%" + value + "%";
                }
                string sql = $"UPDATE {table} SET {newdata} WHERE {field} {op} '{value}'";
                MySqlCommand cmd = new MySqlCommand(sql, this.conn);
                this.AffectedRows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                this.AffectedRows = -1;
                if (this.debug)
                {
                    this.msg = ex.Message;
                }
            }


        }

        // deleteAll
        public void deleteAll(string table)
        {
            try
            {
                this.conn.Close();
                this.conn.Open();
                string sql = $"DELETE FROM {table}";
                MySqlCommand cmd = new MySqlCommand(sql, this.conn);
                this.AffectedRows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                this.AffectedRows = -1;
                if (this.debug)
                {
                    this.msg = ex.Message;
                }
            }

        }

        // delete
        public void delete(string table, string field, string value, string op = "eq")
        {
            try
            {
                this.conn.Close();
                this.conn.Open();
                op = this.GetOp(op);
                if (op == "like")
                {
                    value = "%" + value + "%";
                }
                string sql = $"DELETE FROM {table} WHERE {field} {op} {value}";
                MySqlCommand cmd = new MySqlCommand(sql, this.conn);
                this.AffectedRows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                this.AffectedRows = -1;
                if (this.debug)
                {
                    this.msg = ex.Message;
                }
            }



        }

        public void selectBetween(string table, string field, string value_1, string value_2)
        {
            try
            {
                this.conn.Close();
                this.conn.Open();

                string sql = $"SELECT * FROM {table} WHERE {field}  BETWEEN '{value_1}' AND '{value_2}'";
                MySqlCommand cmd = new MySqlCommand(sql, this.conn);
                this.results = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                this.AffectedRows = -1;
                if (this.debug)
                {
                    this.msg = ex.Message;
                }
            }

        }

        private string GetOp(string value)
        {
            try
            {
                // eq, lt, gt, lte, gte, nt, lk
                string op = "=";
                switch (value)
                {
                    case "eq": { op = "="; break; }
                    case "lt": { op = "<"; break; }
                    case "gt": { op = ">"; break; }
                    case "lte": { op = "<="; break; }
                    case "gte": { op = ">="; break; }
                    case "nt": { op = "<>"; break; }
                    case "lk": { op = "like"; break; }
                }
                return op;
            }
            catch (Exception ex)
            {
                this.AffectedRows = -1;
                if (this.debug)
                {
                    this.msg = ex.Message;

                }
                return msg;
            }

        }
    }
}

