#pragma once
#include "PlacedWord.h"
#include "InternalChar.h"
#include <stack>

class InternalChar;

class CrossWordsField
{
private:
	int _wordsToInsert;
	const char DefSymbol = '*';

	int DimX;
	int DimY;

	InternalChar InternalMatrix[100][100] ;

	std::stack<PlacedWord> PlacedWords;

public:
	CrossWordsField(int dimX, int dimY, int wordsToInsert);
};
