# **Charset（字符集、编码类型）**
The most common charset is `UTF-8` and `GB2312`(`GBK`)
> UTF-8 国际通用字库  
> GB2312(GBK) 主要偏向中文
   
`UTF-8`一个汉字3个字节  
`GB2312`一个汉字2个字节  
大多数浏览器默认使用`UTF-8`，因此如果文档与之不匹配，则可能出现乱码。  
<br />
# **常用标签**
See the html example code file [CommonMarkups.html](Examples/CommonMarkups.html)
## **文字样式**
> **标题：**`<h1></h1>`......`<h6></h6>`  
> **段落：**`<p></p>`  
> **加粗：**`<strong></strong>`或者`<b></b>`  
> **倾斜：**`<i></i>`或者`<em></em>`  
> **下划线：**`<u></u>`  
> **上下标：**`<sup></sup>`和`<sub></sub>`
## **特殊字符**
> **换行：**`<br />`  
> **分割线：**`<hr />`  
> **备案符：**`&copy`  
> **空格：**`&nbsp`  
> **左右尖括号：**`&lt` `&gt`
## **列表**
- ### 无序列表
```html
<ul>
    <li>这是第一行</li>
    <li>这是第二行</li>
    <li>这是第三行</li>
    <li>这是第四行</li>
</ul>
```
- ### 有序列表
```html
<ol type = “a” start = 3>
    <li>这是第一行</li>
    <li>这是第二行</li>
    <li>这是第三行</li>
    <li>这是第四行</li>
</ol>
```
属性 `type` = 1,a,A,i,I  
`start` = 3 起始序号
- ### 自定义列表
```html
<dl>
    <dt>自定义列表</dt>
    <dd>这是第一行</dd>
    <dd>这是第二行</dd>
    <dd>这是第三行</dd>
</dl>
```
## **图片**
```html
<img src = "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1543971017960&di=181a0cf54137cc5a5bdb7c1d28d456a5&imgtype=0&src=http%3A%2F%2Fpic37.nipic.com%2F20140209%2F2531170_112946779000_2.jpg" />
```

## **超链接**
```html
<a href = "https://www.baidu.com" targe = "_blank">百度一下，你就知道</a>
```
`targe`取值分为:   
`_blank`新建窗口打开  
`_self`当前窗口打开  
<br />

# **表格**
See the example code file [Tables.html](Examples/Tables.html)