using ObjectInfo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Base
{
    public interface IDataAccess<T> where T : MasterDataBase, new()
    {
        string DbPackage { get; }
        string ProfileKeyField { get; }
        string DbTable { get; }
        string ViewDBTable { get; }
        string SeqName { get; }
        string InsertSqlText { get; }
        string UpdateSqlText { get; }

        List<T> GetAll(string requestId);
        T GetById(string requestId, decimal id);
        T GetById(string requestId, IDbTransaction transaction, decimal id);
        long Insert(string requestId, T data);
        long Insert(string requestId, IDbTransaction transaction, T data);
        long Insert_List(string requestId, List<T> datas, out List<long> resCodes);
        long Insert_List(string requestId, IDbTransaction transaction, List<T> datas, out List<long> resCodes);
        long Update(string requestId, T data);
        long Update(string requestId, IDbTransaction transaction, T data);
        long Update_List(string requestId, List<T> datas, out List<long> resCodes);
        long Update_List(string requestId, IDbTransaction transaction, List<T> datas, out List<long> resCodes);
        long Delete(string requestId, decimal id);
        long Delete(string requestId, IDbTransaction transaction, decimal id);
        long Delete_List(string requestId, List<decimal> ids, out List<long> resCodes);
        long Delete_List(string requestId, IDbTransaction transaction, List<decimal> ids, out List<long> resCodes);
    }
}
