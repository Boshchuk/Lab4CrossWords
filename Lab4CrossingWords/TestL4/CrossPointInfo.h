#pragma once
#include <string>

class CrossPointInfo
{
public:
	int Word1Number;
	int Word2Number;

	char Letter;

	int W1Pos;
	int W2Pos;

public:
	bool IsSameWordsCross(CrossPointInfo toCheck);

	std::string ToString();

	int GetThisCross(int firstKey);

	int GetPairedCrossNum(int oneOfPairNum);
};
