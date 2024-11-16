#if !UNITY_EDITOR_OSX && UNITY_EDITOR && FALSE
#define HAS_CODEDOM
#endif
#if HAS_CODEDOM
using System.CodeDom;
using System.CodeDom.Compiler;
#endif
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public static class StringExtentions {
	public static string PrintUniCodeCharacterInfo(this string s) {
		string res = "";
		for (int i = 0; i < s.Length; i++) {
			res += "!" + s[i] + "[" + ((int) s[i]) + "]";
		}

		res += "!";
		return res;
	}

	public static int ToInt(this string s) {
		return int.Parse(s, CultureInfo.InvariantCulture.NumberFormat);
	}

	public static float ToFloat(this string s) {
		return float.Parse(s, CultureInfo.InvariantCulture.NumberFormat);
	}

	public static bool TryToFloat(this string s, out float result) {
		return float.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out result);
	}

	public static bool TryToInt(this string s, out int result) {
		return int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture.NumberFormat, out result);
	}

	public static string CapitalizeFirstLetter(this string s) {
		return s[0].ToString().ToUpper() + s.Substring(1);
	}

	public static string LowerFirstLetter(this string s) {
		return s[0].ToString().ToLower() + s.Substring(1);
	}

	public static string Invert(this string s) {
		string result = "";
		for (int i = s.Length - 1; i >= 0; i--) {
			result += s[i];
		}

		return result;
	}

	public static void CopyToClipboard(this string s) {
		GUIUtility.systemCopyBuffer = s;
	}

	public static string ToCSharpLiteral(this string input) {
		using var writer = new StringWriter();

#if HAS_CODEDOM
			using var provider = CodeDomProvider.CreateProvider("CSharp");
	        provider.GenerateCodeFromExpression(new CodePrimitiveExpression(input), writer, null);
#endif

		return writer.ToString();
	}
	
	public static List<string[]> ParseCSV(this string csvContent)
	{
		List<string[]> csvData = new List<string[]>();
		StringBuilder currentField = new StringBuilder();
		List<string> currentRow = new List<string>();
		bool insideQuotes = false;

		for (int i = 0; i < csvContent.Length; i++)
		{
			char c = csvContent[i];

			if (c == '"')
			{
				if (insideQuotes && i + 1 < csvContent.Length && csvContent[i + 1] == '"')
				{
					// Handle escaped quotes
					currentField.Append('"');
					i++; // Skip the next quote
				}
				else
				{
					// Toggle the insideQuotes flag
					insideQuotes = !insideQuotes;
				}
			}
			else if (c == ',' && !insideQuotes)
			{
				// End of a field
				currentRow.Add(currentField.ToString());
				currentField.Clear();
			}
			else if ((c == '\n' || c == '\r') && !insideQuotes)
			{
				// End of a line
				if (currentField.Length > 0 || currentRow.Count > 0)
				{
					currentRow.Add(currentField.ToString());
					currentField.Clear();
				}

				if (currentRow.Count > 0)
				{
					csvData.Add(currentRow.ToArray());
					currentRow.Clear();
				}
			}
			else
			{
				// Regular character, add to the current field
				currentField.Append(c);
			}
		}

		// Add the last row if there's data
		if (currentField.Length > 0 || currentRow.Count > 0)
		{
			currentRow.Add(currentField.ToString());
			csvData.Add(currentRow.ToArray());
		}

		return csvData;
	}
}
