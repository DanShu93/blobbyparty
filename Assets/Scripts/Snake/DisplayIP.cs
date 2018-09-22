using UnityEngine;
using UnityEngine.UI;
using System.Net.NetworkInformation;
using System.Net;

public class DisplayIP : MonoBehaviour
{
    Text text;

    void Start()
    {
        //Debug.Log(UnityEngine.Networking.NetworkManager.singleton.networkAddress);
        text = GetComponent<Text>();

        UpdateIP();
    }


    public void UpdateIP()
    {
        int IPsFound = 0;

        // Get a list of all network interfaces (usually one per network card, dialup, and VPN connection)
        NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

        foreach (NetworkInterface network in networkInterfaces)
        {
            if (network.Name.Contains("vEthernet")) { continue; } //code written by me (Ramesh). Should prevent more than one IP Address being used. 


            // Read the IP configuration for each network
            IPInterfaceProperties properties = network.GetIPProperties();

            // Each network interface may have multiple IP addresses
            foreach (IPAddressInformation address in properties.UnicastAddresses)
            {

                // We're only interested in IPv4 addresses for now
                if (address.Address.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork)
                    continue;

                // Ignore loopback addresses (e.g., 127.0.0.1)
                if (IPAddress.IsLoopback(address.Address))
                    continue;

                if (IPsFound == 0)
                {
                    text.text = "Open\n" + "<b>" + address.Address.ToString() + "</b> \nin your phones' browser";
                    Debug.Log("IP address found: " + address.Address.ToString() + " (" + network.Name + ")");
                }
                else
                {
                    Debug.LogError("More than one IP Address was found. Unsure which one should be displayed. IP address: "
                        + address.Address.ToString() + " (" + network.Name + ")");
                }
                IPsFound++;
            }
        }
    }
}

//Checking if If IP starts with 192. might work to exclude all other IPs
//Maybe Setting the IP via this game is possible 