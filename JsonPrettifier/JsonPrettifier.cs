using System;
using System.Linq;
using System.Text;

namespace JsonPrettifier
{
	public class JsonPrettifier
	{
		private readonly string _indentText;
		
		public JsonPrettifier(string indentText = "  ")
		{
			_indentText = indentText;
		}

		/// <summary>
		/// prettifies a json string
		/// json in accordance with the RFC 8259 specification
		/// </summary>
		/// <param name="json">valid json input</param>
		/// <returns>prettified json</returns>
		public string PrettifyJson(string json)
		{
			var output = new StringBuilder();
			var inString = false;
			var indents = "";
			var indentCount = 0;
			var startContainer = false;
			
			for(var i = 0; i < json.Length; i++)
			{
				/*
				 * loops through the json, setting flags to monitor for two special cases which
				 * affect the output:
				 * 1 - ignore json grammar characters when inside a string
				 * 2 - don't put the } or ] character on a new line if the object/array is empty
				 */

				var character = json[i];
				if (inString)
				{
					output.Append(character);
					switch (character)
					{
						case '\\':
							var nextCharacter = json[i + 1];
							if (nextCharacter == '"' || nextCharacter == '\\') {
								/*
								 * when the escaped character is the backslash or the quote character,
								 * write the next character directly to the output and skip processing
								 * to avoid treating an escaped quote as the end of the string
								 */
								output.Append(nextCharacter);
								i++;
							}
							break;
						case '"':
							//end of string
							inString = false;
							break;
					}
				}
				else
				{
					switch (character)
					{
						case '{':
						case '[':
							//object/array start
							indents = string.Concat(Enumerable.Repeat(_indentText, ++indentCount));
							output.Append(character);
							startContainer = true;
							break;
						case '}':
						case ']':
							//object/array end
							indents = string.Concat(Enumerable.Repeat(_indentText, --indentCount));
							if (!startContainer) output.Append($"\n{indents}");
							startContainer = false;
							output.Append(character);
							break;
						case ':':
							output.Append(": ");
							break;
						case ',':
							output.Append($",\n{indents}");
							break;
						case '\t':
						case '\n':
						case '\r':
						case ' ':
							//ignore existing whitespace
							break;
						default:
							if (startContainer)
							{
								//first value in an object/array so prefix with a newline and indent
								output.Append($"\n{indents}");
								startContainer = false;
							}
							//check for start of string values
							inString = character == '"';
							output.Append(character);
							break;
					}
				}
			}

			return output.ToString();
		}
	}
}
