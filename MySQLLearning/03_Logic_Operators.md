# Basic operators
**NOT** `NOT`		  
**Equal and Not Equal** `=` and `!=`  
**Greater than and Less than** `>`, `>=`, `<`, `<=`  
**And Or** `AND` `OR`  
**Between** `... where salary BETWEEN 6000 AND 8000;` Note this includes 6000 and 8000;  
**In** `... where salary IN(6000, 7000, 8000);`  

# CASE statement
Example 1:  
```MySQL
SELECT *,
	case
		when salary>=7000 then "high"
		when salary>=6000 then "medium"
		else "low"
	end as tag
from employee order by salary desc;
```
Example 2:
```MySQL
SELECT *,
	case
		when title like "%Engineer%" then 1
		when salary like "%Architect%" then 2
		else 3
	end as tag
from employee order by tag;
```