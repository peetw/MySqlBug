using System;
using MySql.Data.MySqlClient;
using MySql.Data.Types;

namespace MySqlBug
{
    public static class Program
    {
        /// <summary>
        /// Demonstrates the steps necessary to reproduce MySQL Bug #86974
        /// See: https://bugs.mysql.com/bug.php?id=86974
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            // MySQL WKB representation of an empty GeometryCollection
            // As reported by:
            //  SELECT HEX(ST_GeometryCollectionFromText("GEOMETRYCOLLECTION()"));
            // Output:
            //  00-00-00-00-01-07-00-00-00-00-00-00-00
            var bytes = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x01, 0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            // Attempt to initialize MySqlGeometry with empty GeometryCollection
            // Will fail with System.ArgumentOutOfRangeException
            try
            {
                var mySqlGeometry = new MySqlGeometry(MySqlDbType.Geometry, bytes);
                Console.WriteLine("WKT: {0}", mySqlGeometry.GetWKT());
            }
            catch(ArgumentOutOfRangeException ex)
            {
                Console.WriteLine("Failed to initialize MySqlGeometry:\n\n{0}", ex);
            }
        }
    }
}
