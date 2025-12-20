using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
	public static class MiniJSON
	{
		public static object Deserialize(string json)
		{
			if (json == null)
			{
				return null;
			}
			return MiniJSON.Parser.Parse(json);
		}

		public static string Serialize(object obj)
		{
			return MiniJSON.Serializer.Serialize(obj);
		}

		private sealed class Parser : IDisposable
		{
			private Parser(string jsonString)
			{
				this.json = new StringReader(jsonString);
			}

			public static bool IsWordBreak(char c)
			{
				return char.IsWhiteSpace(c) || "{}[],:\"".IndexOf(c) != -1;
			}

			public static object Parse(string jsonString)
			{
				object result;
				using (MiniJSON.Parser parser = new MiniJSON.Parser(jsonString))
				{
					result = parser.ParseValue();
				}
				return result;
			}

			public void Dispose()
			{
				this.json.Dispose();
				this.json = null;
			}

			private Dictionary<string, object> ParseObject()
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				this.json.Read();
				for (;;)
				{
					MiniJSON.Parser.TOKEN nextToken = this.NextToken;
					switch (nextToken)
					{
					case MiniJSON.Parser.TOKEN.NONE:
						goto IL_37;
					default:
						if (nextToken != MiniJSON.Parser.TOKEN.COMMA)
						{
							string text = this.ParseString();
							if (text == null)
							{
								goto Block_2;
							}
							if (this.NextToken != MiniJSON.Parser.TOKEN.COLON)
							{
								goto Block_3;
							}
							this.json.Read();
							dictionary[text] = this.ParseValue();
						}
						break;
					case MiniJSON.Parser.TOKEN.CURLY_CLOSE:
						return dictionary;
					}
				}
				IL_37:
				return null;
				Block_2:
				return null;
				Block_3:
				return null;
			}

			private List<object> ParseArray()
			{
				List<object> list = new List<object>();
				this.json.Read();
				bool flag = true;
				while (flag)
				{
					MiniJSON.Parser.TOKEN nextToken = this.NextToken;
					switch (nextToken)
					{
					case MiniJSON.Parser.TOKEN.SQUARED_CLOSE:
						flag = false;
						break;
					default:
					{
						if (nextToken == MiniJSON.Parser.TOKEN.NONE)
						{
							return null;
						}
						object item = this.ParseByToken(nextToken);
						list.Add(item);
						break;
					}
					case MiniJSON.Parser.TOKEN.COMMA:
						break;
					}
				}
				return list;
			}

			private object ParseValue()
			{
				MiniJSON.Parser.TOKEN nextToken = this.NextToken;
				return this.ParseByToken(nextToken);
			}

			private object ParseByToken(MiniJSON.Parser.TOKEN token)
			{
				switch (token)
				{
				case MiniJSON.Parser.TOKEN.STRING:
					return this.ParseString();
				case MiniJSON.Parser.TOKEN.NUMBER:
					return this.ParseNumber();
				case MiniJSON.Parser.TOKEN.TRUE:
					return true;
				case MiniJSON.Parser.TOKEN.FALSE:
					return false;
				case MiniJSON.Parser.TOKEN.NULL:
					return null;
				default:
					switch (token)
					{
					case MiniJSON.Parser.TOKEN.CURLY_OPEN:
						return this.ParseObject();
					case MiniJSON.Parser.TOKEN.SQUARED_OPEN:
						return this.ParseArray();
					}
					return null;
				}
			}

			private string ParseString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				this.json.Read();
				bool flag = true;
				while (flag)
				{
					if (this.json.Peek() == -1)
					{
						break;
					}
					char nextChar = this.NextChar;
					if (nextChar != '"')
					{
						if (nextChar != '\\')
						{
							stringBuilder.Append(nextChar);
						}
						else if (this.json.Peek() == -1)
						{
							flag = false;
						}
						else
						{
							nextChar = this.NextChar;
							switch (nextChar)
							{
							case 'r':
								stringBuilder.Append('\r');
								break;
							default:
								if (nextChar != '"' && nextChar != '/' && nextChar != '\\')
								{
									if (nextChar != 'b')
									{
										if (nextChar != 'f')
										{
											if (nextChar == 'n')
											{
												stringBuilder.Append('\n');
											}
										}
										else
										{
											stringBuilder.Append('\f');
										}
									}
									else
									{
										stringBuilder.Append('\b');
									}
								}
								else
								{
									stringBuilder.Append(nextChar);
								}
								break;
							case 't':
								stringBuilder.Append('\t');
								break;
							case 'u':
							{
								char[] array = new char[4];
								for (int i = 0; i < 4; i++)
								{
									array[i] = this.NextChar;
								}
								stringBuilder.Append((char)Convert.ToInt32(new string(array), 16));
								break;
							}
							}
						}
					}
					else
					{
						flag = false;
					}
				}
				return stringBuilder.ToString();
			}

			private object ParseNumber()
			{
				string nextWord = this.NextWord;
				if (nextWord.IndexOf('.') == -1)
				{
					long num;
					long.TryParse(nextWord, NumberStyles.Any, CultureInfo.InvariantCulture, out num);
					return num;
				}
				double num2;
				double.TryParse(nextWord, NumberStyles.Any, CultureInfo.InvariantCulture, out num2);
				return num2;
			}

			private void EatWhitespace()
			{
				while (char.IsWhiteSpace(this.PeekChar))
				{
					this.json.Read();
					if (this.json.Peek() == -1)
					{
						break;
					}
				}
			}

			private char PeekChar
			{
				get
				{
					return Convert.ToChar(this.json.Peek());
				}
			}

			private char NextChar
			{
				get
				{
					return Convert.ToChar(this.json.Read());
				}
			}

			private string NextWord
			{
				get
				{
					StringBuilder stringBuilder = new StringBuilder();
					while (!MiniJSON.Parser.IsWordBreak(this.PeekChar))
					{
						stringBuilder.Append(this.NextChar);
						if (this.json.Peek() == -1)
						{
							break;
						}
					}
					return stringBuilder.ToString();
				}
			}

			private MiniJSON.Parser.TOKEN NextToken
			{
				get
				{
					this.EatWhitespace();
					if (this.json.Peek() == -1)
					{
						return MiniJSON.Parser.TOKEN.NONE;
					}
					char peekChar = this.PeekChar;
					switch (peekChar)
					{
					case ',':
						this.json.Read();
						return MiniJSON.Parser.TOKEN.COMMA;
					case '-':
					case '0':
					case '1':
					case '2':
					case '3':
					case '4':
					case '5':
					case '6':
					case '7':
					case '8':
					case '9':
						return MiniJSON.Parser.TOKEN.NUMBER;
					default:
						switch (peekChar)
						{
						case '[':
							return MiniJSON.Parser.TOKEN.SQUARED_OPEN;
						default:
							switch (peekChar)
							{
							case '{':
								return MiniJSON.Parser.TOKEN.CURLY_OPEN;
							default:
								if (peekChar != '"')
								{
									string nextWord = this.NextWord;
									if (nextWord != null)
									{
										if (nextWord == "false")
										{
											return MiniJSON.Parser.TOKEN.FALSE;
										}
										if (nextWord == "true")
										{
											return MiniJSON.Parser.TOKEN.TRUE;
										}
										if (nextWord == "null")
										{
											return MiniJSON.Parser.TOKEN.NULL;
										}
									}
									return MiniJSON.Parser.TOKEN.NONE;
								}
								return MiniJSON.Parser.TOKEN.STRING;
							case '}':
								this.json.Read();
								return MiniJSON.Parser.TOKEN.CURLY_CLOSE;
							}
							break;
						case ']':
							this.json.Read();
							return MiniJSON.Parser.TOKEN.SQUARED_CLOSE;
						}
						break;
					case ':':
						return MiniJSON.Parser.TOKEN.COLON;
					}
				}
			}

			private const string WORD_BREAK = "{}[],:\"";

			private StringReader json;

			private enum TOKEN
			{
				NONE,
				CURLY_OPEN,
				CURLY_CLOSE,
				SQUARED_OPEN,
				SQUARED_CLOSE,
				COLON,
				COMMA,
				STRING,
				NUMBER,
				TRUE,
				FALSE,
				NULL
			}
		}

		private sealed class Serializer
		{
			private Serializer()
			{
				this.builder = new StringBuilder();
			}

			public static string Serialize(object obj)
			{
				MiniJSON.Serializer serializer = new MiniJSON.Serializer();
				serializer.SerializeValue(obj, 1);
				return serializer.builder.ToString();
			}

			private void SerializeValue(object value, int indentationLevel)
			{
				string str;
				IList anArray;
				IDictionary obj;
				if (value == null)
				{
					this.builder.Append("null");
				}
				else if ((str = (value as string)) != null)
				{
					this.SerializeString(str);
				}
				else if (value is bool)
				{
					this.builder.Append((!(bool)value) ? "false" : "true");
				}
				else if ((anArray = (value as IList)) != null)
				{
					this.SerializeArray(anArray, indentationLevel);
				}
				else if ((obj = (value as IDictionary)) != null)
				{
					this.SerializeObject(obj, indentationLevel);
				}
				else if (value is char)
				{
					this.SerializeString(new string((char)value, 1));
				}
				else
				{
					this.SerializeOther(value);
				}
			}

			private void SerializeObject(IDictionary obj, int indentationLevel)
			{
				bool flag = true;
				this.builder.Append('{');
				this.builder.Append('\n');
				for (int i = 0; i < indentationLevel; i++)
				{
					this.builder.Append('\t');
				}
				IEnumerator enumerator = obj.Keys.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj2 = enumerator.Current;
						if (!flag)
						{
							this.builder.Append(',');
							this.builder.Append('\n');
							for (int j = 0; j < indentationLevel; j++)
							{
								this.builder.Append('\t');
							}
						}
						this.SerializeString(obj2.ToString());
						this.builder.Append(':');
						indentationLevel++;
						this.SerializeValue(obj[obj2], indentationLevel);
						indentationLevel--;
						flag = false;
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
				this.builder.Append('\n');
				for (int k = 0; k < indentationLevel - 1; k++)
				{
					this.builder.Append('\t');
				}
				this.builder.Append('}');
			}

			private void SerializeArray(IList anArray, int indentationLevel)
			{
				this.builder.Append('[');
				bool flag = true;
				for (int i = 0; i < anArray.Count; i++)
				{
					object value = anArray[i];
					if (!flag)
					{
						this.builder.Append(',');
					}
					this.SerializeValue(value, indentationLevel);
					flag = false;
				}
				this.builder.Append(']');
			}

			private void SerializeString(string str)
			{
				this.builder.Append('"');
				foreach (char c in str.ToCharArray())
				{
					switch (c)
					{
					case '\b':
						this.builder.Append("\\b");
						break;
					case '\t':
						this.builder.Append("\\t");
						break;
					case '\n':
						this.builder.Append("\\n");
						break;
					default:
						if (c != '"')
						{
							if (c != '\\')
							{
								int num = Convert.ToInt32(c);
								if (num >= 32 && num <= 126)
								{
									this.builder.Append(c);
								}
								else
								{
									this.builder.Append("\\u");
									this.builder.Append(num.ToString("x4"));
								}
							}
							else
							{
								this.builder.Append("\\\\");
							}
						}
						else
						{
							this.builder.Append("\\\"");
						}
						break;
					case '\f':
						this.builder.Append("\\f");
						break;
					case '\r':
						this.builder.Append("\\r");
						break;
					}
				}
				this.builder.Append('"');
			}

			private void SerializeOther(object value)
			{
				if (value is float)
				{
					this.builder.Append(((float)value).ToString("R", CultureInfo.InvariantCulture));
				}
				else if (value is int || value is uint || value is long || value is sbyte || value is byte || value is short || value is ushort || value is ulong)
				{
					this.builder.Append(value);
				}
				else if (value is double || value is decimal)
				{
					this.builder.Append(Convert.ToDouble(value).ToString("R", CultureInfo.InvariantCulture));
				}
				else if (value is Vector2)
				{
					Vector2 vector = (Vector2)value;
					this.builder.Append(string.Concat(new string[]
					{
						"\"(",
						vector.x.ToString("R", CultureInfo.InvariantCulture),
						",",
						vector.y.ToString("R", CultureInfo.InvariantCulture),
						")\""
					}));
				}
				else if (value is Vector3)
				{
					Vector3 vector2 = (Vector3)value;
					this.builder.Append(string.Concat(new string[]
					{
						"\"(",
						vector2.x.ToString("R", CultureInfo.InvariantCulture),
						",",
						vector2.y.ToString("R", CultureInfo.InvariantCulture),
						",",
						vector2.z.ToString("R", CultureInfo.InvariantCulture),
						")\""
					}));
				}
				else if (value is Vector4)
				{
					Vector4 vector3 = (Vector4)value;
					this.builder.Append(string.Concat(new string[]
					{
						"\"(",
						vector3.x.ToString("R", CultureInfo.InvariantCulture),
						",",
						vector3.y.ToString("R", CultureInfo.InvariantCulture),
						",",
						vector3.z.ToString("R", CultureInfo.InvariantCulture),
						",",
						vector3.w.ToString("R", CultureInfo.InvariantCulture),
						")\""
					}));
				}
				else if (value is Quaternion)
				{
					Quaternion quaternion = (Quaternion)value;
					this.builder.Append(string.Concat(new string[]
					{
						"\"(",
						quaternion.x.ToString("R", CultureInfo.InvariantCulture),
						",",
						quaternion.y.ToString("R", CultureInfo.InvariantCulture),
						",",
						quaternion.z.ToString("R", CultureInfo.InvariantCulture),
						",",
						quaternion.w.ToString("R", CultureInfo.InvariantCulture),
						")\""
					}));
				}
				else
				{
					this.SerializeString(value.ToString());
				}
			}

			private StringBuilder builder;
		}
	}
}
