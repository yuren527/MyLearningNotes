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
- Overload plus operator;

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
	Complex operator+(const Complex &c1, const Complex &c2);
	Complex operator+(const Complex &c1, double d);
	
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

Complex Complex::operator+(const Complex &c1, const Complex &c2){
	return Complex(c1.GetReal() + c2.GetReal(), c1.GetImaginary() + c2.GetImaginary());
}

Complex Complex::operator+(const Complex &c1, double d){
	return Complex(c1.GetReal() + d, c1.GetImaginary());
}
```
Beware that we cant do `c2 = 3.2 + c1;` using functions above, because the parameter order should not change, the left side of the operator is passing to the first parameter while the right side to the second, if needed, overload another function.
