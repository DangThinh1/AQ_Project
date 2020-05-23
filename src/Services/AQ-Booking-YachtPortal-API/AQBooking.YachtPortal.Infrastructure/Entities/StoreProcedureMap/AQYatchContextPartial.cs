using AQBooking.YachtPortal.Core.Models.Yachts.StoreProcedure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace AQBooking.YachtPortal.Infrastructure.Entities
{
    public partial class AQYachtContext
    {
      
        /// <summary>
        /// Map with store procedure usp_Yatch_Search
        /// </summary>
        public DbSet<YachtSearchItem> YachtSearchs { get; set; }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<YachtSearchItem>()
                .Ignore(x=>x.CustomProperties)
                .HasKey(x => x.YachtID)                
                ;
            modelBuilder.Entity<YachtSimilarItem>()
             .Ignore(x => x.CustomProperties)
             .HasKey(x => x.YachtID)

             ;

        }
      
        public IQueryable<TEntity> EntityFromSql<TEntity>(string sql, params DbParameter[] parameters) where TEntity : class
        {
            return Set<TEntity>().FromSql(CreateSqlWithParameters(sql, parameters), parameters);
        }
        public DbParameter GetParameter(string parameterName, object parameterValue)
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
