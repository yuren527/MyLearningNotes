Cpp doesn't have native delegate and event module, but as its flexibility, I implemented my own delegate and event functionality;
```C++
#include <iostream>
#include <vector>
#include <map>

class MyDelegate {
public:
	std::vector<void (*)(int)> funcPtrs;
	void Bind(void (*func)(int)) {
		funcPtrs.push_back(func);
	}
	void Invoke(int a) {
		for (auto f : funcPtrs) {
			f(a);
		}
	}
};

class MyMemberDelegate {
public:
	std::multimap<void*, void (*)(void*, int)> funcPtrs;
	void Bind(void* obj, void (*func)(void*, int)) {
		funcPtrs.insert(std::make_pair(obj, func));
	}
	void Invoke(int a) {
		for (auto f : funcPtrs) {
			f.second(f.first, a);
		}
	}
};

void TestFunc1(int a) {
	std::cout << "Func1: " << a << std::endl;
}

class TestObject {
public:
	void TestFunc2(int a) {
		std::cout << "Func2: " << a << std::endl;
	}
	static void TestFunc2Wrapper(TestObject* obj, int a) {
		obj->TestFunc2(a);
	}
};

int main()
{
	MyDelegate del1;
	del1.Bind(&TestFunc1);
	del1.Invoke(4);

	MyMemberDelegate del2;
	TestObject obj;
	del2.Bind(&obj, reinterpret_cast<void (*)(void*, int)>(&TestFunc2));
	del2.Invoke(2);
	
}
```
For non-member functions or static functions, it's easy to bind it to a delegate;

But, for member functions, things get a little complex. Because member function can't be passed into other functions without a object, so we need to wrap the member function into a non-member function  or a static member function with a context argument followed, this non-member wrapper function will know which object to call from and then we tell it what member function to call, then we can use the wrapper function instead to pass into the delegate object, which can add or remove a function pointer and call it from the delegate object and anywhere; 

[See the Article to get more information](https://stackoverflow.com/questions/12662891/how-can-i-pass-a-member-function-where-a-free-function-is-expected)  
Also ca see the example in example folder. 

# Using macros
There is another way to bind a member function to a delegate, using macro:  
> #define MAKE_MY_MEMBER_DELEGATE(FUNC_NAME, OBJ_TYPE) static void FUNC_NAME_Wrapper(OBJ_TYPE* obj, int x){obj->FUNC_NAME(x);}  

> #define BIND_MY_MEMBER_DELEGATE(DEL, OBJ, OBJ_TYPE, FUNC_NAME) DEL.Bind(&OBJ, reinterpret_cast<void (*)(void*, int)>(&OBJ_TYPE::FUNC_NAME_Wrapper));
- The first macro should be called to automatically declare a wrapper function; 
- The second one is used to bind the function to a delegate; 
```C++
#define MAKE_MY_MEMBER_DELEGATE(FUNC_NAME, OBJ_TYPE) static void FUNC_NAME_Wrapper(OBJ_TYPE* obj, int x){obj->FUNC_NAME(x);}  
#define BIND_MY_MEMBER_DELEGATE(DEL, OBJ, OBJ_TYPE, FUNC_NAME) DEL.Bind(&OBJ, reinterpret_cast<void (*)(void*, int)>

class TestObject {
public:
	void TestFunc2(int a) {
		std::cout << "Func2: " << a << std::endl;
	}
	MAKE_MY_MEMBER_DELEGATE(TestFunc2, TestObject);
};

int main()
{
	MyDelegate del1;
	del1.Bind(&TestFunc1);
	del1.Invoke(4);

	MyMemberDelegate del2;
	TestObject obj;
	BIND_MY_MEMBER_DELEGATE(del2, obj, TestObject, TestFunc2);
	del2.Invoke(2);
	
}
```
