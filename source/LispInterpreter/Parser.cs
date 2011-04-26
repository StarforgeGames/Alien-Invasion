using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LispInterpreter
{
    class Parser
    {
        public dynamic parse(IEnumerator<char> e)
        {
            dynamic result = null;
            bool preventShift = false;

            while (true)
            {
                if (!preventShift)
                {
                    if (!e.MoveNext())
                    {
                        break;
                    }
                }
                preventShift = false;

                char c = e.Current;
                if (char.IsNumber(c))
                {
                    result = parseNumber(e);
                    preventShift = true;
                }
                else if (c == '"')
                    result = parseText(e);
                else if (c == '\'')
                    result = parseQuote(e);
                else if (c == '(')
                    result = parseList(e);
                else if (char.IsWhiteSpace(c))
                    continue;
                else
                {
                    result = parseSymbol(e);
                    preventShift = true;
                }
                break;
            }
            return result;
        }

        private LispSymbol parseSymbol(IEnumerator<char> e)
        {
            StringBuilder temp = new StringBuilder();
            do
            {
                char c = e.Current;
                if (!char.IsWhiteSpace(c) && (c != '(') && (c != '.') && (c != '\'') && (c != ')'))
                {
                    temp.Append(c);
                }
                else
                {
                    break;
                }
            } while (e.MoveNext());

            return new LispSymbol(temp.ToString());
        }

        private LispList parseList(IEnumerator<char> e)
        {
            LispList l = new LispList();
            bool preventShift = false;

            while (true)
            {
                if (!preventShift)
                {
                    if (!e.MoveNext())
                    {
                        break;
                    }
                }
                preventShift = false;

                dynamic elem;

                char c = e.Current;
                if (char.IsNumber(c))
                {
                    elem = parseNumber(e);
                    preventShift = true;
                }
                else if (c == '"')
                    elem = parseText(e);
                else if (c == '\'')
                    elem = parseQuote(e);
                else if (c == '(')
                    elem = parseList(e);
                else if (char.IsWhiteSpace(c))
                    continue;
                else if (c == ')')
                {
                    elem = null;
                    break;
                }
                else
                {
                    elem = parseSymbol(e);
                    preventShift = true;
                }
                l.Add(elem);
            }

            return l;
        }

        private LispList parseQuote(IEnumerator<char> e)
        {
            var list = new LispList();

            list.Add(new LispQuote());
            list.Add(parse(e));

            return list;
        }

        private LispString parseText(IEnumerator<char> e)
        {
            StringBuilder temp = new StringBuilder();
            while (e.MoveNext())
            {
                if (e.Current == '"')
                {
                    break;
                }
                else
                {
                    temp.Append(e.Current);
                }
            }
            return new LispString(temp.ToString());
        }

        private dynamic parseNumber(IEnumerator<char> e)
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
                    temp.Append('.');
                    isInteger = false;
                }
                else
                {
                    break;
                }
            } while (e.MoveNext() && isInteger);

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

                return new LispInteger(value);
            }
            else
            {
                double value = double.Parse(temp.ToString());
                return new LispFloat(value);
            }
        }
    }
}
