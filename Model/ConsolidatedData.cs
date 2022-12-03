using System.Runtime.Serialization;

namespace Model;

[DataContract]
public class ConsolidatedData
{
    [DataMember]
    public DateTime Timestamp { get; set; }
    
    [DataMember]
    public double Value { get; set; }
}