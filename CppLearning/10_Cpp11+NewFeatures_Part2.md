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