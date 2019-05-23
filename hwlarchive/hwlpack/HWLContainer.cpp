#include "stdafx.h"

HWLContainer::HWLContainer() {
	pFile = nullptr;
	tinf_init();
}

HWLContainer::~HWLContainer() {
	if (pFile != nullptr) {
		fclose(this->pFile);
	}
}

bool HWLContainer::Open(std::string filename) {
	pFile = fopen(filename.c_str(), "rb+");
	if (!pFile) {
		printf("Failed to open file: %s\n", filename.c_str());
		return false;
	}

	fread(&header, sizeof(HWLHeader), 1, pFile);
	if (header.uSignature != 'TD3D') {
		printf("Invalid format: %s\n", filename.c_str());
		return false;
	}
	printf("  header.uFileTableOffset: %ul\n", header.uFileTableOffset);
	fseek(pFile, header.uFileTableOffset, SEEK_SET);

	std::vector<HWLNode> vNodes;

	uint32_t uNumItems = 0;
	fread(&uNumItems, 4, 1, pFile);
	char tmpName[21];
	for (unsigned int i = 0; i < uNumItems; ++i) {
		fread(tmpName, 20, 1, pFile);
		tmpName[20] = 0;
		HWLNode node;
		node.sTextureName = std::string(tmpName);
		vNodes.push_back(node);
	}

	for (unsigned int i = 0; i < uNumItems; ++i) {
		uint32_t uTextureOffset = 0;
		vNodes[i].uNodeOffsetInFileTable = ftell(pFile);
		fread(&uTextureOffset, 4, 1, pFile);
		vNodes[i].uTextureOffset = uTextureOffset;
	}

	for (HWLNode &node : vNodes) {
		mNodes[node.sTextureName] = node;
		printf(" - node: sTextureName: %s uTextureOffset: %ul uNodeOffsetInFileTable: %ul\n", node.sTextureName.c_str(), node.uTextureOffset, node.uNodeOffsetInFileTable);
	}

	return true;
}

void HWLContainer::UpdateTexture(std::string textureName, uint32_t width, uint32_t height, uint32_t compressedSize, uint16_t *pPixels) {
	if (width % 4 != 0 || height %4 != 0) {
		printf(" error updating %s, width and height must be multiple of 4 (must implement)", textureName.c_str());
		return;
	}

	fseek(pFile, 0, SEEK_END);
	size_t endPosition = ftell(pFile);

	uint32_t nodeOffset = mNodes[textureName].uNodeOffsetInFileTable;
	fseek(pFile, nodeOffset, SEEK_SET);
	uint32_t newTextureOffset = endPosition;
	fwrite(&newTextureOffset, 4, 1, pFile);
	mNodes[textureName].uTextureOffset = newTextureOffset;

	HWLTextureHeader header = HWLTextureHeader();
	header.uAreaHeigth = header.uBufferHeight = header.uHeight = height;
	header.uAreaWidth = header.uBufferWidth = header.uWidth = width;
	header.uAreaX = header.uAreaY = 0;
	header.uCompressedSize = compressedSize;

	fseek(pFile, newTextureOffset, SEEK_SET);
	fwrite(&header, sizeof(HWLTextureHeader), 1, pFile);
	fwrite(pPixels, sizeof(uint16_t), header.uWidth * header.uHeight, pFile); // TODO: cantidad de pixeles

	printf(" texture %s updated\n", textureName.c_str());
}

std::vector<std::string> HWLContainer::GetAllTextureNames() {
	std::vector<std::string> names;
	for (auto& node: mNodes) {
		names.push_back(node.first);
	}
	return names;
}

// do not used
long HWLContainer::FindFileTablePos(std::string name) {
	fseek(pFile, header.uFileTableOffset, SEEK_SET);

	uint32_t uNumItems = 0;
	fread(&uNumItems, 4, 1, pFile);
	char tmpName[21];
	long posInFileTable;

	for (unsigned int i = 0; i < uNumItems; ++i) {
		fread(tmpName, 20, 1, pFile);
		tmpName[20] = 0;
		//if (strcmp(tmpName, name.c_str())) {
		//	break;
		//}
		if (name.compare(tmpName)) {
			break;
		}
	}
	posInFileTable = ftell(pFile);
	return posInFileTable;
}

// do not used
void HWLContainer::LoadTexture(std::string textureName) {
	size_t uOffset = mNodes[textureName].uTextureOffset;
	fseek(pFile, uOffset, SEEK_SET);

	HWLTextureHeader textureHeader;
	fread(&textureHeader, sizeof(HWLTextureHeader), 1, pFile);
	printf(" LoadTexture: texture %s %ux%u\n", textureName.c_str(), textureHeader.uWidth, textureHeader.uHeight);

	uint16_t *pPixels = new uint16_t[textureHeader.uWidth * textureHeader.uHeight];

	if (textureHeader.uCompressedSize) {

		uint8_t *pCompressedData = (uint8_t*)malloc(textureHeader.uCompressedSize);
		unsigned int decodeSz;
		fread(pCompressedData, 1, textureHeader.uCompressedSize, pFile);

		int result;
		if (result = tinf_zlib_uncompress(pPixels, &decodeSz, pCompressedData, textureHeader.uCompressedSize)) {
			printf("Failed to decompress texture: %s -> %i", textureName.c_str(), result);
			return;
		}

		if (decodeSz != textureHeader.uWidth * textureHeader.uHeight * 2) { // 16 bits)
			printf("Failed to decompress texture: %s -> decoded data different expected length", textureName.c_str());
			return;
		}

		free(pCompressedData);
	}
	else {
		fread(pPixels, 2, textureHeader.uWidth * textureHeader.uHeight, pFile);
	}

	bool isAllSameSizeH = textureHeader.uWidth == textureHeader.uBufferWidth && textureHeader.uWidth == textureHeader.uAreaWidth;
	bool isAllSameSizeV = textureHeader.uHeight == textureHeader.uBufferHeight && textureHeader.uHeight == textureHeader.uAreaHeigth;
	if (!isAllSameSizeH || !isAllSameSizeV) {
		printf("Wrong texture dimensions: %s", textureName.c_str());
	}

	free(pPixels);
}
 