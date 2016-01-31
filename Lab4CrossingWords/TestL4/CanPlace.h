#pragma once
#include "Point.h"

class CanPlace
{
public:
	bool CanTop;

	Point TopStart;


	bool CanLeft;

	Point LeftStart;

public:
	CanPlace();
	CanPlace(bool canTop, Point topStart, bool canLeft, Point leftStart);

public:
	bool Posible() const;
	
};