using System;
using System.Collections.Generic;

namespace Transformation
{
    class RPNA  //Reverse Polish Notation Arithmetical expressions
    {
        //Метод возвращает true, если проверяемый символ - разделитель ("пробел" или "равно"
        static private bool IsDelimeter(char el)
        {
            if ((" =".IndexOf(el) != -1))
                return true;
            return false;
        }

        //Метод возвращает true, если проверяемый символ - оператор
        static private bool IsOperatorArif(char el)
        {
            if (("+-/*^()".IndexOf(el) != -1))
                return true;
            return false;
        }

        //Метод возвращает приоритет оператора
        static private byte GetPriorityArif(char el)
        {
            switch (el)
            {
                case '(': return 0;
                case ')': return 1;
                case '+': return 2;
                case '-': return 3;
                case '*': return 4;
                case '/': return 4;
                case '^': return 5;
                default: return 6;
            }
        }

        static public string GetExpressionArif(string input)
        {
            string output = string.Empty; //Строка для хранения выражения
            Stack<char> operStack = new Stack<char>(); //Стек для хранения операторов

            for (int i = 0; i < input.Length; i++) //Для каждого символа в входной строке
            {
                //Разделители пропускаем
                if (IsDelimeter(input[i]))
                    continue; //Переходим к следующему символу

                //Если символ - цифра, то считываем все число
                if (Char.IsDigit(input[i])) //Если цифра
                {
                    //Читаем до разделителя или оператора, что бы получить число
                    while (!IsDelimeter(input[i]) && !IsOperatorArif(input[i]))
                    {
                        output += input[i]; //Добавляем каждую цифру числа к нашей строке
                        i++; //Переходим к следующему символу

                        if (i == input.Length) break; //Если символ - последний, то выходим из цикла
                    }

                    output += " ";
                    i--; //Возвращаемся на один символ назад, к символу перед разделителем
                }

                //Если символ - оператор
                if (IsOperatorArif(input[i])) //Если оператор
                {
                    if (input[i] == '(') //Если символ - открывающая скобка
                        operStack.Push(input[i]); //Записываем её в стек
                    else if (input[i] == ')') //Если символ - закрывающая скобка
                    {
                        //Выписываем все операторы до открывающей скобки в строку
                        char s = operStack.Pop();

                        while (s != '(')
                        {
                            output += s.ToString() + ' ';
                            s = operStack.Pop();
                        }
                    }
                    else //Если любой другой оператор
                    {
                        if (operStack.Count > 0) //Если в стеке есть элементы
                            if (GetPriorityArif(input[i]) <= GetPriorityArif(operStack.Peek())) //Eсли приоритет нашего оператора меньше или равен приоритету оператора на вершине стека
                                output += operStack.Pop().ToString() + " "; //То добавляем последний оператор из стека в строку с выражением

                        operStack.Push(char.Parse(input[i].ToString())); //Если стек пуст или приоритет оператора выше - добавляем операторов на вершину стека

                    }
                }
            }

            //Когда прошли по всем символам, выкидываем из стека все оставшиеся там операторы в строку
            while (operStack.Count > 0)
                output += operStack.Pop() + " ";

            return output; //Возвращаем выражение в постфиксной записи
        }
    }

    class RPNL //Reverse Polish Notation Logical Expressions
    {
        static private bool IsDelimeter(char el)
        {
            if ((" =".IndexOf(el) != -1))
                return true;
            return false;
        }

        static private bool IsOperatorLog(string el)
        {
            List<string> actions = new List<string>() { "and", "or", "&", "^", "v", "not", "(", ")", "<->", "=", "~", "->", "+", "|", "*" };
            if (actions.IndexOf(el) != -1)
                return true;
            return false;
        }

        static private byte GetPriorityLog(string el)
        {
            switch (el)
            {
                case "(": return 0;
                case ")": return 1;
                case "<->": return 2; //Эквиваленция 
                case "=": return 2; //Эквиваленция 
                case "~": return 2; //Эквиваленция 
                case "->": return 3; //Импликация
                case "or": return 4; //Дизъюнкция 
                case "v": return 4; //Дизъюнкция 
                case "+": return 4; //Дизъюнкция 
                case "|": return 4; //Дизъюнкция 
                case "and": return 5; //Конъюнкция
                case "^": return 5; //Конъюнкция
                case "&": return 5; //Конъюнкция
                case "*": return 5; //Конъюнкция
                case "not": return 6; //Конъюнкция
                default: return 7;
            }
        }

        static private List<string> GetSplitLine(string input)
        {
            List<string> output = new List<string>();
            for (int i = 0; i < input.Length; i++)
            {
                if ("&^v=~+|*".IndexOf(input[i]) != -1)
                    output.Add(input[i].ToString());
                else if ("<-".IndexOf(input[i]) != -1)
                {
                    string element = string.Empty;
                    while (input[i] == '-' || input[i] == '>')
                    {
                        element += input[i];
                        i++;
                        if (i == input.Length) break;
                    }

                    output.Add(element);
                    i--;
                }
                else if (Char.IsLetter(input[i]))
                {
                    string element = string.Empty;
                    while (Char.IsLetter(input[i]) || char.IsDigit(input[i]))
                    {
                        element += input[i];
                        i++;
                        if (i == input.Length) break;
                    }

                    output.Add(element);
                    i--;
                }
                else if (input[i] == '(' || input[i] == ')')
                    output.Add(input[i].ToString());
            }
            return output;
        }

        static public string GetExpressionLog(string input)
        {
            string output = string.Empty;
            Stack<string> operStack = new Stack<string>();
            List<string> objects = GetSplitLine(input); //List хранит все объекты строки

            foreach (string element in objects)
            {
                if (!IsOperatorLog(element))
                    output += (element + " ");
                else if (IsOperatorLog(element))
                {
                    if (element == "(")
                        operStack.Push(element);
                    else if (element == ")")
                    {
                        string s = operStack.Pop();

                        while (s != "(")
                        {
                            output += s + " ";
                            s = operStack.Pop();
                        }
                    }
                    else
                    {
                        if (operStack.Count > 0)
                            if (GetPriorityLog(element) <= GetPriorityLog(operStack.Peek()))
                                output += operStack.Pop() + " ";

                        operStack.Push(element);
                    }
                }
            }

            while (operStack.Count > 0)
                output += operStack.Pop() + " ";

            return output;
        }
    }
}
