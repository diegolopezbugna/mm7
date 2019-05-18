#pragma once

#include <cstdint>
#include <cstdio>
#include <map>

//#include "Engine/Strings.h"

#include "stdafx.h"

/*
* HWL format:
*
* (uid) File magic -- "D3DT"
* (uid) File table offset
*
*
* HWL file table:
* (uid) File count -- total number of files ('count')
* (char[count][20]) -- File names; null-terminated + stack garbage
* (uid[count]) -- File data offsets; relative to file beginning.
*
* HWL file data:
*
*/

struct HWLHeader {
	uint32_t uSignature;
	uint32_t uFileTableOffset;
};

struct HWLTextureHeader {
	uint32_t uCompressedSize;
	uint32_t uBufferWidth;
	uint32_t uBufferHeight;
	uint32_t uAreaWidth;
	uint32_t uAreaHeigth;
	uint32_t uWidth;
	uint32_t uHeight;
	uint32_t uAreaX;
	uint32_t uAreaY;
};

struct HWLNode {
	std::string sTextureName;
	size_t uTextureOffset;
	size_t uNodeOffsetInFileTable;
};

//class HWLTexture {
//public:
//	inline HWLTexture() {}
//
//	int uBufferWidth;
//	int uBufferHeight;
//	int uAreaWidth;
//	int uAreaHeigth;
//	unsigned int uWidth;
//	unsigned int uHeight;
//	int uAreaX;
//	int uAreaY;
//	uint16_t *pPixels;
//};

class HWLContainer {
public:
	HWLContainer();
	virtual ~HWLContainer();

	bool Open(const char* pFilename);
	void UpdateTexture(std::string textureName, HWLTextureHeader header, uint16_t *pPixels);
	void LoadTexture(std::string textureName);
	long FindFileTablePos(std::string);

protected:
	FILE *pFile;
	HWLHeader header;
	std::map<std::string, HWLNode> mNodes;
};
