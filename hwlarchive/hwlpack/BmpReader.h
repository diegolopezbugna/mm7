#pragma once

#define WIN32_LEAN_AND_MEAN
#include <windows.h>


class BmpReader {

public:
	uint16_t *Read16BitsFile(std::string fileName);

};