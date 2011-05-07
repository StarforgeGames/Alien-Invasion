using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;

namespace LispInterpreter
{
    class Parser
    {
        public List<LispElement> parse(Stream stream)
        {
            var reader = new StreamReader(stream);
            var expressions = new List<LispElement>();
            
            while (!reader.EndOfStream)
            {
                LispElement expression = parseExpression(reader);
                expressions.Add(expression);
            }
            return expressions;
        }

        private LispElement parseExpression(StreamReader reader)
        {
            skipWhitespaces(reader);

            if (reader.EndOfStream)
            {
                throw new NotSupportedException("Tried to parse Expression at the end of the stream");
            }
            char c = (char)reader.Peek();

            switch (c)
            {
                case '"':
                    return parseString(reader);
                case '\'':
                    return parseQuote(reader);
                case '(':
                    return parseList(reader);
                default:
                    if (char.IsNumber(c))
                    {
                        return parseNumber(reader);
                    }
                    else
                    {
                        return parseSymbol(reader);
                    }
            }
        }

        private LispList parseList(StreamReader reader)
        {
            reader.Read();

            skipWhitespaces(reader);
            var list = new LispList();

            while (!reader.EndOfStream)
            {
                char c = (char)reader.Peek();

                if (c == ')')
                {
                    reader.Read();
                    return list;
                }
                else
                {
                    dynamic expression = parseExpression(reader);
                    list.Add(expression);
                }

                skipWhitespaces(reader);
            }

            throw new NotImplementedException("Missing closing brace!");
        }

        private LispSymbol parseSymbol(StreamReader reader)
        {
            StringBuilder temp = new StringBuilder();
            while(!reader.EndOfStream)
            {
                char c = (char)reader.Peek();
                if (!char.IsWhiteSpace(c) && (c != '"')  && (c != '(') && (c != '.') && (c != '\'') && (c != ')'))
                {
                    reader.Read();
                    temp.Append(c);
                }
                else
                {
                    break;
                }
            }

            return new LispSymbol(temp.ToString());
        }

        private LispElement parseNumber(StreamReader reader)
        {
            StringBuilder temp = new StringBuilder();

            do
            {
                if (reader.EndOfStream)
                {
                    if (temp.Length == 0)
                    {
                        throw new NotSupportedException("Tried to parse a number without any digits!");
                    }
                    else
                    {
                        int value = int.Parse(temp.ToString());
                        return new LispInteger(value);
                    }
                }

                char c = (char)reader.Peek();

                if (char.IsNumber(c))
                {
                    temp.Append(c);
                    reader.Read();
                }
                else if (c == 'f')
                {
                    reader.Read();
                                
                    float value = float.Parse(temp.ToString(), NumberFormatInfo.InvariantInfo);
                    return new LispFloat(value);
                }
                else if (c == '.')
                {
                    temp.Append('.');
                    reader.Read();
                    break;
                }
                else
                {
                    int value = int.Parse(temp.ToString());
                    return new LispInteger(value);
                }
                
            } while (true);

            do
            {
                if (reader.EndOfStream)
                {
                    double value = double.Parse(temp.ToString(), NumberFormatInfo.InvariantInfo);
                    return new LispDouble(value);
                }
                char c = (char)reader.Peek();

                if (char.IsNumber(c))
                {
                    temp.Append(c);
                    reader.Read();
                }
                else if (c == 'f')
                {
                    reader.Read();

                    float value = float.Parse(temp.ToString(), NumberFormatInfo.InvariantInfo);
                    return new LispFloat(value);
                }
                else
                {
                    break;
                }
            } while (true);

            throw new NotImplementedException("There is something seriously wrong with number parsing!");
        }

        private LispList parseQuote(StreamReader reader)
        {
            reader.Read();

            var list = new LispList();

            list.Add(new LispSymbol("quote"));
            list.Add(parseExpression(reader));

            return list;
        }

        private LispString parseString(StreamReader reader)
        {
            reader.Read();

            var text = new StringBuilder();
            
            while (!reader.EndOfStream)
            {
                char c = (char)reader.Peek();
                if (c == '"')
                {
                    reader.Read();
                    return new LispString(text.ToString());
                }
                else
                {
                    reader.Read();
                    text.Append(c);
                }
            }
            throw new NotSupportedException("File ended while trying to parse string. Closing doublequotes might be missing!");
        }

        private void skipWhitespaces(StreamReader reader)
        {
            while (!reader.EndOfStream)
            {
                char c = (char)reader.Peek();
                if (char.IsWhiteSpace(c))
                {
                    reader.Read();
                }
                else
                {
                    return;
                }
            }
        }
    }
}
