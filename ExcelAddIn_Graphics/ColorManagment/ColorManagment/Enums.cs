namespace ColorManagment
{
    /// <summary>
    /// The name of a color model
    /// </summary>
    public enum ColorModel
    {
        CIEXYZ,
        CIELab,
        CIELuv,
        CIEYxy,
        CIELCHab,
        CIELCHuv,
        LCH99,
        LCH99b,
        LCH99c,
        LCH99d,
        DEF,
        Bef,
        BCH,
        RGB,
        HSV,
        HSL,
        CMYK,
        CMY,
        YCbCr,
        Gray,
        Color2,
        Color3,
        Color4,
        Color5,
        Color6,
        Color7,
        Color8,
        Color9,
        Color10,
        Color11,
        Color12,
        Color13,
        Color14,
        Color15,
    }
    
    /// <summary>
    /// Chromatic adaption calculation method
    /// </summary>
    public enum AdaptionMethod
    {
        XYZScaling,
        VonKries,
        Bradford,
    }

    /// <summary>
    /// Rendering Intent for ICC colors
    /// </summary>
    public enum RenderingIntent
    {
        Perceptual,
        RelativeColorimetric,
        AbsoluteColorimetric,
        Saturation,
    }

    /// <summary>
    /// Method of conversion for ICC colors
    /// </summary>
    public enum ICCconversionMethod
    {
        LUT,
        MatrixTRC,
        MultiprocessElement,
    }

    /// <summary>
    /// Type of conversion for ICC colors
    /// </summary>
    public enum ICCconversionType
    {
        NComponentLUT,
        ThreeComponentMatrix,
        Monochrome,
    }

    /// <summary>
    /// The name of a colorspace
    /// </summary>
    public enum RGBSpaceName
    {
        NTSCRGB,
        BruceRGB,
        CIERGB,
        AdobeRGB,
        AppleRGB,
        ProPhotoRGB,
        sRGB,
        WideGamutRGB,
        BestRGB,
        BetaRGB,
        ColorMatchRGB,
        DonRGB4,
        EktaSpacePS5,
        PAL_SECAMRGB,
        SMPTE_C_RGB,

        ICC,
    }

    /// <summary>
    /// The name of a YCbCr colorspace
    /// </summary>
    public enum YCbCrSpaceName
    {
        //SD TV
        ITU_R_BT601_625,
        ITU_R_BT601_525,
        //HD TV
        ITU_R_BT709_1125,
        ITU_R_BT709_1250,

        ICC,
    }

    /// <summary>
    /// The name of a whitepoint
    /// </summary>
    public enum WhitepointName
    {
        A,
        B,
        C,
        D50,
        D55,
        D65,
        D75,
        E,
        F2,
        F7,
        F11,
        Custom,
    }

    /// <summary>
    /// Method to calculate the CIE 94 color difference
    /// </summary>
    public enum CIE94DifferenceMethod
    {
        GraphicArts,
        Textiles,
    }
    
    /// <summary>
    /// Method to calculate the CMC color difference
    /// </summary>
    public enum CMCDifferenceMethod
    {
        /// <summary>
        /// l:c = 2:1
        /// </summary>
        Acceptability,
        /// <summary>
        /// l:c = 1:1
        /// </summary>
        Perceptibility,
    }
}