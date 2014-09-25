using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Almirante.Network;
using Tests.NetworkProtocol;
using Almirante.Engine.Core;
using Tests.NetworkClient.Scenes;
using System.Diagnostics;

namespace Tests.NetworkClient.Objects
{
    /// <summary>
    /// Player class
    /// </summary>
    public class Player : NetClient
    {
        /// <summary>
        /// Player instance.
        /// </summary>
        private static Player instance = null;

        /// <summary>
        /// Player instance.
        /// </summary>
        public static Player Instance
        {
            get
            {
                return Player.instance = Player.instance ?? new Player();
            }
            private set
            {
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        private Player()
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="success"></param>
        protected override void OnConnect(bool success)
        {
            // When connected, go to home screen.
            if (success)
            {
                AlmiranteEngine.Scenes.Switch<Home>();
            }
            else
            {
                AlmiranteEngine.Scenes.Switch<Disconnect>();
            }
        }

        /// <summary>
        /// </summary>
        protected override void OnDisconnect()
        {
            // When disconnected, pops all scenes and go to disconnection scene.
            while (true)
            {
                try
                {
                    AlmiranteEngine.Scenes.Pop();
                }
                catch (Exception)
                {
                    break;
                }
            }
            AlmiranteEngine.Scenes.Push<Disconnect>();
        }

        /// <summary>
        /// Error occurred.
        /// </summary>
        /// <param name="ex"></param>
        protected override void OnError(Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }
}
