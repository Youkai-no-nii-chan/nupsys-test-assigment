using System.Runtime.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Model;

[DataContract]
public class Data
{
    [BsonId]
    [DataMember]
    public MongoDB.Bson.ObjectId Id { get; set; }
    
    [DataMember]
    public DateTime Timestamp { get; set; }
    
    [DataMember]
    public double Value { get; set; }
}