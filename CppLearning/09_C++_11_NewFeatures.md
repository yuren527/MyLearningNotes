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
# Ring buffer class
```C++
#include <iostream>
using namespace std;

template<typename T>
class ring(){
private:
    int m_pos;_
    int m_size;
    T* m_values;
public:
    ring(int size):m_pose(0), m_size(size), m_values(NULL):{
        m_values = new T(size);
	}

    ~ring(){
        delete [] m_values;
    }

    int size(){
        return m_size;
    }

    void add(T value){
        m_values[m_pos] = value;
        if(m_pose == m_size){
            m_pose = 0; 
        }
    }

    T& get(){
        return m_values[m_pos];
    }
}
```
`delete` only frees the memory of a[0], while `delete []` frees all the memories in the collection, it calls the destructor of all the elements in the array, but when using a primitive type as elements, destructor would not be called, the size of element is fixed and the system wll take care of it;

So, when using a class as elements, use `delete []` to free the memories of all elements, but for primitive type `delete` is fine; A template class should use `delete []` as we don't know if it's a primitive type or not;

