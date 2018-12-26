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
# Functor
Example:
```C++
#include "stdafx.h"
#include <iostream>
using namespace std;

struct Test {
	virtual bool operator()(string &text) = 0;
};

struct MatchTest : public Test {
	virtual bool operator()(string &text) {
		return text == "lion";
	}
};

void Check(string text, Test &test) {
	if (test(text)) {
		cout << "Matches" << endl;
	}
	else {
		cout << "Not Match" << endl;
	}
}

int main() {
	MatchTest m;
	Check("liond", m);
	return 0;
}
```
A functor is a struct or class that overloaded `operator()`, this operator can take any numbers of arguments, and can return any type;
