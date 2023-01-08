using System;
using System.Collections.Generic;
using Realms;
using MongoDB.Bson;
public class ServersCollection : RealmObject
{
    [MapTo("_id")]
    [PrimaryKey]
    public ObjectId? Id { get; set; }
    public ObjectId ID { get; set; }
    public string IP { get; set; }
    public string ServerName { get; set; }
}