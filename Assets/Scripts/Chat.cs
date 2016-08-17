using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System.Collections;

public class Chat : MonoBehaviour
{
	//just a random number
	public const short chatMessage = 131;

	[SerializeField]
	private Text container;
	[SerializeField]
	private ScrollRect rect;

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

		//just a hack to jump a frame and scrolldown the chat
		Invoke("ScrollDown", .1f);
	}

	public void SendMessage(InputField input)
	{
		StringMessage myMessage = new StringMessage ();
		//getting the value of the input
		myMessage.value = input.text;

		//sending to server
		NetworkManager.singleton.client.Send (chatMessage, myMessage);
	}

	private void ScrollDown()
	{
		if (rect != null)
			rect.verticalScrollbar.value = 0;
	}
}