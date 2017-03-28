using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsvHelper.Tests.Writing
{
	[TestClass]
	public class FixedWidthTests
	{
		[TestMethod]
		public void AllowEmptyDelimiter()
		{
			using (var stream = new MemoryStream())
			using (var reader = new StreamReader(stream))
			using (var writer = new StreamWriter(stream))
			using (var csv = new CsvWriter(writer))
			{
				csv.Configuration.Delimiter = "";

				var records = new List<Test>
				{
					new Test { Id = 1, Name = "one" },
					new Test { Id = 2, Name = "two" }
				};

				csv.Configuration.RegisterClassMap<SimpleTestMap>();
				csv.WriteRecords(records);
				writer.Flush();
				stream.Position = 0;

				var expected = new StringBuilder();
				expected.AppendLine("IdName");
				expected.AppendLine("1one");
				expected.AppendLine("2two");

				var result = reader.ReadToEnd();

				Assert.AreEqual(expected.ToString(), result);
			}
		}

		[TestMethod]
		public void WidthMapping()
		{
			using (var stream = new MemoryStream())
			using (var reader = new StreamReader(stream))
			using (var writer = new StreamWriter(stream))
			using (var csv = new CsvWriter(writer))
			{
				csv.Configuration.Delimiter = "";
				csv.Configuration.QuoteNoFields = true;
				csv.Configuration.HasHeaderRecord = false;

				var records = new List<Test>
				{
					new Test { Id = 1, Name = "one" },
					new Test { Id = 2, Name = "two" }
				};

				csv.Configuration.RegisterClassMap<WidthTestMap>();
				csv.WriteRecords(records);
				writer.Flush();
				stream.Position = 0;

				var expected = new StringBuilder();
				/////////////////////123451234567890
				expected.AppendLine("1    one       ");
				expected.AppendLine("2    two       ");

				var result = reader.ReadToEnd();

				Assert.AreEqual(expected.ToString(), result);
			}
		}

		[TestMethod]
		public void WidthAlignRightMapping()
		{
			using (var stream = new MemoryStream())
			using (var reader = new StreamReader(stream))
			using (var writer = new StreamWriter(stream))
			using (var csv = new CsvWriter(writer))
			{
				csv.Configuration.Delimiter = "";
				csv.Configuration.QuoteNoFields = true;
				csv.Configuration.HasHeaderRecord = false;

				var records = new List<Test>
				{
					new Test { Id = 1, Name = "one" },
					new Test { Id = 2, Name = "two" }
				};

				csv.Configuration.RegisterClassMap<WidthAlighRightTestMap>();
				csv.WriteRecords(records);
				writer.Flush();
				stream.Position = 0;

				var expected = new StringBuilder();
				/////////////////////123451234567890
				expected.AppendLine("    1one       ");
				expected.AppendLine("    2two       ");

				var result = reader.ReadToEnd();

				Assert.AreEqual(expected.ToString(), result);
			}
		}

		[TestMethod]
		public void WidthAlignLeftErrorModeTrimMapping()
		{
			using (var stream = new MemoryStream())
			using (var reader = new StreamReader(stream))
			using (var writer = new StreamWriter(stream))
			using (var csv = new CsvWriter(writer))
			{
				csv.Configuration.Delimiter = "";
				csv.Configuration.QuoteNoFields = true;
				csv.Configuration.HasHeaderRecord = false;

				var records = new List<Test>
				{
					new Test { Id = 123456, Name = "one" },
					new Test { Id = 2, Name = "two" }
				};

				csv.Configuration.RegisterClassMap<WidthTestMap>();
				csv.WriteRecords(records);
				writer.Flush();
				stream.Position = 0;

				var expected = new StringBuilder();
				/////////////////////123451234567890
				expected.AppendLine("12345one       ");
				expected.AppendLine("2    two       ");

				var result = reader.ReadToEnd();

				Assert.AreEqual(expected.ToString(), result);
			}
		}

		[TestMethod]
		public void WidthAlignRightErrorModeTrimMapping()
		{
			using (var stream = new MemoryStream())
			using (var reader = new StreamReader(stream))
			using (var writer = new StreamWriter(stream))
			using (var csv = new CsvWriter(writer))
			{
				csv.Configuration.Delimiter = "";
				csv.Configuration.QuoteNoFields = true;
				csv.Configuration.HasHeaderRecord = false;

				var records = new List<Test>
				{
					new Test { Id = 123456, Name = "one" },
					new Test { Id = 2, Name = "two" }
				};

				csv.Configuration.RegisterClassMap<WidthAlighRightTestMap>();
				csv.WriteRecords(records);
				writer.Flush();
				stream.Position = 0;

				var expected = new StringBuilder();
				/////////////////////123451234567890
				expected.AppendLine("23456one       ");
				expected.AppendLine("    2two       ");

				var result = reader.ReadToEnd();

				Assert.AreEqual(expected.ToString(), result);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(CsvTypeConverterException))]
		public void WidthErrorModeThrowMapping()
		{
			using (var stream = new MemoryStream())
			using (var reader = new StreamReader(stream))
			using (var writer = new StreamWriter(stream))
			using (var csv = new CsvWriter(writer))
			{
				csv.Configuration.Delimiter = "";
				csv.Configuration.QuoteNoFields = true;
				csv.Configuration.HasHeaderRecord = false;

				var records = new List<Test>
				{
					new Test { Id = 123456, Name = "one" },
					new Test { Id = 2, Name = "two" }
				};

				csv.Configuration.RegisterClassMap<WidthErrorModeThrowTestMap>();
				csv.WriteRecords(records);
				writer.Flush();
				stream.Position = 0;

				var expected = new StringBuilder();
				/////////////////////123451234567890
				expected.AppendLine("23456one       ");
				expected.AppendLine("    2two       ");

				var result = reader.ReadToEnd();

				Assert.AreEqual(expected.ToString(), result);
			}
		}

		private class Test
		{
			public int Id { get; set; }
			public string Name { get; set; }
		}

		private sealed class SimpleTestMap : CsvClassMap<Test>
		{
			public SimpleTestMap()
			{
				Map(m => m.Id);
				Map(m => m.Name);
			}
		}

		private sealed class WidthTestMap : CsvClassMap<Test>
		{
			public WidthTestMap()
			{
				Map(m => m.Id).Width(5);
				Map(m => m.Name).Width(10);
			}
		}

		private sealed class WidthAlighRightTestMap : CsvClassMap<Test>
		{
			public WidthAlighRightTestMap()
			{
				Map(m => m.Id).Width(5).Align(CsvAlign.Right);
				Map(m => m.Name).Width(10);
			}
		}

		private sealed class WidthErrorModeThrowTestMap : CsvClassMap<Test>
		{
			public WidthErrorModeThrowTestMap()
			{
				Map(m => m.Id).Width(5).Align(CsvAlign.Right, CsvAlignErrorMode.Throw);
				Map(m => m.Name).Width(10);
			}
		}
	}
}
