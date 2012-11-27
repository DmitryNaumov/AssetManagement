namespace AssetManagement.Infrastructure
{
	public abstract class Message
	{
	}

	public static class MessageExtensions
	{
		public static void ReplyWith(this Message @receivedMessage, object outgoingMessage)
		{
			var context = ReceiveContext.Current;
			
			context.ServiceBus.Publish(outgoingMessage, x => x.SetCorrelationId(context.CorrelationId));
		}
	}
}