// TestL4.cpp : Defines the entry point for the console application.

#include "stdafx.h"
#include "Word.h"
#include "CrossWordsField.h"
#include <windows.h>

static int count;

std::map<int, std::vector<Word>> combinations;

int main()
{
	setlocale(LC_ALL, "ru-Ru"); // TODO: check

	auto wordsList = std::vector <Word>();
	UINT cp;
	
	std::string mystr;
	for (int i = 0; i < 7; i++)
	{
		std::cout << "������� �����: ";
		cp = GetConsoleCP();
		SetConsoleCP(1251);
		getline(std::cin, mystr);


		wordsList.push_back(Word(i+1, mystr));
		SetConsoleCP(cp);
	}

	std::cout << "�����: " << std::endl;

	
	SetConsoleCP(1251);
	SetConsoleOutputCP(1251);

	for (int i = 0; i < 7; i++)
	{
		std::cout << wordsList.at(i).Number << ". : " << wordsList.at(i).Text << std::endl;
	}

	
	/*1. : ���
	2. : ��������
	3. : �����
	4. : �����
	5. : �������
	6. : ��������
	7. : ������*/

	// work combo
	/*wordsList.push_back(Word(1, "��������"));
	wordsList.push_back(Word(2, "�����"));
	wordsList.push_back(Word(3, "������"));
	wordsList.push_back(Word(4, "������"));
	wordsList.push_back(Word(5, "������"));
	wordsList.push_back(Word(6, "�������"));
	wordsList.push_back(Word(7, "��������"));*/
	

	// not worked
	//wordsList.push_back(Word(1, "���"));
	//wordsList.push_back(Word(2, "��������"));
	//wordsList.push_back(Word(3, "�����"));
	//wordsList.push_back(Word(4, "�����"));
	//wordsList.push_back(Word(5, "�������"));
	//wordsList.push_back(Word(6, "��������"));
	//wordsList.push_back(Word(7, "������"));

	/*wordsList.push_back(Word(1, "���"));
	wordsList.push_back(Word(2, "���"));
	wordsList.push_back(Word(3, "���"));
	wordsList.push_back(Word(4, "���"));
	wordsList.push_back(Word(5, "���"));
	wordsList.push_back(Word(6, "���"));
	wordsList.push_back(Word(7, "���"));*/


	CrossWordsField field = CrossWordsField(50, 50, 7);

	std::vector<Word> words = std::vector<Word>(wordsList);

	bool res =  field.ProcessWordsBackTrack (words);



	if (res == false)
	{
		std::cout << "cant" << std::endl;
	}
	else
	{
		field.DrawMatrix();
	}

	system("pause");
	return 0;
}