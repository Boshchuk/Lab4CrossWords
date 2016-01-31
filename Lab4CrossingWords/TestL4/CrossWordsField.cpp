#include "stdafx.h"
#include "CrossWordsField.h"
#include <iostream>
#include "CrossPointInfo.h"
#include <list>
#include "CanPlace.h"


CrossWordsField::CrossWordsField(int dimX, int dimY, int wordsToInsert)
{
	PlacedWords = std::stack<PlacedWord>();
	_wordsToInsert = wordsToInsert;
	DimX = dimX;
	DimY = dimY;
	//InternalMatrix = InternalChar[DimX][ DimY];
	for (int i = 0; i < DimX; i++)
	{
		for (int j = 0; j < DimY; j++)
		{
			InternalMatrix[i][j] = InternalChar(0, DefSymbol);
		}
	}
}


void CrossWordsField::DrawMatrix()
{
	//Console.ForegroundColor = ConsoleColor.DarkGray;
	for (int i = 0; i < DimX; i++)
	{

		for (int j = 0; j < DimY; j++)
		{
			InternalChar internalChar = InternalMatrix[i][j];

			if (internalChar.Symbol != DefSymbol)
			{
				//Console.ForegroundColor = ConsoleColor.Green;

			}
			if (internalChar.PlacedCount > 1)
			{
				//Console.ForegroundColor = ConsoleColor.Red;
			}

			//Console.Write("{0}", internalChar.Symbol);
			std::cout << internalChar.Symbol;


			//Console.ForegroundColor = ConsoleColor.DarkGray;
		}
		std::cout << std::endl;
	}
}

bool CrossWordsField::ProcessWordsBackTrack(std::vector<Word> words)
{
	if (words.size() == 0)
	{
		if (PlacedWords.size() == _wordsToInsert)
		{
			std::cout << "all placed" << std::endl;

			return true;
		}
		else
		{
			return false;
		}
	}

	auto wordToPlace = words.front();

	if (PlacedWords.size() == 0)
	{
		Point p = Point();
		p.X = DimX / 2;
		p.Y = DimY / 2;
		InsertWord(wordToPlace, Horisontal, p);

		std::vector<Word>::iterator minusOne = words.erase(words.begin()); 
		return ProcessWordsBackTrack(words);
	}
	else
	{
		auto next = words.front();
		auto avalibelePos = CrossPositions(next);

		if (avalibelePos.size() == 0)
		{
			if (swapCount >= _wordsToInsert + 1)
			{
				return false;
			}

			swapCount++;

			auto inseted = RemoveLastWord()._word;

			words.push_back(inseted);

			return ProcessWordsBackTrack(words);
		}
		else
		{

			// try insert
			auto result = false;


			while (result == false)
			{
				if (avalibelePos.size() == 0)
				{
					break;
				}
				auto f = avalibelePos.front();

				//if (f == nullptr avalibelePos.end().)
				// TODO: need to be implimented?????
				//{
					//break;
				//}

				auto cPos = CanPlaceToCrossPos(f, next);

				if (cPos.Posible())
				{
					InsertWord(cPos, next);

					words.erase(words.begin()); //Remove(next);

					result = true;
				}
				else
				{
					avalibelePos.erase(avalibelePos.begin());// Remove(f);
					result = false;
				}

			}

			if (!result)
			{
				RemoveLastWord();
				auto it = words.erase(words.begin()); //Remove(next);
				
				words.push_back(next);
			}
		}

		return ProcessWordsBackTrack(words);
	}
}

void CrossWordsField::InsertWord(Word word, PlaceDirection direction, Point startPoint)
{
	PlacedWord *item = new PlacedWord(word, direction, startPoint);

	switch (item->_placeDirection)
	{

	case Horisontal:
	{
		InsertWordHorisontal(word, item->StartPoint);
		break;
	}
	case Vertiacal:
	{
		InsertWordVertical(word, item->StartPoint);
		break;
	}
	}

	PlacedWords.push (*item);
}

std::vector<Cross> CrossWordsField::CrossPositions(Word word)
{
	bool result = false;

	bool pointToInsertExist = false;

	auto avalible = std::vector<Cross>();

	for (int i = 0; i < DimX; i++)
	{
		for (int j = 0; j < DimY; j++)
		{

			for (int iter = 0; iter < word.Text.length(); iter++)
			{
				auto c = word.Text[iter];
				if (c == InternalMatrix[i][ j].Symbol && InternalMatrix[i][ j].PlacedCount <= 1)
				{
					pointToInsertExist = true;
					avalible.push_back(Cross(Point(i, j), c));
				}
			}
		}
	}

	return avalible;
}

