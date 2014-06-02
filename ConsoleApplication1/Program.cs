using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace ConsoleApplication1
{
	class Program
	{
		static void Main(string[] args)
		{
			dynamic person =  DynamicDecorator<Person>.GetDecorated(new Person());
			person.Age = 35;
			person.AgeFormatter = new Func<dynamic, string>(p => string.Format("{0} your age is {1}", p.Name, p.Age));
			person.SetInterceptor("AgeFormatter", new Dummy());
			Console.WriteLine(person.Name);
			Console.WriteLine(person.Age);
			Console.WriteLine(person.AgeFormatter(person));
			Console.ReadKey();
		}

		
	}


	public class Dummy
	{
		public string AgeFormatter(dynamic person)
		{
				return string.Format("[Indercepted] person's age in json age:{0}", person.Age);
			
		}
	}
	public class Person
	{
		public Person()
		{
			Name = "Preet";
		}
		public string Name { get; set; }
	}

	public class DynamicDecorator<T> : DynamicObject
	{
		public T ObjectToDecorate { get; set; }
		private Dictionary<string, object> _properties;
		private Dictionary<string, object> _interceptors;
		
		private DynamicDecorator(T objectToDecorate)
		{
			ObjectToDecorate = objectToDecorate;
			_properties = new Dictionary<string, object>();
			_interceptors = new Dictionary<string, object>();
			AddExistingProperties();
		}

		private void AddExistingProperties()
		{
			var existingProperties = ObjectToDecorate.GetType().GetProperties().ToList();
			foreach (var existingProperty in existingProperties)
			{
				_properties.Add(existingProperty.Name, existingProperty.GetValue(ObjectToDecorate));
			}
		}

		public void SetInterceptor(string memberToIntercept, object d)
		{
			if (_interceptors.ContainsKey(memberToIntercept))
			{
				_interceptors[memberToIntercept] = d;
				return;
			}
			_interceptors.Add(memberToIntercept, d);	
			
		}

		

		public static dynamic GetDecorated(T person)
		{
			return new DynamicDecorator<T>(person);
		}

		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{

			
				return _properties.TryGetValue(binder.Name, out result);
			
			
		}

		public override bool TrySetMember(SetMemberBinder binder, object value)
		{
			if (_properties.ContainsKey(binder.Name))
			{
				_properties[binder.Name] = value;
			}
			_properties.Add(binder.Name, value);
			return true;
		}

		public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
		{
			// check if the interceptor has been set for this member
			if (_interceptors.ContainsKey(binder.Name))
			{
				var toInvoke = _interceptors[binder.Name];
					var method = toInvoke.GetType().GetMethods().FirstOrDefault(info => info.Name.Contains(binder.Name));
					result = method.Invoke(toInvoke, args);
					return true;
				
			}
			return base.TryInvokeMember(binder, args, out result);
		}
	}
}
