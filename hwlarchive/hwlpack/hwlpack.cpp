// hwlpack.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#define HWL_TEX_AMASK  0x8000
#define HWL_TEX_RMASK  0x7C00
#define HWL_TEX_GMASK  0x03E0
#define HWL_TEX_BMASK  0x001F


int main()
{
	std::string dataFolder = "C:\\Program Files (x86)\\3DO\\Might and Magic VII\\DATA\\";
	std::string texturesFolder = "C:\\Program Files (x86)\\3DO\\Might and Magic VII\\DATA\\";

	printf("Starting...\n");

	HWLContainer* pD3DBitmaps = new HWLContainer();
	pD3DBitmaps->Open(dataFolder + "d3dbitmap.hwl");

	auto textureNames = pD3DBitmaps->GetAllTextureNames();

	for (auto textureName in textureNames) {

		BmpReader bmpReader = BmpReader();
		uint32_t width = 0;
		uint32_t height = 0;

		std::string folder = 
		uint16_t *pixels = bmpReader.Read24BitsFileTo16Bits(texturesFolder + textureName + ".bmp", width, height);
		uint32_t compressedSize = zlib::compressBound(width * height * 2);
		uint8_t *compressedPixels = (uint8_t *)malloc(compressedSize);

		auto result = zlib::compress(compressedPixels, &compressedSize, (uint8_t *)pixels, width * height * 2);
		if (result != Z_OK) {
			printf(" error compressing %s -> result: %i\n", textureName, result);
			continue;
		}
		header.uCompressedSize = compressedSize;

		pD3DBitmaps->UpdateTexture(textureName, width, height, (uint16_t *)compressedPixels, compressedSize);

	}

	delete pD3DBitmaps;

	printf("Finished. Press enter to exit.\n");
	scanf();
}

