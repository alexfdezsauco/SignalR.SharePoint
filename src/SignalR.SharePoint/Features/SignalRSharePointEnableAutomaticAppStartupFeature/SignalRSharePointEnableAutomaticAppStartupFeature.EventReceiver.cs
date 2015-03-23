// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignalRSharePointEnableAutomaticAppStartupFeature.EventReceiver.cs" company="SANDs">
//   Copyright © 2014 SANDs. All rights reserved
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SignalR.SharePoint.Features.SignalRSharePointEnableAutomaticAppStartupFeature
{
    using System.Runtime.InteropServices;

    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Administration;

    /// <summary>
    ///     This class handles events raised during feature activation, deactivation, installation, uninstallation, and
    ///     upgrade.
    /// </summary>
    /// <remarks>
    ///     The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>
    [Guid("aa4e5e2e-431a-40c9-8777-aa79c817e9d8")]
    public class SignalRSharePointEnableAutomaticAppStartupFeatureEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.
        #region Public Methods and Operators

        /// <summary>
        /// The feature activated.
        /// </summary>
        /// <param name="properties">
        /// The properties.
        /// </param>
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            var webApplication = properties.Feature.Parent as SPWebApplication;
            if (webApplication != null)
            {
                webApplication.WebConfigModifications.Add(new SPWebConfigModification { Path = "/configuration/appSettings/add[@key='owin:AutomaticAppStartup']", Name = "value", Value = "true", Sequence = 12, Owner = string.Empty, Type = SPWebConfigModification.SPWebConfigModificationType.EnsureAttribute });

                webApplication.Update();
                webApplication.WebService.ApplyWebConfigModifications();
            }
        }

        /// <summary>
        /// The feature deactivating.
        /// </summary>
        /// <param name="properties">
        /// The properties.
        /// </param>
        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            var webApplication = properties.Feature.Parent as SPWebApplication;
            if (webApplication != null)
            {
                webApplication.WebConfigModifications.Add(new SPWebConfigModification { Path = "/configuration/appSettings/add[@key='owin:AutomaticAppStartup']", Name = "value", Value = "false", Sequence = 12, Owner = string.Empty, Type = SPWebConfigModification.SPWebConfigModificationType.EnsureAttribute });

                webApplication.Update();
                webApplication.WebService.ApplyWebConfigModifications();
            }
        }

        #endregion
    }
}