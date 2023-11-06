# OTP Demo with FT Optix

## Designtime setup
1. Install package `GoogleAuthenticator` by [Brandon Potter](https://github.com/BrandonPotter/GoogleAuthenticator) (tested version 3.0.0)
2. Add `<CopyLocalLockFileAssemblies>true</CopyLocalLockFileAssemblies>` after `<AppendTargetFrameworkToOutputPath>false</AppendTargetFrameworkToOutputPath>` in `csproj` file
3. Compile

## Runtime setup
1. Click on the tab to add device
2. Generate QR Code
3. Scan QR code with GoogleAuthenticator app
4. Move to CheckPin page
5. Type OTP code and hit enter. LED will show the validation result

## Known issues
- Key should be stored to a persistent file and reloaded at every reboot of the machine to avoid readding device at every reboot of the application
