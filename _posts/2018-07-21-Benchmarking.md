---
title: Benchmarking
layout: csharp
date: 2018-07-21
---

```csharp
﻿#define CSHARP7

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Benchmarking
{
    public class CastTesting
    {
        class Result
        {
            public long Time { get; set; }
            public string Label { get; set; }
        }

        public static Type A()
        {
            return typeof(A);
        }

        public static void Main()
        {
            Console.WriteLine("Starting benchmark...");

            SortedDictionary<long, string> results = new SortedDictionary<long, string>();
            Console.WriteLine("Starting baseline...");
            long baseline = BenchmarkType(new Baseline());
            Console.WriteLine("Finished baseline. Baseline = {0}ms.", baseline);
            var casters = (from t in typeof(CastTesting).Assembly.GetTypes()
                         where t.GetInterface("ICaster") != null && t.Name != "Baseline" && t.GetConstructor(Type.EmptyTypes) != null
                         select (ICaster)Activator.CreateInstance(t)).ToArray();
            for (int i = 0; i < casters.Length; ++i)
            {
                var caster = casters[i];
                string name = caster.GetType().Name;
                Console.WriteLine("Beginning {0}", name);
                long result = BenchmarkType(caster);
                results[result] = name;
                Console.WriteLine("Done with {0} in {3}: {1}/{2}", name, i, casters.Length, result);
            }

            long fastestTime = results.First().Key - baseline;
            foreach (var kvp in results)
            {
                long thisTime = kvp.Key - baseline;
                double slowDown = (1 - 1.0 * fastestTime / thisTime) * 100;
                Console.WriteLine("{0,-40}: {1,-8}  {2:0.##}% slower than fastest.", kvp.Value, thisTime, slowDown);
            }

            Console.WriteLine();
            Console.WriteLine("Adjusted for a baseline of {0} milliseconds.", baseline);
            Console.WriteLine("Processing done! Press any key to quit.");
            Console.ReadKey();
        }

        const long Iterations = 9000000000;

        static long BenchmarkType(ICaster caster)
        {
            var doer = new Doer();
            GC.Collect();
            var watch = Stopwatch.StartNew();
            for (long i = 0; i < Iterations; ++i)
            {
                caster.Do(new A(), doer);
                caster.Do(new B(), doer);
                caster.Do(new C(), doer);
                caster.Do(new D(), doer);
                caster.Do(new E(), doer);
                caster.Do(new F(), doer);
            }
            return watch.ElapsedMilliseconds;
        }
    }

    public interface ICaster
    {
        void Do(IDo o, Doer doer);
    }

    public class Baseline : ICaster
    {
        public void Do(IDo o, Doer doer) { }
    }

    public class TypeDictionary : ICaster
    {
        static Dictionary<Type, Action<Doer, object>> s_dict = new Dictionary<Type, Action<Doer, object>>()
        {
            { typeof(A), (d, o) => d.Do((A)o) },
            { typeof(B), (d, o) => d.Do((B)o) },
            { typeof(C), (d, o) => d.Do((C)o) },
            { typeof(D), (d, o) => d.Do((D)o) },
            { typeof(E), (d, o) => d.Do((E)o) },
            { typeof(F), (d, o) => d.Do((F)o) },
        };

        public void Do(IDo o, Doer doer)
        {
            Action<Doer, object> actor;
            s_dict.TryGetValue(o.GetType(), out actor);
            actor(doer, o);
        }
    }

    public class HandleDict : ICaster
    {
        static Dictionary<RuntimeTypeHandle, Action<Doer, object>> s_dict = new Dictionary<RuntimeTypeHandle, Action<Doer, object>>()
        {
            { typeof(A).TypeHandle, (d, o) => d.Do((A)o) },
            { typeof(B).TypeHandle, (d, o) => d.Do((B)o) },
            { typeof(C).TypeHandle, (d, o) => d.Do((C)o) },
            { typeof(D).TypeHandle, (d, o) => d.Do((D)o) },
            { typeof(E).TypeHandle, (d, o) => d.Do((E)o) },
            { typeof(F).TypeHandle, (d, o) => d.Do((F)o) }
        };

        public void Do(IDo o, Doer doer)
        {
            s_dict[Type.GetTypeHandle(o)](doer, o);
        }
    }

    public class HandleElse : ICaster
    {
        static RuntimeTypeHandle s_a = typeof(A).TypeHandle;
        static RuntimeTypeHandle s_b = typeof(B).TypeHandle;
        static RuntimeTypeHandle s_c = typeof(C).TypeHandle;
        static RuntimeTypeHandle s_d = typeof(D).TypeHandle;
        static RuntimeTypeHandle s_e = typeof(E).TypeHandle;
        static RuntimeTypeHandle s_f = typeof(F).TypeHandle;

        public void Do(IDo o, Doer doer)
        {
            var handle = Type.GetTypeHandle(o);
            if (handle.Equals(s_a)) doer.Do((A)o);
            else if (handle.Equals(s_b)) doer.Do((B)o);
            else if (handle.Equals(s_c)) doer.Do((C)o);
            else if (handle.Equals(s_d)) doer.Do((D)o);
            else if (handle.Equals(s_e)) doer.Do((E)o);
            else if (handle.Equals(s_f)) doer.Do((F)o);
        }
    }

    public class IfIsCStyle : ICaster
    {
        public void Do(IDo o, Doer doer)
        {
            if (o is A)
                ((A)o).Do(doer);
            else if (o is B)
                ((B)o).Do(doer);
            else if (o is C)
                ((C)o).Do(doer);
            else if (o is D)
                ((D)o).Do(doer);
            else if (o is E)
                ((E)o).Do(doer);
            else if (o is F)
                ((F)o).Do(doer);
        }
    }

    public class TypeElse : ICaster
    {
        public void Do(IDo o, Doer doer)
        {
            var type = o.GetType();
            if (type == typeof(A)) doer.Do((A)o);
            else if (type == typeof(B)) doer.Do((B)o);
            else if (type == typeof(C)) doer.Do((C)o);
            else if (type == typeof(D)) doer.Do((D)o);
            else if (type == typeof(E)) doer.Do((E)o);
            else if (type == typeof(F)) doer.Do((F)o);
        }
    }

    public class CastAsEachTime : ICaster
    {
        public void Do(IDo o, Doer doer)
        {
            var a = o as A;
            var b = o as B;
            var c = o as C;
            var d = o as D;
            var e = o as E;
            var f = o as F;

            if (a != null)
                doer.Do(a);
            else if (b != null)
                doer.Do(b);
            else if (c != null)
                doer.Do(c);
            else if (d != null)
                doer.Do(d);
            else if (e != null)
                doer.Do(e);
            else if (f != null)
                doer.Do(f);
        }
    }

    public class Polymorphic : ICaster
    {
        public void Do(IDo o, Doer doer)
        {
            o.Do(doer);
        }
    }

    public class CastWithElse : ICaster
    {
        public void Do(IDo o, Doer doer)
        {
            var a = o as A;
            if (a != null)
                doer.Do(a);
            else
            {
                var b = o as B;
                if (b != null)
                    doer.Do(b);
                else
                {
                    var c = o as C;
                    if (c != null)
                        doer.Do(c);
                    else
                    {
                        var d = o as D;
                        if (d != null)
                            doer.Do(d);
                        else
                        {
                            var e = o as E;
                            if (e != null)
                                doer.Do(e);
                            else
                            {
                                var f = o as F;
                                if (f != null)
                                    doer.Do(f);
                            }
                        }
                    }
                }
            }
        }
    }

#if CSHARP7
    public class PatternMatchIfElse : ICaster
    {
        public void Do(IDo o, Doer doer)
        {
            if (o is A a)
                doer.Do(a);
            else if (o is B b)
                doer.Do(b);
            else if (o is C c)
                doer.Do(c);
            else if (o is D d)
                doer.Do(d);
            else if (o is E e)
                doer.Do(e);
            else if (o is F f)
                doer.Do(f);
        }
    }

    public class PatternMatchSwitch : ICaster
    {
        public void Do(IDo o, Doer doer)
        {
            switch (o)
            {
                case A a: doer.Do(a); break;
                case B b: doer.Do(b); break;
                case C c: doer.Do(c); break;
                case D d: doer.Do(d); break;
                case E e: doer.Do(e); break;
                case F f: doer.Do(f); break;
            }
        }
    }
#endif

    public interface IDo { void Do(Doer doer); }

    public class A : IDo { public void Do(Doer doer) { ++doer.Count; } }
    public class B : IDo { public void Do(Doer doer) { ++doer.Count; } }
    public class C : IDo { public void Do(Doer doer) { ++doer.Count; } }
    public class D : IDo { public void Do(Doer doer) { ++doer.Count; } }
    public class E : IDo { public void Do(Doer doer) { ++doer.Count; } }
    public class F : IDo { public void Do(Doer doer) { ++doer.Count; } }

    public class Doer
    {
        // Storing the number of iterations
        // just so the compiler can't optimize
        // away all the hard work we're benchmarking.
        public long Count;

        public void Do(A a) { a.Do(this); }
        public void Do(B b) { b.Do(this); }
        public void Do(C c) { c.Do(this); }
        public void Do(D d) { d.Do(this); }
        public void Do(E e) { e.Do(this); }
        public void Do(F f) { f.Do(this); }
    }

    //public class TryCatchC : ICaster
    //{
    //    public void Do(IDo o, Doer doer)
    //    {
    //        try
    //        {
    //            ((A)o).Do(doer);
    //        }
    //        catch (InvalidCastException)
    //        {
    //            try
    //            {
    //                ((B)o).Do(doer);
    //            }
    //            catch (InvalidCastException)
    //            {
    //                try
    //                {
    //                    ((C)o).Do(doer);
    //                }
    //                catch (InvalidCastException)
    //                {
    //                    try
    //                    {
    //                        ((D)o).Do(doer);
    //                    }
    //                    catch (InvalidCastException)
    //                    {
    //                        try
    //                        {
    //                            ((E)o).Do(doer);
    //                        }
    //                        catch (InvalidCastException)
    //                        {
    //                            ((F)o).Do(doer);
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}
}
```
