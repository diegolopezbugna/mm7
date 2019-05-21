// hwlpack.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#define HWL_TEX_AMASK  0x8000
#define HWL_TEX_RMASK  0x7C00
#define HWL_TEX_GMASK  0x03E0
#define HWL_TEX_BMASK  0x001F

int main()
{
	printf("Starting...\n");

	HWLContainer pD3DBitmaps;
	//pD3DBitmaps = HWLContainer();
	//pD3DBitmaps.Open("C:\\Program Files \\Diego\\Documents\\visual studio 2017\\Projects\\hwlarchive\\Debug\\d3dbitmap.hwl");
	pD3DBitmaps.Open("C:\\Program Files (x86)\\3DO\\Might and Magic VII\\DATA\\d3dbitmap.hwl");
	pD3DBitmaps.LoadTexture("DRSRSCAP");
	pD3DBitmaps.LoadTexture("DRSRNCAP");

	BmpReader bmpReader = BmpReader();
	uint32_t width = 0;
	uint32_t height = 0;
	uint16_t *pixels = bmpReader.Read16BitsFile("C:\\Program Files (x86)\\3DO\\Might and Magic VII\\DATA\\DRSRSCAP.bmp", width, height);
	uint8_t *compressedPixels = (uint8_t *)malloc(compressBound(width * height * 2));

	HWLTextureHeader header = HWLTextureHeader();
	header.uAreaHeigth = header.uBufferHeight = header.uHeight = height;
	header.uAreaWidth = header.uBufferWidth = header.uWidth = width;
	header.uAreaX = header.uAreaY = 0;
	header.uCompressedSize = 0;
	uLongf compressedSize = compressBound(width * height * 2);

	auto result = compress(compressedPixels, &compressedSize, (uint8_t *)pixels, width * height * 2);
	if (result != Z_OK) {
		printf("error compressing %i", result);
	}
	header.uCompressedSize = compressedSize;

	pD3DBitmaps.UpdateTexture("DRSRSCAP", header, (uint16_t *)compressedPixels);
	//pD3DBitmaps.UpdateTexture("DRSRNCAP", header, pixels);
	//pD3DBitmaps.UpdateTexture("DRSRECAP", header, pixels);
	//pD3DBitmaps.UpdateTexture("DRSRWCAP", header, pixels);

	scanf("press");
}

