using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Aerospike.Client;
using AerospikeBenchmarks.Models;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;
using MessagePack;
using Newtonsoft.Json;
using ProtoBuf;

namespace AerospikeBenchmarks
{
    [Config(typeof(BenchmarkConfig))]
    [SimpleJob(RunStrategy.Throughput, targetCount: 10, launchCount: 5, warmupCount: 5)]
    public class AerospikeBenchmarks
    {
        // Aerospike config
        private AerospikeClient _client;
        private const string Namespace = "nsdev";
        private const string Set = "setBenchmark";
        private const string Bin = "binBench";

        // Aerospike serialization/deserialization keys
        private const string NewtonsoftBenchmarkKey = "NewtonsoftBenchmark";
        private const string ProtobufBenchmarkKey = "ProtobufBenchmark";
        private const string MessagePackBenchmarkKey = "MessagePackBenchmark";
        private const string DotNetJsonBenchmarkKey = "DotNetJsonBenchmark";

        // Aerospike Scan/Query benchmark keys
        private const string IncrementalKeyBase = "pk";
        private int TestObjectCount = 5000;

        [GlobalSetup]
        public void Setup()
        {
            _client = new AerospikeClient("192.168.1.54", 3000);
            CreateTestObjectsEnMasse();
        }

        public void CreateTestObjectsEnMasse()
        {
            for (int x = 0; x < TestObjectCount; x++)
            {
                ProductProtobuf product = new ProductProtobuf()
                {
                    Id = x,
                    Description = "The next generation of widgets are here, order one today!",
                    Manufacturer = "ACME Corp",
                    Name = "Widget 2.0",
                    Price = 5.00f
                };

                var serializedProdct = JsonConvert.SerializeObject(product);

                Bin bin = new Bin(Bin, serializedProdct);
                string testKey = $"{IncrementalKeyBase}-{x}";
                Key key = new Key(Namespace, Set, testKey);

                _client.Put(null, key, bin);
            }
        }

        [Benchmark]
        public void ScanBenchmark()
        {
            ScanPolicy policy = new ScanPolicy();
            policy.concurrentNodes = true;
            policy.priority = Priority.DEFAULT;
            policy.includeBinData = true;

            _client.ScanAll(policy, Namespace, Set, ScanCallback);
        }

        [Benchmark]
        public void GetBenchmark()
        {
            for (int x = 0; x < TestObjectCount; x++)
            {
                string testKey = $"{IncrementalKeyBase}-{x}";
                Key key = new Key(Namespace, Set, testKey);
                Record record = _client.Get(null, key);

                if (record != null)
                {
                    foreach (KeyValuePair<string, object> product in record.bins)
                    {
                        var prod = (string)product.Value;
                        var deserializedProdct = JsonConvert.DeserializeObject<ProductJson>(prod);
                    }
                }
            }
        }

        
        [Benchmark]
        public void NewtonsoftJsonSerializationBenchmark()
        {
            ProductJson product = new ProductJson()
            {
                Id = 1,
                Description = "The next generation of widgets are here, order one today!",
                Manufacturer = "ACME Corp",
                Name = "Widget 2.0",
                Price = 5.00f
            };

            var serializedProdct = JsonConvert.SerializeObject(product);
            
            Bin bin = new Bin(Bin, serializedProdct);
            Key key = new Key(Namespace, Set, NewtonsoftBenchmarkKey);

            _client.Put(null, key, bin);
        }

        [Benchmark]
        public void NewtonsoftJsonDeserializationBenchmark()
        {
            Key key = new Key(Namespace, Set, NewtonsoftBenchmarkKey);
            Record record = _client.Get(null, key);

            if (record != null)
            {
                foreach (KeyValuePair<string, object> product in record.bins)
                {
                    var prod = (string) product.Value;
                    var deserializedProdct = JsonConvert.DeserializeObject<ProductJson>(prod);
                }
            }
        }

