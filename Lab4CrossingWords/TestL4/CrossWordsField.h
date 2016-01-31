#pragma once
#include "PlacedWord.h"
#include "InternalChar.h"
#include <stack>
#include <vector>
#include "Cross.h"


class CanPlace;
class CrossPointInfo;
class InternalChar;

class CrossWordsField
{
private:
	int _wordsToInsert;
	const char DefSymbol = '*';

	int DimX;
	int DimY;
	int swapCount;
	InternalChar InternalMatrix[100][100] ;

	std::stack<PlacedWord> PlacedWords;

public:
	CrossWordsField(int dimX, int dimY, int wordsToInsert);
	void DrawMatrix();
	bool ProcessWordsBackTrack(std::vector<Word> words);

	void InsertWord(Word word, PlaceDirection direction, Point startPoint);

	std::vector<Cross> CrossPositions(Word word);
	PlacedWord RemoveLastWord();

	CanPlace CanPlaceToCrossPos(Cross crossPos, Word word);
	void InsertWord(CanPlace place, Word word);
private:
	void InsertWordHorisontal(Word word, Point point);
	void InsertWordVertical(Word word, Point point);
};
