using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuf;

namespace AerospikeBenchmarks.Models
{
    [ProtoContract]
    public class ProductProtobuf
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }
        [ProtoMember(3)]
        public string Description { get; set; }
        [ProtoMember(4)]
        public string Manufacturer { get; set; }
        [ProtoMember(5)]
        public float Price { get; set; }
    }
}
