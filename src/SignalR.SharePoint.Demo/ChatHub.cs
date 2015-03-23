namespace SignalR.SharePoint.Demo
{
    using Microsoft.AspNet.SignalR;

    /// <summary>
    /// The chat hub.
    /// </summary>
    public class ChatHub : Hub
    {
        #region Public Methods and Operators

        /// <summary>
        /// The send.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        public void Send(string name, string message)
        {
            this.Clients.All.broadcastMessage(name, message);
        }

        #endregion
    }
}