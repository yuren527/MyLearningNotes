There are 3 types of data in MySQL:  
**Numeric**  
**Date and Time**  
**String**
# NUMERIC
**Integer**(Exact)  
INT, SMALLINT, TINYINT, MEDIUMINT, BIGINT  
INT UNSIGNED, SMALLINT UNSIGNED, TINYINT UNSIGNED, MEDIUMINT UNSIGNED, BIGINT UNSIGNED  
**Fixed-Point**(Exact)  
DECIMAL, NUMERIC  
**Floating-Point**(Approximate)  
FLOAT, DOUBLE  
**Bit**  
BIT: *One byte is 8 bit*

**Integer type value range and storage erquired**  

|Type|Storage(Bytes)|Value Range Signed|Value Range Unsigned|  
|---|---|  
|TINYINT|1|-128~127|0~255|
|SMALLINT|2|-32768~32767|0~65535|
|MEDIUMINT|3|-8388608~8388607|0~16777215|
|INT|4|-2147483648~2147483647|0~4294967295|
|BIGINT|8|-2<sup>63</sup>~2<sup>63</sup>-1|0~2<sup>64</sup>-1|  

**In MySQL, NUMERIC type is implemented as DECIMAL, so NUMERIC is the same with DECIMAL**  
To declare a data using DECIMAL, should specify the number of digit:  
`create table(price DECIMAL(5, 2));`This means the price data can have 5 digits at max, including two digits after the point; So the value range of price is -999.99 ~ 999.99; If assigning a number which has more digits after the point, it will be round off; If the number is out of range, there will be a error;  

The required storage of DECIMAL depends on the number of digits spcified when delaring, for DERCIMAL(M,D), if M>D, the it takes M+2 bytes, otherwise D+2 bytes;

**Storage of float is 4 bytes and double is 8 bytes**  

**Bit**  
`BIT(M)` to use a bit type, M can range from 1 to 64, default is 1;

As default, MySQL can display binary number using `SELECT`:  
- use `a+0` to translate binary a to a decimal number;
- `bin(a+0)` to translate a decimal to a binary;   
- `hex(a+0)` to translate a decimal to a hexadecimal;
- `oct(a+0)` to translate a decimal to a octanary;
- use `b'1111'` to indicate that this is a binary data when inserting data;  
# DATE and TIME
**DATE**
> INSERT INTO test(_date) values('2018-10-10');	  
> INSERT INTO test(_date) values('20181010');  
> INSERT INTO test(_date) values(20181010);

All three syntaxes of inserting DATE does the same thing;   
DATE can be NULL, but must explicitly assigned as follow:  
> INSERT INTO test(_date) values(NULL);  
> 
**TIME**  
TIME is a little complicated when using different insert syntax:  
```
mysql> Insert into test(t) values('12:10:10');
Query OK, 1 row affected (0.01 sec)

mysql> Insert into test(t) values('12:10');
Query OK, 1 row affected (0.01 sec)

mysql> Insert into test(t) values(1210);
Query OK, 1 row affected (0.01 sec)

mysql> Insert into test(t) values(12);
Query OK, 1 row affected (0.01 sec)

mysql> Insert into test(t) values('12');
Query OK, 1 row affected (0.01 sec)
```
Outputs:
```
mysql> select * from test;
+----------+
| t        |
+----------+
| 12:10:10 |
| 12:10:00 |
| 00:12:10 |
| 00:00:12 |
| 00:00:12 |
+----------+
```

**YEAR**  
Ussaully we use a 4-digit number or string, YEAR data range from 1901 to 2155;  
If use a 1 or 2-digit, things become much more complicated:  
- As a 1 or 2-digit number, it ranges from 1 to 99, and when it's in range of 1 to 69, the result is 2001 to 2069, but it reasults in 1970 to 1999 when it's in range of 70 to 99;  
- As a 1 or 2-digit string, it ranges from 0 to 99, and when it's in range of 0 to 69, the result is 2000 to 2069, but it reasults in 1970 to 1999 when it's in range of 70 to 99;  
- So if we want 2000 in 1 or 2-digit, make it '0' or '00';
- If insert a 0 as number, it will results in a internal value of 0000;

**DATETIME and TIMESTAMP**  
DATATIME needs 8 byte for storage while TIMESTAMP needs only 4 bytes, and TIMESTAMP is much faster the DATETIME for index;  

Different range size
- TIMESTAMP: 1970-01-01 00:00:00 - 2038-01-09 -0:14:07;
- DATETIME: 1000-01-01 00:00:00 - 9999-12-31 23:59:59;

TIMESTAMP changes automatically when the mysql timezone changed, while DATETIME does not change;

Auto update and default time function of TIMESTAMP:
> mysql> create table time_test(a DATETIME, b TIMESTAMP default now() on update now());
> Query OK, 0 rows affected (0.02 sec)  

