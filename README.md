# Aerospike Benchmarks
A collection of benchmarks to help gauge the performance of reading and writing data to and from the Aerospike NoSQL database. 

# Requirements 
* .NET Core 3.1
* Aerospike Community Edition 4+

# Setup
You'll need to point the Aerospike client to your cluster's hostname and port. If you have Aerospike Enterprise edition and have authentication enabled, you'll need to add your credentials and applicable authentication code. After that, just run the application in Release mode and wait for the results to appear in the /bin/Release/netcoreapp3.1/BenchmarkDotNet.Artifacts folder.

# Currently supported benchmarks
* Serialization and deserialization benchmark with reading and writing serialized/deserialized data 
	* Newtonsoft.JSON
	* System.Text.Json
	* MessagePack
	* Protobuf-net
* Scan vs Get to retrieve all records and bins in a set

# Benchmark Results
You can view benchmark results derived from running on my machines listed below. For reference, I have also included the specifications of my Aerospike server.

Aerospike Server
* Type: VM
* Build: Aerospike Community Edition 4.5.2.2
* OS: CentOS 6
* CPU: 2 cores of an Intel Core i5-3450
* RAM: 2GB
* Disk: SSD

https://shravanj.com/dev/aerospike/benchmarks/serialization.html

https://shravanj.com/dev/aerospike/benchmarks/scanvsget.html

# Special Thanks
Special thanks to [Jeremy Cantu](https://github.com/Jac21) and his project [BenchmarkDotNetDeepDive](https://github.com/Jac21/CSharpMenagerie/tree/master/Reference/BenchmarkDotNetDeepDive) for the inspiration and initial setup!
