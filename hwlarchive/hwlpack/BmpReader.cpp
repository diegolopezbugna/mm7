#include "stdafx.h"

uint16_t *BmpReader::Read16BitsFile(std::string fileName, uint32_t &outWidth, uint32_t &outHeight) {

	FILE *pFile = fopen(fileName.c_str(), "rb");

	BITMAPFILEHEADER header1;
	BITMAPINFOHEADER header2; // TODO: another bmp version?

	fread(&header1, sizeof(BITMAPFILEHEADER), 1, pFile);
	fread(&header2, sizeof(BITMAPINFOHEADER), 1, pFile);

	fseek(pFile, header1.bfOffBits, SEEK_SET);  // just in case

	uint16_t *pReversedPixels = new uint16_t[header2.biWidth * header2.biHeight]; // 16 bits

	fread(pReversedPixels, 2, header2.biWidth * header2.biHeight, pFile);

	fclose(pFile);

	uint16_t *pPixels = new uint16_t[header2.biWidth * header2.biHeight]; // 16 bits

	// TODO: tiene que ser multiplo de 4
	for (int h = 0; h < header2.biHeight; h++) {
		for (int w = 0; w < header2.biWidth; w++) {
			pPixels[(511 - h) * header2.biWidth + w] = pReversedPixels[h * header2.biWidth + w];
		}
	}

	outWidth = header2.biWidth;
	outHeight = header2.biHeight;

	return pPixels;
}

uint8_t *BmpReader::Read24BitsFile(std::string fileName) {

	FILE *pFile = fopen(fileName.c_str(), "rb");

	BITMAPFILEHEADER header1;
	BITMAPV4HEADER header2;

	fread(&header1, sizeof(BITMAPFILEHEADER), 1, pFile);
	fread(&header2, sizeof(BITMAPV4HEADER), 1, pFile);

	uint8_t *pPixels = new uint8_t[header2.bV4Width * header2.bV4Height * 3]; // 24 bits

	fread(pPixels, 3, header2.bV4Width * header2.bV4Height, pFile);

	fclose(pFile);

	return pPixels;
}