        [Benchmark]
        public void DotNetJsonSerializationBenchmark()
        {
            ProductJson product = new ProductJson()
            {
                Id = 1,
                Description = "The next generation of widgets are here, order one today!",
                Manufacturer = "ACME Corp",
                Name = "Widget 2.0",
                Price = 5.00f
            };

            var serializedProdct = System.Text.Json.JsonSerializer.Serialize(product);

            Bin bin = new Bin(Bin, serializedProdct);
            Key key = new Key(Namespace, Set, DotNetJsonBenchmarkKey);

            _client.Put(null, key, bin);
        }

        [Benchmark]
        public void DotNetJsonDeserializationBenchmark()
        {
            Key key = new Key(Namespace, Set, DotNetJsonBenchmarkKey);
            Record record = _client.Get(null, key);

            if (record != null)
            {
                foreach (KeyValuePair<string, object> product in record.bins)
                {
                    var prod = (string)product.Value;
                    var deserializedProdct = System.Text.Json.JsonSerializer.Deserialize<ProductJson>(prod);
                }
            }
        }

        [Benchmark]
        public void MessagePackSerializationBenchmark()
        {
            ProductMessagePack product = new ProductMessagePack()
            {
                Id = 1,
                Description = "The next generation of widgets are here, order one today!",
                Manufacturer = "ACME Corp",
                Name = "Widget 2.0",
                Price = 5.00f
            };

            var serializedProdct = MessagePackSerializer.Serialize(product);

            Bin bin = new Bin(Bin, serializedProdct);
            Key key = new Key(Namespace, Set, MessagePackBenchmarkKey);

            _client.Put(null, key, bin);
        }

        [Benchmark]
        public void MessagePackDeserializationBenchmark()
        {
            Key key = new Key(Namespace, Set, MessagePackBenchmarkKey);
            Record record = _client.Get(null, key);

            if (record != null)
            {
                foreach (KeyValuePair<string, object> product in record.bins)
                {
                    var prod = (byte []) product.Value;
                    var deserializedProdct = MessagePackSerializer.Deserialize<ProductMessagePack>(prod);
                }
            }
        }

        [Benchmark]
        public void ProtobufSerializationBenchmark()
        {
            ProductProtobuf product = new ProductProtobuf()
            {
                Id = 1,
                Description = "The next generation of widgets are here, order one today!",
                Manufacturer = "ACME Corp",
                Name = "Widget 2.0",
                Price = 5.00f
            };

            var serializedProdct = ProtoSerializeProduct(product);

            Bin bin = new Bin(Bin, serializedProdct);
            Key key = new Key(Namespace, Set, ProtobufBenchmarkKey);

            _client.Put(null, key, bin);
        }

        [Benchmark]
        public void ProtobufDeserializationBenchmark()
        {
            Key key = new Key(Namespace, Set, ProtobufBenchmarkKey);
            Record record = _client.Get(null, key);

            if (record != null)
            {
                foreach (KeyValuePair<string, object> product in record.bins)
                {
                    var prod = (byte[])product.Value;
                    var deserializedProdct = ProtoDeserializeProdct(prod);
                }
            }
        }

        public byte[] ProtoSerializeProduct(ProductProtobuf product)
        {
            using (var stream = new MemoryStream())
            {
                Serializer.Serialize(stream, product);
                return stream.ToArray();
            }
        }

        public ProductProtobuf ProtoDeserializeProdct(byte[] serializedData)
        {
            using (var stream = new MemoryStream(serializedData))
            {
                return Serializer.Deserialize<ProductProtobuf>(stream);
            }
        }

        public void ScanCallback(Key key, Record record)
        {
            if (record != null)
            {
                foreach (KeyValuePair<string, object> product in record.bins)
                {
                    var prod = (string)product.Value;
                    var deserializedProdct = JsonConvert.DeserializeObject<ProductJson>(prod);
                }
            }
        }
    }
}
