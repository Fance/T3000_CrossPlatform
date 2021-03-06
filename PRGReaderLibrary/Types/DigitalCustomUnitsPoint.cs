namespace PRGReaderLibrary
{
    using System;
    using System.Collections.Generic;

    public class CustomDigitalUnitsPoint : Version, IBinaryObject, ICloneable
    {
        public bool Direct { get; set; }
        public string DigitalUnitsOff { get; set; }
        public string DigitalUnitsOn { get; set; }

        public bool IsEmpty =>
            string.IsNullOrWhiteSpace(DigitalUnitsOff) &&
            string.IsNullOrWhiteSpace(DigitalUnitsOn);

        public CustomDigitalUnitsPoint(bool direct = false,
            string digitalUnitsOff = "",
            string digitalUnitsOn = "",
            FileVersion version = FileVersion.Current)
            : base(version)
        {
            Direct = direct;
            DigitalUnitsOff = digitalUnitsOff;
            DigitalUnitsOn = digitalUnitsOn;
        }

        #region Binary data

        public static int GetCount(FileVersion version = FileVersion.Current)
        {
            switch (version)
            {
                case FileVersion.Current:
                    return 8;

                default:
                    throw new FileVersionNotImplementedException(version);
            }
        }

        public static int GetSize(FileVersion version = FileVersion.Current)
        {
            switch (version)
            {
                case FileVersion.Dos:
                case FileVersion.Current:
                    return 25;

                default:
                    throw new FileVersionNotImplementedException(version);
            }
        }

        /// <summary>
        /// FileVersion.Current - Need 25 bytes
        /// FileVersion.Dos - Need 25 bytes
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        /// <param name="version"></param>
        public CustomDigitalUnitsPoint(byte[] bytes, int offset = 0, FileVersion version = FileVersion.Current)
            : base(version)
        {
            switch (FileVersion)
            {
                case FileVersion.Dos:
                case FileVersion.Current:
                    Direct = bytes.ToByte(ref offset).ToBoolean();
                    DigitalUnitsOff = bytes.GetString(ref offset, 12).ClearBinarySymvols();
                    DigitalUnitsOn = bytes.GetString(ref offset, 12).ClearBinarySymvols();
                    break;

                default:
                    throw new FileVersionNotImplementedException(FileVersion);
            }

            CheckOffset(offset, GetSize(FileVersion));
        }

        /// <summary>
        /// FileVersion.Current - 25 bytes
        /// FileVersion.Dos - 25 bytes
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            var bytes = new List<byte>();

            switch (FileVersion)
            {
                case FileVersion.Dos:
                case FileVersion.Current:
                    bytes.Add(Direct.ToByte());
                    bytes.AddRange(DigitalUnitsOff.ToBytes(12));
                    bytes.AddRange(DigitalUnitsOn.ToBytes(12));
                    break;

                default:
                    throw new FileVersionNotImplementedException(FileVersion);
            }

            CheckSize(bytes.Count, GetSize(FileVersion));

            return bytes.ToArray();
        }

        #endregion

        public object Clone() => new CustomDigitalUnitsPoint(Direct, DigitalUnitsOff, DigitalUnitsOn, FileVersion);
    }
}