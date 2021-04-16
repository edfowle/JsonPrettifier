using JsonPrettifier;
using NUnit.Framework;

namespace JsonPrettifier.UnitTests
{
	public class JsonPrettifierTests
	{
		private JsonPrettifier _jsonPrettifier { get; set; }
		
		[SetUp]
		public void Setup()
		{
			_jsonPrettifier = new JsonPrettifier();
		}
		
		[TestCase("{\"test\":[\"A\",\"B\",{\"C\":[null],\"D\":{\"E:\":20.5,\"F\":{}}}]}", "{\n  \"test\": [\n    \"A\",\n    \"B\",\n    {\n      \"C\": [\n        null\n      ],\n      \"D\": {\n        \"E:\": 20.5,\n        \"F\": {}\n      }\n    }\n  ]\n}")]
		[TestCase("\"\"", "\"\"")]
		[TestCase("[\"\\\"\",0]", "[\n  \"\\\"\",\n  0\n]")]
		[TestCase("[\"\\\\\",0]", "[\n  \"\\\\\",\n  0\n]")]
		[TestCase("[\"\\u1234\",0]", "[\n  \"\\u1234\",\n  0\n]")]
		public void PrettifyJson(string input, string expectedOutput)
		{
			var output = _jsonPrettifier.PrettifyJson(input);
			Assert.AreEqual(expectedOutput, output);
		}
	}
}