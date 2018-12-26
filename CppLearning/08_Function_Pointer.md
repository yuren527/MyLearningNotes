# Function Pointer
Example:
```C++
#include <iostream>
#include <vector>
#include <algorithm>
using namespace std;

bool Match(string text) {
    return text.size() == 3;
}

bool MatchN(string text, int n) {
    return text.size() == n;
}

int CountString(vector<string> texts, int n, bool (*func)(string, int))
{
    int tally = 0;
    for (int i = 0; i < texts.size(); i++) {
        if (func(texts[i], n)){
            tally++;
        }
    }
    return tally;
}

int main(){
    vector<string> texts;
    texts.push_back("one");
    texts.push_back("two");
    texts.push_back("three");
    texts.push_back("four");
    texts.push_back("five");

    cout << count_if(texts.begin(), texts.end(), Match) << endl;
    cout << CountString(texts, 4, MatchN) << endl;
    return 0;
}
```