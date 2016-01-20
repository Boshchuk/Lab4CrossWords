#pragma once
#include <string>

class Word
{
public:
	int Number;
	std::string Text;

	Word(int n, std::string str);
	~Word();
};
