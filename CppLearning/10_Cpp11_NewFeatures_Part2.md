# Lumbda Expression
Lumbda Expression ia actually a anonymous function which contains only a few code;

`[](){};` This is a valid lumbda expression which can compile;

Let's see how to use it:
```C++
void test(void (*pfunc)()) {
	pfunc();
}

int main() {
	auto func = []() {cout << "hello" << endl; };

	func();

	test(func);

	test([]() {cout << "hello" << endl; });

	return 0;
}
```
**This is lumbda expression with parameters:**
```C++
auto runDivide(double (*divide)(double, double)) {
	return divide(10, 5);
}

int main() {
	auto pDivide = [](double a, double b) -> double{
		if (b == 0.0) {
			return 0;
		}
		return a / b;
	};

	cout << pDivide(9, 3) << endl;

	cout << runDivide(pDivide) << endl;
	return 0;
}
```
When return type is ambiguous, a trailer return type should be used to indicate return type;

Notice the `void (*func)()` syntax;

**lumbda capture expressions**
```C++
#include "stdafx.h"
#include <string>
#include <iostream>
using namespace std;

class Test {
private:
	int one{ 1 };
	int two{ 2 };

public:
	void test1() {
		int three{ 3 };
		int four{ 4 };

		//pass three by value
		auto pfunc1 = [three]() {
			//three = 33;   pass by value, so this cannot be modified
			cout << three << endl;
		};
		pfunc1();

		//default pass local variables by value but three by reference
		auto pfunc2 = [=, &three]() {
			three = 33;
			cout << three << endl;
		};
		pfunc2();
		cout << three << endl;

		//default pass local variables by reference, but three by value
		auto pfunc3 = [&, three]() {
			//three = 33;
			cout << three << endl;
		};
		pfunc3();

		//pass instance variables by reference;
		auto pfunc4 = [this, three]() {
			one = 11;
		};
		pfunc4();
		cout << one << endl;

		//default pass local variables by value and pass instance variables by ref
		auto pfunc5 = [=, this]() {
			int _one = one;
		};
		pfunc5();
		cout << one << endl;

	}
};

int main() {
	Test test;
	test.test1();
	
	return 0;
};
```
`this` instance variables cannot be passed by value directly;

Variables passed by value cannot be changed in lambda expression, except we use `mutable` keyword like this:
```C++
[one]() mutable {
	one = 8;
cout << one << endl;
}
```
# Standard function type
Below shows how to use a lambda expression, function pointer, functor, and standard function type to pass function to function:
```C++
#include "stdafx.h"
#include <string>
#include <vector>
#include <iostream>
#include <algorithm>
#include <functional>
using namespace std;

bool func(string& text) {
	return text.size() == 4;
}

class Functor {
public:
	bool operator()(const string& text) const {
		return text.size() == 4;
	}
} functor;

int main() {
	int size{ 4 };
	auto lambda = [size](string& text) {return text.size() == size; };

	vector<string> texts{ "one", "two", "three", "four", "five" };
	int count;

	count = count_if(texts.begin(), texts.end(), lambda);
	cout << count << endl;

	count = count_if(texts.begin(), texts.end(), func);
	cout << count << endl;

	count = count_if(texts.begin(), texts.end(), functor);
	cout << count << endl;

	function<int(int, int)> add = [](int a, int b) {return a + b; };
	cout << add(4, 5) << endl;
}
```
# Delegating constructor
```C++
#include "stdafx.h"
#include <string>
#include <iostream>

using namespace std;

int main() {
	class Parent {
		int id{ 5 };
		string text{ "default text" };
	public:
		Parent(string text) : text(text) {
			cout << "parent constructor takes parameter" << endl;
		}
		Parent() : Parent("hello") {
			cout << "parent constructor takes no parameter" << endl;
		}
	};

	class Child : public Parent {
	public:
		Child() {
			cout << "child constructor takes no parameter" << endl;
		}
	};

	Child child;
	return 0;
}
```
We can call constructor in another constructor, this is called delegating constructor; This feature only works in C\++11, but will not work in C++98; 
# Copy elision
We can turn off copy elision by give a flag `-fno-elide-constructors` in somewhere of the compiler if the compiler surports turning off	if;
```C++
#include <string>
#include <iostream>

using namespace std;

class Test {
public:
	Test() {
		cout << "constructor" << endl;
	}

	Test(const Test& test) {
		cout << "copy constructor" << endl;
	}

	~Test() {
		cout << "destructor" << endl;
	}
};

Test getTest() {
	return Test();
}

int main() {
	Test test = getTest();
	return 0;
}
```
Output:
>constructor     
>destructor

