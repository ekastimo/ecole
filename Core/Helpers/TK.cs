using System;
using Core.Exceptions;

namespace Core.Helpers
{
    public class Tk
    {

        public static void AssertValidIds(params Guid[] ids)
        {
            foreach (var guid in ids)
            {
                if (guid == Guid.Empty)
                    throw new ClientFriendlyException($"Invalid record id:{guid}");
            }
        }
    }
}
