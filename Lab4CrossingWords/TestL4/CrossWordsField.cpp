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

void CrossWordsField::ClearInternal()
{
	PlacedWords.empty();

	for (int i = 0; i < DimX; i++)
	{
		for (int j = 0; j < DimY; j++)
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

Point CrossWordsField::CulcStartPoint(PlacedWord placedWord, CrossPointInfo info, PlaceDirection direction, Word word)
{
	Point pointToPlace= Point();

	Point crossPoint = Point();

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
	Point pointToPlace = CulcStartPoint(placedWord, info, direction, word);
	
	bool eculimint = false;

	int ecuCount = 0;
	if (placedWord._word.Text == word.Text)
	{
		eculimint = true;
	}




	bool result = true;
	switch (direction)
	{
	case Horisontal:
	{
		for (int index = 0; index < word.Text.length(); index++)
		{
			std::_Simple_types<char>::value_type c = word.Text[index];
			InternalChar internalChar = InternalMatrix[pointToPlace.X + index][ pointToPlace.Y];

			if ((internalChar.Symbol == DefSymbol) || (internalChar.Symbol == c))
			{
				if (internalChar.Symbol == c)
				{
					ecuCount++;
				}
				if (eculimint && ecuCount >1)
				{
					result = false;
				}
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
			std::_Simple_types<char>::value_type c = word.Text[index];
			InternalChar internalChar = InternalMatrix[pointToPlace.X][ pointToPlace.Y + index];

			if ((internalChar.Symbol == DefSymbol) || (internalChar.Symbol == c))
			{
				if (internalChar.Symbol == c)
				{
					ecuCount++;
				}
				
				if (eculimint && ecuCount >1)
				{
					result = false;
				}
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

bool operator==(const CrossPointInfo lhs, const CrossPointInfo rhs)
{
	return lhs.Word1Number == rhs.Word1Number && lhs.Word2Number == rhs.Word2Number;
}

bool operator!=(const CrossPointInfo lhs, const CrossPointInfo rhs)
{
	return !(lhs == rhs);
}

//bool CrossWordsField::ProcessWords(std::vector<Word> words, std::map<int, std::vector<CrossPointInfo>> dictionary)
//{
//	if (words.size() == 0)
//	{
//		if (PlacedWords.size() == _wordsToInsert)
//		{
//			std::cout << "all placed" << std::endl;
//
//			return true;
//		}
//		else
//		{
//			return false;
//		}
//	}
//
//	std::_Simple_types<Word>::value_type wordToPlace = words.front();
//
//	if (PlacedWords.size() == 0)
//	{
//		Point p = Point();
//		p.X = DimX / 2;
//		p.Y = DimY / 2;
//		InsertWord(wordToPlace, Horisontal, p);
//
//		std::vector<Word>::iterator minusOne = words.erase(words.begin()); // TODO check
//		return ProcessWords(words, dictionary);
//	}
//	else
//	{
//		bool canPlace = false;
//
//		std::map<int, std::vector<CrossPointInfo>>::mapped_type relatedInfos = dictionary.at(wordToPlace.Number);
//
//		std::_Vector_iterator<std::_Vector_val<std::_Simple_types<CrossPointInfo>>> attemptPlace = std::find_if(relatedInfos.begin(), relatedInfos.end(),
//			[&](const CrossPointInfo info)
//		{
//			return info.Word1Number == wordToPlace.Number || info.Word2Number == wordToPlace.Number;
//		});
//			
//
//		if ( attemptPlace != relatedInfos.end())
//		{
//			int idOfWordToConnect = attemptPlace->GetPairedCrossNum(wordToPlace.Number);
//
//			std::_List_iterator<std::_List_val<std::_List_simple_types<PlacedWord>>> placedToConnect = std::find_if(PlacedWords.begin(),
//				PlacedWords.end(),
//				[&](const PlacedWord word)
//			{
//				return word._word.Number == idOfWordToConnect;
//			});
//
//			if (placedToConnect != PlacedWords.)
//			{
//				PlaceDirection dir = Horisontal;
//
//				switch (placedToConnect->_placeDirection)
//				{
//				case Horisontal:
//				{
//					dir = Vertiacal;
//					break;
//				}
//				case Vertiacal:
//				{
//					dir = Horisontal;
//					break;
//				}
//				}
//				if (CanPlaceWord((*placedToConnect), (*attemptPlace), dir, (wordToPlace)))
//				{
//					canPlace = true;
//					Point startPoint = CulcStartPoint((*placedToConnect), (*attemptPlace), dir, (wordToPlace));
//					InsertWord((wordToPlace), dir, startPoint);
//
//					std::vector<Word>::iterator minusOne = words.erase(words.begin());
//				}
//			}
//		}
//		else
//		{
//			return false;
//		}
//
//		if (!canPlace)
//		{
//		
//			std::vector<CrossPointInfo> vec = dictionary.at(wordToPlace.Number);
//	
//			
//			auto result = std::remove_if(vec.begin(), vec.end(), [&](const CrossPointInfo info)
//			{
//				return (info.Word1Number == attemptPlace->Word1Number &&
//					info.Word2Number == attemptPlace->Word2Number);
//			});
//
//		/*	if (result == vec.end())
//			{
//				return false;
//			}*/
//
//			vec.erase(result);
//			dictionary.at(wordToPlace.Number) = vec;
//			std::vector<Word>::iterator itr =words.erase(words.begin());
//			words.push_back(Word((*itr).Number,(*itr).Text));
//		}
//		return ProcessWords(words, dictionary);
//	}
//}

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

std::string::size_type IndexOf(std::string input, std::string toFind)
{
	auto loc = input.find(toFind, 0);
	if (loc != std::string::npos) {
		return loc;
	}
	else {
		return std::string::npos;
	}
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
