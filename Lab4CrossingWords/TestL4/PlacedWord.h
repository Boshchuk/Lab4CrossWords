#pragma once
#include "Word.h"
#include "Point.h"

enum PlaceDirection;

class PlacedWord
{
public:
	Word _word;
	PlaceDirection _placeDirection;
	Point StartPoint;

public:
	PlacedWord();
	PlacedWord(Word word, PlaceDirection placeDirection, Point startPoint);
	
};
