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
		std::cout << "¬ведите слово: ";
		cp = GetConsoleCP();
		SetConsoleCP(1251);
		getline(std::cin, mystr);


		wordsList.push_back(Word(i+1, mystr));
		SetConsoleCP(cp);
	}

	std::cout << "—лова: " << std::endl;

	
	SetConsoleCP(1251);
	SetConsoleOutputCP(1251);

	for (int i = 0; i < 7; i++)
	{
		std::cout << wordsList.at(i).Number << ". : " << wordsList.at(i).Text << std::endl;
	}

	
	/*1. : худ
	2. : персонаж
	3. : слово
	4. : кисть
	5. : печенка
	6. : ловкость
	7. : качели*/

	// work combo
	/*wordsList.push_back(Word(1, "художник"));
	wordsList.push_back(Word(2, "кисть"));
	wordsList.push_back(Word(3, "мастер"));
	wordsList.push_back(Word(4, "студи€"));
	wordsList.push_back(Word(5, "пейзаж"));
	wordsList.push_back(Word(6, "палитра"));
	wordsList.push_back(Word(7, "мольберт"));*/
	

	// not worked
	//wordsList.push_back(Word(1, "худ"));
	//wordsList.push_back(Word(2, "персонаж"));
	//wordsList.push_back(Word(3, "слово"));
	//wordsList.push_back(Word(4, "кисть"));
	//wordsList.push_back(Word(5, "печенка"));
	//wordsList.push_back(Word(6, "ловкость"));
	//wordsList.push_back(Word(7, "качели"));

	/*wordsList.push_back(Word(1, "сон"));
	wordsList.push_back(Word(2, "сон"));
	wordsList.push_back(Word(3, "сон"));
	wordsList.push_back(Word(4, "сон"));
	wordsList.push_back(Word(5, "сон"));
	wordsList.push_back(Word(6, "сон"));
	wordsList.push_back(Word(7, "сон"));*/


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