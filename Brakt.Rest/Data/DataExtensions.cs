using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace Brakt.Rest.Data
{
    public static class DataExtensions
    {
        internal static byte ToBit(this bool boolean)
        {
            if (boolean) return 1;

            return 0;
        }

        internal static bool ToBool(this byte bit)
        {
            if (bit == 1) return true;
            else if (bit == 0) return false;

            throw new ArgumentException("bit value should be 1 or 0");
        }

        internal static byte[] ToBlob<T>(this T obj)
        {
            var formatter = new BinaryFormatter();

            using var stream = new MemoryStream();

            formatter.Serialize(stream, obj);
            return stream.ToArray();
        }

        internal static T FromBlob<T>(this byte[] bytes)
        {
            using var stream = new MemoryStream();

            var formatter = new BinaryFormatter();

            stream.Write(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);

            return (T)formatter.Deserialize(stream);
        }

        internal static object DbNullIfNull(this object obj)
        {
            if (obj == null) return DBNull.Value;

            return obj;
        }
    }
}
