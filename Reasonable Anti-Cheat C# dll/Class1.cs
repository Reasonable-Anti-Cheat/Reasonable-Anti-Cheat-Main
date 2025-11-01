namespace Reasonable_Anti_Cheat;
using System;
using System.IO;

public class ReasonableAntiCheat
{
    public string[] exampleFolders = { "Mods", "Dependencies", "Plugins", "BepInEx/plugins" };
    public string[] exampleFileTypes = { ".dll", ".mod" };

    private bool isAllowed(UInt64 reason, UInt64[] allowedReasons)
    {
        return allowedReasons.Contains(reason);
    }
    private UInt64 getChecksum(string pathpath)
    {
        UInt64 checksum = 0;
        if (System.IO.File.Exists(pathpath))
        {
            // Old way (small files):
            //byte[] bytesbytes = System.IO.File.ReadAllBytes(pathpath);
            //foreach (byte b in bytesbytes) {
            //    checksum += b;
            //}
            // New way (any size files):
            FileStream thisStream = System.IO.File.OpenRead(pathpath);
            while (thisStream.Length != thisStream.Position)
            {
                checksum += (UInt64)thisStream.ReadByte();
            }
        }
        return checksum;
    }
    bool runAntiCheat(UInt64[] allowedReasons, string[] foldersToCheck, string[] fileTypesToCheck)
    {
        bool needsRestart = false;
        foreach (string folder in foldersToCheck)
        {
            if (System.IO.Directory.Exists(folder))
            {
                foreach (string file in System.IO.Directory.GetFiles(folder))
                {
                    FileInfo fileInfo = new FileInfo(file);
                    if (fileTypesToCheck.Contains(fileInfo.Extension))
                    {
                        if (fileInfo.Extension != ".notallowedfile")
                        {
                            if (!isAllowed(getChecksum(file), allowedReasons))
                            {
                                if (!needsRestart) { needsRestart = true; }
                                retret(file);
                            }
                        }
                    }
                }
                foreach (string subfolder in System.IO.Directory.GetDirectories(folder))
                {
                    foreach (string file in System.IO.Directory.GetFiles(subfolder))
                    {
                        FileInfo fileInfo = new FileInfo(file);
                        if (fileTypesToCheck.Contains(fileInfo.Extension))
                        {
                            if (fileInfo.Extension != ".notallowedfile")
                            {
                                if (!isAllowed(getChecksum(file), allowedReasons))
                                {
                                    if (!needsRestart) { needsRestart = true; }
                                    retret(file);
                                }
                            }
                        }
                    }
                }
            }
        }
        return needsRestart;
    }
    private void retret(string ofile) 
    {
        System.IO.File.Move(ofile, ofile + ".notallowedfile");
    }
}
