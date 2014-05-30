namespace Lucy.Bundle
{
    public enum BundleProcessing {
        /// <summary>
        /// Render each file in bundle as separate link
        /// </summary>
        ManyFiles,
        /// <summary>
        /// Render bundle as single file but not compressed
        /// </summary>
        Combined,

        /// <summary>
        /// Render bundle as single, minified file
        /// </summary>
        CombinedMinified 
    }
    public enum BundleTypes
    {
        /// <summary>
        /// JavaScript
        /// </summary>
        Script,
        /// <summary>
        /// Css style sheet
        /// </summary>
        StyleSheet
    }
}
