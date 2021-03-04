using System;

/// <summary>
/// This is a namespace
/// </summary>
namespace Lib
{
    /// <summary>
    /// This is a class
    /// </summary>
    public class Class1
    {
        /// <summary>
        /// This is a field;
        /// </summary>
        public string _foo;

        private int _myVar;

        /// <summary>
        /// This is a property.
        /// </summary>
        public int MyProperty
        {
            get { return _myVar; }
            set { _myVar = value; }
        }

        /// <summary>
        /// This is a ctor.
        /// </summary>
        /// <param name="foo">This is a param.</param>
        public Class1(string foo)
        {
            _foo = foo;
        }

        /// <summary>
        /// This is a method.
        /// </summary>
        /// <param name="foo">This is a param</param>
        public void Foo(string foo)
        {
            _foo = foo;
        }
    }
}
