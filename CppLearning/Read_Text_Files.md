## Example
```C++
#include <iostream>
#include <fstream>
using namespace std;

int main()
{
    string inFileName = "text.txt";
    iftream inFile;
    inFile.open(inFileName);
    
    if(inFile.is_open()){
        string line;
        
        while(inFile)
        {
            getLine(inFile, line);
            cout << line << endl;
        }
        
        inFile.close();
    }
    else{
        cout << "Cannot open file: " << inFileName << endl;
    }
    
    return 0;
}
```
- When we use `inFile >> line;` This means read the next word, not next line, to read next line we shoud use `getLine(inFile, line);`  
- `while(inFile)` is weired just like check if the `inFile` object exists, the reason is that the `==` operator is overloaded so that could be used more convinient.  
- `while(!inFile.eof())` which means "end of file" is exactly the same with above, if we have `==` operator overloaded, use this. 
