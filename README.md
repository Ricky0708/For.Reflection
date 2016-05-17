# For.Reflection
High performance reflection component, it can be any component's core..

This is a high performance reflection component,<br/>
you can use it to make your value injector or any component...

# Performance
* stande call (use namespace **_TypeProcessor_**)
  * 50,000 records with 11 property from a object inject to a new object cost about 127ms
* delegate call (use namespace **_Core_** to get delegate and call it by yourself) 
  * 1,000,000 records cost about 620ms

# Features
* Property get set
* Field get set
* Create instance (available with generic)
* Void call (available with generic)
* Method call (available with generic)

# How to use (There is a performance test and unit test in the project)
* Add reference For.Reflection
* using For.Reflection
* stand call
```C#
            TypeProcessor processor = new TypeProcessor(typeof(TestType));
            for (int i = 0; i < 50000; i++)
            {
                var ToObj = processor.CreateInstance();
                foreach (PropertyInfo item in fromObj.GetType().GetProperties())
                {
                    processor.SetProperty(ToObj, item.Name, processor.GetProperty(fromObj, item.Name));
                }
            }
```
* delegate call
```C#
            Core.delgCreateInstance delgCreate = Core.GenCreateInstanceDelg(Core.MakeCtorInfo(typeof(TestType), null));
            List<Core.delgSetProperty> setters = new List<Core.delgSetProperty>();
            List<Core.delgGetProperty> getters = new List<Core.delgGetProperty>();
            PropertyInfo[] propertyInfos = fromObj.GetType().GetProperties();

            for (int i = 0; i < propertyInfos.Count(); i++)
            {
                setters.Add(Core.GenSetPropertyValueDelg(o, propertyInfos[i]));
                getters.Add(Core.GenGetPropertyValueDelg(o, propertyInfos[i]));
            }
            object toObj;
            for (int i = 0; i < 1000000; i++)
            {
                toObj = delgCreate();
                for (int n = 0; n < setters.Count(); n++)
                {
                    setters[n](toObj, getters[n](fromObj));
                }

            }
```

# History
* ** V 1.1.0.0**
  * add new namespace 『TypeProcessor』, and this will be stand call
  * remove old stand call 『Processor』
  

* **V 1.0.0.0**
  * Upload component
