# FOREIGN KEY
Syntax:
```sql
CREATE TABLE orders(
	id INT AUTO_INCREMENT PRIMARY KEY,
	order_date DATE,
	amount DECIMAL(8, 2),
	customer_id INT,
	FOREIGN KEY(customer_id) REFERENCES customers(id)
);
```
Declare customer_id as a foreign key, which references the id column in customers, any foreign key value that cannot be found in referencing column will not be accepted; So the value is constrained; 

# INNER JOIN
composite a table using two tables which are related; Syntax:
```
mysql> select * from customers inner join orders where customers.id=orders.customer_id;
+----+------------+-----------+----------------+----+------------+--------+-------------+
| id | first_name | last_name | email          | id | order_date | amount | customer_id |
+----+------------+-----------+----------------+----+------------+--------+-------------+
|  1 | Robin      | Jackman   | roj@gmail.com  |  1 | 2001-10-12 |  99.12 |           1 |
|  2 | Taylor     | Edward    | taed@gmail.com |  2 | 2001-09-21 | 110.99 |           2 |
|  1 | Robin      | Jackman   | roj@gmail.com  |  3 | 2001-10-13 |  12.19 |           1 |
|  3 | Vivian     | Dickens   | vidi@gmail.com |  4 | 2001-11-29 |  88.09 |           3 |
|  4 | Harley     | Gilbert   | hgi@gmail.com  |  5 | 2001-11-11 | 205.01 |           4 |
+----+------------+-----------+----------------+----+------------+--------+-------------+
5 rows in set (0.00 sec)
```
Inner join take datas that are related, unrelated datas will not be selected;

# LEFT JOIN and RIGHT JOIN
```
mysql> select * from customers left join orders on customers.id=orders.customer_id;
+----+------------+-----------+------------------+------+------------+--------+-------------+
| id | first_name | last_name | email            | id   | order_date | amount | customer_id |
+----+------------+-----------+------------------+------+------------+--------+-------------+
|  1 | Robin      | Jackman   | roj@gmail.com    |    1 | 2001-10-12 |  99.12 |           1 |
|  2 | Taylor     | Edward    | taed@gmail.com   |    2 | 2001-09-21 | 110.99 |           2 |
|  1 | Robin      | Jackman   | roj@gmail.com    |    3 | 2001-10-13 |  12.19 |           1 |
|  3 | Vivian     | Dickens   | vidi@gmail.com   |    4 | 2001-11-29 |  88.09 |           3 |
|  4 | Harley     | Gilbert   | hgi@gmail.com    |    5 | 2001-11-11 | 205.01 |           4 |
|  5 | a          | b         | yuren527@163.com | NULL | NULL       |   NULL |        NULL |
+----+------------+-----------+------------------+------+------------+--------+-------------+
6 rows in set (0.00 sec)
```
`LEFT JOIN` and `RIGHT JOIN` takes the interaction of two tables, and the whole left or right table; 
 
Use `ON` instead of `WHERE` in left and right join, because it is a little different from inner join, that the datas taken from left or right table do not need to meet the filter conditions;  

 # IFNULL()
`IFNULL(value1, value2);`  
If `value1` equals `NULL`, then return `value` instead;
```
mysql> select first_name, last_name, amount from customers left join orders on customers.id=orders.customer_id;
+------------+-----------+--------+
| first_name | last_name | amount |
+------------+-----------+--------+
| Robin      | Jackman   |  99.12 |
| Taylor     | Edward    | 110.99 |
| Robin      | Jackman   |  12.19 |
| Vivian     | Dickens   |  88.09 |
| Harley     | Gilbert   | 205.01 |
| a          | b         |   NULL |
+------------+-----------+--------+
6 rows in set (0.00 sec)

mysql> select first_name, last_name, IFNULL(amount, 0) from customers left join orders on customers.id=orders.customer_id;
+------------+-----------+-------------------+
| first_name | last_name | IFNULL(amount, 0) |
+------------+-----------+-------------------+
| Robin      | Jackman   |             99.12 |
| Taylor     | Edward    |            110.99 |
| Robin      | Jackman   |             12.19 |
| Vivian     | Dickens   |             88.09 |
| Harley     | Gilbert   |            205.01 |
| a          | b         |              0.00 |
+------------+-----------+-------------------+
6 rows in set (0.00 sec)
```

# ON DELETE CASCADE
If a data is referenced by some datas in other tables, the referenced data will be unable to be deleted, as this operation will lead to a error in other tables. Because of it, If we need to delete a data which is referenced by other, we should declare the referencing data as `ON DELETE CASCADE`, then when we delete the referenced data, this referencing datas will also be deleted, this make the referenced data deletable; **This is sometimes dangerous, because deleting referenced data will also delete all the referencing data;**

Syntax:
```sql
CREATE TABLE orders(
	id INT AUTO_INCREMENT PRIMARY KEY,
	order_date DATE,
	amount DECIMAL(8, 2),
	customer_id INT,
	FOREIGN KEY(customer_id)
		REFERENCES customers(id)
		ON DELETE CASCADE
);
```

