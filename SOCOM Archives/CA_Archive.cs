using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SOCOM_Archives
{
    class CA_Archive
    {
        private Header_Type1 ArchiveHead1;
        private File_Type1[] ArchiveFiles1;
        private Header_Type2 ArchiveHead2; private File_Type2[] ArchiveFiles2;
        private string ArchiveBrowsePath; private int ArchiveOpen;
        private ArchiveFolder DirectoryListings;
        

        public struct Header_Type1
        {
            public uint MainHeaderSize;
            public uint FileHeaderSize;
            public uint ArchiveVersion;
            public uint ArchiveHeaderSize;
            public uint BodySize;
            public uint unusedID;
            public uint Compression;
            public UInt16 Errors;
            public UInt16 Warnings;
            public uint[] padding;
            public uint FileCount;
        }
        private struct File_Type1
        {
            public string Name;
            public string FullPath;
            public uint VarName;
            public uint dbID;
            public uint Entry;
            public uint Size;
            public uint ChecksumV1;
            public uint Empty;
            public byte[] Contents;
        }

        private struct Header_Type2
        {
            public uint MainHeaderSize;
            public uint unknown1;
            public uint unknownID;
            public uint unknown2;
            public uint unknown3;
            public string Version;
            public uint FileCount;
            public uint FileHeaderSize;
        }
        private struct File_Type2
        {
            public uint FileHeadSize;
            public string Name;
            public string FullPath;
            public byte[] padding;
            public uint Entry;
            public uint Size;
            public byte[] Contents;
        }

        private struct ArchiveFolder
        {
            public string Name;
            public int FileCount;
            public int SubFolderCount;
            public ArchiveFolder[] SubFolders;
            public int[] FileIndexes;
        }

        public int OpenArchive(string fName)
        {
            byte[] fData;

            try
            {
                fData = File.ReadAllBytes(fName);
            }
            catch
            {
                return -1;
            }
            
            return LoadArchive(fData);
        }

        public void NewArchive(int type)
        {
            if (type == 1)
            {
                byte[] blank = new byte[0xc8];

                blank[0] = 0xc8;
                blank[4] = 0x58;
                blank[8] = 6;
                blank[0x17] = 0x12; blank[0x16] = 0x34; blank[0x15] = 0x56; blank[0x14] = 0x78;

                LoadArchive(blank);
            }
            else if (type == 2)
            {
                byte[] blank = new byte[0xfc];

                blank[0] = 0xfc;
                blank[4] = 0x01;
                blank[11] = 0x12; blank[10] = 0x34; blank[9] = 0x56; blank[8] = 0x78;
                blank[12] = 0x02;

                LoadArchive(blank);
            }
        }

        public int LoadArchive(byte[] fData)
        {
            uint i = 0;
            uint i2 = 0;
            uint fType = BitConverter.ToUInt32(fData, 0);

            if (fType == 0xc8)
            {
                ArchiveOpen = 1;
                ArchiveHead1.MainHeaderSize = BitConverter.ToUInt32(fData, 0);
                ArchiveHead1.FileHeaderSize = BitConverter.ToUInt32(fData, 4);
                ArchiveHead1.ArchiveVersion = BitConverter.ToUInt32(fData, 8);
                ArchiveHead1.ArchiveHeaderSize = BitConverter.ToUInt32(fData, 0xc);
                ArchiveHead1.BodySize = BitConverter.ToUInt32(fData, 0x10);
                ArchiveHead1.unusedID = BitConverter.ToUInt32(fData, 0x14);
                ArchiveHead1.Compression = BitConverter.ToUInt32(fData, 0x18);
                ArchiveHead1.Errors = BitConverter.ToUInt16(fData, 0x1c);
                ArchiveHead1.Warnings = BitConverter.ToUInt16(fData, 0x1e);
                ArchiveHead1.padding = new uint[0x2a];
                ArchiveHead1.FileCount = BitConverter.ToUInt32(fData, 0xc4);

                ArchiveFiles1 = new File_Type1[ArchiveHead1.FileCount];

                uint FileHeadEntry = ArchiveHead1.MainHeaderSize;
                for (i = 0; i < ArchiveHead1.FileCount; i++)
                {
                    i2 = FileHeadEntry;
                    
                    ArchiveFiles1[i].FullPath = "";
                    while (fData[i2] > 0)
                    {
                        ArchiveFiles1[i].FullPath += Convert.ToChar(fData[i2]);
                        i2++;
                    }
                    string spTmp = ArchiveFiles1[i].FullPath + "/";
                    spTmp = spTmp.Replace('\\', '/');
                    spTmp = spTmp.Replace("//", "/");
                    string[] sp = spTmp.Split('/');
                    ArchiveFiles1[i].Name = sp[sp.Length - 2];

                    ArchiveFiles1[i].VarName = BitConverter.ToUInt32(fData, Convert.ToInt32(FileHeadEntry + 0x40));
                    ArchiveFiles1[i].dbID = BitConverter.ToUInt32(fData, Convert.ToInt32(FileHeadEntry + 0x44));
                    ArchiveFiles1[i].Entry = BitConverter.ToUInt32(fData, Convert.ToInt32(FileHeadEntry + 0x48));
                    ArchiveFiles1[i].Size = BitConverter.ToUInt32(fData, Convert.ToInt32(FileHeadEntry + 0x4c));
                    ArchiveFiles1[i].ChecksumV1 = BitConverter.ToUInt32(fData, Convert.ToInt32(FileHeadEntry + 0x50));
                    ArchiveFiles1[i].Empty = BitConverter.ToUInt32(fData, Convert.ToInt32(FileHeadEntry + 0x54));
                    ArchiveFiles1[i].Contents = new byte[ArchiveFiles1[i].Size];

                    for (i2 = 0; i2 < ArchiveFiles1[i].Size; i2++)
                        ArchiveFiles1[i].Contents[i2] = fData[ArchiveFiles1[i].Entry + ArchiveHead1.ArchiveHeaderSize + i2];

                    FileHeadEntry += ArchiveHead1.FileHeaderSize;
                }

                BuildDirectoryListings();
                return Convert.ToInt32(ArchiveHead1.FileCount);
            }
            else if (fType == 0xfc)
            {
                ArchiveOpen = 2;
                ArchiveHead2.MainHeaderSize = BitConverter.ToUInt32(fData, 0);
                ArchiveHead2.unknown1 = BitConverter.ToUInt32(fData, 4);
                ArchiveHead2.unknownID = BitConverter.ToUInt32(fData, 8);
                ArchiveHead2.unknown2 = BitConverter.ToUInt32(fData, 0xc);
                ArchiveHead2.unknown3 = BitConverter.ToUInt32(fData, 0x10);

                i2 = 0x14;
                ArchiveHead2.Version = "";
                while (fData[i2] != 0)
                {
                    ArchiveHead2.Version += Convert.ToChar(fData[i2]);
                    i2++;
                }
                ArchiveHead2.FileCount = BitConverter.ToUInt32(fData, 0x98);
                ArchiveHead2.FileHeaderSize = BitConverter.ToUInt32(fData, 0x9c);

                ArchiveFiles2 = new File_Type2[ArchiveHead2.FileCount];

                i2 = 0xa0;
                for (i = 0; i < ArchiveHead2.FileCount; i++)
                {
                    ArchiveFiles2[i].FileHeadSize = BitConverter.ToUInt32(fData, Convert.ToInt32(i2));

                    uint i3 = i2 + 4;
                    ArchiveFiles2[i].FullPath = "";
                    while (fData[i3] != 0)
                    {
                        ArchiveFiles2[i].FullPath += Convert.ToChar(fData[i3]);
                        i3++;
                    }
                    string spTmp = ArchiveFiles2[i].FullPath + "/";
                    spTmp = spTmp.Replace('\\', '/');
                    spTmp = spTmp.Replace("//", "/");
                    string[] sp = spTmp.Split('/');
                    ArchiveFiles2[i].Name = sp[sp.Length - 2];
                    
                    ArchiveFiles2[i].padding = new byte[(i2+ 0x44) - i3];
                    
                    ArchiveFiles2[i].Entry = BitConverter.ToUInt32(fData, Convert.ToInt32(i2 + 0x44));
                    ArchiveFiles2[i].Size = BitConverter.ToUInt32(fData, Convert.ToInt32(i2 + 0x48));

                    ArchiveFiles2[i].Contents = new byte[ArchiveFiles2[i].Size];
                    for (i3 = 0; i3 < ArchiveFiles2[i].Size; i3++)
                    {
                        ArchiveFiles2[i].Contents[i3] = fData[ArchiveFiles2[i].Entry + i3];
                    }
                    
                    i2 += ArchiveFiles2[i].FileHeadSize;
                    
                }

                BuildDirectoryListings();
                return Convert.ToInt32(ArchiveHead2.FileCount);
            }

            ArchiveOpen = 0;
            return -1;
        }

        private void BuildDirectoryListings()
        {
            uint totalFiles = 0;

            if (ArchiveOpen == 1)
                totalFiles = ArchiveHead1.FileCount;
            else if (ArchiveOpen == 2)
                totalFiles = ArchiveHead2.FileCount;
            else
                return;

            DirectoryListings = new ArchiveFolder();
            DirectoryListings.Name = "z:";

            for (int i = 0; i < totalFiles; i++)
            {
                string[] subs;
                string tmpDIR = "";
                string tmpFName = "";

                if (ArchiveOpen == 1)
                    tmpDIR = ArchiveFiles1[i].FullPath; tmpFName = ArchiveFiles1[i].Name;
                if (ArchiveOpen == 2)
                    tmpDIR = ArchiveFiles2[i].FullPath; tmpFName = ArchiveFiles2[i].Name; 

                tmpDIR = tmpDIR.Substring(0, tmpDIR.Length - tmpFName.Length);

                tmpDIR = tmpDIR.Replace('\\', '/') + "/";
                int oldLen = tmpDIR.Length;
                do { oldLen = tmpDIR.Length; tmpDIR = tmpDIR.Replace("//", "/"); }
                while (tmpDIR.Length != oldLen);
                
                subs = tmpDIR.Split('/');
                if (subs[0] != "")
                    AddDIRListing(subs, 0, tmpFName,  ref DirectoryListings, i);
                else
                    AddDIRListing(subs, 1, tmpFName, ref DirectoryListings, i);
            }
        }
        public void AddFolder(string folderPath)
        {
            string tmpDIR = folderPath + "/";
            string[] subs = tmpDIR.Split('/');
            AddDIRListing(subs, 1, "", ref DirectoryListings, -1);
        }
        private void AddDIRListing(string[] fPath, int fIndex, string fName, ref ArchiveFolder CurrentDIR, int FileIndex)
        {
            string debugStr = string.Empty;
            for (int i = 0; i < CurrentDIR.SubFolderCount; i++)
            {
                if (CurrentDIR.SubFolders[i].Name == fPath[fIndex])
                {
                    if (fIndex == fPath.Length - 1)
                    {
                        for (int i2 = 0; i2 < fIndex; i2++)
                            debugStr += fPath[i2] + "/";
                        Console.WriteLine("(To-Do 1) Add File: " + debugStr);
                        return;
                    }
                    else
                    {
                        AddDIRListing(fPath, fIndex + 1, fName, ref CurrentDIR.SubFolders[i], FileIndex);
                        return;
                    }
                }
            }
            if (fIndex == fPath.Length - 1)
            {
                if (fName.Length > 0)
                {
                    CurrentDIR.FileCount++;
                    Array.Resize(ref CurrentDIR.FileIndexes, CurrentDIR.FileCount);
                    CurrentDIR.FileIndexes[CurrentDIR.FileCount - 1] = FileIndex;
                }
                return;
            }

            CurrentDIR.SubFolderCount++;
            Array.Resize(ref CurrentDIR.SubFolders, CurrentDIR.SubFolderCount);

            CurrentDIR.SubFolders[CurrentDIR.SubFolderCount-1].Name = fPath[fIndex];

            int subI = CurrentDIR.SubFolderCount - 1;
            AddDIRListing(fPath, fIndex + 1, fName, ref CurrentDIR.SubFolders[subI], FileIndex);
        }
        
        public void AddFile(string filePath, string archivePath, uint varName)
        {
            byte[] fileData;
            uint fileSize;

            if (ArchiveOpen == 0) { return; }

            try {
                fileData = File.ReadAllBytes(filePath);
            }
            catch {
                return;
            }

            fileSize = Convert.ToUInt32(fileData.Length);

            if (ArchiveOpen == 1)
            {
                ArchiveHead1.FileCount++;
                Array.Resize(ref ArchiveFiles1, ArchiveFiles1.Length + 1);

                int fIndex = ArchiveFiles1.Length - 1;

                ArchiveFiles1[fIndex].FullPath = archivePath.Replace("z:/","");
                ArchiveFiles1[fIndex].FullPath = ArchiveFiles1[fIndex].FullPath.Replace("/", "\\");

                string spTmp = ArchiveFiles1[fIndex].FullPath + "/";
                spTmp = spTmp.Replace('\\', '/');
                spTmp = spTmp.Replace("//", "/");
                string[] sp = spTmp.Split('/');
                ArchiveFiles1[fIndex].Name = sp[sp.Length - 2];

                CA_Checksums CAChecksum = new CA_Checksums();

                ArchiveFiles1[fIndex].VarName = varName;
                ArchiveFiles1[fIndex].dbID = 0;
                ArchiveFiles1[fIndex].Entry = 0;
                ArchiveFiles1[fIndex].Size = fileSize;
                ArchiveFiles1[fIndex].ChecksumV1 = CAChecksum.GenerateChecksum(fileData, fileSize);
                ArchiveFiles1[fIndex].Empty = 0;

                ArchiveFiles1[fIndex].Contents = new byte[fileSize];
                for (int i = 0; i < fileSize; i++)
                {
                    ArchiveFiles1[fIndex].Contents[i] = fileData[i];
                }

                BuildDirectoryListings();
            }
            else if (ArchiveOpen == 2)
            {
                ArchiveHead2.FileCount++;
                Array.Resize(ref ArchiveFiles2, ArchiveFiles2.Length + 1);

                int fIndex = ArchiveFiles2.Length - 1;

                ArchiveFiles2[fIndex].FullPath = archivePath;
                ArchiveFiles2[fIndex].FullPath = archivePath.Replace("z:/", "");
                ArchiveFiles2[fIndex].FullPath = ArchiveFiles2[fIndex].FullPath.Replace("/", "\\");
                
                string spTmp = ArchiveFiles2[fIndex].FullPath + "/";
                spTmp = spTmp.Replace('\\', '/');
                spTmp = spTmp.Replace("//", "/");
                string[] sp = spTmp.Split('/');
                ArchiveFiles2[fIndex].Name = sp[sp.Length - 2];
                
                ArchiveFiles2[fIndex].Entry = 0;
                ArchiveFiles2[fIndex].Size = fileSize;

                ArchiveFiles2[fIndex].Contents = new byte[fileSize];
                for (int i = 0; i < fileSize; i++)
                {
                    ArchiveFiles2[fIndex].Contents[i] = fileData[i];
                }

                BuildDirectoryListings();
            }
        }

        public void DeleteFile(string fullPath, string fileName)
        {
            if (ArchiveOpen == 0) { return; }

            string[] splitPath = fullPath.Split('/');

            DelFile(ref DirectoryListings, splitPath, 0,fileName);
            BuildDirectoryListings();
        }
        private void DelFile(ref ArchiveFolder parent, string[] path, int pathI, string fName)
        {
            if (pathI == path.Length - 2)
            {
                for (int i = 0; i < parent.FileCount; i++)
                {
                    if (ArchiveOpen == 1)
                    {
                        if (ArchiveFiles1[parent.FileIndexes[i]].Name == fName)
                        {
                            int fIndex = parent.FileIndexes[i];

                            for (int i2 = i; i2 < parent.FileCount - 1; i2++)
                            {
                                parent.FileIndexes[i2] = parent.FileIndexes[i2 + 1];
                            }
                            Array.Resize(ref parent.FileIndexes, parent.FileIndexes.Length - 1);
                            parent.FileCount--;

                            int totIndex = Convert.ToInt32(ArchiveHead1.FileCount);
                            for (int i2 = fIndex; i2 < totIndex - 1; i2++)
                            {
                                ArchiveFiles1[i2] = ArchiveFiles1[i2 + 1];
                            }
                            ArchiveHead1.FileCount--;

                            return;
                        }
                    }
                    else if (ArchiveOpen == 2)
                    {
                        if (ArchiveFiles2[parent.FileIndexes[i]].Name == fName)
                        {
                            int fIndex = parent.FileIndexes[i];

                            for (int i2 = i; i2 < parent.FileCount - 1; i2++)
                            {
                                parent.FileIndexes[i2] = parent.FileIndexes[i2 + 1];
                            }
                            Array.Resize(ref parent.FileIndexes, parent.FileIndexes.Length - 1);
                            parent.FileCount--;

                            int totIndex = Convert.ToInt32(ArchiveHead2.FileCount);
                            for (int i2 = fIndex; i2 < totIndex - 1; i2++)
                            {
                                ArchiveFiles2[i2] = ArchiveFiles2[i2 + 1];
                            }

                            ArchiveHead2.FileCount--;
                            Array.Resize(ref ArchiveFiles2, ArchiveFiles2.Length - 1);
                            
                            return;
                        }
                    }
                }
                return;
            }

            for (int i = 0; i < parent.SubFolderCount; i++)
            {
                if (parent.SubFolders[i].Name == path[pathI + 1])
                {
                    DelFile(ref parent.SubFolders[i], path, pathI + 1, fName);
                    return;
                }
            }
        }

        public bool SaveArchive(string fPath)
        {
            if (ArchiveOpen == 0) { return false; }

            try
            {
                if (File.Exists(fPath))
                    File.Delete(fPath);
            }
            catch { return false; }

            BinaryWriter bw;
            try
            {
                bw = new BinaryWriter(File.Open(fPath, FileMode.Create));
            }
            catch { return false; }
            
            if (ArchiveOpen == 1)
            {
                bw.Write(Convert.ToUInt32(0x000000c8)); // Header Size
                bw.Write(Convert.ToUInt32(0x00000058)); // File Headers Size
                bw.Write(Convert.ToUInt32(0x00000006)); // Version
                bw.Write(Convert.ToUInt32((ArchiveHead1.FileCount * 0x58) + 0xc8)); // Full Head Size

                uint fEntry = 0;
                uint bodySize = 0;
                for (int i = 0; i < ArchiveHead1.FileCount; i++)
                {
                    bodySize += ArchiveFiles1[i].Size;
                    fEntry += bodySize;
                    while ((fEntry & 0xF) != 0) { fEntry += 4; bodySize += 4; }
                }
                bw.Write(bodySize); // Body Size

                bw.Write(Convert.ToUInt32(0x4532abcd)); // ?
                bw.Write(Convert.ToUInt32(0));          // Compression
                bw.Write(Convert.ToUInt16(0x0000));     // Errors
                bw.Write(Convert.ToUInt16(0x0000));     // Warnings

                for (uint i = 0x20; i < 0xc4; i += 4)
                    bw.Write(Convert.ToUInt32(0));

                bw.Write(ArchiveHead1.FileCount); // File Count

                /*
                    public uint MainHeaderSize;
                    public uint FileHeaderSize;
            
                    public uint unknown1;

                    public uint ArchiveHeaderSize;
                    public uint BodySize;
            
                    public uint unknownID;

                    public uint empty;
                    public uint unknown2;

                    public uint[] padding;

                    public uint FileCount;
                */

                fEntry = 0;
                for (int i = 0; i < ArchiveHead1.FileCount; i++)
                {
                    bw.Write(ArchiveFiles1[i].FullPath.ToCharArray());
                    for (int i2 = ArchiveFiles1[i].FullPath.Length; i2 < 0x40; i2++)
                    {
                        bw.Write(Convert.ToChar(0));
                    }
                    bw.Write(ArchiveFiles1[i].VarName);
                    bw.Write(ArchiveFiles1[i].dbID);
                    bw.Write(fEntry);
                    bw.Write(ArchiveFiles1[i].Size);
                    bw.Write(ArchiveFiles1[i].ChecksumV1);
                    bw.Write(ArchiveFiles1[i].Empty);

                    fEntry += ArchiveFiles1[i].Size;
                    while ((fEntry & 0xF) != 0) { fEntry += 4; }
                    /*
                        public string Name;
                        public string FullPath;
                        public byte[] Padding;
                        public uint VarName;
                        public uint unknown1;
                        public uint Entry;
                        public uint Size;
                        public uint ChecksumV1;
                        public uint Empty;
                        public byte[] Contents; 
                    */
                }

                fEntry = 0;
                for (int i = 0; i < ArchiveHead1.FileCount; i++)
                {
                    bw.Write(ArchiveFiles1[i].Contents);
                    fEntry += ArchiveFiles1[i].Size;
                    while ((fEntry & 0xF) != 0) { fEntry += 4; bw.Write(Convert.ToUInt32(0)); }
                }
                
                bw.Close();
            }
            else if (ArchiveOpen == 2)
            {
                bw.Write(Convert.ToUInt32(0x000000fc));
                bw.Write(ArchiveHead2.unknown1);
                bw.Write(Convert.ToUInt32(0x12345678));
                bw.Write(ArchiveHead2.unknown2);
                bw.Write(ArchiveHead2.unknown3);
                if (ArchiveHead2.Version.Length > 0)
                {
                    bw.Write(ArchiveHead2.Version.ToCharArray());
                    for (int i = (ArchiveHead2.Version.Length + 0x14); i < 0x98; i++)
                        bw.Write(Convert.ToChar(0));
                }
                else
                {
                    for (int i = 0x14; i < 0x98; i += 4)
                        bw.Write(Convert.ToUInt32(0x00000000));
                }

                bw.Write(ArchiveHead2.FileCount);
                bw.Write(Convert.ToUInt32(0x0000005c));

                uint Entry = (ArchiveHead2.FileCount * 0x5c) + 0xa0;
                for (int i = 0; i < ArchiveHead2.FileCount; i++)
                {
                    bw.Write(Convert.ToUInt32(0x0000005c));
                    bw.Write(ArchiveFiles2[i].FullPath.ToCharArray());
                    for (int i2 = ArchiveFiles2[i].FullPath.Length; i2 < 0x40; i2++)
                        bw.Write(Convert.ToChar(0));

                    bw.Write(Entry);
                    bw.Write(ArchiveFiles2[i].Size);

                    bw.Write(Convert.ToUInt32(0x00000000));
                    bw.Write(Convert.ToUInt32(0x00000000));
                    bw.Write(Convert.ToUInt32(0x00000000));
                    bw.Write(Convert.ToUInt32(0x00000000));
                    
                    Entry += ArchiveFiles2[i].Size;
                }
                for (int i = 0; i < ArchiveHead2.FileCount; i++)
                {
                    bw.Write(ArchiveFiles2[i].Contents);
                }

                bw.Close();
            }

            return true;
        }

        public bool SetFileBytes(int index, ref byte[] newData)
        {
            if (ArchiveOpen == 1)
            {
                ArchiveFiles1[index].Contents = new byte[newData.Length];
                ArchiveFiles1[index].Size = Convert.ToUInt32(newData.Length);
                for (int i = 0; i < ArchiveFiles1[index].Size; i++)
                    ArchiveFiles1[index].Contents[i] = newData[i];

                return true;
            }
            else if (ArchiveOpen == 2)
            {
                ArchiveFiles2[index].Contents = new byte[newData.Length];
                ArchiveFiles2[index].Size = Convert.ToUInt32(newData.Length);
                for (int i = 0; i < ArchiveFiles2[index].Size; i++)
                    ArchiveFiles2[index].Contents[i] = newData[i];

                return true;
            }
            
            return false;
        }

        public bool GetFileBytes(int index, out byte[] ret)
        {
            if (ArchiveOpen == 1)
            {
                ret = new byte[ArchiveFiles1[index].Size];
                for (int i = 0; i < ArchiveFiles1[index].Size; i++)
                    ret[i] = ArchiveFiles1[index].Contents[i];

                return true;
            }
            else if (ArchiveOpen == 2)
            {
                ret = new byte[ArchiveFiles2[index].Size];
                for (int i = 0; i < ArchiveFiles2[index].Size; i++)
                    ret[i] = ArchiveFiles2[index].Contents[i];

                return true;
            }

            ret = new byte[0];
            return false;
        }

        public bool ExtractFile(string fPath, int index)
        {
            if (File.Exists(fPath))
                try {
                    File.Delete(fPath);
                }
                catch {
                    return false;
                }
            try
            {
                if (ArchiveOpen == 1)
                    File.WriteAllBytes(fPath, ArchiveFiles1[index].Contents);
                else if (ArchiveOpen == 2)
                    File.WriteAllBytes(fPath, ArchiveFiles2[index].Contents);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public int GetArchiveType() => ArchiveOpen;

        public int GetFileCount()
        {
            if (ArchiveOpen == 1)
                return Convert.ToInt32(ArchiveHead1.FileCount);
            if (ArchiveOpen == 2)
                return Convert.ToInt32(ArchiveHead2.FileCount);

            return 0;
        }

        public string GetDIRListing(string dirPath)
        {
            var dirSP = $"{dirPath}//".Replace(@"\\", @"/")
                .Split(new string[] { @"/" }, StringSplitOptions.RemoveEmptyEntries);

            return GetDIRList(ref dirSP, 0, ref DirectoryListings);
        }
        private string GetDIRList(ref string[] path, int pathI, ref ArchiveFolder folder)
        {
            //Console.WriteLine(folder.Name + " == " + path[pathI] + "; " + (path.Length - 2).ToString() + "; " + pathI.ToString());

            //if (pathI < path.Length - 2)
            //{
                for (int i = 0; i < folder.SubFolderCount; i++)
                {
                    if (folder.SubFolders[i].Name == path[pathI + 1])
                        return GetDIRList(ref path, pathI + 1, ref folder.SubFolders[i]);
                }
            //}
            //Console.WriteLine("2) " + folder.Name + " == " + path[pathI] + "; " + (path.Length - 2).ToString() + "; " + pathI.ToString());

            string listRet = string.Empty;
            if ((pathI == (path.Length - 2)) && (folder.Name == path[pathI]))
            {
                listRet += "[DIR] ." + Convert.ToChar(0x0a);
                listRet += "[DIR] .." + Convert.ToChar(0x0a);

                for (int i = 0; i < folder.SubFolderCount; i++)
                    listRet += "[DIR] " + folder.SubFolders[i].Name + Convert.ToChar(0x0a);
                for (int i = 0; i < folder.FileCount; i++)
                    if (ArchiveOpen == 1) 
                        listRet += "[FILE] " + folder.FileIndexes[i].ToString("X4") + " " + ArchiveFiles1[folder.FileIndexes[i]].Name + Convert.ToChar(0x0a);
                    
                    else if (ArchiveOpen == 2)
                        listRet += "[FILE] " + folder.FileIndexes[i].ToString("X4") + " " + ArchiveFiles2[folder.FileIndexes[i]].Name + Convert.ToChar(0x0a); 

                return listRet;
            }

            listRet += "[DIR] ." + Convert.ToChar(0x0a);
            listRet += "[DIR] .." + Convert.ToChar(0x0a);
            return listRet;
        }

        public string GetFileName(int index)
        {
            if (ArchiveOpen == 1)
                return ArchiveFiles1[index].Name;
            else if (ArchiveOpen == 2)
                return ArchiveFiles2[index].Name;

            return string.Empty;
        }
        

        public string GetFileDetails(int index)
        {
            string details = "";

            if (ArchiveOpen == 1)
            {
                details += "Archive Type 1";
                details += Convert.ToChar(0x0d);
                details += Convert.ToChar(0x0a);
                details += "Version " + ArchiveHead1.ArchiveVersion.ToString();
                details += Convert.ToChar(0x0d);
                details += Convert.ToChar(0x0a);

                details += "Compression Level ";
                if (ArchiveHead1.Compression > 0)
                    details += ArchiveHead1.Compression.ToString();
                else
                    details += "N/A";

                details += Convert.ToChar(0x0d);
                details += Convert.ToChar(0x0a);

                details += Convert.ToChar(0x0d);
                details += Convert.ToChar(0x0a);

                details += "File: " + ArchiveFiles1[index].Name;
                details += Convert.ToChar(0x0d);
                details += Convert.ToChar(0x0a);

                details += "Size: " + ArchiveFiles1[index].Size.ToString() + " bytes";
                details += Convert.ToChar(0x0d);
                details += Convert.ToChar(0x0a);
                details += Convert.ToChar(0x0d);
                details += Convert.ToChar(0x0a);

                details += "Database ID: " + ArchiveFiles1[index].dbID.ToString("X8");
                details += Convert.ToChar(0x0d);
                details += Convert.ToChar(0x0a);

                details += "Var Name: " + HexToStr(HexReverse(ArchiveFiles1[index].VarName.ToString("X8"))).Replace('\0', ' ');
                details += Convert.ToChar(0x0d);
                details += Convert.ToChar(0x0a);

                details += "Checksum: " + ArchiveFiles1[index].ChecksumV1.ToString("X8");
                details += Convert.ToChar(0x0d);
                details += Convert.ToChar(0x0a);

                details += "Uknown2: " + ArchiveFiles1[index].Empty.ToString("X8");
                details += Convert.ToChar(0x0d);
                details += Convert.ToChar(0x0a);

                CA_Checksums CACheckSum = new CA_Checksums();
                uint cs = CACheckSum.GenerateChecksum(ArchiveFiles1[index].Contents, ArchiveFiles1[index].Size);

                details += Convert.ToChar(0x0d);
                details += Convert.ToChar(0x0a);
                details += "Generated Checksum: " + cs.ToString("X8");
                details += Convert.ToChar(0x0d);
                details += Convert.ToChar(0x0a);

                return details;
            }
            else if (ArchiveOpen == 2)
            {
                details += "File: " + ArchiveFiles2[index].Name;
                details += Convert.ToChar(0x0d);
                details += Convert.ToChar(0x0a);

                details += "Size: " + ArchiveFiles2[index].Size.ToString() + " bytes";
                details += Convert.ToChar(0x0d);
                details += Convert.ToChar(0x0a);
                details += Convert.ToChar(0x0d);
                details += Convert.ToChar(0x0a);

                return details;
            }

            return "";
        }

        public void FixAllChecksums()
        {
            if (ArchiveOpen != 1) { return; }

            CA_Checksums CAChecksum = new CA_Checksums();
            for (int i = 0; i < ArchiveHead1.FileCount; i++)
                ArchiveFiles1[i].ChecksumV1 = CAChecksum.GenerateChecksum(ArchiveFiles1[i].Contents, ArchiveFiles1[i].Size);
        }

        private string HexToStr(string hexStr)
        {
            string ret = string.Empty;
            for (int i = 0; i < hexStr.Length; i += 2)
                ret += Convert.ToChar(Convert.ToByte(hexStr.Substring(i, 2), 16));

            return ret;
        }

        private string HexReverse(string hexStr)
        {
            string ret = string.Empty;
            for (int i = 0; i < hexStr.Length; i += 2)
                ret = hexStr.Substring(i, 2) + ret;

            return ret;
        }
    }
}
