## Example
If we throw two exceptions, one is `bad_alloc` and another is `exception`  
then we try to catch them, see the code below:
```C++
#include <iostream>
#include <exception>

using namespace std;
void GoesWrong(){
    bool error1Detected = true;
    bool error2Detected = true;
  
    if(error1Detected)
    {
        throw bad_alloc();
    }
    if(error2Detected)
    {
        throw exception();
    }
}

int main()
{
    try
    {
        GoesWrong();
    }
    catch(exception &e)
    {
        cout << "Cathing exception:" << e.what() << endl;
    }
    catch(bad_alloc &e)
    {
        cout << "Cathing bad_alloc:" << e.what() << endl;
    }
	
    return 0;
}
```
after running this, we get a output like this:  
`Catching exception:bad_alloc`  
this is definitely not what we expected, the clue is **POLYMORPHISM**  
`bad_alloc` **IS** an `exception`, but `exception` **IS NOT** a `bad_alloc`, and we know `what()` is a virtual function
that's why we got this output

## "Unreferenced local variable"
Sometimes it cannot compile due to "Unreferenced local variable", To solve it, just do as below:
```C++
catch(exception& e){
	(void)e;
	cout << e.what() << endl;
}
```
Then the error just gone;
