**Example:**
```C++
#include <iostream>
#include <stdarg.h>

int Sum(int num, ...) {
	va_list lst;
	int sum = 0;
	va_start(lst, num);
	for (int i = 0; i < num; i++) {
		int n = va_arg(lst, int);
		sum += n;
	}
	va_end(lst);
	return sum;
}


int main()
{
    std::cout << "Hello World!\n";
	int s = Sum(6, 1, 2, 3, 4, 5, 6);
	std::cout << s << std::endl;
}

```
- First, include `<stdarg.h>`;
- Declare a `va_list` to store the arguments;
- A int argument should be passed in, to determine the number of the arguments; Or, there should be some other way to do so; otherwise, unexpected result will come up;
- Use `va_start(...)` to initialize the `va_list`; And `va_arg()` to iterate the arguments, usually in a loop; Then `va_end()` to finalize;
