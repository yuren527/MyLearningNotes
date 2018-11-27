源代码参见[Example_0.cpp](Examples/Example_0/Example_0.cpp) 

## 在C++中输出string 
需要在cout中输出string，必须加上`#include <string>`，c++中重载了`string`类型  
<br/>
## struct内存对齐
```C++
#pragma pack(16)
struct Person
{
	char name[32];
	int age;
	float height;
};
#pragma pack()
```
##### 自然对齐：及默认结构体变量成员中最大的长度设置为对齐字节
```C++
struct node
{
      char a;
      int b;
      short c;
};
```
默认以最大长度int类型4字节对齐。此时占用内存为12byte
##### 指定对齐
```C++
#pragma pack(n)  //设置以n字节对齐 超出n字节长度默认以超出字节长度对齐
#pragma pack () //取消指定
```
如：
```C++
#pragma pack(2)
struct node1
{
      char a;
      char b;
      int c;
};
#pragma pack()
```
此时sizeof（struct node1）为6byte;再比如：
```C++
struct node
{
     char a;
     short b;
     int c;
 
     struct node *Last;
     struct node *Next;
};
```
由于默认内存对齐字节为int型四个字节，故：a占用一个字节,偏移量为1，
b为short型，2字节，由于a偏移量1不是short型数据2字节的整数倍，故编译器
在a后边填充一个空字节，然后存储b，此时偏移量为4，为int型的整数倍，直接
存储int数据c，最后两个由于为指针，所以每一个都为四字节，及一共16字节。
此时应该看16字节是否为每一个成员的整数倍，不足后边补充空字节填充。





- 原则一：结构体中元素是按照定义顺序一个一个放到内存中去的，但并不是紧密排列的。从结构体存储的首地址开始，每一个元素放置到内存中时，它都会认为内存是以它自
己的大小来划分的，因此元素放置的位置一定会在自己宽度的整数倍上开始（以结构体变量首地址为0计算）。

- 原则二：在经过第一原则分析后，检查计算出的存储单元是否为所有元素中最宽的元素的长度的整数倍，是，则结束；若不是，则补齐为它的整数倍。
！