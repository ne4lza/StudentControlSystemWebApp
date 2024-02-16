using EYOkulProjectWebUI.Subscription.Concreate;
using System.Runtime.CompilerServices;

namespace EYOkulProjectWebUI.Subscription.Middleware
{
   static public class DatabaseSubscriptionMiddleware
    {
        public static void UseDatabaseSubsription<T>(this IApplicationBuilder builder, string tableName) where T : class, IService_Transaction
        {
            if (tableName == null)
            {
                throw new ArgumentNullException(nameof(tableName), "TableName cannot be null.");
            }

            var subscription = (T)builder.ApplicationServices.GetService(typeof(T));
            subscription?.configure(tableName);
        }
    }
}
