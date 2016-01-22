#include "stdafx.h"
#include "PlacedWord.h"

PlacedWord::PlacedWord()
{
}

PlacedWord::PlacedWord(Word word, PlaceDirection placeDirection, Point startPoint)
{
	_word = word;
	_placeDirection = placeDirection;
	StartPoint = startPoint;
}
