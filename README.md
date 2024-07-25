# KGetVersion
This project queries the version information for `ntoskrnl.exe`, `ci.dll`, and `fltMgr.sys` to obtain offsets. This can then be used to calculate kernel-land offsets for kernel exploitation activities to remove EDR telemetry hooks! Huzzah!

### Updates
- This project will query a static list of offsets included in `offsets.csv`. This was gleaned from the great work on [`EDRSandblast`](https://github.com/wavestone-cdt/EDRSandblast). I owe much to this project!!
- Keep in mind that the newest offsets may not be there if it hasn't been updated in a while...

# Usage
```PowerShell
.\KGetVersion.exe
```