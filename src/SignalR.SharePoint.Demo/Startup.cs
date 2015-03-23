using Microsoft.Owin;

using SignalR.SharePoint.Demo;

[assembly: OwinStartup(typeof(Startup))]

namespace SignalR.SharePoint.Demo
{
    using Owin;

    /// <summary>
    /// The startup.
    /// </summary>
    public class Startup
    {
        #region Public Methods and Operators

        /// <summary>
        /// The configuration.
        /// </summary>
        /// <param name="app">
        /// The app.
        /// </param>
        public void Configuration(IAppBuilder app)
        {
            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();
        }

        #endregion
    }
}