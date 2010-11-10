using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LispInterpreter
{
    public class Interpreter
    {

        public void Eval(char[] code)
        {
            parse(code.AsEnumerable().GetEnumerator());
        }

        public void parse(IEnumerator<char> e)
        {
            while(e.MoveNext())
            {
                char c = e.Current;
                if (char.IsNumber(c))
                    parseNumber(e);
                else if (c == '"')
                    parseText(e);
                else if (c == '\'')
                    parseQuote(e);
                else if (c == '(')
                    parseFunction(e);
                else if (char.IsWhiteSpace(c))
                    continue;
                else
                {
                    parseSymbol(e);
                }
            }

        }

        private void parseSymbol(IEnumerator<char> e)
        {
            StringBuilder temp = new StringBuilder();
            do
            {
                if (e.Current == '"')
                {
                    e.MoveNext();
                    break;
                }
                else
                {
                    temp.Append(e.Current);
                }
            } while (e.MoveNext());
            throw new NotImplementedException();
        }

        private void parseFunction(IEnumerator<char> e)
        {
            throw new NotImplementedException();
        }

        private void parseQuote(IEnumerator<char> e)
        {
            throw new NotImplementedException();
        }

        private void parseText(IEnumerator<char> e)
        {
            e.MoveNext();
            StringBuilder temp = new StringBuilder();
            do
            {
                if (e.Current == '"')
                {
                    e.MoveNext();
                    break;
                }
                else
                {
                    temp.Append(e.Current);
                }
            } while (e.MoveNext());
            throw new NotImplementedException();
        }

        private void parseNumber(IEnumerator<char> e)
        {
            StringBuilder temp = new StringBuilder();
            bool isInteger = true;

            do
            {
                if (char.IsNumber(e.Current))
                {
                    temp.Append(e.Current);
                }
                else if (e.Current == '.')
                {
                    isInteger = false;
                }
                else
                {
                    break;
                }
            }while(e.MoveNext() && isInteger);

            do
            {
                if (char.IsNumber(e.Current))
                {
                    temp.Append(e.Current);
                }
                else
                {
                    break;
                }
            } while (e.MoveNext());

            if (isInteger)
            {
                int value = int.Parse(temp.ToString());
            }
            else
            {
                double value = double.Parse(temp.ToString());
            }

            throw new NotImplementedException();
        }
    }
}
