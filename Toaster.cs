using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Overwatch
{
    internal class Toaster
    {
        static string errorArgument = "Error";
        public static void ToastNotification()
        {
            var builder = new ToastContentBuilder()
                .AddText("hi")
                .AddButton(new ToastButton()
                    .SetContent("That's a button")
                    .AddArgument(errorArgument));
            builder.Show();
            ToastNotificationManagerCompat.OnActivated += OnActivated;
        }
        private static void OnActivated(ToastNotificationActivatedEventArgsCompat e)
        {
            Console.WriteLine(e.Argument);
            if(e.Argument == errorArgument)
            {
                Console.WriteLine("That's an error");
            }
        }
        public static void DefaultToastNotification(string header, string msg)
        {
            new ToastContentBuilder()
                .AddText(header)
                .AddText(msg)
                .Show();
        }
    }
}
