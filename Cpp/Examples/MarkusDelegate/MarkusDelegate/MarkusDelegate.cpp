// MarkusDelegate.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include "MarkusDelegate.h"

using namespace Markus;

int TestFunc1(int a, int b) {
	cout << "This is test func 1" << endl;
	return a + b;
}

class TestClass {
public:
	int TestMemFunc1(int a, int b) {
		cout << "This is member test func 1" << endl;
		return a + b;
	}
};

int main()
{
	TestClass testObj;

	MarkusDelegate<int, int, int> testDelegate;
	testDelegate.Add(&TestFunc1);
	testDelegate.Add<TestClass>(&testObj, &TestClass::TestMemFunc1);
	cout << testDelegate.GetDelegateNum() << endl;
	testDelegate.Invoke(1, 2);
	testDelegate.Remove(&TestFunc1);
	testDelegate.Remove<TestClass>(&testObj, &TestClass::TestMemFunc1);
	cout << testDelegate.GetDelegateNum() << endl;
	testDelegate.Invoke(1, 2);
	return 0;
}


