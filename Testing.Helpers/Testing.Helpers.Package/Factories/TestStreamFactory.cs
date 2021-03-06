﻿using System;
using System.Reflection;
using System.Text;
using TNDStudios.Helpers.AzureFunctions.Testing.Helpers;
using TNDStudios.Helpers.AzureFunctions.Testing.Mocks;

namespace TNDStudios.Helpers.AzureFunctions.Testing.Factories
{
    public enum TestStreamType
    {
        Content,
        EmbeddedResource
    }

    public static class TestStreamFactory
    {
        public static TestStream CreateStream(TestStreamType type) => CreateStream(type, Encoding.UTF8, String.Empty);
        public static TestStream CreateStream(TestStreamType type, Encoding encoding) => CreateStream(type, encoding, String.Empty);
        public static TestStream CreateStream(TestStreamType type, Encoding encoding, String loadFrom)
        {
            String value = String.Empty;

            switch (type)
            {
                case TestStreamType.EmbeddedResource:

                    Assembly assembly = Assembly.GetCallingAssembly();
                    try
                    {
                        loadFrom = loadFrom.Replace("*", assembly?.GetName()?.Name); // Wildcarding of the path to avoid issues when renaming projects
                        value = assembly.GetResourceString(loadFrom, encoding);
                    }
                    catch(Exception ex)
                    {
                        value = $"Error collecting test data: '{ex.Message}'";
                    }

                    break;

                default:

                    value = loadFrom;

                    break;
            }

            TestStream result = new TestStream(encoding, value);
            
            return result;
        }
    }
}
