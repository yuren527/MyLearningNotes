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
