namespace SignalR.SharePoint.Extensions
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Microsoft.SharePoint.Administration;

    /// <summary>
    ///     The web config modification collection extensions.
    /// </summary>
    public static class SPWebConfigModificationCollectionExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        /// The add if required.
        /// </summary>
        /// <param name="this">
        /// The this.
        /// </param>
        /// <param name="modification">
        /// The modification.
        /// </param>
        public static void AddIfRequired(this Collection<SPWebConfigModification> @this, SPWebConfigModification modification)
        {
            if (!@this.Contains(modification, SPWebConfigModificationComparer.Default))
            {
                modification.Sequence = (uint)@this.Count;
                @this.Add(modification);
            }
        }

        /// <summary>
        /// The add if required.
        /// </summary>
        /// <param name="this">
        /// The this.
        /// </param>
        /// <param name="modifications">
        /// The modifications.
        /// </param>
        public static void AddIfRequired(this Collection<SPWebConfigModification> @this, IEnumerable<SPWebConfigModification> modifications)
        {

            foreach (SPWebConfigModification modification in modifications)
            {
                @this.AddIfRequired(modification);
            }
        }

        /// <summary>
        /// Removes if owned by the give <paramref name="owner"/>
        /// </summary>
        /// <param name="this">
        /// The this.
        /// </param>
        /// <param name="owner">
        /// The owner.
        /// </param>
        public static void RemoveIfIsOwnedBy(this Collection<SPWebConfigModification> @this, string owner)
        {
            

            for (int i = @this.Count - 1; i >= 0; i--)
            {
                SPWebConfigModification webConfigModification = @this[i];
                if (webConfigModification.Owner == owner)
                {
                    @this.Remove(webConfigModification);
                }
            }
        }

        #endregion
    }
}