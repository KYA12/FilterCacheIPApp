﻿using System.Net;
using System.Net.Sockets;

namespace FilterIPApp.Filters
{
    /* Create IPAddress Extensions class to implement IsInRange method that checks, if client's IP Address is in the IP Whitelist*/
    public static class IPAddressExtensions
    {
        public static bool IsInRange(this IPAddress address, IPAddress start, IPAddress end)
        {

            AddressFamily addressFamily = start.AddressFamily;
            byte[] lowerBytes = start.GetAddressBytes();
            byte[] upperBytes = end.GetAddressBytes();

            if (address.AddressFamily != addressFamily)
            {
                return false;
            }

            byte[] addressBytes = address.GetAddressBytes();

            bool lowerBoundary = true, upperBoundary = true;

            for (int i = 0; i < lowerBytes.Length &&
                (lowerBoundary || upperBoundary); i++)
            {
                if ((lowerBoundary && addressBytes[i] < lowerBytes[i]) ||
                    (upperBoundary && addressBytes[i] > upperBytes[i]))
                {
                    return false;
                }

                lowerBoundary &= (addressBytes[i] == lowerBytes[i]);
                upperBoundary &= (addressBytes[i] == upperBytes[i]);
            }

            return true;
        }
    }
}
