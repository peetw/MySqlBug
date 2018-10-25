# MySqlBug

Demonstrates the steps necessary to reproduce MySQL [Bug #86974](https://bugs.mysql.com/bug.php?id=86974).

**NOTE: THIS BUG IS NOW FIXED AS OF v6.10.8 AND v8.0.13**

## Description

When trying to create an instance of the MySqlGeometry class for an empty geometry collection, using the following constructor:

    public MySqlGeometry(MySqlDbType type, byte[] val) {...}

It fails with the a ArgumentOutOfRangeException.

This is due to the fact that the MySQL WKB representation for an empty geometry collection is only 13 bytes in length:

    SELECT HEX(ST_GeometryCollectionFromText("GEOMETRYCOLLECTION()"));

    00-00-00-00-01-07-00-00-00-00-00-00-00

Whereas the MySqlGeometry class assumes that the minimum length of the WKB that it will receive is 21 bytes (WKB for a POINT (25 bytes) minus the 4 bytes from the MySQL-specific SRID extension). Thus causing it to try and read from beyond the end of the byte array and resulting in the observed ArgumentOutOfRangeException.
