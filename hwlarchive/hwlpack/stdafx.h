// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently, but
// are changed infrequently
//

#pragma once

#include "targetver.h"

#include <stdio.h>
#include <tchar.h>



// TODO: reference additional headers your program requires here

#include "HWLContainer.h"
#include "BmpReader.h"
#include <cstring>
#include <vector>
#include "tinf\tinf.h"

namespace zlib {
	#define ZLIB_WINAPI
	#include <zlib.h>
}
