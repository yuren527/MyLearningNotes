# my implemented delegate in C++ #
.h:
```C++
#pragma once
#include <list>

using namespace std;

namespace Markus{
	template<typename Ret_Ty, typename... P>
	class MarkusProxyBase {
	public:
		virtual void Execute(P... args) = 0;
		virtual ~MarkusProxyBase() {
			cout << "Proxy base deleted" << endl;
		}
	};

	template<typename Ret_Ty, typename... P>
	class MarkusFuncProxy : public MarkusProxyBase<Ret_Ty, P...>{
	private:
		Ret_Ty(*FuncPtr)(P...);
	public:
		MarkusFuncProxy(Ret_Ty(*func)(P...)) : FuncPtr(func) {}
		~MarkusFuncProxy() {
			cout << "Func proxy deleted" << endl;
		}
		virtual void Execute(P... args) override {
			if (FuncPtr) {
				FuncPtr(args...);
			}
		}
		bool Equals(Ret_Ty(*func)(P...)) {
			return FuncPtr == func;
		}
	};

	template<typename Obj_Ty, typename Ret_Ty, typename... P>
	class MarkusMemProxy : public MarkusProxyBase<Ret_Ty, P...> {
	private:
		Obj_Ty* ObjPtr;
		Ret_Ty(Obj_Ty::* FuncPtr)(P...);
	public:
		MarkusMemProxy(Obj_Ty* obj, Ret_Ty(__thiscall Obj_Ty::* func)(P...)) : ObjPtr(obj), FuncPtr(func) {}
		~MarkusMemProxy() {
			cout << "Mem proxy deleted" << endl;
		}
		virtual void Execute(P... args) override {
			if (ObjPtr && FuncPtr) {
				(ObjPtr->*FuncPtr)(args...);
			}
		}
		bool Equals(Obj_Ty* obj, Ret_Ty(Obj_Ty::* func)(P...)) {
			return ObjPtr == obj && FuncPtr == func;
		}
	};

	template<typename Ret_Ty, typename... P>
	class MarkusDelegate {
	private:
		list<MarkusProxyBase<Ret_Ty, P...>*> Proxies;
	public:
		~MarkusDelegate() {
			for (MarkusProxyBase<Ret_Ty, P...>* it : Proxies) {
				delete it;
			}
		}
		void Clear() {
			for (MarkusProxyBase<Ret_Ty, P...>* it : Proxies) {
				delete it;
			}
			Proxies.empty();
		}
		void Add(Ret_Ty(*func)(P...)) {
			Proxies.push_back(new MarkusFuncProxy<Ret_Ty, P...>(func));
		}
		template<typename Obj_Ty>
		void Add(Obj_Ty* obj, Ret_Ty(Obj_Ty::* func)(P...)) {
			Proxies.push_back(new MarkusMemProxy<Obj_Ty, Ret_Ty, P...>(obj, func));
		}
		void Remove(Ret_Ty(*func)(P...)) {
			for (MarkusProxyBase<Ret_Ty, P...>* it : Proxies) {
				if (MarkusFuncProxy<Ret_Ty, P...> * proxy = dynamic_cast<MarkusFuncProxy<Ret_Ty, P...>*>(it)) {
					if (proxy->Equals(func)) {
						Proxies.remove(it);
						delete it;
						return;
					}
				}
			}
		}
		template<typename Obj_Ty>
		void Remove(Obj_Ty* obj, Ret_Ty(Obj_Ty::* func)(P...)) {
			for (MarkusProxyBase<Ret_Ty, P...>* it : Proxies) {
				if (MarkusMemProxy<Obj_Ty, Ret_Ty, P...> * proxy = dynamic_cast<MarkusMemProxy<Obj_Ty, Ret_Ty, P...>*>(it)) {
					if (proxy->Equals(obj, func)) {
						Proxies.remove(it);
						delete it;
						return;
					}
				}
			}
		}
		void Invoke(P... args) {
			for (MarkusProxyBase<Ret_Ty, P...>* it : Proxies) {
				it->Execute(args...);
			}
		}
		size_t GetDelegateNum() const {
			return Proxies.size();
		}
	};
}
```

See [example](https://github.com/yuren527/MyLearningNotes/tree/master/Cpp/Examples/MarkusDelegate);

# Using macros
There is another way to bind a member function to a delegate, using macro:  
> #define MAKE_MY_MEMBER_DELEGATE(FUNC_NAME, OBJ_TYPE) static void FUNC_NAME_Wrapper(OBJ_TYPE* obj, int x){obj->FUNC_NAME(x);}  

> #define BIND_MY_MEMBER_DELEGATE(DEL, OBJ, OBJ_TYPE, FUNC_NAME) DEL.Bind(&OBJ, reinterpret_cast<void (*)(void*, int)>(&OBJ_TYPE::FUNC_NAME_Wrapper));
- The first macro should be called to automatically declare a wrapper function; 
- The second one is used to bind the function to a delegate; 
```C++
#define MAKE_MY_MEMBER_DELEGATE(FUNC_NAME, OBJ_TYPE) static void FUNC_NAME_Wrapper(OBJ_TYPE* obj, int x){obj->FUNC_NAME(x);}  
#define BIND_MY_MEMBER_DELEGATE(DEL, OBJ, OBJ_TYPE, FUNC_NAME) DEL.Bind(&OBJ, reinterpret_cast<void (*)(void*, int)>

class TestObject {
public:
	void TestFunc2(int a) {
		std::cout << "Func2: " << a << std::endl;
	}
	MAKE_MY_MEMBER_DELEGATE(TestFunc2, TestObject);
};

int main()
{
	MyDelegate del1;
	del1.Bind(&TestFunc1);
	del1.Invoke(4);

	MyMemberDelegate del2;
	TestObject obj;
	BIND_MY_MEMBER_DELEGATE(del2, obj, TestObject, TestFunc2);
	del2.Invoke(2);
	
}
```
