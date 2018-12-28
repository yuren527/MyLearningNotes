# Function Pointer
Example:
```C++
#include <iostream>
#include <vector>
#include <algorithm>
using namespace std;

bool Match(string text) {
    return text.size() == 3;
}

bool MatchN(string text, int n) {
    return text.size() == n;
}

int CountString(vector<string> texts, int n, bool (*func)(string, int))
{
    int tally = 0;
    for (int i = 0; i < texts.size(); i++) {
        if (func(texts[i], n)){
            tally++;
        }
    }
    return tally;
}

int main(){
    vector<string> texts;
    texts.push_back("one");
    texts.push_back("two");
    texts.push_back("three");
    texts.push_back("four");
    texts.push_back("five");

    cout << count_if(texts.begin(), texts.end(), Match) << endl;
    cout << CountString(texts, 4, MatchN) << endl;
    return 0;
}
```
# Abstract classes and pure virtual functions
Example:
```C++
#include <iostream>
#include <vector>
using namespace std;

class Animal {
public:
	virtual void Speak() = 0;
	virtual void Run() = 0;
};

class Dog : public Animal{
public:
	virtual void Speak() {
		cout << "Woof!" << endl;
	}
};

class Labrado : public Dog {
public:
	void Run() {
		cout << "Labrado running" << endl;
	}
};

int main() {
	Animal *animals[5];
	//Animal animals[5];
	vector<Animal> animals_vec;

	//Dog dog;
	Labrado lab;
	lab.Run();
	lab.Speak();
	return 0;
}
```
`Vitual void Speak() = 0;` This means this function is a pure virtual function. A class that contains any pure virtual function is a abstact class;

`Dog` implement one of the pure virtual functions in `Animal`, but not all pure virtual functions are implemented, so `Dog` is also a abstract class;

`Labrado` finally implemented all the pure virtual functions, so `Labrado` is a concrete class;

Abstract classes cannot be instantiated, so arrays of abstract objects are not allowed because the constructor of abstract class cannot be called, but array of pointers to abstract objects or vector of abstract objects is allowed, because they only store pointers, so are the other similar collections;
# Functor
Example:
```C++
#include "stdafx.h"
#include <iostream>
using namespace std;

struct Test {
	virtual bool operator()(string &text) = 0;
};

struct MatchTest : public Test {
	virtual bool operator()(string &text) {
		return text == "lion";
	}
};

void Check(string text, Test &test) {
	if (test(text)) {
		cout << "Matches" << endl;
	}
	else {
		cout << "Not Match" << endl;
	}
}

int main() {
	MatchTest m;
	Check("liond", m);
	return 0;
}
```
A functor is a struct or class that overloaded `operator()`, this operator can take any numbers of arguments, and can return any type;
