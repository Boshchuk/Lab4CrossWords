#include "stdafx.h"
#include "InternalChar.h"


InternalChar::InternalChar()
{
}

InternalChar::InternalChar(int c, char ch)
{
	Symbol = ch;
	PlacedCount = c;
}

InternalChar::~InternalChar()
{
}