Outputs:  
> +-------+-----------+------+-----+-------------------+-----------------------------------------------+
> | Field | Type      | Null | Key | Default           | Extra                                         |
> +-------+-----------+------+-----+-------------------+-----------------------------------------------+
> | a     | datetime  | YES  |     | NULL              |                                               |
> | b     | timestamp | YES  |     | CURRENT_TIMESTAMP | DEFAULT_GENERATED on update CURRENT_TIMESTAMP |
> +-------+-----------+------+-----+-------------------+-----------------------------------------------+
> 2 rows in set (0.00 sec)  

So ,if we want anything automatically changes when data changed, use `ON UPDATE`;  
`NOW()` can be used to get current system time;  

**timezone**  
`show variables;` can be used to display system variables;  
Use `show variables like "%time_zone%";` to find variables relative to time zone;  
`set time_zone="-12:00";` change time zone 12 hours backward;  
`set time_zone=system;`  

**DATE and TIME functions**  
- `NOW()` return current DATETIME;  
- `CURDATE()` return current DATE;
- `CURTIME()` return current TIME;  
- `CURRENT_TIMESTAMP()` return current TIMESTAMP; 
- `DAY()` synonym for `DAYOFMONTH()`;
- `DAYNAME(d DATE)` return the name of the weekday of d;
- `DAYOFMONTH(d DATE)` 1-31;
- `DAYOFYEAR(d DATE)` 1-366;
- `DAYOFWEEK(d DATE)` 1-7;(Sunday is the first day of a week) 
- `MONTH(d DATE)` return the month index of a date;
- `MONTNAME(d DATE)` return the month name of a date;
- `DATE_FORMAT(d DATETIME)` format d into some format: [documents of DATE_FORMAT()](https://dev.mysql.com/doc/refman/5.7/en/date-and-time-functions.html#function_date-format)  
An example:  
>  mysql> select concat_ws(" ", first_name, last_name, "was hired on", DATE_FORMAT(hire_date, '%D %M %Y')) as info from employee order by hire_date;  
```
+-------------------------------------------------+
| info                                            |
+-------------------------------------------------+
| Eliza Clifford was hired on 19th October 1998   |
| Harley Gilbert was hired on 17th July 2000      |
| Robin Jackman was hired on 12th October 2001    |
| Taylor Edward was hired on 21st September 2002  |
| Nancy Newman was hired on 23rd January 2007     |
| Vivian Dickens was hired on 29th August 2012    |
| Melinda Clifford was hired on 29th October 2013 |
| Harry Clifford was hired on 10th December 2015  |
| Jack Chan was hired on 7th September 2018       |
+-------------------------------------------------+
9 rows in set (0.00 sec)
```

[Function Documents](https://dev.mysql.com/doc/refman/5.7/en/date-and-time-functions.html)
# String
**CHAR and VARCHAR**  
- `CHAR` is fixed length string(0-255);
- `VARCHAR` is variable length string(0-65535);

|Value|CHAR(4)|Storage Required|VARCHAR(4)|Storage Required|
|---|---|---|---|---|
|''|'&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;'|4 bytes|''|1 byte|
|'ab'|'ab&nbsp;&nbsp;&nbsp;&nbsp;'|4 bytes|'ab'|3 bytes|
|'abcd'|'abcd'|4 bytes|'abcd'|5 bytes|
|'abcdefg'|'abcd'|4 bytes|'abcd'|5 bytes|

`VARCHAR` needs one extra byte to store the length of data;  
`VARCHAR` use less storage than `CHAR` in most situation, but it may be slower because sometimes it needs more operation such as requiring more storage when updating data;    

**BINARY and VARBINARY**  
[Documents](https://dev.mysql.com/doc/refman/5.7/en/binary-varbinary.html)  
These two types are rarely used; The most different between BINARY-VARBINARY and CHAR-VARCHAR is: when delaring a `CHAR` or `VARCHAR` using `CHAR(4)`, that means the length of string is 4, but for `BINARY` and `VARBINARY`, it means the bytes it needs to storage is 4;  

**BLOB and TEXT**														   
`BLOB` and `TEXT` are large objects; BLOB is treated as binary objects;  
[Documents](https://dev.mysql.com/doc/refman/5.7/en/blob.html)

**ENUM and SET**  
`ENUM` is a simple type which no need to say;  

`SET` is somhow similar to `ENUM`, `SET` can have up to 8 elements(up tp 64 bytes storage), and its instance value can be zero or any combinations of its elments; The values are stored as bits, for example `SET('one', 'two', 'trhee')`:  

|Decimal|Binary|Value|
|---|---|---|
|0|000|""|
|1|001|one|
|2|010|two|
|3|011|one, two|
|4|100|three|
|5|101|one, three|
|6|110|two, three|
|7|111|one, two, three|  

# CONVERT()
Convert something into another type:  
```
mysql> SELECT CONVERT(7.4323453453453453, DECIMAL(2, 1));
+--------------------------------------------+
| CONVERT(7.4323453453453453, DECIMAL(2, 1)) |
+--------------------------------------------+
|                                        7.4 |
+--------------------------------------------+
1 row in set (0.00 sec)
```