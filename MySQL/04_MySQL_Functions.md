# String functions
**CONCAT()**  

**CONCAST_WS()**  

**LOWER(), UPPER()**  

**LEFT(), RIGHT()**  
```
mysql> select LEFT('MySQL', 2), RIGHT('MySQL', 2);
+------------------+-------------------+
| LEFT('MySQL', 2) | RIGHT('MySQL', 2) |
+------------------+-------------------+
| My               | QL                |
+------------------+-------------------+
```

**LENGTH()**  

**LTRIM(), RTRIM()** Get rid of space from a string starts from left or right;  
```  
mysql> select length(ltrim('  MySQL'));
+--------------------------+
| length(ltrim('  MySQL')) |
+--------------------------+
|                        5 |
+--------------------------+
1 row in set (0.00 sec)

mysql> select length(rtrim('MySQL  '));
+--------------------------+
| length(rtrim('MySQL  ')) |
+--------------------------+
|                        5 |
+--------------------------+
1 row in set (0.00 sec)
```

**TRIM()** Get rid of something at left or right or both side from a string; Default to get rid of space;   
```
mysql> select trim('  My SQL  ');
+--------------------+
| trim('  My SQL  ') |
+--------------------+
| My SQL             |
+--------------------+
1 row in set (0.00 sec)

mysql> Select trim(leading '&' from '&&My SQL&&');
+-------------------------------------+
| trim(leading '&' from '&&My SQL&&') |
+-------------------------------------+
| My SQL&&                            |
+-------------------------------------+
1 row in set (0.00 sec)

mysql> Select trim(trailing '&' from '&&My SQL&&');
+--------------------------------------+
| trim(trailing '&' from '&&My SQL&&') |
+--------------------------------------+
| &&My SQL                             |
+--------------------------------------+
1 row in set (0.00 sec)
```

**REPLACE()** Replace some chars with some other chars;  
```
mysql> select replace('&&My&SQL&&', '&', '$');
+---------------------------------+
| replace('&&My&SQL&&', '&', '$') |
+---------------------------------+
| $$My$SQL$$                      |
+---------------------------------+
1 row in set (0.00 sec)
```

**SUBSTRING()**

# Numeric functions  
**FLOOR() and CEIL()**

**DIV() and MOD()** DIV(): Return the integer of a division calculation; MOD(): % operator;  

**POWER(a, b)**

**ROUND(a float, b int)** The real round-off operation, b is how many digit to keep after the point;

# Date and Time  Functions
**NOW(), CURDATE(), CURTIME()**

**DATE_FORMAT()**
```
mysql> select date_format(curdate(), '%m/%d/%y');
+------------------------------------+
| date_format(curdate(), '%m/%d/%y') |
+------------------------------------+
| 03/08/19                           |
+------------------------------------+
1 row in set (0.00 sec)
```

**DATE_ADD()**
```
mysql> SELECT DATE_ADD('2010-01-01', INTERVAL 10 DAY);
+-----------------------------------------+
| DATE_ADD('2010-01-01', INTERVAL 10 DAY) |
+-----------------------------------------+
| 2010-01-11                              |
+-----------------------------------------+
1 row in set (0.00 sec)

mysql> SELECT DATE_ADD('2010-01-01', INTERVAL -10 WEEK);
+-------------------------------------------+
| DATE_ADD('2010-01-01', INTERVAL -10 WEEK) |
+-------------------------------------------+
| 2009-10-23                                |
+-------------------------------------------+
1 row in set (0.00 sec)

mysql> SELECT DATE_ADD('2010-01-01', INTERVAL 10 MONTH);
+-------------------------------------------+
| DATE_ADD('2010-01-01', INTERVAL 10 MONTH) |
+-------------------------------------------+
| 2010-11-01                                |
+-------------------------------------------+
1 row in set (0.00 sec)

mysql> SELECT DATE_ADD('2010-01-01', INTERVAL 10 YEAR);
+------------------------------------------+
| DATE_ADD('2010-01-01', INTERVAL 10 YEAR) |
+------------------------------------------+
| 2020-01-01                               |
+------------------------------------------+
1 row in set (0.00 sec)
```
 
**DATEDIFF()**
```
mysql> SELECT DATEDIFF('2018-1-1', '2018-3-1');
+----------------------------------+
| DATEDIFF('2018-1-1', '2018-3-1') |
+----------------------------------+
|                              -59 |
+----------------------------------+
1 row in set (0.00 sec)
```

# Information functions
**CONNECTION_ID()** ID of connection of client;  

**DATABASE()** current database;  

**LAST_INSERT_ID()** ID of last inserted data; If multiple datas inserted at same time, return the first inserted ID;


**USER()** return current user name;

**VERSION()** return version of MySQL;

# Aggregation functions
**AVG()**

**COUNT()**

**MAX()**

**MIN()**

**SUM()**

# Encryption
**Change password**  
`SET PASSWORD=PASSWORD('abc123');`

**Show warnings**  
`Show warnings;`

**access mysql with username and password**  
`mysql -u <username> -p<passworld>` Note: no `;`

**MD5() and sha1()** two encryption algorithms;
```
mysql> create table users(username VARCHAR(100) NOT NULL, password VARCHAR(100) NOT NULL);
Query OK, 0 rows affected (0.03 sec)

mysql> desc users;
+----------+--------------+------+-----+---------+-------+
| Field    | Type         | Null | Key | Default | Extra |
+----------+--------------+------+-----+---------+-------+
| username | varchar(100) | NO   |     | NULL    |       |
| password | varchar(100) | NO   |     | NULL    |       |
+----------+--------------+------+-----+---------+-------+
2 rows in set (0.00 sec)

mysql> INSERT INTO users VALUES('test', MD5('testpass'));
Query OK, 1 row affected (0.01 sec)

mysql> select * from users;
+----------+----------------------------------+
| username | password                         |
+----------+----------------------------------+
| test     | 179ad45c6ce2cb97cf1029e212046e81 |
+----------+----------------------------------+
1 row in set (0.00 sec)
```
Usage of `sha1()` is the same;