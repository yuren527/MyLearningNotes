# Basics
## Database level commands
> show databases;  
> create database <name\>;  
> drop database <name\>;  
> use <database name\>;  
> select database();  
## Delimister
`;` is delimiter, which is used to excute commands, without it the command won't execute; We can use `delimister` to modify the symbol of delimister:  
> delimister && //no former delimister
## Use upper case for keywords
It is recommended to use upper case for keywords, as it's more obvious when reading:
> CREATE DATABASE mydatabase;  
## Table data types
There are many types in database table, but they can be sorted into three categories: Numeric, String, Date;  
For more information, check the MySQL docs: [MySQL data type reference](https://dev.mysql.com/doc/refman/5.7/en/data-type-overview.html);
## Table commands
> CREATE TABLE <name\>(...);
```MySQL
CREATE TABLE person(
name VARCHAR[20],
phone VARCHAR[20],
age INT
);
```
Other table commands:  
> show tables;  
> show columns from <table_name\>;  
> desc <table_name>;  
> drop table<table_name\>;
## Insert data to table
```MySQL
INSERT INTO person(name, phone, age)
VALUES("Zhang san", "12345", 20),
      ("Li si", "67890", 30);
```
## Select
Select is a very important keyword in MySQL;
Basic use syntax:
> select * from <table_name\>; //print all columns of table;  
> select <column_name\> from <table_name\>; //print a column of table;  
> select <column1_name\>, <column2_name\> from <table_name\>; //print two columns of table;

We can use `as` to specify a "nick name" for the columns:
> select <column_name\> as <short_name\> from <table_name\>;
## NULL and NOT NULL and DEFAULT
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
## PRIMARY KEY, AUTO_INCREMENT and UNIQUE
### PRIMARY KEY
If set a column as `PRIMARY KEY` as below, then its value cannot be `NULL`:
> create table person(id INT PRIMARY KEY, name VARCHAR(20));  
### AUTO_INCREMENT
but if we don't want to give a specific value every time we insert, we can mark `id` as `AUTO_INCREMENT`;
> create table person(id INT PRIMARY KEY AUTO_INCREMENT, name VARCHAR(20));

then it will increment the value of `id` when we leave it blank when inserting data; **But, `AUTO_INCREMENT` must be used with `PRIMARY KEY` or `UNIQUE` key**;  
### United key
If we need some united key, such as `GUID`, we can mark multiple columns as `PRIMARY KEY`, use symtax as below:
> create table person(id1 INT, id2 INT, name VARCHAR(20), PRIMARY KEY(id1, id2));  

Each key of a united key can be duplicate, as long as its union is unique; for example: 1-2 can exist with 1-1, but another 1-2 is not allowed;
### UNIQUE
`UNIQUE` values can be `NULL`, but if it's not `NULL`, it has to be unique;  
## Where, NOT, and, or
> select * from <table_name\> where <condition\>;    

Above means return datas when data meets condition following `where`, use `and`, `or` to form multiple conditions;  
`Not` means when condition not met:  
> select * from <table_name\> where NOT <condition\>  

condition syntax: `first_name="John"`;
## Update data through UPDATE
> update emplyee set salary=20000, note="updated" where title="Software Architect";  

Change salary to 20000, and set note to "updated", when title of the data is "Software Architect"; 
**Note: use `set` without `where` is dangerous, it will change all the datas;**
## DELETE and IF NOT EXISTS
We can use `DELETE` to delete datas from a table, and also dangerous when using it without `where` condition as that will delete all the datas in the table;  
> DELETE from <table_name\> WHERE <condition\>  

If want to check for existance or duplicate name of a table before creating one, use `IF NOT EXISTS` after `CREATE TABLE`:
> CREATE TABLE IF NOT EXISTS emplyees(...);
## SOURCE <path\>
Import executions from file:
> SOURCE /Users/PengXiao/test.sql;

