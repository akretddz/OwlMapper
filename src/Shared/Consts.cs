using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Shared
{
    public static class Consts
    {
        public static class ApplicationInfo 
        {
            public const string Name            = "OwlMapper";
            public const string ApplicationCode = "owlmapper.api.core";
        }

        public static class Configuration
        {
            public static class Sections
            {
                public const string Module = "module";
            }
            public static class Properties
            {
                public const string IsEnabled = "isEnabled";
            }
        }

        public static readonly string[] ModuleNamesArray =
        [
            "Account",
        ];
    }
}
