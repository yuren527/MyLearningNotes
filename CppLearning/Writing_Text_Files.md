## Example
```C++
#include <iostream>
#include <fstream>

using namespace std;

int main(){
	ofstream outFile;
	string outFileName = "text.txt";
  
	outFile.open(outFileName);
	if(outFile.is_open()){
		outFile << "Hello there" << endl;
		outFile << "123" << endl;
		outFlie.close();
	}
	else{
		cout << "Could not create file: " << outFileName << endl;
	}
	
	return 0;
}
```
We used a `ofstream` class which is short for "out file stream"  
We can also use `fstream` instead
```
fstream outFile;
outFile.open(outFileName, ios::out);
```
