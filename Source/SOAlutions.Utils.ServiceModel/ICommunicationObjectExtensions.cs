// © 2013 SOAlutions, Inc. All rights reserved.
// Please direct all inquiries to http://www.soalutions.net

using System;
using System.ServiceModel;

public static class ICommunicationObjectExtensions
{
    public static void SafeClose(this ICommunicationObject channel)
    {
        if (channel != null)
        {
            try
            {
                channel.Close();
            }
            catch (TimeoutException)
            {
                channel.Abort();
            }
            catch (CommunicationException)
            {
                channel.Abort();
            }
        }
    }
}