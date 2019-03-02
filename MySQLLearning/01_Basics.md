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
