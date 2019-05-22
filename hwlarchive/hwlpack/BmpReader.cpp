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

uint16_t *BmpReader::Read24BitsFileTo16Bits(std::string fileName, uint32_t &outWidth, uint32_t &outHeight) {

	FILE *pFile = fopen(fileName.c_str(), "rb");

	BITMAPFILEHEADER header1;
	BITMAPINFOHEADER header2; // TODO: another bmp version?

	fread(&header1, sizeof(BITMAPFILEHEADER), 1, pFile);
	fread(&header2, sizeof(BITMAPINFOHEADER), 1, pFile);

	fseek(pFile, header1.bfOffBits, SEEK_SET);  // just in case

	uint8_t *pPixels24 = new uint8_t[header2.biWidth * header2.biHeight * 3]; // 24 bits

	fread(pPixels24, 3, header2.biWidth * header2.biHeight, pFile);

	fclose(pFile);

	uint16_t *pPixels = new uint16_t[header2.biWidth * header2.biHeight]; // 16 bits

	// TODO: tiene que ser multiplo de 4
	for (int h = 0; h < header2.biHeight; h++) {
		for (int w = 0; w < header2.biWidth; w++) {

			//pPixels[(511 - h) * header2.biWidth + w] = pReversedPixels[h * header2.biWidth + w];

            //555 format, each color is from 0 to 32, instead of 0 to 256
            //int p24i = h * width_in_bytes_32 + w * 4;
            //int p16i = h * width_in_bytes_16 + w * 2;
            int src_i = (h * header2.biWidth + w) * 3; // TODO: padding???
			uint16_t red = (uint16_t)(pPixels24[src_i + 0] * 31.f / 255.f);
			uint16_t grn = (uint16_t)(pPixels24[src_i + 1] * 31.f / 255.f);
			uint16_t blu = (uint16_t)(pPixels24[src_i + 2] * 31.f / 255.f);
            uint16_t sum = (red) | (grn << 5) | (blu << 10);

            int dest_i = ((511 - h) * header2.biWidth + w); // * 2;   ???
            memcpy(&pPixels[dest_i], &sum, 2);
		}
	}

	outWidth = header2.biWidth;
	outHeight = header2.biHeight;

	return pPixels;
}


