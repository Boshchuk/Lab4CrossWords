// TestL4.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"



int    A[] = { 1, 2, 3, 4, 5 }; //��, � �������...
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
void Perestanovki(int *M, int n, int N)//M - ������, n - ����� �������������� ���������, 
									   //N - �������� ������ �������
{
	if (n == 1)
		Obrabotka(M, N); //���� ������ ������������
	else
	{
		for (int i = 0; i < n; i++)
		{
			swap(M[i], M[n - 1]); //������ ��������� ������� � ������,
								  //� ��� ����� � � ����� �����.
			Perestanovki(M, n - 1, N); //��������� �������, ��� n-1 ���������
			swap(M[i], M[n - 1]); //���������� - � ������. ���� ������� ������ � �������
								  //��������� ��� ���������� ������ ���������
		}
	}
}
int main()
{
	Perestanovki(A, 7, 7);
	system("pause");
	return 0;
}