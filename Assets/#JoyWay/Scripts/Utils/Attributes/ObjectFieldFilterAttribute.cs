using System;
using UnityEngine;

/// <summary>
 /// Allow to display an attribute in inspector without allow editing
 /// </summary>
 public class ObjectFieldFilterAttribute : PropertyAttribute {
     public Type T;
 
     public ObjectFieldFilterAttribute(Type t) {
         T = t;
     }
 }