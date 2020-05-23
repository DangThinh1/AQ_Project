using AQBooking.Admin.Core.Models.Post;
using AQBooking.Admin.Core.Models.PostDetail;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace AQBooking.Admin.Infrastructure.Databases.CMSEntities
{
    public partial class AQCMSContext
    {

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            // map with usp_Post_Search
            modelBuilder.Entity<PostViewModel>()
                .Ignore(x => x.CustomProperties)
                .HasKey(x => x.PostID)
                ;

            // map with usp_Post_Next_Prev
            modelBuilder.Entity<NavigationInfo>()
                .HasKey(x => x.PostID);
        }

        public IQueryable<TEntity> EntityFromSql<TEntity>(string sql, params DbParameter[] parameters) where TEntity : class
        {
            return Set<TEntity>().FromSqlRaw(CreateSqlWithParameters(sql, parameters), parameters);
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
