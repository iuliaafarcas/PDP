using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace Task__product_of_2_big_numbers
{
    class Program
    {

        static List<long> addNumbers(List<long> firstNumber, List<long> secondNumber)
        {
            List<long> result = new List<long>();
            int i = 0;
            long remainder = 0;
            for (i = 0; i < firstNumber.Count && i < secondNumber.Count; i++)
            {
                long currentResult = firstNumber[firstNumber.Count - 1 - i] + secondNumber[secondNumber.Count - 1 - i] + remainder;
                remainder = currentResult / 10;
                result.Insert(0, currentResult % 10);
            }
            for (; i < firstNumber.Count; i++)
            {
                long currentResult = firstNumber[firstNumber.Count - 1 - i] + remainder;
                remainder = currentResult / 10;
                result.Insert(0, currentResult % 10);
            }
            if (remainder != 0)
            {
                result.Insert(0, remainder);
            }
            return result;
        }

        static void multiplyWithTenToThePower(List<long> number, int power)
        {
            for (int i = 0; i < power; i++)
            {
                number.Add(0);
            }
        }

        static List<long> multiplyWithDigit(List<long> number, long digit)
        {
            List<long> result = new List<long>();
            long remainder = 0;

            for (int i = number.Count - 1; i>=0; i--)
            {
                long currentResult = number[i] * digit + remainder;
                remainder = currentResult / 10;
                result.Insert(0, currentResult % 10);
            }
            if (remainder != 0)
            {
                result.Insert(0, remainder);
            }
            return result;
        }

        static List<long> bigNumbersProduct(List<long> firstNumber, List<long> secondNumber, int start, int end)
        {
            List<long> result = new List<long>();
            for (int i = start; i < end; i++)
            {
                List<long> currentResult = multiplyWithDigit(firstNumber, secondNumber[i]);
                multiplyWithTenToThePower(currentResult, secondNumber.Count - 1 - i);
                if (result.Count < currentResult.Count)
                {
                    result = addNumbers(currentResult, result);
                } else
                {
                    result = addNumbers(result, currentResult);
                }
            }
            return result;

        }
        static void Main(string[] args)
        {
            long n1=45678;
            long n2=4321;
            long aux;
            List<long> number2 = new List<long>();
            List<long> number1 = new List<long>();
            int noThreads = 3;
            List<long> prod = new List<long>();

            if (n1 < n2) { aux = n1; n1 = n2; n2 = aux; }
            while (n2 != 0)
            {
                number2.Insert(0, n2 % 10);
                n2 = n2 / 10;
            }
            while (n1 != 0)
            {
                number1.Insert(0, n1% 10);
                n1 = n1 / 10;
            }

            List<Task<List<long>>> taskList = new List<Task<List<long>>>();

            int digitsPerTask = number2.Count / noThreads;
            for (int i = 0; i < noThreads; i++)
            {
                int start = i * digitsPerTask;
                int end = i == noThreads - 1 ? number2.Count : start + digitsPerTask;
                Task<List<long>> task = Task.Factory.StartNew(() => bigNumbersProduct(number1, number2, start, end));
                taskList.Add(task);
            }

            for (int i = 0; i < noThreads; i++)
            {
                List<long> partialResult = taskList[i].Result;
                if (partialResult.Count > prod.Count)
                {
                    prod = addNumbers(partialResult, prod);
                } else
                {
                    prod = addNumbers(prod, partialResult);
                }
            }
           
            Console.WriteLine(String.Concat(prod));

        }
    }
}
