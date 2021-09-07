using System;

namespace MuffiNet.Backend.Exceptions
{
    public class ExampleEntityNotFoundException : Exception
    {

        public ExampleEntityNotFoundException(int exampleEntityId) : base($"ExampleEntity with id {exampleEntityId} was not found")
        {
        }
    }
}