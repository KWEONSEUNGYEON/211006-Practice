﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCSharp_02
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("배열의 길이 정해주세요.");
            int arrsize = int.Parse(Console.ReadLine());

            int[] numbers = new int[arrsize];
            for(int i =0; i < arrsize; i++)
            {
                Console.WriteLine($"[{i}]번째 값 입력"); //[0]번째값입력 ~ [arrsize-1]번째 값 입력
                numbers[i] = int.Parse(Console.ReadLine());
            }
            Array.Sort(numbers); //정렬(크기순 정렬)
            // Array.Reverse(numbers); //내가 만든 배열의 인덱스를 거꾸로 뒤집는 거(크기순 정렬 아님)
            // 만약 내림차순 정렬하고 싶으면 sort를 통해서 오름차순 정렬 후, 그걸 reverse

            foreach(var item in numbers)
            {
                Console.WriteLine(item);
            }

            Array.Reverse(numbers); //오름차순정렬을 하기 위해서, 일부러 반대로 뒤집어봄..

            for(int i = 0; i < arrsize; i++) //버블소트
            {
                for(int j = 0; j <  arrsize-1;  j++)
                {
                    if(numbers[j] > numbers[j+1])
                    {
                        int temp = numbers[j];
                        numbers[j] = numbers[j + 1];
                        numbers[j + 1] = temp;
                    }
                }
            }

            foreach (var item in numbers)
            {
                Console.WriteLine(item);
            }

            Array.Reverse(numbers);

            int minth; //최솟값의 위치
            for(int i = 0; i<arrsize-1; i++)
            {
                minth = i; 
                for(int j = i+1; j < arrsize; j++)
                {
                    if(numbers[i] < numbers[minth])
                        minth = j;
                }
                int temp = numbers[minth];
                numbers[minth] = numbers[i];
                numbers[i] = temp; 
            }
            foreach (var item in numbers)
            {
                Console.WriteLine(item);
            }

        }
    }
}
