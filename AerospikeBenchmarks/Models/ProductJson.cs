using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace AerospikeBenchmarks.Models
{
    public class ProductJson
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public float Price { get; set; }
    }
}
