﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSRebot.GenerateEntityTools.Entity;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace CSRebot.GenerateEntityTools.Traversers
{
    public class MySqlTraverser : ITraverser
    {

        MySqlConnectionStringBuilder _connectionStringBuilder;

        public MySqlTraverser(CommandOptions options)
        {
            //todo 这里可以查询项目配置文件中的appsettings.json或app.config中的连接字符串

            // "--host=127.0.01","--db=testdb","--user=root","--pwd=gsw2021","--port=3069"
            _connectionStringBuilder = new MySqlConnectionStringBuilder()
            {
                Server = options["--host"],
                Database = options["--db"],
                UserID = options["--user"],
                Password = options["--pwd"],
                Port = options.ContainsKey("--port") ? uint.Parse(options["--port"]) : 3306,
            };
        }
        public DataBase Traverse()
        {
            return GetDataBase();
        }
        DataBase GetDataBase()
        {
            var dataBase = new DataBase()
            {
                DataBaseName = _connectionStringBuilder.Database
            };

            using (var con = new MySqlConnection(_connectionStringBuilder.ConnectionString))
            {
                var sql = @$"select table_name as tablename,table_comment as tabledescribe from information_schema.tables where table_schema='{_connectionStringBuilder.Database}' and table_type='BASE TABLE';";
                var cmd = new MySqlCommand(sql, con);
                con.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var table = new Table();
                    table.TableName = reader.GetFieldValue<string>("tablename");
                    table.TableDescribe = reader.GetFieldValue<string>("tabledescribe");
                    dataBase.Tables.Add(table);
                }
                con.Close();
            }
            GetFields(dataBase);
            return dataBase;
        }


        void GetFields(DataBase entityDir)
        {
            foreach (var table in entityDir.Tables)
            {
                var sql = @$"select character_maximum_length as fieldsize,column_name as fieldname,data_type as dbtype,column_comment as fielddescribe from information_schema.columns where table_name = '{table.TableName}' ";
                using (var con = new MySqlConnection(_connectionStringBuilder.ConnectionString))
                {
                    var cmd = new MySqlCommand(sql, con);
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var field = new Field();
                        field.FieldName = reader.GetFieldValue<string>("fieldname");
                        field.FieldDescribe = reader.GetFieldValue<string>("fielddescribe");
                        field.DBType = reader.GetFieldValue<string>("dbtype");
                        var size = reader.GetFieldValue<object>("fieldsize");
                        if (size != DBNull.Value)
                        {
                            field.FieldSize =Convert.ToInt64(size);
                        }
                        table.Fields.Add(field);
                    }
                }
            }
        }

    }


}
