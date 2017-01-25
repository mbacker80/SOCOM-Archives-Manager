using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOCOM_Archives
{
    class CA_Archive
    {
        private Header_Type1 ArchiveHead1; private File_Type1[] ArchiveFiles1;
        private Header_Type2 ArchiveHead2; private File_Type2[] ArchiveFiles2;
        private string ArchiveBrowsePath; private int ArchiveOpen; private ArchiveFolder DirectoryListings;

        public struct Header_Type1
        {
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
        }
        private struct File_Type1
        {
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
        }

        private struct Header_Type2
        {
            public uint MainHeaderSize;
            public uint unknown1;
            public uint unknownID;
            public uint unknown2;
            public uint unknown3;
            public string Version;
            public uint[] padding;

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
            if (System.IO.File.Exists(fName) == false) { return -1; }

            try
            {
                fData = System.IO.File.ReadAllBytes(fName);
            }
            catch
            {
                return -1;
            }
            

            return LoadArchive(fData);
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
                ArchiveHead1.unknown1 = BitConverter.ToUInt32(fData, 8);
                ArchiveHead1.ArchiveHeaderSize = BitConverter.ToUInt32(fData, 0xc);
                ArchiveHead1.BodySize = BitConverter.ToUInt32(fData, 0x10);
                ArchiveHead1.unknownID = BitConverter.ToUInt32(fData, 0x14);
                ArchiveHead1.empty = BitConverter.ToUInt32(fData, 0x18);
                ArchiveHead1.unknown2 = BitConverter.ToUInt32(fData, 0x1c);
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
                    ArchiveFiles1[i].unknown1 = BitConverter.ToUInt32(fData, Convert.ToInt32(FileHeadEntry + 0x44));
                    ArchiveFiles1[i].Entry = BitConverter.ToUInt32(fData, Convert.ToInt32(FileHeadEntry + 0x48));
                    ArchiveFiles1[i].Size = BitConverter.ToUInt32(fData, Convert.ToInt32(FileHeadEntry + 0x4c));
                    ArchiveFiles1[i].ChecksumV1 = BitConverter.ToUInt32(fData, Convert.ToInt32(FileHeadEntry + 0x50));
                    ArchiveFiles1[i].Empty = BitConverter.ToUInt32(fData, Convert.ToInt32(FileHeadEntry + 0x54));
                    ArchiveFiles1[i].Contents = new byte[ArchiveFiles1[i].Size];

                    for (i2 = 0; i2 < ArchiveFiles1[i].Size; i2++)
                    {
                        ArchiveFiles1[i].Contents[i2] = fData[ArchiveFiles1[i].Entry + ArchiveHead1.ArchiveHeaderSize + i2];
                    }

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
            {
                totalFiles = ArchiveHead1.FileCount;
            }
            else if (ArchiveOpen == 2)
            {
                totalFiles = ArchiveHead2.FileCount;
            }
            else
            {
                return;
            }

            DirectoryListings = new ArchiveFolder();
            DirectoryListings.Name = "z:";

            for (int i = 0; i < totalFiles; i++)
            {
                string[] subs;
                string tmpDIR = "";
                string tmpFName = "";
                if (ArchiveOpen == 1) { tmpDIR = ArchiveFiles1[i].FullPath; tmpFName = ArchiveFiles1[i].Name; }
                if (ArchiveOpen == 2) { tmpDIR = ArchiveFiles2[i].FullPath; tmpFName = ArchiveFiles2[i].Name; }

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
        private void AddDIRListing(string[] fPath, int fIndex, string fName, ref ArchiveFolder CurrentDIR, int FileIndex)
        {
            string debugStr = "";
            for (int i = 0; i < CurrentDIR.SubFolderCount; i++)
            {
                if (CurrentDIR.SubFolders[i].Name == fPath[fIndex])
                {
                    if (fIndex == fPath.Length - 1)
                    {
                        for (int i2 = 0; i2 < fIndex; i2++)
                        {
                            debugStr += fPath[i2] + "/";
                        }
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
                CurrentDIR.FileCount++;
                Array.Resize(ref CurrentDIR.FileIndexes, CurrentDIR.FileCount);
                CurrentDIR.FileIndexes[CurrentDIR.FileCount - 1] = FileIndex;
                return;
            }

            CurrentDIR.SubFolderCount++;
            Array.Resize(ref CurrentDIR.SubFolders, CurrentDIR.SubFolderCount);

            CurrentDIR.SubFolders[CurrentDIR.SubFolderCount-1].Name = fPath[fIndex];

            int subI = CurrentDIR.SubFolderCount - 1;
            AddDIRListing(fPath, fIndex + 1, fName, ref CurrentDIR.SubFolders[subI], FileIndex);
        }
        
        public bool ExtractFile(string fPath, int index)
        {
            if (System.IO.File.Exists(fPath))
            {
                try { System.IO.File.Delete(fPath); } catch { return false; }
            }

            try
            {
                if (ArchiveOpen == 1)
                {
                    System.IO.File.WriteAllBytes(fPath, ArchiveFiles1[index].Contents);
                }
                else if (ArchiveOpen == 2)
                {
                    System.IO.File.WriteAllBytes(fPath, ArchiveFiles2[index].Contents);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public int GetArchiveType()
        {
            return ArchiveOpen;
        }

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
            string dirTmp = dirPath + "/";
            dirTmp = dirTmp.Replace('\\', '/');
            dirTmp = dirTmp.Replace("//", "/");
            string[] dirSP = dirTmp.Split('/');

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
                    {
                        return GetDIRList(ref path, pathI + 1, ref folder.SubFolders[i]);
                    }
                }
            //}
            //Console.WriteLine("2) " + folder.Name + " == " + path[pathI] + "; " + (path.Length - 2).ToString() + "; " + pathI.ToString());

            string listRet = "";
            if ((pathI == (path.Length - 2)) && (folder.Name == path[pathI]))
            {
                listRet += "[DIR] ." + Convert.ToChar(0x0a);
                listRet += "[DIR] .." + Convert.ToChar(0x0a);
                for (int i = 0; i < folder.SubFolderCount; i++)
                {
                    listRet += "[DIR] " + folder.SubFolders[i].Name + Convert.ToChar(0x0a);
                }
                for (int i = 0; i < folder.FileCount; i++)
                {
                    if (ArchiveOpen == 1) { listRet += "[FILE] " + folder.FileIndexes[i].ToString("X4") + " " + ArchiveFiles1[folder.FileIndexes[i]].Name + Convert.ToChar(0x0a); }
                    else if (ArchiveOpen == 2) { listRet += "[FILE] " + folder.FileIndexes[i].ToString("X4") + " " + ArchiveFiles2[folder.FileIndexes[i]].Name + Convert.ToChar(0x0a); }
                }
                return listRet;
            }

            listRet += "[DIR] ." + Convert.ToChar(0x0a);
            listRet += "[DIR] .." + Convert.ToChar(0x0a);
            return listRet;
        }

        public string GetFileName(int index)
        {
            if (ArchiveOpen == 1)
            {
                return ArchiveFiles1[index].Name;
            }
            else if (ArchiveOpen == 2)
            {
                return ArchiveFiles2[index].Name;
            }

            return "";
        }
        

        public string GetFileDetails(int index)
        {
            string details = "";

            if (ArchiveOpen == 1)
            {
                details += "File: " + ArchiveFiles1[index].Name;
                details += Convert.ToChar(0x0d);
                details += Convert.ToChar(0x0a);

                details += "Size: " + ArchiveFiles1[index].Size.ToString() + " bytes";
                details += Convert.ToChar(0x0d);
                details += Convert.ToChar(0x0a);
                details += Convert.ToChar(0x0d);
                details += Convert.ToChar(0x0a);

                details += "Uknown1: " + ArchiveFiles1[index].unknown1.ToString("X8");
                details += Convert.ToChar(0x0d);
                details += Convert.ToChar(0x0a);

                details += "Var Name: " + HexToStr(HexReverse(ArchiveFiles1[index].VarName.ToString("X8")));
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

        private string HexToStr(string hexStr)
        {
            string ret = "";
            for (int i = 0; i < hexStr.Length; i += 2)
            {
                ret += Convert.ToChar(Convert.ToByte(hexStr.Substring(i, 2), 16));
            }

            return ret;
        }

        private string HexReverse(string hexStr)
        {
            string ret = "";
            for (int i = 0; i < hexStr.Length; i += 2)
            {
                ret = hexStr.Substring(i, 2) + ret;
            }

            return ret;
        }
    }
}
