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

	CrossPointInfo(int w1n, int w2n, char l, int w1pos, int w2pos);


	bool IsSameWordsCross(CrossPointInfo toCheck);

	std::string ToString();

	int GetThisCross(int firstKey);

	int GetPairedCrossNum(int oneOfPairNum);

};
