using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ImageGallery.Notifications
{
    public class PubnubConsole
    {
        static void Main()
        {
            // Start the HTML5 Pubnub client
            Process.Start("..\\..\\..\\ImageGallery.Client\\PubNub-HTML5-Client.html");

            System.Threading.Thread.Sleep(2000);

            PubnubAPI pubnub = new PubnubAPI(
                "pub-c-19954ee6-4ad9-4aa0-a88e-70c4dc98fcf9",               // PUBLISH_KEY
                "sub-c-9da4b4a2-04e6-11e3-a5e8-02ee2ddab7fe",               // SUBSCRIBE_KEY
                "sec-c-ZDI0NmRjNGQtMTE5YS00YTg4LWFjMTYtODIzYTliMmMxOGQ5",   // SECRET_KEY
                true                                                        // SSL_ON?
            );
            string channel = "samurai-jack-channel";

            // Publish a sample message to Pubnub
            List<object> publishResult = pubnub.Publish(channel, "Hello Pubnub!");
            Console.WriteLine(
                "Publish Success: " + publishResult[0].ToString() + "\n" +
                "Publish Info: " + publishResult[1]
            );

            // Show PubNub server time
            object serverTime = pubnub.Time();
            Console.WriteLine("Server Time: " + serverTime.ToString());

            // Subscribe for receiving messages (in a background task to avoid blocking)
            System.Threading.Tasks.Task t = new System.Threading.Tasks.Task(
                () =>
                pubnub.Subscribe(
                    channel,
                    delegate(object message)
                    {
                        Console.WriteLine("Received Message -> '" + message + "'");
                        return true;
                    }
                )
            );
            t.Start();

            // Read messages from the console and publish them to Pubnub
            while (true)
            {
                Console.Write("Enter a message to be sent to Pubnub: ");
                string msg = Console.ReadLine();
                pubnub.Publish(channel, msg);
                Console.WriteLine("Message {0} sent.", msg);
            }
        }
    }
}