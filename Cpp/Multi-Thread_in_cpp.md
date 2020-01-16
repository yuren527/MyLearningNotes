A thread can be created in several ways:  

- Using a function potiner
- Using a functor
- Using a lambda function  
- Using tasks

Thesw methods are very similar with minor differences;

## Using a function pointer  
function:  
```C++
void accumulator_func(const std::vector<int>& v, unsigned long long& acm, unsigned int beginIndex, unsigned int endIndex) {
	acm = 0;
	for (unsigned int i = beginIndex; i < endIndex; i++) {
		acm += v[i];
	}
}
```
include `<tread>`;  

First we need to pass the pointer of the function when creating a thread, then pass in the parameters of the function to the tread constructor;
```C++
void method_1() {
	std::vector<int> v{ 1,2,3,4,5,6,7,8,9 };
	unsigned long long acm1 = 0;
	unsigned long long acm2 = 0;

	std::thread t1(&accumulator_func, std::ref(v), std::ref(acm1), 0, v.size() / 2);
	std::thread t2(&accumulator_func, std::ref(v), std::ref(acm2), v.size() / 2, v.size());

	t1.join();
	t2.join();

	std::cout << "acm1: " << acm1 << std::endl;
	std::cout << "acm2: " << acm2 << std::endl;
	std::cout << "acm: " << acm1 + acm2 << std::endl;
}
```
Important: All parameters passed to the constructor are passed by value unless we wrap them in `std::ref`;  

Thread created by `std::tread` do not have return values, if we want to return something, we should store it in one of the parameters passed by reference;  

## Using a functor
```C++
class  AccumulatorFunctor {
public:
	void operator()(const std::vector<int>& v, unsigned int beginIndex, unsigned int endIndex) {
		_acm = 0;
		for (unsigned int i = beginIndex; i < endIndex; i++) {
			_acm += v[i];
		}
	}
	unsigned long long _acm;
};

void method_2() {
	std::vector<int> v{ 1,2,3,4,5,6,7,8,9 };
	AccumulatorFunctor accumulator_1 = AccumulatorFunctor();
	AccumulatorFunctor accumulator_2 = AccumulatorFunctor();
	std::thread t1(std::ref(accumulator_1), std::ref(v), 0, v.size() / 2);
	std::thread t2(std::ref(accumulator_2), std::ref(v), v.size() / 2, v.size());
	t1.join();
	t2.join();

	std::cout << "acm_1: " << accumulator_1._acm << std::endl;
	std::cout << "acm_2: " << accumulator_2._acm << std::endl;
	std::cout << "acm_1 + acm_2: " << accumulator_1._acm + accumulator_2._acm << std::endl;
}
```
Everything is very similar except:
- The first paramemter is the functor object;
- Instead of passing a reference to the functor to store the result, we can store its return value in a member variable in the functor object;

## Using lmabda functions
```C++
void method_3() {
	std::vector<int> v{ 1,2,3,4,5,6,7,8,9 };
	unsigned long long acm1 = 0;
	unsigned long long acm2 = 0;
	std::thread t1([&acm1, &v] {
		for (unsigned int i = 0; i < v.size() / 2; i++)
		{
			acm1 += v[i];
		}
		}
	);
	std::thread t2([&acm2, &v] {
		for (unsigned int i = v.size() / 2; i < v.size(); i++)
		{
			acm2 += v[i];
		}
		}
	);

	t1.join();
	t2.join();

	std::cout << "acm1: " << acm1 << std::endl;
	std::cout << "acm2: " << acm2 << std::endl;
	std::cout << "acm1 + acm2: " << acm1 + acm2 << std::endl;
}
```
What's different of this method is that, we can pass in the references using lambda captures;  

## Using Tasks  
Include `<future>`
```C++
void method_4() {
	std::vector<int> v{ 1,2,3,4,5,6,7,8,9 };
	auto f1 = [](const std::vector<int>& v, unsigned int left, unsigned int right) {
		unsigned long long acm = 0;
		for (unsigned int i = left; i < right; ++i) {
			acm += v[i];
		}
		return acm;
	};

	auto t1 = std::async(f1, std::ref(v), 0, v.size() / 2);
	auto t2 = std::async(f1, std::ref(v), v.size() / 2, v.size());

	unsigned long long acm1 = t1.get();
	unsigned long long acm2 = t2.get();

	std::cout << "acm1: " << acm1 << std::endl;
	std::cout << "acm2: " << acm2 << std::endl;
	std::cout << "acm1 + acm2: " << acm1 + acm2 << std::endl;
}
```
- Tasks are created using `std::async`;
- The returned value from `std::async` is called a `std::future`,  it just means t1 and t2 are variables whose value will be assigned to in the future. We get their values by calling `t1.get()` and `t2.get()`;
- If the future values are not ready, upon calling `get()` the main thread blocks until the future value becones ready;(similar to `join()`)
- Each task by default starts as soon as it it created, there isa a way to change this which is not covered here;  

## Shared Memory and Shared Resources
In short, threads should be careful when they read/write into shared memory and resources (such as files) to avoid race conditions.  

C++14 provides several constructs to synchronize threads to avoid such race conditions.  

Using Mutex, lock,() and unlock() (Not recommended)  

The following code shows how we create a critical section such that each thread accesses std::cout exclusively:  
```C++
std::vector<int> v{ 1,2,3,4,5,6,7,8,9 };
	std::mutex g_display_mutex;
	auto f1 = [&g_display_mutex](const std::vector<int>& v, unsigned int left, unsigned int right) {
		unsigned long long acm = 0;
		for (unsigned int i = left; i < right; ++i) {
			acm += v[i];
		}
		
		g_display_mutex.lock();
		std::thread::id this_id = std::this_thread::get_id();
		std::cout << "My thread id is: " << this_id << "; result: " << acm << std::endl;
		g_display_mutex.unlock();
		
		return acm;
	};
```
- A critical section (i.e. guaranteed to be run only by a single thread at each time) is created using lock()
- The critical section ends upon calling unlock()
- Each thread waits at lock() and only enters the critical section if no other thread is inside that section.  

While the above method works, it is not recommended because:  

- It is not exception safe: if the code before lock generates an exception, unlock() will not be executed, and we never release the mutex which might cause deadlock
- We always have to be careful not to forget to call unlock()
- Using std::lock_guard (recommended)  

Below is the same critical section using lock_guard:  

```C++
std::lock_guard<std::mutex> guard(g_display_mutex);
std::thread::id this_id = std::this_thread::get_id();
std::cout << "My thread id is: " << this_id << "; result: " << acm << std::endl;
```
- The code coming after std::lock_guard creation is automatically locked. No need for explicit lock() and unlock() function calls.
- The critical section automatically ends when std::lock_guard goes out of scope. This makes it exception safe, and also we don’t need to remember to call unlock()
- lock_guard still requires using a variable of type std::mutex in its constructor.
