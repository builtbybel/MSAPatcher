# MSAPatcher

With Microsoft’s recent Windows 11 updates, [the bypass for the network requirement (NRO) was effectively blocked](https://blogs.windows.com/windows-insider/2025/03/28/announcing-windows-11-insider-preview-build-26200-5516-dev-channel/) , forcing users into an online account creation. MSAPatcher brings back the simplicity of the bypassnro.cmd one-liner, allowing you to bypass the NRO without having to manually add registry keys or deal with complex workarounds.

This app restores the original method, making it easier than ever to bypass the network requirement during the Windows 11 setup process. Now, you can simply execute a single command instead of dealing with convoluted registry changes.

**MSAPatcher helps you bypass the network requirement during Windows 11 setup. Choose from two methods:**

---

## Method 1: Classic BypassNRO (Registry Key Method)

1. **Prepare the USB Stick**:
   - **Option 1A**: Manually copy `MSAPatcher.exe` and `bypassnro.cmd` to the root of your USB stick.
   - **Option 1B**: Run `MSAPatcher.exe` to automatically patch the USB stick (it will copy the necessary files).

2. **During Windows Setup**:
   - At the "Welcome" screen, press **Shift + F10** to open the Command Prompt.
   - Switch to the USB drive (`D:` or `E:`).
   - Run `bypassnro.cmd` to set the registry key and bypass the network requirement.

3. **Restart**:
   - After the process finishes, restart the PC (press **Y** when asked).

---

## Method 2: Direct Local Account Creation

1. **Run `MSAPatcher.exe`**.
2. **Choose Option 2** to trigger the local account creation screen directly (only for Windows 11 Home/Pro).

