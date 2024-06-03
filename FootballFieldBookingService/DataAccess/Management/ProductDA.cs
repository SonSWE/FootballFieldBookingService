using CommonLib;
using ObjectInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Management
{
    public class ProductDA : DataAccessBase<ProductInfo>
    {
        public override string ProfileKeyField => CommonLib.ProfileKeyField.PRODUCT;
        public override string DbTable => CommonLib.DbTable.PRODUCT;
        public override string InsertSqlText => $@"
                INSERT INTO {DbTable} (
                    ProductId, ProductName, Price, Count, Description,
                    CreateBy, CreateDate
                ) VALUES (
                    :ProductId, :ProductName, :Price, :Count, :Description,
                    :CreateBy, :CreateDate
                )";
        public override string UpdateSqlText => $@"
                UPDATE {DbTable} SET 
                    ProductName = :ProductName, 
                    Price = :Price, 
                    Count = :Count,  
                    Description = :Description, 
                    
                    Modified_By = :Modified_By, 
                    Modified_Date = :Modified_Date
                WHERE {ProfileKeyField} = :ProductId ";
    }
}
