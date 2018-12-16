//Example -- Reading Writing Binary Files

#include <string>
#include <iostream>
#include <fstream>
using namespace std;

#pragma pack(16)
struct Person
{
	char name[32];
	int age;
	float height;
};
#pragma pack()

int main()
{	
	string fileName = "testFile";

	Person someone = { "Markus", 34, 1.75f };
	ofstream output;
	output.open(fileName, ios::binary);
	if (output.is_open())
	{
		output.write(reinterpret_cast<char*>(&someone), sizeof(Person));
		output.close();
	}
	else
	{
		cout << "Could not create file " + fileName << endl;
	}

	Person someoneElse;
	ifstream input;
	input.open(fileName, ios::binary);
	if (input.is_open())
	{
		input.read(reinterpret_cast<char*>(&someoneElse), sizeof(Person));
		input.close();
	}
	else
	{
		cout << "Could not open file " + fileName << endl;
	}

	cout << someoneElse.name << ", " << someoneElse.age << ", " << someoneElse.height << endl;
	
    return 0;
}
