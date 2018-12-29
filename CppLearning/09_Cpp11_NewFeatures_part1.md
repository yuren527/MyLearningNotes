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
# C\++98 and C++11 Initialization
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
# initializer_list
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