#!/bin/bash

cd "${0%/*}"

export LD_LIBRARY_PATH=/opt/intel/mediasdk/lib64:/usr/local/lib:/usr/local/lib64:/usr/lib64
export MFX_HOME=/opt/intel/mediasdk
export DISPLAY=:1
export COMPlus_DbgEnableMiniDump=1
export COMPlus_DbgMiniDumpType=1

dotnet ./TVUBMTool.dll

