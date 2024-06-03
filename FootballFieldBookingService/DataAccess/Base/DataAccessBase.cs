using CommonLib;
using Dapper;
using DataAccess.Base;
using log4net.Core;
using Microsoft.AspNetCore.Mvc.Rendering;
using ObjectInfo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class DataAccessBase<T> : IDataAccess<T> where T : MasterDataBase, new()
    {
        public virtual string DbPackage { get; } = string.Empty;
        public virtual string ProfileKeyField { get; } = string.Empty;
        public virtual string DbTable => string.Empty;
        public virtual string ViewDBTable => string.Empty;
        public virtual string SeqName => string.Empty;

        public virtual string InsertSqlText => string.Empty;
        public virtual string UpdateSqlText => string.Empty;

        // GET
        #region GET

        public virtual List<T> GetAll(string requestId)
        {
            var requestTime = DateTime.Now;
            Logger.log.Info($"[{requestId}] Start.");

            List<T> result = null;

            using var connection = SQLHelper.GetConnection(Config_Info.gConnectionString);

            result = connection.Query<T>($"SELECT * FROM {DbTable}").ToList();

            //
            Logger.log.Info($"[{requestId}] End. Tong thoi gian {ConstLog.GetProcessingMilliseconds(requestTime)} (ms)");
            return result;
        }

        public virtual T GetById(string requestId, decimal id)
        {
            using var connection = SQLHelper.GetConnection(Config_Info.gConnectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            return GetById(requestId, transaction, id);
        }

        public virtual T GetById(string requestId, IDbTransaction transaction, decimal id)
        {
            var requestTime = DateTime.Now;
            Logger.log.Info($"[{requestId}] Start. id=[{id}]]");

            string sqlText = $"SELECT * FROM {DbTable} WHERE {ProfileKeyField} = :id";
            var result = transaction.Connection.QueryFirstOrDefault<T>(sqlText, new { id }, transaction);

            //
            Logger.log.Info($"[{requestId}] End. Tong thoi gian {ConstLog.GetProcessingMilliseconds(requestTime)} (ms)");
            return result;
        }

        #endregion

        // INSERT
        #region INSERT

        public virtual long Insert(string requestId, T data)
        {
            using var connection = SQLHelper.GetConnection(Config_Info.gConnectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            var result = Insert(requestId, transaction, data);
            if (result > 0)
            {
                transaction.Commit();
            }
            else
            {
                transaction.Rollback();
            }

            return result;
        }

        public virtual long Insert(string requestId, IDbTransaction transaction, T data)
        {
            var requestTime = DateTime.Now;
            Logger.log.Info($"[{requestId}] Start. data=[{JsonHelper.Serialize(data)}]");

            long result = ErrorCodes.Err_Unknown;

            if (data == null)
            {
                result = ErrorCodes.Err_DataNull;
                goto endFunc;
            }

            //
            var insertCount = transaction.Connection.Execute(InsertSqlText, data, transaction);

            result = insertCount > 0 ? ErrorCodes.Success : ErrorCodes.Err_InvalidData;

            //
            if (result > 0)
            {
                result = InsertChildData(requestId, transaction, data);
            }

        //
        endFunc:
            Logger.log.Info($"[{requestId}] End. Tong thoi gian {ConstLog.GetProcessingMilliseconds(requestTime)} (ms)");
            return result;
        }

        public virtual long Insert_List(string requestId, List<T> datas, out List<long> resCodes)
        {
            using var connection = SQLHelper.GetConnection(Config_Info.gConnectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            var result = Insert_List(requestId, transaction, datas, out resCodes);
            if (result > 0)
            {
                transaction.Commit();
            }
            else
            {
                transaction.Rollback();
            }

            return result;
        }

        public virtual long Insert_List(string requestId, IDbTransaction transaction, List<T> datas, out List<long> resCodes)
        {
            var requestTime = DateTime.Now;
            Logger.log.Info($"[{requestId}] Start. datas=[{JsonHelper.Serialize(datas)}]");

            resCodes = new List<long>();
            long result = ErrorCodes.Err_Unknown;

            if (datas == null || datas.Count == 0)
            {
                result = ErrorCodes.Err_DataNull;
                goto endFunc;
            }

            // Duyet insert tung ban ghi
            for (int i = 0; i < datas.Count; i++)
            {
                resCodes.Add(Insert(requestId, transaction, datas[i]));
            }

            //
            if (resCodes.All(x => x > 0))
            {
                result = ErrorCodes.Success;
            }
            else
            {
                result = resCodes.Any(x => x < 0) ? resCodes.FirstOrDefault(x => x < 0) : ErrorCodes.Err_InvalidData;
            }

        //
        endFunc:
            Logger.log.Info($"[{requestId}] End. Tong thoi gian {ConstLog.GetProcessingMilliseconds(requestTime)} (ms)");
            return result;
        }

        public virtual long InsertChildData(string requestId, IDbTransaction transaction, T data)
        {
            return ErrorCodes.Success;
        }

        #endregion

        // UPDATE
        #region UPDATE

        public virtual long Update(string requestId, T data)
        {
            using var connection = SQLHelper.GetConnection(Config_Info.gConnectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            var result = Update(requestId, transaction, data);
            if (result > 0)
            {
                transaction.Commit();
            }
            else
            {
                transaction.Rollback();
            }

            return result;
        }

        public virtual long Update(string requestId, IDbTransaction transaction, T data)
        {
            var requestTime = DateTime.Now;
            Logger.log.Info($"[{requestId}] Start. data=[{JsonHelper.Serialize(data)}]");

            long result = ErrorCodes.Err_Unknown;

            if (data == null)
            {
                result = ErrorCodes.Err_DataNull;
                goto endFunc;
            }

            //
            var updatedCount = transaction.Connection.Execute(UpdateSqlText, data, transaction);

            result = updatedCount == 1 ? ErrorCodes.Success : ErrorCodes.Err_InvalidData;

            //
            if (result > 0)
            {
                result = UpdateChildData(requestId, transaction, data);
            }

        //
        endFunc:
            Logger.log.Info($"[{requestId}] End. Tong thoi gian {ConstLog.GetProcessingMilliseconds(requestTime)} (ms)");
            return result;
        }

        public virtual long Update_List(string requestId, List<T> datas, out List<long> resCodes)
        {
            using var connection = SQLHelper.GetConnection(Config_Info.gConnectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            var result = Update_List(requestId, transaction, datas, out resCodes);
            if (result > 0)
            {
                transaction.Commit();
            }
            else
            {
                transaction.Rollback();
            }

            return result;
        }

        public virtual long Update_List(string requestId, IDbTransaction transaction, List<T> datas, out List<long> resCodes)
        {
            var requestTime = DateTime.Now;
            Logger.log.Info($"[{requestId}] Start. datas=[{JsonHelper.Serialize(datas)}]");

            resCodes = new List<long>();
            long result = ErrorCodes.Err_Unknown;

            if (datas == null || datas.Count == 0)
            {
                result = ErrorCodes.Err_DataNull;
                goto endFunc;
            }

            // Duyet insert tung ban ghi
            for (int i = 0; i < datas.Count; i++)
            {
                resCodes.Add(Update(requestId, transaction, datas[i]));
            }

            //
            if (resCodes.All(x => x > 0))
            {
                result = ErrorCodes.Success;
            }
            else
            {
                result = resCodes.Any(x => x < 0) ? resCodes.FirstOrDefault(x => x < 0) : ErrorCodes.Err_InvalidData;
            }

        //
        endFunc:
            Logger.log.Info($"[{requestId}] End. Tong thoi gian {ConstLog.GetProcessingMilliseconds(requestTime)} (ms)");
            return result;
        }

        public virtual long UpdateChildData(string requestId, IDbTransaction transaction, T data)
        {
            return ErrorCodes.Success;
        }

        #endregion


        // UPDATE
        #region DELETE

        public virtual long Delete(string requestId, decimal id)
        {
            using var connection = SQLHelper.GetConnection(Config_Info.gConnectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            var result = Delete(requestId, transaction, id);
            if (result > 0)
            {
                transaction.Commit();
            }
            else
            {
                transaction.Rollback();
            }

            return result;
        }

        public virtual long Delete(string requestId, IDbTransaction transaction, decimal id)
        {
            var requestTime = DateTime.Now;
            Logger.log.Info($"[{requestId}] Start. id=[{id}]");

            long result = ErrorCodes.Err_Unknown;

            if (id <= 0)
            {
                result = ErrorCodes.Err_DataNull;
                goto endFunc;
            }
            string sqlText = $"DELETE FROM {DbTable} WHERE {ProfileKeyField} = :id";
            //
            var deleteCount = transaction.Connection.Execute(UpdateSqlText, new { id }, transaction);

            result = deleteCount == 1 ? ErrorCodes.Success : ErrorCodes.Err_InvalidData;

        //
        endFunc:
            Logger.log.Info($"[{requestId}] End. Tong thoi gian {ConstLog.GetProcessingMilliseconds(requestTime)} (ms)");
            return result;
        }

        public virtual long Delete_List(string requestId, List<decimal> ids, out List<long> resCodes)
        {
            using var connection = SQLHelper.GetConnection(Config_Info.gConnectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            var result = Delete_List(requestId, transaction, ids, out resCodes);
            if (result > 0)
            {
                transaction.Commit();
            }
            else
            {
                transaction.Rollback();
            }

            return result;
        }

        public virtual long Delete_List(string requestId, IDbTransaction transaction, List<decimal> ids, out List<long> resCodes)
        {
            var requestTime = DateTime.Now;
            Logger.log.Info($"[{requestId}] Start. ids=[{JsonHelper.Serialize(ids)}]");

            resCodes = new List<long>();
            long result = ErrorCodes.Err_Unknown;

            if (ids == null || ids.Count == 0)
            {
                result = ErrorCodes.Err_DataNull;
                goto endFunc;
            }

            // Duyet insert tung ban ghi
            for (int i = 0; i < ids.Count; i++)
            {
                resCodes.Add(Delete(requestId, transaction, ids[i]));
            }

            //
            if (resCodes.All(x => x > 0))
            {
                result = ErrorCodes.Success;
            }
            else
            {
                result = resCodes.Any(x => x < 0) ? resCodes.FirstOrDefault(x => x < 0) : ErrorCodes.Err_InvalidData;
            }

        //
        endFunc:
            Logger.log.Info($"[{requestId}] End. Tong thoi gian {ConstLog.GetProcessingMilliseconds(requestTime)} (ms)");
            return result;
        }

        #endregion
    }
}
