using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Dictionary
{
    public static class Dictionary
    {
        public enum CartStatus
        {
            InProgress = 0,
            InOrderingProcess = 1,
            Processed = 2,
        }

        public enum ProductType
        {
            Accessories = 1,
            Phones = 2,
            Television = 3

        }

        public enum Roles
        {
            Administrator = 0,
            Customer = 1
        }
    }
}
