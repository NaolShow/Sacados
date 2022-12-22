namespace Sacados {

    /// <summary>
    /// Represents a <see cref="IContainerUI"/> that will handle the display of <see cref="ISlotUI"/>
    /// </summary>
    public interface IContainerUI {

        /// <summary>
        /// Determines if the <see cref="IContainerUI"/> is built or not
        /// </summary>
        bool IsBuilt { get; }

        /// <summary>
        /// Builds the <see cref="IContainerUI"/>
        /// </summary>
        void Build();
        /// <summary>
        /// Unbuilds the <see cref="IContainerUI"/>
        /// </summary>
        void Unbuild();

        /// <summary>
        /// Toggles the build state of the <see cref="IContainer"/>
        /// </summary>
        void Toggle();

    }

}
