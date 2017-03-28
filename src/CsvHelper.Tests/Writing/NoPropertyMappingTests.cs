using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvHelper.Tests.Writing
{
	[TestClass]
	public class NoPropertyMappingTests
	{
		[TestMethod]
		public void NoPropertyWithHeaderAndNameTest()
		{
			using( var stream = new MemoryStream() )
			using( var reader = new StreamReader( stream ) )
			using( var writer = new StreamWriter( stream ) )
			using( var csv = new CsvWriter( writer ) )
			{
				var list = new List<Test>
				{
					new Test { Id = 1, Value = 10 },
					new Test { Id = 2, Value = 20 }
				};

				csv.Configuration.RegisterClassMap<TestWithNameMap>();
				csv.WriteRecords( list );

				writer.Flush();
				stream.Position = 0;

				var result = reader.ReadToEnd();

				var expected = new StringBuilder();
				expected.AppendLine( "Id,Constant,Value" );
				expected.AppendLine( "1,const,10" );
				expected.AppendLine( "2,const,20" );

				Assert.AreEqual( expected.ToString(), result );
			}
		}

		[TestMethod]
		public void NoPropertyWithHeaderAndNoNameTest()
		{
			using( var stream = new MemoryStream() )
			using( var reader = new StreamReader( stream ) )
			using( var writer = new StreamWriter( stream ) )
			using( var csv = new CsvWriter( writer ) )
			{
				var list = new List<Test>
				{
					new Test { Id = 1, Value = 10 },
					new Test { Id = 2, Value = 20 }
				};

				csv.Configuration.RegisterClassMap<TestWithNoNameMap>();
				csv.WriteRecords( list );

				writer.Flush();
				stream.Position = 0;

				var result = reader.ReadToEnd();

				var expected = new StringBuilder();
				expected.AppendLine( "Id,,Value" );
				expected.AppendLine( "1,const,10" );
				expected.AppendLine( "2,const,20" );

				Assert.AreEqual( expected.ToString(), result );
			}
		}

		[TestMethod]
		public void NoPropertyWithNoHeaderAndNameTest()
		{
			using( var stream = new MemoryStream() )
			using( var reader = new StreamReader( stream ) )
			using( var writer = new StreamWriter( stream ) )
			using( var csv = new CsvWriter( writer ) )
			{
				var list = new List<Test>
				{
					new Test { Id = 1, Value = 10 },
					new Test { Id = 2, Value = 20 }
				};

				csv.Configuration.HasHeaderRecord = false;
				csv.Configuration.RegisterClassMap<TestWithNameMap>();
				csv.WriteRecords( list );

				writer.Flush();
				stream.Position = 0;

				var result = reader.ReadToEnd();

				var expected = new StringBuilder();
				expected.AppendLine( "1,const,10" );
				expected.AppendLine( "2,const,20" );

				Assert.AreEqual( expected.ToString(), result );
			}
		}

		[TestMethod]
		public void NoPropertyWithNoHeaderAndNoNameTest()
		{
			using( var stream = new MemoryStream() )
			using( var reader = new StreamReader( stream ) )
			using( var writer = new StreamWriter( stream ) )
			using( var csv = new CsvWriter( writer ) )
			{
				var list = new List<Test>
				{
					new Test { Id = 1, Value = 10 },
					new Test { Id = 2, Value = 20 }
				};

				csv.Configuration.HasHeaderRecord = false;
				csv.Configuration.RegisterClassMap<TestWithNoNameMap>();
				csv.WriteRecords( list );

				writer.Flush();
				stream.Position = 0;

				var result = reader.ReadToEnd();

				var expected = new StringBuilder();
				expected.AppendLine( "1,const,10" );
				expected.AppendLine( "2,const,20" );

				Assert.AreEqual( expected.ToString(), result );
			}
		}

		private class Test
		{
			public int Id { get; set; }
            public int Value { get; set; }
		}

		private sealed class TestWithNameMap : CsvClassMap<Test>
		{
			public TestWithNameMap()
			{
				Map( m => m.Id );
				Map().Name( "Constant" ).Constant( "const" );
				Map( m => m.Value );
			}
		}

		private sealed class TestWithNoNameMap : CsvClassMap<Test>
		{
			public TestWithNoNameMap()
			{
				Map( m => m.Id );
				Map().Constant( "const" );
				Map(m => m.Value);
			}
		}
	}
}
