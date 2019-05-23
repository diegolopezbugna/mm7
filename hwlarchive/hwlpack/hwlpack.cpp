// hwlpack.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#define HWL_TEX_AMASK  0x8000
#define HWL_TEX_RMASK  0x7C00
#define HWL_TEX_GMASK  0x03E0
#define HWL_TEX_BMASK  0x001F


int main()
{
	const char* dataFolder = "C:\\Program Files (x86)\\3DO\\Might and Magic VII\\DATA\\";
	const char* texturesFolder = "E:\\MM7\\converted\\";

	printf("Starting...\n");

	HWLContainer* pD3DBitmaps = new HWLContainer();
	pD3DBitmaps->Open(std::string(dataFolder) + "d3dbitmap.hwl");
	auto kk = std::string(dataFolder) + std::string(dataFolder);
	auto textureNames = pD3DBitmaps->GetAllTextureNames();

	for (auto textureName: textureNames) {

		BmpReader bmpReader = BmpReader();
		uint32_t width = 0;
		uint32_t height = 0;

		auto bmpFilename = std::string(texturesFolder) + textureName + ".bmp";
		uint16_t *pixels = bmpReader.Read24BitsFileTo16Bits(bmpFilename, width, height);
		if (pixels == nullptr) {
			printf(" error reading %s\n", bmpFilename.c_str());
			continue;
		}

		zlib::uLong compressedSize = zlib::compressBound(width * height * 2);
		uint8_t *compressedPixels = (uint8_t *)malloc(compressedSize);

		auto result = zlib::compress(compressedPixels, &compressedSize, (uint8_t *)pixels, width * height * 2);
		if (result != Z_OK) {
			printf(" error compressing %s -> result: %i\n", textureName, result);
			continue;
		}

		pD3DBitmaps->UpdateTexture(textureName, width, height, compressedSize, (uint16_t *)compressedPixels);

		delete[] pixels;
		free(compressedPixels);
	}

	delete pD3DBitmaps;

	printf("Finished. Press enter to exit.\n");
	scanf("enter");
}

