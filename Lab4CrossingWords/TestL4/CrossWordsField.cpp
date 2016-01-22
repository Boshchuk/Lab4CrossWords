#include "stdafx.h"
#include "CrossWordsField.h"
#include <iostream>
#include "CrossPointInfo.h"
#include <list>


CrossWordsField::CrossWordsField(int dimX, int dimY, int wordsToInsert)
{
	PlacedWords = std::list<PlacedWord>();
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

void CrossWordsField::ClearInternal()
{
	PlacedWords.empty();

	for (auto i = 0; i < DimX; i++)
	{
		for (auto j = 0; j < DimY; j++)
		{
			InternalMatrix[i][j].Symbol = DefSymbol;
			InternalMatrix[i][j].PlacedCount = 0;
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
			auto internalChar = InternalMatrix[i][j];

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

Point CrossWordsField::CulcStartPoint(PlacedWord placedWord, CrossPointInfo info, PlaceDirection direction, Word word)
{
	auto pointToPlace = Point();

	auto crossPoint = Point();

	switch (placedWord._placeDirection)
	{
	case Horisontal:
	{
		crossPoint.X = placedWord.StartPoint.X + info.GetThisCross(placedWord._word.Number);
		crossPoint.Y = placedWord.StartPoint.Y;
		break;
	}
	case Vertiacal:
	{
		crossPoint.X = placedWord.StartPoint.X;
		crossPoint.Y = placedWord.StartPoint.Y + info.GetThisCross(placedWord._word.Number);
		break;
	}
	}

	switch (direction)
	{
	case Horisontal:
	{
		pointToPlace.X = crossPoint.X - info.GetThisCross(word.Number);
		pointToPlace.Y = crossPoint.Y;
		break;
	}
	case Vertiacal:
	{
		pointToPlace.X = crossPoint.X;
		pointToPlace.Y = crossPoint.Y - info.GetThisCross(word.Number);
		break;
	}
	}
	return pointToPlace;
}

bool CrossWordsField::CanPlaceWord(PlacedWord placedWord, CrossPointInfo info, PlaceDirection direction, Word word)
{
	auto pointToPlace = CulcStartPoint(placedWord, info, direction, word);


	auto result = true;
	switch (direction)
	{
	case Horisontal:
	{
		for (int index = 0; index < word.Text.length(); index++)
		{
			auto c = word.Text[index];
			auto internalChar = InternalMatrix[pointToPlace.X + index][ pointToPlace.Y];

			if ((internalChar.Symbol == DefSymbol) || (internalChar.Symbol == c))
			{
				continue;
			}
			else
			{
				result = false;
			}
		}
		break;
	}

	case Vertiacal:
	{
		for (int index = 0; index < word.Text.length(); index++)
		{
			auto c = word.Text[index];
			auto internalChar = InternalMatrix[pointToPlace.X][ pointToPlace.Y + index];

			if ((internalChar.Symbol == DefSymbol) || (internalChar.Symbol == c))
			{
				continue;
			}
			else
			{
				result = false;
			}
		}
		break;

	}
	}

	return result;
}

bool CrossWordsField::ProcessWords(std::vector<Word> words, std::map<int, std::vector<CrossPointInfo>> dictionary)
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

	auto wordToPlace = words.begin();

	if (PlacedWords.size() == 0)
	{
		auto p = Point();
		p.X = DimX / 2;
		p.Y = DimY / 2;
		InsertWord((*wordToPlace), Horisontal, p);

		auto minusOne = words.erase(words.begin()); // TODO check
		return ProcessWords(words, dictionary);
	}
	else
	{
		auto canPlace = false;

		auto relatedInfos = dictionary[wordToPlace->Number];

		auto attemptPlace = std::find_if(relatedInfos.begin(),
			relatedInfos.end(),
			[&](const CrossPointInfo info)
		{
			return info.Word1Number == wordToPlace->Number || info.Word2Number == wordToPlace->Number;
		});
			
			

		if (attemptPlace != relatedInfos.end())
		{
			auto idOfWordToConnect = attemptPlace->GetPairedCrossNum(wordToPlace->Number);

			auto placedToConnect = std::find_if(PlacedWords.begin(),
				PlacedWords.end(),
				[&](const PlacedWord word)
			{
				return word._word.Number == idOfWordToConnect;
			});
				
				
				//PlacedWords. FirstOrDefault(x = > x.Word.Number == idOfWordToConnect);

			if (placedToConnect != PlacedWords.end())
			{
				PlaceDirection dir = Horisontal;

				switch (placedToConnect->_placeDirection)
				{
				case Horisontal:
				{
					dir = Vertiacal;
					break;
				}
				case Vertiacal:
				{
					dir = Horisontal;
					break;
				}
				}
				if (CanPlaceWord((*placedToConnect), (*attemptPlace), dir, (*wordToPlace)))
				{
					canPlace = true;
					auto startPoint = CulcStartPoint((*placedToConnect), (*attemptPlace), dir, (*wordToPlace));
					InsertWord((*wordToPlace), dir, startPoint);

					auto minusOne = words.erase(words.begin());
				}
			}

		}
		else
		{
			return false;
		}

		if (!canPlace)
		{
			dictionary.erase(wordToPlace->Number);//   [wordToPlace->Number].erase(attemptPlace);// Remove(attemptPlace);
			auto itr =words.erase(wordToPlace);
			words.push_back(Word((*itr).Number,(*itr).Text));
		}
		return ProcessWords(words, dictionary);
	}
}

void CrossWordsField::InsertWord(Word word, PlaceDirection direction, Point startPoint)
{
	auto item = new PlacedWord(word, direction, startPoint);

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

	PlacedWords.push_back (*item);
}

void CrossWordsField::InsertWordHorisontal(Word word, Point point)
{
	for (int index = 0; index < word.Text.length(); index++)
	{
		auto c = word.Text[index];
		InternalMatrix[point.X + index][ point.Y].Symbol = c;
		InternalMatrix[point.X + index][ point.Y].PlacedCount++;
	}
}

void CrossWordsField::InsertWordVertical(Word word, Point point)
{
	for (int index = 0; index < word.Text.length(); index++)
	{
		auto c = word.Text[index];
		InternalMatrix[point.X][ point.Y + index].Symbol = c;
		InternalMatrix[point.X][ point.Y + index].PlacedCount++;
	}
}
