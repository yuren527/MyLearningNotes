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
