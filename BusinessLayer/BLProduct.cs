using DataLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace SampleWebService.BusinessLayer
{
    public class BLProduct : BLCommon
    {
        #region Product Properties
        public string sProductCode { get; set; }
        public int iProductLikeCount { get; set; }
        #endregion

        #region Object related variables
        DbFactory ObjDbfactory;
        #endregion

        #region Initialize database connection method
        protected void InitializeDb()
        {
            ObjDbfactory = new DbFactory(DataBaseType.SQLServer, ConfigurationManager.ConnectionStrings["ExCambiiConnectionString"].ConnectionString);
        }
        #endregion

        /// <summary>
        /// Like/Unlike product by an user
        /// </summary>
        /// <param name="userCode">Unique id of the user</param>
        /// <param name="isLiked">is user like the product or not</param>
        /// <returns></returns>
        public BLProduct LikeProduct(string userCode, bool isLiked)
        {
            DbDataReader dbReader = null;
            try
            {
                InitializeDb();
                List<DbParams> objLstDbParams = new List<DbParams>();
                objLstDbParams.Add(new DbParams(DbType.Guid, 100, userCode, "@UserCode", ParameterDirection.Input));
                objLstDbParams.Add(new DbParams(DbType.Guid, 100, sProductCode, "@ProductCode", ParameterDirection.Input));
                objLstDbParams.Add(new DbParams(DbType.Boolean, 100, isLiked, "@IsLiked", ParameterDirection.Input));
                dbReader = ObjDbfactory.GetReader("SP_LikeProduct", false, objLstDbParams);
                while (dbReader.Read())
                {
                    IResultNo = ToInteger(dbReader["ResultValue"]);
                    SResult = ToString(dbReader["Result"]);
                    if (IResultNo == 1)
                    {
                        iProductLikeCount = ToInteger(dbReader["LikeCount"]);
                    }
                }
            }
            catch (Exception ex)
            {
                SendLogMail("BLUser : LikeProduct", ex.Message);
            }
            return this;
        }
    }
}