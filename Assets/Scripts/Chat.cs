using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class Chat : MonoBehaviour
{
	public const short chatMessage = 131;

	[SerializeField]
	private Text container;

	private void Start()
	{
		if (NetworkServer.active) 
		{
			NetworkServer.RegisterHandler(chatMessage, ServerReceiveMessage);
		}
		
		NetworkManager.singleton.client.RegisterHandler (chatMessage, ReceiveMessage);
	}

	private void ServerReceiveMessage(NetworkMessage message)
	{
		StringMessage myMessage = new StringMessage ();
		myMessage.value = message.conn.connectionId + ": " + message.ReadMessage<StringMessage> ().value;

		NetworkServer.SendToAll (Chat.chatMessage, myMessage);
	}

	private void ReceiveMessage(NetworkMessage message)
	{
		string messageText = message.ReadMessage<StringMessage> ().value;
		container.text += "\n" + messageText;
	}

	public void SendMessage(InputField input)
	{
		StringMessage myMessage = new StringMessage ();
		myMessage.value = input.text;

		NetworkManager.singleton.client.Send (chatMessage, myMessage);
	}
}