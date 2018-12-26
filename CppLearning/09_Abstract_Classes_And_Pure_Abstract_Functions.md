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