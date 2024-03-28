using BepInEx;
using UnityEngine;
using BoplFixedMath;
using Steamworks;
using UnityEngine.Networking;
using JetBrains.Annotations;
using HarmonyLib;
using System;
using System.Reflection;
using static Mono.Security.X509.X520;
using Steamworks.Data;
using System.Runtime.InteropServices;

namespace Goop
{
    [BepInPlugin("com.goopdevs.goop", "Goop", "0.1")]
    public class Goop : BaseUnityPlugin
    {
        private Rect windowRect = new Rect(60, 50, 500, 500);
        public string GUIText = "Enable Goop";
        public string GUIName = "Goop!";
        private UnityEngine.Color guiColor = UnityEngine.Color.green;

        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Goop is now gooping all over the place!");

            Harmony harmony = new Harmony("com.melon.goop");


        }

        void Update()
        {
        
        }

        public void Start()
        {

        }



        void OnGUI()
        {
            GUI.backgroundColor = guiColor;
            GUI.color = guiColor;
            if (GUI.Button(new Rect(10, 85, 100, 20), GUIText))
            {
                if (goopenabled == false)
                {
                    GUIText = "Disable goop :(";
                    goopenabled = true;
                }
                else
                {
                    GUIText = "Enable Goop!";
                    goopenabled = false;
                }
            }
            if (goopenabled)
            {
                windowRect = GUI.Window(10000, windowRect, GoopGUI, GUIName);
            }
        }

        public bool goopenabled = false;
        public UnityEngine.Color textColor;
        public UnityEngine.Color buttonColor;
        public string Credits = "By Melon, Codemob, and Minidogg";
        public static bool AntiReadyUp;
        public static bool ForceReadyUp;
        public static bool AntiKick;
        public static bool ChooseMap;
        public static bool SendReadyUpReject;

        void GoopGUI(int windowID)
        {
            if(GUI.Button(new Rect(60, 50, 170f, 30f), "No Ready Up: "+boolToOnOffTag(SendReadyUpReject)))
            {
                SendReadyUpReject = !SendReadyUpReject;
            }

        }
    }
    // PUBLIC UNO's
    public static class HarmonyPatches
    {

    }
    // PRIVATE UNO's
    [HarmonyPatch(typeof(SteamManager))]
    public class SendReadyUpReject
    {
        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        private static void Update_NoReadyUp_Plug(SteamManager __instance)
        {
            if (Goop.SendReadyUpReject)
            {
                byte[] uiCommunicationBuffer = new byte[1];
                Debug.Log("sending ready up reject to everyone");
                NetworkTools.EncodeUICom(ref uiCommunicationBuffer, UIPacketType.reject_readyUp);
                for (int i = 0; i < __instance.connectedPlayers.Count; i++)
                {
                    __instance.connectedPlayers[i].Connection.SendMessage(uiCommunicationBuffer, SendType.Reliable);
                }
            }
        }
    }

   
}