This is because of the elision constructor, if had it turned off, we can see a bunch of constructors and copy constructors, as the function of `getTest()` will not be elided;

# Constructor and memory
```C++
class Test {
private:
	const int SIZE{ 100 };
	int* _pbuffer{nullptr};
public:
	Test() {
		cout << "constructor" << endl;
		_pbuffer = new int[SIZE] {};
		//memset(_pbuffer, 0, SIZE * sizeof(int)); another way to initialize _pbuffer that wealth knowing
	}

	Test(int value) {
		cout << "parameterized constructor" << endl;
		_pbuffer = new int[SIZE] {};
		for (int i = 0; i < SIZE; i++) {
			_pbuffer[i] = 7 * value;
		}
	}

	Test(const Test& other) {
		cout << "copy constructor" << endl;
		_pbuffer = new int[SIZE] {};
		memcpy(_pbuffer, other._pbuffer, SIZE * sizeof(int));
	}

	~Test() {
		cout << "destructor" << endl;
		delete[] _pbuffer;
	}

	Test& operator=(const Test& other) {
		cout << "Assignment" << endl;
		_pbuffer = new int[SIZE] {};
		memcpy(_pbuffer, other._pbuffer, SIZE * sizeof(int));
		return *this;
	}
};
```
**Two useful functions:**
- `memset(_pubuffer, 0, SIZE * sizeof(int))`;
- `memcpy(_pbuffer, other._pbuffer, Size * sizeof(int))`;
# LValue and RValue
- LValue is some variable you can take an address of;
- RValue is some temporary value that returns from a function or some primitive value which has no variable name, which we cannot get an address from;
```C++
int value = 7;
int* pvalue1 = &value;
int* pvalue2 = &7; //Error, cannot take adrress of 7;
Test* test1 = &test;
Test* test2 = &getTest(); //Error, getTest() is a RValue;
int* pvalue3 = &++value;
int* pvalue4 = &value++; //Error;
int* s = &(7 + value); //Error, RValue;
```
**Prefix increment is a LValue while postfix increment is a RValue;**  
the `value` in a postfix increment is a temporary value that copied from the original variable to be used to do increment;  

Although, we can combine RValue to a LValue in some way:  
`const Test& test = getTest();` a const variable makes the RValue's life time extended to the end of scope;  
That means, we can use RValue as reference parameter in a copy constructor or some other functions:  
```C++
Test(const Test& other){...}
Test test(Test(1));
``` 
**`&&test` does not means reference to reference of `test`, but means a reference to a RValue;**
```C++
Test&& test = getTest(); 
```
`getTest()` is a return value that we normally cannot take address of, but with `&&` we can do this;
**There is a way to check if a value is LValue or RValue:**
```C++
void check(Test& test){
	cout << "this is a LValue" << endl;
}
void check(Test&& test){
	cout << "this is a RValue" << endl;
}
```
using these overloaded functions, we can find out whether a parameter is LValue or RValue;
```C++
check(test); //LValue
check(getTest()); //RValue
check(Test()); //RValue
```
**Move constructor**   
A copy constructor can use RValue as parameter because it's a const parameter, but that still needs to allocate extra memory;  
With move constructor we can steal the resources from the RValue which has done most of the hard work of allocating;
```C++
Test(Test&& other){
	_pbuffer = other._pbuffer;
}
```
Now we can directly change the pointer to pointing to RValue without allocating memory;  
But this will cause some problem:
```C++
~Test(){
	delete[] _pbuffer;
}  
```
This is the destructor, it frees the memory we allocated as to avoid memory leaking;  
When `other` is destructed, it also frees the memory that `this.pbuffer` pointing to, as we stole the resource of other, pointing to the same memory address;
So, to do this correctly:
```C++
Test(Test&& other){
	_pbuffer = other._pbuffer;
	other.pbuffer = nullptr;
}
```
It will be OK to free `nullptr`;
# Move assignment
Like the move constructor, move assignment is more efficient than normal assignment;  
See normal assignment in [Constructor and memory];	  
The move assignment:  
```C++
Test& operator=(Test&& other){
	delete[] _pbuffer;
	_pbuffer = other._pbuffer;
	other._pbuffer = nullptr;
	return *this;
}
```
Be careful to remember to free the memory that should be freed: `delete[] _pbuffer;`, and same with move constructor, make sure it don't get freed when it should not: `other._pbuffer = nullptr;`

