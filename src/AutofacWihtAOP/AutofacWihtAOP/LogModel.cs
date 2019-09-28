using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AutofacWithAOP
{
    public class AuditLogModel
    {
        [BsonId]
        public ObjectId ID { get; set; }
        public int UserID { get; set; }
        public string UserCode { get; set; }
        public string FieldName { get; set; }
        public string FunctionName { get; set; }
        public string Action { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string Remark { get; set; }
        public string IP { get; set; }
        public DateTime CreateDate { get; set; }
    }
}