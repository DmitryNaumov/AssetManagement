namespace AssetManagement.Infrastructure.Messaging
{
	public abstract class Message
	{
	}

	public static class MessageExtensions
	{
		public static void ReplyWith(this Message @receivedMessage, object outgoingMessage)
		{
			var context = ReceiveContext.Current;
			
			context.Bus.Publish(outgoingMessage, x => x.SetCorrelationId(context.CorrelationId));
		}
	}
}