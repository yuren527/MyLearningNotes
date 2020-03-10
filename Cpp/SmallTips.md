# const 指针的两种用法
```C++
int main() {
    int i = 0;
    int j = 1;

    // const 指针，指针内容不可修改
    int *const p = &i; 
    p = &j; // error, 由于p被定义为const指针，因此其指向的物理地址是不可修改的
    *p =10; // right, 通过const指针可以修改其所指对象

    // 指向const对象的指针，不可通过指针修改对象
    const int *pp = &i;
    *pp = 10; //error，指向const对象的指针不能修改其所指对象
    pp = &j; // right, 可以修改指向const对象指针的值（即修改指针）

    return 0;
}
```

# 简单的时钟
·clock_t t = clock();` 现在的时间  
```C++
clock_t startTime = clock();
clock_t endTime = clock();
double passedTime = endTime - startTime;
```
**C++ Reference:**
```
Returns the processor time consumed by the program.

The value returned is expressed in clock ticks, which are units of time of a constant but system-specific length (with a relation of CLOCKS_PER_SEC clock ticks per second).

The epoch used as reference by clock varies between systems, but it is related to the program execution (generally its launch). To calculate the actual processing time of a program, the value returned by clock shall be compared to a value returned by a previous call to the same function.
```

# Pit of iterating #
Do NOT remove any element in list or vector when using `For(auto a : list)` to iterate elements, unless immediately return or break the loop after removing! Otherwise, a address error will occur when it comes to next loop;  
