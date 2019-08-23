// DelegateTestCpp.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include "pch.h"
#include <iostream>
#include <map>

using namespace std;

class Delegate {
public:
	multimap<void*, void(*)(void*, int)> fptrs;
	
	void Excute(int addAmount) {
		for (auto f : fptrs) {
			f.second(f.first, addAmount);
		}
	}

	void Add( void (*func)(void*, int), void* ctxt) {
		fptrs.insert(make_pair(ctxt, func));
	}
};

class Test1 {
public:
	int num;
	Test1(int _num) {
		num = _num;
	}
	void testFunc(int add) {
		cout << "Function run " << num + add << endl;
	}
};

class Test2 {
public:
	void testFunc2() {
		cout << "Hey" << endl;
	}
};

void forwarder_Test1(void* context, int addAmount) {
	static_cast<Test1*>(context)->testFunc(addAmount);
}

void forwarder_Test2(void* context, int addAmount) {
	static_cast<Test2*>(context)->testFunc2();
}

int main()
{
	Test1 test1(1);
	Test2 test2;
	Delegate del;

	del.Add(&forwarder_Test1, &test1);
	del.Add(&forwarder_Test2, &test2);
	del.Excute(4);
	return 0;
}
