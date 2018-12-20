# Vector
`Vector`可以像`array`一样指定长度
```C++
vector<string> strings(5);
```
# Vector and memory
A vector actually contains a internal array, we can initialize the size of the internal array with:
> vector\<int\> test(20);  
    
This initialize a internal array with the size of `20`  
But, ***once we add new element to the vector that is beyond the original size, a new internal array with twice the original size is created, and it will copy the old elements.***   
</br>
So, we should know that the difference between `size()` and `capacity()`.One is the length of the vector and the other indicates the size of the internal array.   
</br>
**Some of the functions should be highlightened:**  
`clear()` is to dicard all the elements in the vector, but do not change the size or capacity of the vector.  
`resize()` means to give a new size of the vector.  
`reserve()` means to give a new capacity of the internal array, which can only be increased.  
# Two-Dimensional Vector
**Example**
```C++
#include <iostream>
#include <vector>
using namespace std;

int main()
{
    vector< vector<int> > grid(3, vector<int>(4, 7));
    grid[1].push_back(8);
    
    for(int row = 0; row < grid.size(); row++)
    {
        for(int col = 0; col < grod[row].size(); col++)
        {
            cout << grid[row][col] << flush;
        }
        cout << endl;
    }
    return 0;
}
```
This will be the test result
> 7777  
> 77778  
> 7777
