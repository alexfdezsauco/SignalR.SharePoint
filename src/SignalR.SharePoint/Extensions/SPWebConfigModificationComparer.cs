// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SPWebConfigModificationComparer.cs" company="SANDs">
//   Copyright © 2014 SANDs. All rights reserved
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SignalR.SharePoint.Extensions
{
    using System.Collections.Generic;

    using Microsoft.SharePoint.Administration;

    /// <summary>
    ///     The web config modification comparer.
    /// </summary>
    public class SPWebConfigModificationComparer : IEqualityComparer<SPWebConfigModification>
    {
        #region Static Fields

        /// <summary>
        ///     The instance.
        /// </summary>
        private static SPWebConfigModificationComparer instance;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Prevents a default instance of the <see cref="SPWebConfigModificationComparer" /> class from being created.
        /// </summary>
        private SPWebConfigModificationComparer()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the default.
        /// </summary>
        public static SPWebConfigModificationComparer Default
        {
            get
            {
                return instance ?? (instance = new SPWebConfigModificationComparer());
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="x">
        /// The x.
        /// </param>
        /// <param name="y">
        /// The y.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool Equals(SPWebConfigModification x, SPWebConfigModification y)
        {
            return x.Name == y.Name && x.Path == y.Path && x.Owner == y.Owner && x.Type == y.Type && x.Value == y.Value;
        }

        /// <summary>
        /// Gets the hash code.
        /// </summary>
        /// <param name="obj">
        /// The object.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public int GetHashCode(SPWebConfigModification obj)
        {
            if (obj != null)
            {
                return obj.GetHashCode();
            }

            return 0;
        }

        #endregion
    }
}