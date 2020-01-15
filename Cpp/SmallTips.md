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