# An example for many-to-many relationship
Tables:  
```
mysql> desc books;
+---------------+--------------+------+-----+---------+----------------+
| Field         | Type         | Null | Key | Default | Extra          |
+---------------+--------------+------+-----+---------+----------------+
| id            | int(11)      | NO   | PRI | NULL    | auto_increment |
| title         | varchar(100) | NO   |     | NULL    |                |
| released_year | year(4)      | NO   |     | NULL    |                |
| language      | varchar(100) | NO   |     | NULL    |                |
| paperback     | int(11)      | NO   |     | NULL    |                |
+---------------+--------------+------+-----+---------+----------------+
5 rows in set (0.00 sec)

mysql> desc reviewers;
+------------+--------------+------+-----+---------+----------------+
| Field      | Type         | Null | Key | Default | Extra          |
+------------+--------------+------+-----+---------+----------------+
| id         | int(11)      | NO   | PRI | NULL    | auto_increment |
| first_name | varchar(100) | YES  |     | NULL    |                |
| last_name  | varchar(100) | YES  |     | NULL    |                |
+------------+--------------+------+-----+---------+----------------+
3 rows in set (0.00 sec)

mysql> desc reviews;
+-------------+--------------+------+-----+---------+----------------+
| Field       | Type         | Null | Key | Default | Extra          |
+-------------+--------------+------+-----+---------+----------------+
| id          | int(11)      | NO   | PRI | NULL    | auto_increment |
| rating      | decimal(2,1) | YES  |     | NULL    |                |
| book_id     | int(11)      | YES  | MUL | NULL    |                |
| reviewer_id | int(11)      | YES  | MUL | NULL    |                |
+-------------+--------------+------+-----+---------+----------------+
4 rows in set (0.00 sec)
```
Practices:  
```
mysql> select books.title, CONVERT(AVG(reviews.rating), 
DECIMAL(3, 2)) as avg_rating 
from books left join reviews 
on books.id=reviews.book_id group by books.title order by avg_rating desc;
+---------------------------------------------------------------+------------+
| title                                                         | avg_rating |
+---------------------------------------------------------------+------------+
| Minna No Nihongo: Beginner 1, 2nd Edition                     |       9.90 |
| Collection Folio, no. 2                                       |       9.40 |
| The Fault in Our Stars                                        |       9.36 |
| Santo remedio: Ilustrado y a color                            |       8.60 |
| Harry Potter Und der Stein der Weisen (German Edition)        |       8.12 |
| Fifty Shades of Grey Series                                   |       8.12 |
| Civilian Publishing Alif Baa Taa: Learning My Arabic Alphabet |       8.08 |
| Santo Remedio                                                 |       7.94 |
| The Hunger Games (Book 3)                                     |       7.52 |
| Splatoon 2                                                    |       5.38 |
+---------------------------------------------------------------+------------+
10 rows in set (0.00 sec)

mysql> select reviewers.first_name, reviewers.last_name, 
IFNULL(CONVERT(AVG(reviews.rating), DECIMAL(3, 2)), 0.00) as avg_rating 
from reviewers left join reviews 
on reviewers.id=reviews.reviewer_id group by reviewers.id;
+------------+-----------+------------+
| first_name | last_name | avg_rating |
+------------+-----------+------------+
| Thomas     | Stoneman  |       8.02 |
| Wyatt      | Skaggs    |       7.71 |
| Kimbra     | Masters   |       7.86 |
| Domingo    | Cortes    |       7.78 |
| Colt       | Steele    |       8.77 |
| Pinkie     | Petit     |       7.25 |
| Marlon     | Crafford  |       0.00 |
+------------+-----------+------------+
7 rows in set (0.00 sec)

mysql> select first_name, last_name, COUNT(reviews.id) as COUNT, 
IFNULL(MIN(reviews.rating), 0.0) as MIN, IFNULL(MAX(reviews.rating), 0.0) as MAX, 
IFNULL(CONVERT(AVG(reviews.rating), DECIMAL(3, 2)), 0.00) as AVG, 
CASE when COUNT(reviews.id)=0 then "INACTIVE" else "ACTIVE" 
end as STATUS 
from reviewers left join reviews 
on reviewers.id=reviews.reviewer_id group by reviewers.id order by COUNT desc;
+------------+-----------+-------+-----+-----+------+----------+
| first_name | last_name | COUNT | MIN | MAX | AVG  | STATUS   |
+------------+-----------+-------+-----+-----+------+----------+
| Colt       | Steele    |    10 | 4.5 | 9.9 | 8.77 | ACTIVE   |
| Domingo    | Cortes    |     8 | 5.8 | 9.1 | 7.78 | ACTIVE   |
| Wyatt      | Skaggs    |     8 | 5.5 | 9.3 | 7.71 | ACTIVE   |
| Kimbra     | Masters   |     7 | 6.8 | 9.0 | 7.86 | ACTIVE   |
| Thomas     | Stoneman  |     5 | 7.0 | 9.5 | 8.02 | ACTIVE   |
| Pinkie     | Petit     |     4 | 4.3 | 8.8 | 7.25 | ACTIVE   |
| Marlon     | Crafford  |     0 | 0.0 | 0.0 | 0.00 | INACTIVE |
+------------+-----------+-------+-----+-----+------+----------+
7 rows in set (0.00 sec)
```