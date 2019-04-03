# Setup for C++ development in VS
- Add following directories to Additional include:  
  - `C:\Program Files\MySQL\Connector C++ 8.0\include`  
  - `C:\Program Files\MySQL\MySQL Server 8.0\include`

- Add following directories to Linker\General\Additional library directories:
  - `C:\Program Files\MySQL\MySQL Server 8.0\lib`  
- Add `libmysql.lib` to Linker\Input\Additional Dependencies;
  - Copy libmysql.dll from `C:\Program Files\MySQL\MySQL Server 8.0\lib` to the executable directory;
  - Copy `libeay32.dll` and `ssleay32.dll` from `C:\Program Files\MySQL\MySQL Server 8.0\bin` to the executable directory;

# Essential example
```C++
#include "pch.h"
#include <iostream>
#include <mysql.h>

using namespace std;

int main()
{
	MYSQL* conn;
	MYSQL_RES* res;
	MYSQL_ROW row;
	conn = mysql_init(0);
	conn = mysql_real_connect(conn, "localhost", "root", "tmwfiawc", "test", 3306, NULL, 0);

	if (conn) {
		cout << "success connected to database" << endl;
	}
	else {
		cout << "not success" << endl;
	}

	string query = "SELECT * FROM testtable";
	const char* q = query.c_str();

	int qstate = mysql_query(conn, q);
	if (!qstate) {
		res = mysql_store_result(conn);
		while (row = mysql_fetch_row(res)) {
			printf("name: %s, age: %s\n", row[0], row[1]);
		}
	}
	else {
		cout << "query failed: " << mysql_error(conn) << endl;
	}

}
``` 