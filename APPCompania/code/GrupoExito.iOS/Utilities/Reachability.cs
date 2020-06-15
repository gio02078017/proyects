using System;
using System.Net;
using CoreFoundation;
using SystemConfiguration;

namespace GrupoExito.iOS.Utilities
{
    public enum NetworkStatus
    {
        NotReachable,
        ReachableViaCarrierDataNetwork,
        ReachableViaWiFiNetwork
    }

    public static class Reachability
    {
        public const string HostName = "www.google.com";

        public static bool IsReachableWithoutRequiringConnection(NetworkReachabilityFlags flags)
        {
            bool isReachable = (flags & NetworkReachabilityFlags.Reachable) != 0;
            bool noConnectionRequired = (flags & NetworkReachabilityFlags.ConnectionRequired) == 0
                || (flags & NetworkReachabilityFlags.IsWWAN) != 0;

            return isReachable && noConnectionRequired;
        }

        public static bool IsHostReachable(string host)
        {
            if (string.IsNullOrEmpty(host))
            {
                return false;
            }

            using (var r = new NetworkReachability(host))
            {
                if (r.TryGetFlags(out NetworkReachabilityFlags flags))
                {
                    return IsReachableWithoutRequiringConnection(flags);
                }
            }

            return false;
        }

        public static event EventHandler ReachabilityChanged;

        static void OnChange(NetworkReachabilityFlags flags)
        {
            ReachabilityChanged?.Invoke(null, EventArgs.Empty);
        }

        static NetworkReachability adHocWiFiNetworkReachability;

        public static bool IsAdHocWiFiNetworkAvailable(out NetworkReachabilityFlags flags)
        {
            if (adHocWiFiNetworkReachability == null)
            {
                var ipAddress = new IPAddress(new byte[] { 169, 254, 0, 0 });
                adHocWiFiNetworkReachability = new NetworkReachability(ipAddress.MapToIPv6());
                adHocWiFiNetworkReachability.SetNotification(OnChange);
                adHocWiFiNetworkReachability.Schedule(CFRunLoop.Current, CFRunLoop.ModeDefault);
            }

            return adHocWiFiNetworkReachability.TryGetFlags(out flags) && IsReachableWithoutRequiringConnection(flags);
        }

        static NetworkReachability defaultRouteReachability;

        static bool IsNetworkAvailable(out NetworkReachabilityFlags flags)
        {
            if (defaultRouteReachability == null)
            {
                var ipAddress = new IPAddress(0);
                defaultRouteReachability = new NetworkReachability(ipAddress.MapToIPv6());
                defaultRouteReachability.SetNotification(OnChange);
                defaultRouteReachability.Schedule(CFRunLoop.Current, CFRunLoop.ModeDefault);
            }

            return defaultRouteReachability.TryGetFlags(out flags) && IsReachableWithoutRequiringConnection(flags);
        }

        static NetworkReachability remoteHostReachability;

        public static NetworkStatus RemoteHostStatus()
        {
            NetworkReachabilityFlags flags;
            bool reachable;

            if (remoteHostReachability == null)
            {
                remoteHostReachability = new NetworkReachability(HostName);
                reachable = remoteHostReachability.TryGetFlags(out flags);
                remoteHostReachability.SetNotification(OnChange);
                remoteHostReachability.Schedule(CFRunLoop.Current, CFRunLoop.ModeDefault);
            }
            else
            {
                reachable = remoteHostReachability.TryGetFlags(out flags);
            }

            if (!reachable)
            {
                return NetworkStatus.NotReachable;
            }

            if (!IsReachableWithoutRequiringConnection(flags))
            {
                return NetworkStatus.NotReachable;
            }

            return (flags & NetworkReachabilityFlags.IsWWAN) != 0 ?
                NetworkStatus.ReachableViaCarrierDataNetwork : NetworkStatus.ReachableViaWiFiNetwork;
        }

        public static NetworkStatus InternetConnectionStatus()
        {
            bool defaultNetworkAvailable = IsNetworkAvailable(out NetworkReachabilityFlags flags);

            if (defaultNetworkAvailable && ((flags & NetworkReachabilityFlags.IsDirect) != 0))
            {
                return NetworkStatus.NotReachable;
            }

            if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
            {
                return NetworkStatus.ReachableViaCarrierDataNetwork;
            }

            if (flags == 0)
            {
                return NetworkStatus.NotReachable;
            }

            return NetworkStatus.ReachableViaWiFiNetwork;
        }

        public static NetworkStatus LocalWifiConnectionStatus()
        {
            if (IsAdHocWiFiNetworkAvailable(out NetworkReachabilityFlags flags))
            {
                if ((flags & NetworkReachabilityFlags.IsDirect) != 0)
                {
                    return NetworkStatus.ReachableViaWiFiNetwork;
                }
            }

            return NetworkStatus.NotReachable;
        }
    }
}
