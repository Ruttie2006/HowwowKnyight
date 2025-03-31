<div align="center">
  <br/>
  
  [![license](https://img.shields.io/github/license/Ruttie2006/HowwowKnyight.svg?style=flat-square)](LICENSE)
  [![on Lumafly](https://img.shields.io/badge/on-Lumafly-blue.svg?style=flat-square)](https://github.com/fifty-six/scarab)
  [![on Scarab](https://img.shields.io/badge/on-Scarab-blue.svg?style=flat-square)](https://themulhima.github.io/Lumafly/)
  [![for version](https://img.shields.io/badge/for_hk-v1.5.78-blue.svg?style=flat-square)](https://themulhima.github.io/Lumafly/)
  [![get support](https://img.shields.io/badge/get_support-on_discord-darkgreen.svg?style=flat-square)](https://discord.gg/y9tX7z9HzR)
  [![Downloads](https://img.shields.io/github/downloads/Ruttie2006/HowwowKnyight-1.5/latest/total?label=downloads&color=darkgreen&style=flat-square)](https://github.com/Ruttie2006/HowwowKnyight/releases)
  [![Latest Release](https://img.shields.io/github/v/release/Ruttie2006/HowwowKnyight-1.5?style=flat-square)](https://github.com/Ruttie2006/HowwowKnyight/releases/latest)

  <img src="/HowwowKnyight/Resources/OwOTitle.png" alt="Logo" height="250">
</div>

---  
  

## About

HowwowKnyight is a joke mod that replaces in-game text with UwU language.  
This mod was originally created by [Henpemaz](https://github.com/henpemaz).  
You can still find the original version [here](https://github.com/henpemaz/HowwowKnyight).  

## Compiling

> [!WARNING]  
> This is only for developers.  
> If you simply want to use this mod, you can safely skip over this section.

Compilation is (fairly) simple.
1. Clone the repository.
2. Create a file called `LocalProperties.props` in the root of the cloned repository.
3. Create a `Project` node with `PropertyGroup` child and set `HKRefs` to the directory containing the **modded** dll files.

After this, you should be able to compile the mod.
An example `LocalProperties.props` would look something like:
```xml
<Project>
    <PropertyGroup>
        <HKRefs>PATH/TO/MANAGED/DIRECTORY</HKRefs>
    </PropertyGroup>
</Project>
```

## FAQ

**Q:** Can this mod be disabled in-game?  
**A:** Yes, you can disable individual parts of this mod in its mod menu. Disable all features and the mod does nothing.  

**Q:** Does this mod affect gameplay?  
**A:** Other than text being harder to read, no. This mod does not affect any game mechanics.  

**Q:** Can this mod be configured?  
**A:** Yes! Simply edit the globalsettings file.  