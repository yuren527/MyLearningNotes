# Template classes and functions
Not much to say, only some little clues to memorize.

See code shows below:
```C++
#include <iostream>
using namespace std;

template<class T>
void print(T n){
    cout << "Template version: " << n << endl;
}

void print(int value){
    cout << "Non-template version: " << value << endl;
}

template<typename T>
void show(){
    cout << T() << endl;
}
	
int main(){
    print<string>("Hello");
    print<int>(5);
    print(6);
    print<>(7);
    return 0;
	
    show<double>(); //L1
    show<>();  //L2
    show();  //L3
}
```
- In template we can use both class and typename for class declaration;

- If a function have both template version and non-template version, use something like `print<>(7);` to specify to use a template version, without the diamond brackets means using a non-template version;

- If a template function takes no argument, it's nesessary to specify a type in the diamond brackets so that the compiler can infer what type to use, ortherwise it will be a error, just like what shows above, L1 will compile normally while L2 and L3 will cause a error;