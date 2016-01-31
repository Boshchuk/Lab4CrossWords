#include "stdafx.h"
#include "CanPlace.h"

CanPlace::CanPlace()
{
	CanTop = false;
	//TopStart = topStart;
	CanLeft = false;
	//LeftStart = leftStart;
}

CanPlace::CanPlace(bool canTop, Point topStart, bool canLeft, Point leftStart)
{
	CanTop = canTop;
	TopStart = topStart;
	CanLeft = canLeft;
	LeftStart = leftStart;
}

bool CanPlace::Posible() const
{
	return (CanLeft || CanTop);
}
