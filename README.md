# KGetVersion
This project queries the version information for `ntoskrnl.exe`, `ci.dll`, and `fltMgr.sys` to obtain offsets. This can then be used to calculate kernel-land offsets for kernel exploitation activities to remove EDR telemetry hooks! Huzzah!

### Updates
- This project will query a static list of offsets included in `offsets.csv`. This was gleaned from the great work on [`EDRSandblast`](https://github.com/wavestone-cdt/EDRSandblast). I owe much to this project!!
- Keep in mind that the newest offsets may not be there if it hasn't been updated in a while...

# Usage
```PowerShell
.\KGetVersion.exe
```

```
PS C:\Users\mayh3m\source\repos> C:\Users\mayh3m\source\repos\KGetVersion\KGetVersion\bin\x64\Release\KGetVersion.exe
[|] Getting ntoskrnl.exe version
[|] ntoskrnl_22621-3880 - 10.0.22621.3880 (WinBuild.160101.0800)
[+] Ntoskrnl offset! - ntoskrnl_22621-3880.exe,d0c440,d0c240,d0c040,87a,c32058,20,60,d1da58,d1da80,c8,c37560
[|] Getting fltMgr.sys version
[|] fltmgr_22621-2506 - 10.0.22621.2506 (WinBuild.160101.0800)
[+] Fltmgr offset! - fltmgr_22621-2506.sys,2e700,58,68,8,48,10,60,68,58,a8,70
[|] Getting ci.dll version
[|] ci_22621-1 - 10.0.22621.1 (WinBuild.160101.0800)
[-] Version not found...check the version manually.
```