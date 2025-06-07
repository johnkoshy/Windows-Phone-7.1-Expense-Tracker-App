using System;
using System.Runtime.Serialization;  // Required for DataContract

[DataContract]  // Now this should work
public class Expense
{
    [DataMember]  // And this too
    public double Amount { get; set; }

    [DataMember]
    public string Category { get; set; }

    [DataMember]
    public DateTime Date { get; set; }

    [DataMember]
    public string Notes { get; set; }
}