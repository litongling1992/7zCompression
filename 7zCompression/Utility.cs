using System;
using System.IO;

namespace _7zCompression
{
    public class Utility
    {
        /// <summary>
        /// 调用7Z的压缩数据接口
        /// </summary>
        /// <param name="inpbuf">将要压缩的二进制数据</param>
        /// <returns></returns>
        public static byte[] LzmaCompress(byte[] inpbuf)
        {
            try
            {
                CoderPropID[] propIDs =
                {
                    CoderPropID.DictionarySize,
                    CoderPropID.PosStateBits,
                    CoderPropID.LitContextBits,
                    CoderPropID.LitPosBits,
                    CoderPropID.Algorithm,
                    CoderPropID.NumFastBytes,
                    CoderPropID.MatchFinder,
                    CoderPropID.EndMarker
                };
                object[] properties =
                {
                    23,
                    2,
                    3,
                    2,
                    1,
                    128,
                    "bt4",
                    true
                };
                var enc = new LZMA.Encoder();
                enc.SetCoderProperties(propIDs, properties);
                //压缩的数据文件
                var msInp = new MemoryStream(inpbuf);
                //输出的压缩数据
                var msOut = new MemoryStream();
                enc.WriteCoderProperties(msOut);
                enc.Code(msInp, msOut, -1, -1, null);
                return msOut.ToArray();
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 解压数据
        /// </summary>
        /// <param name="inpbuf">已经压缩的数据</param>
        /// <returns></returns>
        public static byte[] LzmaDecompress(byte[] inpbuf)
        {
            try
            {
                var dec = new LZMA.Decoder();
                byte[] prop = new byte[5];
                Array.Copy(inpbuf, prop, 5);
                dec.SetDecoderProperties(prop);
                MemoryStream msInp = new MemoryStream(inpbuf);
                msInp.Seek(5, SeekOrigin.Current);
                MemoryStream msOut = new MemoryStream();
                dec.Code(msInp, msOut, -1, -1, null);
                return msOut.ToArray();
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 将路径下的文件转为byte[]数据
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static byte[] GetDataByFilePath(string filePath)
        {
            try
            {
                FileStream fs = new FileStream(filePath, FileMode.Open);
                byte[] byteData = new byte[fs.Length];
                fs.Read(byteData, 0, byteData.Length);
                fs.Close();
                return byteData;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}

