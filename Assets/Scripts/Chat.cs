using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class Chat : MonoBehaviour
{
	//just a random number
	public const short chatMessage = 131;

	[SerializeField]
	private Text container;

	private void Start()
	{
		//if the client is also the server
		if (NetworkServer.active) 
		{
			//registering the server handler
			NetworkServer.RegisterHandler(chatMessage, ServerReceiveMessage);
		}
		
		//registering the client handler
		NetworkManager.singleton.client.RegisterHandler (chatMessage, ReceiveMessage);
	}

	private void ServerReceiveMessage(NetworkMessage message)
	{
		StringMessage myMessage = new StringMessage ();
		//we are using the connectionId as player name only to exemplify
		myMessage.value = message.conn.connectionId + ": " + message.ReadMessage<StringMessage> ().value;

		//sending to all connected clients
		NetworkServer.SendToAll (Chat.chatMessage, myMessage);
	}

	private void ReceiveMessage(NetworkMessage message)
	{
		//reading message
		string messageText = message.ReadMessage<StringMessage> ().value;
		//adding message to chat
		container.text += "\n" + messageText;
	}

	public void SendMessage(InputField input)
	{
		StringMessage myMessage = new StringMessage ();
		//getting the value of the input
		myMessage.value = input.text;

		//sending to server
		NetworkManager.singleton.client.Send (chatMessage, myMessage);
	}
}