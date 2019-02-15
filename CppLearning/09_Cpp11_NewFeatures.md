# What's in this note
- [Auto decltype and typeid](#Auto-decltype-and-typeid)
- [Nested template classes]
- [Ring buffer class iterable]
- [Cpp98 and Cpp11 Initialization]
- [Initializer_list]
- [default and delete]
- [Lumbda Expression]
- [Standard function type]
- [Delegating constructor]
- [Copy elision]
- [Constructor and memory]
- [LValue and RValue]
- [Casting]
- [Perfect forwarding]
- [Binding]
- [Unique pointer and shared pointer]
# Auto decltype and typeid
Example:
```C++
#include <iostream>
#include <typeinfo>
using namespace std;

template<class T>
auto Test(T value) -> decltype(value) {
    return value;
}

int get() {
    return 999;
}

auto Test2() -> decltype(get()) {
    return get();
}

int main() {
    string s = "Bob";
    cout << typeid(Test(s)).name() << endl;
    cout << typeid(Test2()).name() << endl;
    return 0;
}
```
If use `typeid()`, `typeinfo` must be included;

`decltype` is very useful when used with `auto` keyword. it returns the type of a value;

`auto Test(T value) -> decltype(value)`, `auto` can be used to find out what type should return, especially useful in template;

`decltype` can also be used to a function;

**`auto` can also be used to simplify collection declaration**  
`auto texts = {"one", "two", "three"};` instead of `string texts[] = {"one", "two", "three"};` this will work!
# Nested template classes
```C++
#include <iostream>
using namespace std;

class ring{
public:
    class iterator;
}

class ring::iterator{
public:
    void print(){
        cout << "Hello from iterator!" <, endl;
    }
}

/*void ring::iterator::print(){
    cout << "Hello from iterator!" <, endl;
}*/*
```
Usually, we don't seperately implement functions in a class, it's not safe, but actually we can do this; 
# Ring buffer class iterable
ring.h:
```C++
#pragma once
template<typename T>
class ring
{
private:
	int m_pos;
	int m_size;
	T* m_values;
public:
	class iterator;

	ring() {}
	ring(int size) :m_pos(0), m_size(size), m_values(NULL){
		m_values = new T[size];
	}

	~ring() {
		delete[] m_values;
	}

	int size() {
		return m_size;
	}

	void add(T value) {
		m_values[m_pos++] = value;
		if (m_pos == m_size) {
			m_pos = 0;
		}
	}

	T& get(int pos) {
		return m_values[pos];
	}

	iterator begin() {
		return iterator(0, *this);
	}
	iterator end() {
		return iterator(m_size, *this);
	}
};


template<typename T>
class ring<T>::iterator {
private:
	int m_pos;
	ring& m_ring;
public:
	iterator(int pos, ring& aRing) :m_pos(pos), m_ring(aRing) {

	}

	iterator& operator++(int) {
		m_pos++;
		return *this;
	}
	iterator& operator++() {
		m_pos++;
		return *this;
	}
	T& operator*() {
		return m_ring.get(m_pos);
	}
	bool operator!=(const iterator& other) const {
		return m_pos != other.m_pos;
	}
};
```
main.cpp:
```C++
#include "stdafx.h"
#include <string>
#include <iostream>
#include "ring.h"
using namespace std;

int main() {
	ring<string> textring(3);

	textring.add("one");
	textring.add("two");
	textring.add("three");
	textring.add("four");

	for (ring<string>::iterator it = textring.begin(); it != textring.end(); it++)
	{
		cout << *it << endl;
	}

	cout << endl;

	for (auto text : textring) {
		cout << text << endl;
	}
	return 0;
}
```
`delete` only frees the memory of a[0], while `delete []` frees all the memories in the collection, it calls the destructor of all the elements in the array, but when using a primitive type as elements, destructor would not be called, the size of element is fixed and the system wll take care of it;

So, when using a class as elements, use `delete []` to free the memories of all elements, but for primitive type `delete` is fine; A template class should use `delete []` as we don't know if it's a primitive type or not;

It seems template can only be used in .h file;
**To Make class iterable**
- Create a nested `template<typename T>` class `iterator` which contain a `int` and a collection reference;
- Overload `operator!=`, `operator*`, `operator++`;
- Create functions named `begin()` and `end()` in the class;
# Cpp98 and Cpp11 Initialization
C++98:
```C++
#include "stdafx.h"
#include <string>
#include <iostream>
using namespace std;

int main() {
	int values[3] = { 1,2,3 };
	cout << values[0] << endl;
	
	class C {
	public:
		int id;
		string text;
	};
	C c1 = {7, "hi"};
	cout << c1.text << endl;

	struct S {
		int id;
		string text;
	};
	S s1 = { 5, "hello" };
	cout << s1.id << endl;

	struct R {
		int id;
		string text;
	} r1 = {6, "sss"};
	cout << r1.text << endl;

	struct{
		int id;
		string text;
	} r2 = { 6, "ooo" };
	cout << r2.text << endl;
	return 0;
}
```
We cannot initialize `vector` or other collections this way in C++98;

C++11:
```C++
#include "stdafx.h"
#include <string>
#include <vector>
#include <iostream>
using namespace std;

int main() {
	int value{ 5 };
	cout << value << endl;
	

	string s{ "hello" };
	cout << s << endl;

	int intarr[]{ 1,2,3 };
	cout << intarr[1] << endl;

	int* pInts = new int[3] {1, 2, 3};
	cout << pInts[1] << endl;

	int value1{};
	cout << value1 << endl;

	int* pSomething{};
	cout << pSomething << endl;

	int a2[3]{};
	cout << a2[1] << endl;

	struct {
		int id;
		string text;
	}s1{7, "t"};
	cout << s1.text << endl;

	vector<string> strings{ "one", "two", "three" };
	cout << strings[1] << endl;

	return 0;
}
```
The `{}` can nearly do any thing when initializing in C++11;

When using this way to initialize a class or a struct, `{}` can only be used for public members;

In C++11, vector and other collections can be initialized using `{}`;
# Initializer_list
```C++
#include "stdafx.h"
#include <string>
#include <initializer_list>
#include <iostream>
using namespace std;

class Test {
public:
    Test(initializer_list<string> l) {
        for (auto i : l){
            cout << i << endl;
        }
    }

    void print(initializer_list<int> l) {
        for (auto i : l) {
            cout << i << endl;
        }
    }
};

int main() {
    Test test{ "one", "two", "three" };

    test.print({ 4, 5, 6 });
    return 0;
}
```
`initializer_list` is iterable, use like above;
# default and delete
**default**

If a class is defined with any constructors, the compiler will not generate a default constructor. This is useful in many cases, but it is some times vexing. For example, we defined the class as below:
```C++
class A
{
public:
    A(int a){};
};
```
Then, if we do:
```C++
A a;
```
The compiler complains that we have no default constructor. That's because compiler did not make the one for us because we've already had one that we defined.

We can force the compiler make the one for us by using default specifier:
```C++
class A
{
public:
    A(int a){}
    A() = default;
};
```
Then, compiler won't complain for this any more:
```C++
A a;
```
**delete**
Suppose we have a class with a constructor taking an integer:
 ```C++
class A
{
public:
    A(int a){};
};
```
Then, the following three operations will be successful:
```C++
A a(10);     // OK
A b(3.14);   // OK  3.14 will be converted to 3
a = b;       // OK  We have a compiler generated assignment operator
```
However, what if that was not we wanted. We do not want the constructor allow double type parameter nor the assignment to work.

C++11 allows us to disable certain features by using delete:
```C++
class A
{
public:
    A(int a){};
    A(double) = delete;         // conversion disabled
    A& operator=(const A&) = delete;  // assignment operator disabled
};
```
Then, if we write the code as below:
```C++
A a(10);     // OK
A b(3.14);   // Error: conversion from double to int disabled
a = b;       // Error: assignment operator disabled
```
In that way, we could achieve what we intended.
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

**Move assignment**  
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
# Casting
```C++
#include <string>
#include <iostream>

using namespace std;

class Parent {
public:
	virtual void Print() {
		cout << "Parent" << endl;
	}
};

class Brother : public Parent{
public:
	void Print() {
		cout << "Brother" << endl;
	}
};

int main() {
	Parent parent;
	Brother brother;
	Parent* pp = &parent;
	Brother* pb = &brother;
	Parent* ppb = &brother;
	Brother* pbb = static_cast<Brother*>(ppb); //Downward casting for pointer is allowed, but unsafe, avoid to use
	Parent* ppb2 = static_cast<Parent*>(pbb); //Upward casting

	Parent&& p = static_cast<Parent&&>(parent); //Cast a LValue to RValue

	Brother bb = static_cast<Brother>(parent); //Downward casting for obejct is not allowed, cause an error
	Parent pb2 = static_cast<Parent>(brother);

	Brother* bb = dynamic_cast<Brother*>(&parent); // invalid casting, return nullptr
}
```
**`static_cast`** can cast pointer upward or downward, but for object downward is not allowed, downward casting for pointer is unsafe, avoid using;

**`dynamic_cast`** can only cast pointers, checks whether this is a upward casting, if yes, return the address, otherwise `nullptr`;

**`retinterpret_cast`** is something that unlikely to be used, it can cast from any to any, which is definitely silly;

**I'd say casting is something so complicated, be careful with it for now, and never use a downward casting**
# Perfect forwarding
```C++
Test test;
auto &&test1 = Test();  //L1
auto &&test2 = test;	//L2
```
The L1 is OK but why is the L2 still compiles without error, that is because some reference collapsing rules inside C++11 says when auto reference boiled down to same lvalue, that it collapses to a lvalue;

So, there will be some bugs in template functions:
```C++
#include <iostream>

using namespace std;

class Test {
};

void check(Test &test) {
	cout << "lvalue" << endl;
}

void check(Test &&test) {
	cout << "rvalue" << endl;
}

template<typename T>
void call(T &&t) {
	check(forward<T>(t)); // forward<>(), does exactly the same with static_cast<>() 
}

int main() {
	Test test;
	call(test);
	call(Test());
}
``` 
`void call(T &&t)` has an internal `auto` keyword, so the argument passed in will always collapse to lvalue, so that `void check(Test &&test)` will never has a chance to be called, unless we use `forward<T>()` to cast it to what it should be; 
# Binding
```C++
#include <iostream>
#include <functional>
using namespace std;
using namespace placeholders;

class Test {
public:
	int add(int a, int b, int c) {
		cout << a << "," << b << "," << c << endl;
		return a + b + c;
	}
};

int add(int a, int b, int c) {
	cout << a << "," << b << "," << c << endl;
	return a + b + c;
}

int run(function<int(int, int)> func) {
	return func(7, 3);
}

int main() {
	auto calculate1 = bind(add, 1, 2, 3);
	cout << calculate1() << endl;

	Test test;
	auto calculate2 = bind(&Test::add, test, _2, _1, 100); //_1, _2 is placeholder, which allows binding function to pass in arguments
	cout << calculate2(4, 5) << endl;

	run(calculate2);

	return 0;
}
```
Include `functional` and `using namespace placeholders;` to use `bind()` and placeholders in it, above shows what we can do with it;

binding a static function is a little more complicated, we should google that afterward;
# Unique pointer and shared pointer
```C++
#include <iostream>
#include <memory>
using namespace std;

class Test {
public:
	Test() {
		cout << "Created" << endl;
	}

	~Test() {
		cout << "Destroyed" << endl;
	}
};

class Temp {
private:
	unique_ptr<Test[]> tests;
public:
	Temp() : tests(new Test[2]) {

	}
};

int main() {
	{
		unique_ptr<Test> test(new Test());
	}
	{
		Temp temp;
	}
	cout << "Finished" << endl;
	cout << endl;

	//shared_ptr<Test> sTest1(new Test());
	shared_ptr<Test> sTest1 = make_shared<Test>();
	{
		shared_ptr<Test> sTest2 = sTest1;
	}
	cout << "Finished 2" << endl;
}
```
- `unique_ptr` and `shared_ptr` is also called smart pointer, to use them, remember to include `<memory>`;
- Before C\++17 shared pointer cannot point to arrays, but after C++17 it is allowed;
- For `shared_ptr`, we can use `make_shared<T>()` instead of `shared_ptr<T> sTest(new T());`
- If a class has a smart pointer as member variable, it should be initialized in constructor;
- Smart pointers automatically deallocate memories when reach the end of the its scope;
- Remember difference between these two type of pointers;
- There is another smart pointer called `auto_ptr`, but it's deprecated, just to know if seeing one;