PlacedWord CrossWordsField::RemoveLastWord()
{
	PlacedWord placedWord = PlacedWords.top();
	PlacedWords.pop();

	switch (placedWord._placeDirection)
	{
	case Horisontal:
	{
		for (auto index = 0; index < placedWord._word.Text.length(); index++)
		{
			InternalMatrix[placedWord.StartPoint.X + index][ placedWord.StartPoint.Y].PlacedCount--;

			if (InternalMatrix[placedWord.StartPoint.X + index][ placedWord.StartPoint.Y].PlacedCount == 0)
			{
				InternalMatrix[placedWord.StartPoint.X + index][ placedWord.StartPoint.Y].Symbol = DefSymbol;
			}
		}

		break;
	}
	case Vertiacal:
	{
		for (auto index = 0; index < placedWord._word.Text.length(); index++)
		{
			InternalMatrix[placedWord.StartPoint.X][ placedWord.StartPoint.Y + index].PlacedCount--;

			if (InternalMatrix[placedWord.StartPoint.X][ placedWord.StartPoint.Y + index].PlacedCount == 0)
			{
				InternalMatrix[placedWord.StartPoint.X][ placedWord.StartPoint.Y + index].Symbol = DefSymbol;
			}
		}

		break;
	}
	}
	return placedWord;
}


CanPlace CrossWordsField::CanPlaceToCrossPos(Cross crossPos, Word word)
{
	Point shitTop = Point(crossPos._point.X, crossPos._point.Y - word.Text.find(crossPos._symbol,0));
	Point shitLeft = Point(crossPos._point.X - word.Text.find(crossPos._symbol, 0), crossPos._point.Y);

	auto canTop = true;
	auto canLeft = true;

	auto topCrossingCount = 0;
	auto leftCrossingCount = 0;

	// try top
	for (int index = 0; index < word.Text.length(); index++)
	{
		auto c = word.Text.at(index);
		auto internalChar = InternalMatrix[shitTop.X][ shitTop.Y + index];

		if ((internalChar.Symbol == DefSymbol) || (internalChar.Symbol == c))
		{
			if (internalChar.Symbol == c)
			{
				topCrossingCount++;
			}
			if (topCrossingCount > 1)
			{
				canTop = false;
				break;
			}

			continue;
		}
		else
		{
			canTop = false;
			break;
		}
	}

	for (int index = 0; index < word.Text.length(); index++)
	{
		auto c = word.Text.at(index);
		auto internalChar = InternalMatrix[shitLeft.X + index][ shitLeft.Y];

		if ((internalChar.Symbol == DefSymbol) || (internalChar.Symbol == c))
		{
			if (internalChar.Symbol == c)
			{
				leftCrossingCount++;
			}
			if (leftCrossingCount > 1)
			{
				canLeft = false;
				break;
			}
			continue;
		}
		else
		{
			canLeft = false;
			break;
		}
	}
	return CanPlace(canTop, shitTop, canLeft, shitLeft);
}

void CrossWordsField::InsertWord(CanPlace place, Word word)
{
	if (place.CanTop)
	{
		InsertWord(word, Vertiacal, place.TopStart);
	}
	if (place.CanLeft)
	{
		InsertWord(word, Horisontal, place.LeftStart);
	}
};

void CrossWordsField::InsertWordHorisontal(Word word, Point point)
{
	for (int index = 0; index < word.Text.length(); index++)
	{
		std::_Simple_types<char>::value_type c = word.Text[index];
		InternalMatrix[point.X + index][ point.Y].Symbol = c;
		InternalMatrix[point.X + index][ point.Y].PlacedCount++;
	}
}

void CrossWordsField::InsertWordVertical(Word word, Point point)
{
	for (int index = 0; index < word.Text.length(); index++)
	{
		std::_Simple_types<char>::value_type c = word.Text[index];
		InternalMatrix[point.X][ point.Y + index].Symbol = c;
		InternalMatrix[point.X][ point.Y + index].PlacedCount++;
	}
}
