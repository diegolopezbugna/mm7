// hwlpack.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

int main()
{
	printf("Starting...\n");

	// opcion1
	// crear archivo .hwl
	// leer imagen .png o .bmp
	// agregar imagen al .hwl
	// grabar en disco el .hwl

	// opcion2
	// leer archivo .hwl
	// buscar nodo a actualizar
	// cambiar los datos del nodo en memoria
	// recalcular todos los offsets siguientes
	// grabar

	HWLContainer pD3DBitmaps;
	//pD3DBitmaps = HWLContainer();
	//pD3DBitmaps.Open("C:\\Program Files \\Diego\\Documents\\visual studio 2017\\Projects\\hwlarchive\\Debug\\d3dbitmap.hwl");
	pD3DBitmaps.Open("C:\\Program Files (x86)\\3DO\\Might and Magic VII\\DATA\\d3dbitmap.hwl");
	pD3DBitmaps.LoadTexture("DRSRSCAP");
	pD3DBitmaps.LoadTexture("DRSRNCAP");

	//HWLTextureHeader textureHeader = HWLTextureHeader();
	//uint16_t pixels[1000];
	//pD3DBitmaps.UpdateTexture("DRSRSCAP", textureHeader, pixels);

	scanf("press");
}

