using System;
using System.Collections.Generic;
using System.Text;
using MessagePack;

namespace AerospikeBenchmarks.Models
{
    [MessagePackObject]
    public class ProductMessagePack
    {
        [Key(0)]
        public int Id { get; set; }
        [Key(1)]
        public string Name { get; set; }
        [Key(2)]
        public string Description { get; set; }
        [Key(3)]
        public string Manufacturer { get; set; }
        [Key(4)]
        public float Price { get; set; }
    }
}
