﻿using System;
using System.Collections.Generic;
using System.Dynamic;

namespace PlayingWithDynamic
{
  internal class DynamicClass : DynamicObject
  {
    private readonly Dictionary<string, object> _dictionary = new Dictionary<string, object>();

    public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
    {
      var argList = new Dictionary<string, object>();
      var i = 0;
      foreach (var obj in args)
      {
        var name = binder.CallInfo.ArgumentNames.Count > i ? binder.CallInfo.ArgumentNames[i] : ("arg" + i);
        argList.Add(name, obj);
        i++;
      }
      PrintArgs(binder.Name, argList);

      result = null;
      return true;
    }

    public override bool TrySetMember(SetMemberBinder binder, object value)
    {
      _dictionary[binder.Name] = value;
      return true;
    }

    public override bool TryGetMember(GetMemberBinder binder, out object result)
    {
      _dictionary.TryGetValue(binder.Name, out result);
      return true;
    }

    public void PrintArgs(string name, Dictionary<string, object> argList)
    {
      Console.WriteLine("Called: {0}", name);
      foreach (var key in argList.Keys)
      {
        Console.WriteLine("\t{0}: {1}", key, argList[key]);
      }
    }
  }

  internal class Program
  {
    private static void Main(string[] args)
    {
      dynamic obj = new DynamicClass();
      obj.MissingMethod(count: 12, value: "This is a string");

      obj.NoProperty = true;
      Console.WriteLine("NoProperty? {0}", obj.NoProperty);
      Console.WriteLine("NotFound? {0}", obj.NotFound);

      Console.ReadKey();
    }
  }
}