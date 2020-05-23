using Microsoft.EntityFrameworkCore;
using AQConfigurations.Infrastructure.Databases.Entities;
using System.Data.Common;
using System.Linq;
using System.Data.SqlClient;
using System;
using System.Data;

namespace AQConfigurations.Infrastructure.Databases
{
    public partial class AQConfigurationsDbContext
    {

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {

        }

        public IQueryable<TEntity> EntityFromSql<TEntity>(string sql, params DbParameter[] parameters) where TEntity : class
        {
            return Set<TEntity>().FromSql(CreateSqlWithParameters(sql, parameters), parameters);
        }
        public  DbParameter GetParameter(string parameterName, object parameterValue)
        {
            var parameter = new SqlParameter();
            parameter.ParameterName = parameterName;
            if (parameterValue == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
                parameter.Value = parameterValue;
            return parameter;
        }
        protected virtual string CreateSqlWithParameters(string sql, params DbParameter[] parameters)
        {
            //add parameters to sql
            for (var i = 0; i <= (parameters?.Length ?? 0) - 1; i++)
            {
                DbParameter parameter = parameters[i];

                sql = $"{sql}{(i > 0 ? "," : string.Empty)} {parameter.ParameterName}={parameter.ParameterName}1";
                parameter.ParameterName = $"{parameter.ParameterName}1";
                //whether parameter is output
                if (parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Output)
                    sql = $"{sql} output";
            }

            return sql;
        }

    }
}
