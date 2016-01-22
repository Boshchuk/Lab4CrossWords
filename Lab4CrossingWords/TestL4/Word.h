#pragma once
#include <string>

class Word
{
public:
	int Number;
	std::string Text;

public:
	Word();
	Word(int n, std::string str);
	~Word();
};
