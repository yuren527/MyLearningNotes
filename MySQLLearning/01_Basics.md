- [Database and Table level commands](#Database-and-Table-level-commands)  
- [Tips](#Tips)  
- [SELECT](#SELECT)  
- [NULL and NOT NULL and DEFAULT](#NULL-and-NOT-NULL-and-DEFAULT)  
- [PRIMARY KEY and AUTO_INCREMENT and UNIQUE](#PRIMARY-KEY-and-AUTO_INCREMENT-and-UNIQUE)  
- [WHERE NOT AND OR](#WHERE-NOT-AND-OR)  
- [UPDATE SET](#UPDATE-SET)  
- [DELETE and IF NOT EXISTS](#DELETE-and-IF-NOT-EXISTS)  
- [MySQL string functions](#MySQL-string-functions)
- [Refining SELECT](#Refining-SELECT)
- [GROUP BY and Aggregation](#GROUP-BY-and-Aggregation)

# Database and Table level commands
**Database level commands**
> show databases;  
> create database <name\>;  
> drop database <name\>;  
> use <database name\>;  
> select database();  

**Table level commands**
> show tables;  
> show columns from <table_name\>;  
> desc <table_name>;  
> drop table<table_name\>;

**Create table**
```sql
CREATE TABLE person(
name VARCHAR[20],
phone VARCHAR[20],
age INT
);
```
**Insert data to table**
```sql
INSERT INTO person(name, phone, age)
VALUES("Zhang san", "12345", 20),
      ("Li si", "67890", 30);
```
# Tips
**MySQL language is case-insensitive**, there is a way to make it case-sensitive, google it if nessessary;

**Delimister**  
`;` is delimiter, which is used to excute commands, without it the command won't execute; We can use `delimister` to modify the symbol of delimister:  
> delimister && //no former delimister
> 
**Use upper case for keywords**
It is recommended to use upper case for keywords, as it's more obvious when reading:
> CREATE DATABASE mydatabase;  

**Table data types**
There are many types in database table, but they can be sorted into three categories: Numeric, String, Date;  
For more information, check the MySQL docs: [MySQL data type reference](https://dev.mysql.com/doc/refman/5.7/en/data-type-overview.html);

**SOURCE <path\>**  
Import executions from file:
> SOURCE /Users/PengXiao/test.sql;

# Encoding
**Create default utf8 datebase**  
> CREATE DATABASE dbname DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

**Alter existing database to utf8**
> ALTER DATABASE dbname CHARSET utf8 COLLATE utf8_general_ci;

**Alter existing table to utf8**
> ALTER TABLE tablename CONVERT TO CHARACTER SET utf8;

changing database encoding charset can only affect newly created tables, alter existing table using the third command;  

# SELECT
`SELECT` is a very important keyword in MySQL;
Basic use syntax:
> select * from <table_name\>; //print all columns of table;  
> select <column_name\> from <table_name\>; //print a column of table;  
> select <column1_name\>, <column2_name\> from <table_name\>; //print two columns of table;

We can use `as` to specify a "nick name" for the columns:
> select <column_name\> as <short_name\> from <table_name\>;
# NULL and NOT NULL and DEFAULT
If create a table like this:
> create table person1(name varchar(20, phone varchar(20), age int);  
> desc person1;  

we will get output:  
```
mysql> desc person1;
+-------+-------------+------+-----+---------+-------+
| Field | Type        | Null | Key | Default | Extra |
+-------+-------------+------+-----+---------+-------+
| name  | varchar(20) | YES  |     | NULL    |       |
| phone | varchar(20) | YES  |     | NULL    |       |
| age   | int(11)     | YES  |     | NULL    |       |
+-------+-------------+------+-----+---------+-------+
3 rows in set (0.00 sec)
```
From above we can see, as default, values can be `NULL`, and the default value is `NULL`;
If we want a value cannot be NULL, and set its default value to other value:
> create table person2(name varchar(20) NOT NULL DEFAULT "no", phone varchar(20), age int);  

If we set a column to `NOT NULL`, then we have to set a value to it when we insert a new row, other than leave it blank, or change its default value to something else;
# PRIMARY KEY and AUTO_INCREMENT and UNIQUE
**PRIMARY KEY**  
If set a column as `PRIMARY KEY` as below, then its value cannot be `NULL`:
> create table person(id INT PRIMARY KEY, name VARCHAR(20));  

**AUTO_INCREMENT**  
but if we don't want to give a specific value every time we insert, we can mark `id` as `AUTO_INCREMENT`;
> create table person(id INT PRIMARY KEY AUTO_INCREMENT, name VARCHAR(20));

then it will increment the value of `id` when we leave it blank when inserting data; **But, `AUTO_INCREMENT` must be used with `PRIMARY KEY` or `UNIQUE` key**;  

**United key**  
If we need some united key, such as `GUID`, we can mark multiple columns as `PRIMARY KEY`, use symtax as below:
> create table person(id1 INT, id2 INT, name VARCHAR(20), PRIMARY KEY(id1, id2));  

Each key of a united key can be duplicate, as long as its union is unique; for example: 1-2 can exist with 1-1, but another 1-2 is not allowed;  

**UNIQUE**   
 	
`UNIQUE` values can be `NULL`, but if it's not `NULL`, it has to be unique;  
# WHERE NOT AND OR
> select * from <table_name\> where <condition\>;    

Above means return datas when data meets condition following `where`, use `and`, `or` to form multiple conditions;  
`Not` means when condition not met:  
> select * from <table_name\> where NOT <condition\>  

condition syntax: `first_name="John"`;
# UPDATE SET
> update emplyee set salary=20000, note="updated" where title="Software Architect";  

Change salary to 20000, and set note to "updated", when title of the data is "Software Architect"; 
**Note: use `set` without `where` is dangerous, it will change all the datas;**
# DELETE and IF NOT EXISTS
We can use `DELETE` to delete datas from a table, and also dangerous when using it without `where` condition as that will delete all the datas in the table;  
> DELETE from <table_name\> WHERE <condition\>  

If want to check for existance or duplicate name of a table before creating one, use `IF NOT EXISTS` after `CREATE TABLE`:
> CREATE TABLE IF NOT EXISTS emplyees(...);
# MySQL string functions
**CONCAT** and **CONCAT_WS**  
append two or more strings together:  
`CONCAT("A", "B");` outputs: `AB`;  
`CONCAT_WS("-", "A", "B");` outputs: `A-B`;  

**SUBSTRING**  
`SUBSTRING("Hello World", 1, 5);` outputs: `Hello`;  
`SUBSTRING("Hello World", 7);` outputs: `World`; Counting from beginning;    
`SUBSTRING("Hello World", -3);` outputs: `rld`; which means count from the end;  

**REPLACE**  
`REPLACE("Hello World", "World", "MySQL");` outputs: `Hello MySQL`; Replace the second parameter string in the first parameter, with the third;  

**REVERSE**  
Reverse the string;  

**CHAR_LENGTH**  
Return the length of a string;  

**UPPER** and **LOWER**  
Turn the string into upper or lower case, has no effect on int and symbols; 
# Refining SELECT
**ORDER BY**  
To order selection by column(s); *Use after `where` syntax*;  
`select title, title_year, gross, imdb_score from movie where director_name="peter Jackson" order by gross desc limit 1;`   
`desc` stands for descendant, order from biger to smaller;  

 column name can be replaced by index of the column names in the selection:  
 `select title, title_year, gross, imdb_score from movie where director_name="peter Jackson" order by 3 desc limit 1;` `3` stands for `gross` as it's the third in the selections;     

`order by 1, 2` and `order by title, title_name` means use multiple columns to order, 1 as primary while 2 as secondary;  

**LIMIT**  
***Use above example***  
To list certain number of selections;  
`LIMIT 1, 2` means list 2 datas starting from the second one(index starts from 0, so 1 is actually the second);  

**LIKE**  
`where director_name like "%C%"`  
condition that any director_name has a "C" in it;
- `"C%"` means starts with "C" followed by any string of any length;  
- `"%C%"` means any string of any length that contains a "C";  
- `"_C%"` "_" means this position is one and only one letter that don't know what is it;   
- `"____"` (four "_") means this is a string which has four letters;
- **CONCLUSION** "\_" and "%" both means something arbitary, but "\_" is one letter while "%" is a string(of any length);  

If a string contains "%" or "\_" that makes it ambiguous when using `like` syntax, then andd a "\\" before it to translate the symbol, just the same with md language;
# GROUP BY and Aggregation
**MAX MIN SUM AVG**  
syntax: 
> MAX(salary)  
> MIN(salary)  
> SUM(salary)  
> AVG(salary)  

*MAX and MIN* can be used for numeric, string and date type, while *SUM and AVG* can only be used for numeric and date;

**COUNT**  
return the number of a column;

**DISTINCT**  
return the number of unique data of a column;

**GROUP BY**  
Use to group datas, syntax:  
> mysql> select title, count(*), sum(salary), avg(salary) from employee group by title order by sum(salary);   
 
outputs:  
```
+------------------------+----------+-------------+-------------------+
| title                  | count(*) | sum(salary) | avg(salary)       |
+------------------------+----------+-------------+-------------------+
| Test Engineer          |        1 |        6500 |              6500 |
| Project Manager        |        1 |        8500 |              8500 |
| Database Administrator |        2 |       12800 |              6400 |
| Software Architect     |        2 |       15200 |              7600 |
| Software Engineer      |        3 |       15350 | 5116.666666666667 |
+------------------------+----------+-------------+-------------------+
```
**HAVING**
`where` can only used to filter raw datas;
> select * from employee group by title where title="software engineer";  
> select * from employee where title = "software engineer" group by title;  

The first line will report a syntax error while the second line execute successfully;  
That means we can only filter datas before group them;  

If we want to filter datas after group, use `HAVING`:  
> select * from employee group by title having title="software engineer";
