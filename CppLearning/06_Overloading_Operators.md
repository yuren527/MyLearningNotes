# Printing-Overloading Left Bit Shift & assignment overloading
See example below:
```C++
#include <iostream>
using namespace std;

class Test
{
private:
	int id;
	string name;
public:
	Test() : id(0), name(""){}
	Test(int id, string name) : id(id), name(name){}
	
	void print()
	{
		cout << id << ": " << name << endl;
	}
	
	const Test &operator=(const Test &other)
	{
		id = other.id;
		name = other.name;
		return *this;
	}
	Test(const Test &other)
	{
		*this = other;
	}
	
	friend ostream &operator<<(ostream &out, const Test &test)
	{
		out << test.id << ": " << test.name;
		return out;
	}
}

int main()
{
	Test test1(10, "Mike");
	cout << test1 << endl;
	return 0;
}
```
> Test test2 = test1;  
> Test test2(test1);  

The first line showed above is not calling a assignment but a copy constructor, just exactly the same with the second line, Be aware of it.  
# A Complex Number Class  
This section shows how to overload operators in a complex number class, what we are goning to do is:
- Overload copy constructor;
- Overload assignment operator;
- Overload left bit shift operator;

See example below:  

**Complex.h:**  
```C++
#include <iostream>
using namespace std;

class Complex{
private:
	double real;
	double imaginary;
	
public:
	Complex(){}
	Complex(double real, double imaginary) : real(real), imaginary(imaginary){}
	Complex(const Complex &other);
	
	const Complex &operator=(const Complex &other);
	
	double GetReal() const { return real;}
	double GetImaginary() const { return imaginary;}
}

ostream &operator<<(ostream &out, const Complex &c);
```
**Complex.cpp:**
```C++
#include "Complex.h"

Complex::Complex(const Complex &other){
	real = other.real;
	imaginary = other.imaginary;
}

const Complex &Complex::operator=(const Complex &other){
	real = other.real;
	imaginary = other.imaginary;
	return *this;
}

ostream &operator<<(ostream &out, const Complex &c){
	out << "(" << c.real << "," << c.imaginary << ")";
	return out;
}
```
