// TestL4.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"



int    A[] = { 1, 2, 3, 4, 5 }; //ну, к примеру...
void Obrabotka(int *M, int n)
{
	for (int i = 0; i < n; i++)
		std::cout << M[i] << " ";
	std::cout << std::endl;
		
	
}
void swap(int &a, int &b)
{
	int    temp = a;
	a = b;
	b = temp;
}
void Perestanovki(int *M, int n, int N)//M - массив, n - число переставляемых элементов, 
									   //N - реальный размер массива
{
	if (n == 1)
		Obrabotka(M, N); //если нечего переставлять
	else
	{
		for (int i = 0; i < n; i++)
		{
			swap(M[i], M[n - 1]); //меняем последний элемент с каждым,
								  //в том числе и с самим собой.
			Perestanovki(M, n - 1, N); //запускаем функцию, для n-1 элементов
			swap(M[i], M[n - 1]); //поигрались - и хватит. Надо вернуть массив в прежнее
								  //состояние для следующего обмена элементов
		}
	}
}
int main()
{
	Perestanovki(A, 7, 7);
	system("pause");
	return 0;
}