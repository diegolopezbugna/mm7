#include "stdafx.h"

uint16_t *BmpReader::Read16BitsFile(std::string fileName) {

	FILE *pFile = fopen(fileName.c_str(), "rb");

	BITMAPFILEHEADER header1;
	BITMAPV4HEADER header2;

	fread(&header1, sizeof(BITMAPFILEHEADER), 1, pFile);
	fread(&header2, sizeof(BITMAPV4HEADER), 1, pFile);

	uint16_t *pPixels = new uint16_t[header2.bV4Width * header2.bV4Height]; // 16 bits

	fread(pPixels, 2, header2.bV4Width * header2.bV4Height, pFile);

	return pPixels;
}