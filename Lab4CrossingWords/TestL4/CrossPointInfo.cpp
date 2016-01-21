#include "stdafx.h"

#include "CrossPointInfo.h"

bool CrossPointInfo::IsSameWordsCross(CrossPointInfo toCheck)
{
	return (Word1Number == toCheck.Word1Number && Word2Number == toCheck.Word2Number);
}

std::string CrossPointInfo::ToString()
{
	return "Not implimented"; //TODO: add this   // Word1Number.
}

int CrossPointInfo::GetThisCross(int firstKey)
{
	if (firstKey == Word1Number)
	{
		return W1Pos;
	}

	if (firstKey == Word2Number)
	{
		return W2Pos;
	}
}

int CrossPointInfo::GetPairedCrossNum(int oneOfPairNum)
{
	if (oneOfPairNum == Word1Number)
	{
		return Word2Number;
	}

	if (oneOfPairNum == Word2Number)
	{
		return Word1Number;
	}
}
