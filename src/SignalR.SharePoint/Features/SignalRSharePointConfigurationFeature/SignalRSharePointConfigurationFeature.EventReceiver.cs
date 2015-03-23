// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SignalRSharePointConfigurationFeature.EventReceiver.cs" company="SANDs">
//   Copyright © 2014 SANDs. All rights reserved
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace SignalR.SharePoint.Features.SignalRSharePointConfigurationFeature
{
    using System.Runtime.InteropServices;

    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Administration;

    using SignalR.SharePoint.Extensions;

    /// <summary>
    ///     This class handles events raised during feature activation, deactivation, installation, uninstallation, and
    ///     upgrade.
    /// </summary>
    /// <remarks>
    ///     The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>
    [Guid("790d5a5e-9012-49fb-a285-558b75f03ea8")]
    public class SignalRSharePointConfigurationFeatureEventReceiver : SPFeatureReceiver
    {
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
                // string owner = properties.Feature.DefinitionId.ToString();
                
                webApplication.WebConfigModifications.AddIfRequired(new SPWebConfigModification { Path = "/configuration", Name = "system.web", Value = "<system.web/>", Sequence = 1, Owner = string.Empty, Type = SPWebConfigModification.SPWebConfigModificationType.EnsureSection });
                webApplication.WebConfigModifications.AddIfRequired(new SPWebConfigModification { Path = "/configuration/system.web", Name = "trust", Value = "<trust/>", Sequence = 2, Owner = string.Empty, Type = SPWebConfigModification.SPWebConfigModificationType.EnsureSection });
                webApplication.WebConfigModifications.AddIfRequired(new SPWebConfigModification { Path = "/configuration/system.web/trust", Name = "level", Value = "Full", Sequence = 3, Owner = string.Empty, Type = SPWebConfigModification.SPWebConfigModificationType.EnsureAttribute });
                webApplication.WebConfigModifications.AddIfRequired(new SPWebConfigModification { Path = "/configuration/system.web/trust", Name = "legacyCasModel", Value = "false", Sequence = 4, Owner = string.Empty, Type = SPWebConfigModification.SPWebConfigModificationType.EnsureAttribute });

                webApplication.WebConfigModifications.AddIfRequired(new SPWebConfigModification { Path = "/configuration", Name = "runtime", Value = "<runtime/>", Sequence = 5, Owner = string.Empty, Type = SPWebConfigModification.SPWebConfigModificationType.EnsureSection });

                webApplication.WebConfigModifications.AddIfRequired(new SPWebConfigModification { Path = "/configuration/runtime", Name = "child::*[local-name() = 'assemblyBinding' and namespace-uri() = 'urn:schemas-microsoft-com:asm.v1'][1]", Value = "<assemblyBinding />", Sequence = 6, Owner = string.Empty, Type = SPWebConfigModification.SPWebConfigModificationType.EnsureSection });
                
                // TODO: Improvement is required to enable remove
                webApplication.WebConfigModifications.AddIfRequired(new SPWebConfigModification { Path = "/configuration/runtime/child::*[local-name() = 'assemblyBinding' and namespace-uri() = 'urn:schemas-microsoft-com:asm.v1'][1]", Name = "dependentAssembly/assemblyIdentity[@name='Newtonsoft.Json']", Value = "<dependentAssembly><assemblyIdentity name=\"Newtonsoft.Json\" publicKeyToken=\"30ad4fe6b2a6aeed\" culture=\"neutral\" /><bindingRedirect oldVersion=\"0.0.0.0-6.0.0.0\" newVersion=\"6.0.0.0\" /></dependentAssembly>", Sequence = 7, Owner = properties.Feature.DefinitionId.ToString(), Type = SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode });
                webApplication.WebConfigModifications.AddIfRequired(new SPWebConfigModification { Path = "/configuration/runtime/child::*[local-name() = 'assemblyBinding' and namespace-uri() = 'urn:schemas-microsoft-com:asm.v1'][1]", Name = "dependentAssembly/assemblyIdentity[@name='Microsoft.Owin']", Value = "<dependentAssembly><assemblyIdentity name=\"Microsoft.Owin\" publicKeyToken=\"31bf3856ad364e35\" culture=\"neutral\" /><bindingRedirect oldVersion=\"0.0.0.0-2.1.0.0\" newVersion=\"2.1.0.0\" /></dependentAssembly>", Sequence = 8, Owner = properties.Feature.DefinitionId.ToString(), Type = SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode });
                webApplication.WebConfigModifications.AddIfRequired(new SPWebConfigModification { Path = "/configuration/runtime/child::*[local-name() = 'assemblyBinding' and namespace-uri() = 'urn:schemas-microsoft-com:asm.v1'][1]", Name = "dependentAssembly/assemblyIdentity[@name='Microsoft.Owin.Security']", Value = "<dependentAssembly><assemblyIdentity name=\"Microsoft.Owin.Security\" publicKeyToken=\"31bf3856ad364e35\" culture=\"neutral\" /><bindingRedirect oldVersion=\"0.0.0.0-2.1.0.0\" newVersion=\"2.1.0.0\" /></dependentAssembly>", Sequence = 9, Owner = properties.Feature.DefinitionId.ToString(), Type = SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode });

                //// webApplication.WebConfigModifications.AddIfRequired(new SPWebConfigModification { Path = "/configuration/runtime/child::*[local-name() = 'assemblyBinding' and namespace-uri() = 'urn:schemas-microsoft-com:asm.v1'][1]", Name = "child::*[local-name() = 'dependentAssembly' and namespace-uri() = 'urn:schemas-microsoft-com:asm.v1']/child::*[local-name() = 'assemblyIdentity' and namespace-uri() = 'urn:schemas-microsoft-com:asm.v1' and @name = 'Newtonsoft.Json']", Value = "<dependentAssembly><assemblyIdentity name=\"Newtonsoft.Json\" publicKeyToken=\"30ad4fe6b2a6aeed\" culture=\"neutral\" /><bindingRedirect oldVersion=\"0.0.0.0-6.0.0.0\" newVersion=\"6.0.0.0\" /></dependentAssembly>", Sequence = 7, Owner = properties.Feature.DefinitionId.ToString(), Type = SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode });
                //// webApplication.WebConfigModifications.AddIfRequired(new SPWebConfigModification { Path = "/configuration/runtime/child::*[local-name() = 'assemblyBinding' and namespace-uri() = 'urn:schemas-microsoft-com:asm.v1'][1]", Name = "child::*[local-name() = 'dependentAssembly' and namespace-uri() = 'urn:schemas-microsoft-com:asm.v1']/child::*[local-name() = 'assemblyIdentity' and namespace-uri() = 'urn:schemas-microsoft-com:asm.v1' and @name = 'Microsoft.Owin']", Value = "<dependentAssembly><assemblyIdentity name=\"Microsoft.Owin\" publicKeyToken=\"31bf3856ad364e35\" culture=\"neutral\" /><bindingRedirect oldVersion=\"0.0.0.0-2.1.0.0\" newVersion=\"2.1.0.0\" /></dependentAssembly>", Sequence = 8, Owner = properties.Feature.DefinitionId.ToString(), Type = SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode });
                //// webApplication.WebConfigModifications.AddIfRequired(new SPWebConfigModification { Path = "/configuration/runtime/child::*[local-name() = 'assemblyBinding' and namespace-uri() = 'urn:schemas-microsoft-com:asm.v1'][1]", Name = "child::*[local-name() = 'dependentAssembly' and namespace-uri() = 'urn:schemas-microsoft-com:asm.v1']/child::*[local-name() = 'assemblyIdentity' and namespace-uri() = 'urn:schemas-microsoft-com:asm.v1' and @name = 'Microsoft.Owin.Security']", Value = "<dependentAssembly><assemblyIdentity name=\"Microsoft.Owin.Security\" publicKeyToken=\"31bf3856ad364e35\" culture=\"neutral\" /><bindingRedirect oldVersion=\"0.0.0.0-2.1.0.0\" newVersion=\"2.1.0.0\" /></dependentAssembly>", Sequence = 9, Owner = properties.Feature.DefinitionId.ToString(), Type = SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode });
            
                webApplication.WebConfigModifications.AddIfRequired(new SPWebConfigModification { Path = "/configuration", Name = "appSettings", Value = "<appSettings/>", Sequence = 10, Owner = string.Empty, Type = SPWebConfigModification.SPWebConfigModificationType.EnsureSection });
                webApplication.WebConfigModifications.AddIfRequired(new SPWebConfigModification { Path = "/configuration/appSettings", Name = "add[@key='owin:AutomaticAppStartup']", Value = "<add key=\"owin:AutomaticAppStartup\" />", Sequence = 11, Owner = string.Empty, Type = SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode });
                webApplication.WebConfigModifications.AddIfRequired(new SPWebConfigModification { Path = "/configuration/appSettings/add[@key='owin:AutomaticAppStartup']", Name = "value", Value = "false", Sequence = 12, Owner = string.Empty, Type = SPWebConfigModification.SPWebConfigModificationType.EnsureAttribute });
                                                                                                                                               
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
                webApplication.WebConfigModifications.RemoveIfIsOwnedBy(properties.Feature.DefinitionId.ToString());
                webApplication.Update();
                webApplication.WebService.ApplyWebConfigModifications();
            }
        }

        #endregion
    }
}