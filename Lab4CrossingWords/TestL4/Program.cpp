// TestL4.cpp : Defines the entry point for the console application.

#include "stdafx.h"
#include "Word.h"
#include "CrossWordsField.h"
#include "CrossPointInfo.h"
#include <windows.h>

static int count;

std::map<int, std::vector<Word>> combinations;

void Obrabotka(std::vector<Word> words)
{
	combinations.insert(std::pair<int, std::vector<Word>>(count, words) );
	
	// TODO debug display

	count++;
	
}

bool IsNeo(Word w, int key) {
	return (w.Number == key);
}

std::vector<Word> swap(int keyA, int keyB, std::vector<Word> words)
{
//http://stackoverflow.com/questions/6679096/using-find-if-on-a-vector-of-object

//http://stackoverflow.com/questions/26903602/an-enclosing-function-local-variable-cannot-be-referenced-in-a-lambda-body-unles
	
	std::_Vector_iterator<std::_Vector_val<std::_Simple_types<Word>>> objA = std::find_if(words.begin(),
		words.end(),
		[&](const Word word) 
			{
				return	word.Number == keyA;
			}
		);

	Word t = Word(objA->Number, objA->Text);
	
	std::_Vector_iterator<std::_Vector_val<std::_Simple_types<Word>>> b = std::find_if(words.begin(),
		words.end(),
		[&](const Word word)
	{
		return	word.Number == keyB;
	}
	);

	std::_Vector_iterator<std::_Vector_val<std::_Simple_types<Word>>> a = std::find_if(words.begin(),
		words.end(),
		[&](const Word word)
	{
		return	word.Number == keyA;
	}
	);

	(*a).Text = (*b).Text;
	(*a).Number = (*b).Number;
	
	(*b).Text = t.Text;
	(*b).Number = t.Number;

	return words;
}
void Perestanovki(std::vector<Word> words, int n)//M - массив, n - число переставляемых элементов, 
									   //N - реальный размер массива
{
	if (n == 1)
	{
		Obrabotka(words); //если нечего переставлять
	}
	else
	{
		for (int i = 0; i < n; i++)
		{
		 	words = swap(words[i].Number, words[n - 1].Number, words); //меняем последний элемент с каждым,
								  //в том числе и с самим собой.
			Perestanovki(words, n - 1); //запускаем функцию, для n-1 элементов
		   	words = swap(words[i].Number, words[n - 1].Number, words); //поигрались - и хватит. Надо вернуть массив в прежнее
								  //состояние для следующего обмена элементов
		}
	}
}
static std::vector<CrossPointInfo> GetCrossInfo(Word word1, Word word2)
{
	auto result = std::vector<CrossPointInfo>();
	for (int i = 0; i < word1.Text.length(); i++)
	{
		auto w1 = word1.Text[i];
		for (int index = 0; index < word2.Text.length(); index++)
		{
			char w2 = word2.Text[index];
			if (w1 == w2)
			{
				auto info = CrossPointInfo(word1.Number, word2.Number, w1, i, index);

				result.push_back(info);
			}
		}
	}
	return result;
}


static std::vector<CrossPointInfo> GetCrossPointInfos(std::vector<Word> words)
{
	auto result = std::vector<CrossPointInfo>();

	for (int index = 0; index < words.size() - 1; index++)
	{
		for (int seconwIndex = index + 1; seconwIndex < words.size(); seconwIndex++)
		{
			auto inf = GetCrossInfo(words[index], words[seconwIndex]);

			for(auto Iter = inf.begin(); Iter != inf.end(); ++Iter)
			{
				result.push_back(*Iter);
			}
		}
	}

	return result;
}
static std::vector<CrossPointInfo> GetRelatedForWord(int wordKey,std::vector<CrossPointInfo> crossPointInfos)
{
	int differecntCross = 0;

	std::vector<CrossPointInfo> infos = std::vector<CrossPointInfo>();

	for (auto iter = crossPointInfos.begin(); iter != crossPointInfos.end(); ++iter)
	{
		if (iter->Word1Number == wordKey || iter->Word2Number ==wordKey)
		{
			if (infos.size() == 0)
			{
				infos.push_back(*iter);
			}
			auto needAdd = true;

			for (auto it = infos.begin(); it != infos.end(); ++it)
			{
				if (iter->IsSameWordsCross(*it))
				{
					needAdd = false;
					break;
				}
			}

			if (needAdd)
			{
				infos.push_back (*iter);
			}
		}
	}
	
	return infos;
}

int main()
{
	setlocale(LC_ALL, "Russian"); // TODO: check

	auto wordsList = std::vector <Word>();
	SetConsoleCP(1251);
	SetConsoleOutputCP(1251);

	

	std::string mystr;
	for (int i = 0; i < 7; i++)
	{
		std::cout << "Введите слово: ";
		getline(std::cin, mystr);

		wordsList.push_back(Word(i+1, mystr));
	}

	std::cout << "Слова: " << std::endl;

	for (int i = 0; i < 7; i++)
	{
		std::cout << wordsList.at(i).Number << ". : " << wordsList.at(i).Text << std::endl;
	}

	/*
	1. : худ
	2. : персонаж
	3. : слово
	4. : кисть
	5. : печенка
	6. : ловкость
	7. : качели

	// work combo
	wordsList.push_back(Word(1, "художник"));
	wordsList.push_back(Word(2, "кисть"));
	wordsList.push_back(Word(3, "мастер"));
	wordsList.push_back(Word(4, "студия"));
	wordsList.push_back(Word(5, "пейзаж"));
	wordsList.push_back(Word(6, "палитра"));
	wordsList.push_back(Word(7, "мольберт"));
	*/

	// not worked
	/*wordsList.push_back(Word(1, "худ"));
	wordsList.push_back(Word(2, "персонаж"));
	wordsList.push_back(Word(3, "слово"));
	wordsList.push_back(Word(4, "кисть"));
	wordsList.push_back(Word(5, "печенка"));
	wordsList.push_back(Word(6, "ловкость"));
	wordsList.push_back(Word(7, "качели"));*/

	

	Perestanovki(wordsList, 7);

	std::cout << count << std::endl;

	std::vector<CrossPointInfo> crossPointInfos = GetCrossPointInfos(wordsList);

	std::map<int, std::vector<CrossPointInfo>> dick = std::map<int, std::vector<CrossPointInfo>>();

	for (int i = 1; i < 8; i ++)
	{
		std::vector<CrossPointInfo> res = GetRelatedForWord(i, crossPointInfos);

		dick.insert(std::pair<int, std::vector<CrossPointInfo>>(i, res));
	}
	
	CrossWordsField field = CrossWordsField(50, 50, 7);

	std::vector<Word> words = std::vector<Word>(wordsList);
	std::map<int, std::vector<CrossPointInfo>> d = std::map<int, std::vector<CrossPointInfo>>(dick);


	bool res = false;
	int attemts = count - 1;
	while (!res && attemts != 0)
	{
		res = field.ProcessWords(words, d);

		if (res == false)
		{
		    words = combinations[attemts];

			d = std::map<int, std::vector<CrossPointInfo>>(dick);

			field.ClearInternal();
			attemts--;
		}

	}

	if (attemts == 0 && res == false)
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